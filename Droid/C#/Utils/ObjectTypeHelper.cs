using System;

namespace CorpTrainingManager.Droid
{
	public class JavaObjectWrapper<T> : Java.Lang.Object
	{
		public T Obj { get; set; }
	}
}

