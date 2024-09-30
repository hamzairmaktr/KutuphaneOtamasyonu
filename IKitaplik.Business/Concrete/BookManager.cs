using Core.Utilities.Results;
using DataAccess.UnitOfWork;
using FluentValidation;
using IKitaplik.Business.Abstract;
using IKitaplik.DataAccess.Abstract;
using IKitaplik.DataAccess.Concrete.EntityFramework;
using IKitaplık.Entities.Concrete;
using IKitaplık.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IKitaplik.Business.Concrete
{
    public class BookManager : IBookService
    {
        IBookRepository _bookRepository;
        IValidator<Book> _validator;
        IMovementService _movementService;
        IUnitOfWork _unitOfWork;

        public BookManager(IBookRepository bookRepository, IValidator<Book> validator, IMovementService movementService, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
            _bookRepository = bookRepository;
            _movementService = movementService;
        }

        public IDataResult<Book> Add(Book book)
        {
            try
            {
                var validationResult = _validator.Validate(book);
                if (!validationResult.IsValid)
                {
                    return new ErrorDataResult<Book>(message: validationResult.Errors.Select(e => e.ErrorMessage).First());
                }
                _unitOfWork.BeginTransaction();
                _bookRepository.Add(book);

                var response = _movementService.Add(new Movement
                {
                    BookId = book.Id,
                    MovementDate = DateTime.Now,
                    Title = "Kitap Eklendi",
                    Note = $"{DateTime.Now:g} tarihinde {book.Name} adlı kitap kayıt edildi"
                });

                if (!response.Success)
                {
                    _unitOfWork.Rollback();
                    return new ErrorDataResult<Book>(message: "Kitap hareketlerine veri eklerken hata oluştu");
                }

                _unitOfWork.Commit();
                return new SuccessDataResult<Book>(book, message: "Kitap başarı ile oluşturuldu");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new ErrorDataResult<Book>(message: "Kitap oluşturulurken hata oluştu : " + ex.Message);
            }
        }

        public IResult Delete(Book book)
        {
            try
            {
                _bookRepository.Delete(book);
                return new SuccessResult("Kitap başarı ile oluşturuldu");
            }
            catch (Exception ex)
            {
                return new ErrorResult("Kitap silinirken hata oluştu : " + ex.Message);
            }
        }

        public IDataResult<List<BookGetDTO>> GetAll()
        {
            try
            {
                List<BookGetDTO> books = _bookRepository.GetAllBookDTOs();
                return new SuccessDataResult<List<BookGetDTO>>(books, "Kitaplar başarı ile çekildi");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<BookGetDTO>>("Kitaplar çekilirken bir hata oluştu : " + ex.Message);
            }
        }

        public IDataResult<Book> GetById(int id)
        {
            try
            {
                Book book = _bookRepository.Get(p => p.Id == id);
                if (book != null)
                {
                    return new SuccessDataResult<Book>(book, "Kitap başarı ile çekildi");
                }
                return new ErrorDataResult<Book>("İlgili kitap bulunamadı");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<Book>("İlgili kitap çekilirken hata oluştu : " + ex.Message);
            }
        }

        public IDataResult<List<BookGetDTO>> GetAllByName(string name)
        {
            try
            {
                List<BookGetDTO> books = _bookRepository.GetAllBookFilteredDTOs(p => p.Name == name);
                return new SuccessDataResult<List<BookGetDTO>>(books, "Kitaplar başarı ile çekildi");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<BookGetDTO>>("İlgili kitaplar çekilirken hata oluştu : " + ex.Message);
            }
        }

        public IResult Update(Book book)
        {
            try
            {
                var validationResult = _validator.Validate(book);
                if (!validationResult.IsValid)
                {
                    return new ErrorResult(validationResult.Errors.Select(e => e.ErrorMessage).First());
                }

                _bookRepository.Update(book);
                return new SuccessResult("Kitap başarı ile güncellendi");
            }
            catch (Exception ex)
            {
                return new ErrorResult("Kitap güncellenirken hata oluştu : " + ex.Message);
            }
        }

        public IDataResult<Book> GetByBarcode(string barcode)
        {
            try
            {
                Book book = _bookRepository.Get(p => p.Barcode == barcode);
                if (book != null)
                {
                    return new SuccessDataResult<Book>(book, "Kitap başarı ile çekildi");
                }
                return new ErrorDataResult<Book>("İlgili kitap bulunamadı");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<Book>("İlgili kitap çekilirken hata oluştu : " + ex.Message);
            }
        }
    }
}
