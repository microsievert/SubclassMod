using Exiled.Events.EventArgs.Player;

using SubclassMod.Managers;
using SubclassMod.Components;

using UnityEngine;

namespace SubclassMod.Events
{
    public class PlayerHandlers
    {
        public void OnSpawned(SpawnedEventArgs ev) => SubclassesManager.AssignPlayer(ev.Player, ev.Player.Role.Type);

        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            if (ev.Player.GameObject.TryGetComponent(out SubclassedPlayer subclassedPlayerComponent))
                Object.Destroy(subclassedPlayerComponent);
        }
    }
}