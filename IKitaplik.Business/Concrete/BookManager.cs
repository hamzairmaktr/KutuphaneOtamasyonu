using Core.Utilities.Results;
using FluentValidation;
using IKitaplik.Business.Abstract;
using IKitaplik.Entities.Concrete;
using IKitaplik.DataAccess.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using IKitaplik.Entities.DTOs.BookDTOs;
using AutoMapper;
using System.Threading.Tasks;

namespace IKitaplik.Business.Concrete
{
    public class BookManager : IBookService
    {
        private readonly IValidator<Book> _validator;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMovementService _movementService;
        private readonly IMapper _mapper;
        private readonly IImageService _imageService;

        public BookManager(IValidator<Book> validator, IMovementService movementService, IUnitOfWork unitOfWork, IMapper mapper, IImageService imageService)
        {
            _movementService = movementService;
            _unitOfWork = unitOfWork;
            _validator = validator;
            _mapper = mapper;
            _imageService = imageService;
        }

        public async Task<IDataResult<Book>> AddAsync(BookAddDto bookAddDto, bool isDonation = false)
        {
            if (isDonation)
            {
                return await AddLocalAsync(bookAddDto);
            }
            else
            {
                return await HandleWithTransactionHelper.Handling<Book>(async () =>
                {
                    return await AddLocalAsync(bookAddDto);
                }, _unitOfWork);
            }
        }

        public async Task<IResult> DeleteAsync(int id)
        {
            return await HandleWithTransactionHelper.Handling(async () =>
            {
                var res = await GetByIdAsync(id);
                if (!res.Success)
                {
                    return new ErrorResult(res.Message);
                }
                await _unitOfWork.Books.DeleteAsync(res.Data);
                await _imageService.DeleteAllAsync(Entities.Enums.ImageType.Book, id);
                return new SuccessResult("Kitap başarı ile silindi");

            }, _unitOfWork);
        }

        public async Task<IResult> BookAddedPieceAsync(BookAddPieceDto bookAddPieceDto, bool isDonationOrDeposit = false)
        {
            if (isDonationOrDeposit)
            {
                return await BookAddedPieceLocalAsync(bookAddPieceDto);
            }
            else
            {
                return await HandleWithTransactionHelper.Handling(async () =>
                {
                    return await BookAddedPieceLocalAsync(bookAddPieceDto);
                }, _unitOfWork);

            }
        }

        public async Task<IDataResult<List<BookGetDTO>>> GetAllFilteredAsync(BookFilterDto filter)
        {
            List<BookGetDTO> books = new List<BookGetDTO>();
            if (!string.IsNullOrEmpty(filter.barcode))
                books = await _unitOfWork.Books.GetAllBookDTOsAsync(dto => dto.Barcode.Equals(filter.barcode));
            else if (!string.IsNullOrEmpty(filter.category) && !string.IsNullOrEmpty(filter.title))
                books = await _unitOfWork.Books.GetAllBookDTOsAsync(dto => dto.CategoryName.Equals(filter.category) && dto.Name.Contains(filter.title));
            else if (!string.IsNullOrEmpty(filter.title))
                books = await _unitOfWork.Books.GetAllBookDTOsAsync(dto => dto.Name.Contains(filter.title));
            else if (!string.IsNullOrEmpty(filter.category))
                books = await _unitOfWork.Books.GetAllBookDTOsAsync(dto => dto.CategoryName.Equals(filter.category));
            else
                return new ErrorDataResult<List<BookGetDTO>>("Hatalı filtreleme yaptınız");

            if (books.Count > 0)
            {
                return new SuccessDataResult<List<BookGetDTO>>(books);
            }

            return new ErrorDataResult<List<BookGetDTO>>("Kayıt bulunamadı");
        }

        public async Task<IDataResult<List<BookGetDTO>>> GetAllAsync()
        {
            try
            {
                List<BookGetDTO> books = await _unitOfWork.Books.GetAllBookDTOsAsync();
                return new SuccessDataResult<List<BookGetDTO>>(books, "Kitaplar başarı ile çekildi");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<BookGetDTO>>("Kitaplar çekilirken bir hata oluştu : " + ex.Message);
            }
        }

