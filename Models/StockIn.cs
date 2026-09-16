using System;

namespace EpoxyFloorManager.Models;

public enum StockInStatus
{
    Pending,
    Completed
}

public class StockIn
{
    public int Id { get; set; }
    public int InventoryId { get; set; }
    public Inventory? Inventory { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice => Quantity * UnitPrice;
    public DateTime ReceivedDate { get; set; } = DateTime.Now;
    public string ReceivedBy { get; set; } = string.Empty;
    public StockInStatus Status { get; set; } = StockInStatus.Completed;
}
