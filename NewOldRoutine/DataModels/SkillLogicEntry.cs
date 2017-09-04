using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Loki.Common;
using Loki.Game;
using Loki.Game.Objects;

namespace NewOldRoutine.DataModels
{
    public class SkillLogicEntry : INotifyPropertyChanged
    {
        public SkillLogicProvider LogicProvider { get; }
        private readonly ObservableCollection<SkillEntry> possibleSkillEntries = new ObservableCollection<SkillEntry>();

        private SkillEntry skillEntry;

        private static bool IsCastable(Skill skill)
        {
            return skill.IsCastable;
        }

        public SkillLogicEntry(SkillLogicProvider logicProvider)
        {
            LogicProvider = logicProvider;
            skillEntry = new SkillEntry(logicProvider.LinkedSkill);
            LokiPoe.InGameState.SkillBarHud.SkillBarSkills.Where(IsCastable).Where(logicProvider.SkillEvaluator).Select(skill => new SkillEntry(skill)).ForEach(possibleSkillEntries.Add);
        }

        public bool Enabled
        {
            get => LogicProvider.Enabled;
            set
            {
                LogicProvider.Enabled = value;
                OnPropertyChanged();
            }
        }

        public string Name => LogicProvider.Name;

        public SkillEntry Skill
        {
            get => skillEntry;
            set
            {
                skillEntry = value;
                LogicProvider.LinkedSkill = skillEntry.Skill;
            }
        }

        public ObservableCollection<SkillEntry> PossibleEntries => possibleSkillEntries;

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
