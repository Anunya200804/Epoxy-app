using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using EpoxyFloorManager.Models;

namespace EpoxyFloorManager.Data;

public static class DbInitializer
{
    public static void Initialize(EpoxyDbContext context)
    {
        context.Database.EnsureCreated();

        // Auto-migrate new columns if they don't exist
        MigrateNewColumns(context);

        // Seed default company profile if empty
        if (!context.CompanyProfiles.Any())
        {
            context.CompanyProfiles.Add(new CompanyProfile
            {
                Name = "บริษัท เอ็กซ์เพิร์ท อีพ็อกซี่ จำกัด",
                Address = "999/99 ถนนรัชดาภิเษก แขวงจตุจักร เขตจตุจักร กรุงเทพมหานคร 10900",
                Phone = "02-123-4567",
                Email = "contact@expertepoxy.com",
                TaxId = "0105563999888"
            });
            context.SaveChanges();
        }

        // Ensure all users have correct Thai names
        var adminUser = context.Users.FirstOrDefault(u => u.Username == "admin");
        if (adminUser != null)
        {
            adminUser.FullName = "สมเกียรติ รักงาน";
            adminUser.Role = UserRole.Admin;
        }

        var pmUser = context.Users.FirstOrDefault(u => u.Username == "pm");
        if (pmUser != null)
        {
            pmUser.FullName = "วิชัย จัดการดี";
            pmUser.Role = UserRole.ProjectManager;
        }

        var staffUser = context.Users.FirstOrDefault(u => u.Username == "staff");
        if (staffUser != null)
        {
            staffUser.FullName = "สมศรี มีใจ";
            staffUser.Role = UserRole.Staff;
        }

        context.SaveChanges();

        if (context.Users.Any())
        {
            return;
        }

        // 1. Seed Users
        var users = new User[]
        {
            new User { Username = "admin", PasswordHash = "admin123", Role = UserRole.Admin, FullName = "สมเกียรติ รักงาน", Phone = "081-111-1111" },
            new User { Username = "pm", PasswordHash = "pm123", Role = UserRole.ProjectManager, FullName = "วิชัย จัดการดี", Phone = "082-222-2222" },
            new User { Username = "staff", PasswordHash = "staff123", Role = UserRole.Staff, FullName = "สมศรี มีใจ", Phone = "083-333-3333" }
        };
        context.Users.AddRange(users);
        context.SaveChanges();

        // 2. Seed Customers
        var customer1 = new Customer { Name = "บริษัท สยามโลจิสติกส์ แวร์เฮาส์ จำกัด", Phone = "02-555-9999", Address = "888 นิคมอุตสาหกรรมบางปู สมุทรปราการ", LineId = "siam_logistics", CreatedDate = DateTime.Now.AddDays(-30) };
        var customer2 = new Customer { Name = "กาแฟแสนอร่อย คาเฟ่ (คุณจอย)", Phone = "089-876-5432", Address = "99 ซอยสุขุมวิท 21 กรุงเทพฯ", LineId = "joy_coffee", CreatedDate = DateTime.Now.AddDays(-20) };
        var customer3 = new Customer { Name = "คุณสมชาย พัฒนาการ", Phone = "081-234-5678", Address = "123/45 ถนนพัฒนาการ กรุงเทพฯ", LineId = "somchai_epoxy", CreatedDate = DateTime.Now.AddDays(-10) };
        context.Customers.AddRange(customer1, customer2, customer3);
        context.SaveChanges();

        // 3. Seed Teams and Technicians
        var teamA = new TechnicianTeam { TeamName = "ทีมช่าง A (งานใหญ่)", LeaderName = "ช่างแก้ว" };
        var teamB = new TechnicianTeam { TeamName = "ทีมช่าง B (งานด่วน)", LeaderName = "ช่างน้อย" };
        context.TechnicianTeams.AddRange(teamA, teamB);
        context.SaveChanges();

        var tech1 = new Technician { FullName = "นายแก้ว กล้าหาญ", Phone = "084-123-4567", Position = "หัวหน้าทีม A", Status = TechnicianStatus.Busy, TeamId = teamA.Id };
        var tech2 = new Technician { FullName = "นายศักดิ์ สิทธิ์ดี", Phone = "084-234-5678", Position = "ช่างเตรียมพื้นผิว", Status = TechnicianStatus.Busy, TeamId = teamA.Id };
        var tech3 = new Technician { FullName = "นายพล คนเก่ง", Phone = "084-345-6789", Position = "ช่างเคลือบ Epoxy", Status = TechnicianStatus.Busy, TeamId = teamA.Id };
        var tech4 = new Technician { FullName = "นายทองดี มีชัย", Phone = "085-111-2222", Position = "หัวหน้าทีม B", Status = TechnicianStatus.Available, TeamId = teamB.Id };
        var tech5 = new Technician { FullName = "นายมด แดงดี", Phone = "085-333-4444", Position = "ช่างเคลือบ Epoxy", Status = TechnicianStatus.Available, TeamId = teamB.Id };
        context.Technicians.AddRange(tech1, tech2, tech3, tech4, tech5);
        context.SaveChanges();

        // 4. Seed Inventory
        var inv1 = new Inventory { Name = "Epoxy Resin A (สารเรซิ่น)", Quantity = 150m, Unit = "ชุด", ReorderPoint = 30m };
        var inv2 = new Inventory { Name = "Epoxy Hardener B (สารเร่งแข็ง)", Quantity = 150m, Unit = "ชุด", ReorderPoint = 30m };
        var inv3 = new Inventory { Name = "Epoxy Self-Leveling Primer", Quantity = 45m, Unit = "แกลลอน", ReorderPoint = 15m };
        var inv4 = new Inventory { Name = "เทปกาว Masking Tape 2 นิ้ว", Quantity = 12m, Unit = "ม้วน", ReorderPoint = 20m };
        var inv5 = new Inventory { Name = "รองเท้าตะปู (Spiked Shoes)", Quantity = 8m, Unit = "คู่", ReorderPoint = 5m };
        context.Inventories.AddRange(inv1, inv2, inv3, inv4, inv5);
        context.SaveChanges();

        // 5. Seed StockIn Records
        var stock1 = new StockIn { InventoryId = inv1.Id, Quantity = 50m, ReceivedDate = DateTime.Now.AddDays(-15), ReceivedBy = "ช่างแก้ว", Status = StockInStatus.Completed };
        var stock2 = new StockIn { InventoryId = inv2.Id, Quantity = 50m, ReceivedDate = DateTime.Now.AddDays(-15), ReceivedBy = "ช่างแก้ว", Status = StockInStatus.Completed };
        context.StockIns.AddRange(stock1, stock2);
        context.SaveChanges();

        // 6. Seed Quotations
        var quote1 = new Quotation
        {
            CustomerId = customer1.Id,
            JobDescription = "เคลือบพื้น Epoxy Self-Leveling โรงงานบางปู ความหนา 2 มม.",
            AreaSqm = 500m,
            PricePerUnit = 650m,
            DepositAmount = 97500m,
            Status = QuotationStatus.Approved,
            CreatedDate = DateTime.Now.AddDays(-12)
        };
        
        var quote2 = new Quotation
        {
            CustomerId = customer2.Id,
            JobDescription = "เคลือบพื้น Epoxy Coating ห้องครัว คาเฟ่สุขุมวิท",
            AreaSqm = 80m,
            PricePerUnit = 850m,
            DepositAmount = 20400m,
            Status = QuotationStatus.Approved,
            CreatedDate = DateTime.Now.AddDays(-8)
        };

        var quote3 = new Quotation
        {
            CustomerId = customer3.Id,
            JobDescription = "งานเตรียมพื้นผิวและทารองพื้น โชว์รูมพัฒนาการ",
            AreaSqm = 120m,
            PricePerUnit = 300m,
            DepositAmount = 10800m,
            Status = QuotationStatus.Draft,
            CreatedDate = DateTime.Now.AddDays(-2)
        };
        
        context.Quotations.AddRange(quote1, quote2, quote3);
        context.SaveChanges();

        // 7. Seed Installation Processes
        var proj1 = new InstallationProcess
        {
            QuotationId = quote1.Id,
            TechnicianTeamId = teamA.Id,
            StepStatus = InstallationStep.EpoxyApplying,
            StartDate = DateTime.Now.AddDays(-5),
            PaymentStatus = PaymentStatus.Pending
        };

        var proj2 = new InstallationProcess
        {
            QuotationId = quote2.Id,
            TechnicianTeamId = teamB.Id,
            StepStatus = InstallationStep.Completed,
            StartDate = DateTime.Now.AddDays(-7),
            EndDate = DateTime.Now.AddDays(-2),
            PaymentStatus = PaymentStatus.Paid,
            PaymentDate = DateTime.Now.AddDays(-2)
        };
        context.InstallationProcesses.AddRange(proj1, proj2);
        context.SaveChanges();

        // 8. Seed Finances
        var fin1 = new Finance { Type = FinanceType.Income, Amount = 97500m, Description = "รับเงินมัดจำ 30% ใบเสนอราคา เลขที่ QT" + quote1.Id, RecordedDate = DateTime.Now.AddDays(-12), QuotationId = quote1.Id, ReferenceNo = "REC-1001" };
        var fin2 = new Finance { Type = FinanceType.Income, Amount = 20400m, Description = "รับเงินมัดจำ 30% ใบเสนอราคา เลขที่ QT" + quote2.Id, RecordedDate = DateTime.Now.AddDays(-8), QuotationId = quote2.Id, ReferenceNo = "REC-1002" };
        var fin3 = new Finance { Type = FinanceType.Income, Amount = 47600m, Description = "รับเงินงวดสุดท้าย (ค่าติดตั้งที่เหลือ) โครงการใบเสนอราคา QT" + quote2.Id, RecordedDate = DateTime.Now.AddDays(-2), QuotationId = quote2.Id, ReferenceNo = "REC-1003" };
        var fin4 = new Finance { Type = FinanceType.Expense, Amount = 35000m, Description = "ซื้อสีรองพื้นและเทปกาว ล็อตใหม่", RecordedDate = DateTime.Now.AddDays(-10), ReferenceNo = "EXP-2001" };
        var fin5 = new Finance { Type = FinanceType.Expense, Amount = 15000m, Description = "จ่ายค่าแรงช่างล่วงหน้า ทีม B (งานด่วน)", RecordedDate = DateTime.Now.AddDays(-5), ReferenceNo = "EXP-2002" };

        context.Finances.AddRange(fin1, fin2, fin3, fin4, fin5);
        context.SaveChanges();
    }

