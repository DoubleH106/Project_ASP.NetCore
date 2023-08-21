using System;
using System.Collections.Generic;

namespace NguyenQuangHuong.Models;

public partial class News
{
    public int Id { get; set; }

    public int? EmployessId { get; set; }

    public string? Title { get; set; }

    public string? Content { get; set; }

    public string? ImageUrl { get; set; }

    public DateTime? PublishedDate { get; set; }

    public bool? Status { get; set; }

    public virtual Employee? Employess { get; set; }
}
