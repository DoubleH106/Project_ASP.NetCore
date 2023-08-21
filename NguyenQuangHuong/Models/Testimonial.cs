using System;
using System.Collections.Generic;

namespace NguyenQuangHuong.Models;

public partial class Testimonial
{
    public int TestimonialId { get; set; }

    public int? AccountsUserId { get; set; }

    public string? TestimonialText { get; set; }

    public int? ServiceId { get; set; }

    public virtual AccountsUser? AccountsUser { get; set; }

    public virtual Servicess? Service { get; set; }
}
