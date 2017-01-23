/*
 *
 *  Copyright 2015 Netflix, Inc.
 *
 *     Licensed under the Apache License, Version 2.0 (the "License");
 *     you may not use this file except in compliance with the License.
 *     You may obtain a copy of the License at
 *
 *         http://www.apache.org/licenses/LICENSE-2.0
 *
 *     Unless required by applicable law or agreed to in writing, software
 *     distributed under the License is distributed on an "AS IS" BASIS,
 *     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *     See the License for the specific language governing permissions and
 *     limitations under the License.
 *
 */

using System;
using System.Net;
using System.Threading;
using FIDO.Datasources.FIDO.Support.ErrorHandling;
using FIDO.Datasources.FIDO.Support.FidoDB;
using FIDO.Datasources.FIDO.Support.Hashing;
using FIDO.Datasources.FIDO.Support.Rest;
using FIDO.Datasources.FIDO.Support.XMLHelper;

namespace FIDO.Datasources.FIDO.Support.Sysmgmt
{
  public static class SysMgmtJamf
  {
    public static FidoReturnValues RunJamfQuery(FidoReturnValues lFidoReturnValues)
    {
      try
      {
        if (lFidoReturnValues.Inventory == null)
        {
          lFidoReturnValues.Inventory = new Inventory();
        }

        lFidoReturnValues.Inventory.Jamf = GetJamf(lFidoReturnValues, !string.IsNullOrEmpty(lFidoReturnValues.Hostname));

        if (lFidoReturnValues.Inventory?.Jamf != null)
        {
          lFidoReturnValues.Inventory.PrimInv = "jamf";
          if (lFidoReturnValues.Inventory.Jamf.Location != null)
          {
            lFidoReturnValues.Username = lFidoReturnValues.Inventory.Jamf.Location.Username;
          }
          if (lFidoReturnValues.Inventory.Jamf.General != null)
          {
            lFidoReturnValues.Inventory.LastUpdated = lFidoReturnValues.Inventory.Jamf.General.Report_date ??
                                                      string.Empty;
          }
          if (lFidoReturnValues.Inventory.Jamf.Hardware != null)
          {
            lFidoReturnValues.Inventory.Domain = lFidoReturnValues.Inventory.Jamf.Hardware.Active_directory_status ??
                                                 string.Empty;
            lFidoReturnValues.Inventory.OSName = lFidoReturnValues.Inventory.Jamf.Hardware.Os_name + " " +
                                                 lFidoReturnValues.Inventory.Jamf.Hardware.Os_version ?? string.Empty;
          }
          if (lFidoReturnValues.Inventory?.Jamf?.Software != null & lFidoReturnValues.Inventory?.Jamf?.Software?.Running_services != null)
          {
            lFidoReturnValues.Inventory.CBRunning =lFidoReturnValues.Inventory.Jamf.Software.Running_services.Name.Contains("com.carbonblack.daemon")
                .ToString() ?? string.Empty;
            lFidoReturnValues.Inventory.SentRunning =
              lFidoReturnValues.Inventory.Jamf.Software.Running_services.Name.Contains("com.sentinelone.sentineld")
                .ToString() ?? string.Empty;
          }

        }
        return lFidoReturnValues;
      }
      catch (Exception e)
      {
        Fido_EventHandler.SendEmail("Fido Error", "Fido Failed: {0} Exception caught in retrieving JAMF inventory section:" + e);
      }

      return null;
    }

    private static JamfReturnValues.Computer GetJamf(FidoReturnValues lFidoReturnValues, bool isHostName)
    {
      JamfReturnValues.Computer JamfReturn;
      if (isHostName)
      {
        Console.WriteLine(@"Querying JAMF for computer by hostname.");
        JamfReturn = GetJamfInventoryByID(null, lFidoReturnValues.Hostname);
        return JamfReturn;
      }
      else
      {
        Console.WriteLine(@"Querying JAMF for computers by IP.");
        JamfReturn = GetJamfInventoryByID(lFidoReturnValues.SrcIP, null);
        return JamfReturn;
      }
    }

    private static JamfReturnValues.Computer GetJamfInventoryByID(string srcIP, string hostname)
    {
      var jamfReturn = new JamfReturnValues.Computer();
      ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
      ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
      //todo: move this to DB
      var request = string.IsNullOrEmpty(srcIP) ? @"https://nfjamf.jamfcloud.com/JSSResource/computers/name/" + hostname : @"https://nfjamf.jamfcloud.com/JSSResource/computers/match/" + srcIP;

      var alertRequest = (HttpWebRequest)WebRequest.Create(request);
      alertRequest.Credentials = new NetworkCredential(Base64.Decode(@"Zmlkbw=="), Base64.Decode(@"Q3J1cDI0OTdqZmpmeXR5dCE="));
      alertRequest.Method = "GET";
      try
      {
        var getREST = new Fido_Rest_Connection();
        var stringreturn = getREST.RestCall(alertRequest);
        Thread.Sleep(500);
        if (string.IsNullOrEmpty(stringreturn)) return null;
        jamfReturn = stringreturn.ParseXML<JamfReturnValues.Computer>();
      }
      catch (Exception e)
      {
        Fido_EventHandler.SendEmail("Fido Error", "Fido Failed: {0} Exception caught in retrieving JAMF inventory section:" + e + " " + request);
      }

      return jamfReturn;
    }

    private static JamfReturnValues.Computer ComputerReturn(JamfReturnValues.Computers computers, FidoReturnValues lFidoReturnValues)
    {
      var c = new JamfReturnValues.Computer();

      foreach (var computer in computers.Computer)
      {
        c = GetJamfInventoryByID(computer.Id, null);
        //var i = 0;
        //while (c.General == null & i < 10)
        //{
        //  c = GetJamfInventoryByID(computer.Id, null);
        //  ++i;
        //}
        if (c.General == null) continue;
        if (c.General.Ip_address == lFidoReturnValues.SrcIP)
        {
          lFidoReturnValues.Inventory.Jamf = c;
        }
      }

      return c;
    }
  }
}
