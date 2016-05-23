using System;
using UIKit;
using System.Collections.Generic;
using System.Linq;

namespace CorpTrainingManager.iOS
{
    public class LearnersViewController : UIViewController
    {
        public IList<User> Learners { get; set; }

        public LearnersViewController()
        {
            Title = "Learners";
        }

        public async override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.White;

            var learnersTable = new UITableView();
            View.AddSubview(learnersTable);

            View.ConstrainLayout(() =>
                learnersTable.Frame.Top == View.Frame.Top &&
                learnersTable.Frame.Left == View.Frame.Left &&
                learnersTable.Frame.Right == View.Frame.Right &&
                learnersTable.Frame.Bottom == View.Frame.Bottom
            );

            // Loading indicator
            var loadingOverlay = new LoadingOverlay(View.Bounds);
            View.AddSubview(loadingOverlay);

            try
            {
                Learners = await ResultsUtil.GetUsersListAsync();
            }
            catch (Exception e)
            {
                var alert = UIAlertController.Create("Something goes wrong", String.Format("Please check your Internet connection and try again.{0} Details: {1}", Environment.NewLine, e.Message), UIAlertControllerStyle.Alert);
                alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                PresentViewController(alert, true, null); 
            }

            if (Learners != null)
            {
                learnersTable.Source = new LearnersTableSource(this, Learners);
                learnersTable.ReloadData();
            }

            loadingOverlay.HideThenRemove();
        }

        public class LearnersTableSource : UITableViewSource
        {
            public UIViewController Container { get; private set; }

            public IList<User> Items { get; set; }

            private string cellIdentifier = "TableCell";

            public LearnersTableSource(UIViewController container, IList<User> items)
            {
                Container = container;
                Items = items;
            }

            public override nint RowsInSection(UITableView tableview, nint section)
            {
                return Items.Count;
            }

            public async override void RowSelected(UITableView tableView, Foundation.NSIndexPath indexPath)
            {
                // Loading indicator
                var loadingOverlay = new LoadingOverlay(Container.View.Bounds);
                Container.View.Add(loadingOverlay);

                List<UserResults> testResults = null;
                try
                {
                    testResults = (await ResultsUtil.GetUserResultsAsync(Items[indexPath.Row].UserId)).ToList();
                }
                catch (Exception e)
                {
                    var alert = UIAlertController.Create("Something goes wrong", String.Format("Please check your Internet connection and try again.{0} Details: {1}", Environment.NewLine, e.Message), UIAlertControllerStyle.Alert);
                    alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                    Container.PresentViewController(alert, true, null);
                }
                loadingOverlay.HideThenRemove();

                if ((testResults != null) && (testResults.Count > 0))
                {
                    Container.NavigationController.PushViewController(new TestResultsViewController(testResults), true);
                }
                else
                {
                    var alert = UIAlertController.Create("Oops!", "This learner hasn't finished any lesson", UIAlertControllerStyle.Alert);
                    alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
                    Container.PresentViewController(alert, true, null); 
                }

                tableView.DeselectRow(indexPath, true);
            }

            public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
            {
                var cell = tableView.DequeueReusableCell(cellIdentifier);

                if (cell == null)
                {
                    cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);
                }

                cell.TextLabel.Text = Items[indexPath.Row].DisplayName;

                return cell;
            }
        }
    }
}

