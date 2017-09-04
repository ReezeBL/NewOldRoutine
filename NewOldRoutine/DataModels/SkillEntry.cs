using Loki.Game.GameData;
using Loki.Game.Objects;

namespace NewOldRoutine.DataModels
{
    public class SkillEntry
    {
        public Skill Skill { get; }

        public SkillEntry(Skill skill)
        {
            Skill = skill;
        }

        public InventorySlot Slot => Skill.InventorySlot;

        public int SocketIndex => Skill.SocketIndex;

        public string Name => $"{Skill.Name} [{Slot}: {SocketIndex}]";

        public override string ToString()
        {
            return Skill != null ? Name : "None";
        }
    }
}
