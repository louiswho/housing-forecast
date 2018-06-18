using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.ServiceBus;
using Microsoft.Extensions.Logging;

namespace Housing.Forecast.Service.Controllers
{
  public abstract class BaseController : Controller
  {
    protected readonly ILogger logger;
    protected readonly IQueueClient queueClient;
    protected abstract void UseReceiver();
    protected abstract void UseSender(Message message);

    protected BaseController(ILoggerFactory loggerFactory, IQueueClient queueClientSingleton)
    {
      logger = loggerFactory.CreateLogger(this.GetType().Name);
      queueClient = queueClientSingleton;
    }

    protected virtual async Task ReceiverExceptionHandler(ExceptionReceivedEventArgs exceptionReceivedEventArgs)
    {
      logger.LogError(exceptionReceivedEventArgs.Exception.ToString());
      await Task.CompletedTask;
    }

    protected virtual async Task ReceiverMessageProcessAsync(Message message, CancellationToken cancellationToken)
    {
      if (null == message || cancellationToken.IsCancellationRequested)
      {
        return;
      }

      if (message.SystemProperties.IsLockTokenSet)
      {
        logger.LogInformation($"{message.MessageId}\n{Encoding.UTF8.GetString(message.Body)}");
        await queueClient.CompleteAsync(message.SystemProperties.LockToken);
      }
    }
    
    protected virtual async Task SenderMessageProcessAsync(Message message)
    {
      logger.LogInformation(message.MessageId);
      await queueClient.SendAsync(message);
    }
  }
}
