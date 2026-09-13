using DomainClassOld;
using Microsoft.EntityFrameworkCore;
using System.Xml;

namespace DAL.Context
{
    public class MatabDBContextOld : DbContext
    {
        public string ConnectionString { get; set; }
        public static string ServerName { get; set; }
        public static string HostName { get; set; }
        public static string Port { get; set; }
        public DbSet<TblUser> TblUser { get; set; }
        public DbSet<TblPatient> TblPatient { get; set; }
        public DbSet<TblVisit> TblVisit { get; set; }
        public DbSet<TblSetup> TblSetup { get; set; }
        public DbSet<TblDocumentType> TblDocumentType { get; set; }
        public DbSet<TblDocument> TblDocument { get; set; }
        public DbSet<TblSicknessType> TblSicknessType { get; set; }
        public DbSet<TblSubSicknessType> TblSubSicknessType { get; set; }
        public DbSet<TblTreatmentPlace> TblTreatmentPlace { get; set; }
        public DbSet<TblHelpDoctorSecretary> TblHelpDoctorSecretary { get; set; }
        public DbSet<TblTashkhis> TblTashkhis { get; set; }
        public MatabDBContextOld()
        {
            ConnectionString = "Server=.;Database=DBMatab;Trusted_Connection=True;MultipleActiveResultSets=True;Max Pool Size=200;;TrustServerCertificate=True";
            //Port = "55662";
            //HostName = "localhost";
            //ServerName = Properties.Settings.Default.ServerIP;
            ////ConnectionString = "Data Source=" + ServerName + ";Database=DBMatab; User Id=sc; Password=147computer369";
            // ServerName = ".\\sqlexpress";
            //ConnectionString = "Data Source=" + ServerName + ";Database=DBMatab; User Id=sc; Password=147computer369";

            //دکتر خاندوزی
            //ServerName = "KHANDOZI-PC";
            // ConnectionString = "Data Source=" + ServerName + ";Database=DBMatab; User Id=sc; Password=khandozi147!$&";

            //کانکشن خانم دکتر خدابخشی
            //ServerName = "server-pc";
            //ConnectionString = "Data Source=" + ServerName + ";Database=DBMatab; User Id=sc; Password=147computer369";
            //ServerName = "milad-pc\\SQL2012";
            //ConnectionString = "Data Source=" + ServerName + ";Database=DBMatab; User Id=sc; Password=147computer369";
            //سرور مرکز میرداماد
            //ServerName = "192.168.1.62";
            //ConnectionString = "Data Source=" + ServerName + ";Database=DBMatab; User Id=sc; Password=147computer369";
            //کانکشن خانم دکتر اکبری
            //ServerName = "server-akbari";
            //ServerName = "2.179.254.136";
            //ConnectionString = "Data Source=" + ServerName + ";Database=DBMatab; User Id=sc; Password=147computer369;TrustServerCertificate=True;";
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder
                    .UseSqlServer(ConnectionString)
                    .UseLazyLoadingProxies();
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");

            //////////////////////////////////////////////////////////////////////////////
            ////////////////////////////   TblUser    ////////////////////////////////////


            modelBuilder.Entity<TblUser>()
                .HasIndex(table => new
                {
                    table.UserName
                })
                .IsUnique();

            modelBuilder.Entity<TblUser>()
               .Property(b => b.Unique)
               .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<TblUser>()
               .Property(b => b.CreationTime)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<TblUser>()
             .Property(b => b.ModifiedDate)
              .HasDefaultValueSql("GETDATE()");
            //////////////////////////////////////////////////////////////////////////////
            ////////////////////////////   TblPatient    ////////////////////////////////////

            modelBuilder.Entity<TblPatient>()
               .Property(b => b.Unique)
               .HasDefaultValueSql("NEWID()");

            modelBuilder.Entity<TblPatient>()
               .Property(b => b.CreationTime)
                .HasDefaultValueSql("GETDATE()");

            modelBuilder.Entity<TblPatient>()
             .Property(b => b.ModifiedDate)
              .HasDefaultValueSql("GETDATE()");
            ////////////////////////////////////////////////////////////////////////////
            //////////////////////////   TblVisit    ////////////////////////////////////

            modelBuilder.Entity<TblVisit>()
            .HasOne(b => b.TblPatient)
            .WithMany(a => a.TblVisits)
            .OnDelete(DeleteBehavior.Cascade);


            ////////////////////////////////////////////////////////////////////////////
            //////////////////////////   TblSetup   ////////////////////////////////////
            modelBuilder.Entity<TblSetup>()
            .HasOne(b => b.TblPatient)
            .WithMany(a => a.TblSetups)
            .OnDelete(DeleteBehavior.Cascade);
            ////////////////////////////////////////////////////////////////////////////
            //////////////////////////   TblDocument   ////////////////////////////////////
            modelBuilder.Entity<TblDocument>()
            .HasOne(b => b.TblPatient)
            .WithMany(a => a.TblDocuments)
            .OnDelete(DeleteBehavior.Cascade);
        }
    }
}