﻿using HarmonyLib;
using UnityEngine;

namespace ConfigurableBuildMenus
{
    class ModInfo : KMod.UserMod2
    {
        public static string Namespace { get; private set; }

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);

            Namespace = GetType().Namespace;
            Debug.Log($"{Namespace}: Loaded from: {this.mod.ContentPath}");
            Debug.Log($"{Namespace}: Mod version: {this.mod.packagedModInfo.version} " +
                        $"supporting game build {this.mod.packagedModInfo.minimumSupportedBuild} ({this.mod.packagedModInfo.supportedContent})");
        }
    }
}
