
	public class DiceFace
	{
		//is modifier, is value, is special, etc etc.
		public int value;

		public int GetValue()
		{
			return value;
		}
		
		public static DiceFace CreateNormalDiceFace(int val)
		{
			return new DiceFace()
			{
				value = val
			};
		}
	}