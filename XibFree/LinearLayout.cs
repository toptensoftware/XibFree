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

		/// <summary>
		/// Gets or sets the spacing between stacked subviews
		/// </summary>
		/// <value>The amount of spacing.</value>
		public float Spacing
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
			float width = LayoutParameters.TryResolveWidth(this, parentWidth);
			float height = LayoutParameters.TryResolveHeight(this, parentHeight);

			// Allow room for padding
			if (width != float.MaxValue)
				width -= Padding.TotalWidth();

			// Work out the total fixed size
			float totalFixedSize = 0;
			float totalWeight = 0;
			int visibleViewCount = 0;
			foreach (var v in SubViews.Where(x=>!x.Gone))
			{
				if (v.LayoutParameters.HeightUnits==Units.ParentRatio)
				{
					// We'll deal with this later
					
					// For now, lets just total up the specified weights
					totalWeight += v.LayoutParameters.Weight;
				}
				else
				{
					// Lay it out
					v.Measure(adjustLayoutWidth(width, v), float.MaxValue);
					totalFixedSize += v.GetMeasuredSize().Height;
				}
				
				// Include margins
				totalFixedSize += v.LayoutParameters.Margins.TotalHeight();
				visibleViewCount++;
			}
			
			
			// Also need to include our own padding
			totalFixedSize += Padding.TotalHeight();

			// And spacing between controls
			if (visibleViewCount>1)
				totalFixedSize += (visibleViewCount-1) * Spacing;

			float totalVariableSize = 0;
			if (LayoutParameters.HeightUnits == Units.ContentRatio || height == float.MaxValue)
			{
				// This is a weird case: we have a height of wrap content, but child items that want to fill parent too.
				// Temporarily switch those items to wrap content and use their natural size
				foreach (var v in SubViews.Where(x=>!x.Gone && x.LayoutParameters.HeightUnits==Units.ParentRatio))
				{
					v.Measure(adjustLayoutWidth(width, v), float.MaxValue);
					totalVariableSize += v.GetMeasuredSize().Height;
				}
			}
			else
			{
				// If we've had an explicit weight passed to us, ignore the calculated total weight and use it instead
				if (_totalWeight!=0)
					totalWeight = _totalWeight;
				
				// Work out how much room we've got to share around
				float room = height - totalFixedSize;

				// Layout the fill parent items
				foreach (var v in SubViews.Where(x=>!x.Gone && x.LayoutParameters.HeightUnits==Units.ParentRatio))
				{
					// Work out size
					if (room<0)
						room = 0;
					float size = totalWeight==0 ? room : room * v.LayoutParameters.Weight / totalWeight;

					// Measure it
					v.Measure(adjustLayoutWidth(width, v), size);

					// Update total size
					totalVariableSize += v.GetMeasuredSize().Height;

					// Adjust the weighting calculation in case the view didn't accept our measurement
					room -= v.GetMeasuredSize().Height;
					totalWeight -= v.LayoutParameters.Weight;
				}
			}

			SizeF sizeMeasured = SizeF.Empty;

			if (width == float.MaxValue)
			{
				// Work out the maximum width of all children that aren't fill parent
				sizeMeasured.Width = 0;
				foreach (var v in SubViews.Where(x=>!x.Gone && x.LayoutParameters.WidthUnits!=Units.ParentRatio))
				{
					float totalChildWidth = v.GetMeasuredSize().Width + v.LayoutParameters.Margins.TotalWidth();
					if (totalChildWidth > sizeMeasured.Width)
						sizeMeasured.Width = totalChildWidth;
				}
				
				// Set the width of all children that are fill parent
				foreach (var v in SubViews.Where(x=>!x.Gone && x.LayoutParameters.WidthUnits==Units.ParentRatio))
				{
					v.Measure(sizeMeasured.Width, v.GetMeasuredSize().Height);
				}

				sizeMeasured.Width += Padding.TotalWidth();
			}
			else
			{
				width += Padding.TotalWidth();
			}

			if (height == float.MaxValue)
			{
				height = totalFixedSize + totalVariableSize;
			}
			
			// And finally, set our measure dimensions
			SetMeasuredSize(LayoutParameters.ResolveSize(new SizeF(width, height), sizeMeasured));
		}

		// Do measurement when in horizontal orientation
		private void MeasureHorizontal(float parentWidth, float parentHeight)
		{
			// Work out our height
			float layoutWidth = LayoutParameters.TryResolveWidth(this, parentWidth);
			float layoutHeight = LayoutParameters.TryResolveHeight(this, parentHeight);

			// Allow room for padding
			if (layoutHeight != float.MaxValue)
				layoutHeight -= Padding.TotalHeight();

			// Work out the total fixed size
			float totalFixedSize = 0;
			float totalWeight = 0;
			int visibleViewCount = 0;
			foreach (var v in SubViews.Where(x=>!x.Gone))
			{
				if (v.LayoutParameters.WidthUnits==Units.ParentRatio)
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
				visibleViewCount++;
			}

			// Also need to include our own padding
			totalFixedSize += Padding.TotalWidth();

			// And spacing between controls
			if (visibleViewCount>1)
				totalFixedSize += (visibleViewCount-1) * Spacing;
			
			float totalVariableSize = 0;
			if (LayoutParameters.WidthUnits == Units.ContentRatio || layoutWidth == float.MaxValue)
			{
				// This is a weird case: we have a width of wrap content, but child items that want to fill parent too.
				// Temporarily switch those items to wrap content and use their natural size
				foreach (var v in SubViews.Where(x=>!x.Gone && x.LayoutParameters.WidthUnits==Units.ParentRatio))
				{
					v.Measure(float.MaxValue, adjustLayoutHeight(layoutHeight, v));
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

				// Layout the fill parent items
				foreach (var v in SubViews.Where(x=>!x.Gone && x.LayoutParameters.WidthUnits==Units.ParentRatio))
				{
					// Work out size
					if (room<0)
						room = 0;
					float size = totalWeight==0 ? room : room * v.LayoutParameters.Weight / totalWeight;

					// Measure it
					v.Measure(size, adjustLayoutHeight(layoutHeight, v));

					// Update total size
					totalVariableSize += v.GetMeasuredSize().Width;

					// Adjust the weighting calculation in case the view didn't accept our measurement
					room -= v.GetMeasuredSize().Width;
					totalWeight -= v.LayoutParameters.Weight;
				}
			}

			SizeF sizeMeasured = SizeF.Empty;

			if (layoutHeight == float.MaxValue)
			{
				// Work out the maximum height of all children that aren't fill parent
				sizeMeasured.Height = 0;
				foreach (var v in SubViews.Where(x=>!x.Gone && x.LayoutParameters.HeightUnits!=Units.ParentRatio))
				{
					float totalChildHeight = v.GetMeasuredSize().Height + v.LayoutParameters.Margins.TotalHeight();
					if (totalChildHeight > sizeMeasured.Height)
						sizeMeasured.Height = totalChildHeight;
				}
				
				// Set the height of all children that are fill parent
				foreach (var v in SubViews.Where(x=>!x.Gone && x.LayoutParameters.HeightUnits==Units.ParentRatio))
				{
					v.Measure(v.GetMeasuredSize().Width, sizeMeasured.Height);
				}

				sizeMeasured.Height += Padding.TotalHeight();
			}
			else
			{
				layoutHeight += Padding.TotalHeight();
			}

			

			if (layoutWidth == float.MaxValue)
			{
				layoutWidth = totalFixedSize + totalVariableSize;
			}
			
			// And finally, set our measure dimensions
			SetMeasuredSize(LayoutParameters.ResolveSize(new SizeF(layoutWidth, layoutHeight), sizeMeasured));
		}

		// Overridden to layout the subviews
		protected override void onLayout(RectangleF newPosition, bool parentHidden)
		{
			base.onLayout(newPosition, parentHidden);

			if (!parentHidden && Visible)
			{
				if (_orientation==Orientation.Vertical)
				{
					LayoutVertical(newPosition);
				}
				else
				{
					LayoutHorizontal(newPosition);
				}
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

			bool first = true;

			foreach (var v in SubViews)
			{
				// Hide hidden views
				if (v.Gone)
				{
					v.Layout(RectangleF.Empty, false);
					continue;
				}

				if (!first)
					y += Spacing;
				else
					first = false;
				

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

				
				v.Layout(new RectangleF(x, y, size.Width, size.Height), false);

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

			bool first = true;

			foreach (var v in SubViews)
			{
				// Hide hidden views
				if (v.Gone)
				{
					v.Layout(RectangleF.Empty, false);
					continue;
				}

				if (!first)
					x += Spacing;
				else
					first = false;
				
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
				
				
				v.Layout(new RectangleF(x, y, size.Width, size.Height), false);
				
				x += size.Width + v.LayoutParameters.Margins.Right;
			}
		}

		private float getTotalSpacing()
		{
			if (Spacing == 0)
				return 0;

			int visibleViews = SubViews.Count(x=>!x.Gone);
			if (visibleViews>1)
				return (visibleViews-1) * Spacing;
			else
				return 0;
		}
		
		// Helper to get the total measured height of all subviews, including all padding and margins
		private float getTotalMeasuredHeight()
		{
			return Padding.TotalWidth() + getTotalSpacing() + SubViews.Where(x=>!x.Gone).Sum(x=>x.GetMeasuredSize().Height + x.LayoutParameters.Margins.TotalHeight());
		}
		
		// Helper to get the total measured width of all subviews, including all padding and margins
		private float getTotalMeasuredWidth()
		{
			return Padding.TotalHeight() + getTotalSpacing() + SubViews.Where(x=>!x.Gone).Sum(x=>x.GetMeasuredSize().Width + x.LayoutParameters.Margins.TotalWidth());
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

