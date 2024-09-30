using Core.Utilities.Results;
using IKitaplık.Entities.Concrete;
using IKitaplık.Entities.DTOs;
using IKitaplik.Business.Abstract;
using IKitaplik.DataAccess.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Business.Concrete
{
    public class DepositManager : IDepositService
    {
        private readonly IDepositRepository _depositRepository;
        private readonly IBookRepository _bookRepository;
        private readonly IStudentRepository _studentRepository;

        public DepositManager(IDepositRepository depositRepository, IBookRepository bookRepository, IStudentRepository studentRepository)
        {
            _depositRepository = depositRepository;
            _bookRepository = bookRepository;
            _studentRepository = studentRepository;
        }

        public IResult DepositGiven(Deposit deposit, int bookId, int studentId)
        {
            try
            {
                var book = _bookRepository.Get(b => b.Id == bookId);
                var student = _studentRepository.Get(s => s.Id == studentId);

                if (book == null || student == null)
                {
                    return new ErrorResult("Kitap veya öğrenci bulunamadı.");
                }

                if (book.Piece <= 0)
                {
                    return new ErrorResult("Kitabın mevcut sayısı yetersiz.");
                }

                if(student.Situation == true)
                {
                    return new ErrorResult("Bu öğrencide zaten bir kitap emanet verilmiş.");
                }

                // Kitap adedini düşür
                book.Piece -= 1;
                _bookRepository.Update(book);

                // Öğrenciyi kitap okuyor olarak işaretle
                student.Situation = true;
                _studentRepository.Update(student);

                // Emanet kaydını oluştur
                deposit.BookId = bookId;
                deposit.StudentId = studentId;

                _depositRepository.Add(deposit);
                return new SuccessResult("Kitap başarıyla emanet verildi.");
            }
            catch (Exception ex)
            {
                return new ErrorResult("Kitap emanet verilirken hata oluştu: " + ex.Message);
            }
        }

        public IResult DepositReceived(Deposit deposit)
        {
            try
            {
                var existingDeposit = _depositRepository.Get(d => d.Id == deposit.Id);

                if (existingDeposit == null)
                {
                    return new ErrorResult("Emanet kaydı bulunamadı.");
                }

                var book = _bookRepository.Get(b => b.Id == existingDeposit.BookId);
                var student = _studentRepository.Get(s => s.Id == existingDeposit.StudentId);

                if (book == null || student == null)
                {
                    return new ErrorResult("Kitap veya öğrenci bulunamadı.");
                }

                // Kitap adedini artır
                book.Piece += 1;
                _bookRepository.Update(book);

                // Öğrenciyi kitap okumuyor olarak işaretle
                student.Situation = false;
                student.NumberofBooksRead += 1; // Okunan kitap sayısını artır

                // Öğrenci kitabı hasarlı getirdi
                if (deposit.IsItDamaged)
                {
                    student.Point -= 10;
                }

                // Öğrenci kitabı geç getirdi
                if (deposit.AmILate)
                {
                    student.Point -= 5;
                }

                // Öğrenci kitapı getirdi
                student.Point += 10;

                _studentRepository.Update(student);

                // Emanet kaydını güncelle
                existingDeposit.DeliveryDate = DateTime.Now;
                _depositRepository.Update(existingDeposit);

                return new SuccessResult("Kitap başarıyla iade alındı.");
            }
            catch (Exception ex)
            {
                return new ErrorResult("Kitap iade alınırken hata oluştu: " + ex.Message);
            }
        }

        public IResult Delete(Deposit deposit)
        {
            try
            {
                _depositRepository.Delete(deposit);
                return new SuccessResult("Emanet kaydı başarıyla silindi.");
            }
            catch (Exception ex)
            {
                return new ErrorResult("Emanet kaydı silinirken hata oluştu: " + ex.Message);
            }
        }

        public IDataResult<List<DepositGetDTO>> GetAllDTO()
        {
            try
            {
                var deposits = _depositRepository.GetAllDepositDTOs();
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
                var deposit = _depositRepository.GetDepositFilteredDTOs(p=>p.Id == id);
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
                var deposit = _depositRepository.Get(d => d.Id == id);
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

        public IResult ExtendDueDate(int depositId, int additionalDays, DateTime date, bool asDate)
        {
            try
            {
                var data = GetById(depositId);
                if (data.Success)
                {
                    if (asDate)
                    {
                        // Belli bir tarihe kadar süre ver
                        data.Data.DeliveryDate = date;
                    }
                    else
                    {
                        // Gün belirle o gün kadar süre ver
                        data.Data.DeliveryDate.AddDays(additionalDays);
                    }
                    _depositRepository.Update(data.Data);
                    return new SuccessResult("Emanete ek süre verildi");
                }
                else
                {
                    return new ErrorResult(data.Message);
                }
            }
            catch (Exception ex)
            {
                return new ErrorResult("Emanetin süresi uzatılırken hata oluştu : " + ex.Message);
            }
        }
    }
}
