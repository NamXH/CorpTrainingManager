using System;
using UIKit;
using System.Collections.Generic;
using System.Linq;

namespace CorpTrainingManager.iOS
{
    public class TestResultsViewController : UIViewController
    {
        public IList<UserResults> TestResults { get; set; }

        public TestResultsViewController(IList<UserResults> results, string title)
        {
            Title = title;
            TestResults = results;
        }

        public async override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.White;

            var testResultsTable = new UITableView();
            View.AddSubview(testResultsTable);

            View.ConstrainLayout(() =>
                testResultsTable.Frame.Top == View.Frame.Top &&
                testResultsTable.Frame.Left == View.Frame.Left &&
                testResultsTable.Frame.Right == View.Frame.Right &&
                testResultsTable.Frame.Bottom == View.Frame.Bottom
            );

            testResultsTable.Source = new TestResultsTableSource(this, TestResults);
        }

        public class TestResultsTableSource : UITableViewSource
        {
            public UIViewController Container { get; private set; }

            public IList<UserResults> Items { get; set; }

            private string cellIdentifier = "TableCell";

            public TestResultsTableSource(UIViewController container, IList<UserResults> items)
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
//                // Loading indicator
//                var loadingOverlay = new LoadingOverlay(Container.View.Bounds);
//                Container.View.Add(loadingOverlay);
//            
//                List<UserResults> resultsList = null;
//                try
//                {
//                    resultsList = (await ResultsUtil.GetUserResultsAsync(Items[indexPath.Row].UserId)).ToList();
//                }
//                catch (Exception e)
//                {
//                    var alert = UIAlertController.Create("Something goes wrong", String.Format("Please check your Internet connection and try again.{0} Details: {1}", Environment.NewLine, e.Message), UIAlertControllerStyle.Alert);
//                    alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
//                    Container.PresentViewController(alert, true, null);
//                }
//                loadingOverlay.HideThenRemove();
//            
//                if ((resultsList != null) && (resultsList.Count > 0))
//                {
//                    //                    Container.NavigationController.PushViewController(new LessonScreenViewController(Items[indexPath.Row].Id, resultsList, 0, new List<ScreenAnswer>()), true);
//                }
//                else
//                {
//                    var alert = UIAlertController.Create("Oops!", "This learner hasn't finished any lesson", UIAlertControllerStyle.Alert);
//                    alert.AddAction(UIAlertAction.Create("OK", UIAlertActionStyle.Default, null));
//                    Container.PresentViewController(alert, true, null);
//                }
            
                tableView.DeselectRow(indexPath, true);
            }

            public override UITableViewCell GetCell(UITableView tableView, Foundation.NSIndexPath indexPath)
            {
                var cell = tableView.DequeueReusableCell(cellIdentifier);

                if (cell == null)
                {
                    cell = new UITableViewCell(UITableViewCellStyle.Value1, cellIdentifier);
                }

                cell.TextLabel.Text = Items[indexPath.Row].LessonName;
                cell.DetailTextLabel.Text = String.Format("{0}/{1}", Items[indexPath.Row].UserScore, Items[indexPath.Row].TotalScore);
                if (Items[indexPath.Row].UserScore == Items[indexPath.Row].TotalScore)
                {
                    cell.DetailTextLabel.TextColor = UIColor.Green;
                }
                else
                {
                    cell.DetailTextLabel.TextColor = UIColor.Red;
                }

                return cell;
            }
        }
    }
}

