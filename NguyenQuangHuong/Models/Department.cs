using System;
using System.Collections.Generic;

namespace NguyenQuangHuong.Models;

public partial class Department
{
    public int DepartmentId { get; set; }

    public string? DepartmentName { get; set; }

    public string? Location { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual ICollection<Recruitment> Recruitments { get; set; } = new List<Recruitment>();
}
