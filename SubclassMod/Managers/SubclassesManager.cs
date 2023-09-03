using System.Collections.Generic;
using System.Linq;

using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.CustomItems.API.Features;

using SubclassMod.Enums;
using SubclassMod.Components;

using UnityEngine;

using MEC;

using PlayerRoles;

using SubclassMod.Models;

using Random = UnityEngine.Random;

namespace SubclassMod.Managers
{
    internal static class SubclassesManager
    {
        private static readonly List<string> IgnoredPlayers = new List<string>();
        private static readonly Dictionary<SubclassInfo, byte> SubclassedCounter = new Dictionary<SubclassInfo, byte>();

        /// <summary>
        /// Forces player as randomly selected subclass.
        /// </summary>
        /// <param name="player">Target player.</param>
        /// <param name="role">Target base role.</param>
        internal static void AssignPlayer(Player player, RoleTypeId role) => 
            Timing.RunCoroutine(InternalAssignPlayer(player, role));

        /// <summary>
        /// Forces player as specified subclass.
        /// </summary>
        /// <param name="player">Target player.</param>
        /// <param name="subclass">Target subclass info.</param>
        internal static void AssignPlayer(Player player, SubclassInfo subclass) =>
            Timing.RunCoroutine(ForceAsSubclass(player, subclass));
        
        /// <summary>
        /// Forces player as custom character.
        /// </summary>
        /// <param name="player">Target player.</param>
        /// <param name="character">Target character info.</param>
        internal static void SpawnCustomCharacter(Player player, CharacterInfo character) =>
            Timing.RunCoroutine(ForceAsCharacter(player, character));

        /// <summary>
        /// Getting subclass by specified subclass id.
        /// </summary>
        /// <param name="id">Subclass ID for searching.</param>
        /// <param name="subclasses">Reference to result array.</param>
        /// <returns></returns>
        internal static bool TryGetSubclasses(int id, out SubclassInfo[] subclasses)
        {
            SubclassInfo[] targetSubclasses =
                SubclassMod.Instance.Config.CustomSubclasses.Where(x => x.Id == id).ToArray();

            if (targetSubclasses.Length == 0)
            {
                subclasses = null;
                return false;
            }

            subclasses = targetSubclasses;
            return true;
        }
        
        private static IEnumerator<float> InternalAssignPlayer(Player player, RoleTypeId role)
        {
            yield return Timing.WaitForSeconds(0.35f);

            if (IgnoredPlayers.Remove(player.UserId))
                yield break;

            if (role.Equals(RoleTypeId.Tutorial) || player.GameObject.TryGetComponent<SubclassedPlayer>(out _))
                yield break;

            if (TryGetSubclasses(role, out SubclassInfo[] targetSubclasses))
            {
                if (Random.Range(0f, 100f) <= SubclassMod.Instance.Config.SubclassChance && SubclassMod.Instance.Config.CustomRolesInfo.TryGetValue(role, out RoleInfo targetRole))
                {
                    Timing.RunCoroutine(ForceAsOverriddenRole(player, targetRole));
                    
                    yield break;
                }

                Timing.RunCoroutine(ForceAsSubclass(player, targetSubclasses.RandomItem()));

                yield break;
            }
            
            if (SubclassMod.Instance.Config.CustomRolesInfo.TryGetValue(role, out RoleInfo targetOverriddenRole))
            {
                Timing.RunCoroutine(ForceAsOverriddenRole(player, targetOverriddenRole));
                
                yield break;
            }

            player.DisplayNickname = PlayersManager.GetRoleName(player);
        }

        private static bool TryGetSubclasses(RoleTypeId role, out SubclassInfo[] subclasses)
        {
            SubclassInfo[] roleSubclasses = SubclassMod.Instance.Config.CustomSubclasses.Where(x => x.BaseRole == role && !x.ForceclassOnly && IsSubclassFree(x)).ToArray();

            if (roleSubclasses.Length == 0)
            {
                subclasses = null;
                return false;
            }

            subclasses = roleSubclasses;
            return true;
        }
        
