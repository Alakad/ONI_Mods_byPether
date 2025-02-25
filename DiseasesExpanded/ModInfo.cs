﻿using HarmonyLib;
using KMod;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DiseasesExpanded
{
    class ModInfo : KMod.UserMod2
    {
        public static string Namespace = "";

        public override void OnLoad(Harmony harmony)
        {
            base.OnLoad(harmony);

            Namespace = GetType().Namespace;
            Debug.Log($"{Namespace}: Loaded from: {this.mod.ContentPath}");
            Debug.Log($"{Namespace}: Mod version: {this.mod.packagedModInfo.version} " +
                        $"supporting game build {this.mod.packagedModInfo.minimumSupportedBuild} ({this.mod.packagedModInfo.supportedContent})");
        }

        public override void OnAllModsLoaded(Harmony harmony, IReadOnlyList<Mod> mods)
        {
            base.OnAllModsLoaded(harmony, mods);

            if(Settings.Instance.AutoDetectRelatedMods)
            {
                foreach (Mod mod in mods)
                    if (mod.staticID == "1911357229.Steam"
                        || Type.GetType("DiseasesReimagined.DiseasesPatch, DiseasesReimagined", false) != null) // double check in case steam messes with the ID
                    {
                        Settings.Instance.RebalanceForDiseasesRestored = mod.IsActive();
                        string activeString = mod.IsActive() ? "Active" : "NOT Active";
                        Debug.Log($"{Namespace}: Mod Id = \"{mod.staticID}\", Title = \"{mod.title}\", detected to be {activeString}.");
                    }

                JsonSerializer<Settings>.Serialize(Settings.Instance);
            }
        }
    }
}
