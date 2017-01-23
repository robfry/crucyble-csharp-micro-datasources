// /*
// *
// *  Copyright 2016  Netflix, Inc.
// *
// *     Licensed under the Apache License, Version 2.0 (the "License");
// *     you may not use this file except in compliance with the License.
// *     You may obtain a copy of the License at
// *
// *         http://www.apache.org/licenses/LICENSE-2.0
// *
// *     Unless required by applicable law or agreed to in writing, software
// *     distributed under the License is distributed on an "AS IS" BASIS,
// *     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// *     See the License for the specific language governing permissions and
// *     limitations under the License.
// *
// */

using System;
using System.Collections.Generic;
using System.Net;
using FIDO.Datasources.FIDO.Support.API.Endpoints;
using FIDO.Datasources.FIDO.Support.ErrorHandling;
using FIDO.Datasources.FIDO.Support.FidoDB;
using FIDO.Datasources.FIDO.Support.Hashing;
using FIDO.Datasources.FIDO.Support.Rest;
using Newtonsoft.Json;

namespace FIDO.Datasources.FIDO.Support.Sysmgmt
{
  public static class SysMgmtSentinelOne
  {
    public static FidoReturnValues RunSentinelOneQuery(FidoReturnValues lFidoReturnValues)
    {
      lFidoReturnValues.Inventory.SentinelOne = GetSentinelOneHost("fubar");
      if (!string.IsNullOrEmpty(lFidoReturnValues.Inventory.SentinelOne.software_information.os_name))
      {
        lFidoReturnValues.Inventory.PrimInv = "sentinelone";
        lFidoReturnValues.Inventory.SentRunning = "True";
        lFidoReturnValues.Inventory.SentVersion = lFidoReturnValues.Inventory.SentinelOne.agent_version;
        lFidoReturnValues.Inventory.OSName = lFidoReturnValues.Inventory.SentinelOne.software_information.os_name + @" " + lFidoReturnValues.Inventory.SentinelOne.software_information.os_revision;
        lFidoReturnValues.Inventory.Domain = lFidoReturnValues.Inventory.SentinelOne.network_information.domain;
        lFidoReturnValues.Inventory.LastUpdated = lFidoReturnValues.Inventory.SentinelOne.last_active_date;
        lFidoReturnValues.Inventory.Hostname = lFidoReturnValues.Inventory.SentinelOne.network_information.computer_name;
        lFidoReturnValues.Hostname = lFidoReturnValues.Inventory.SentinelOne.network_information.computer_name;
      }

      foreach (var interFace in lFidoReturnValues.Inventory.SentinelOne.network_information.interfaces)
      {
        if (interFace.inet.Count > 0)
        {
          if (interFace.inet?.Count > 0)
          {
            var sIP = interFace.inet[0].ToString();
            if (sIP.StartsWith("10.") | sIP.StartsWith("192.168.") | sIP.StartsWith("100.") | sIP.StartsWith("172."))
            {
              lFidoReturnValues.SrcIP = sIP;
            }
          }
        }
      }
      return lFidoReturnValues;
    }

    private static Object_SentinelOne_Inventory_Class.SentinelOne GetSentinelOneHost(string hostHash)
    {
      Console.WriteLine(@"Gathering inventory data from SentinelOne.");
      var invSentinelOne = new Object_SentinelOne_Inventory_Class.SentinelOne();
      var configs = GetSentinelOneConfigs();
      foreach (var entry in configs.rows)
      {
        var request = entry.value.configs.server + entry.value.configs.query[0] + hostHash;
        var alertRequest = (HttpWebRequest) WebRequest.Create(request);
        alertRequest.Method = "GET";
        alertRequest.Headers[@"Authorization"] = @"Token " + Base64.Decode(entry.value.configs.token);

        try
        {
          var getREST = new Fido_Rest_Connection();
          var stringreturn = getREST.RestCall(alertRequest);
          if (string.IsNullOrEmpty(stringreturn)) return invSentinelOne;
          invSentinelOne = JsonConvert.DeserializeObject<Object_SentinelOne_Inventory_Class.SentinelOne>(stringreturn);
          if (invSentinelOne != null)
          {
            return invSentinelOne;
          }
          Console.WriteLine(@"Finished retrieving SentinelOne alerts.");
        }
        catch (Exception e)
        {
          Fido_EventHandler.SendEmail("Fido Error", "Fido Failed: {0} Exception caught in SentinelOne alert area:" + e);
        }
      }

      return invSentinelOne;
    }

    private static SentinelOneConfigs GetSentinelOneConfigs()
    {
      var configs = new SentinelOneConfigs();
      var request = API_Endpoints.PrimaryConfig.host + API_Endpoints.PrimaryConfig.fido_configs_sysmgmt.sysmgmt.vendors + "?key=\"sentinelone\"";
      var invRequest = (HttpWebRequest)WebRequest.Create(request);
      invRequest.Method = "GET";
      try
      {
        var getREST = new Fido_Rest_Connection();
        var stringreturn = getREST.RestCall(invRequest);
        if (string.IsNullOrEmpty(stringreturn)) return configs;
        configs = JsonConvert.DeserializeObject<SentinelOneConfigs>(stringreturn);
        if (configs != null)
        {
          return configs;
        }
        Console.WriteLine(@"Finished retrieving SentinelOne alerts.");
      }
      catch (Exception e)
      {
        Fido_EventHandler.SendEmail("Fido Error", "Fido Failed: {0} Exception caught in SentinelOne alert area:" + e);
      }
      return configs;
    }

    public class Config
    {
      public string server { get; set; }
      public string token { get; set; }
      public List<string> query { get; set; }

    }

    public class Value
    {
      public string vendor { get; set; }
      public int type { get; set; }
      public int server { get; set; }
      public string label { get; set; }
      public Config configs { get; set; }
    }

    public class Row
    {
      public string id { get; set; }
      public string key { get; set; }
      public Value value { get; set; }
    }

    public class SentinelOneConfigs
    {
      public int total_rows { get; set; }
      public int offset { get; set; }
      public List<Row> rows { get; set; }
    }

  }
}