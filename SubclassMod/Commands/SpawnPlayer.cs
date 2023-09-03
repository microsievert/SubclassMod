using System;
using System.Linq;

using CommandSystem;

using Exiled.API.Features;
using Exiled.Permissions.Extensions;

using SubclassMod.Managers;

using MEC;

using SubclassMod.Models;

namespace SubclassMod.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class SpawnPlayer : ICommand
    {
        public string Command => "force";
        public string Description => "Usage: force <id/*> <roleId>";

        public string[] Aliases { get; } = Array.Empty<string>();

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("scmod.force"))
            {
                response = "Error. You don't have permissions to use this command";
                return false;
            }

            if (arguments.Count < 2)
            {
                response = "Error. Minimum two arguments required.";
                return false;
            }

            string[] args = arguments.ToArray();

            if (PlayersManager.GetByMention(args[0], out Player[] targetPlayers) && int.TryParse(args[1], out int roleId))
            {
                if (!SubclassesManager.TryGetSubclasses(roleId, out SubclassInfo[] subclasses))
                {
                    response = "Failed to find subclass with that id. Try again";
                    return false;
                }

                SubclassInfo targetSubclass = subclasses.First();

                foreach (Player targetPlayer in targetPlayers)
                    SubclassesManager.AssignPlayer(targetPlayer, targetSubclass);

                response = $"Player successfully forced as {targetSubclass.Name}";
                return true;
            }

            response = "Failed to find player(s) with that id or parse role id. Try again";
            return false;
        }
    }
}