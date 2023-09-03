using System;
using System.Linq;

using CommandSystem;

using Exiled.API.Features;

using SubclassMod.Managers;
using SubclassMod.Models;

namespace SubclassMod.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class SelectCharacter : ICommand
    {
        public string Command => "character-select";
        public string Description => String.Empty;

        public string[] Aliases => Array.Empty<string>();

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!arguments.Any())
            {
                response = "At least 1 argument required (ID)";
                return false;
            }

            if (!int.TryParse(arguments.At(0), out int characterId))
            {
                response = "Incorrect argument format. Expected number (CHARACTER ID)";
                return false;
            }

            if (!TryFindCharacter(characterId, out CharacterInfo character))
            {
                response = "No characters associated with that ID. Try again.";
                return false;
            }

            Player caller = Player.Get(sender);
            
            if (!character.AllowedUsers.Contains(caller.UserId))
            {
                response = "Error. You don't permitted to spawn as this character";
                return false;
            }
            
            SubclassesManager.SpawnCustomCharacter(caller, character);

            response = String.Empty;
            return false;
        }

        private bool TryFindCharacter(int id, out CharacterInfo character)
        {
            if (!SubclassMod.Instance.Config.CustomCharacters.Any())
            {
                character = null;
                return false;
            }
            
            CharacterInfo targetCharacter = SubclassMod.Instance.Config.CustomCharacters.First(x => x.Id == id);

            character = targetCharacter;
            return true;
        }
    }
}