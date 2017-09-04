using Loki;
using Loki.Common;

namespace NewOldRoutine
{
    public class GeneralSettings :JsonSettings
    {
        private static GeneralSettings instance;
        public static GeneralSettings Instance => instance ?? (instance = new GeneralSettings());

        public static SkillLogicProvider[] Providers { get; set; }

        public GeneralSettings() : base(GetSettingsFilePath(Configuration.Instance.Name, "NewOldRoutine", "Core.json"))
        {

        }
    }
}
