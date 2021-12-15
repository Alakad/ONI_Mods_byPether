using HarmonyLib;
using UnityEngine;
using Database;
using STRINGS;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Runtime.CompilerServices;
using ButcherStation;

namespace RoomsExpanded
{
    class RoomsExpanded_Patches_Fishery
    {
        public static void AddRoom(ref RoomTypes __instance)
        {
            if (Settings.Instance.Fishery.IncludeRoom)
            {   
                __instance.Add(RoomTypes_AllModded.Fishery);

                if (Settings.Instance.Agricultural.IncludeRoom)
                {
                    RoomConstraintTags.AddStompInConflict(RoomTypes_AllModded.Fishery.primary_constraint, __instance.CreaturePen.primary_constraint);
                    
                    foreach (RoomConstraints.Constraint AgriculturalConstraint in __instance.CreaturePen.additional_constraints)
                    {
                        foreach (RoomConstraints.Constraint FisheryConstraint in RoomTypes_AllModded.Fishery.additional_constraints)
                        {
                            if (FisheryConstraint == AgriculturalConstraint)
                                RoomConstraintTags.AddStompInConflict(FisheryConstraint,AgriculturalConstraint);    
                        }
                    }
                }

                if (Settings.Instance.Aquarium.IncludeRoom)
                {
                    RoomConstraintTags.AddStompInConflict(RoomTypes_AllModded.Fishery.primary_constraint,RoomTypes_AllModded.Aquarium.primary_constraint);

                    foreach (RoomConstraints.Constraint AquariumConstraint in RoomTypes_AllModded.Aquarium.additional_constraints)
                    {
                        foreach (RoomConstraints.Constraint FisheryConstraint in RoomTypes_AllModded.Fishery.additional_constraints)
                        {
                            if (FisheryConstraint == AquariumConstraint)
                                RoomConstraintTags.AddStompInConflict(FisheryConstraint,AquariumConstraint);    
                        }
                    }
                }
            }
        }
        
        [HarmonyPatch(typeof(FishingStationConfig))]
        [HarmonyPatch("ConfigureBuildingTemplate")]
        public static class FishingStationConfig_ConfigureBuildingTemplate_Patch
        {
            public static void Postfix(GameObject go)
            {
                if (!Settings.Instance.Fishery.IncludeRoom) return;
                //go.GetComponent<KPrefabID>().RemoveTag(RoomConstraints.ConstraintTags.RanchStation);
                go.GetComponent<KPrefabID>().AddTag(RoomConstraintTags.FishingStationTag);
            }
        }

        [HarmonyPatch(typeof(ColonyAchievement))]
        [HarmonyPatch(MethodType.Constructor)]
        [HarmonyPatch(new Type[] { typeof(string), typeof(string), typeof(string), typeof(string), typeof(bool), typeof(List<ColonyAchievementRequirement>), typeof(string), typeof(string), typeof(string), typeof(string), typeof(System.Action<KMonoBehaviour>), typeof(string), typeof(string), typeof(string[]) })]
        public static class ColonyAchievement_Constructor_Patch
        {
            public static void Prefix(string platformAchievementId, ref List<ColonyAchievementRequirement> requirementChecklist)
            {
                if (!Settings.Instance.Fishery.IncludeRoom) return;
                if (platformAchievementId != "VARIETY_OF_ROOMS") return;

                ColonyAchievementRequirement delete = null;
                foreach(var req in requirementChecklist)
                {
                    RoomType roomType = Traverse.Create(req).Field("roomType").GetValue<RoomType>();
                    if (roomType.Id == "Farm")
                        delete = req;
                }
                if (delete != null)
                {
                    requirementChecklist.Remove(delete);
                    Debug.Log("RoomsExpanded: VARIETY_OF_ROOMS - removed Farm requirement");
                }
            }
        }
    }
}