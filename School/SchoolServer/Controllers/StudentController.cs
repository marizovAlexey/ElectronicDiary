using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using School.Classes;
using SchoolServer.Dto;
using Microsoft.EntityFrameworkCore;
using SchoolServer.Services;
using System.Security.Claims;

namespace SchoolServer.Controllers;

/// <summary>
/// Студенты
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class StudentController : ControllerBase
{
    private readonly ApplicationContext _context;
    private readonly IRoleCookieValidator _roleCookieValidator;
    private readonly IMapper _mapper;

    /// <summary>
    /// Конструктор класса StudentsController
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
    public StudentController(ApplicationContext context, IMapper mapper, IRoleCookieValidator roleCookieValidator)
    {
        _context = context;
        _mapper = mapper;
        _roleCookieValidator = roleCookieValidator;
    }

       /// <summary>
    /// Получение всех студентов
    /// </summary>
    /// <returns>Список всех студентов</returns>
    [HttpGet(Name = "GetStudents")]
    public async Task<ActionResult<IEnumerable<StudentGetDto>>> GetStudents()
    {
        var permission = await _roleCookieValidator.CheckPermissions(HttpContext);
        if (!permission)
        {
            return NotFound();
        }
        return await _mapper.ProjectTo<StudentGetDto>(_context.Students).ToListAsync();
    }

    [HttpPost("GetGrades")]
    public async Task<ActionResult<List<GradeGetDto>?>> GetGradesByStudentId([FromBody] int id)
    {
        var permission = await _roleCookieValidator.CheckPermissions(HttpContext);
        if (!permission)
        {
            return NotFound();
        }
        var student = await _context.Students.FindAsync(id);

        if (student == null)
        {
            return NotFound();
        }

        var grades = _context.Grades.Where(x => x.StudentId == id);
        var listGrades = await _mapper.ProjectTo<GradeGetDto>(grades).ToListAsync();

        return Ok(listGrades);
    }

    [HttpGet("GetUserID")]
    public async Task<ActionResult<int?>> GetUserID()
    {
        var userLogin = HttpContext.User.FindFirst(ClaimsIdentity.DefaultNameClaimType)?.Value;
        if (string.IsNullOrEmpty(userLogin))
        {
            return NotFound();
        }
        
        var user = await _context.Users.SingleOrDefaultAsync(x => x.Login == userLogin);
        return Ok(user?.Id);
    }


    [HttpPost("GetStudentByUserID")]
    public async Task<ActionResult<ReaderStudent>> GetStudentByUserID([FromBody] int userId)
    {
        var permission = _roleCookieValidator.CheckAuthorization(HttpContext);
        if (!permission)
        {
            return NotFound();
        }
        var student = await _context.Students.SingleOrDefaultAsync(x => x.UserId == userId);
        var studentMap = _mapper.Map<StudentGetDto>(student);

        if (student == null)
        {
            return NotFound();
        }

        var grades = _context.Grades.Where(x => x.StudentId == studentMap.Id);
        var listGrades = await _mapper.ProjectTo<GradeGetDto>(grades).ToListAsync();

        var @class = await _context.Classes.FindAsync(student.ClassId);

        var studentInfo = new ReaderStudent()
        {
            Student = studentMap,
            Grades = listGrades,
            Class = _mapper.Map<ClassGetDto>(@class)
        };

        return Ok(studentInfo);
    }

    /// <summary>
    /// Изменение данных выбранного студента
    /// </summary>
    /// <param name="id">Идентификатор студента</param>
    /// <param name="student">Студент, данные которого нужно изменить</param>
    /// <returns>Результат выполнения операции</returns>
    [HttpPut("{id}", Name = "PutStudent")]
    public async Task<IActionResult> PutStudent(int id, StudentPostDto student)
    {
        if (_context.Students == null)
        {
            return NotFound();
        }

        var studentToModify = await _context.Students.FindAsync(id);
        if (studentToModify == null)
        {
            return NotFound();
        }

        _mapper.Map(student, studentToModify);

        await _context.SaveChangesAsync();

        return Ok();
    }

    /// <summary>
    /// Добавление нового студента
    /// </summary>
    /// <param name="student">студент</param>
    /// <returns>Добавленный студент</returns>
    [HttpPost(Name = "PostStudent")]
    public async Task<ActionResult<int>> PostStudent(StudentPostDto student)
    {
        if (_context.Students == null)
        {
            return Problem("Entity set 'DiaryDomainDbContext.Students'  is null.");
        }

        var mappedStudent = _mapper.Map<Student>(student);

        _context.Students.Add(mappedStudent);
        await _context.SaveChangesAsync();

        return Ok(mappedStudent.Id);
    }

    /// <summary>
    /// Удаление студента
    /// </summary>
    /// <param name="id">Идентификатор студента </param>
    /// <returns>Результат выполнения операции</returns>
    [HttpDelete("{id}", Name = "DeleteStudent")]
    public async Task<IActionResult> DeleteStudent(int id)
    {
        if (_context.Students == null)
        {
            return NotFound();
        }
        var student = await _context.Students.FindAsync(id);
        if (student == null)
        {
            return NotFound();
        }

        _context.Students.Remove(student);
        await _context.SaveChangesAsync();

        return Ok();
    }

}
