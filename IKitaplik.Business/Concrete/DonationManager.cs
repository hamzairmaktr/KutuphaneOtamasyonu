using Core.Utilities.Results;
using FluentValidation;
using IKitaplik.Business.Abstract;
using IKitaplik.DataAccess.Abstract;
using IKitaplık.Entities.Concrete;
using IKitaplık.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Business.Concrete
{
    public class DonationManager : IDonationService
    {
        IDonationRepository _repository;
        IBookService _bookService;
        IStudentService _studentService;
        IValidator<Book> _validator;
        public DonationManager(IDonationRepository repository, IBookService bookService, IStudentService studentService,IValidator<Book> validator)
        {
            _validator = validator;
            _bookService = bookService;
            _repository = repository;
            _studentService = studentService;
        }
        public IResult Add(Book book, Donation donation)
        {
            int bookId = 0;
            // Kitap barcodunu kontrol et eğer ilgili barcode varsa +1 yap yoksa yeni kitap ekle
            var bookControl = _bookService.GetByBarcode(book.Barcode);
            if (bookControl.Success)
            {
                bookControl.Data.Piece += 1;
                bookId = bookControl.Data.Id;
                var updatedBook = _bookService.Update(bookControl.Data);
                if (!updatedBook.Success)
                {
                    return new ErrorResult(updatedBook.Message);
                }
            }
            else
            {
                var addedBook = _bookService.Add(book);
                bookId = addedBook.Data.Id;
                if (!addedBook.Success)
                {
                    return new ErrorResult(addedBook.Message);
                }
            }

            // Öğrenciyi bul eğer öğrenci var ise kitap sağlam ise 20 hasarlı ise 10 puan ekle
            var student = _studentService.GetById(donation.StudentId);
            if (student.Success)
            {
                if (donation.IsItDamaged == true)
                {
                    student.Data.Point += 10;
                }
                else
                {
                    student.Data.Point += 20;
                }
                _studentService.Update(student.Data);
            }
            else
            {
                return new ErrorResult("Öğrenci bulunamadı");
            }

            donation.BookId = bookId; // Kitap ID'sini donation'a ekle
            _repository.Add(donation);
            return new SuccessResult("Bağış başarıyla eklendi.");
        }

        public IDataResult<List<DonationGetDTO>> GetAllDTO()
        {
            var res = _repository.GetAllDTO();
            if (res.Count <= 0)
            {
                return new ErrorDataResult<List<DonationGetDTO>>("Kayıt bulunamadı");
            }
            return new SuccessDataResult<List<DonationGetDTO>>(res,"Kayıtlar çekildi");
        }

        public IDataResult<DonationGetDTO> GetByIdDTO(int id)
        {
            var res = _repository.GetDTO(p=>p.Id == id);
            if (res is null)
            {
                return new ErrorDataResult<DonationGetDTO>("Kayıt bulunamadı");
            }
            return new SuccessDataResult<DonationGetDTO>(res, "İlgili kayıt çekildi");
        }
    }
}
