using Core.Entities;

namespace IKitaplik.Entities.DTOs.BookDTOs
{
    public class BookAutocompleteDto : IDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public string WriterName { get; set; }
        public string ShelfNo { get; set; }
    }
}
