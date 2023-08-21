using System;
using System.Collections.Generic;

namespace NguyenQuangHuong.Models;

public partial class Employee
{
    public int UserId { get; set; }

    public string? FullName { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public bool? Gender { get; set; }

    public DateTime? Birthday { get; set; }

    public string? Avata { get; set; }

    public string? Address { get; set; }

    public DateTime? LastLogin { get; set; }

    public DateTime? CreateDate { get; set; }

    public int? RoleId { get; set; }

    public string? EducationalQualification { get; set; }

    public int? DepartmentId { get; set; }

    public string? Achievements { get; set; }

    public bool? Active { get; set; }

    public int? BranchId { get; set; }

    public virtual Branch? Branch { get; set; }

    public virtual Department? Department { get; set; }

    public virtual ICollection<News> News { get; set; } = new List<News>();

    public virtual Role? Role { get; set; }
}
