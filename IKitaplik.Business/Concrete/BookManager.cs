using Core.Utilities.Results;
using FluentValidation;
using IKitaplik.Business.Abstract;
using IKitaplık.Entities.Concrete;
using IKitaplık.Entities.DTOs;
using IKitaplik.DataAccess.UnitOfWork;

namespace IKitaplik.Business.Concrete
{
    public class BookManager : IBookService
    {
        
        private readonly IValidator<Book> _validator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMovementService _movementService;

        public BookManager( IValidator<Book> validator, IMovementService movementService, IUnitOfWork unitOfWork)
        {
            _movementService = movementService;
            _unitOfWork = unitOfWork;
            _validator = validator;
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
                _unitOfWork.Books.Add(book);

                var result = _movementService.Add(new Movement
                {
                    BookId = book.Id,
                    MovementDate = DateTime.Now,
                    Title = "Kitap Eklendi",
                    Note = $"{DateTime.Now:g} tarihinde {book.Name} adlı kitap kayıt edildi"
                });

                if (!result.Success)
                {
                    _unitOfWork.Rollback();
                    return new ErrorDataResult<Book>(result.Message);
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
                _unitOfWork.BeginTransaction();
                _unitOfWork.Books.Delete(book);


                var result = _movementService.Add(new Movement
                {
                    BookId = book.Id,
                    MovementDate = DateTime.Now,
                    Title = "Kitap Silindi",
                    Note = $"{DateTime.Now:g} tarihinde {book.Name} isimli kitapın tüm kayıtları silindi"
                });

                if (!result.Success)
                {
                    _unitOfWork.Rollback();
                    return new ErrorDataResult<Book>(result.Message);
                }

                _unitOfWork.Commit();
                return new SuccessResult("Kitap başarı ile oluşturuldu");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new ErrorResult("Kitap silinirken hata oluştu : " + ex.Message);
            }
        }

        public IDataResult<List<BookGetDTO>> GetAll()
        {
            try
            {
                List<BookGetDTO> books = _unitOfWork.Books.GetAllBookDTOs();
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
                Book book = _unitOfWork.Books.Get(p => p.Id == id);
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
                List<BookGetDTO> books = _unitOfWork.Books.GetAllBookFilteredDTOs(p => p.Name == name);
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
                _unitOfWork.BeginTransaction();

                _unitOfWork.Books.Update(book);

                var result = _movementService.Add(new Movement
                {
                    BookId = book.Id,
                    MovementDate = DateTime.Now,
                    Title = "Kitap Güncellendi",
                    Note = $"{DateTime.Now:g} tarihinde {book.Name} adlı kitap silindi",
                });

                if (!result.Success)
                {
                    _unitOfWork.Rollback();
                    return new ErrorResult(result.Message);
                }

                _unitOfWork.Commit();
                return new SuccessResult("Kitap başarı ile güncellendi");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new ErrorResult("Kitap güncellenirken hata oluştu : " + ex.Message);
            }
        }

        public IDataResult<Book> GetByBarcode(string barcode)
        {
            try
            {
                Book book = _unitOfWork.Books.Get(p => p.Barcode == barcode);
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
