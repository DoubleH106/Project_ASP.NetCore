using System;
using System.Collections.Generic;

namespace NguyenQuangHuong.Models;

public partial class Chat
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? Contents { get; set; }

    public DateTime? Datetime { get; set; }

    public virtual AccountsUser? User { get; set; }
}
