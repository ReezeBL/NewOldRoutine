using System.Collections.ObjectModel;
using Loki;
using Loki.Common;
using NewOldRoutine.DataModels;

namespace NewOldRoutine
{
    public class GeneralSettings :JsonSettings
    {
        private static GeneralSettings instance;
        public static GeneralSettings Instance => instance ?? (instance = new GeneralSettings());

        public static SkillLogicProvider[] Providers { get; set; }
        public int CombatRange { get; set; } = 70;

        public static ObservableCollection<SkillProviderView> ProviderWrappers { get; } =
            new ObservableCollection<SkillProviderView>();

        public GeneralSettings() : base(GetSettingsFilePath(Configuration.Instance.Name, "NewOldRoutine", "Core.json"))
        {

        }
    }
}