    private static void MigrateNewColumns(EpoxyDbContext context)
    {
        try
        {
            // Add UnitPrice column to Inventories table if not exists
            var hasInvPrice = context.Database.ExecuteSqlRaw(@"
                SELECT COUNT(*) FROM information_schema.columns 
                WHERE table_schema = DATABASE() 
                AND table_name = 'Inventories' 
                AND column_name = 'UnitPrice'");
            
            try
            {
                context.Database.ExecuteSqlRaw(
                    "ALTER TABLE `Inventories` ADD COLUMN `UnitPrice` DECIMAL(65,30) NOT NULL DEFAULT 0");
            }
            catch { /* Column already exists */ }

            // Add PaymentSlipUrl and approval columns to Quotations table if not exists
            try
            {
                context.Database.ExecuteSqlRaw(
                    "ALTER TABLE `Quotations` ADD COLUMN `PaymentSlipUrl` LONGTEXT NULL");
            }
            catch { /* Column already exists */ }

            try
            {
                context.Database.ExecuteSqlRaw(
                    "ALTER TABLE `Quotations` ADD COLUMN `ApprovedDate` DATETIME NULL");
            }
            catch { /* Column already exists */ }

            try
            {
                context.Database.ExecuteSqlRaw(
                    "ALTER TABLE `Quotations` ADD COLUMN `ApprovedBy` VARCHAR(255) NULL");
            }
            catch { /* Column already exists */ }

            try
            {
                context.Database.ExecuteSqlRaw(
                    "ALTER TABLE `Quotations` ADD COLUMN `PaymentNote` LONGTEXT NULL");
            }
            catch { /* Column already exists */ }

            try
            {
                context.Database.ExecuteSqlRaw(
                    "ALTER TABLE `InstallationProcesses` ADD COLUMN `FinalPaymentSlipUrl` LONGTEXT NULL");
            }
            catch { /* Column already exists */ }

            try
            {
                context.Database.ExecuteSqlRaw(
                    "ALTER TABLE `InstallationProcesses` ADD COLUMN `FinalPaymentNote` LONGTEXT NULL");
            }
            catch { /* Column already exists */ }

            try
            {
                context.Database.ExecuteSqlRaw(
                    "ALTER TABLE `InstallationProcesses` ADD COLUMN `FinalPaymentApprovedBy` VARCHAR(255) NULL");
            }
            catch { /* Column already exists */ }

            // Create InstallationStepDetails table if not exists
            try
            {
                context.Database.ExecuteSqlRaw(@"
                    CREATE TABLE IF NOT EXISTS `InstallationStepDetails` (
                        `Id` INT AUTO_INCREMENT PRIMARY KEY,
                        `InstallationProcessId` INT NOT NULL,
                        `Step` INT NOT NULL,
                        `Description` LONGTEXT NULL,
                        `PhotoUrl` LONGTEXT NULL,
                        CONSTRAINT `FK_InstallationStepDetails_InstallationProcesses` FOREIGN KEY (`InstallationProcessId`) REFERENCES `InstallationProcesses` (`Id`) ON DELETE CASCADE
                    )");
            }
            catch { /* Table already exists or creation failed */ }

            // Create InstallationStepMaterials table if not exists
            try
            {
                context.Database.ExecuteSqlRaw(@"
                    CREATE TABLE IF NOT EXISTS `InstallationStepMaterials` (
                        `Id` INT AUTO_INCREMENT PRIMARY KEY,
                        `InstallationStepDetailId` INT NOT NULL,
                        `InventoryId` INT NOT NULL,
                        `QuantityUsed` DECIMAL(65,30) NOT NULL,
                        CONSTRAINT `FK_InstallationStepMaterials_InstallationStepDetails` FOREIGN KEY (`InstallationStepDetailId`) REFERENCES `InstallationStepDetails` (`Id`) ON DELETE CASCADE,
                        CONSTRAINT `FK_InstallationStepMaterials_Inventories` FOREIGN KEY (`InventoryId`) REFERENCES `Inventories` (`Id`) ON DELETE RESTRICT
                    )");
            }
            catch { /* Table already exists or creation failed */ }

            // Create TechnicianAttendances table if not exists
            try
            {
                context.Database.ExecuteSqlRaw(@"
                    CREATE TABLE IF NOT EXISTS `TechnicianAttendances` (
                        `Id` INT AUTO_INCREMENT PRIMARY KEY,
                        `TechnicianId` INT NOT NULL,
                        `Date` DATETIME NOT NULL,
                        `Status` INT NOT NULL DEFAULT 0,
                        `Note` LONGTEXT NULL,
                        `RecordedBy` VARCHAR(255) NULL,
                        CONSTRAINT `FK_TechnicianAttendances_Technicians` FOREIGN KEY (`TechnicianId`) REFERENCES `Technicians` (`Id`) ON DELETE CASCADE
                    )");
            }
            catch { /* Table already exists or creation failed */ }

            // Create CompanyProfiles table if not exists
            try
            {
                context.Database.ExecuteSqlRaw(@"
                    CREATE TABLE IF NOT EXISTS `CompanyProfiles` (
                        `Id` INT AUTO_INCREMENT PRIMARY KEY,
                        `Name` VARCHAR(255) NOT NULL,
                        `Address` LONGTEXT NOT NULL,
                        `Phone` VARCHAR(100) NOT NULL,
                        `Email` VARCHAR(100) NOT NULL,
                        `TaxId` VARCHAR(100) NOT NULL
                    )");
            }
            catch { /* Table already exists or creation failed */ }
        }
        catch
        {
            // Ignore migration errors - columns may already exist
        }
    }
}
