using Exiled.API.Features;

using SubclassMod.Models;

using UnityEngine;

namespace SubclassMod.Components
{
    internal class SubclassedPlayer : MonoBehaviour
    {
        public ReferenceHub Hub { get; set; }
        public RoleInfo ActiveRole { get; set; }
        public SubclassInfo ActiveSubclass { get; set; }

        private void OnDestroy()
        {
            Player owner = Player.Get(transform.gameObject);

            owner.DisplayNickname = null;
            owner.CustomInfo = string.Empty;
        }
    }
}