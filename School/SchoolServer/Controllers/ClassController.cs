using AutoMapper;
using SchoolServer.Dto;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using School.Classes;
using SchoolServer.Services;

namespace SchoolServer.Controllers;

/// <summary>
/// Классы
/// </summary>
[Route("api/[controller]")]
[ApiController]
public class ClassController : ControllerBase
{
    private readonly ApplicationContext _context;
    private readonly IRoleCookieValidator _roleCookieValidator;

    private readonly IMapper _mapper;

    /// <summary>
    /// Конструктор ClassController
    /// </summary>
    /// <param name="context"></param>
    /// <param name="mapper"></param>
   public ClassController(ApplicationContext context, IMapper mapper, IRoleCookieValidator roleCookieValidator)
    {
        _context = context;
        _mapper = mapper;
        _roleCookieValidator = roleCookieValidator;
    }

    /// <summary>
    /// Получение всех классов
    /// </summary>
    /// <returns>Список всех классов</returns>
    [HttpGet(Name = "GetClasses")]
    public async Task<ActionResult<IEnumerable<ClassGetDto>>> GetClasses()
    {
        if (await _roleCookieValidator.CheckPermissions(HttpContext))
        {
            return await _mapper.ProjectTo<ClassGetDto>(_context.Classes).ToListAsync();
        }
        return NotFound();
    }

    /// <summary>
    /// Получение класса по id
    /// </summary>
   /// <returns>Класс</returns>
    [HttpPost("GetClassStudents")]
    public async Task<ActionResult<List<StudentGetDto>>> GetClass([FromBody] int id)
    {
        var permission = await _roleCookieValidator.CheckPermissions(HttpContext);
        if (!permission)
        {
            return NotFound();
        }
        var @class = await _context.Classes.FindAsync(id);

        if (@class == null)
        {
            return NotFound();
        }

        var students = _context.Students.Where(x => x.ClassId == id);
        var getStudents = await _mapper.ProjectTo<StudentGetDto>(students).ToListAsync();

        return Ok(getStudents);
    }
    /// <summary>
    /// Изменение данных о классе
    /// </summary>
    /// <param name="id">Идентификатор класса</param>
    /// <param name="class">Изменяемый класс</param>
    /// <returns>Результат выполнения операции</returns>
    [HttpPut("{id}", Name = "PutClass")]
    public async Task<IActionResult> PutClass(int id, ClassPostDto @class)
    {
        var permission = _roleCookieValidator.CheckAuthorization(HttpContext);
        if (!permission)
        {
            return Unauthorized();
        }

        var classToModify = await _context.Classes.FindAsync(id);
        if (classToModify == null)
        {
            return NotFound();
        }

        _mapper.Map(@class, classToModify);

        await _context.SaveChangesAsync();

        return Ok();
    }

    /// <summary>
    /// Добавление нового класса
    /// </summary>
    /// <param name="class">класс</param>
    /// <returns>Созданный класс</returns>
    [HttpPost(Name = "PostClass")]
    public async Task<ActionResult<int>> PostClass(ClassPostDto @class)
    {
        var permission = _roleCookieValidator.CheckAuthorization(HttpContext);
        if (!permission)
        {
            return Unauthorized();
        }
        var mappedClass = _mapper.Map<Class>(@class);

        _context.Classes.Add(mappedClass);
        await _context.SaveChangesAsync();

        return Ok(mappedClass.Id);
    }

    /// <summary>
    /// Удаление класса
    /// </summary>
    /// <param name="id">Идентификатор удаляемого класса</param>
    /// <returns>Результат выполнения операции</returns>
    [HttpDelete("{id}", Name = "DeleteClass")]
    public async Task<IActionResult> DeleteClass(int id)
    {
        var permission = _roleCookieValidator.CheckAuthorization(HttpContext);
        if (!permission)
        {
            return Unauthorized();
        }
        var @class = await _context.Classes.FindAsync(id);
        if (@class == null)
        {
            return NotFound();
        }

        _context.Classes.Remove(@class);
        await _context.SaveChangesAsync();

        return Ok();
    }
}


