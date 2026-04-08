using Microsoft.Win32;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text.RegularExpressions;

namespace TinyNvidiaUpdateChecker.Handlers
{
    class OldMetadataHandler
    {
        /// <summary>
        /// Cache max duration in days
        /// </summary>
        static int cacheDuration = 60;

        /// <summary>
        /// Cached GPU Data
        /// </summary>
        static JObject cachedGPUData;

        /// <summary>
        /// Cached OS Data
        /// </summary>
        static OSClassRoot cachedOSData;

        /// <summary>
        /// GPU name lookup powered by PCI Lookup
        /// </summary>
        static string PCILookupAPI = "https://www.pcilookup.com/api.php";

        public static void PrepareCache(bool forceRecache = false)
        {
            var gpuData = GetCachedMetadata("gpu-data.json", forceRecache);
            var osData = GetCachedMetadata("os-data.json", forceRecache);

            // Validate GPU Data JSON
            try {
                cachedGPUData = JObject.Parse(gpuData);
            } catch {
                gpuData = GetCachedMetadata("gpu-data.json", true);
                cachedGPUData = JObject.Parse(gpuData);
            }

            // Validate OS JSON
            try {
                cachedOSData = JsonConvert.DeserializeObject<OSClassRoot>(osData);
            } catch {
                osData = GetCachedMetadata("os-data.json", true);
                cachedOSData = JsonConvert.DeserializeObject<OSClassRoot>(osData);
            }
        }

        /// <summary>
        /// Uses PCI Lookup API to get a GPU label
        /// </summary>
        /// <returns>Found GPU label</returns>
        public static string LookupGpuLabel(string vendorID, string deviceID, string rawGpuLabel)
        {
            try
            {
                Regex apiRegex = new(@"([A-Za-z0-9]+( [A-Za-z0-9]+)+)");

                string url = $"{PCILookupAPI}?action=search&vendor={vendorID}&device={deviceID}";
                string rawData = MainConsole.SendGetRequest(url);
                PCILookupClassRoot apiResponse = JsonConvert.DeserializeObject<PCILookupClassRoot>(rawData);

                if (apiResponse != null && apiResponse.Count > 0)
                {
                    string rawName = apiResponse[0].desc;

                    if (apiRegex.IsMatch(rawName))
                    {
                        string foundLabel = apiRegex.Match(rawName).Value.Trim();

                        return foundLabel;
                    }
                }
            } catch { }

            return rawGpuLabel;
        }

        public static (bool, int) GetGpuIdFromName(string name, bool isNotebook)
        {
            try {
                int gpuId = (int)cachedGPUData[isNotebook ? "notebook" : "desktop"][name];
                return (true, gpuId);
            } catch {
                return (false, 0);
            }
        }
        public static OSClassRoot RetrieveOSData() { return cachedOSData; }

        private static dynamic GetCachedMetadata(string fileName, bool forceRecache)
        {
            string dataPath = Path.Combine(ConfigurationHandler.configDirectoryPath, fileName);

            // If the cache exists and is not outdated, then it can be used
            if (File.Exists(dataPath) && !forceRecache) {
                DateTime lastUpdate = File.GetLastWriteTime(dataPath);
                var days = (DateTime.Now - lastUpdate).TotalDays;

                if (days < cacheDuration) {
                    try {
                        return File.ReadAllText(dataPath);
                    } catch {

                    }
                }
            }

            // Delete corrupt/old file if it exists
            if (File.Exists(dataPath)) {
                try {
                    File.Delete(dataPath);
                } catch {
                    // error
                }
            }

            // Download the file and cache it
            string rawData = MainConsole.SendGetRequest($"{MainConsole.gpuMetadataRepo}/{fileName}");

            try {
                File.AppendAllText(dataPath, rawData);
            } catch {
                // Unable to cache
            }

            return rawData;
        }

