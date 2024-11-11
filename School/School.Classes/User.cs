using System;
namespace School.Classes;

public class User
{
    public int Id { get; set; }
    public string Login { get; set; }
    public string Password { get; set; }

    public int? RoleId { get; set; }
    public Role Role { get; set; }
}

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; }
}
