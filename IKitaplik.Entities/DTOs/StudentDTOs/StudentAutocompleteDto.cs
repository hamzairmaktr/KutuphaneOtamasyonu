using Core.Entities;

namespace IKitaplik.Entities.DTOs.StudentDTOs
{
    public class StudentAutocompleteDto : IDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int StudentNumber { get; set; }
        public string Class { get; set; }
    }
}
