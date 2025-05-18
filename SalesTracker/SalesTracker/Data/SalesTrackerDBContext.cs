using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SalesTracker.Models;

namespace SalesTracker.Data
{
    public class SalesTrackerDBContext : DbContext
    {
        public SalesTrackerDBContext()
        {
        }

        public SalesTrackerDBContext(DbContextOptions<SalesTrackerDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Edition> Editions { get; set; } = null!;
        public virtual DbSet<EditionETL> EditionsETL { get; set; } = null!;
        public virtual DbSet<SaleType> SaleTypes { get; set; } = null!;
        public virtual DbSet<PageHits> PageHits { get; set; } = null!;
        public virtual DbSet<SaleTypeETL> SaleTypesETL { get; set; } = null!;

        public virtual DbSet<ApiModel> ApiModel { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApiModel>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<EditionETL>(entity =>
            {
                entity.HasNoKey();
            });

            modelBuilder.Entity<SaleTypeETL>(entity =>
            {
                entity.HasNoKey();
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
