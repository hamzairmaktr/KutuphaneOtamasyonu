using Core.Utilities.Results;
using IKitaplik.Entities.Concrete;
using IKitaplik.Business.Abstract;
using IKitaplik.DataAccess.UnitOfWork;
using IKitaplik.Entities.DTOs.BookDTOs;
using AutoMapper;
using IKitaplik.Entities.DTOs.DepositDTOs;
using System.Threading.Tasks;

namespace IKitaplik.Business.Concrete
{
    public class DepositManager : IDepositService
    {
        private readonly IBookService _bookService;
        private readonly IStudentService _studentService;
        private readonly IMovementService _movementService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;
        public DepositManager(IBookService bookService,
                              IStudentService studentService,
                              IMovementService movementService,
                              IUnitOfWork unitOfWork,
                              IMapper mapper,
                              IImageService imageService)
        {
            _unitOfWork = unitOfWork;
            _movementService = movementService;
            _bookService = bookService;
            _studentService = studentService;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task<IResult> DepositGivenAsync(DepositAddDto depositAddDto)
        {
            return await HandleWithTransactionHelper.Handling(async () =>
            {
                var deposit = _mapper.Map<Deposit>(depositAddDto);
                deposit.CreatedDate = DateTime.Now;
                deposit.DeliveryDate = depositAddDto.IssueDate.AddDays(14);
                var book = await ValidateBookAsync(deposit.BookId);
                var student = await ValidateStudentAsync(deposit.StudentId);

                if (book.Data.Piece <= 0) return new ErrorResult("Kitabın mevcut sayısı yetersiz.");
                if (!student.Data.Situation) return new ErrorResult("Bu öğrencide zaten bir kitap emanet verilmiş.");

                await _bookService.BookAddedPieceAsync(new BookAddPieceDto { Id = deposit.BookId, BeAdded = -1 }, true);

                student.Data.Situation = false;
                var studentDto = _mapper.Map<StudentUpdateDto>(student.Data);
                await _studentService.UpdateAsync(studentDto, true);

                await _unitOfWork.Deposits.AddAsync(deposit);

                await AddMovementAsync("Emanet Verildi", $"{student.Data.Name} adlı öğrenciye {book.Data.Name} adlı kitap emanet verildi", deposit.BookId, deposit.StudentId, deposit.Id);

                return new SuccessResult("Kitap başarıyla emanet verildi.");
            }, _unitOfWork);
        }
        public async Task<IResult> DepositReceivedAsync(DepositUpdateDto depositUpdateDto)
        {
            return await HandleWithTransactionHelper.Handling(async () =>
            {
                var existingDeposit = await _unitOfWork.Deposits.GetAsync(d => d.Id == depositUpdateDto.Id);
                if (existingDeposit == null) return new ErrorResult("Emanet kaydı bulunamadı.");

                var book = await ValidateBookAsync(existingDeposit.BookId);
                var student = await ValidateStudentAsync(existingDeposit.StudentId);

                await _bookService.BookAddedPieceAsync(new BookAddPieceDto { Id = existingDeposit.BookId, BeAdded = 1 }, true);
                await UpdateStudentOnReturnAsync(student, depositUpdateDto);
                existingDeposit.DeliveryDate = depositUpdateDto.DeliveryDate;
                existingDeposit.IsItDamaged = depositUpdateDto.IsItDamaged;
                existingDeposit.AmILate = depositUpdateDto.AmILate;
                existingDeposit.UpdatedDate = DateTime.Now;
                existingDeposit.Note = depositUpdateDto.Note;
                existingDeposit.IsDelivered = true;
                await _unitOfWork.Deposits.UpdateAsync(existingDeposit);

                await AddMovementAsync("Emanet Teslim Alındı", $"{student.Data.Name} adlı öğrenci {book.Data.Name} adlı kitapı {GetDepositStatus(depositUpdateDto)} teslim etti", book.Data.Id, student.Data.Id, depositUpdateDto.Id);

                return new SuccessResult("Kitap başarıyla iade alındı.");
            }, _unitOfWork);
        }
        public async Task<IResult> DeleteAsync(int id)
        {
            return await HandleWithTransactionHelper.Handling(async () =>
            {
                var deposit = await GetByIdAsync(id);
                if (!deposit.Success)
                    return new ErrorResult(deposit.Message);
                await _unitOfWork.Deposits.DeleteAsync(deposit.Data);
                await _imageService.DeleteAllAsync(Entities.Enums.ImageType.Deposit, id);
                await AddMovementAsync("Emanet Kaydı Silindi", "Emanet kaydı silindi.", deposit.Data.BookId, deposit.Data.StudentId, deposit.Data.Id);
                return new SuccessResult("Emanet kaydı başarıyla silindi.");
            }, _unitOfWork);
        }
        public async Task<IResult> ExtendDueDateAsync(DepositExtentDueDateDto depositExtentDueDateDto)
        {
            return await HandleWithTransactionHelper.Handling(async () =>
            {
                var depositR = await GetByIdAsync(depositExtentDueDateDto.DepositId);
                var deposit = depositR.Data;
                if (depositExtentDueDateDto.AsDate)
                    deposit.DeliveryDate = depositExtentDueDateDto.Date.GetValueOrDefault();
                else
                    deposit.DeliveryDate = deposit.DeliveryDate.AddDays(depositExtentDueDateDto.AdditionalDays.GetValueOrDefault());

                var book = await _bookService.GetByIdAsync(deposit.BookId);
                var student = await _studentService.GetByIdAsync(deposit.StudentId);

                await AddMovementAsync("Emanete Ekstra Süre Verildi", $"{student.Data.Name} adlı öğrenci aldığı {book.Data.Name} adlı kitabı zamanında teslim edemediğinden ilgili teslim tarihi ertelendi. Yeni Tarih : {deposit.DeliveryDate.ToString("dd.MM.yyyy")}", deposit.BookId, deposit.StudentId, deposit.Id);

                await _unitOfWork.Deposits.UpdateAsync(deposit);
                return new SuccessResult("Emanete ek süre verildi.");
            }, _unitOfWork);
        }
        public async Task<IDataResult<PagedResult<DepositGetDTO>>> GetAllDTOAsync(int page, int pageSize, bool includeDelivered)
        {
            try
            {
                var deposits = await _unitOfWork.Deposits.GetAllDepositDTOsAsync(page, pageSize,filter: p => includeDelivered || !p.IsDelivered);
                return new SuccessDataResult<PagedResult<DepositGetDTO>>(deposits, "Emanet kayıtları başarıyla çekildi.");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<PagedResult<DepositGetDTO>>("Emanet kayıtları çekilirken hata oluştu: " + ex.Message);
            }
        }
        public async Task<IDataResult<DepositGetDTO>> GetByIdDTOAsync(int id)
        {
            try
            {
                var deposit = await _unitOfWork.Deposits.GetDepositFilteredDTOsAsync(p => p.Id == id);
                if (deposit != null)
                {
                    return new SuccessDataResult<DepositGetDTO>(deposit, "Emanet kaydı başarıyla çekildi.");
                }
                return new ErrorDataResult<DepositGetDTO>("Emanet kaydı bulunamadı.");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<DepositGetDTO>("Emanet kaydı çekilirken hata oluştu: " + ex.Message);
            }
        }
        public async Task<IDataResult<Deposit>> GetByIdAsync(int id)
        {
            try
            {
                var deposit = await _unitOfWork.Deposits.GetAsync(d => d.Id == id);
                if (deposit != null)
                {
                    return new SuccessDataResult<Deposit>(deposit, "Emanet kaydı başarıyla çekildi.");
                }
                return new ErrorDataResult<Deposit>("Emanet kaydı bulunamadı.");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<Deposit>("Emanet kaydı çekilirken hata oluştu: " + ex.Message);
            }
        }

        private async Task AddMovementAsync(string title, string note, int bookId, int studentId, int depositId)
        {
            await _movementService.AddAsync(new Movement
            {
                BookId = bookId,
                StudentId = studentId,
                DepositId = depositId,
                MovementDate = DateTime.Now,
                Title = title,
                Note = $"{DateTime.Now:g} - {note}"
            });
        }
        private async Task UpdateStudentOnReturnAsync(IDataResult<Student> student, DepositUpdateDto deposit)
        {
            student.Data.Situation = true;
            student.Data.NumberofBooksRead += 1;

            if (deposit.IsItDamaged) student.Data.Point -= 10;
            if (deposit.AmILate) student.Data.Point -= 5;
            student.Data.Point += 10;
            var studentDto = _mapper.Map<StudentUpdateDto>(student.Data);
            await _studentService.UpdateAsync(studentDto, true);
        }
        private async Task<IDataResult<Book>> ValidateBookAsync(int bookId)
        {
            var book = await _bookService.GetByIdAsync(bookId);
            if (!book.Success) throw new Exception(book.Message);
            return book;
        }
        private async Task<IDataResult<Student>> ValidateStudentAsync(int studentId)
        {
            var student = await _studentService.GetByIdAsync(studentId);
            if (!student.Success) throw new Exception(student.Message);
            return student;
        }
        private string GetDepositStatus(DepositUpdateDto deposit)
        {
            if (deposit.IsItDamaged && deposit.AmILate) return "hasarlı ve geç";
            if (deposit.IsItDamaged) return "hasarlı";
            if (deposit.AmILate) return "geç";
            return "sağlam";
        }
    }
}
