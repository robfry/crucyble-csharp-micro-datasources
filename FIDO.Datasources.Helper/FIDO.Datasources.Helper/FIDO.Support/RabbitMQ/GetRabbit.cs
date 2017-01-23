using System;
using System.Net;
using System.Text;
using FIDO.Datasources.FIDO.Support.API.Endpoints;
using FIDO.Datasources.FIDO.Support.ErrorHandling;
using FIDO.Datasources.FIDO.Support.FidoDB;
using FIDO.Datasources.FIDO.Support.Rest;
using FIDO.Datasources.FIDO.Support.Sysmgmt;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.MessagePatterns;

namespace FIDO.Datasources.FIDO.Support.RabbitMQ
{
  public static class GetRabbit
  {
    public static FidoReturnValues ReceiveNotificationQueue(string host, string queue, GetRabbitEnum enumType)
    {
      Console.WriteLine(@"Subscribing to : " + host + @" and queue: " + queue);
      var lFidoReturnValues = new FidoReturnValues();
      var factory = new ConnectionFactory() { HostName = host };
      try
      {
        using (IConnection connection = factory.CreateConnection())
        {
          using (IModel model = connection.CreateModel())
          {
            var subscription = new Subscription(model, queue, false);
            while (true)
            {
              BasicDeliverEventArgs basicDeliveryEventArgs = subscription.Next();
              string messageContent = Encoding.UTF8.GetString(basicDeliveryEventArgs.Body);

              Console.WriteLine(messageContent);
              var rabbitmq = JsonConvert.DeserializeObject<Object_RabbitMQ.EventMsg>(messageContent);
              if (rabbitmq.notification.uuid == null) return null;
              lFidoReturnValues = GetFidoJson(rabbitmq.notification.uuid);

              lFidoReturnValues = ReturnHostDetection(lFidoReturnValues, enumType);

              if (lFidoReturnValues != null)
              {
                subscription.Ack(basicDeliveryEventArgs);
              }
              return lFidoReturnValues;
            }
          }
        }
      }
      catch (Exception e)
      {
        Console.WriteLine(e.GetBaseException().Message);
        Fido_EventHandler.SendEmail(e.GetBaseException().Message, "Fido Failed: {0} Exception caught retrieving messages from queue:" + e);
      }
      return lFidoReturnValues;
    }

    private static FidoReturnValues ReturnHostDetection(FidoReturnValues lFidoReturnValues, GetRabbitEnum enumType)
    {
      try
      {
        switch (enumType)
        {
          case GetRabbitEnum.Landesk:
            lFidoReturnValues = SysmgmtLandesk.RunLandeskQuery(lFidoReturnValues, lFidoReturnValues.SrcIP, lFidoReturnValues.Hostname);
            return lFidoReturnValues;
          case GetRabbitEnum.Jamf:
            lFidoReturnValues = SysMgmtJamf.RunJamfQuery(lFidoReturnValues);
            return lFidoReturnValues;
          case GetRabbitEnum.AD:
            lFidoReturnValues = SysMgmtActiveDirectory.RunADQuery(lFidoReturnValues);
            return lFidoReturnValues;
          case GetRabbitEnum.CB:
            lFidoReturnValues = SysMgmtCarbonBlack.RunCBQuery(lFidoReturnValues);
            return lFidoReturnValues;
          case GetRabbitEnum.S1:
            lFidoReturnValues = SysMgmtSentinelOne.RunSentinelOneQuery(lFidoReturnValues);
            return lFidoReturnValues;
        }
      }
      catch (Exception e)
      {
        Console.WriteLine(e.GetBaseException().Message);
        Fido_EventHandler.SendEmail(e.GetBaseException().Message, "Fido Failed: {0} Exception caught retrieving messages from queue:" + e);
      }
      return null;
    }

    private static FidoReturnValues GetFidoJson(string uuid)
    {

      //Load Fido configs from CouchDB
      var query = API_Endpoints.PrimaryConfig.host + API_Endpoints.PrimaryConfig.fido_events_alerts.dbname + @"/" + uuid;
      FidoReturnValues lFidoReturnValues;
      var connect = new Fido_Rest_Connection();

      try
      {
        var connection = (HttpWebRequest)WebRequest.Create(query);
        var stringreturn = connect.RestCall(connection);
        if (string.IsNullOrEmpty(stringreturn)) return null;
        lFidoReturnValues = JsonConvert.DeserializeObject<FidoReturnValues>(stringreturn);
        lFidoReturnValues.UUID = uuid;
        if (lFidoReturnValues == null)
        {
          Console.WriteLine(stringreturn);
        }
      }
      catch (Exception e)
      {
        Console.WriteLine(e.GetBaseException().Message);
        Fido_EventHandler.SendEmail(e.GetBaseException().Message, "Fido Failed: {0} Exception caught in REST call to CouchDB to retrieve FIDO object:" + e);
        return null;
      }

      return lFidoReturnValues;
    }
  }
}
