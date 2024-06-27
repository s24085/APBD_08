using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EX8.Models
{
    public partial class TripsDbContext : DbContext
    {
        public TripsDbContext()
        {
        }

        public TripsDbContext(DbContextOptions<TripsDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Client> Clients { get; set; }

        public virtual DbSet<ClientTrip> ClientTrips { get; set; }

        public virtual DbSet<Country> Countries { get; set; }

        public virtual DbSet<Trip> Trips { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
            => optionsBuilder.UseSqlServer("Server=localhost,1433;Database=TripsDB;User Id=SA;Password=StrongPass123;TrustServerCertificate=True;");

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>(entity =>
            {
                entity.HasKey(e => e.IdClient).HasName("Client_pk");

                entity.ToTable("Client");

                entity.Property(e => e.Email).HasMaxLength(120);
                entity.Property(e => e.FirstName).HasMaxLength(120);
                entity.Property(e => e.LastName).HasMaxLength(120);
                entity.Property(e => e.Pesel).HasMaxLength(120);
                entity.Property(e => e.Telephone).HasMaxLength(120);
            });

            modelBuilder.Entity<ClientTrip>(entity =>
            {
                entity.HasKey(e => new { e.IdClient, e.IdTrip }).HasName("Client_Trip_pk");

                entity.ToTable("Client_Trip");

                entity.Property(e => e.PaymentDate).HasColumnType("datetime");
                entity.Property(e => e.RegisteredAt).HasColumnType("datetime");

                entity.HasOne(d => d.IdClientNavigation).WithMany(p => p.ClientTrips)
                    .HasForeignKey(d => d.IdClient)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Table_5_Client");

                entity.HasOne(d => d.IdTripNavigation).WithMany(p => p.ClientTrips)
                    .HasForeignKey(d => d.IdTrip)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Table_5_Trip");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.HasKey(e => e.IdCountry).HasName("Country_pk");

                entity.ToTable("Country");

                entity.Property(e => e.Name).HasMaxLength(120);

                entity.HasMany(d => d.IdTrips).WithMany(p => p.IdCountries)
                    .UsingEntity<Dictionary<string, object>>(
                        "CountryTrip",
                        r => r.HasOne<Trip>().WithMany()
                            .HasForeignKey("IdTrip")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("Country_Trip_Trip"),
                        l => l.HasOne<Country>().WithMany()
                            .HasForeignKey("IdCountry")
                            .OnDelete(DeleteBehavior.ClientSetNull)
                            .HasConstraintName("Country_Trip_Country"),
                        j =>
                        {
                            j.HasKey("IdCountry", "IdTrip").HasName("Country_Trip_pk");
                            j.ToTable("Country_Trip");
                        });
            });

            modelBuilder.Entity<Trip>(entity =>
            {
                entity.HasKey(e => e.IdTrip).HasName("Trip_pk");

                entity.ToTable("Trip");

                entity.Property(e => e.DateFrom).HasColumnType("datetime");
                entity.Property(e => e.DateTo).HasColumnType("datetime");
                entity.Property(e => e.Description).HasMaxLength(220);
                entity.Property(e => e.Name).HasMaxLength(120);
            });

            // Seed data
            modelBuilder.Entity<Client>().HasData(
                new Client { IdClient = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com", Telephone = "123-456-7890", Pesel = "12345678901" },
                new Client { IdClient = 2, FirstName = "Jane", LastName = "Doe", Email = "jane.doe@example.com", Telephone = "098-765-4321", Pesel = "23456789012" }
            );

            modelBuilder.Entity<Trip>().HasData(
                new Trip { IdTrip = 1, Name = "Trip to Rome", Description = "A wonderful trip to Rome.", DateFrom = new DateTime(2023, 7, 1), DateTo = new DateTime(2023, 7, 7), MaxPeople = 20 },
                new Trip { IdTrip = 2, Name = "Trip to Paris", Description = "A romantic trip to Paris.", DateFrom = new DateTime(2023, 8, 1), DateTo = new DateTime(2023, 8, 7), MaxPeople = 15 }
            );

            modelBuilder.Entity<ClientTrip>().HasData(
                new ClientTrip { IdClient = 1, IdTrip = 1, RegisteredAt = new DateTime(2023, 6, 1), PaymentDate = null },
                new ClientTrip { IdClient = 2, IdTrip = 2, RegisteredAt = new DateTime(2023, 6, 1), PaymentDate = new DateTime(2023, 6, 15) }
            );
        }
    }
}
