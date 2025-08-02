using Core.Entities;

public class StudentGetDto: IDto
{
    public int Id { get; set; }
    public int StudentNumber { get; set; }
    public string Name { get; set; }
    public string Class { get; set; }
    public string TelephoneNumber { get; set; }
    public string EMail { get; set; }
    public int Point { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
} 