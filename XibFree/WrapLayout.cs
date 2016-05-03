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
using CoreGraphics;
using System.Linq;
using System.Collections.Generic;

namespace XibFree
{
    /// <summary>
    /// Horizontal layout which children are of Fixed or WrapContent width
    /// Children are placed on new lines when width of parent is exceeded
    /// DOES NOT SUPPORT Children with ParentRatio
    /// </summary>
	public class WrapLayout : ViewGroup
	{
		/// <summary>
        /// Initializes a new instance of the <see cref="XibFree.WrapLayout"/> class.
		/// </summary>
        public WrapLayout()
		{
			Gravity = Gravity.TopLeft;
		}

		/// <summary>
		/// Explicitly specify the total weight of the sub views that have size of FillParent
		/// </summary>
		/// <value>The total weight.</value>
		/// <description>If not specified, the total weight is calculated by adding the LayoutParameters.Weight of
		/// each subview that has a size of FillParent.</description>
		public nfloat TotalWeight
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
		public nfloat Spacing
		{
			get;
			set;
		}

		// Overridden to provide layout measurement
		protected override void onMeasure(nfloat parentWidth, nfloat parentHeight)
		{
			MeasureHorizontal(parentWidth, parentHeight);
		}

        private class Row 
        {
            public nfloat Width { get; set; }
            public nfloat Height { get; set; }
            public nfloat YPosition { get; set; }
            public List<View> Views { get; set; }

            public Row()
            {
                Views = new List<View>();
            }
        }
        private List<View> _goneViews;
        private List<Row> _rows;


		// Do measurement when in horizontal orientation
		private void MeasureHorizontal(nfloat parentWidth, nfloat parentHeight)
		{
			// Work out our height
			nfloat layoutWidth = LayoutParameters.TryResolveWidth(this, parentWidth);
            nfloat layoutHeight = LayoutParameters.TryResolveHeight(this, parentHeight);

			// Work out the total fixed size
			int visibleViewCount = 0;
            var paddings = Padding.TotalWidth();

            _rows = new List<Row>();
            _goneViews = new List<View>();

            var row = new Row();
            _rows.Add(row);
            Func<nfloat> spacing = () => visibleViewCount == 0 ? 0 : Spacing;
			foreach (var v in SubViews)
			{
                if (v.Gone)
                {
                    _goneViews.Add(v);
                    continue;
                }

                v.Measure(parentWidth - paddings - v.LayoutParameters.Margins.TotalWidth(), adjustLayoutHeight(layoutHeight, v));
				
                var width = v.GetMeasuredSize().Width + v.LayoutParameters.Margins.TotalWidth();

                if (row.Width + width + spacing() > parentWidth)
                {
                    visibleViewCount = 0;
                    var newRow = new Row();
                    newRow.YPosition = row.YPosition + row.Height;
                    row = newRow;
                    _rows.Add(row);
                }
                row.Width += v.GetMeasuredSize().Width + v.LayoutParameters.Margins.TotalWidth() + spacing();
                row.Height = NMath.Max(row.Height, v.GetMeasuredSize().Height);

                visibleViewCount++;
                layoutWidth = NMath.Max(layoutWidth, row.Width);
                row.Views.Add(v);
			}

            layoutHeight = row.YPosition + row.Height;
			
			CGSize sizeMeasured = CGSize.Empty;

			layoutHeight += Padding.TotalHeight();
            layoutWidth += Padding.TotalWidth();
			
            // And finally, set our measure dimensions
			SetMeasuredSize(LayoutParameters.ResolveSize(new CGSize(layoutWidth, layoutHeight), sizeMeasured));
		}

		// Overridden to layout the subviews
		protected override void onLayout(CGRect newPosition, bool parentHidden)
		{
			base.onLayout(newPosition, parentHidden);

			if (!parentHidden && Visible)
			{
				LayoutHorizontal(newPosition);
			}
		}


		// Do subview layout when in horizontal orientation
		void LayoutHorizontal(CGRect newPosition)
		{
            foreach (var v in _goneViews)
            {
                v.Layout(CGRect.Empty, false);
            }

            var totalHeight = this.GetMeasuredSize().Height;
            foreach (var row in _rows)
            {
                nfloat x;
                switch (Gravity & Gravity.HorizontalMask)
                {
                    default:
                        x = newPosition.Left + Padding.Left;
                        break;

                    case Gravity.Right:
                        x = newPosition.Right - row.Width + Padding.Left;
                        break;

                    case Gravity.CenterHorizontal:
                        x = (newPosition.Left + newPosition.Right)/2 - row.Width/2 + Padding.Left;
                        break;

                }

                foreach (var v in row.Views)
                {
                      var g = v.LayoutParameters.Gravity & Gravity.VerticalMask;
                      if (g == Gravity.None)
                          g = Gravity & Gravity.VerticalMask;
                    
                      nfloat y;
                      y = newPosition.Top + row.YPosition + Padding.Top + v.LayoutParameters.Margins.Top;
                      switch (g)
                      {
                        case Gravity.Bottom:
                            y = y + row.Height - v.GetMeasuredSize().Height;
                              break;
                              
                        case Gravity.CenterVertical:
                            y = y + (row.Height - v.GetMeasuredSize().Height) / 2;
                              break;
                      }  

                    v.Layout(new CGRect(x + v.LayoutParameters.Margins.Left, y, v.GetMeasuredSize().Width, v.GetMeasuredSize().Height), false);
                    x += v.GetMeasuredSize().Width + v.LayoutParameters.Margins.TotalWidth();
                }
            }
		}

		private nfloat getTotalSpacing()
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
		private nfloat getTotalMeasuredHeight()
		{
			return (nfloat)(Padding.TotalWidth() + getTotalSpacing() + SubViews.Where(x=>!x.Gone).Sum(x=>x.GetMeasuredSize().Height + x.LayoutParameters.Margins.TotalHeight()));
		}
		
		// Helper to get the total measured width of all subviews, including all padding and margins
		private nfloat getTotalMeasuredWidth()
		{
			return (nfloat)(Padding.TotalHeight() + getTotalSpacing() + SubViews.Where(x=>!x.Gone).Sum(x=>x.GetMeasuredSize().Width + x.LayoutParameters.Margins.TotalWidth()));
		}

		// Helper to adjust the parent width passed down to subviews during measurement
		private nfloat adjustLayoutWidth(nfloat width, View c)
		{
			if (width == nfloat.MaxValue)
				return width;
			
			return width - c.LayoutParameters.Margins.TotalWidth();
		}

		// Helper to adjust the parent height passed down to subviews during measurement
		private nfloat adjustLayoutHeight(nfloat height, View c)
		{
			if (height == nfloat.MaxValue)
				return height;
			
			return height - c.LayoutParameters.Margins.TotalHeight();
		}

		public Action<WrapLayout> Init
		{
			set
			{
				value(this);
			}
		}

		// Fields
		private nfloat _totalWeight;
	}
}

