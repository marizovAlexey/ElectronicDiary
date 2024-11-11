using Microsoft.EntityFrameworkCore;

namespace School.Classes;

public class ApplicationContext: DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    public DbSet<Student> Students { get; set; }

    public DbSet<Class> Classes { get; set; }

    public DbSet<Grade> Grades { get; set; }

    public DbSet<Subject> Subjects { get; set; }

    public ApplicationContext(DbContextOptions options) : base(options)
    {
        // Database.EnsureDeleted();
        Database.EnsureCreated();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // добавляем роли
        var adminRole = new Role { Id = 1, Name = "admin" };
        var userRole = new Role { Id = 2, Name = "reader" };
        var teacherRole = new Role { Id = 3, Name = "teacher" };

        var adminUser = new User { Id = 1, Login = "admin", Password = "admin", RoleId = adminRole.Id };
        var teacherUser = new User { Id = 2, Login = "teacher", Password = "teacher", RoleId = teacherRole.Id };
        var studentUser = new User { Id = 3, Login = "student", Password = "student", RoleId = userRole.Id };

        modelBuilder.Entity<Role>().HasData(new Role[] { adminRole, userRole, teacherRole });
        modelBuilder.Entity<User>().HasData(new User[] { adminUser, teacherUser, studentUser });

        var class1 = new Class { Id = 1, Number = 5, Letter = 'a' };
        var class2 = new Class { Id = 2, Number = 6, Letter = 'в' };
        var class3 = new Class { Id = 3, Number = 7, Letter = 'б' };

        modelBuilder.Entity<Class>().HasData(new List<Class> { class1, class2, class3 });

        var subject1 = new Subject(1, "Philosophy", 2008, null);
        var subject2 = new Subject(2, "Physics", 2020, null);
        var subject3 = new Subject(3, "Programming", 2023, null);

        modelBuilder.Entity<Subject>().HasData(new List<Subject> { subject1, subject2, subject3 });

        var student1 = new Student { Id = 1, FirstName = "Marizov", LastName = "Alexey", Patronymic = "Alexeevich", Passport = "2001-11111", ClassId = class1.Id, BirthDate = DateTime.SpecifyKind(DateTime.Parse("2001/1/10"), DateTimeKind.Utc), UserId = studentUser.Id };
        var student2 = new Student { Id = 2, FirstName = "Voronin", LastName = "Konstantin", Patronymic = "Borisovich", Passport = "2001-11111", ClassId = class2.Id, BirthDate = DateTime.SpecifyKind(DateTime.Parse("2000/1/11"), DateTimeKind.Utc) };
        var student3 = new Student { Id = 3, FirstName = "Arshavin", LastName = "Andrey", Patronymic = "Alexeevich", Passport = "2001-11111", ClassId = class3.Id, BirthDate = DateTime.SpecifyKind(DateTime.Parse("2001/2/16"), DateTimeKind.Utc) };
        var student4 = new Student { Id = 4, FirstName = "Barovik", LastName = "Andrey", Patronymic = "Alexeevich", Passport = "2001-11111", ClassId = class3.Id, BirthDate = DateTime.SpecifyKind(DateTime.Parse("2001/2/16"), DateTimeKind.Utc) };
        var student5 = new Student { Id = 5, FirstName = "Serebrin", LastName = "Andrey", Patronymic = "Alexeevich", Passport = "2001-11111", ClassId = class3.Id, BirthDate = DateTime.SpecifyKind(DateTime.Parse("2001/2/16"), DateTimeKind.Utc) };

        modelBuilder.Entity<Student>().HasData(new List<Student> { student1, student2, student3, student4, student5 });

        var grade1 = new Grade { Id = 1, SubjectId = subject1.Id, StudentId = student1.Id, Mark = 5, Date = DateTime.SpecifyKind(DateTime.Parse("2022/01/13"), DateTimeKind.Utc) };
        var grade2 = new Grade { Id = 2, SubjectId = subject2.Id, StudentId = student2.Id, Mark = 5, Date = DateTime.SpecifyKind(DateTime.Parse("2022/10/10"), DateTimeKind.Utc) };
        var grade3 = new Grade { Id = 3, SubjectId = subject3.Id, StudentId = student3.Id, Mark = 5, Date = DateTime.SpecifyKind(DateTime.Parse("2022/04/16"), DateTimeKind.Utc) };
        var grade4 = new Grade { Id = 4, SubjectId = subject3.Id, StudentId = student4.Id, Mark = 5, Date = DateTime.SpecifyKind(DateTime.Parse("2022/04/16"), DateTimeKind.Utc) };
        var grade5 = new Grade { Id = 5, SubjectId = subject3.Id, StudentId = student5.Id, Mark = 5, Date = DateTime.SpecifyKind(DateTime.Parse("2022/04/16"), DateTimeKind.Utc) };

        modelBuilder.Entity<Grade>().HasData(new List<Grade> { grade1, grade2, grade3, grade4, grade5 });

        base.OnModelCreating(modelBuilder);
    }
}
