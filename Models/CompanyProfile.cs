namespace EpoxyFloorManager.Models;

public class CompanyProfile
{
    public int Id { get; set; }
    public string Name { get; set; } = "บริษัท เอ็กซ์เพิร์ท อีพ็อกซี่ จำกัด";
    public string Address { get; set; } = "999/99 ถนนรัชดาภิเษก แขวงจตุจักร เขตจตุจักร กรุงเทพมหานคร 10900";
    public string Phone { get; set; } = "02-123-4567";
    public string Email { get; set; } = "contact@expertepoxy.com";
    public string TaxId { get; set; } = "0105563999888";
}
