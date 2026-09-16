using System;

namespace EpoxyFloorManager.Models;

public class Customer
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string? LineId { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.Now;
}
