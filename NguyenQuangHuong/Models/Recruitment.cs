using System;
using System.Collections.Generic;

namespace NguyenQuangHuong.Models;

public partial class Recruitment
{
    public int RecruitmentId { get; set; }

    public int? DepartmentId { get; set; }

    public string? EmployeeName { get; set; }

    public string? EmployeeAddress { get; set; }

    public string? Phone { get; set; }

    public string? Avata { get; set; }

    public string? EducationalQualification { get; set; }

    public bool? Active { get; set; }

    public bool? Gender { get; set; }

    public DateTime? Birthday { get; set; }

    public string? Email { get; set; }

    public virtual Department? Department { get; set; }
}
