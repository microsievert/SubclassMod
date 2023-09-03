using System.Collections.Generic;
using System.ComponentModel;

using Exiled.API.Enums;
using Exiled.API.Interfaces;

using PlayerRoles;

using SubclassMod.Models;

namespace SubclassMod
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        public bool Debug { get; set; } = false;

        [Description("Replace roleplay names for class d to number identificators")]
        public bool ClassDNumbers { get; set; } = false;

        [Description("Chance to get subclass instead of overwritten role")]
        public float SubclassChance { get; set; } = 70f;

        [Description("Time while that will be shown spawn info broadcast after spawn")]
        public ushort SpawnBroadcastTime { get; set; } = 15;

        [Description("First names for human classes")]
        public List<string> HumanFirstNames { get; set; } = new List<string>();
        
        [Description("Special signs for human classes")]
        public List<string> HumanSpecialSigns { get; set; } = new List<string>();

        [Description("Additional info for roles")]
        public Dictionary<RoleTypeId, RoleInfo> CustomRolesInfo { get; set; } = new Dictionary<RoleTypeId, RoleInfo>
        {
            [RoleTypeId.FacilityGuard] = new RoleInfo(),
            [RoleTypeId.Scientist] = new RoleInfo()
        };
        
        [Description("List of subclasses for every base role. EVERY new custom subclass start from - (ID'S SHOULD BE UNIQUE)")]
        public List<SubclassInfo> CustomSubclasses { get; set; } = new List<SubclassInfo>();

        [Description("List of player's personal custom characters that can be selected by console command")]
        public List<CharacterInfo> CustomCharacters { get; set; } = new List<CharacterInfo>();

        [Description("List of rooms that won't be selected as player spawn room.")]
        public List<RoomType> RestrictedSpawnRooms { get; set; } = new List<RoomType>
        {
            RoomType.EzShelter,
            RoomType.EzCollapsedTunnel,
            RoomType.EzVent,
            RoomType.Lcz330,
            RoomType.Lcz173,
            RoomType.Lcz914,
            RoomType.LczArmory
        };
    }
}