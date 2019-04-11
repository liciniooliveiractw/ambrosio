using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AmbrosioBot.Dialogs.Enei;
using AmbrosioBot.Utils;
using Microsoft.Bot.Builder.Dialogs;

namespace AmbrosioBot.Dialogs.TellTime
{
    public class TellTimeDialog : ComponentDialog
    {
        private ResponseTokens _responder = new ResponseTokens();

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

        private async Task<DialogTurnResult> TellTime(WaterfallStepContext sc, CancellationToken cancellationToken)
        {
            var tokens = new Dictionary<string, string>();
            var currentTime = DateTime.Now.ToString("h:mm tt");
            tokens.Add("Time", currentTime);

            await _responder.ReplyWithTokens(sc.Context, TellTimeResponses.ResponseIds.TellTimeMessage, tokens);
            return await sc.EndDialogAsync();
        }
    }
}

