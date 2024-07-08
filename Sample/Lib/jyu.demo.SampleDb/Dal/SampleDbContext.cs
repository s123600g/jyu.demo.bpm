using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using jyu.demo.SampleDb.Dao;

namespace jyu.demo.SampleDb.Dal;

public partial class SampleDbContext : DbContext
{
    public SampleDbContext()
    {
    }

    public SampleDbContext(DbContextOptions<SampleDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ProductApplicationHistory> ProductApplicationHistories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
