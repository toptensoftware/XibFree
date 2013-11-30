using System;

namespace XibFree
{
	public class Dimension
	{
		private float _value;
		private Units _unit;

		public Dimension(float value, Units units = Units.Absolute)
		{
			_value = value;
			_unit = units;
		}

		public static Dimension FillParent
		{
			get { return Dimension.ParentRatio(1.0f); }
		}

		public static Dimension WrapContent
		{
			get { return Dimension.ContentRatio(1.0f); }
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
			return new Dimension(value, Units.Absolute);
		}

		public float Value { get { return _value; } }
		public Units Unit { get { return _unit; } }
		public float Ratio 
		{
			get 
			{ 
				return (_unit == Units.Absolute) ? 1 : _value; 
			} 
		}
	}
}

