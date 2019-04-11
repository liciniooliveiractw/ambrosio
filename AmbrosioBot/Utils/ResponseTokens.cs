using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.TemplateManager;
using Microsoft.Bot.Schema;

namespace AmbrosioBot.Utils
{
    public class ResponseTokens : TemplateManager
    {
        public string ReplaceToken(string original, IDictionary<string,string> tokens)
        {
            string answer = string.Copy(original);
            return tokens.Select(a => answer = answer.Replace(string.Concat("{", a.Key, "}"), a.Value)).Last();
        }

        public async Task ReplyWithTokens(ITurnContext turnContext, string templateId, IDictionary<string, string> tokens, object data = null)
        {
            BotAssert.ContextNotNull(turnContext);

            // apply template
            Activity boundActivity = await RenderTemplate(turnContext, turnContext.Activity?.AsMessageActivity()?.Locale, templateId, data).ConfigureAwait(false);
            boundActivity.Text = ReplaceToken(boundActivity.Text, tokens);
            boundActivity.Speak = ReplaceToken(boundActivity.Speak, tokens);
            if (boundActivity != null)
            {
                await turnContext.SendActivityAsync(boundActivity);
                return;
            }
            return;
        }
    }
}
