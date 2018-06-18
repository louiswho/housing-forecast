using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;

namespace Housing.Forecast.Service.Controllers
{
  [Route("api/[controller]")]
  public class ForecastController : BaseController
  {
    public ForecastController(ILoggerFactory loggerFactory, IQueueClient queueClientSingleton)
      : base(loggerFactory, queueClientSingleton) {}
    
    public async Task<IActionResult> Get()
    {
      return await Task.Run(() => Ok());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
      return await Task.Run(() => Ok());
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody]object value)
    {
      return await Task.Run(() => Ok());
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody]object value)
    {
      return await Task.Run(() => Ok());
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      return await Task.Run(() => Ok());
    }

    protected override void UseReceiver()
    {
      var messageHandlerOptions = new MessageHandlerOptions(ReceiverExceptionHandler)
      {
        AutoComplete = false
      };

      queueClient.RegisterMessageHandler(ReceiverMessageProcessAsync, messageHandlerOptions);
    }
    
    protected override void UseSender(Message message)
    {
      Task.Run(() =>
        SenderMessageProcessAsync(message)
      );
    }
  }
}
