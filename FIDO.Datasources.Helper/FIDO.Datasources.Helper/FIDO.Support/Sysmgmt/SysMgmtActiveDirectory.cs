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
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Net;
using FIDO.Datasources.FIDO.Support.API.Endpoints;
using FIDO.Datasources.FIDO.Support.ErrorHandling;
using FIDO.Datasources.FIDO.Support.FidoDB;
using FIDO.Datasources.FIDO.Support.Hashing;
using FIDO.Datasources.FIDO.Support.Rest;
using Newtonsoft.Json;

namespace FIDO.Datasources.FIDO.Support.Sysmgmt
{
  public static class SysMgmtActiveDirectory
  {
    public static FidoReturnValues RunADQuery(FidoReturnValues lFidoReturnValues)
    {
      var acctDesktop = "";
      try
      {
        if (!string.IsNullOrEmpty(lFidoReturnValues.Hostname) || !string.IsNullOrEmpty(lFidoReturnValues.Username))
        {
          lFidoReturnValues.IsHostKnown = true;

          Console.WriteLine(@"Attempting to retrieve detailed user information.");

          //query Active Directory to get detailed user/manager information
          var username = string.Empty;
          if (!string.IsNullOrEmpty(lFidoReturnValues.Username))
          {
            var usernameAry = lFidoReturnValues.Username.Split('\\');
            if (usernameAry.Count() > 1)
            {
              if ((usernameAry[1].ToLower() != acctDesktop))
              {
                username = lFidoReturnValues.Username.Contains("\\") ? usernameAry[1] : lFidoReturnValues.Username;
              }
            }
            else if (usernameAry.Count() == 1)
            {
              username = usernameAry[0];
            }
            var lUserReturn = Getuserinfo(username);
            if (!string.IsNullOrEmpty(username))
            {
              if (lFidoReturnValues.UserInfo == null)
              {
                lFidoReturnValues.UserInfo = new UserReturnValues();
              }
              lFidoReturnValues.UserInfo = lUserReturn;
            }
          }
          else
          {
            Console.WriteLine(@"Unable to get detailed user information.");
            lFidoReturnValues.Username = "Unknown username";
          }
        }
        return lFidoReturnValues;
      }
      catch (Exception e)
      {
        Fido_EventHandler.SendEmail("Fido Error", "Fido Failed: {0} Exception caught in Active Directory grab user info area:" + e);
      }
      return null;
    }

    private static UserReturnValues Getuserinfo(string sUserId)
    {

      try
      {
        var AdConfigs = GetADConfigs();
        var lUserInfo = new UserReturnValues();
        var DomainPath = AdConfigs.rows[0].value.configs.basedn;
        var user = Base64.Decode(AdConfigs.rows[0].value.configs.userid);
        var pwd = Base64.Decode(AdConfigs.rows[0].value.configs.pwd);

        var searchRoot = new DirectoryEntry(DomainPath, user, pwd);
        var search = new DirectorySearcher(searchRoot)
        {
          Filter = "(&(objectClass=user)(objectCategory=person)(sAMAccountName=" + sUserId + "))"
        };

        search.PropertiesToLoad.Add("samaccountname");
        search.PropertiesToLoad.Add("mail");
        search.PropertiesToLoad.Add("displayname");
        search.PropertiesToLoad.Add("department");
        search.PropertiesToLoad.Add("title");
        search.PropertiesToLoad.Add("employeeType");
        search.PropertiesToLoad.Add("manager");
        search.PropertiesToLoad.Add("info");
        search.PropertiesToLoad.Add("l");
        search.PropertiesToLoad.Add("st");
        search.PropertiesToLoad.Add("streetAddress");
        search.PropertiesToLoad.Add("mobile");

        lUserInfo.UserEmail = string.Empty;
        lUserInfo.UserID = string.Empty;
        lUserInfo.Username = string.Empty;
        lUserInfo.Department = string.Empty;
        lUserInfo.Title = string.Empty;
        lUserInfo.EmployeeType = string.Empty;
        lUserInfo.CubeLocation = string.Empty;
        lUserInfo.City = string.Empty;
        lUserInfo.State = string.Empty;
        lUserInfo.StreetAddress = string.Empty;
        lUserInfo.MobileNumber = string.Empty;
        lUserInfo.ManagerID = string.Empty;
        lUserInfo.ManagerMail = string.Empty;
        lUserInfo.ManagerMobile = string.Empty;
        lUserInfo.ManagerTitle = string.Empty;
        lUserInfo.ManagerName = string.Empty;

        var resultCol = search.FindAll();
        if (!resultCol.PropertiesLoaded.Any() && resultCol == null) return lUserInfo;
        for (var counter = 0; counter < resultCol.Count; counter++)
        {
          var result = resultCol[counter];
          if (result.Properties.Contains("samaccountname") && result.Properties.Contains("mail") && result.Properties.Contains("displayname"))
          {
            if (result.Properties["mail"].Count > 0) lUserInfo.UserEmail = (String)result.Properties["mail"][0] ?? string.Empty;
            if (result.Properties["samaccountname"].Count > 0) lUserInfo.UserID = (String)result.Properties["samaccountname"][0] ?? string.Empty;
            if (result.Properties["displayname"].Count > 0) lUserInfo.Username = (String)result.Properties["displayname"][0] ?? string.Empty;
            if (result.Properties["department"].Count > 0) lUserInfo.Department = (String)result.Properties["department"][0] ?? string.Empty;
            if (result.Properties["title"].Count > 0) lUserInfo.Title = (String)result.Properties["title"][0] ?? string.Empty;
            if (result.Properties["employeeType"].Count > 0) lUserInfo.EmployeeType = (String)result.Properties["employeeType"][0] ?? string.Empty;
            if (result.Properties["manager"].Count > 0) lUserInfo.ManagerName = (String)result.Properties["manager"][0] ?? string.Empty;
            if (result.Properties["info"].Count > 0) lUserInfo.CubeLocation = (String)result.Properties["info"][0] ?? string.Empty;
            if (result.Properties["l"].Count > 0) lUserInfo.City = (String)result.Properties["l"][0] ?? string.Empty;
            if (result.Properties["st"].Count > 0) lUserInfo.State = (String)result.Properties["st"][0] ?? string.Empty;
            if (result.Properties["streetAddress"].Count > 0) lUserInfo.StreetAddress = (String)result.Properties["streetAddress"][0] ?? string.Empty;
            if (result.Properties["mobile"].Count > 0) lUserInfo.MobileNumber = (String)result.Properties["mobile"][0] ?? string.Empty;
          }

          if (string.IsNullOrEmpty(lUserInfo.ManagerName)) continue;
          var lManagerValues = Getmanagerinfo(lUserInfo.ManagerName);
          for (var i = 0; i < lManagerValues.Count; i++)
          {
            if (!lManagerValues[i].Any()) continue;
            switch (i)
            {
              case 0:
                lUserInfo.ManagerMail = lManagerValues[0];
                break;
              case 1:
                lUserInfo.ManagerID = lManagerValues[1];
                break;
              case 2:
                lUserInfo.ManagerName = lManagerValues[2];
                break;
              case 3:
                lUserInfo.ManagerTitle = lManagerValues[3];
                break;
              case 4:
                lUserInfo.ManagerMobile = lManagerValues[4];
                break;
            }
          }
        }

        return lUserInfo;
      }
      catch (Exception e)
      {
        Fido_EventHandler.SendEmail("Fido Error", "Fido Failed: {0} Exception caught in Active Directory grab user info area:" + e);
      }
      return null;
    }

