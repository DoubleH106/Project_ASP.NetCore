using System;
using System.Collections.Generic;

namespace NguyenQuangHuong.Models;

public partial class AccountsUser
{
    public int Id { get; set; }

    public string? FullName { get; set; }

    public string? Phone { get; set; }

    public string? PassWord { get; set; }

    public DateTime? Birthday { get; set; }

    public string? Address { get; set; }

    public string? Email { get; set; }

    public string? Avata { get; set; }

    public string? Gender { get; set; }

    public DateTime? CreataDate { get; set; }

    public virtual ICollection<Chat> Chats { get; set; } = new List<Chat>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Testimonial> Testimonials { get; set; } = new List<Testimonial>();
}
