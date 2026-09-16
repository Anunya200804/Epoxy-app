using System;
using System.Collections.Generic;

namespace EpoxyFloorManager.Models;

public class InstallationStepDetail
{
    public int Id { get; set; }
    
    public int InstallationProcessId { get; set; }
    public InstallationProcess? InstallationProcess { get; set; }
    
    public InstallationStep Step { get; set; }
    public string Description { get; set; } = string.Empty;
    public string PhotoUrl { get; set; } = string.Empty; // Store Base64 string for simplicity
    
    public List<InstallationStepMaterial> UsedMaterials { get; set; } = new();
}
