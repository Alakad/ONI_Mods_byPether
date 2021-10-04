﻿using HarmonyLib;
using Database;
using System;
using UnityEngine;
using Klei.AI;
using System.Collections.Generic;

namespace DiseasesExpanded
{
    public class DiseasesExpanded_Patches_AllCommon
    {
        [HarmonyPatch(typeof(ColorSet))]
        [HarmonyPatch("Init")]
        public static class ColorSet_Init_Patch
        {
            static bool initalized = false;

            public static void Postfix(ColorSet __instance)
            {
                if (initalized)
                    return;

                Dictionary<string, Color32> namedLookup = Traverse.Create(__instance).Field("namedLookup").GetValue<Dictionary<string, Color32>>();
                namedLookup.Add(HungerGerms.ID, HungerGerms.colorValue);
                namedLookup.Add(BogInsects.ID, BogInsects.colorValue);
                namedLookup.Add(FrostShards.ID, FrostShards.colorValue);
                namedLookup.Add(GassyGerms.ID, GassyGerms.colorValue);

                initalized = true;
            }
        }

        [HarmonyPatch(typeof(Db))]
        [HarmonyPatch("Initialize")]
        public static class Db_Initialize_Patch
        {
            public static void Prefix()
            {
                BasicModUtils.MakeStrings(BogInsects.ID, STRINGS.GERMS.BOGINSECTS.NAME, STRINGS.GERMS.BOGINSECTS.DESCRIPTIVE_SYMPTOMS, STRINGS.GERMS.BOGINSECTS.DESCRIPTION, STRINGS.GERMS.BOGINSECTS.LEGEND_HOVERTEXT);
                BasicModUtils.MakeStrings(FrostShards.ID, STRINGS.GERMS.FROSTHARDS.NAME, STRINGS.GERMS.FROSTHARDS.DESCRIPTIVE_SYMPTOMS, STRINGS.GERMS.FROSTHARDS.DESCRIPTION, STRINGS.GERMS.FROSTHARDS.LEGEND_HOVERTEXT);
                BasicModUtils.MakeStrings(GassyGerms.ID, STRINGS.GERMS.GASSYGERMS.NAME, STRINGS.GERMS.GASSYGERMS.DESCRIPTIVE_SYMPTOMS, STRINGS.GERMS.GASSYGERMS.DESCRIPTION, STRINGS.GERMS.GASSYGERMS.LEGEND_HOVERTEXT);
                BasicModUtils.MakeStrings(HungerGerms.ID, STRINGS.GERMS.HUNGERGERMS.NAME, STRINGS.GERMS.HUNGERGERMS.DESCRIPTIVE_SYMPTOMS, STRINGS.GERMS.HUNGERGERMS.DESCRIPTION, STRINGS.GERMS.HUNGERGERMS.LEGEND_HOVERTEXT);

                BasicModUtils.MakeStrings(BogSickness.ID, STRINGS.DISEASES.BOGSICKNESS.NAME, STRINGS.DISEASES.BOGSICKNESS.DESCRIPTIVE_SYMPTOMS, STRINGS.DISEASES.BOGSICKNESS.DESCRIPTION, STRINGS.DISEASES.BOGSICKNESS.LEGEND_HOVERTEXT);
                BasicModUtils.MakeStrings(FrostSickness.ID, STRINGS.DISEASES.FROSTSICKNESS.NAME, STRINGS.DISEASES.FROSTSICKNESS.DESCRIPTIVE_SYMPTOMS, STRINGS.DISEASES.FROSTSICKNESS.DESCRIPTION, STRINGS.DISEASES.FROSTSICKNESS.LEGEND_HOVERTEXT);
                BasicModUtils.MakeStrings(GasSickness.ID, STRINGS.DISEASES.GASSICKNESS.NAME, STRINGS.DISEASES.GASSICKNESS.DESCRIPTIVE_SYMPTOMS, STRINGS.DISEASES.GASSICKNESS.DESCRIPTION, STRINGS.DISEASES.GASSICKNESS.LEGEND_HOVERTEXT);
                BasicModUtils.MakeStrings(HungerSickness.ID, STRINGS.DISEASES.HUNGERSICKNESS.NAME, STRINGS.DISEASES.HUNGERSICKNESS.DESCRIPTIVE_SYMPTOMS, STRINGS.DISEASES.HUNGERSICKNESS.DESCRIPTION, STRINGS.DISEASES.HUNGERSICKNESS.LEGEND_HOVERTEXT);

                ExpandExposureTable();
            }

