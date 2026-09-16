using System.Collections.Generic;

namespace EpoxyFloorManager.Models;

public class TechnicianTeam
{
    public int Id { get; set; }
    public string TeamName { get; set; } = string.Empty;
    public string LeaderName { get; set; } = string.Empty;

    // Navigation property
    public List<Technician> Technicians { get; set; } = new();
}
