using Exiled.API.Interfaces;

namespace SubclassMod
{
    public class Translation : ITranslation
    {
        public string SpawnDescriptionInfo { get; private set; } = "Your name: <color=green>{0}</color> \n You are: <color=red>{1}</color> \n Description: <color=grey><i>{2}</i></color>";
        public string ClassDBadge { get; private set; } = "D-{0}";
    }
}