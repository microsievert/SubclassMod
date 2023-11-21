using System;

using Exiled.API.Features;

using SubclassMod.Events;

namespace SubclassMod
{
    public class SubclassMod : Plugin<Config, Translation>
    {
        public static SubclassMod Instance;

        public override string Name => "SubclassMod";
        public override string Author => "microsievert";
        
        public override Version Version { get; } = new Version(3, 0, 0);
        public override Version RequiredExiledVersion { get; } = new Version(8, 4, 2);

        private PlayerHandlers _playerHandlers;

        public override void OnEnabled() 
        {
            Instance = this;
            
            RegisterEvents();

            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            Instance = null;
            
            UnregisterEvents();
            
            base.OnDisabled();
        }
        

        private void RegisterEvents()
        {
            _playerHandlers = new PlayerHandlers();

            Exiled.Events.Handlers.Player.Spawned += _playerHandlers.OnSpawned;
            Exiled.Events.Handlers.Player.ChangingRole += _playerHandlers.OnChangingRole;
        }

        private void UnregisterEvents()
        {
            Exiled.Events.Handlers.Player.Spawned -= _playerHandlers.OnSpawned;
            Exiled.Events.Handlers.Player.ChangingRole -= _playerHandlers.OnChangingRole;

            _playerHandlers = null;
        }
    }
}