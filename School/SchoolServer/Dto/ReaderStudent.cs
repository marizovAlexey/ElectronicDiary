namespace SchoolServer.Dto;

public class ReaderStudent
{
    public StudentGetDto Student { get; set; }
    public ClassGetDto Class { get; set; }
    public List<GradeGetDto>? Grades { get; set; }
}
