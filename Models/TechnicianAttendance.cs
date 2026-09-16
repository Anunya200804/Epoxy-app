namespace EpoxyFloorManager.Models;

public enum AttendanceStatus
{
    Working,   // ทำงาน
    Leave,     // ลา
    Absent     // ขาด
}

public class TechnicianAttendance
{
    public int Id { get; set; }
    public int TechnicianId { get; set; }
    public DateTime Date { get; set; } = DateTime.Today;
    public AttendanceStatus Status { get; set; } = AttendanceStatus.Working;
    public string Note { get; set; } = string.Empty;
    public string RecordedBy { get; set; } = string.Empty;

    // Navigation
    public Technician? Technician { get; set; }
}
