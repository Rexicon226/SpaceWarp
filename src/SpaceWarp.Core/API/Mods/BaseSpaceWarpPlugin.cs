﻿using BepInEx;
using BepInEx.Logging;
using JetBrains.Annotations;
using KSP.Game;
using KSP.Messages;
using KSP.VFX;
using SpaceWarp.API.Mods.JSON;
using SpaceWarp.API.Configuration;
using SpaceWarp.API.Logging;

namespace SpaceWarp.API.Mods;

/// <summary>
/// Represents a KSP2 Mod, you should inherit from this and do your manager processing.
/// </summary>
[PublicAPI]
public abstract class BaseSpaceWarpPlugin : BaseUnityPlugin, ISpaceWarpMod
{
    #region KspBehaviour things

    protected static GameInstance Game => GameManager.Instance == null ? null : GameManager.Instance.Game;

    protected MessageCenter Messages => Game.Messages;

    // ReSharper disable Unity.NoNullPropagation
    // fine because its null checked by Game properly
    protected ContextualFxSystem CFXSystem => Game?.GraphicsManager?.ContextualFxSystem;

    protected bool IsGameShuttingDown => Game == null;

    #endregion

    public ModInfo SpaceWarpMetadata { get; internal set; }
    internal ManualLogSource ModLogger => Logger;
    public string PluginFolderPath { get; internal set; }

    public string IdBySpec => GetGuidBySpec(Info, SpaceWarpMetadata);

    /// <summary>
    ///     1st stage initialization
    ///     This is called before any of the game is actually loaded, it is called as early as possible in the games bootstrap
    ///     process.
    /// </summary>
    public virtual void OnPreInitialized()
    {
    }

    /// <summary>
    ///     2nd stage initialization
    ///     This is called after the game is loaded, and after your mods assets are loaded.
    /// </summary>
    public virtual void OnInitialized()
    {
    }

    /// <summary>
    ///     3rd stage initialization
    ///     This is called after all mods have done first stage initialization
    /// </summary>
    public virtual void OnPostInitialized()
    {
    }
    private BepInExLogger _logger;
    public ILogger SWLogger => _logger ??= new BepInExLogger(Logger);
    private BepInExConfigFile _configFile;
    public IConfigFile SWConfiguration => _configFile ??= new BepInExConfigFile(Config);
    public SpaceWarpPluginDescriptor SWMetadata { get; set; }

    internal static string GetGuidBySpec(PluginInfo pluginInfo, ModInfo modInfo)
    {
        return modInfo.Spec >= SpecVersion.V1_2
            ? pluginInfo.Metadata.GUID
            : modInfo.ModID;
    }
}