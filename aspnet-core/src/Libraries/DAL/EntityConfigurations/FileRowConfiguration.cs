using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DAL.EntityConfigurations;
internal class FileRowConfiguration:IEntityTypeConfiguration<FileRow>
{
  public void Configure(EntityTypeBuilder<FileRow> builder)
  {
    builder
      .Property(x => x.Color)
      .IsRequired()
      .HasMaxLength(125);
    builder
      .Property(x => x.Label)
      .IsRequired()
      .HasMaxLength(512);
    builder
      .Property(x => x.Number)
      .IsRequired();
    builder
      .HasOne(x => x.UploadedFile)
      .WithMany(x => x.FileRows)
      .HasForeignKey(x => x.UploadedFileId);

  }
}
