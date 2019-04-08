namespace AmbrosioBot.Dialogs
{
    using Microsoft.Bot.Builder;
    using Microsoft.Bot.Builder.Dialogs;
    using Microsoft.Bot.Schema;
    using System.Threading;
    using System.Threading.Tasks;

    public abstract class RouterDialog : ComponentDialog
    {
        public RouterDialog(string dialogId)
            : base(dialogId)
        {
        }

        /// <summary>
        /// Called when [begin dialog asynchronous].
        /// </summary>
        /// <param name="innerDc">The inner dc.</param>
        /// <param name="options">The options.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        protected override Task<DialogTurnResult> OnBeginDialogAsync(DialogContext innerDc, object options, CancellationToken cancellationToken = default(CancellationToken))
        {
            return OnContinueDialogAsync(innerDc, cancellationToken);
        }

        /// <summary>
        /// Called when [continue dialog asynchronous].
        /// </summary>
        /// <param name="innerDc">The inner dc.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns></returns>
        protected override async Task<DialogTurnResult> OnContinueDialogAsync(DialogContext innerDc, CancellationToken cancellationToken = default(CancellationToken))
        {
            var activity = innerDc.Context.Activity;

            if (activity.IsStartActivity())
            {
                await OnStartAsync(innerDc);
            }

            switch (activity.Type)
            {
                case ActivityTypes.Message:
                    {
                        if (activity.Value != null)
                        {
                            await OnEventAsync(innerDc);
                        }
                        else if (!string.IsNullOrEmpty(activity.Text))
                        {
                            var result = await innerDc.ContinueDialogAsync();

                            switch (result.Status)
                            {
                                case DialogTurnStatus.Empty:
                                    {
                                        await RouteAsync(innerDc);
                                        // Waterfalls with no turns should Complete.
                                        if (innerDc.Stack.Count == 0)
                                            await CompleteAsync(innerDc);
                                        break;
                                    }

                                case DialogTurnStatus.Complete:
                                    {
                                        await CompleteAsync(innerDc);

                                        // End active dialog
                                        await innerDc.EndDialogAsync();
                                        break;
                                    }

                                default:
                                    {
                                        break;
                                    }
                            }
                        }

                        break;
                    }

                case ActivityTypes.Event:
                    {
                        await OnEventAsync(innerDc);
                        break;
                    }

                default:
                    {
                        await OnSystemMessageAsync(innerDc);
                        break;
                    }
            }

            return EndOfTurn;
        }


        /// <summary>
        /// Called when the inner dialog stack is empty.
        /// </summary>
        /// <param name="innerDc">The dialog context for the component.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected abstract Task RouteAsync(DialogContext innerDc, CancellationToken cancellationToken = default(CancellationToken));

        /// <summary>
        /// Called when the inner dialog stack is complete.
        /// </summary>
        /// <param name="innerDc">The dialog context for the component.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected virtual Task CompleteAsync(DialogContext innerDc, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Called when an event activity is received.
        /// </summary>
        /// <param name="innerDc">The dialog context for the component.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected virtual Task OnEventAsync(DialogContext innerDc, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Called when a system activity is received.
        /// </summary>
        /// <param name="innerDc">The dialog context for the component.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected virtual Task OnSystemMessageAsync(DialogContext innerDc, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// Called when a conversation update activity is received.
        /// </summary>
        /// <param name="innerDc">The dialog context for the component.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        protected virtual Task OnStartAsync(DialogContext innerDc, CancellationToken cancellationToken = default(CancellationToken))
        {
            return Task.CompletedTask;
        }
    }
}