namespace EpoxyFloorManager.Models;

public enum TechnicianStatus
{
    Available,
    Busy
}

public class Technician
{
    public int Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public TechnicianStatus Status { get; set; } = TechnicianStatus.Available;

    public int? TeamId { get; set; }
    public TechnicianTeam? Team { get; set; }
}
