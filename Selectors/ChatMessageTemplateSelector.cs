using ChatApp.Models;

namespace ChatApp.Selectors
{
    class ChatMessageTemplateSelector : DataTemplateSelector
    {
        public DataTemplate MyMessageTemplate { get; set; }
        public DataTemplate OtherMessageTemplate { get; set; }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var message = item as ChatMessage;
            return message?.IsMyMessage == true ? MyMessageTemplate : OtherMessageTemplate;
        }
    }
}
