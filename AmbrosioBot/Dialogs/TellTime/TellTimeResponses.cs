using AmbrosioBot.Dialogs.TellTime.Resources;
using AmbrosioBot.Utils;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.TemplateManager;
using Microsoft.Bot.Schema;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AmbrosioBot.Dialogs.TellTime
{
    public class TellTimeResponses : TemplateManager
    {
        private static LanguageTemplateDictionary _responseTemplates = new LanguageTemplateDictionary
        {
            ["default"] = new TemplateIdMap
            {
                { ResponseIds.TellTimeMessage, (context, data) =>
                    MessageFactory.Text(
                        text: TellTimeStrings.TellTimeResponse,
                        ssml: TellTimeStrings.TellTimeResponse,
                        inputHint: InputHints.ExpectingInput) },
            }
        };

        public TellTimeResponses()
        {
            Register(new DictionaryRenderer(_responseTemplates));
        }

        public class ResponseIds
        {
            public const string TellTimeMessage = "TellTimeResponse";
        }
    }
}