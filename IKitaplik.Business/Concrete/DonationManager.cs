﻿using Core.Utilities.Results;
using IKitaplik.Business.Abstract;
using IKitaplik.Entities.Concrete;
using IKitaplik.DataAccess.UnitOfWork;
using IKitaplik.Entities.DTOs.BookDTOs;
using AutoMapper;
using IKitaplik.Entities.DTOs.DonationDTOs;

namespace IKitaplik.Business.Concrete
{
    public class DonationManager : IDonationService
    {
        private readonly IBookService _bookService;
        private readonly IStudentService _studentService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMovementService _movementService;
        public DonationManager(IBookService bookService, IStudentService studentService, IUnitOfWork unitOfWork, IMovementService movementService,IMapper mapper)
        {
            _movementService = movementService;
            _unitOfWork = unitOfWork;
            _bookService = bookService;
            _studentService = studentService;
            _mapper = mapper;
        }
        public IResult Add(BookAddDto bookAddDto, DonationAddDto donationAddDto)
        {
            return HandleWithTransactionHelper.Handling(() =>
            {
                var donation = _mapper.Map<Donation>(donationAddDto);
                donation.CreatedDate = DateTime.Now;
                int bookId;
                // Kitap barcodunu kontrol et eğer ilgili barcode varsa +1 yap yoksa yeni kitap ekle
                var bookControl = _bookService.GetByBarcode(bookAddDto?.Barcode ?? "0");
                if (bookControl.Success)
                {
                    bookControl.Data.Piece += 1;
                    bookId = bookControl.Data.Id;
                    var updatedBook = _bookService.BookAddedPiece(new BookAddPieceDto { Id = bookControl.Data.Id, BeAdded = bookControl.Data.Piece }, true);
                    if (!updatedBook.Success)
                    {
                        return new ErrorResult(updatedBook.Message);
                    }
                }
                else
                {
                    var addedBook = _bookService.Add(bookAddDto ?? new BookAddDto(), true);
                    if (!addedBook.Success || addedBook.Data == null)
                    {
                        return new ErrorResult(addedBook.Message);
                    }
                    bookId = addedBook.Data.Id;
                }

                // Öğrenciyi bul eğer öğrenci var ise kitap sağlam ise 20 hasarlı ise 10 puan ekle
                var student = _studentService.GetById(donation.StudentId);
                if (student.Success && student.Data != null)
                {
                    if (donation.IsItDamaged == true)
                    {
                        student.Data.Point += 10;
                    }
                    else
                    {
                        student.Data.Point += 20;
                    }
                    var studentDto = _mapper.Map<StudentUpdateDto>(student.Data);
                    _studentService.Update(studentDto,true);
                }
                else
                {
                    return new ErrorResult("Öğrenci bulunamadı");
                }

                donation.BookId = bookId; // Kitap ID'sini donation'a ekle
                _unitOfWork.Donations.Add(donation);
                _movementService.Add(new Movement
                {
                    BookId = bookId,
                    CreatedDate = DateTime.Now,
                    DonationId = donation.Id,
                    StudentId = donation.StudentId,
                    MovementDate = DateTime.Now,
                    Type = Entities.Enums.MovementType.Donation,
                    Title = "Bağış yapıldı",
                    Note = $"{student.Data.Name} adlı öğrenci {bookAddDto.Name} adlı kitabı bağış olarak teslim etti"
                });
                return new SuccessResult("Bağış başarıyla eklendi.");

            }, _unitOfWork);
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
