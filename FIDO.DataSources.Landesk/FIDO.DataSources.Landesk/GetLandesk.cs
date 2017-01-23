using System;
using FIDO.Datasources.FIDO.Support.ErrorHandling;
using FIDO.Datasources.FIDO.Support.Event.Queue;
using FIDO.Datasources.FIDO.Support.FidoDB;
using FIDO.Datasources.FIDO.Support.RabbitMQ;

namespace FIDO.DataSources.Landesk
{
  static class GetLandesk
  {
    private static void Main(string[] args)
    {
      GetLandeskQueue();
    }

    private static void GetLandeskQueue()
    {
      try
      {
        while (true)
        {
          var lFidoReturnValues = GetRabbit.ReceiveNotificationQueue(EventQueue.PrimaryConfig.host, EventQueue.PrimaryConfig.datasources.inventory.landesk.queue, GetRabbitEnum.Landesk);
          if (lFidoReturnValues?.Inventory?.Landesk == null) continue;

          var postmsg = new PostRabbit();
          postmsg.SendToRabbit(lFidoReturnValues.TimeOccured, lFidoReturnValues.UUID, EventQueue.PrimaryConfig.datasources.inventory.jamf.exchange, EventQueue.PrimaryConfig.datasources.inventory.jamf.queue, EventQueue.PrimaryConfig.host, EventQueue.PrimaryConfig);

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
