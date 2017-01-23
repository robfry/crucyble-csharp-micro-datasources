using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FIDO.Datasources.FIDO.Support.ErrorHandling;
using FIDO.Datasources.FIDO.Support.Event.Queue;
using FIDO.Datasources.FIDO.Support.FidoDB;
using FIDO.Datasources.FIDO.Support.RabbitMQ;

namespace FIDO.DataSources.CarbonBlack
{
  static class GetCarbonBlack
  {
    private static void Main(string[] args)
    {
      GetCarbonBlackQueue();
    }

    private static void GetCarbonBlackQueue()
    {
      try
      {
        while (true)
        {
          var lFidoReturnValues = GetRabbit.ReceiveNotificationQueue(EventQueue.PrimaryConfig.host, EventQueue.PrimaryConfig.datasources.inventory.carbonblack.queue, GetRabbitEnum.CB);
          if (lFidoReturnValues?.Inventory?.CarbonBlack == null) continue;

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
