namespace XibFree
{
	public class Dimension
	{
		public float Value { get; private set; }
		public Units Unit { get; private set; }

		public Dimension(float value, Units units = Units.Absolute)
		{
			Value = value;
			Unit = units;
		}

		public static Dimension FillParent
		{
			get { return ParentRatio(1.0f); }
		}

		public static Dimension WrapContent
		{
			get { return ContentRatio(1.0f); }
		}

		public static Dimension ParentRatio(float value)
		{
			return new Dimension(value, Units.ParentRatio);
		}

		public static Dimension AspectRatio(float value)
		{
			return new Dimension(value, Units.AspectRatio);
		}

		public static Dimension ContentRatio(float value)
		{
			return new Dimension(value, Units.ContentRatio);
		}

		public static Dimension Absolute(float value)
		{
			return new Dimension(value);
		}

		public float Ratio 
		{
			get  { return (Unit == Units.Absolute) ? 1 : Value; } 
		}
	}
}

