using Core.Utilities.Results;
using FluentValidation;
using IKitaplik.Business.Abstract;
using IKitaplik.DataAccess.Abstract;
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

        public BookManager(IBookRepository bookRepository, IValidator<Book> validator)
        {
            _validator = validator;
            _bookRepository = bookRepository;
        }

        public IResult Add(Book book)
        {
            try
            {
                var validationResult = _validator.Validate(book);
                if (!validationResult.IsValid)
                {
                    return new ErrorResult(validationResult.Errors.Select(e => e.ErrorMessage).First());
                }
                _bookRepository.Add(book);
                return new SuccessResult("Kitap başarı ile oluşturuldu");
            }
            catch (Exception ex)
            {
                return new ErrorResult("Kitap oluşturulurken hata oluştu : " + ex.Message);
            }
        }

        public IResult Delete(Book book)
        {
            try
            {
                IDataResult<Book> dataResult = GetById(book.Id);
                if (dataResult.Success)
                {
                    _bookRepository.Delete(dataResult.Data);
                    return new SuccessResult("Kitap başarı ile oluşturuldu");
                }
                else
                {
                    return new ErrorResult(dataResult.Message);
                }
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
                IDataResult<Book> dataResult = GetById(book.Id);
                if (dataResult.Success)
                {
                    _bookRepository.Update(book);
                    return new SuccessResult("Kitap başarı ile güncellendi");
                }
                else
                {
                    return new ErrorResult(dataResult.Message);
                }
            }
            catch (Exception ex)
            {
                return new ErrorResult("Kitap güncellenirken hata oluştu : " + ex.Message);
            }
        }
    }
}
