using System;
using System.Collections.Generic;

namespace NguyenQuangHuong.Models;

public partial class Region
{
    public int RegionId { get; set; }

    public string? RegionName { get; set; }

    public virtual ICollection<Branch> Branches { get; set; } = new List<Branch>();
}
