using System;
using System.Collections.Generic;

namespace CRUD_Application.Scaffolded.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public string Name { get; set; } = null!;

    public int Age { get; set; }

    public string Email { get; set; } = null!;

    public int DepartmentId { get; set; }

    public string? PhoneNumber { get; set; }

    public virtual Department Department { get; set; } = null!;
}