    private static List<string> Getmanagerinfo(string sUserDN)
    {
      try
      {
        var AdConfigs = GetADConfigs();
        var DomainPath = AdConfigs.rows[0].value.configs.basedn;
        var user = Base64.Decode(AdConfigs.rows[0].value.configs.userid);
        var pwd = Base64.Decode(AdConfigs.rows[0].value.configs.pwd);
        var lManagerValues = new List<string>();
        var searchRoot = new DirectoryEntry(DomainPath, user, pwd);
        var search = new DirectorySearcher(searchRoot)
        {
          Filter = "(&(objectClass=user)(objectCategory=person)(distinguishedName=" + sUserDN + "))"
        };
        search.PropertiesToLoad.Add("mail");
        search.PropertiesToLoad.Add("samaccountname");
        search.PropertiesToLoad.Add("displayname");
        search.PropertiesToLoad.Add("title");
        search.PropertiesToLoad.Add("mobile");

        SearchResultCollection resultCol = search.FindAll();
        for (var counter = 0; counter < resultCol.Count; counter++)
        {
          //var UserNameEmailString = string.Empty;
          var result = resultCol[counter];
          if (result.Properties["mail"].Count > 0) lManagerValues.Add((String)result.Properties["mail"][0]);
          if (result.Properties["samaccountname"].Count > 0) lManagerValues.Add((String)result.Properties["samaccountname"][0]); 
          if (result.Properties["displayname"].Count > 0) lManagerValues.Add((String)result.Properties["displayname"][0]); 
          if (result.Properties["title"].Count > 0) lManagerValues.Add((String)result.Properties["title"][0]); 
          if (result.Properties["mobile"].Count > 0) lManagerValues.Add((String)result.Properties["mobile"][0]); 
          
        }
        return lManagerValues;
      }
      catch (Exception error)
      {
        Fido_EventHandler.SendEmail("Fido Error", "Fido Failed: {0} Exception caught in Active Directory grab manager info area:" + error);
      }
      return null;
    }

    private static ADConfigs GetADConfigs()
    {
      var connect = new Fido_Rest_Connection();
      var request = API_Endpoints.PrimaryConfig.host + API_Endpoints.PrimaryConfig.fido_configs_sysmgmt.sysmgmt.vendors + "?key=\"activedirectory\"";
      var connection = (HttpWebRequest)WebRequest.Create(request);
      var stringreturn = connect.RestCall(connection);
      if (string.IsNullOrEmpty(stringreturn)) return null;
      var detectorsReturn = JsonConvert.DeserializeObject<ADConfigs>(stringreturn);
      var retAD = detectorsReturn;
      return retAD;

    }

    public class ADConfigs
    {
        public int total_rows { get; set; }
        public int offset { get; set; }
        public List<Row> rows { get; set; }

      public class Configs
      {
        public string basedn { get; set; }
        public string userid { get; set; }
        public string pwd { get; set; }
      }

      public class Value
      {
        public string _id { get; set; }
        public string _rev { get; set; }
        public int type { get; set; }
        public int server { get; set; }
        public string label { get; set; }
        public string vendor { get; set; }
        public Configs configs { get; set; }
      }

      public class Row
      {
        public string id { get; set; }
        public string key { get; set; }
        public Value value { get; set; }
      }
    }
  }
}
