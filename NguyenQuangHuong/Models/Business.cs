using System;
using System.Collections.Generic;

namespace NguyenQuangHuong.Models;

public partial class Business
{
    public int Id { get; set; }

    public string? BusinesName { get; set; }

    public string? Thumbnail { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Servicess> Servicesses { get; set; } = new List<Servicess>();
}
