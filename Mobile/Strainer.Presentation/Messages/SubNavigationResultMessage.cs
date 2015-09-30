using Cirrious.MvvmCross.Plugins.Messenger;

namespace Strainer.Messages
{
    public class SubNavigationResultMessage<TResult> : MvxMessage
    {
        public TResult Result { get; private set; }
        public string MessageId { get; set; }

        public SubNavigationResultMessage(object sender, string messageId, TResult result)
            : base(sender)
        {
            Result = result;
            MessageId = messageId;
        }
    }
}