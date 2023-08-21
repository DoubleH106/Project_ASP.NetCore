using System;
using System.Collections.Generic;

namespace NguyenQuangHuong.Models;

public partial class ServiceBooking
{
    public int BookingId { get; set; }

    public int? ServiceId { get; set; }

    public DateTime? BookingDate { get; set; }

    public DateTime? EndBookingDate { get; set; }

    public bool? BookingStatus { get; set; }

    public decimal? Price { get; set; }

    public int? NumberOfGuards { get; set; }

    public int? OrderId { get; set; }

    public bool? Confirm { get; set; }

    public bool? Status { get; set; }

    public virtual Order? Order { get; set; }

    public virtual Servicess? Service { get; set; }
}
