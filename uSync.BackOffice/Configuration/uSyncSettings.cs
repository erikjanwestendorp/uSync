﻿using System;
using System.Collections.Generic;
using System.ComponentModel;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace uSync.BackOffice.Configuration
{
    /// <summary>
    /// uSync Settings
    /// </summary>
    [JsonObject(NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class uSyncSettings
    {
        /// <summary>
        /// Location where all uSync files are saved by default
        /// </summary>
        [DefaultValue("uSync/v9/")]
        public string RootFolder { get; set; } = "uSync/v9/";

        /// <summary>
        /// The default handler set to use on all notification triggered events
        /// </summary>
        [DefaultValue(uSync.Sets.DefaultSet)]
        public string DefaultSet { get; set; } = uSync.Sets.DefaultSet;

        /// <summary>
        /// Import when Umbraco boots (can be group name or 'All' so everything is done, blank or 'none' == off)
        /// </summary>
        [DefaultValue("None")]
        public string ImportAtStartup { get; set; } = "None";

        /// <summary>
        /// Export when Umbraco boots
        /// </summary>
        [DefaultValue("None")]
        public string ExportAtStartup { get; set; } = "None";

        /// <summary>
        /// Export when an item is saved in Umbraco
        /// </summary>
        [DefaultValue("All")]
        public string ExportOnSave { get; set; } = "All";


        /// <summary>
        /// The handler groups that are enabled in the UI.
        /// </summary>
        [DefaultValue("All")]
        public string UIEnabledGroups { get; set; } = "All";

        /// <summary>
        /// Debug reports (creates an export into a temp folder for comparison)
        /// </summary>
        [DefaultValue(false)]
        public bool ReportDebug { get; set; } = false;

        /// <summary>
        /// Ping the AddOnUrl to get the json used to show the addons dashboard
        /// </summary>
        [DefaultValue(true)]
        public bool AddOnPing { get; set; } = true;

        /// <summary>
        /// Pre Umbraco 8.4 - rebuild the cache was needed after content was imported
        /// </summary>
        [DefaultValue(false)]
        public bool RebuildCacheOnCompletion { get; set; } = false;

        /// <summary>
        /// Fail if the items parent is not in umbraco or part of the batch being imported
        /// </summary>
        [DefaultValue(false)]
        public bool FailOnMissingParent { get; set; } = false;

        /// <summary>
        /// Should folder keys be cached (for speed)
        /// </summary>
        [DefaultValue(true)]
        public bool CacheFolderKeys { get; set; } = true;


        /// <summary>
        /// Show a version check warning to the user if the folder version is less than the version expected by uSync.
        /// </summary>
        [DefaultValue(true)]
        public bool ShowVersionCheckWarning { get; set; } = true;

        /// <summary>
        /// Custom mapping keys, allows users to add a simple config mapping to make one property type to behave like an existing one
        /// </summary>
        public IDictionary<string, string> CustomMappings { get; set; } = new Dictionary<string, string>();

        /// <summary>
        /// location of SignalR hub script
        /// </summary>
        [DefaultValue("")]
        public string SignalRRoot { get; set; } = string.Empty;

        /// <summary>
        /// Should the history view be on of off ? 
        /// </summary>
        [DefaultValue(true)]
        public bool EnableHistory { get; set; } = true;

        /// <summary>
        /// Default file extension for the uSync files. 
        /// </summary>
        [DefaultValue("config")]
        public string DefaultExtension { get; set; } = "config";

        /// <summary>
        /// Import the uSync folder on the first boot. 
        /// </summary>
        [DefaultValue(false)]
        public bool ImportOnFirstBoot { get; set; } = false ;

        /// <summary>
        /// Handler group(s) to run on first boot, default is All (so full import)
        /// </summary>
        [DefaultValue("All")]
        public string FirstBootGroup { get; set; } = "All";
    }

}
