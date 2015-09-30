using Cirrious.MvvmCross.Plugins.Messenger;

namespace Strainer.Messages
{
    public class AppActivated : MvxMessage
    {
        public AppActivated(object sender)
            : base(sender)
        {
        }
    }
}