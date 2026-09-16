using System;

namespace EpoxyFloorManager.Models;

public enum FinanceType
{
    Income, // รายรับ
    Expense // รายจ่าย
}

public class Finance
{
    public int Id { get; set; }
    public FinanceType Type { get; set; }
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime RecordedDate { get; set; } = DateTime.Now;
    
    public int? QuotationId { get; set; }
    public Quotation? Quotation { get; set; }
    
    public string ReferenceNo { get; set; } = string.Empty;
}
