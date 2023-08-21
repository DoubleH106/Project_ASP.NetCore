using System;
using System.Collections.Generic;

namespace NguyenQuangHuong.Models;

public partial class Branch
{
    public int BranchId { get; set; }

    public string? BranchName { get; set; }

    public int? RegionId { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();

    public virtual Region? Region { get; set; }
}
