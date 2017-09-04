using System.Threading.Tasks;
using Loki.Bot;
using Loki.Game.Objects;

namespace NewOldRoutine.SkillLogic
{
    public class ExampleLogicProvider : SkillLogicProvider
    {
        public override string Name => "Example Logic Provider";
        public override string Author { get; }
        public override string Description { get; }
        public override string Version { get; }

        public override async Task<bool> PreCombatHandling()
        {
            return false;
        }

        public override async Task<bool> CombatHandling(NetworkObject target)
        {
            return false;
        }
    }
}