        private static IEnumerator<float> ForceAsSubclass(Player player, SubclassInfo subclassInfo)
        {
            RoleTypeId initialRole = player.Role.Type;

            if (!initialRole.Equals(player.Role.Type))
                yield break;
            
            if (player.Role.Type != subclassInfo.BaseRole)
                player.RoleManager.ServerSetRole(subclassInfo.BaseRole, RoleChangeReason.Respawn);

            SubclassedPlayer subclassPlayer = player.GameObject.AddComponent<SubclassedPlayer>();
                
            subclassPlayer.ActiveSubclass = subclassInfo;

            switch (subclassInfo.SpawnMethod)
            {
                case SpawnMethod.SpawnPositions:
                    player.Position = subclassInfo.SpawnPositions.RandomItem();
                        
                    break;
                case SpawnMethod.SpawnRooms:
                    Room targetRoom = Room.Get(subclassInfo.SpawnRooms.RandomItem()) ??
                                      Room.Get(RoomType.EzIntercom);

                    player.Position = targetRoom.Position + Vector3.up;
                        
                    break;
                case SpawnMethod.SpawnZone:
                    ZoneType targetZone = subclassInfo.SpawnZones.RandomItem();
                        
                    player.Position = Room.List.Where(x => x.Zone == targetZone && !SubclassMod.Instance.Config.RestrictedSpawnRooms.Contains(x.Type)).ToList().RandomItem().Position + Vector3.up;
                        
                    break;
            }
                
            player.Health = subclassInfo.Health;
                
            player.ClearInventory();
                
            foreach (ItemType item in subclassInfo.Items)
                player.AddItem(item);

            foreach (uint itemId in subclassInfo.CustomItems)
                CustomItem.TryGive(player, itemId, false);

            foreach (AmmoType type in subclassInfo.Ammo.Keys)
                player.AddAmmo(type, subclassInfo.Ammo[type]);

            player.DisplayNickname = PlayersManager.GetRoleName(player, subclassInfo);

            player.CustomInfo = subclassInfo.CustomInfo;

            BroadcastRole(player, player.DisplayNickname, subclassInfo.Name, subclassInfo.Description);

            if (SubclassedCounter.ContainsKey(subclassInfo))
                SubclassedCounter[subclassInfo] += 1;
            else 
                SubclassedCounter.Add(subclassInfo, 1);
        }

        private static IEnumerator<float> ForceAsOverriddenRole(Player player, RoleInfo roleInfo)
        {
            RoleTypeId initialRole = player.Role;

            if (!initialRole.Equals(player.Role.Type))
                yield break;

            SubclassedPlayer subclassPlayer = player.GameObject.AddComponent<SubclassedPlayer>();
            subclassPlayer.ActiveRole = roleInfo;

            player.DisplayNickname = PlayersManager.GetRoleName(player, roleInfo);
            player.CustomInfo = roleInfo.CustomInfo;

            if (roleInfo.InventoryOverridden)
            {
                player.ClearInventory();
                
                foreach (ItemType item in roleInfo.InventoryOverwrite)
                    player.AddItem(item);
                    
                foreach (uint itemId in roleInfo.InventoryCustomItems)
                    CustomItem.TryGive(player, itemId, false);
            }
        }

        private static IEnumerator<float> ForceAsCharacter(Player player, CharacterInfo characterInfo)
        {
            IgnoredPlayers.Add(player.UserId);

            yield return Timing.WaitForSeconds(0.15f);

            player.RoleManager.ServerSetRole(characterInfo.BaseRole, RoleChangeReason.Respawn);

            yield return Timing.WaitForSeconds(0.45f);

            Room spawnRoom = Room.Random(characterInfo.SpawnZone);

            player.Position = spawnRoom.Position + Vector3.up;

            player.ClearInventory();

            foreach (ItemType item in characterInfo.InventoryOverride)
                player.AddItem(item);
            
            foreach (uint itemId in characterInfo.InventoryCustomItems)
                CustomItem.TryGive(player, itemId, false);

            player.Scale = Vector3.one * characterInfo.Scale;

            player.CustomInfo = characterInfo.Info;
            player.DisplayNickname = characterInfo.Name;
        }

        private static bool IsSubclassFree(SubclassInfo subclassInfo)
        {
            if (subclassInfo.MaxPlayers.Equals(0))
                return true;
            
            if (Random.Range(0f, 100f) <= subclassInfo.SpawnPercent)
                return false;

            if (!SubclassedCounter.ContainsKey(subclassInfo))
                return true;

            return SubclassedCounter[subclassInfo] < subclassInfo.MaxPlayers;
        }

        private static void BroadcastRole(Player player, string roleName, string className, string classDescription) =>
            player.Broadcast(SubclassMod.Instance.Config.SpawnBroadcastTime, string.Format(SubclassMod.Instance.Translation.SpawnDescriptionInfo, roleName, className, classDescription));
    }
}