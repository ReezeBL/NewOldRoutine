using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using log4net;
using Loki.Bot;
using Loki.Common;

// ReSharper disable once CheckNamespace
namespace NewOldRoutine
{
    public class NewOldRoutine : IRoutine
    {
        private static readonly ILog Log = Logger.GetLoggerInstanceForType();

        private Gui gui;
        private SkillLogicProvider[] providers = Array.Empty<SkillLogicProvider>();

        public UserControl Control => gui ?? (gui = new Gui());
        public JsonSettings Settings => GeneralSettings.Instance;

        public MessageResult Message(Message message)
        {
            if (providers.Where(logicProvider => logicProvider.Enabled)
                .Any(logicProvider => logicProvider.Message(message) == MessageResult.Processed))
                return MessageResult.Processed;
            return MessageResult.Unprocessed;
        }

        public void Start()
        {
            
        }

        public void Stop()
        {
            
        }

        public void Tick()
        {
            
        }

        public string Name => "NewOldRoutine";
        public string Author => "Siamant";
        public string Description => "One routine to rule them all";
        public string Version => "0.0.1";

        public void Initialize()
        {
            var baseType = typeof(SkillLogicProvider);
            providers = baseType
                .Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(baseType) && !t.IsAbstract && t.GetConstructor(Type.EmptyTypes) != null)
                .Select(t => (SkillLogicProvider)Activator.CreateInstance(t))
                .ToArray();

            GeneralSettings.Providers = providers;
        }

        public void Deinitialize()
        {
            
        }

        public async Task<LogicResult> Logic(Logic logic)
        {
            if (logic.Id != "hook_combat")
                return LogicResult.Unprovided;

            foreach (var provider in providers)
            {
                if (!provider.Enabled) continue;
                if (await provider.PreCombatHandling())
                    return LogicResult.Provided;
            }

            return LogicResult.Unprovided;
        }
    }
}