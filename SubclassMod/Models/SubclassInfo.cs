using System.Collections.Generic;

using Exiled.API.Enums;

using PlayerRoles;

using SubclassMod.Enums;
using SubclassMod.Patterns;

using UnityEngine;

namespace SubclassMod.Models
{
    public class SubclassInfo : INamedRole
    {
        public int Id { get; set; } = 0;
        public int MaxPlayers { get; set; } = 0;
        public float Health { get; set; } = 100f;
        public float SpawnPercent { get; set; } = 50f;

        public string Name { get; set; } = "Unknown subclass";
        public string Description { get; set; } = "Description of unknown subclass";
        public string NamePrefix { get; set; } = "Unk.";
        public string NamePostfix { get; set; } = "!";
        public string CustomInfo { get; set; } = "Unknown Person";
        
        public bool ForceclassOnly { get; set; } = false;

        public RoleTypeId BaseRole { get; set; } = RoleTypeId.ClassD;
        public SpawnMethod SpawnMethod { get; set; } = SpawnMethod.SpawnZone;
        public NamingMethod NamingMethod { get; set; } = NamingMethod.Nickname;

        public Dictionary<AmmoType, ushort> Ammo { get; set; } = new Dictionary<AmmoType, ushort>
        {
            [AmmoType.Nato9] = 10,
            [AmmoType.Nato556] = 10,
            [AmmoType.Nato762] = 10
        };

        public List<int> CustomItems { get; set; } = new List<int>();
        
        public List<ItemType> Items { get; set; } = new List<ItemType>();
        public List<ZoneType> SpawnZones { get; set; } = new List<ZoneType>();
        public List<RoomType> SpawnRooms { get; set; } = new List<RoomType>();
        public List<Vector3> SpawnPositions { get; set; } = new List<Vector3>();
    }
}