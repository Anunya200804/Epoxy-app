using System;

namespace EpoxyFloorManager.Models;

public enum InstallationStep
{
    SurfacePrep,   // เตรียมพื้นผิว
    Priming,       // ทาสีรองพื้น
    EpoxyApplying, // เคลือบ Epoxy
    Completed      // เสร็จสิ้น (ส่งมอบงาน)
}

public enum PaymentStatus
{
    Pending, // ค้างชำระ
    Paid     // ชำระเงินครบถ้วน
}

public class InstallationProcess
{
    public int Id { get; set; }
    
    public int QuotationId { get; set; }
    public Quotation? Quotation { get; set; }
    
    public int? TechnicianTeamId { get; set; }
    public TechnicianTeam? TechnicianTeam { get; set; }
    
    public InstallationStep StepStatus { get; set; } = InstallationStep.SurfacePrep;
    
    public DateTime StartDate { get; set; } = DateTime.Now;
    public DateTime? EndDate { get; set; }
    
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
    public DateTime? PaymentDate { get; set; }
    
    public string? FinalPaymentSlipUrl { get; set; }
    public string? FinalPaymentNote { get; set; }
    public string? FinalPaymentApprovedBy { get; set; }

    // Computed properties for 7-day payment due date and 3% daily late penalty
    public DateTime? DueDate => EndDate?.Date.AddDays(7);

    public int OverdueDays
    {
        get
        {
            if (!EndDate.HasValue) return 0;
            var due = EndDate.Value.Date.AddDays(7);
            if (PaymentStatus == PaymentStatus.Pending)
            {
                var today = DateTime.Today;
                return today > due ? (int)(today - due).TotalDays : 0;
            }
            else if (PaymentDate.HasValue)
            {
                var paid = PaymentDate.Value.Date;
                return paid > due ? (int)(paid - due).TotalDays : 0;
            }
            return 0;
        }
    }

    public int DaysRemaining
    {
        get
        {
            if (!EndDate.HasValue || PaymentStatus == PaymentStatus.Paid) return 0;
            var due = EndDate.Value.Date.AddDays(7);
            var today = DateTime.Today;
            return due >= today ? (int)(due - today).TotalDays : 0;
        }
    }

    public decimal LatePenaltyRate => 0.03m; // 3% ต่อวัน

    public decimal LatePenaltyAmount
    {
        get
        {
            if (OverdueDays <= 0) return 0;
            var balance = Quotation?.BalanceAmount ?? 0;
            return Math.Round(OverdueDays * (balance * LatePenaltyRate), 2);
        }
    }

    public decimal TotalPayableAmount
    {
        get
        {
            var balance = Quotation?.BalanceAmount ?? 0;
            return balance + LatePenaltyAmount;
        }
    }
}
