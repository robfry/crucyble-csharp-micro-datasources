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
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace FIDO.Datasources.FIDO.Support.Sysmgmt
{
  [DataContract]
  public class Object_CarbonBlack_Inventory_Class
  {

    [DataContract]
    public class CarbonBlackEntry
    {
      [DataMember]
      [JsonProperty("os_environment_display_string")]
      internal string OSName { get; set; }

      [DataMember]
      [JsonProperty("supports_cblr")]
      internal string SupportsCBLR { get; set; }

      [DataMember]
      [JsonProperty("last_update")]
      internal DateTime LastUpdated { get; set; }

      [DataMember]
      [JsonProperty("build_id")]
      internal string BuildID { get; set; }

      [DataMember]
      [JsonProperty("is_isolating")]
      internal bool isIsolating { get; set; }

      [DataMember]
      [JsonProperty("computer_dns_name")]
      internal string HostDNSName { get; set; }

      [DataMember]
      [JsonProperty("id")]
      internal Int16 ID { get; set; }

      [DataMember]
      [JsonProperty("network_isolation_enabled")]
      internal bool NetworkIsolationEnabled { get; set; }

      [DataMember]
      [JsonProperty("status")]
      internal string Status { get; set; }

      [DataMember]
      [JsonProperty("sensor_health_message")]
      internal string SensorHealthMessage { get; set; }

      [DataMember]
      [JsonProperty("build_version_string")]
      internal string ClientVersion { get; set; }

      [DataMember]
      [JsonProperty("computer_sid")]
      internal string ComputerSID { get; set; }

      [DataMember]
      [JsonProperty("next_checkin_time")]
      internal DateTime NextCheckinTime { get; set; }

      [DataMember]
      [JsonProperty("node_id")]
      internal short NodeID { get; set; }

      [DataMember]
      [JsonProperty("computer_name")]
      internal string HostName { get; set; }

      [DataMember]
      [JsonProperty("supports_isolation")]
      internal bool SupportsIso { get; set; }

      [DataMember]
      [JsonProperty("parity_host_id")]
      internal string ParityHostID { get; set; }

      [DataMember]
      [JsonProperty("network_adapters")]
      internal string NetworkAdapters { get; set; }

      [DataMember]
      [JsonProperty("sensor_health_status")]
      internal string SensorHealthStatus { get; set; }

      [DataMember]
      [JsonProperty("restart_queued")]
      internal bool RestartQueued { get; set; }

      [DataMember]
      [JsonProperty("notes")]
      internal string Notes { get; set; }

      [DataMember]
      [JsonProperty("os_environment_id")]
      internal string OSEnvironmentID { get; set; }

      [DataMember]
      [JsonProperty("boot_id")]
      internal string BootID { get; set; }

      [DataMember]
      [JsonProperty("last_checkin_time")]
      internal DateTime LastCheckinTime { get; set; }

      [DataMember]
      [JsonProperty("group_id")]
      internal short GroupdID { get; set; }

      [DataMember]
      [JsonProperty("display")]
      internal bool Display { get; set; }

      [DataMember]
      [JsonProperty("uninstall")]
      internal bool Uninstall { get; set; }
    }
  }

}
