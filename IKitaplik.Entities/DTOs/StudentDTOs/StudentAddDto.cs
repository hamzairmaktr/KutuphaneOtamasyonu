using Core.Entities;

public class StudentAddDto: IDto
{
    public int StudentNumber { get; set; }
    public string Name { get; set; }
    public string Class { get; set; }
    public string TelephoneNumber { get; set; }
    public string EMail { get; set; }
} 