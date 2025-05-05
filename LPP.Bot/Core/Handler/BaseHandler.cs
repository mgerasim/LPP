namespace LPP.Bot.Core.Handler
{
    public abstract class BaseHandler
    {
        protected CurrentUserState userState;

        public BaseHandler(CurrentUserState userState)
        {
            this.userState = userState;
        }

        public abstract Task RunAsync();
    }
}
