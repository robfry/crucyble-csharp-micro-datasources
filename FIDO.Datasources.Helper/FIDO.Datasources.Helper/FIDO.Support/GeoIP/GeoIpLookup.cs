﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using FIDO.Datasources.FIDO.Support.ErrorHandling;
using FIDO.Datasources.FIDO.Support.Rest;
using Newtonsoft.Json;

namespace FIDO.Datasources.FIDO.Support.GeoIP
{
  public class GeoIpLookup
  {
    public GeoIpObject.Location GetLookup(List<string> dstip)
    {
      Console.WriteLine(@"Running GEO IP lookup.");
      //currently needed to bypass site without a valid cert.
      //todo: make ssl bypass configurable
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
      var request = "http://100.127.241.104:8080/json/" + dstip[0];
      Thread.Sleep(2000);
      var alertRequest = (HttpWebRequest) WebRequest.Create(request);
      alertRequest.Method = "GET";

      try
      {
        var getRest = new Fido_Rest_Connection();
        var stringreturn = getRest.RestCall(alertRequest);
        if (string.IsNullOrEmpty(stringreturn)) return null;
        var geoReturn = JsonConvert.DeserializeObject<GeoIpObject.Location>(stringreturn);
        if (geoReturn != null)
        {
          return geoReturn;
        }
        Console.WriteLine(@"Finished processing GEO IP lookup.");
      }
      catch (Exception e)
      {
        Fido_EventHandler.SendEmail("Fido Error", "Fido Failed: {0} Exception caught in GEO IP lookup with getting json:" + e);
      }
      return null;
    }

  }
}
