using System;
using STRINGS;
using UnityEngine;
using System.Collections.Generic;
using Database;

// file added by Alakad
namespace RoomsExpanded
{
    class RoomTypeFisheryData : RoomTypeAbstractData
    {
        public static readonly string RoomId = "CreaturePen";

        public RoomTypeFisheryData()
        {
            Id = RoomId;
            Name = STRINGS.ROOMS.TYPES.FISHERY.NAME;
            Tooltip = STRINGS.ROOMS.TYPES.FISHERY.TOOLTIP;
            Effect = STRINGS.ROOMS.TYPES.FISHERY.EFFECT;
            Catergory = Db.Get().RoomTypeCategories.Agricultural;
            ConstraintPrimary = new RoomConstraints.Constraint((Func<KPrefabID, bool>)(bc => bc.HasTag(RoomConstraintTags.FishingStationTag)),
                                                               (Func<Room, bool>)null,
                                                               name: STRINGS.ROOMS.CRITERIA.FISHINGSTATION.NAME,
                                                               description: STRINGS.ROOMS.CRITERIA.FISHINGSTATION.DESCRIPTION);                  
            ConstrantsAdditional = new RoomConstraints.Constraint[4]
            {
                new RoomConstraints.Constraint((Func<KPrefabID, bool>)(bc => bc.HasTag(RoomConstraintTags.AquariumReleaseTag)),
                                               (Func<Room, bool>)null,
                                               name: STRINGS.ROOMS.CRITERIA.FISHRELEASE.NAME,
                                               description: STRINGS.ROOMS.CRITERIA.FISHRELEASE.DESCRIPTION),
                new RoomConstraints.Constraint((Func<KPrefabID, bool>)(bc => bc.HasTag(RoomConstraintTags.AquariumFeederTag)),
                                               (Func<Room, bool>)null,
                                               name: STRINGS.ROOMS.CRITERIA.FISHFEEDER.NAME,
                                               description: STRINGS.ROOMS.CRITERIA.FISHFEEDER.DESCRIPTION),
                RoomConstraints.MINIMUM_SIZE_12,
                RoomConstraintTags.GetMaxSizeConstraint(Settings.Instance.Fishery.MaxSize)
            };

            RoomDetails = new RoomDetails.Detail[2]
            {
                new RoomDetails.Detail((Func<Room, string>)(room => string.Format((string)ROOMS.DETAILS.SIZE.NAME, (object)room.cavity.numCells))),
                new RoomDetails.Detail((Func<Room, string>)(room => string.Format((string)ROOMS.DETAILS.CREATURE_COUNT.NAME, (object)(room.cavity.creatures.Count + room.cavity.eggs.Count))))
            };

            Priority = 0;
            Upgrades = null;
            SingleAssignee = false;
            PriorityUse = false;
            Effects = null;
            SortKey = SortingCounter.GetAndIncrement(SortingCounter.FarmSortKey);
        }
        
    }
}
