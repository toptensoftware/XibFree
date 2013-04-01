//  XibFree - http://www.toptensoftware.com/xibfree/
//
//  Copyright 2013  Copyright © 2013 Topten Software. All Rights Reserved
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

using System;
using System.Drawing;
using System.Linq;

namespace XibFree
{
	public class LinearLayout : ViewGroup
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="XibFree.LinearLayout"/> class.
		/// </summary>
		/// <param name="orientation">Specifies the horizontal or vertical orientation of this layout.</param>
		public LinearLayout(Orientation orientation)
		{
			_orientation = orientation;
			Gravity = Gravity.TopLeft;
		}

		/// <summary>
		/// Explicitly specify the total weight of the sub views that have size of FillParent
		/// </summary>
		/// <value>The total weight.</value>
		/// <description>If not specified, the total weight is calculated by adding the LayoutParameters.Weight of
		/// each subview that has a size of FillParent.</description>
		public float TotalWeight
		{
			get
			{
				return _totalWeight;
			}
			set
			{
				_totalWeight = value;
			}
		}

		/// <summary>
		/// Specifies the gravity for views contained within this layout
		/// </summary>
		/// <value>One of the Gravity constants</value>
		public Gravity Gravity
		{
			get;
			set;
		}

		// Overridden to provide layout measurement
		protected override void onMeasure(float parentWidth, float parentHeight)
		{
			if (_orientation==Orientation.Vertical)
			{
				MeasureVertical(parentWidth, parentHeight);
			}
			else
			{
				MeasureHorizontal(parentWidth, parentHeight);
			}
		}

		// Do measurement when in vertical orientation
		private void MeasureVertical(float parentWidth, float parentHeight)
		{
			// Work out our width
			float layoutWidth = LayoutParameters.Width;
			if (layoutWidth == AutoSize.FillParent)
			{
				layoutWidth = parentWidth - Padding.Left - Padding.Right;
			}
			else if (layoutWidth == AutoSize.WrapContent)
			{
				layoutWidth = float.MaxValue;
			}
			
			// Work out the total fixed size
			float totalFixedSize = 0;
			float totalWeight = 0;
			foreach (var v in SubViews.Where(x=>!x.Hidden))
			{
				if (v.LayoutParameters.Height==AutoSize.FillParent)
				{
					// We'll deal with this later
					
					// For now, lets just total up the specified weights
					totalWeight += v.LayoutParameters.Weight;
				}
				else
				{
					// Lay it out
					v.Measure(adjustLayoutWidth(layoutWidth, v), float.MaxValue);
					totalFixedSize += v.GetMeasuredSize().Height;
				}
				
				// Include margins
				totalFixedSize += v.LayoutParameters.Margins.TotalWidth();
			}
			
			
			// Also need to include our own padding
			totalFixedSize += Padding.Top + Padding.Bottom;
			
			// Resolve our 
			float layoutHeight = LayoutParameters.Height;
			if (layoutHeight == AutoSize.FillParent)
				layoutHeight = parentHeight;
			
			float totalVariableSize = 0;
			if (layoutHeight == AutoSize.WrapContent || layoutHeight == float.MaxValue)
			{
				// This is a weird case: we have a height of wrap content, but child items that want to fill parent too.
				// Temporarily switch those items to wrap content and use their natural size
				foreach (var v in SubViews.Where(x=>!x.Hidden && x.LayoutParameters.Height==AutoSize.FillParent))
				{
					v.LayoutParameters.Height = AutoSize.WrapContent;
					v.Measure(adjustLayoutWidth(layoutWidth, v), float.MaxValue);
					v.LayoutParameters.Height = AutoSize.FillParent;
					totalVariableSize += v.GetMeasuredSize().Height;
				}
			}
			else
			{
				// If we've had an explicit weight passed to us, ignore the calculated total weight and use it instead
				if (_totalWeight!=0)
					totalWeight = _totalWeight;
				
				// Work out how much room we've got to share around
				float room = layoutHeight - totalFixedSize;
				if (room<0)
					room = 0;
				
				// Layout the fill parent items
				foreach (var v in SubViews.Where(x=>!x.Hidden && x.LayoutParameters.Height==AutoSize.FillParent))
				{
					// Work out size
					float size = room * v.LayoutParameters.Weight / totalWeight;
					v.Measure(adjustLayoutWidth(layoutWidth, v), size);
					totalVariableSize += v.GetMeasuredSize().Height;
				}
			}
			
			// Resolve our width
			float width = LayoutParameters.Width;
			if (width == AutoSize.FillParent)
			{
				width = parentWidth;
			}
			else if (width == AutoSize.WrapContent)
			{
				// Work out the maximum width of all children that aren't fill parent
				width = 0;
				foreach (var v in SubViews.Where(x=>!x.Hidden && x.LayoutParameters.Width!=AutoSize.FillParent))
				{
					float totalChildWidth = v.GetMeasuredSize().Width + v.LayoutParameters.Margins.TotalWidth();
					if (totalChildWidth > width)
						width = totalChildWidth;
				}
				
				// Set the width of all children that are fill parent
				foreach (var v in SubViews.Where(x=>!x.Hidden && x.LayoutParameters.Width==AutoSize.FillParent))
				{
					v.Measure(width, v.GetMeasuredSize().Height);
				}

				width += Padding.TotalWidth();
			}
			
			// Resolve our height
			float height = LayoutParameters.Height;
			if (height == AutoSize.FillParent)
			{
				height = parentHeight;
			}
			else if (height == AutoSize.WrapContent)
			{
				height = totalFixedSize + totalVariableSize;
			}
			
			// And finally, set our measure dimensions
			SetMeasuredSize(new SizeF(width, height));
		}

