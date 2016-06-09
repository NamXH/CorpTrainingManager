using System;
using Android.Views;
using Android.Graphics;
using Android.Widget;
using Android.Content;
using Android.Util;
using Android.Graphics.Drawables;

namespace CorpTrainingManager.Droid
{
	public class SideBar:View
	{
		//Touch Event
		private OnTouchingLetterChangedListener onTouchingLetterChangedListener;

		// 26 alphabet
		public static String[] b = { "A", "B", "C", "D", "E", "F", "G", "H", "I",  
			"J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V",  
			"W", "X", "Y", "Z", "#"
		};
		//checked
		private int choose = -1;

		private Paint paint = new Paint ();

		private TextView mTextDialog;

		/**
     * textview for sidebar
     * 
     * @param mTextDialog
     */
		public void setTextView (TextView mTextDialog)
		{
			this.mTextDialog = mTextDialog;
		}


		public SideBar (Context context) : base (context)
		{
		}


		public SideBar (Context context, IAttributeSet attrs) : base (context, attrs)
		{
		}


		public SideBar (Context context, IAttributeSet attrs, int defStyleAttr) : base (context, attrs, defStyleAttr)
		{
		}

		protected override void OnDraw (Canvas canvas)
		{
			base.OnDraw (canvas);
			int height = this.Height;//get height
			int width = this.Width;//get width
			int singleHeight = height / b.Length;//get the height of every alpha
			for (int i = 0; i < b.Length; i++) {
				paint.Color = Color.Rgb (33, 65, 98);
				paint.SetTypeface (Typeface.DefaultBold);
				paint.AntiAlias = true;
				paint.TextSize = 20;
				//set checked 
				if (i == choose) {
					paint.Color = Color.ParseColor ("#3399ff");
					paint.FakeBoldText = true;//set bold
				}
				//x坐标等于=中间-字符串宽度的一般
				float xPos = width / 2 - paint.MeasureText (b [i]) / 2;
				float yPos = singleHeight * i + singleHeight;
				canvas.DrawText (b [i], xPos, yPos, paint);
				paint.Reset ();//重置画笔
			}
		}

		public override bool DispatchTouchEvent (MotionEvent e)
		{
			float y = e.GetY ();//点击y坐标
			int oldChoose = choose;

			OnTouchingLetterChangedListener listener = onTouchingLetterChangedListener;

			int c = (int)(y / this.Height * b.Length);//点击y坐标所占高度的比例*b数组的长度就等于点击b中的个数

			switch (e.Action) {
			case MotionEventActions.Up:
				SetBackgroundDrawable (new ColorDrawable (Color.Black));//设置背景颜色
				choose = -1;
				Invalidate ();
				if (mTextDialog != null) {
					mTextDialog.Visibility = ViewStates.Visible;
				}
				break;

			default:
				SetBackgroundResource (Resource.Drawable.sidebar_background);
				if (oldChoose != c) {
					if (c >= 0 && c < b.Length) {
						if (listener != null) {
							listener.onTouchingLetterChanged (b [c]);
						}
						if (mTextDialog != null) {
							mTextDialog.Text = b [c];
							mTextDialog.Visibility = ViewStates.Visible;
						}
						choose = c;
						Invalidate ();
					}
				}
				break;
			}
			return true;
		}

		/**
	 * 向外松开的方法
	 * 
	 * @param onTouchingLetterChangedListener
	 */
		public void setOnTouchingLetterChangedListener (
			OnTouchingLetterChangedListener onTouchingLetterChangedListener)
		{
			this.onTouchingLetterChangedListener = onTouchingLetterChangedListener;
		}

		/**
	 * 
	 * 接口
	 * 
	 * @author 
	 *
	 */
		public interface OnTouchingLetterChangedListener
		{
			void onTouchingLetterChanged (String s);
		}

	}
}

