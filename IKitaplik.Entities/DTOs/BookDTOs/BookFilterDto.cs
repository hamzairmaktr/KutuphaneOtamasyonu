using Core.Entities;

namespace IKitaplik.Entities.DTOs.BookDTOs;

public class BookFilterDto:IDto
{
    public string barcode { get; set; }
    public string title { get; set; }
    public string category { get; set; }
}