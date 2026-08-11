using System;
using System.Collections.Generic;

namespace CRUD_Application.Scaffolded.Models;

public partial class Department
{
    public int DepartmentId { get; set; }

    public string DepartmentName { get; set; } = null!;

    public string DepartmentCode { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<Student> Students { get; set; } = new List<Student>();
}
