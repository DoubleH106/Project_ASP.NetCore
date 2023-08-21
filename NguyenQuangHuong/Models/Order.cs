using System;
using System.Collections.Generic;

namespace NguyenQuangHuong.Models;

public partial class Order
{
    public int Id { get; set; }

    public int? AccountId { get; set; }

    public bool? Paid { get; set; }

    public DateTime? PaymentDate { get; set; }

    public bool? Deletee { get; set; }

    public virtual AccountsUser? Account { get; set; }

    public virtual ICollection<ServiceBooking> ServiceBookings { get; set; } = new List<ServiceBooking>();
}
