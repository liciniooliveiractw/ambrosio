using AmbrosioBot.Dialogs.Enei.Resources;

namespace AmbrosioBot.Dialogs.Enei
{
    using global::AmbrosioBot.Dialogs.Enei.Resources;
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.TemplateManager;
    using Microsoft.Bot.Schema;


    public class EneiResponses : TemplateManager
    {
        private static LanguageTemplateDictionary _responseTemplates = new LanguageTemplateDictionary
        {
            ["default"] = new TemplateIdMap
            {
                { ResponseIds.WelcomeEneiMessage, (context, data) =>
                    MessageFactory.Text(
                        text: EneiStrings.WelcomeEneiMessage,
                        ssml: EneiStrings.WelcomeEneiMessage,
                        inputHint: InputHints.ExpectingInput) },
            }
        };

        public EneiResponses()
        {
            Register(new DictionaryRenderer(_responseTemplates));
        }

        public class ResponseIds
        {
            public const string WelcomeEneiMessage = "WelcomeEneiMessage";
        }
    }
}