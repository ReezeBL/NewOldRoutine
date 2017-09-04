using System.Threading.Tasks;
using System.Windows.Controls;
using log4net;
using Loki.Bot;
using Loki.Common;
using Loki.Game;
using Loki.Game.Objects;

namespace NewOldRoutine
{
    public abstract class SkillLogicProvider : IAuthored, IConfigurable, IMessageHandler
    {
        protected static ILog Log = Logger.GetLoggerInstanceForType();

        #region Implementation of IAuthored

        public abstract string Name { get; }
        public abstract string Author { get; }
        public abstract string Description { get; }
        public abstract string Version { get; }

        #endregion

        #region Implementation of IConfigurable

        public virtual UserControl Control => null;
        public virtual JsonSettings Settings => null;

        #endregion

        #region Implementation of IMessageHandler

        public virtual MessageResult Message(Message message)
        {
            return MessageResult.Unprocessed;
        }

        #endregion

        #region Skill Wrapping

        public Skill LinkedSkill { get; set; }

        protected bool UseSkillAt(Vector2i position, bool inPlace = false)
        {
            if(LinkedSkill == null || !LinkedSkill.CanUse())
                return false;
            var uaerr = LokiPoe.InGameState.SkillBarHud.UseAt(LinkedSkill.Slot, inPlace, position);
            if (uaerr == LokiPoe.InGameState.UseResult.None) return true;

            Log.ErrorFormat("[Logic] UseAt returned {0} for {1}.", uaerr, LinkedSkill.Name);
            return false;
        }

        protected bool UseSkillOn(NetworkObject target, bool inPlace = false)
        {
            if (LinkedSkill == null || !LinkedSkill.CanUse())
                return false;
            var uaerr = LokiPoe.InGameState.SkillBarHud.UseOn(LinkedSkill.Slot, inPlace, target);
            if (uaerr == LokiPoe.InGameState.UseResult.None) return true;

            Log.ErrorFormat("[Logic] UseAt returned {0} for {1}.", uaerr, LinkedSkill.Name);
            return false;
        }

        protected bool UseSkill(bool inPlace = false)
        {
            if (LinkedSkill == null || !LinkedSkill.CanUse())
                return false;
            var uaerr = LokiPoe.InGameState.SkillBarHud.Use(LinkedSkill.Slot, inPlace);
            if (uaerr == LokiPoe.InGameState.UseResult.None) return true;

            Log.ErrorFormat("[Logic] UseAt returned {0} for {1}.", uaerr, LinkedSkill.Name);
            return false;
        }

        #endregion

        public virtual bool SkillEvaluator(Skill skill)
        {
            return true;
        }

        public bool Enabled { get; set; }
        public abstract Task<bool> PreCombatHandling();
        public abstract Task<bool> CombatHandling(NetworkObject target);
    }
}
