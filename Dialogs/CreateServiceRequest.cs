using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Threading.Tasks;

namespace POSBot
{
    [Serializable]
    public class CreateServiceRequest
    {
        public async Task Start(IDialogContext context, string incident)
        {
            await new CloseContact().Start(context,incident);
            /*var incidentNumber = "P" + new Random().Next(1000, 9999);
            await context.SayAsync(text: $"An incident ticket has been created for you.", speak: $"An incident ticket has been created for you.");
            await new CloseContact().Start(context,incidentNumber);*/

        }
    }
}