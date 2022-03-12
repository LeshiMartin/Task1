using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.EntityConfigurations;

internal class UploadedFileConfiguration : IEntityTypeConfiguration<UploadedFile>
{
  public void Configure ( EntityTypeBuilder<UploadedFile> builder )
  {
    builder
      .Property (x => x.FileName)
      .IsRequired ()
      .HasMaxLength (250);
    builder
      .Property (x => x.FileUri)
      .IsRequired ()
      .HasMaxLength (1500);
    builder
      .HasMany (x => x.FileRows)
      .WithOne (x => x.UploadedFile)
      .HasForeignKey (x => x.UploadedFileId);
  }
}