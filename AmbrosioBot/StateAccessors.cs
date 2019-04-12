using Microsoft.Bot.Builder;
using System;

namespace AmbrosioBot
{
    /// <summary>
    ///
    /// </summary>
    public class StateAccessors
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StateAccessors"/> class.
        /// </summary>
        /// <param name="conversationState">State of the conversation.</param>
        /// <param name="userState">State of the user.</param>
        public StateAccessors(ConversationState conversationState, UserState userState)
        {
            ConversationState = conversationState ?? throw new ArgumentNullException(nameof(conversationState));
            UserState = userState ?? throw new ArgumentNullException(nameof(userState));
        }

        /// <summary>
        /// Gets the state of the conversation.
        /// </summary>
        /// <value>
        /// The state of the conversation.
        /// </value>
        public ConversationState ConversationState { get; }

        /// <summary>
        /// Gets the state of the user.
        /// </summary>
        /// <value>
        /// The state of the user.
        /// </value>
        public UserState UserState { get; }
    }
}