using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DoctorAppointment1.Models;

public partial class DoctorContext : DbContext
{
    public DoctorContext()
    {
    }

    public DoctorContext(DbContextOptions<DoctorContext> options)
        : base(options)
    {
    }
    public DbSet<User> Users { get; set; }

    // Constructor to configure the database connection (e.g., SQL Server)
  

    public virtual DbSet<Appointment> Appointments { get; set; }

    public virtual DbSet<Disease> Diseases { get; set; }

    public virtual DbSet<Doctor> Doctors { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=YBANALA-L-5498\\SQLEXPRESS;Initial Catalog=Doctor;Persist Security Info=True;User ID=sa;Password=Welcome2evoke@1234; Trust Server Certificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(e => e.AppointmentId).HasName("PK__Appointm__8ECDFCA25EB31ED2");

            entity.Property(e => e.AppointmentId).HasColumnName("AppointmentID");
            entity.Property(e => e.AppointmentStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AppointmentTime).HasColumnType("datetime");
            entity.Property(e => e.DoctorToVisit)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.MedicalIssue)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PatientEmail)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.PatientName)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.DoctorToVisitNavigation).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.DoctorToVisit)
                .HasConstraintName("FK__Appointme__Docto__7D439ABD");

            entity.HasOne(d => d.MedicalIssueNavigation).WithMany(p => p.Appointments)
                .HasForeignKey(d => d.MedicalIssue)
                .HasConstraintName("FK__Appointme__Medic__7C4F7684");
        });

        modelBuilder.Entity<Disease>(entity =>
        {
            entity.HasKey(e => e.DiseaseName).HasName("PK__Diseases__5112584C48F688DA");

            entity.Property(e => e.DiseaseName)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.DiseaseId).HasColumnName("DiseaseID");
            entity.Property(e => e.SuitableDoctor)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.SuitableDoctorNavigation).WithMany(p => p.Diseases)
                .HasForeignKey(d => d.SuitableDoctor)
                .HasConstraintName("FK__Diseases__Suitab__797309D9");
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(e => e.Name).HasName("PK__Doctors__737584F725513EDD");

            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Address)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.DoctorId).HasColumnName("DoctorID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Qualification)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Specialized)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
