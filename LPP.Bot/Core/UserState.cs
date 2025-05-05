
namespace LPP.Bot.Core
{
    public class UserState
    {
        public string State { get; set; } = "";
        public string CurrentStep { get; set; } = "";

        internal void Clear()
        {
            this.State = "";
            this.CurrentStep = "";
        }
    }
}
