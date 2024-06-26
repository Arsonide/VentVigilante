﻿using System;
using Il2CppInterop.Runtime.Injection;
using BepInEx;
using SOD.Common;
using SOD.Common.BepInEx;
using SOD.Common.Helpers;
using VentrixSyncDisks.Implementation.Common;
using VentrixSyncDisks.Implementation.Config;
using VentrixSyncDisks.Implementation.Disks;
using VentrixSyncDisks.Implementation.Freakouts;
using VentrixSyncDisks.Implementation.Mapping;
using VentrixSyncDisks.Implementation.Pooling;
using VentrixSyncDisks.Implementation.Snooping;

namespace VentrixSyncDisks;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
public class VentrixPlugin : PluginController<VentrixPlugin>
{
    public static Action OnLoaded;
    
    public override void Load()
    {
        base.Load();
        
        VentrixConfig.Initialize(Config);
        
        if (!VentrixConfig.Enabled.Value)
        {
            Utilities.Log($"Plugin {MyPluginInfo.PLUGIN_GUID} is disabled.");
            return;
        }

        Utilities.Log($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");
        Harmony.PatchAll();
        Utilities.Log($"Plugin {MyPluginInfo.PLUGIN_GUID} is patched!");
        ClassInjector.RegisterTypeInIl2Cpp<BasePoolObject>();
        ClassInjector.RegisterTypeInIl2Cpp<DuctMarker>();
        ClassInjector.RegisterTypeInIl2Cpp<EcholocationPulse>();
        ClassInjector.RegisterTypeInIl2Cpp<SnoopHighlighter>();
        ClassInjector.RegisterTypeInIl2Cpp<Timer>();
        Utilities.Log($"Plugin {MyPluginInfo.PLUGIN_GUID} has added custom types!");
        
        Initialize();
    }

    public override bool Unload()
    {
        Uninitialize();
        return base.Unload();
    }

    private void Initialize()
    {
        DiskRegistry.Initialize();
        SnoopManager.Initialize();
        FreakoutManager.Initialize();
        InputManager.Initialize();
        
        Lib.SaveGame.OnAfterLoad -= OnAfterLoad;
        Lib.SaveGame.OnAfterLoad += OnAfterLoad;
    }

    private void Uninitialize()
    {
        if (EcholocationPulsePool.Instance != null)
        {
            EcholocationPulsePool.Instance.Uninitialize();
        }
        
        if (SnoopHighlighterPool.Instance != null)
        {
            SnoopHighlighterPool.Instance.Uninitialize();
        }
        
        if (DuctMarkerPool.Instance != null)
        {
            DuctMarkerPool.Instance.Uninitialize();
        }
        
        DiskRegistry.Uninitialize();
        SnoopManager.Uninitialize();
        FreakoutManager.Uninitialize();
        InputManager.Uninitialize();
        
        Lib.SaveGame.OnAfterLoad -= OnAfterLoad;
    }

    private void OnAfterLoad(object sender, SaveGameArgs args)
    {
        if (EcholocationPulsePool.Instance == null)
        {
            EcholocationPulsePool.Instance = new EcholocationPulsePool();
            EcholocationPulsePool.Instance.Initialize();
        }
        
        if (SnoopHighlighterPool.Instance == null)
        {
            SnoopHighlighterPool.Instance = new SnoopHighlighterPool();
            SnoopHighlighterPool.Instance.Initialize();
        }
        
        if (DuctMarkerPool.Instance == null)
        {
            DuctMarkerPool.Instance = new DuctMarkerPool();
            DuctMarkerPool.Instance.Initialize();
        }
        
        Timer.Create();

        OnLoaded?.Invoke();
    }
}