		// Do measurement when in horizontal orientation
		private void MeasureHorizontal(float parentWidth, float parentHeight)
		{
			// Work out our height
			float layoutHeight = LayoutParameters.Height;
			if (layoutHeight == AutoSize.FillParent)
			{
				layoutHeight = parentHeight - Padding.Top - Padding.Bottom;
			}
			else if (layoutHeight == AutoSize.WrapContent)
			{
				layoutHeight = float.MaxValue;
			}
			
			// Work out the total fixed size
			float totalFixedSize = 0;
			float totalWeight = 0;
			foreach (var v in SubViews.Where(x=>!x.Hidden))
			{
				if (v.LayoutParameters.Width==AutoSize.FillParent)
				{
					// We'll deal with this later
					
					// For now, lets just total up the specified weights
					totalWeight += v.LayoutParameters.Weight;
				}
				else
				{
					// Lay it out
					v.Measure(float.MaxValue, adjustLayoutHeight(layoutHeight, v));
					totalFixedSize += v.GetMeasuredSize().Width;
				}
				
				// Include margins
				totalFixedSize += v.LayoutParameters.Margins.TotalWidth();
			}
			
			
			// Also need to include our own padding
			totalFixedSize += Padding.Left + Padding.Right;
			
			// Resolve our layout width
			float layoutWidth = LayoutParameters.Width;
			if (layoutWidth == AutoSize.FillParent)
				layoutWidth = parentWidth;
			
			float totalVariableSize = 0;
			if (layoutWidth == AutoSize.WrapContent || layoutWidth == float.MaxValue)
			{
				// This is a weird case: we have a width of wrap content, but child items that want to fill parent too.
				// Temporarily switch those items to wrap content and use their natural size
				foreach (var v in SubViews.Where(x=>!x.Hidden && x.LayoutParameters.Width==AutoSize.FillParent))
				{
					v.LayoutParameters.Width = AutoSize.WrapContent;
					v.Measure(float.MaxValue, adjustLayoutHeight(layoutHeight, v));
					v.LayoutParameters.Width = AutoSize.FillParent;
					totalVariableSize += v.GetMeasuredSize().Width;
				}
			}
			else
			{
				// If we've had an explicit weight passed to us, ignore the calculated total weight and use it instead
				if (_totalWeight!=0)
					totalWeight = _totalWeight;
				
				// Work out how much room we've got to share around
				float room = layoutWidth - totalFixedSize;
				if (room<0)
					room = 0;
				
				// Layout the fill parent items
				foreach (var v in SubViews.Where(x=>!x.Hidden && x.LayoutParameters.Width==AutoSize.FillParent))
				{
					// Work out size
					float size = room * v.LayoutParameters.Weight / totalWeight;
					v.Measure(size, adjustLayoutHeight(layoutHeight, v));
					totalVariableSize += v.GetMeasuredSize().Width;
				}
			}
			
			// Resolve our height
			float height = LayoutParameters.Height;
			if (height == AutoSize.FillParent)
			{
				height = parentHeight;
			}
			else if (height == AutoSize.WrapContent)
			{
				// Work out the maximum height of all children that aren't fill parent
				height = 0;
				foreach (var v in SubViews.Where(x=>!x.Hidden && x.LayoutParameters.Height!=AutoSize.FillParent))
				{
					float totalChildHeight = v.GetMeasuredSize().Height + v.LayoutParameters.Margins.TotalHeight();
					if (totalChildHeight > height)
						height = totalChildHeight;
				}
				
				// Set the height of all children that are fill parent
				foreach (var v in SubViews.Where(x=>!x.Hidden && x.LayoutParameters.Height==AutoSize.FillParent))
				{
					v.Measure(v.GetMeasuredSize().Width, height);
				}

				height += Padding.TotalHeight();
			}
			
			// Resolve our width
			float width = LayoutParameters.Width;
			if (width == AutoSize.FillParent)
			{
				width = parentWidth;
			}
			else if (width == AutoSize.WrapContent)
			{
				width = totalFixedSize + totalVariableSize;
			}
			
			// And finally, set our measure dimensions
			SetMeasuredSize(new SizeF(width, height));
		}

