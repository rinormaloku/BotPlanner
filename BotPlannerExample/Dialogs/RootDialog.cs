using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading;
using System.Threading.Tasks;
using AuthBot;
using AuthBot.Dialogs;
using Flurl.Http;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BotPlanner.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<string>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            var message = await item;
            var resourceId = ConfigurationManager.AppSettings["ActiveDirectory.ResourceId"];
            var accessToken = await context.GetAccessToken(resourceId);

            switch (message.Text)
            {
                case "logon":
                    await AuthenticateUser(context, accessToken, resourceId, message);
                    break;
                case "show my tasks":
                    var tasks = await GetMyTasks(accessToken);
                    var resultMessage = GenerateMessageFromTasks(tasks, context.MakeMessage());
                    await context.PostAsync(resultMessage);
                    context.Wait(MessageReceivedAsync);
                    break;
                default:
                    context.Wait(MessageReceivedAsync);
                    break;
            }
        }

        private async Task AuthenticateUser(IDialogContext context, string accessToken, string resourceId, IMessageActivity message)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                await context.Forward(new AzureAuthDialog(resourceId), ResumeAfterAuth, message,
                    CancellationToken.None);
            }
            else
            {
                await context.PostAsync("You are logged in.");
                context.Wait(MessageReceivedAsync);
            }
        }
        
        private async Task<TaskDto[]> GetMyTasks(string accessToken)
        {
            return (await "https://graph.microsoft.com/v1.0/me/planner/tasks"
                    .WithOAuthBearerToken(accessToken)
                    .GetJsonAsync<PlannerTask>())
                .value;
        }

        private async Task ResumeAfterAuth(IDialogContext context, IAwaitable<string> result)
        {
            var message = await result;
            await context.PostAsync(message);
            context.Wait(MessageReceivedAsync);
        }

        private static IMessageActivity GenerateMessageFromTasks(IEnumerable<TaskDto> tasks,
            IMessageActivity resultMessage)
        {
            resultMessage.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            resultMessage.Attachments = new List<Attachment>();

            foreach (var task in tasks)
            {
                resultMessage.Attachments.Add(new HeroCard
                {
                    Title = task.title,
                    Buttons = new List<CardAction>
                    {
                        new CardAction
                        {
                            Title = "Checkout in Planner",
                            Type = ActionTypes.OpenUrl,
                            Value = "https://goo.gl/c79zwx"
                        },
                        new CardAction
                        {
                            Title = "Details",
                            Type = ActionTypes.PostBack,
                            Value = "Not Implemented"
                        },
                    }
                }.ToAttachment());
            }
            return resultMessage;
        }
    }
}