        /// <summary>
        /// Finds the GPU, the version and queries up to date information
        /// </summary>
        public static (GPU, int, bool) GetDriverMetadata(bool forceRecache = false, bool useNewMetadataHandler = false)
        {
            bool isNotebook = false;
            bool isDchDriver = false; // TODO rewrite for each GPU
            Regex nameRegex = new(@"(?<=NVIDIA )(.*(?= \([A-Z]+\))|.*(?= [0-9]+GB)|.*(?= with Max-Q Design)|.*(?= COLLECTORS EDITION)|.*)");
            List<int> notebookChassisTypes = [1, 8, 9, 10, 11, 12, 14, 18, 21, 31, 32];
            List<GPU> gpuList = [];
            int osId = 0;

            if (!useNewMetadataHandler)
            {
                // Check for notebook
                // TODO rewrite and identify GPUs properly
                if (MainConsole.overrideChassisType == 0)
                {
                    foreach (var obj in new ManagementClass("Win32_SystemEnclosure").GetInstances())
                    {
                        foreach (int chassisType in obj["ChassisTypes"] as ushort[])
                        {
                            isNotebook = notebookChassisTypes.Contains(chassisType);
                        }
                    }
                }
                else
                {
                    isNotebook = notebookChassisTypes.Contains(MainConsole.overrideChassisType);
                }

                // Get operating system ID
                OSClassRoot osData = OldMetadataHandler.RetrieveOSData();
                string osVersion = $"{Environment.OSVersion.Version.Major}.{Environment.OSVersion.Version.Minor}";
                string osBit = Environment.Is64BitOperatingSystem ? "64" : "32";

                if (osVersion == "10.0" && Environment.OSVersion.Version.Build >= 22000)
                {
                    foreach (OSClass os in osData)
                    {
                        if (Regex.IsMatch(os.name, "Windows 11"))
                        {
                            osId = os.id;
                            break;
                        }
                    }
                }
                else
                {
                    foreach (OSClass os in osData)
                    {
                        if (os.code == osVersion && Regex.IsMatch(os.name, osBit))
                        {
                            osId = os.id;
                            break;
                        }
                    }
                }

                if (osId == 0)
                {
                    MainConsole.Write("ERROR!");
                    MainConsole.WriteLine();
                    MainConsole.WriteLine("No NVIDIA driver was found for this operating system configuration. Make sure TNUC is updated.");
                    MainConsole.WriteLine();
                    MainConsole.WriteLine($"osVersion: {osVersion}");
                    MainConsole.callExit(1);
                }
            }


            // Check for DCH for newer drivers
            // TODO do we know if this applies to every GPU?
            // UPDATE as of 2026-04-02, NVIDIA no longer creates this registry entry
            // Meaning this check is broken for newer Windows installations
            // OldMetadataHandler relies on the automatic DCH upgrade
            using (var regKey = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Services\nvlddmkm", false))
            {
                if (regKey != null && regKey.GetValue("DCHUVen") != null)
                {
                    isDchDriver = true;
                }
            }

            // Scan computer for GPUs
            foreach (ManagementBaseObject gpu in new ManagementObjectSearcher("SELECT Name, DriverVersion, PNPDeviceID FROM Win32_VideoController").Get())
            {
                string rawGpuLabel = gpu["Name"].ToString();
                string rawVersion = gpu["DriverVersion"].ToString().Replace(".", string.Empty);
                string pnp = gpu["PNPDeviceID"].ToString();

                // Is it a GPU?
                if (pnp.Contains("&DEV_"))
                {
                    string[] split = pnp.Split("&DEV_");
                    string vendorId = split[0][^4..].ToLower();
                    string deviceId = split[1][..4];

                    // Are drivers installed for this GPU? If not Windows reports a generic GPU name which is not sufficient
                    if (Regex.IsMatch(rawGpuLabel, @"^NVIDIA") && nameRegex.IsMatch(rawGpuLabel))
                    {
                        string gpuLabel = nameRegex.Match(rawGpuLabel).Value.Trim().Replace("Super", "SUPER");
                        string cleanVersion = rawVersion.Substring(rawVersion.Length - 5, 5).Insert(3, ".");

                        gpuList.Add(new GPU(gpuLabel, cleanVersion, vendorId, deviceId, true, isNotebook, isDchDriver));
                    }

                    // Name regex does not match, but the vendor is NVIDIA, revert to NewMetadataHandler
                    else if (vendorId == "10de")
                    {
                        // Use API lookup to find GPU label
                        // Otherwise, if system has multiple NVIDIA GPUs, the "choose GPU" dialog will show multiple GPUs with the generic driver
                        string gpuLabel = LookupGpuLabel(vendorId, deviceId, rawGpuLabel);

                        // If NewMetadataHandler mode is enabled, and the vendor is correct, then it's OK to use
                        // Because we don't rely on regex name match
                        // By setting the GPU as isValidated, it will appear as a viable GPU in later code
                        if (useNewMetadataHandler)
                        {
                            gpuList.Add(new GPU(gpuLabel, "000.00", vendorId, deviceId, true, isNotebook, isDchDriver, int.Parse(deviceId)));

                        // OldMetadataHandler requires name regex to match
                        // Reverting to NewMetadataHandler, which doesn't require it
                        } else {
                            gpuList.Add(new GPU(gpuLabel, "000.00", vendorId, deviceId, false, isNotebook, isDchDriver, int.Parse(deviceId)));
                        }
                    }
                }
            }

            // If NewMetadataHandler mode is enabled, then skip ZenitH-AT GetGpuIdFromName code
            if (!useNewMetadataHandler)
            {
                foreach (GPU gpu in gpuList.Where(x => x.isValidated))
                {
                    // Uses ZenitH-AT's nvidia-data repo
                    (bool success, int gpuId) = GetGpuIdFromName(gpu.name, gpu.isNotebook);

                    if (success)
                    {
                        gpu.id = gpuId;
                    }
                    else
                    {
                        // Invert isNotebook switch, perhaps it is an eGPU?
                        (success, gpuId) = GetGpuIdFromName(gpu.name, !gpu.isNotebook);

                        if (success)
                        {
                            gpu.isNotebook = !gpu.isNotebook;
                            gpu.id = gpuId;
                        }
                        else
                        {
                            gpu.isValidated = false;
                        }
                    }
                }
            }

            int gpuCount = gpuList.Where(x => x.isValidated).Count();

            // Was any validated GPU found?
            if (gpuCount > 0)
            {

                // More than one valid GPU was found, prompt user to choose the proper GPU
                if (gpuCount > 1)
                {
                    
                    // Retrieve GPU ID from config, or prompts user to choose, if config is not found
                    int configGpuId = int.Parse(ConfigurationHandler.ReadSetting("GPU ID", gpuList));

                    // Validate that the GPU ID is still active on this system
                    foreach (GPU gpu in gpuList.Where(x => x.isValidated))
                    {
                        if (gpu.id == configGpuId)
                        {
                            return (gpu, osId, true);
                        }
                    }

                    // GPU ID is no longer active on this system, prompt user to choose new GPU
                    configGpuId = int.Parse(ConfigurationHandler.SetupSetting("GPU ID", gpuList));

                    foreach (GPU gpu in gpuList.Where(x => x.isValidated))
                    {
                        if (gpu.id == configGpuId)
                        {
                            return (gpu, osId, true);
                        }
                    }
                }
                else
                {
                    // Only one GPU was found on the system
                    GPU gpu = gpuList.Where(x => x.isValidated).First();
                    return (gpu, osId, true);
                }
            }

            // If no GPU could be validated, then force recaching of OldMetadataHandler once, and loop again.
            // This fixes issues related with outdated cache
            if (!forceRecache & !useNewMetadataHandler)
            {
                PrepareCache(true);
                return GetDriverMetadata(true);
            }
            else
            {
                MainConsole.Write("ERROR!");
                MainConsole.WriteLine();

                if (!gpuList.Any(x => x.vendorId == "10de"))
                {
                    MainConsole.WriteLine("No NVIDIA GPU was detected on this system.");
                    MainConsole.WriteLine();
                }
                else
                {
                    MainConsole.WriteLine("GPU metadata lookup using OldMetadataHandler failed!");
                    MainConsole.WriteLine();
                    MainConsole.WriteLine("Found GPUs:");

                    foreach (GPU gpu in gpuList)
                    {
                        MainConsole.WriteLine($"GPU Name: '{gpu.name}' | VendorId: {gpu.vendorId} | DeviceId: {gpu.deviceId} | IsNotebook: {gpu.isNotebook}");
                    }

                    MainConsole.WriteLine();
                }

                // Return success false state
                // This will fall back to NewMetadataHandler
                return (null, 0, false);
            }
        }
        }
}
