using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Loki.Game;
using Loki.Game.Objects;

namespace NewOldRoutine.DataModels
{
    /// <summary>
    /// View for SkillLogicProvider to display it in UI
    /// </summary>
    public class SkillProviderView : INotifyPropertyChanged
    {
        public SkillLogicProvider LogicProvider { get; }

        private SkillEntry skillEntry;
       
        private static bool IsCastable(Skill skill)
        {
            return skill.IsCastable;
        }

        public SkillProviderView(SkillLogicProvider logicProvider)
        {
            LogicProvider = logicProvider;
            skillEntry = new SkillEntry(logicProvider.LinkedSkill);
        }

        /// <summary>
        /// Enables or disables corresponding SkillLogicProvider
        /// </summary>
        public bool Enabled
        {
            get => LogicProvider.Enabled;
            set
            {
                if (value && LogicProvider.LinkedSkill == null)
                    return;
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

        public ICollection<SkillEntry> PossibleEntries => LokiPoe.InGameState.SkillBarHud.SkillBarSkills.Where(IsCastable).Where(LogicProvider.SkillEvaluator).Select(skill => new SkillEntry(skill)).ToList();

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Forces UI to update its skill list
        /// </summary>
        public void UpdateSkillList()
        {
            OnPropertyChanged(nameof(PossibleEntries));
        }

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
