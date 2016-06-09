
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Threading.Tasks;

namespace CorpTrainingManager.Droid
{
	[Activity (Label = "ResultActivity")]			
	public class ResultActivity : BaseActivity
	{
		private TextView tv_title;
		private ImageButton ib_exit;
		private FrameLayout fl_content;
		private ListView lv_result;
		private LinearLayout ll_load;
		private TextView tv_pull_list_header_title;
		public List<UserResults> results;
		private int user_id;
		private string display_name;
		private MyResultAdapter adapter;

		public override void initListner ()
		{
			ib_exit.Click += delegate(object sender, EventArgs e) {
				StartActivity (new Intent (this, typeof(UserActivity)));
				Finish ();
			};
		}

		public override async void initData ()
		{
			user_id = Intent.GetIntExtra ("user_id", -1);
			display_name = Intent.GetStringExtra ("display_name");
			tv_title.Text = display_name;
			await getResultFromServer ();
			adapter = new MyResultAdapter (this);
			lv_result.Adapter = adapter;
			if (results.Count == 0) {
				tv_pull_list_header_title.Text = "No test result now!";
			} else {
				//show it
				fl_content.Visibility = ViewStates.Visible;
				ll_load.Visibility = ViewStates.Invisible;
			}
		}

		public override void initView ()
		{
			tv_title = FindViewById<TextView> (Resource.Id.tv_title);
			ib_exit = FindViewById<ImageButton> (Resource.Id.ib_exit);
			fl_content = FindViewById<FrameLayout> (Resource.Id.fl_content);
			lv_result = FindViewById<ListView> (Resource.Id.lv_result);
			ll_load = FindViewById<LinearLayout> (Resource.Id.ll_load);
			tv_pull_list_header_title = FindViewById<TextView> (Resource.Id.tv_pull_list_header_title);
			fl_content.Visibility = ViewStates.Invisible;
			ll_load.Visibility = ViewStates.Visible;
		}

		public override int getLayoutResource ()
		{
			return Resource.Layout.activity_result;
		}

		private async Task getResultFromServer ()
		{
			IList<UserResults> resultList = null;
			try {
				resultList = await ResultsUtil.GetUserResultsAsync (user_id);
			} catch (Exception ex) {
				DialogFactory.ToastDialog (this, "Data Error", "Data error,please try again later!", Constants.RESULT_ERROR);
			}
			if (resultList != null) {
				results = new List<UserResults> (resultList);
			}
		}
	}

	public class MyResultAdapter:BaseAdapter
	{

		private ResultActivity activity;

		public MyResultAdapter (ResultActivity activity)
		{
			this.activity = activity;
		}

		public override Java.Lang.Object GetItem (int position)
		{
			return new JavaObjectWrapper<UserResults> (){ Obj = activity.results [position] };
		}

		public override long GetItemId (int position)
		{
			return position;
		}

		public override View GetView (int position, View convertView, ViewGroup parent)
		{
			ResultViewHolder holder;
			UserResults currentResult;
			if (convertView == null) {
				var view = View.Inflate (activity, Resource.Layout.item_userresults, null);
				holder = new ResultViewHolder ();
				holder.tv_title = view.FindViewById<TextView> (Resource.Id.tv_title);
				holder.tv_scores = view.FindViewById<TextView> (Resource.Id.tv_scores);
				convertView = view;
				convertView.Tag = holder;
			} else {
				holder = convertView.Tag as ResultViewHolder;
			}
			currentResult = activity.results [position];
			if (currentResult != null) {
				holder.tv_title.Text = currentResult.LessonName;
				holder.tv_scores.Text = currentResult.UserScore + "/" + currentResult.TotalScore;
			}
			return convertView;
		}

		public override int Count {
			get {
				return activity.results.Count;
			}
		}
	}

	public class ResultViewHolder:Java.Lang.Object
	{
		public TextView tv_title;
		public TextView tv_scores;
	}
}

