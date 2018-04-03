using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace POSBot
{
    public class Help : BasicLuisDialog, IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            await context.SayAsync(text: "Are you having a POS or EID issue?", speak: "Are you having a P O S or E I D issue?");
            context.Wait(this.MessageReceived);
        }
    }
}