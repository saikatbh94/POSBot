using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace POSBot
{
    [Serializable]
    public class CreateLMSTicket : BasicLuisDialog, IDialog<object>
    {
        static GlobalHandler.LMS lms = new GlobalHandler.LMS();
        string f = lms.firstname;
        string fr = lms.freceive;
        string l = lms.lastname;
        string lr = lms.lreceive;
        string ph = lms.phonenumber;
        string phr = lms.phreceive;
        string str = lms.storenumber;
        string strr = lms.strreceive;
        string petxt = lms.previouseidtext;
        string pespk = lms.previouseidspeak;
        string petxtr = lms.ptxtreceive;
        string pespkr = lms.pspkreceive;
        string netxt = lms.neweidtext;
        string nespk = lms.neweidspeak;
        string netxtr = lms.ntxtreceive;
        string nespkr = lms.nspkreceive;
        string pos = lms.position;
        string posr = lms.posreceive;
        string complete = lms.fcom;
        string pf = lms.pfirstname;
        string pl = lms.plastname;
        string pph = lms.pphone;
        string pstr = lms.pstore;
        string ppetxt = lms.ppetxt;
        string ppespk = lms.ppespk;
        string pnetxt = lms.pnetxt;
        string pnespk = lms.pnespk;
        string ppos = lms.ppos;
        List<string> choices = new List<string> { "Yes", "No" };
        public List<string> option = new List<string> { "Redo the form fill up", "Change any field", "Cancel the operation" };
        public List<string> details = new List<string> { "First Name", "Last Name", "Phone Number", "Store Number", "Previous EID", "New EID", "Position", "No change" };
        public string FirstName;
        public string LastName;
        public string PhoneNumber;
        public string StoreNumber;
        public string Neweid;
        public string Previouseid;
        public string Position;
        public async Task StartAsync(IDialogContext context)
        {
            await context.SayAsync(text: "A ticket needs to be created to send to LMS.", speak: "A ticket needs to be created to send to LMS.");
            await context.SayAsync(text: "Please provide all the required information on the screen.", speak: "Please provide all the required information on the screen.");
            string prompt = "Do you want to Proceed?";
            string retryprompt = "Please try again";
            var promptOptions = new PromptOptions<string>(prompt: prompt, options: choices, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
            PromptDialog.Choice(context, Submission, promptOptions);
        }
        public async Task Submission(IDialogContext context, IAwaitable<string> result)
        {
            string choice = await result;
            if (choice.ToLower() == "yes")
            {
                await First(context);
            }
            else
            {
                await context.SayAsync(text: "You have cancelled the operation. You can again start a New conversation by asking me things like 'Error in POS Open', 'EID Merge', etc", speak: "You have cancelled the operation. You can again start a new conversation by asking me things like 'Error in P O S open', 'e i d merge', etc");
                context.Wait(this.MessageReceived);
            }
        }
        public async Task First(IDialogContext context)
        {
            await context.SayAsync(text: f, speak: f);
            context.Wait(FirstNameReceived);
        }
        public async Task FirstNameReceived(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            var FN = await activity;
            int flag = 0;
            foreach(var c in FN.Text.ToCharArray())
            {
                if (char.IsNumber(c))
                {
                    flag = 1;
                    break;
                }
            }
            if (flag == 1)
            {
                await context.SayAsync(text: "First name cannot contain any number. Try again...", speak: "First name cannot contain any number. Try again");
                await First(context);
            }
            else
            {
                this.FirstName = FN.Text;
                await context.SayAsync(text: fr + this.FirstName, speak: fr + this.FirstName);
                await Last(context);
            }
        }
        public async Task Last(IDialogContext context)
        {
            await context.SayAsync(text: l, speak: l);
            context.Wait(LastNameReceived);
        }
        public async Task LastNameReceived(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            var LN = await activity;
            int flag = 0;
            foreach (var c in LN.Text.ToCharArray())
            {
                if (char.IsNumber(c))
                {
                    flag = 1;
                    break;
                }
            }
            if (flag == 1)
            {
                await context.SayAsync(text: "Last name cannot contain any number. Try again...", speak: "Last name cannot contain any number. Try again");
                await Last(context);
            }
            else
            {
                this.LastName = LN.Text;
                await context.SayAsync(text: lr + this.LastName, speak: lr + this.LastName);
                await Phone(context);
            }
        }
        public async Task Phone(IDialogContext context)
        {
            await context.SayAsync(text: ph, speak: ph);
            context.Wait(PhoneNumberReceived);
        }
        public async Task PhoneNumberReceived(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            var PH = await activity;
            int flag = 0;
            foreach (var c in PH.Text.ToCharArray())
            {
                if (!char.IsNumber(c) && c!='-')
                {
                    flag = 1;
                    break;
                }
            }
            if (flag == 1)
            {
                await context.SayAsync(text: "Phone number cannot contain any character. Try again...", speak: "Phone number cannot contain any character. Try again");
                await Phone(context);
            }
            else
            {
                if (PH.Text.Replace("-","").Length!=10)
                {
                    await context.SayAsync(text: "Phone number should be of length 10. Try again...", speak: "Phone number should be of length 10. Try again...");
                    await Phone(context);
                }
                else
                {
                    this.PhoneNumber = PH.Text.Replace("-","");
                    await context.SayAsync(text: phr + this.PhoneNumber, speak: phr + this.PhoneNumber);
                    await context.SayAsync(text: str, speak: str);
                    context.Wait(StoreNumberReceived);
                }
            }
        }
        public async Task StoreNumberReceived(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            var SN = await activity;
            this.StoreNumber = SN.Text.Replace(" ", string.Empty);
            StringBuilder sb = new StringBuilder();
            foreach (char c in this.StoreNumber.ToCharArray())
            {
                if (char.IsNumber(c))
                    sb.Append(" ").Append(c).Append(" ");
                else
                    sb.Append(c);
            }
            string store = sb.ToString().Trim();
            await context.SayAsync(text: strr+this.StoreNumber, speak: strr + store);
            await context.SayAsync(text: petxt, speak: pespk);
            context.Wait(PeidReceived);
        }
        public async Task PeidReceived(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            var PE = await activity;
            this.Previouseid = PE.Text.Replace(" ", string.Empty);
            StringBuilder sb = new StringBuilder();
            foreach (char c in this.Previouseid.ToCharArray())
            {
                if (char.IsNumber(c))
                    sb.Append(" ").Append(c).Append(" ");
                else
                    sb.Append(c);
            }
            string peidspk = sb.ToString().Trim();
            await context.SayAsync(text: petxtr+this.Previouseid, speak: pespkr + peidspk);
            await context.SayAsync(text: netxt, speak: nespk);
            context.Wait(NeidReceived);
        }
        public async Task NeidReceived(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            var NE = await activity;
            this.Neweid = NE.Text.Replace(" ", string.Empty);
            StringBuilder sb = new StringBuilder();
            foreach (char c in this.Neweid.ToCharArray())
            {
                if (char.IsNumber(c))
                    sb.Append(" ").Append(c).Append(" ");
                else
                    sb.Append(c);
            }
            string neidspk = sb.ToString().Trim();
            await context.SayAsync(text: netxtr+this.Neweid, speak: nespkr + neidspk);
            await context.SayAsync(text: pos, speak: pos);
            context.Wait(PositionReceived);
        }
        public async Task PositionReceived(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            var POS = await activity;
            this.Position = POS.Text;
            await context.SayAsync(text: posr+this.Position, speak: posr + this.Position);
            await context.SayAsync(text: complete, speak: complete);
            await ShowDetails(context);
        }
        public async Task ShowDetails(IDialogContext context)
        {
            await context.SayAsync(text: "First Name: "+this.FirstName+"."+"<br/>Last Name: "+this.LastName+"."+"<br/>Phone Number: "+this.PhoneNumber+"."+"<br/>Store Number: "+this.StoreNumber+"."+"<br/>Previous EID: "+this.Previouseid+"."+"<br/>New EID: "+this.Neweid+"."+"<br/>Position: "+this.Position+".", speak: "First name: " + this.FirstName + ". Last name: " + this.LastName + ". Phone number: " + this.PhoneNumber + ". Store number: " + this.StoreNumber + ". Previous e i d: " + this.Previouseid + ". New e i d: " + this.Neweid + ". Position: " + this.Position);
            string prompt = "Do you want to Proceed?";
            string retryprompt = "Please try again";
            var promptOptions = new PromptOptions<string>(prompt: prompt, options: choices, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
            PromptDialog.Choice(context, SubmissionForm, promptOptions);
        }
        public async Task SubmissionForm(IDialogContext context, IAwaitable<string> result)
        {
            string choice = await result;
            if (choice.ToLower() == "yes")
            {
                await context.SayAsync(text: "Your message was registered.", speak: "Your message was registered.");
                await context.SayAsync(text: "Once we resolve it, We will get back to you.", speak: "Once we resolve it, we will get back to you.");

                string prompt = "Do you want to Merge another employee?";
                string retryprompt = "Please try again";
                var promptOptions = new PromptOptions<string>(prompt: prompt, options: choices, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
                PromptDialog.Choice(context, Confirmed, promptOptions);
            }
            else
            {
                string prompt = "Please select one of the following options.";
                string retryprompt = "Please try again";
                var promptOptions = new PromptOptions<string>(prompt: prompt, options: option, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
                PromptDialog.Choice(context, SubmissionNotComplete, promptOptions);
            }
        }
        public async Task SubmissionNotComplete(IDialogContext context, IAwaitable<string> result)
        {
            string option = await result;
            if (option.ToLower().Contains("change"))
            {
                string prompt = "Okay, Tell me which field do you want to change?";
                string retryprompt = "Please Try again";
                var promptOptions = new PromptOptions<string>(prompt: prompt, options: details, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
                PromptDialog.Choice(context, Change, promptOptions);
            }
            else if (option.ToLower().Contains("cancel"))
            {
                await context.SayAsync(text: "You have cancelled the operation", speak: "You have cancelled the operation.");
                await context.SayAsync(text: "You can again start a new conversation by asking me things like 'POS Error', 'EID Merge', etc.", speak: "You can again start a new conversation by asking me things like 'P O S Error', 'E I D Merge', etc");
                context.Wait(this.MessageReceived);
            }
            else if (option.ToLower().Contains("redo"))
            {
                await context.SayAsync(text: "You requested to start the form fill up again", speak: "You requested to start the form fill up again.");
                await First(context);
            }
        }
        public async Task Change(IDialogContext context, IAwaitable<string> result)
        {
            string details = await result;
            if (details.ToLower() == "first name")
            {
                await context.SayAsync(pf + this.FirstName, speak: pf + this.FirstName);
                await FirstRepeat(context);
            }
            else if (details.ToLower() == "last name")
            {
                await context.SayAsync(pl+this.LastName, speak: pl + this.LastName);
                await LastRepeat(context);
            }
            else if (details.ToLower() == "phone number")
            {
                await context.SayAsync(pph+this.PhoneNumber, speak: pph + this.PhoneNumber);
                await PhoneRepeat(context);
            }
            else if (details.ToLower() == "store number")
            {
                await context.SayAsync(pstr+this.StoreNumber, speak: pstr + this.StoreNumber);
                await context.SayAsync(text: str, speak: str);
                context.Wait(StoreNumberRepeat);
            }
            else if (details.ToLower().Contains("previous"))
            {
                await context.SayAsync(ppetxt+this.Previouseid, speak: ppespk + this.Previouseid);
                await context.SayAsync(text: petxt, speak: pespk);
                context.Wait(PEIDRepeat);
            }
            else if (details.ToLower().Contains("new"))
            {
                await context.SayAsync(pnetxt+this.Neweid, speak: pnespk + this.Neweid);
                await context.SayAsync(text: netxt, speak: nespk);
                context.Wait(NEIDRepeat);
            }
            else if (details.ToLower() == "position")
            {
                await context.SayAsync(ppos+this.Position, speak: ppos+this.Position);
                await context.SayAsync(text: pos, speak: pos);
                context.Wait(PositionRepeat);
            }
            else
            {
                await ShowDetails(context);
            }
        }
        public async Task FirstRepeat(IDialogContext context)
        {
            await context.SayAsync(text: f, speak: f);
            context.Wait(FirstNameRepeat);
        }
        public async Task FirstNameRepeat(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            string fname = this.FirstName;
            var fn = await activity;
            int flag = 0;
            foreach (var c in fn.Text.ToCharArray())
            {
                if (char.IsNumber(c))
                {
                    flag = 1;
                    break;
                }
            }
            if (flag == 1)
            {
                await context.SayAsync(text: "First name cannot contain any number. Try again...", speak: "First name cannot contain any number. Try again");
                await FirstRepeat(context);
            }
            else
            {
                this.FirstName = fn.Text;
                await context.SayAsync(text: $"First name is changed form {fname} to {this.FirstName}.", speak: $"First name is changed form {fname} to {this.FirstName}.");
                string prompt = "Do you want to change any other fields?";
                string retryprompt = "Please try again";
                var promptOptions = new PromptOptions<string>(prompt: prompt, options: choices, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
                PromptDialog.Choice(context, ConfirmChange, promptOptions);
            }
        }
        public async Task LastRepeat(IDialogContext context)
        {
            await context.SayAsync(text: l, speak: l);
            context.Wait(LastNameRepeat);
        }
        public async Task LastNameRepeat(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            string lname = this.LastName;
            var ln = await activity;
            int flag = 0;
            foreach (var c in ln.Text.ToCharArray())
            {
                if (char.IsNumber(c))
                {
                    flag = 1;
                    break;
                }
            }
            if (flag == 1)
            {
                await context.SayAsync(text: "Last name cannot contain any number. Try again...", speak: "Last name cannot contain any number. Try again");
                await LastRepeat(context);
            }
            else
            {
                this.LastName = ln.Text;
                await context.SayAsync(text: $"Last name is changed from {lname} to {this.LastName}.", speak: $"Last name is changed from {lname} to {this.LastName}.");
                string prompt = "Do you want to change any other field?";
                string retryprompt = "Please try again";
                var promptOptions = new PromptOptions<string>(prompt: prompt, options: choices, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
                PromptDialog.Choice(context, ConfirmChange, promptOptions);
            }
        }
        public async Task PhoneRepeat(IDialogContext context)
        {
            await context.SayAsync(text: ph, speak: ph);
            context.Wait(PhoneNumberRepeat);
        }
        public async Task PhoneNumberRepeat(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            string phone = this.PhoneNumber;
            var ph = await activity;
            int flag = 0;
            foreach (var c in ph.Text.ToCharArray())
            {
                if (!char.IsNumber(c) && c != '-')
                {
                    flag = 1;
                    break;
                }
            }
            if (flag == 1)
            {
                await context.SayAsync(text: "Phone number cannot contain any character. Try again...", speak: "Phone number cannot contain any character. Try again");
                await Phone(context);
            }
            else
            {
                if (ph.Text.Replace("-", "").Length != 10)
                {
                    await context.SayAsync(text: "Phone number should be of length 10. Try again...", speak: "Phone number should be of length 10. Try again...");
                    await Phone(context);
                }
                else
                {
                    this.PhoneNumber = ph.Text.Replace("-","");
                    await context.SayAsync(text: $"Phone number is changed from {phone} to {this.PhoneNumber}.", speak: $"Phone number is changed form {phone} to {this.PhoneNumber}.");
                    string prompt = "Do you want to change any other field?";
                    string retryprompt = "Please try again";
                    var promptOptions = new PromptOptions<string>(prompt: prompt, options: choices, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
                    PromptDialog.Choice(context, ConfirmChange, promptOptions);
                }
            }
        }
        public async Task StoreNumberRepeat(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            string store = this.StoreNumber;
            var st = await activity;
            this.StoreNumber = st.Text.Replace(" ", string.Empty);
            StringBuilder sb = new StringBuilder();
            foreach (char c in this.Previouseid.ToCharArray())
            {
                if (char.IsNumber(c))
                    sb.Append(" ").Append(c).Append(" ");
                else
                    sb.Append(c);
            }
            string str = sb.ToString().Trim();
            await context.SayAsync(text: $"Store number is changed form {store} to {this.StoreNumber}.", speak: $"Store number is changed form {store} to {str}.");
            string prompt = "Do you want to change any other field?";
            string retryprompt = "Please try again";
            var promptOptions = new PromptOptions<string>(prompt: prompt, options: choices, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
            PromptDialog.Choice(context, ConfirmChange, promptOptions);
        }
        public async Task PEIDRepeat(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            string peid = this.Previouseid;
            var pe = await activity;
            this.Previouseid = pe.Text.Replace(" ", string.Empty);
            StringBuilder sb = new StringBuilder();
            foreach (char c in this.Previouseid.ToCharArray())
            {
                if (char.IsNumber(c))
                    sb.Append(" ").Append(c).Append(" ");
                else
                    sb.Append(c);
            }
            string peidspk = sb.ToString().Trim();
            await context.SayAsync(text: $"Previous EID is changed from {peid} to {this.Previouseid}.", speak: $"Previous e i d is changed from {peid} to {peidspk}");

            string prompt = "Do you want to change any other field?";
            string retryprompt = "Please try again";
            var promptOptions = new PromptOptions<string>(prompt: prompt, options: choices, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
            PromptDialog.Choice(context, ConfirmChange, promptOptions);
        }
        public async Task NEIDRepeat(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            string neid = this.Neweid;
            var ne = await activity;

            this.Neweid = ne.Text.Replace(" ", string.Empty);
            StringBuilder sb = new StringBuilder();
            foreach (char c in this.Previouseid.ToCharArray())
            {
                if (char.IsNumber(c))
                    sb.Append(" ").Append(c).Append(" ");
                else
                    sb.Append(c);
            }
            string neidspk = sb.ToString().Trim();
            await context.SayAsync(text: $"New EID is changed from {neid} to {this.Neweid}.", speak: $"New e i d is changed from {neid} to {neidspk}");

            string prompt = "Do you want to change any other field?";
            string retryprompt = "Please try again";
            var promptOptions = new PromptOptions<string>(prompt: prompt, options: choices, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
            PromptDialog.Choice(context, ConfirmChange, promptOptions);
        }
        public async Task PositionRepeat(IDialogContext context, IAwaitable<IMessageActivity> activity)
        {
            string position = this.Position;
            var pos = await activity;
            this.Position = pos.Text;
            await context.SayAsync(text: $"Position is changed from {position} to {this.Position}.", speak: $"Position is changed form {position} to {this.Position}.");
            string prompt = "Do you want to change any other field?";
            string retryprompt = "Please try again";
            var promptOptions = new PromptOptions<string>(prompt: prompt, options: choices, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
            PromptDialog.Choice(context, ConfirmChange, promptOptions);
        }
        public async Task ConfirmChange(IDialogContext context, IAwaitable<string> result)
        {
            string confirm = await result;
            if (confirm.ToLower() == "yes")
            {
                string prompt = "Okay, what would you like to change?";
                string retryprompt = "Please try again";
                var promptOptions = new PromptOptions<string>(prompt: prompt, options: details, retry: retryprompt, speak: prompt, retrySpeak: retryprompt, promptStyler: new PromptStyler());
                PromptDialog.Choice(context, Change, promptOptions);
            }
            else
            {
                await ShowDetails(context);
            }
        }
        public async Task Confirmed(IDialogContext context, IAwaitable<string> result)
        {
            string confirm = await result;
            if (confirm.ToLower() == "yes")
            {
                
                await new CreateLMSTicket().StartAsync(context);
            }
            else
            {
                var ticketNumber = "L"+new Random().Next(1000,9999);
                await context.SayAsync(text: "A LMSTicket is provided to you and your message has been registered.", speak: "L M S Ticket is provided to you and your message has been registered.");

                await new CreateServiceRequest().Start(context, ticketNumber);

            }
        }
    }
}