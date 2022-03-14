using AppLogic.FileUploadService;
using aspnet_core;
using aspnet_core.Hubs;
using MediatR;

static CancellationToken cancellationToken ()
{
  return new CancellationTokenSource (TimeSpan.FromMinutes (0.5)).Token;
}
var builder = WebApplication.CreateBuilder (args);
builder.Services.RegisterServices (builder.Configuration);

var app = builder.Build ();
app.UseSwaggerGen ();
app.UseCors ("Default");
app.UseCors ("AllowAll");
app.MapHub<MainHub> ("/hub");
app.MapGet ("/files",
  async ( IMediator mediator, ILogger<Program> logger ) =>
{
  try
  {
    var response = await mediator.Send (new GetFilesRequest (),
        cancellationToken ());
    return Results.Ok (response);
  }
  catch ( Exception exc )
  {
    logger.LogError (exc, "{Message}", exc.Message);
    return Results.StatusCode (500);
  }
});

app.MapGet ("/lastUploadedRows",
  async ( IMediator mediator, ILogger<Program> logger ) =>
  {
    try
    {
      var response = await mediator.Send (new GetLastUploadedRowsRequest (),
          cancellationToken ());
      return Results.Ok (response);
    }
    catch ( Exception exc )
    {
      logger.LogError (exc, "{Message}", exc.Message);
      return Results.StatusCode (500);
    }
  });

app.MapPost ("/upload",
 async ( ILogger<Program> Logger,IMediator mediator, HttpRequest request ) =>
  {


    try
    {
      var form = await request.ReadFormAsync();
      var formFile = form.Files[0];
      var response = await mediator.Send(new UploadFileRequest(formFile), cancellationToken());
      return Results.Json(response);
    }
    catch (ArgumentException exc)
    {
      Logger.LogError (exc, "{Message}", exc.Message);
      return Results.BadRequest(exc.Message);
    }
    catch ( Exception exc )
    {
      Logger.LogError (exc, "{Message}", exc.Message);
      return Results.StatusCode (500);
    }
  });

app.UseRouting ();

app.Run ();
