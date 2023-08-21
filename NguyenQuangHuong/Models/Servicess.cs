using System;
using System.Collections.Generic;

namespace NguyenQuangHuong.Models;

public partial class Servicess
{
    public int ServiceId { get; set; }

    public string? ServiceName { get; set; }

    public string? Avata { get; set; }

    public decimal? ServicePrice { get; set; }

    public int? Numberofprople { get; set; }

    public string? Description { get; set; }

    public int? BusinesId { get; set; }

    public int? Views { get; set; }

    public virtual Business? Busines { get; set; }

    public virtual ICollection<ServiceBooking> ServiceBookings { get; set; } = new List<ServiceBooking>();

    public virtual ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();
}
