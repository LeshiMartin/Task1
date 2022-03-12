using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL;

public class CoreDbContext : DbContext
{
  public CoreDbContext ( DbContextOptions<CoreDbContext> options ) : base (options)
  { }

  protected override void OnModelCreating ( ModelBuilder modelBuilder )
  {
    modelBuilder.ApplyConfigurationsFromAssembly (typeof (CoreDbContext).Assembly);
    base.OnModelCreating (modelBuilder);
  }
  public DbSet<UploadedFile>? UploadedFiles { get; set; }
  public DbSet<FileRow>? FileRows { get; set; }
}