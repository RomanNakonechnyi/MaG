using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using UIKit;
using Xamarin.Forms;

namespace MeetAndGo.iOS.Views {
    public class TableSource : UITableViewSource {
        private List<ToolbarItem> _tableItems;
        private string[] _tableItemTexts;
        private readonly string CellIdentifier = "TableCell";

        public TableSource ( List<ToolbarItem> items ) {
            _tableItems = items;
            _tableItemTexts = items.Select ( item => item.Text ).ToArray ();
        }

        public override nint RowsInSection ( UITableView tableview, nint section ) {
            return _tableItemTexts.Length;
        }

        public override UITableViewCell GetCell ( UITableView tableView, NSIndexPath indexPath ) {
            UITableViewCell cell = tableView.DequeueReusableCell ( CellIdentifier );
            string item = _tableItemTexts[indexPath.Row];

            if ( cell == null ) {
                cell = new UITableViewCell ( UITableViewCellStyle.Default, CellIdentifier );
            }

            cell.TextLabel.Text = item;

            return cell;
        }

        public override nfloat GetHeightForRow ( UITableView tableView, NSIndexPath indexPath ) {
            return 56; // Set default row height.
        }

        public override void RowSelected ( UITableView tableView, NSIndexPath indexPath ) {
            var command = _tableItems[indexPath.Row].Command;
            command.Execute ( _tableItems[indexPath.Row].Command );
            tableView.DeselectRow ( indexPath, true );
            tableView.RemoveFromSuperview ();
        }
    }
}