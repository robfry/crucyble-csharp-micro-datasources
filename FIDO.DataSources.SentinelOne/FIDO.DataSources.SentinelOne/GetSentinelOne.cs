using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FIDO.Datasources.FIDO.Support.ErrorHandling;
using FIDO.Datasources.FIDO.Support.Event.Queue;
using FIDO.Datasources.FIDO.Support.FidoDB;
using FIDO.Datasources.FIDO.Support.RabbitMQ;

namespace FIDO.DataSources.SentinelOne
{
  static class GetSentinelOne
  {
    private static void Main(string[] args)
    {
      GetSentinelOneQueue();
    }

    private static void GetSentinelOneQueue()
    {
      try
      {
        while (true)
        {
          var lFidoReturnValues = GetRabbit.ReceiveNotificationQueue(EventQueue.PrimaryConfig.host, EventQueue.PrimaryConfig.datasources.inventory.sentinelone.queue, GetRabbitEnum.S1);
          if (lFidoReturnValues?.Inventory?.SentRunning == null) continue;

          var writeCouch = new Fido_CouchDB();
          writeCouch.WriteToDBFactory(lFidoReturnValues);
          GC.Collect();
        }
      }
      catch (Exception e)
      {
        Fido_EventHandler.SendEmail("Fido Error", "Fido Failed: {0} Exception caught gathering rabbitmq events:" + e);
      }

    }
  }
}
