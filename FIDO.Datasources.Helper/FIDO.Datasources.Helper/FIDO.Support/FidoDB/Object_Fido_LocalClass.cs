﻿/*
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

using System.Collections.Generic;
using System.Data;
using System.Runtime.Serialization;
using FIDO.Datasources.FIDO.Support.API.Endpoints;
using FIDO.Datasources.FIDO.Support.DHCP;
using FIDO.Datasources.FIDO.Support.GeoIP;
using FIDO.Datasources.FIDO.Support.Sysmgmt;

namespace FIDO.Datasources.FIDO.Support.FidoDB
{
  //This is the primary object used throughout FIDO to support
  //the assembly line methodology.
  [DataContract]
  public class FidoReturnValues
  {
    [DataMember]
    public string UUID { get; set; }
    [DataMember]
    public string _rev { get; set; }
    public Object_API_Endpoints.PrimaryConfig ApiConfig { get; set; }
    [DataMember]
    public Inventory Inventory { get; set; }
    [DataMember]
    public SentinelOneReturnValues SentinelOne { get; set; }
    [DataMember]
    public CarbonBlackReturnValues CB { get; set; }
    [DataMember]
    public Object_DDI DDI { get; set; }
    [DataMember]
    public UserReturnValues UserInfo { get; set; }
    [DataMember]
    public FireEyeReturnValues FireEye { get; set; }
    [DataMember]
    public Bit9ReturnValues Bit9 { get; set; }
    [DataMember]
    public AntivirusReturnValues Antivirus { get; set; }
    [DataMember]
    public CyphortReturnValues Cyphort { get; set; }
    [DataMember]
    public ProtectWiseReturnValues ProtectWise { get; set; }
    [DataMember]
    public PaloAltoReturnValues PaloAlto { get; set; }
    [DataMember]
    public NiddelReturnValues Niddel { get; set; }
    //public Matrix_Historical_Helper_Object.HostReturn PreviousAlerts { get; set; }
    //public List<Object_CouchDB_AlertID.Row> OldAlerts { get; set; }
    public HistoricalEvents HistoricalEvent { get; set; }
    [DataMember]
    public GeoIpObject.Location Location { get; set; }
    //[DataMember]
    //public Object_F5_VPN.ESVPN VPN { get; set; }
    [DataMember]
    public double[] location { get; set; }
    [DataMember]
    public bool isBinary { get; set; }
    [DataMember]
    public bool IsHostKnown { get; set; }
    [DataMember]
    public bool IsReboot { get; set; }
    [DataMember]
    public bool IsPatch { get; set; }
    [DataMember]
    public bool IsPreviousAlert { get; set; }
    [DataMember]
    public bool IsMachSeenBefore { get; set; }
    [DataMember]
    public bool IsUserSeenBefore { get; set; }
    [DataMember]
    public bool IsUrlSeenBefore { get; set; }
    [DataMember]
    public bool IsIPSeenBefore { get; set; }
    [DataMember]
    public bool IsDomainSeenBefore { get; set; }
    [DataMember]
    public bool IsHashSeenBefore { get; set; }
    [DataMember]
    public bool IsPCI { get; set; }
    [DataMember]
    public bool IsSendAlert { get; set; }
    [DataMember]
    public bool IsTargetOS { get; set; }
    [DataMember]
    public bool IsTest { get; set; }
    [DataMember]
    public string MalwareType { get; set; }
    [DataMember]
    public string RemoteRegHostname { get; set; }
    [DataMember]
    public string SSHHostname { get; set; }
    [DataMember]
    public string NmapHostname { get; set; }
    [DataMember]
    public string SrcIP { get; set; }
    [DataMember]
    public string SrcIP6 { get; set; }
    [DataMember]
    public List<string> DstIP { get; set; }
    [DataMember]
    public string DNSName { get; set; }
    [DataMember]
    public List<string> Url { get; set; }
    [DataMember]
    public List<string> Hash { get; set; }
    [DataMember]
    public List<string> Domain { get; set; }
    [DataMember]
    public string TimeOccured { get; set; }
    [DataMember]
    public string Hostname { get; set; }
    [DataMember]
    public string Username { get; set; }
    public string SummaryEmail { get; set; }
    [DataMember]
    public string MachineType { get; set; }
    [DataMember]
    public string CurrentDetector { get; set; }
    [DataMember]
    public string AlertID { get; set; }
    [DataMember]
    public double TotalScore { get; set; }
    [DataMember]
    public double ThreatScore { get; set; }
    [DataMember]
    public double MachineScore { get; set; }
    [DataMember]
    public double UserScore { get; set; }
    [DataMember]
    public double BadUrLs { get; set; }
    [DataMember]
    public double BadHashs { get; set; }
    [DataMember]
    public double BadDetectedComms { get; set; }
    [DataMember]
    public double BadDetectedDownloads { get; set; }
    [DataMember]
    public double BadDetectedUrls { get; set; }
    [DataMember]
    public List<string> Recommendation { get; set; }
    [DataMember]
    public List<string> Actions { get; set; }
    [DataMember]
    public List<string> Detectors { get; set; }
  }

  [DataContract]
  public class Inventory
  {
    [DataMember]
    public LandeskReturnValues Landesk { get; set; }

    [DataMember]
    public JamfReturnValues.Computer Jamf { get; set; }

    [DataMember]
    public Object_CarbonBlack_Inventory_Class.CarbonBlackEntry CarbonBlack { get; set; }

    [DataMember]
    public Object_SentinelOne_Inventory_Class.SentinelOne SentinelOne { get; set; }

    //[DataMember]
    //public Object_F5_VPN_Inventory VPN { get; set; }

    [DataMember]
    public string PrimInv { get; set; }

    [DataMember]
    public string Hostname { get; set; }

    [DataMember]
    public string OSName { get; set; }

    [DataMember]
    public string Domain { get; set; }

    [DataMember]
    public string CBVersion { get; set; }

    [DataMember]
    public string CBRunning { get; set; }

    [DataMember]
    public string SentVersion { get; set; }

    [DataMember]
    public string SentRunning { get; set; }

    [DataMember]
    public string LastUpdated { get; set; }

    [DataMember]
    public string MachineScore { get; set; }

    //public JamfReturnValues.Computers Computers { get; set; }
  }

  [DataContract]
  public class FireEyeReturnValues
  {
    [DataMember]
    public string EventTime { get; set; }
    [DataMember]
    public string DstIP { get; set; }
    [DataMember]
    public List<string> URL { get; set; }
    [DataMember]
    public List<string> MD5Hash { get; set; }
    [DataMember]
    public List<string> ChannelHost { get; set; }
    [DataMember]
    public string Referer { get; set; }
    [DataMember]
    public string Original { get; set; }
    [DataMember]
    public string HttpHeader { get; set; }
    [DataMember]
    public bool IsFireEye { get; set; }
    [DataMember]
    public VirusTotalReturnValues VirusTotal { get; set; }
    [DataMember]
    public ThreatGRIDReturnValues ThreatGRID { get; set; }
    [DataMember]
    public AlienVaultReturnValues AlienVault { get; set; }
    //[DataMember]
    //public OpenDNS.OpenDNS OpenDNS { get; set; }
    [DataMember]
    public Bit9ReturnValues Bit9 { get; set; }
  }

  [DataContract]
  public class CyphortReturnValues
  {
    [DataMember]
    public string EventTime { get; set; }
    [DataMember]
    public string DstIP { get; set; }
    [DataMember]
    public string EventID { get; set; }
    [DataMember]
    public string IncidentID { get; set; }
    [DataMember]
    public List<string> MD5Hash { get; set; }
    [DataMember]
    public List<string> URL { get; set; }
    [DataMember]
    public List<string> Domain { get; set; }
    [DataMember]
    public VirusTotalReturnValues VirusTotal { get; set; }
    [DataMember]
    public ThreatGRIDReturnValues ThreatGRID { get; set; }
    [DataMember]
    public AlienVaultReturnValues AlienVault { get; set; }
    //[DataMember]
    //public OpenDNS.OpenDNS OpenDNS { get; set; }
    [DataMember]
    public Bit9ReturnValues Bit9 { get; set; }
    //[DataMember]
    //public Object_Cyphort_Class.CyphortIncident IncidentDetails { get; set; }
    [DataMember]
    public string CyphortJson { get; set; }
  }

  [DataContract]
  public class ProtectWiseReturnValues
  {
    [DataMember]
    public string ProtectWiseType { get; set; }
    [DataMember]
    public string EventTime { get; set; }
    [DataMember]
    public string DstIP { get; set; }
    [DataMember]
    public string EventID { get; set; }
    [DataMember]
    public List<string> MD5 { get; set; }
    [DataMember]
    public List<string> URL { get; set; }
    [DataMember]
    public VirusTotalReturnValues VirusTotal { get; set; }
    [DataMember]
    public ThreatGRIDReturnValues ThreatGRID { get; set; }
    [DataMember]
    public AlienVaultReturnValues AlienVault { get; set; }
    //[DataMember]
    //public OpenDNS.OpenDNS OpenDNS { get; set; }
    [DataMember]
    public Bit9ReturnValues Bit9 { get; set; }
    //[DataMember]
    //public List<Object_ProtectWise_Threat_ConfigClass.ProtectWise_Observation> IncidentDetails { get; set; }
    //[DataMember]
    //public Object_ProtectWise_Threat_ConfigClass.ProtectWise_GEO GEO { get; set; }
    //[DataMember]
    //public Object_ProtectWise_Threat_ConfigClass.ProtectWise_Search_Event EventDetails { get; set; }
    [DataMember]
    public string ProtectWiseJson { get; set; }
  }

  [DataContract]
  public class PaloAltoReturnValues
  {
    [DataMember]
    public string EventTime { get; set; }
    [DataMember]
    public string EventID { get; set; }
    [DataMember]
    public string DstIp { get; set; }
    [DataMember]
    public string Url { get; set; }
    [DataMember]
    public string DstUser { get; set; }
    [DataMember]
    public bool isDst { get; set; }
    [DataMember]
    public VirusTotalReturnValues VirusTotal { get; set; }
    [DataMember]
    public ThreatGRIDReturnValues ThreatGRID { get; set; }
    [DataMember]
    public AlienVaultReturnValues AlienVault { get; set; }
    //[DataMember]
    //public OpenDNS.OpenDNS OpenDNS { get; set; }
    [DataMember]
    public string PANJson { get; set; }
  }

  [DataContract]
  public class NiddelReturnValues
  {
    //[DataMember]
    //public NiddelAlert NiddelAlert { get; set; }
    [DataMember]
    public VirusTotalReturnValues VirusTotal { get; set; }
    [DataMember]
    public ThreatGRIDReturnValues ThreatGRID { get; set; }
    //[DataMember]
    //public OpenDNS.OpenDNS OpenDNS { get; set; }
    [DataMember]
    public string EventTime { get; set; }
    [DataMember]
    public string EventID { get; set; }
    [DataMember]
    public string DstIp { get; set; }
    [DataMember]
    public List<string> Domain { get; set; }
  }

  [DataContract]
  public class AntivirusReturnValues
  {
    [DataMember]
    public string ReceivedTime { get; set; }
    [DataMember]
    public string EventTime { get; set; }
    [DataMember]
    public string ActionTaken { get; set; }
    [DataMember]
    public string Username { get; set; }
    [DataMember]
    public string Status { get; set; }
    [DataMember]
    public string ThreatType { get; set; }
    [DataMember]
    public string FilePath { get; set; }
    [DataMember]
    public string FileName { get; set; }
    [DataMember]
    public string HostName { get; set; }
    [DataMember]
    public string ThreatName { get; set; }
    [DataMember]
    public Bit9ReturnValues Bit9 { get; set; }
  }

  [DataContract]
  public class Bit9ReturnValues
  {
    [DataMember]
    public bool IsBit9 { get; set; }
    [DataMember]
    public string FileDeleted { get; set; }
    [DataMember]
    public string FileExecuted { get; set; }
    [DataMember]
    public string FileName { get; set; }
    [DataMember]
    public string FilePath { get; set; }
    [DataMember]
    public string HostName { get; set; }
    [DataMember]
    public string FileTrust { get; set; }
    [DataMember]
    public string FileThreat { get; set; }
    [DataMember]
    public string[] Bit9Hashes { get; set; }
    //public string[] HostNames { get; set; }
    //[DataMember]
    //public List<FileReport> VTReport { get; set; }
    [DataMember]
    public ThreatGRIDReturnValues ThreatGRID { get; set; }
    //public VirusTotalReturnValues VirusTotal { get; set; }
    //public ThreatGRIDReturnValues ThreatGRID { get; set; }
  }

  [DataContract]
  public class VirusTotalReturnValues
  {
    //[DataMember]
    //public List<FileReport> MD5HashReturn { get; set; }
    //[DataMember]
    //public List<UrlReport> URLReturn { get; set; }
    //[DataMember]
    //public List<Object_VirusTotal_IP.IPReport> IPReturn { get; set; }
    [DataMember]
    public string IPUrl { get; set; }
    [DataMember]
    public double VirusTotalScore { get; set; }
    [DataMember]
    public string VTJson { get; set; }
  }

  [DataContract]
  public class ThreatGRIDReturnValues
  {
    //[DataMember]
    //public Object_ThreatGRID_IP_ConfigClass.ThreatGRID_IP_HLInfo IPInfo { get; set; }
    //[DataMember]
    //public List<Object_ThreatGRID_Threat_ConfigClass.ThreatGRID_Threat_Info> IPThreatInfo { get; set; }
    //[DataMember]
    //public List<Object_ThreatGRID_Search_ConfigClass.ThreatGRID_Search> IPSearch { get; set; }
    //[DataMember]
    //public List<Object_ThreatGRID_Threat_ConfigClass.ThreatGRID_Threat_Info> HashThreatInfo { get; set; }
    //[DataMember]
    //public List<Object_ThreatGRID_Search_ConfigClass.ThreatGRID_Search> HashSearch { get; set; }
    [DataMember]
    public int ThreatScore { get; set; }
    [DataMember]
    public int ThreatIndicators { get; set; }
    [DataMember]
    public int ThreatConfidence { get; set; }
    [DataMember]
    public int ThreatSeverity { get; set; }
    [DataMember]
    public string ThreatGRIDJson { get; set; }
  }

  [DataContract]
  public class AlienVaultReturnValues
  {
    [DataMember]
    public int Reliability { get; set; }
    [DataMember]
    public int Risk { get; set; }
    [DataMember]
    public string Activity { get; set; }
    [DataMember]
    public string Country { get; set; }
    [DataMember]
    public string City { get; set; }
    [DataMember]
    public string Latitude { get; set; }
    [DataMember]
    public string Longitude { get; set; }
  }

  [DataContract]
  public class EventAlerts
  {
    [DataMember]
    public int PrimKey { get; set; }
    [DataMember]
    public int Timer { get; set; }
    [DataMember]
    public string IP { get; set; }
    [DataMember]
    public string Hostname { get; set; }
    [DataMember]
    public string TimeStamp { get; set; }
    [DataMember]
    public int PreviousScore { get; set; }
    [DataMember]
    public string AlertID { get; set; }
    [DataMember]
    public DataTable Alerts { get; set; }
  }

  [DataContract]
  public class HistoricalEvents
  {
    //public Matrix_Historical_Helper_Object.Scoring domain { get; set; }
    //public Matrix_Historical_Helper_Object.Scoring dstip { get; set; }
    //public Matrix_Historical_Helper_Object.Scoring hash { get; set; }
    //public Matrix_Historical_Helper_Object.Scoring url { get; set; }
  }

  [DataContract]
  public class LandeskReturnValues
  {
    [DataMember]
    public string Hostname { get; set; }
    [DataMember]
    public string Domain { get; set; }
    [DataMember]
    public string LastUpdate { get; set; }
    //[DataMember]
    //public string Product { get; set; }
    //[DataMember]
    //public string AgentRunning { get; set; }
    //[DataMember]
    //public string DefInstallDate { get; set; }
    [DataMember]
    public string OSName { get; set; }
    [DataMember]
    public string ComputerIDN { get; set; }
    [DataMember]
    public string Username { get; set; }
    [DataMember]
    public string OSType { get; set; }
    [DataMember]
    public string Type { get; set; }
    [DataMember]
    public string Battery { get; set; }
    [DataMember]
    public string ChassisType { get; set; }
    [DataMember]
    public string OSVersion { get; set; }
    [DataMember]
    public string OSBuild { get; set; }
    [DataMember]
    public string CBVersion { get; set; }
    [DataMember]
    public string CBRunning { get; set; }
    [DataMember]
    public string SentinelVersion { get; set; }
    [DataMember]
    public string SentinelRunning { get; set; }
    [DataMember]
    public List<int> Patches { get; set; }
  }

  [DataContract]
  public class SentinelOneReturnValues
  {
    //[DataMember]
    //public Object_SentinelOne_Alert_Class.SentinelOne Alert { get; set; }
    [DataMember]
    public VirusTotalReturnValues VirusTotal { get; set; }
    [DataMember]
    public ThreatGRIDReturnValues ThreatGRID { get; set; }
  }

  [DataContract]
  public class CarbonBlackReturnValues
  {
    [DataMember]
    public CarbonBlackAlert Alert { get; set; }
  }

  [DataContract]
  public class CarbonBlackAlert
  {
    [DataMember]
    public string EventTime { get; set; }
    [DataMember]
    public string EventID { get; set; }
    [DataMember]
    public string MD5Hash { get; set; }
    [DataMember]
    public string ProcessPath { get; set; }
    [DataMember]
    public string HostCount { get; set; }
    [DataMember]
    public string NetConn { get; set; }
    [DataMember]
    public string AlertType { get; set; }
    [DataMember]
    public string WatchListName { get; set; }
    [DataMember]
    public VirusTotalReturnValues VirusTotal { get; set; }
    [DataMember]
    public ThreatGRIDReturnValues ThreatGRID { get; set; }
    [DataMember]
    public AlienVaultReturnValues AlienVault { get; set; }
    //[DataMember]
    //public OpenDNS.OpenDNS OpenDNS { get; set; }
    [DataMember]
    public Bit9ReturnValues Bit9 { get; set; }
  }

  //[DataContract]
  //public class JamfReturnValues
  //{
  //  [DataMember]
  //  public string ComputerID { get; set; }
  //  [DataMember]
  //  public string Hostname { get; set; }
  //  [DataMember]
  //  public string OSName { get; set; }
  //  [DataMember]
  //  public string LastUpdate { get; set; }
  //  [DataMember]
  //  public string Username { get; set; }
  //  [DataMember]
  //  public string ReportID { get; set; }
  //}

  [DataContract]
  public class TempInventory
  {
    [DataMember]
    public TempInventoryValue[] Entry { get; set; }
  }

  [DataContract]
  public class TempInventoryValue
  {
    [DataMember]
    public string Hostname { get; set; }
    [DataMember]
    public string LastUpdate { get; set; }
    [DataMember]
    public string SrcIP { get; set; }
    [DataMember]
    public string DNSName { get; set; }
    [DataMember]
    public string DHCPName { get; set; }
    [DataMember]
    public string Source { get; set; }
  }

  [DataContract]
  public class UserReturnValues
  {
    [DataMember]
    public string UserEmail { get; set; }
    [DataMember]
    public string UserID { get; set; }
    [DataMember]
    public string Username { get; set; }
    [DataMember]
    public string Department { get; set; }
    [DataMember]
    public string Title { get; set; }
    [DataMember]
    public string EmployeeType { get; set; }
    [DataMember]
    public string ManagerID { get; set; }
    [DataMember]
    public string ManagerName { get; set; }
    [DataMember]
    public string ManagerMail { get; set; }
    [DataMember]
    public string ManagerTitle { get; set; }
    [DataMember]
    public string ManagerMobile { get; set; }
    [DataMember]
    public string CubeLocation { get; set; }
    [DataMember]
    public string City { get; set; }
    [DataMember]
    public string State { get; set; }
    [DataMember]
    public string StreetAddress { get; set; }
    [DataMember]
    public string MobileNumber { get; set; }
  }

}