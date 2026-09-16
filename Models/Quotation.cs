using System;

namespace EpoxyFloorManager.Models;

public enum QuotationStatus
{
    Draft,     // ฉบับร่าง
    Approved,  // อนุมัติ
    Cancelled  // ยกเลิก
}

public class Quotation
{
    public int Id { get; set; }
    
    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }
    
    public string JobDescription { get; set; } = string.Empty;
    public decimal AreaSqm { get; set; }
    public decimal PricePerUnit { get; set; }
    
    public decimal TotalPrice => AreaSqm * PricePerUnit;
    public decimal DepositAmount { get; set; }
    public decimal BalanceAmount => TotalPrice - DepositAmount;
    
    public QuotationStatus Status { get; set; } = QuotationStatus.Draft;
    public DateTime CreatedDate { get; set; } = DateTime.Now;
    
    // Slip & Approval fields
    public string? PaymentSlipUrl { get; set; }
    public DateTime? ApprovedDate { get; set; }
    public string? ApprovedBy { get; set; }
    public string? PaymentNote { get; set; }
}
