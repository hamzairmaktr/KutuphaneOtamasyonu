using Core.Utilities.Results;
using IKitaplik.Entities.Concrete;
using IKitaplik.Business.Abstract;
using IKitaplik.DataAccess.UnitOfWork;
using IKitaplik.Entities.DTOs.BookDTOs;
using AutoMapper;
using IKitaplik.Entities.DTOs.DepositDTOs;

namespace IKitaplik.Business.Concrete
{
    public class DepositManager : IDepositService
    {
        private readonly IBookService _bookService;
        private readonly IStudentService _studentService;
        private readonly IMovementService _movementService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public DepositManager(IBookService bookService,
                              IStudentService studentService,
                              IMovementService movementService,
                              IUnitOfWork unitOfWork,
                              IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _movementService = movementService;
            _bookService = bookService;
            _studentService = studentService;
            _mapper = mapper;
        }

        public IResult DepositGiven(DepositAddDto depositAddDto)
        {
            return HandleWithTransactionHelper.Handling(() =>
            {
                var deposit = _mapper.Map<Deposit>(depositAddDto);
                deposit.CreatedDate = DateTime.Now;
                var book = ValidateBook(deposit.BookId);
                var student = ValidateStudent(deposit.StudentId);

                if (book.Data.Piece <= 0) return new ErrorResult("Kitabın mevcut sayısı yetersiz.");
                if (student.Data.Situation) return new ErrorResult("Bu öğrencide zaten bir kitap emanet verilmiş.");

                _bookService.BookAddedPiece(new BookAddPieceDto { Id = deposit.BookId, BeAdded = -1 }, true);

                UpdateStudentStatus(student, true);

                _unitOfWork.Deposits.Add(deposit);

                AddMovement("Emanet Verildi", $"{student.Data.Name} adlı öğrenciye {book.Data.Name} adlı kitap emanet verildi", deposit.BookId, deposit.StudentId, deposit.Id);

                return new SuccessResult("Kitap başarıyla emanet verildi.");
            }, _unitOfWork);
        }
        public IResult DepositReceived(DepositUpdateDto depositUpdateDto)
        {
            return HandleWithTransactionHelper.Handling(() =>
            {
                var deposit = _mapper.Map<Deposit>(depositUpdateDto);
                var existingDeposit = _unitOfWork.Deposits.Get(d => d.Id == deposit.Id);
                if (existingDeposit == null) return new ErrorResult("Emanet kaydı bulunamadı.");

                var book = ValidateBook(existingDeposit.BookId);
                var student = ValidateStudent(existingDeposit.StudentId);

                _bookService.BookAddedPiece(new BookAddPieceDto { Id = existingDeposit.BookId, BeAdded = 1 },true);
                UpdateStudentOnReturn(student, deposit);

                existingDeposit.IsDelivered = true;
                _unitOfWork.Deposits.Update(existingDeposit);

                AddMovement("Emanet Teslim Alındı", $"{student.Data.Name} adlı öğrenci {book.Data.Name} adlı kitapı {GetDepositStatus(deposit)} teslim etti", book.Data.Id, student.Data.Id, deposit.Id);

                return new SuccessResult("Kitap başarıyla iade alındı.");
            }, _unitOfWork);
        }
        public IResult Delete(int id)
        {
            return HandleWithTransactionHelper.Handling(() =>
            {
                var deposit = GetById(id);
                if(!deposit.Success)
                    return new ErrorResult(deposit.Message);
                _unitOfWork.Deposits.Delete(deposit.Data);
                AddMovement("Emanet Kaydı Silindi", "Emanet kaydı silindi.", deposit.Data.BookId, deposit.Data.StudentId, deposit.Data.Id);
                return new SuccessResult("Emanet kaydı başarıyla silindi.");
            }, _unitOfWork);
        }
        public IResult ExtendDueDate(DepositExtentDueDateDto depositExtentDueDateDto)
        {
            return HandleWithTransactionHelper.Handling(() =>
            {
                var deposit = GetById(depositExtentDueDateDto.DepositId).Data;
                if (depositExtentDueDateDto.AsDate)
                    deposit.DeliveryDate = depositExtentDueDateDto.Date.GetValueOrDefault();
                else
                    deposit.DeliveryDate = deposit.DeliveryDate.AddDays(depositExtentDueDateDto.AdditionalDays.GetValueOrDefault());

                _unitOfWork.Deposits.Update(deposit);
                return new SuccessResult("Emanete ek süre verildi.");
            }, _unitOfWork);
        }
        public IDataResult<List<DepositGetDTO>> GetAllDTO()
        {
            try
            {
                var deposits = _unitOfWork.Deposits.GetAllDepositDTOs();
                return new SuccessDataResult<List<DepositGetDTO>>(deposits, "Emanet kayıtları başarıyla çekildi.");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<DepositGetDTO>>("Emanet kayıtları çekilirken hata oluştu: " + ex.Message);
            }
        }
        public IDataResult<DepositGetDTO> GetByIdDTO(int id)
        {
            try
            {
                var deposit = _unitOfWork.Deposits.GetDepositFilteredDTOs(p => p.Id == id);
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
        public IDataResult<Deposit> GetById(int id)
        {
            try
            {
                var deposit = _unitOfWork.Deposits.Get(d => d.Id == id);
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

        private void AddMovement(string title, string note, int bookId, int studentId, int depositId)
        {
            _movementService.Add(new Movement
            {
                BookId = bookId,
                StudentId = studentId,
                DepositId = depositId,
                MovementDate = DateTime.Now,
                Title = title,
                Note = $"{DateTime.Now:g} - {note}"
            });
        }
        private void UpdateStudentStatus(IDataResult<Student> student, bool isBorrowing)
        {
            student.Data.Situation = isBorrowing;
            var studentDto = _mapper.Map<StudentUpdateDto>(student.Data);
            _studentService.Update(studentDto, true);
        }
        private void UpdateStudentOnReturn(IDataResult<Student> student, Deposit deposit)
        {
            student.Data.Situation = false;
            student.Data.NumberofBooksRead += 1;

            if (deposit.IsItDamaged) student.Data.Point -= 10;
            if (deposit.AmILate) student.Data.Point -= 5;
            student.Data.Point += 10;
            var studentDto = _mapper.Map<StudentUpdateDto>(student.Data);
            _studentService.Update(studentDto,true);
        }
        private IDataResult<Book> ValidateBook(int bookId)
        {
            var book = _bookService.GetById(bookId);
            if (!book.Success) throw new Exception(book.Message);
            return book;
        }
        private IDataResult<Student> ValidateStudent(int studentId)
        {
            var student = _studentService.GetById(studentId);
            if (!student.Success) throw new Exception(student.Message);
            return student;
        }
        private string GetDepositStatus(Deposit deposit)
        {
            if (deposit.IsItDamaged && deposit.AmILate) return "hasarlı ve geç";
            if (deposit.IsItDamaged) return "hasarlı";
            if (deposit.AmILate) return "geç";
            return "sağlam";
        }
    }
}
