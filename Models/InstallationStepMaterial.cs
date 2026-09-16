using System;

namespace EpoxyFloorManager.Models;

public class InstallationStepMaterial
{
    public int Id { get; set; }
    
    public int InstallationStepDetailId { get; set; }
    public InstallationStepDetail? InstallationStepDetail { get; set; }
    
    public int InventoryId { get; set; }
    public Inventory? Inventory { get; set; }
    
    public decimal QuantityUsed { get; set; }
}
