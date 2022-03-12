using AppLogic.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace aspnet_core.Hubs;

public class MainHub:Hub,INotifyFileInProcess,INotifyFileIsNotValid,INotifyFileIsProcessed
{
  private readonly IHubContext<MainHub> _hubContext;

  public MainHub(IHubContext<MainHub> hubContext)
  {
    _hubContext = hubContext;
  }
  public async Task FileIsInProcess(int id)
  {
    await _hubContext.Clients.All.SendAsync("fileIsInProcess", id);
  }

  public async Task FileIsNotValid(int id)
  {
    await _hubContext.Clients.All.SendAsync ("fileIsNotValid", id);
  }

  public async Task FileIsProcessed(int id)
  {
    await _hubContext.Clients.All.SendAsync("fileIsProcessed", id);
  }
}