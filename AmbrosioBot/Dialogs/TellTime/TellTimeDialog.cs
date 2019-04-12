using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AmbrosioBot.Dialogs.Enei;
using AmbrosioBot.Utils;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;

namespace AmbrosioBot.Dialogs.TellTime
{
    public class TellTimeDialog : ComponentDialog
    {
        private TellTimeResponses _responder = new TellTimeResponses();

        public TellTimeDialog(BotServices services)
            : base(nameof(TellTimeDialog))
        {
            var dialogId = nameof(TellTimeDialog);

            var stepsDialogs = new WaterfallStep[]
            {
                TellTime,
            };

            AddDialog(new WaterfallDialog(dialogId, stepsDialogs));
        }
        
        public async Task ReplyWithTokens(
            ITurnContext turnContext, 
            string templateId, 
            IDictionary<string, string> tokens, 
            object data = null)
        {
            BotAssert.ContextNotNull(turnContext);

            // apply template
            var manager = new TellTimeResponses();
            var activity = await manager.RenderTemplate(turnContext, turnContext.Activity?.AsMessageActivity()?.Locale, templateId, data).
                ConfigureAwait(false);

            activity.Text = ResponseTokens.ReplaceToken(activity.Text, tokens);
            activity.Speak = ResponseTokens.ReplaceToken(activity.Speak, tokens);

            await turnContext.SendActivityAsync(activity);
        }
        
        private async Task<DialogTurnResult> TellTime(WaterfallStepContext sc, CancellationToken cancellationToken)
        {
            var tokens = new Dictionary<string, string>();
            var currentTime = DateTime.Now.ToString("h:mm tt");
            tokens.Add("Time", currentTime);

            await ReplyWithTokens(sc.Context, TellTimeResponses.ResponseIds.TellTimeMessage, tokens);
            return await sc.EndDialogAsync();
        }
    }
}

