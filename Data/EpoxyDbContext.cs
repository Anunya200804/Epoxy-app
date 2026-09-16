using Microsoft.EntityFrameworkCore;
using EpoxyFloorManager.Models;

namespace EpoxyFloorManager.Data;

public class EpoxyDbContext : DbContext
{
    public EpoxyDbContext(DbContextOptions<EpoxyDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Technician> Technicians => Set<Technician>();
    public DbSet<TechnicianTeam> TechnicianTeams => Set<TechnicianTeam>();
    public DbSet<Inventory> Inventories => Set<Inventory>();
    public DbSet<Quotation> Quotations => Set<Quotation>();
    public DbSet<StockIn> StockIns => Set<StockIn>();
    public DbSet<InstallationProcess> InstallationProcesses => Set<InstallationProcess>();
    public DbSet<Finance> Finances => Set<Finance>();
    public DbSet<InstallationStepDetail> InstallationStepDetails => Set<InstallationStepDetail>();
    public DbSet<InstallationStepMaterial> InstallationStepMaterials => Set<InstallationStepMaterial>();
    public DbSet<TechnicianAttendance> TechnicianAttendances => Set<TechnicianAttendance>();
    public DbSet<CompanyProfile> CompanyProfiles => Set<CompanyProfile>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure InstallationStepDetail relationships
        modelBuilder.Entity<InstallationStepDetail>()
            .HasOne(d => d.InstallationProcess)
            .WithMany()
            .HasForeignKey(d => d.InstallationProcessId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure InstallationStepMaterial relationships
        modelBuilder.Entity<InstallationStepMaterial>()
            .HasOne(m => m.InstallationStepDetail)
            .WithMany(d => d.UsedMaterials)
            .HasForeignKey(m => m.InstallationStepDetailId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<InstallationStepMaterial>()
            .HasOne(m => m.Inventory)
            .WithMany()
            .HasForeignKey(m => m.InventoryId)
            .OnDelete(DeleteBehavior.Restrict);
            
        // Configure Technician - TechnicianTeam relationship (One-to-Many)
        modelBuilder.Entity<Technician>()
            .HasOne(t => t.Team)
            .WithMany(team => team.Technicians)
            .HasForeignKey(t => t.TeamId)
            .OnDelete(DeleteBehavior.SetNull);

        // Configure Quotation - Customer relationship
        modelBuilder.Entity<Quotation>()
            .HasOne(q => q.Customer)
            .WithMany()
            .HasForeignKey(q => q.CustomerId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure StockIn - Inventory relationship
        modelBuilder.Entity<StockIn>()
            .HasOne(s => s.Inventory)
            .WithMany()
            .HasForeignKey(s => s.InventoryId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure InstallationProcess - Quotation relationship
        modelBuilder.Entity<InstallationProcess>()
            .HasOne(p => p.Quotation)
            .WithMany()
            .HasForeignKey(p => p.QuotationId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure InstallationProcess - TechnicianTeam relationship
        modelBuilder.Entity<InstallationProcess>()
            .HasOne(p => p.TechnicianTeam)
            .WithMany()
            .HasForeignKey(p => p.TechnicianTeamId)
            .OnDelete(DeleteBehavior.SetNull);

        // Configure Finance - Quotation relationship
        modelBuilder.Entity<Finance>()
            .HasOne(f => f.Quotation)
            .WithMany()
            .HasForeignKey(f => f.QuotationId)
            .OnDelete(DeleteBehavior.SetNull);

        // Configure TechnicianAttendance - Technician relationship
        modelBuilder.Entity<TechnicianAttendance>()
            .HasOne(a => a.Technician)
            .WithMany()
            .HasForeignKey(a => a.TechnicianId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
