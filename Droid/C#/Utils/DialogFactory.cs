using System;
using Android.Content;
using Android.App;
using Android.Graphics;

namespace CorpTrainingManager.Droid
{
	public class DialogFactory
	{
		public static void ToastDialog (Context context, String title, String msg, int flag)
		{
			AlertDialog.Builder ab = new AlertDialog.Builder (context);
			ab.SetTitle (title);
			ab.SetMessage (msg);
			ab.SetPositiveButton ("confirm", delegate(object sender, DialogClickEventArgs e) {
				switch (flag) {
				case Constants.USER_ERROR:
					var useractivity = context as UserActivity;
					useractivity.tv_pull_list_header_title.Text = "Data error,please try again later";
					useractivity.tv_pull_list_header_title.SetTextColor (Color.Red);
					break;
				default:
					break;
				}
			});
			ab.Create ().Show ();
		}

		public static void toastNegativePositiveDialog (Context context, String title, String msg, int flag)
		{
			AlertDialog.Builder ab = new AlertDialog.Builder (context);
			ab.SetTitle (title);
			ab.SetMessage (msg);
			ab.SetPositiveButton ("Confirm", delegate(object sender, DialogClickEventArgs e) {
				switch (flag) {
				case Constants.EXIT:
					var useractivity = context as UserActivity;
					useractivity.Finish ();
					break;
				case Constants.RESULT_ERROR:
					var resultactivity = context as ResultActivity;
					resultactivity.StartActivity (new Intent (resultactivity, typeof(UserActivity)));
					resultactivity.Finish ();
					break;
				default:
					break;
				}
			});
			ab.SetNegativeButton ("Cancel", delegate(object sender, DialogClickEventArgs e) {

			});
			ab.Create ().Show ();
		}


	}
}

