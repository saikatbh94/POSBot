using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POSBot
{
    [Serializable]
    public class ConfirmReady : BasicLuisDialog, IDialog<object>
    {
        public int attempt = 0;
        public int successattempt = 0;
        static GlobalHandler.Confirm confirm = new GlobalHandler.Confirm();
        List<string> ready = confirm.ready;
        List<string> proceed = confirm.proceed;
        List<string> status = confirm.status;
        List<string> readyMessage = confirm.readyMessage;
        List<string> proceedMessage = confirm.proceedMessage;
        string step1 = confirm.Step1;
        string step2 = confirm.Step2;
        string step3 = confirm.Step3;
        string step4 = confirm.Step4;
        string step5 = confirm.Step5;
        string step6 = confirm.Step6;
        string step7 = confirm.Step7;
        string step8 = confirm.Step8;
        string step9 = confirm.Step9;
        string step10 = confirm.Step10;
        public async Task StartAsync(IDialogContext context)
        {
            string s = "Step 1: Click on Manager Menu from P O S screen.";
            await context.SayAsync(text: step1 + "<br/>" + step2 + "<br/>" + step3 + "<br/>" + step4, speak: s+" "+step2+" "+step3+" "+step4);
            await context.SayAsync(text: "Please confirm when ready for next step.", speak: "Please confirm when ready for next step.");
            string prompt = new GlobalHandler().GetRandomString(proceedMessage);
            string retryprompt = "Please try again";
            var promptOptions = new PromptOptions<string>(prompt: prompt, options: proceed, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
            PromptDialog.Choice(context, ResumeConfirm, promptOptions);
        }
        public async Task ResumeConfirm(IDialogContext context, IAwaitable<string> result)
        {

            string progress = await result;
            if (progress.ToLower().Contains("yes") || progress.ToLower().Contains("proceed"))
            {
                string q = "Step 6: Press open P E D.";
                string r = "Step 7: Reset the Cashless Device.";
                string s = "Step 8: Select Manager Menu, then Special Functions, and then Reset Cashless device.";
                string t = "Step 9: Image will come up: Please be sure there are no cards inserted in the P E D before continuing, then Press OK.";
                string u = "Step 10: If successful the following message will come up: P E D communication is working.";
                await context.SayAsync(text: step5 + "<br/>" + step6 + "<br/>" + step7 + "<br/>" + step8 + "<br/>" + step9 + "<br/>" + step10, speak: step5+" "+q+" "+r+" "+s+" "+t+" "+u);
                string prompt = "Was the reset successful?";
                string retryprompt = "Please try again";
                var promptOptions = new PromptOptions<string>(prompt: prompt, options: status, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
                PromptDialog.Choice(context, ResumeStatus, promptOptions);
            }
            else if (progress.ToLower().Contains("no") || progress.ToLower().Contains("stop"))
            {
                await context.SayAsync(text: "You have cancelled the operation", speak: "You have cancelled the operation");
                await context.SayAsync(text: "You can again start asking me things like 'Error in POS Open', 'EID Merge' etc", speak: "You can again start asking me things like 'Error in p o s open', 'e i d merge' etc");
                context.Wait(this.MessageReceived);
            }
        }
        public async Task ResumeStatus(IDialogContext context, IAwaitable<string> result)
        {
            string status = await result;
            if ((status.ToLower().Contains("success") && (!status.ToLower().Contains("no") || !status.ToLower().Contains("not"))) || status.ToLower().Contains("yes"))
            {
                var ticket = "P" + new Random().Next(1000, 9999);
                await new CreateServiceRequest().Start(context, ticket);
            }
            else if (status.ToLower().Contains("not success") || status.ToLower().Contains("no success") || status.ToLower().Contains("no") || status.ToLower().Contains("not"))
            {
                successattempt++;
                await context.SayAsync(text: "Performing a PED Rescue via USB is the next step.", speak: "Performing a P E D Rescue via USB is the next step.");
                await context.SayAsync(text: "If you would like to view the instructions for the PED rescue, please see KB0113750 on the OTP portal.", speak: "If you would like to view the instructions for the P E D rescue, please see KB0113750 on the O T P portal.");
                await context.SayAsync(text: "Please note that a PED rescue may take up to 40 minutes to complete.", speak: "Please note that a P E D rescue may take up to 40 minutes to complete.");
                if (successattempt >= 1)
                {
                    await context.SayAsync(text: "Let me transfer you to a person", speak: "Let me transfer you to a person");
                    await new TransferToAPerson().StartAsync(context);
                }
                else
                {
                    await Reset(context);
                }
            }
        }
        public async Task Reset(IDialogContext context)
        {
            string prompt = "Was the reset successful this time?";
            string retryprompt = "Please try again";
            var promptOptions = new PromptOptions<string>(prompt: prompt, options: status, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
            PromptDialog.Choice(context, ResumeStatus, promptOptions);
        }
    }
}