		// Overridden to layout the subviews
		protected override void onLayout(RectangleF newPosition)
		{
			base.onLayout(newPosition);
			if (_orientation==Orientation.Vertical)
			{
				LayoutVertical(newPosition);
			}
			else
			{
				LayoutHorizontal(newPosition);
			}
		}

		// Do subview layout when in vertical orientation
		void LayoutVertical(RectangleF newPosition)
		{
			float y;
			switch (Gravity & Gravity.VerticalMask)
			{
				default:
					y= newPosition.Top + Padding.Top;
					break;

				case Gravity.Bottom:
					y = newPosition.Bottom - getTotalMeasuredHeight() + Padding.Top;
					break;

				case Gravity.CenterVertical:
					y = (newPosition.Top + newPosition.Bottom)/2 - getTotalMeasuredHeight()/2 + Padding.Top;
					break;

			}

			foreach (var v in SubViews.Where(i=>!i.Hidden))
			{
				y+= v.LayoutParameters.Margins.Top;

				SizeF size = v.GetMeasuredSize();

				// Work out horizontal gravity for this control
				var g = v.LayoutParameters.Gravity & Gravity.HorizontalMask;
				if (g == Gravity.None)
					g = Gravity & Gravity.HorizontalMask;

				float x;
				switch (g)
				{
					default:
						x = newPosition.Left + Padding.Left + v.LayoutParameters.Margins.Left;
						break;

					case Gravity.Right:
						x = newPosition.Right - Padding.Right - v.LayoutParameters.Margins.Right - size.Width;
						break;

					case Gravity.CenterHorizontal:
						x = (newPosition.Left + newPosition.Right)/2
							- (size.Width + v.LayoutParameters.Margins.TotalWidth())/2;
						break;
				}

				
				v.Layout(new RectangleF(x, y, size.Width, size.Height));

				y += size.Height + v.LayoutParameters.Margins.Bottom;
			}
		}

		// Do subview layout when in horizontal orientation
		void LayoutHorizontal(RectangleF newPosition)
		{
			float x;
			switch (Gravity & Gravity.HorizontalMask)
			{
				default:
					x = newPosition.Left + Padding.Left;
					break;
					
				case Gravity.Right:
					x = newPosition.Right - getTotalMeasuredWidth() + Padding.Left;
					break;
					
				case Gravity.CenterHorizontal:
					x = (newPosition.Left + newPosition.Right)/2 - getTotalMeasuredWidth()/2 + Padding.Left;
					break;
					
			}
			
			foreach (var v in SubViews.Where(i=>!i.Hidden))
			{
				x += v.LayoutParameters.Margins.Left;
				
				SizeF size = v.GetMeasuredSize();
				
				// Work out vertical gravity for this control
				var g = v.LayoutParameters.Gravity & Gravity.VerticalMask;
				if (g == Gravity.None)
					g = Gravity & Gravity.VerticalMask;
				
				float y;
				switch (g)
				{
					default:
						y = newPosition.Top + Padding.Top + v.LayoutParameters.Margins.Top;
						break;
						
					case Gravity.Bottom:
						y = newPosition.Bottom - Padding.Top - v.LayoutParameters.Margins.Bottom - size.Height;
						break;
						
					case Gravity.CenterVertical:
						y = (newPosition.Top + newPosition.Bottom)/2
							- (size.Height + v.LayoutParameters.Margins.TotalHeight())/2;
						break;
				}
				
				
				v.Layout(new RectangleF(x, y, size.Width, size.Height));
				
				x += size.Width + v.LayoutParameters.Margins.Right;
			}
		}
		
		// Helper to get the total measured height of all subviews, including all padding and margins
		private float getTotalMeasuredHeight()
		{
			return Padding.Top + Padding.Bottom + SubViews.Where(x=>!x.Hidden).Sum(x=>x.GetMeasuredSize().Height + x.LayoutParameters.Margins.TotalHeight());
		}
		
		// Helper to get the total measured width of all subviews, including all padding and margins
		private float getTotalMeasuredWidth()
		{
			return Padding.Left + Padding.Right + SubViews.Where(x=>!x.Hidden).Sum(x=>x.GetMeasuredSize().Width + x.LayoutParameters.Margins.TotalWidth());
		}

		// Helper to adjust the parent width passed down to subviews during measurement
		private float adjustLayoutWidth(float width, View c)
		{
			if (width == float.MaxValue)
				return width;
			
			return width - c.LayoutParameters.Margins.TotalWidth();
		}

		// Helper to adjust the parent height passed down to subviews during measurement
		private float adjustLayoutHeight(float height, View c)
		{
			if (height == float.MaxValue)
				return height;
			
			return height - c.LayoutParameters.Margins.TotalHeight();
		}

		public Action<LinearLayout> Init
		{
			set
			{
				value(this);
			}
		}

		// Fields
		private Orientation _orientation;
		private float _totalWeight;
	}
}

