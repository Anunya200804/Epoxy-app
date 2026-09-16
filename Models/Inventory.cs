namespace EpoxyFloorManager.Models;

public class Inventory
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Quantity { get; set; }
    public string Unit { get; set; } = string.Empty;
    public decimal ReorderPoint { get; set; }
    public decimal UnitPrice { get; set; }
    
    public bool IsLowStock => Quantity <= ReorderPoint;
}
