﻿using BepInEx.Configuration;
using BepInEx.Logging;
using VentrixSyncDisks.Implementation.Common;

namespace VentrixSyncDisks.Implementation.Config;

public static partial class VentrixConfig
{
    public const string NAME_SHORT_RUNNER = "Runner";
    public const string NAME_LONG_RUNNER = "Airway Runner";

    public const string NAME_SHORT_PARKOUR = "Parkour";
    public const string NAME_LONG_PARKOUR = "Ductwork Parkour";

    public const string NAME_SHORT_MAPPING = "Mapping";
    public const string NAME_LONG_MAPPING = "Acoustic Mapping";

    public const string NAME_SHORT_SNOOPING = "Snooping";
    public const string NAME_LONG_SNOOPING = "Grill Snooping";

    public const string NAME_SHORT_SPECTER = "Specter";
    public const string NAME_LONG_SPECTER = "Crawlspace Specter";
    
    public const string NAME_SHORT_MENACE = "Menace";
    public const string NAME_LONG_MENACE = "Shaft Menace";

    private const string ExpectedVersion = "af2cfc5492bd478c9e7ccda801b1151c";
    
    public static ConfigEntry<bool> Enabled;
    public static ConfigEntry<string> Version;
    
    public static ConfigEntry<bool> MobilityEnabled;
    public static ConfigEntry<bool> ReconEnabled;
    public static ConfigEntry<bool> MischiefEnabled;

    public static ConfigEntry<bool> AvailableAtLegitSyncDiskClinics;
    public static ConfigEntry<bool> AvailableAtShadySyncDiskClinics;
    public static ConfigEntry<bool> AvailableAtBlackMarkets;

    public static void Initialize(ConfigFile config)
    {
        Enabled = config.Bind("1. General", "Enabled", true,
                              new ConfigDescription("Another method of enabling and disabling Babbler."));

        Version = config.Bind("1. General", "Version", string.Empty,
                              new ConfigDescription("Babbler uses this to reset your configuration between major versions. Don't modify it or it will reset your configuration!"));

        MobilityEnabled = config.Bind("1. General", "Vent Mobility Enabled", true,
                                              new ConfigDescription("Whether the \"Vent Mobility\" sync disk is in the game."));
        
        ReconEnabled = config.Bind("1. General", "Vent Recon Enabled", true,
                                      new ConfigDescription("Whether the \"Vent Recon\" sync disk is in the game."));
        
        MischiefEnabled = config.Bind("1. General", "Vent Mischief Enabled", true,
                                      new ConfigDescription("Whether the \"Vent Mischief\" sync disk is in the game."));

        AvailableAtLegitSyncDiskClinics = config.Bind("1. General", "Available At Legit Sync Disk Clinics", false,
                                                      new ConfigDescription("Whether Ventrix Industries sync disks are sold at legitimate sync disk clinics."));
        
        AvailableAtShadySyncDiskClinics = config.Bind("1. General", "Available At Shady Sync Disk Clinics", true,
                                                 new ConfigDescription("Whether Ventrix Industries sync disks are sold at black market sync disk clinics."));
        
        AvailableAtBlackMarkets = config.Bind("1. General", "Available At Black Markets", false,
                                              new ConfigDescription("Whether Ventrix Industries sync disks are sold at black markets."));
        
        InitializeMobility(config);
        InitializeRecon(config);
        InitializeMischief(config);

        ProcessUpgrades();
        
        Utilities.Log("VentrixConfig has initialized!", LogLevel.Debug);
    }

    private static void ProcessUpgrades()
    {
        if (Version.Value == ExpectedVersion)
        {
            return;
        }

        Utilities.Log("Detected either a new installation or a major upgrade of Ventrix Sync Disks, resetting the configuration file!");
        Version.Value = ExpectedVersion;
        Reset();
    }

    private static void Reset()
    {
        Enabled.Value = (bool)Enabled.DefaultValue;
        MobilityEnabled.Value = (bool)MobilityEnabled.DefaultValue;
        ReconEnabled.Value = (bool)ReconEnabled.DefaultValue;
        MischiefEnabled.Value = (bool)MischiefEnabled.DefaultValue;
        AvailableAtLegitSyncDiskClinics.Value = (bool)AvailableAtLegitSyncDiskClinics.DefaultValue;
        AvailableAtShadySyncDiskClinics.Value = (bool)AvailableAtShadySyncDiskClinics.DefaultValue;
        AvailableAtBlackMarkets.Value = (bool)AvailableAtBlackMarkets.DefaultValue;

        ResetMobility();
        ResetRecon();
        ResetMischief();
    }
}