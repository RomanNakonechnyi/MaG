using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace MeetAndGo.Helpers
{
    public class UnselectingTrigger : TriggerAction<ListView>
    {
        protected override void Invoke(ListView sender)
        {
            sender.SelectedItem = null;
        }
    }
}
