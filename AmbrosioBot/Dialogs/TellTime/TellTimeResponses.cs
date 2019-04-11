using Microsoft.Bot.Builder.TemplateManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AmbrosioBot.Dialogs.Enei.Resources;
using AmbrosioBot.Dialogs.TellTime.Resources;
using AmbrosioBot.Utils;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

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
