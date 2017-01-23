using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FIDO.Datasources.FIDO.Support.ErrorHandling;
using FIDO.Datasources.FIDO.Support.Event.Queue;
using FIDO.Datasources.FIDO.Support.FidoDB;
using FIDO.Datasources.FIDO.Support.RabbitMQ;

namespace FIDO.DataSources.ActiveDirectory
{
  static class GetActiveDirectory
  {
    private static void Main(string[] args)
    {
      GetActiveDirectoryQueue();
    }

    private static void GetActiveDirectoryQueue()
    {
      try
      {
        while (true)
        {
          var lFidoReturnValues = GetRabbit.ReceiveNotificationQueue(EventQueue.PrimaryConfig.host, EventQueue.PrimaryConfig.datasources.inventory.activedirectory.queue, GetRabbitEnum.AD);
          if (lFidoReturnValues?.UserInfo == null) continue;

          var postmsg = new PostRabbit();
          postmsg.SendToRabbit(lFidoReturnValues.TimeOccured, lFidoReturnValues.UUID, EventQueue.PrimaryConfig.datasources.inventory.carbonblack.exchange, EventQueue.PrimaryConfig.datasources.inventory.carbonblack.queue, EventQueue.PrimaryConfig.host, EventQueue.PrimaryConfig);

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
