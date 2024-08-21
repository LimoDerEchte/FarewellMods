﻿using MelonLoader;
using UniteTheNorth.Systems;
using FarewellCore;
using UniteTheNorth.GUI;
using UnityEngine.SceneManagement;
using BuildInfo = UniteTheNorth.Properties.BuildInfo;

namespace UniteTheNorth;

public class UniteTheNorth : FarewellMod
{
    public static string Version => BuildInfo.Version;
    public static MelonLogger.Instance Logger => Melon<UniteTheNorth>.Logger;

    public override void OnInitializeMelon()
    {
        MessagePackExtender.Initialize();
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        TitleScreenPatcher.Patch(sceneName);
        LocalNetworkManager.OnSceneLoad(sceneName);
        if (sceneName != "Archipelago") return;
        PlayerManager.MainSceneLoaded();
    }

    public override void OnFixedUpdate()
    {
        if (SceneManager.GetActiveScene().name != "Archipelago")
            return;
        PlayerManager.UpdateState();
    }
}