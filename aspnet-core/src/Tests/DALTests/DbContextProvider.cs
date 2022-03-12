using System.Linq;
using DAL;
using Microsoft.EntityFrameworkCore;

namespace DALTests;
internal class DbContextProvider
{
  internal static CoreDbContext GetDbContext ()
  {
    var options = new DbContextOptionsBuilder<CoreDbContext> ()
      .UseInMemoryDatabase (databaseName: "Test")
      .Options;

    var dbContext = new CoreDbContext (options);
    if ( dbContext.FileRows!.Any () )
      dbContext.FileRows!.RemoveRange (dbContext.FileRows);
    if ( dbContext.UploadedFiles!.Any () )
      dbContext.UploadedFiles!.RemoveRange (dbContext.UploadedFiles);
    dbContext.SaveChanges ();
    return dbContext;
  }
}