            public static void Postfix()
            {
                Db.Get().effects.Add(new Effect(FrostSickness.RECOVERY_ID, FrostSickness.RECOVERY_ID, FrostSickness.RECOVERY_ID, 1200, true, true, false));
                Db.Get().effects.Add(new Effect(GasSickness.RECOVERY_ID, GasSickness.RECOVERY_ID, GasSickness.RECOVERY_ID, 1200, true, true, false));
                Db.Get().effects.Add(new Effect(GasCureConfig.EffectID, GasCureConfig.EffectID, GasCureConfig.EffectID, 1200, true, true, false));
                Db.Get().effects.Add(new Effect(MudMaskConfig.EffectID, MudMaskConfig.EffectID, MudMaskConfig.EffectID, 1200, true, true, false));
                Db.Get().effects.Add(new Effect(MegaFeastConfig.EffectID, MegaFeastConfig.EffectID, MegaFeastConfig.EffectID, 3000, true, true, false));
            }

            public static void ExpandExposureTable()
            {
                List<ExposureType> exposureList = new List<ExposureType>();
                foreach (ExposureType et in TUNING.GERM_EXPOSURE.TYPES)
                    exposureList.Add(et);

                exposureList.Add(DiseasesExpanded_Patches_Frost.GetExposureType());
                exposureList.Add(DiseasesExpanded_Patches_Gas.GetExposureType());
                exposureList.Add(DiseasesExpanded_Patches_Bog.GetExposureType());
                exposureList.Add(DiseasesExpanded_Patches_Hunger.GetExposureType());

                TUNING.GERM_EXPOSURE.TYPES = exposureList.ToArray();
            }
        }
        [HarmonyPatch(typeof(Database.Sicknesses))]
        [HarmonyPatch(MethodType.Constructor)]
        [HarmonyPatch(new Type[] { typeof(ResourceSet) })]
        public class Sicknesses_Constructor_Patch
        {
            public static void Postfix(ref Database.Sicknesses __instance)
            {
                __instance.Add(new HungerSickness());
                __instance.Add(new BogSickness());
                __instance.Add(new FrostSickness());
                __instance.Add(new GasSickness());
            }
        }

        [HarmonyPatch(typeof(Diseases))]
        [HarmonyPatch(MethodType.Constructor)]
        [HarmonyPatch(new Type[] { typeof(ResourceSet), typeof(bool) })]
        public class Diseases_Constructor_Patch
        {
            public static void Prefix()
            {
                Assets.instance.DiseaseVisualization.info.Add(new DiseaseVisualization.Info() { name = HungerGerms.ID, overlayColourName = HungerGerms.ID });
                Assets.instance.DiseaseVisualization.info.Add(new DiseaseVisualization.Info() { name = BogInsects.ID, overlayColourName = BogInsects.ID });
                Assets.instance.DiseaseVisualization.info.Add(new DiseaseVisualization.Info() { name = FrostShards.ID, overlayColourName = FrostShards.ID });
                Assets.instance.DiseaseVisualization.info.Add(new DiseaseVisualization.Info() { name = GassyGerms.ID, overlayColourName = GassyGerms.ID });
            }

            public static void Postfix(ref Diseases __instance, bool statsOnly)
            {
                __instance.Add(new HungerGerms(statsOnly));
                __instance.Add(new BogInsects(statsOnly));
                __instance.Add(new FrostShards(statsOnly));
                __instance.Add(new GassyGerms(statsOnly));
            }
        }

        /*
        [HarmonyPatch(typeof(OverlayModes.Rooms))]
        [HarmonyPatch("GetCustomLegendData")]
        public static class Rooms_GetCustomLegendData_Patch
        {
            public static void Postfix()
            {
                foreach (MinionIdentity identity in Components.MinionIdentities)
                {
                    Debug.Log($"{identity.name} :");
                    MinionModifiers modifiers = identity.GetComponent<MinionModifiers>();
                    if (modifiers == null) return;
                    foreach (var v in modifiers.attributes.AttributeTable)
                        Debug.Log($"{v.Attribute.Id} = {v.GetTotalValue()}");
                }
            }
        }*/
    }
}
