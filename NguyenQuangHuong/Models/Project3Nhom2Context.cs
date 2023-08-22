using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NguyenQuangHuong.Models;

public partial class Project3Nhom2Context : DbContext
{
    public Project3Nhom2Context()
    {
    }

    public Project3Nhom2Context(DbContextOptions<Project3Nhom2Context> options)
        : base(options)
    {
    }

    public virtual DbSet<AccountsUser> AccountsUsers { get; set; }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<Business> Businesses { get; set; }

    public virtual DbSet<Chat> Chats { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Recruitment> Recruitments { get; set; }

    public virtual DbSet<Region> Regions { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<ServiceBooking> ServiceBookings { get; set; }

    public virtual DbSet<Servicess> Servicesses { get; set; }

    public virtual DbSet<Testimonial> Testimonials { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=DESKTOP-7FEDEVV\\SQLEXPRESS;Initial Catalog=project3_nhom2;\nIntegrated Security=True;TrustServerCertificate=True;\n");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AccountsUser>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.Avata)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Birthday).HasColumnType("date");
            entity.Property(e => e.CreataDate).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.PassWord)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(e => e.BranchId).HasName("PK__Branches__12CEB0412187D495");

            entity.Property(e => e.BranchId).HasColumnName("Branch_ID");
            entity.Property(e => e.BranchName)
                .HasMaxLength(50)
                .HasColumnName("Branch_Name");
            entity.Property(e => e.RegionId).HasColumnName("Region_ID");

            entity.HasOne(d => d.Region).WithMany(p => p.Branches)
                .HasForeignKey(d => d.RegionId)
                .HasConstraintName("FK__Branches__Region__5441852A");
        });

