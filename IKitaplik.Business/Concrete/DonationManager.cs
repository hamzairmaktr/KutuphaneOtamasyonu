﻿using Core.Utilities.Results;
using IKitaplik.Business.Abstract;
using IKitaplik.Entities.Concrete;
using IKitaplik.Entities.DTOs;
using IKitaplik.DataAccess.UnitOfWork;

namespace IKitaplik.Business.Concrete
{
    public class DonationManager : IDonationService
    {
        private readonly IBookService _bookService;
        private readonly IStudentService _studentService;
        private readonly IUnitOfWork _unitOfWork;
        public DonationManager(IBookService bookService, IStudentService studentService, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _bookService = bookService;
            _studentService = studentService;
        }
        public IResult Add(Book book, Donation donation)
        {
            try
            {
                _unitOfWork.BeginTransaction();
                int bookId;
                // Kitap barcodunu kontrol et eğer ilgili barcode varsa +1 yap yoksa yeni kitap ekle
                var bookControl = _bookService.GetByBarcode(book.Barcode);
                if (bookControl.Success)
                {
                    bookControl.Data.Piece += 1;
                    bookId = bookControl.Data.Id;
                    var updatedBook = _bookService.Update(bookControl.Data);
                    if (!updatedBook.Success)
                    {
                        _unitOfWork.Rollback();
                        return new ErrorResult(updatedBook.Message);
                    }
                }
                else
                {
                    var addedBook = _bookService.Add(book);
                    bookId = addedBook.Data.Id;
                    if (!addedBook.Success)
                    {
                        _unitOfWork.Rollback();
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
                    _unitOfWork.Rollback();
                    return new ErrorResult("Öğrenci bulunamadı");
                }

                donation.BookId = bookId; // Kitap ID'sini donation'a ekle
                _unitOfWork.Donations.Add(donation);
                _unitOfWork.Commit();
                return new SuccessResult("Bağış başarıyla eklendi.");
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                return new ErrorResult("Bağış eklenirken hata oluştu : " + e.Message);
            }
        }

        public IDataResult<List<DonationGetDTO>> GetAllDTO()
        {
            var res = _unitOfWork.Donations.GetAllDTO();
            if (res.Count <= 0)
            {
                return new ErrorDataResult<List<DonationGetDTO>>("Kayıt bulunamadı");
            }
            return new SuccessDataResult<List<DonationGetDTO>>(res, "Kayıtlar çekildi");
        }

        public IDataResult<DonationGetDTO> GetByIdDTO(int id)
        {
            var res = _unitOfWork.Donations.GetDTO(p => p.Id == id);
            if (res is null)
            {
                return new ErrorDataResult<DonationGetDTO>("Kayıt bulunamadı");
            }
            return new SuccessDataResult<DonationGetDTO>(res, "İlgili kayıt çekildi");
        }
    }
}
