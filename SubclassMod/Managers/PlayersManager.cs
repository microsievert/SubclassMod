using System;
using System.Linq;

using Exiled.API.Features;

using SubclassMod.Enums;
using SubclassMod.Patterns;

using PlayerRoles;
using UnityEngine;

namespace SubclassMod.Managers
{
    internal static class PlayersManager
    {
        internal static string GetRoleName(Player player, INamedRole namingData = null)
        {
            if (player.Role.Type == RoleTypeId.ClassD && SubclassMod.Instance.Config.ClassDNumbers)
            {
                if (namingData == null)
                    return $"{String.Format(SubclassMod.Instance.Translation.ClassDBadge, CalcNumericIdentify())}";

                return $"{namingData.NamePrefix}{String.Format(SubclassMod.Instance.Translation.ClassDBadge, CalcNumericIdentify())}{namingData.NamePostfix}";
            }
            
            if (namingData == null)
                return player.Nickname;
            
            switch (namingData.NamingMethod)
            {
                case NamingMethod.Firstname when SubclassMod.Instance.Config.HumanFirstNames.Any():
                    return $"{namingData.NamePrefix}{SubclassMod.Instance.Config.HumanFirstNames.RandomItem()}{namingData.NamePostfix} [{player.Nickname}]";
                case NamingMethod.Signs when SubclassMod.Instance.Config.HumanSpecialSigns.Any():
                    return $"{namingData.NamePrefix}{SubclassMod.Instance.Config.HumanSpecialSigns.RandomItem()}{namingData.NamePostfix} [{player.Nickname}]";
                default:
                    return player.Nickname;
            }
        }
        
        private static int CalcNumericIdentify()
        {
            int numIdentify;

            Player[] displayNamedPlayers = Player.List.Where(x => x.DisplayNickname != null).ToArray();

            do
                numIdentify = UnityEngine.Random.Range(1000, 9999); 
            while (displayNamedPlayers.Count(x => x.DisplayNickname.Contains(numIdentify.ToString())) != 0);

            return numIdentify;
        }
        
        internal static bool GetByMention(string mention, out Player[] players)
        {
            if (mention.Equals("*"))
            {
                players = Player.List.ToArray();
                return true; 
            }

            if (int.TryParse(mention, out int playerId))
            {
                players = new [] { Player.Get(playerId) };
                return true;
            }

            players = null;
            return false;
        }
    }
}