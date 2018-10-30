using MeetAndGo.Models;
using MeetAndGo.Models.Enums;
using MeetAndGo.Views;
using Xamarin.Forms;

namespace MeetAndGo.Controls
{
    public class ViewCellSelector : DataTemplateSelector
    {
        private readonly DataTemplate _organizerDataTemplate;
        private readonly DataTemplate _memberDataTemplate;

        public ViewCellSelector()
        {
            _organizerDataTemplate = new DataTemplate(typeof(OrganizerViewCell));
            _memberDataTemplate = new DataTemplate(typeof(MemberViewCell));
        }

        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var model = item as UserModel;

            if (model.Status == UserStatus.Organizer)
            {
                return _organizerDataTemplate;
            }

            return _memberDataTemplate;
        }
    }
}
