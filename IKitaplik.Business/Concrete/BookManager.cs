using Core.Utilities.Results;
using FluentValidation;
using IKitaplik.Business.Abstract;
using IKitaplik.Entities.Concrete;
using IKitaplik.DataAccess.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using IKitaplik.Entities.DTOs.BookDTOs;

namespace IKitaplik.Business.Concrete
{
    public class BookManager : IBookService
    {
        private readonly IValidator<Book> _validator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMovementService _movementService;

        public BookManager(IValidator<Book> validator, IMovementService movementService, IUnitOfWork unitOfWork)
        {
            _movementService = movementService;
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public IDataResult<Book> Add(Book book)
        {
            try
            {
                book.Id = 0;
                var validationResult = _validator.Validate(book);
                if (!validationResult.IsValid)
                {
                    return new ErrorDataResult<Book>(message: validationResult.Errors.Select(e => e.ErrorMessage)
                        .First());
                }

                var bookBarcodeSearch = GetByBarcode(book.Barcode);
                if (bookBarcodeSearch.Success)
                {
                    var res = BookAddedPiece(bookBarcodeSearch.Data);
                    return new SuccessDataResult<Book>(res.Message);
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

        public IResult Delete(int id)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var res = GetById(id);
                if (!res.Success)
                {
                    return new ErrorResult(res.Message);
                }

                _unitOfWork.Books.Delete(res.Data);
                _unitOfWork.Commit();
                return new SuccessResult("Kitap başarı ile silindi");
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                return new ErrorResult("Kitap silinirken hata oluştu : " + ex.Message);
            }
        }

        public IResult BookAddedPiece(Book books)
        {
            try
            {
                if (GetById(books.Id).Success)
                {
                    _unitOfWork.BeginTransaction();
                    books.Piece += 1;
                    _unitOfWork.Books.Update(books);
                    _unitOfWork.Commit();
                    return new SuccessResult("İlgili kitapın adedi 1 arttı");
                }
                else
                {
                    _unitOfWork.Rollback();
                    return new ErrorResult("İlgili kitap bulunamadı");
                }
            }
            catch (Exception e)
            {
                _unitOfWork.Rollback();
                return new ErrorResult("Kitap sayısı arttırılırken hata oluştu : " + e.Message);
            }
        }

        public IDataResult<List<BookGetDTO>> GetAllFiltered(BookFilterDto filter)
        {
            List<BookGetDTO> books = new List<BookGetDTO>();
            if (!string.IsNullOrEmpty(filter.barcode))
                books = _unitOfWork.Books.GetAllBookFilteredDTOs(dto => dto.Barcode.Equals(filter.barcode));
            else if (!string.IsNullOrEmpty(filter.category) && !string.IsNullOrEmpty(filter.title))
                books = _unitOfWork.Books.GetAllBookFilteredDTOs(dto => dto.CategoryName.Equals(filter.category) && dto.Name.Contains(filter.title));
            else if (!string.IsNullOrEmpty(filter.title))
                books = _unitOfWork.Books.GetAllBookFilteredDTOs(dto => dto.Name.Contains(filter.title));
            else if (!string.IsNullOrEmpty(filter.category))
                books = _unitOfWork.Books.GetAllBookFilteredDTOs(dto => dto.CategoryName.Equals(filter.category));
            else
                return new ErrorDataResult<List<BookGetDTO>>("Hatalı filtreleme yaptınız");

            if (books.Count > 0)
            {
                return new SuccessDataResult<List<BookGetDTO>>(books);
            }

            return new ErrorDataResult<List<BookGetDTO>>("Kayıt bulunamadı");
        }

        public IDataResult<List<BookGetDTO>> GetAll()
        {
            try
            {
                List<BookGetDTO> books = _unitOfWork.Books.GetAllBookDTOs().Take(50).ToList();
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
                List<BookGetDTO> books = _unitOfWork.Books.GetAllBookFilteredDTOs(p => p.Name.Contains(name)).Take(50)
                    .ToList();
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


                if (book.Id <= 0)
                {
                    return new ErrorResult("Id değeri gelmiyor");
                }

                try
                {
                    _unitOfWork.Books.Update(book);
                }
                catch (DbUpdateConcurrencyException)
                {
                    _unitOfWork.Rollback();
                    return new ErrorResult(
                        "Kitap başka bir işlem tarafından güncellendi veya silindi. Lütfen tekrar deneyin.");
                }

                var result = _movementService.Add(new Movement
                {
                    BookId = book.Id,
                    MovementDate = DateTime.Now,
                    Title = "Kitap Güncellendi",
                    Note = $"{DateTime.Now:g} tarihinde {book.Name} adlı kitap güncellendi",
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