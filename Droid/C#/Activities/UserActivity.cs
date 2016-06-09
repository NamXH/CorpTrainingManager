
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
using Javax.Crypto;
using System.Collections.ObjectModel;
using Android.Text;

namespace CorpTrainingManager.Droid
{
	[Activity (Label = "CorpTrainingManager", ScreenOrientation = Android.Content.PM.ScreenOrientation.Portrait)]			
	public class UserActivity : BaseActivity
	{
		public ListView sortListView;
		public SortAdapter adapter;
		private ClearEditText mClearEditText;
		private ImageButton ib_exit;
		private FrameLayout fl_content;
		private LinearLayout ll_load;
		public List<SortModel> sourceDateList;
		public TextView tv_pull_list_header_title;

		public override void initListner ()
		{
			//set edittext listener
			mClearEditText.AddTextChangedListener (new MyTextChangedListener (this));
			//set item click listener
			sortListView.ItemClick += delegate(object sender, AdapterView.ItemClickEventArgs e) {
				SortModel user = sourceDateList [e.Position];
				Intent intent = new Intent (this, typeof(ResultActivity));
				intent.PutExtra ("user_id", user.UserId);
				intent.PutExtra ("display_name", user.Name);
				StartActivity (intent);
			};
			ib_exit.Click += delegate(object sender, EventArgs e) {
				goBack ();	
			};
		}

		public async override void initData ()
		{
			//get data from server
			await getDataFromServer ();
			//set adapter
			adapter = new SortAdapter (this, sourceDateList);
			sortListView.Adapter = adapter;
			//show content
			ll_load.Visibility = ViewStates.Invisible;
			fl_content.Visibility = ViewStates.Visible;
		}

		public override void initView ()
		{
			//find views
			sortListView = FindViewById<ListView> (Resource.Id.country_lvcountry);
			mClearEditText = FindViewById<ClearEditText> (Resource.Id.filter_edit);
			fl_content = FindViewById<FrameLayout> (Resource.Id.fl_content);
			ll_load = FindViewById<LinearLayout> (Resource.Id.ll_load);
			tv_pull_list_header_title = FindViewById<TextView> (Resource.Id.tv_pull_list_header_title);
			ib_exit = FindViewById<ImageButton> (Resource.Id.ib_exit);
			ll_load.Visibility = ViewStates.Visible;
			fl_content.Visibility = ViewStates.Invisible;
		}

		public override int getLayoutResource ()
		{
			return Resource.Layout.acivity_user;
		}

		public async Task getDataFromServer ()
		{
			IList<User> users = null;
			try {
				users = await ResultsUtil.GetUsersListAsync ();
			} catch (Exception ex) {
				DialogFactory.ToastDialog (this, "Data Error", "Error Connecting to Server,please try again later!", Constants.USER_ERROR);
			}
			if (users != null) {
				sourceDateList = new List<SortModel> ();
				foreach (var user in users) {
					string name = user.DisplayName;
					if (name.Length > 0) {
						string firstAlpha = name.Substring (0, 1).ToUpper ();
						SortModel temp = new SortModel ();
						temp.SortLetters = firstAlpha;
						temp.Name = name;
						temp.UserId = user.UserId;
						sourceDateList.Add (temp);
					} else {
						continue;
					}
				}
				sourceDateList.Sort ();
			}
		}

		public override void OnBackPressed ()
		{
			goBack ();
		}

		private void goBack ()
		{
			DialogFactory.toastNegativePositiveDialog (this, "Exit", "Are you sure to exit?", Constants.EXIT);
		}
	}

	public class MyTouchingLetterChangedListener:SideBar.OnTouchingLetterChangedListener
	{
		private UserActivity mUser;

		public MyTouchingLetterChangedListener (UserActivity mUser)
		{
			this.mUser = mUser;
		}

		public void onTouchingLetterChanged (string s)
		{
			//first come
			int position = mUser.adapter.GetPositionForSection (s [0]);
			if (position != -1) {
				mUser.sortListView.SetSelection (position);
			}
		}
	}

	public class MyTextChangedListener:Java.Lang.Object,ITextWatcher
	{

		private UserActivity mUser;

		public MyTextChangedListener (UserActivity mUser)
		{
			this.mUser = mUser;
		}

		public void AfterTextChanged (IEditable s)
		{
			
		}

		public void BeforeTextChanged (Java.Lang.ICharSequence s, int start, int count, int after)
		{
			
		}

		public void OnTextChanged (Java.Lang.ICharSequence s, int start, int before, int count)
		{
			filterData (s.ToString ());
		}


		public void filterData (string str)
		{
			List<SortModel> filterList = new List<SortModel> ();
			if (string.IsNullOrEmpty (str)) {
				filterList = mUser.sourceDateList;
			} else {
				filterList.Clear ();
				foreach (var model in mUser.sourceDateList) {
					string name = model.Name.ToUpper ();
					if (name.IndexOf (str.ToUpper ()) != -1) {
						filterList.Add (model);
					}
				}
			}
			//sort
			filterList.Sort ();
			mUser.adapter.updateListView (filterList);
		}
	}

}