        modelBuilder.Entity<Business>(entity =>
        {
            entity.ToTable("Business");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.BusinesName)
                .HasMaxLength(100)
                .HasColumnName("busines_Name");
            entity.Property(e => e.Thumbnail)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("thumbnail");
        });

        modelBuilder.Entity<Chat>(entity =>
        {
            entity.ToTable("Chat");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Datetime).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");

            entity.HasOne(d => d.User).WithMany(p => p.Chats)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_htk_id_user");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.DepartmentId).HasName("PK__Departme__151675D118580C95");

            entity.Property(e => e.DepartmentId).HasColumnName("Department_ID");
            entity.Property(e => e.DepartmentName)
                .HasMaxLength(100)
                .HasColumnName("Department_Name");
            entity.Property(e => e.Location).HasMaxLength(100);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__Accounts__206D9190F6D2666E");

            entity.Property(e => e.UserId).HasColumnName("User_ID");
            entity.Property(e => e.Achievements).HasColumnType("text");
            entity.Property(e => e.Address).HasMaxLength(100);
            entity.Property(e => e.Avata)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Birthday)
                .HasColumnType("date")
                .HasColumnName("birthday");
            entity.Property(e => e.BranchId).HasColumnName("BranchID");
            entity.Property(e => e.CreateDate).HasColumnType("datetime");
            entity.Property(e => e.DepartmentId).HasColumnName("DepartmentID");
            entity.Property(e => e.EducationalQualification)
                .HasMaxLength(50)
                .HasColumnName("Educational_Qualification");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FullName).HasMaxLength(40);
            entity.Property(e => e.LastLogin).HasColumnType("datetime");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Branch).WithMany(p => p.Employees)
                .HasForeignKey(d => d.BranchId)
                .HasConstraintName("fk_ID_Branch");

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("fk_id_DepartmentID");

            entity.HasOne(d => d.Role).WithMany(p => p.Employees)
                .HasForeignKey(d => d.RoleId)
                .HasConstraintName("fk_ID_Role");
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__News__3214EC27B21A361A");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.EmployessId).HasColumnName("EmployessID");
            entity.Property(e => e.ImageUrl)
                .HasMaxLength(200)
                .HasColumnName("ImageURL");
            entity.Property(e => e.PublishedDate).HasColumnType("datetime");
            entity.Property(e => e.Status).HasDefaultValueSql("((0))");
            entity.Property(e => e.Title).HasMaxLength(200);

            entity.HasOne(d => d.Employess).WithMany(p => p.News)
                .HasForeignKey(d => d.EmployessId)
                .HasConstraintName("FK_ID_News");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AccountId).HasColumnName("AccountID");
            entity.Property(e => e.PaymentDate).HasColumnType("datetime");

            entity.HasOne(d => d.Account).WithMany(p => p.Orders)
                .HasForeignKey(d => d.AccountId)
                .HasConstraintName("fk_id_AccountUser");
        });

        modelBuilder.Entity<Recruitment>(entity =>
        {
            entity.HasKey(e => e.RecruitmentId).HasName("PK__Recruitm__A8F5203E83410792");

            entity.ToTable("Recruitment");

            entity.Property(e => e.RecruitmentId).HasColumnName("Recruitment_ID");
            entity.Property(e => e.Avata)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Birthday).HasColumnType("date");
            entity.Property(e => e.DepartmentId).HasColumnName("Department_ID");
            entity.Property(e => e.EducationalQualification)
                .HasMaxLength(100)
                .HasColumnName("Educational_Qualification");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EmployeeAddress)
                .HasMaxLength(100)
                .HasColumnName("Employee_Address");
            entity.Property(e => e.EmployeeName)
                .HasMaxLength(100)
                .HasColumnName("Employee_Name");
            entity.Property(e => e.Phone)
                .HasMaxLength(10)
                .IsUnicode(false);

            entity.HasOne(d => d.Department).WithMany(p => p.Recruitments)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("fk_ID_Department_ID");
        });

        modelBuilder.Entity<Region>(entity =>
        {
            entity.HasKey(e => e.RegionId).HasName("PK__Regions__A9EAD51F70DE84B7");

            entity.Property(e => e.RegionId).HasColumnName("Region_ID");
            entity.Property(e => e.RegionName)
                .HasMaxLength(50)
                .HasColumnName("Region_Name");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<ServiceBooking>(entity =>
        {
            entity.HasKey(e => e.BookingId).HasName("PK__ServiceB__35ABFDE0F0066011");

            entity.Property(e => e.BookingId).HasColumnName("Booking_ID");
            entity.Property(e => e.BookingDate)
                .HasColumnType("date")
                .HasColumnName("Booking_Date");
            entity.Property(e => e.BookingStatus).HasColumnName("Booking_Status");
            entity.Property(e => e.Confirm).HasColumnName("confirm");
            entity.Property(e => e.EndBookingDate)
                .HasColumnType("date")
                .HasColumnName("EndBooking_Date");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.Price).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.ServiceId).HasColumnName("Service_ID");
            entity.Property(e => e.Status).HasDefaultValueSql("((0))");

            entity.HasOne(d => d.Order).WithMany(p => p.ServiceBookings)
                .HasForeignKey(d => d.OrderId)
                .HasConstraintName("fk_id_ServiceBookings");

            entity.HasOne(d => d.Service).WithMany(p => p.ServiceBookings)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK__ServiceBo__Servi__693CA210");
        });

        modelBuilder.Entity<Servicess>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__Services__BD1A239CE10E1FDA");

            entity.ToTable("Servicess");

            entity.Property(e => e.ServiceId).HasColumnName("Service_ID");
            entity.Property(e => e.Avata)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.BusinesId).HasColumnName("businesID");
            entity.Property(e => e.Numberofprople).HasColumnName("numberofprople");
            entity.Property(e => e.Package).HasColumnName("package");
            entity.Property(e => e.ServiceName)
                .HasMaxLength(100)
                .HasColumnName("Service_Name");
            entity.Property(e => e.ServicePrice)
                .HasColumnType("decimal(10, 2)")
                .HasColumnName("Service_Price");

            entity.HasOne(d => d.Busines).WithMany(p => p.Servicesses)
                .HasForeignKey(d => d.BusinesId)
                .HasConstraintName("fk_htk_id_Services");
        });

        modelBuilder.Entity<Testimonial>(entity =>
        {
            entity.HasKey(e => e.TestimonialId).HasName("PK__Testimon__D2FCAE2B87169657");

            entity.Property(e => e.TestimonialId).HasColumnName("Testimonial_ID");
            entity.Property(e => e.AccountsUserId).HasColumnName("AccountsUserID");
            entity.Property(e => e.ServiceId).HasColumnName("Service_ID");
            entity.Property(e => e.TestimonialText).HasColumnName("Testimonial_Text");

            entity.HasOne(d => d.AccountsUser).WithMany(p => p.Testimonials)
                .HasForeignKey(d => d.AccountsUserId)
                .HasConstraintName("fk_id_Testimonials");

            entity.HasOne(d => d.Service).WithMany(p => p.Testimonials)
                .HasForeignKey(d => d.ServiceId)
                .HasConstraintName("FK_service_ID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