        public async Task<IDataResult<Book>> GetByIdAsync(int id)
        {
            try
            {
                Book book = await _unitOfWork.Books.GetAsync(p => p.Id == id);
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

        public async Task<IDataResult<List<BookGetDTO>>> GetAllByNameAsync(string name)
        {
            try
            {
                List<BookGetDTO> books = await _unitOfWork.Books.GetAllBookDTOsAsync(p => p.Name.Contains(name));
                return new SuccessDataResult<List<BookGetDTO>>(books, "Kitaplar başarı ile çekildi");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<BookGetDTO>>("İlgili kitaplar çekilirken hata oluştu : " + ex.Message);
            }
        }

        public async Task<IResult> UpdateAsync(BookUpdateDto bookUpdateDto)
        {
            return await HandleWithTransactionHelper.Handling(async () =>
            {
                var bookExisting = await GetByIdAsync(bookUpdateDto.Id);
                if (!bookExisting.Success)
                {
                    return new ErrorResult(bookExisting.Message);
                }
                DateTime createdDate = bookExisting.Data.CreatedDate;
                Book book = _mapper.Map<Book>(bookUpdateDto);
                book.CreatedDate = createdDate;
                book.UpdatedDate = DateTime.Now;
                var validationResult = _validator.Validate(book);
                if (!validationResult.IsValid)
                {
                    return new ErrorResult(validationResult.Errors.Select(e => e.ErrorMessage)
                        .First());
                }

                await _unitOfWork.Books.UpdateAsync(book);

                var result = await _movementService.AddAsync(new Movement
                {
                    BookId = book.Id,
                    MovementDate = DateTime.Now,
                    Title = "Kitap Güncellendi",
                    Note = $"{DateTime.Now:g} tarihinde {book.Name} adlı kitap güncellendi",
                });

                if (!result.Success)
                {
                    return new ErrorResult(result.Message);
                }

                return new SuccessResult("Kitap başarı ile güncellendi");

            }, _unitOfWork);
        }

        public async Task<IDataResult<Book>> GetByBarcodeAsync(string barcode)
        {
            try
            {
                Book book = await _unitOfWork.Books.GetAsync(p => p.Barcode == barcode);
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

        private async Task<IDataResult<Book>> AddLocalAsync(BookAddDto bookAddDto)
        {
            var book = _mapper.Map<Book>(bookAddDto);
            book.CreatedDate = DateTime.Now;
            book.Situation = true;

            var bookBarcodeSearch = await GetByBarcodeAsync(book.Barcode);
            if (bookBarcodeSearch.Success)
            {
                return new ErrorDataResult<Book>("Aynı barkoda ait bir kitap var");
            }

            var validationResult = _validator.Validate(book);
            if (!validationResult.IsValid)
            {
                return new ErrorDataResult<Book>(message: validationResult.Errors.Select(e => e.ErrorMessage)
                    .First());
            }

            await _unitOfWork.Books.AddAsync(book);

            var result = await _movementService.AddAsync(new Movement
            {
                BookId = book.Id,
                MovementDate = DateTime.Now,
                Title = "Kitap Eklendi",
                Note = $"{DateTime.Now:g} tarihinde {book.Name} adlı kitap kayıt edildi"
            });

            if (!result.Success)
            {
                return new ErrorDataResult<Book>(result.Message);
            }
            return new SuccessDataResult<Book>(book, message: "Kitap başarı ile oluşturuldu");
        }
        private async Task<IResult> BookAddedPieceLocalAsync(BookAddPieceDto bookAddPieceDto)
        {
            var res = await GetByIdAsync(bookAddPieceDto.Id);
            if (res.Success)
            {
                res.Data.Piece += bookAddPieceDto.BeAdded;
                await _unitOfWork.Books.UpdateAsync(res.Data);
                if (bookAddPieceDto.BeAdded >= 0)
                {
                    return new SuccessResult($"İlgili kitapın adedi {bookAddPieceDto.BeAdded} arttı");
                }
                else
                {
                    return new SuccessResult($"İlgili kitapın adedi {bookAddPieceDto.BeAdded} azaldı");
                }
            }
            else
            {
                return new ErrorResult("İlgili kitap bulunamadı");
            }
        }

        public async Task<IDataResult<List<BookGetDTO>>> GetAllActiveAsync()
        {
            try
            {
                List<BookGetDTO> books = await _unitOfWork.Books.GetAllBookDTOsAsync(p => p.Piece > 0);
                return new SuccessDataResult<List<BookGetDTO>>(books, "Kitaplar başarı ile çekildi");
            }
            catch (Exception ex)
            {
                return new ErrorDataResult<List<BookGetDTO>>("Kitaplar çekilirken bir hata oluştu : " + ex.Message);
            }
        }
    }
}