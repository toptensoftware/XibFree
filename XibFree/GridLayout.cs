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
    public class RowDefinition
    {
        public nfloat Height { get; set; }

        public nfloat MaxHeight { get; set; }

        public nfloat MinHeight { get; set; }

        public Units Unit { get; set; }

        /// <summary>
        /// Gets or sets the weight of a AutoSize.FillParent view relative to its sibling views
        /// </summary>
        /// <value>The weighting value for this view's size.</value>
        public nfloat Weight
        {
            get;
            set;
        }

        public RowDefinition()
        {
            Height = AutoSize.WrapContent;
            Weight = 1;
        }

        internal nfloat YPosition { get; set; }

        internal nfloat CalculatedHeight { get; set; }
    }

    public class ColumnDefinition
    {
        public nfloat Width { get; set; }

        public nfloat MinWidth { get; set; }

        public nfloat MaxWidth { get; set; }

        public Units Unit { get; set; }

        /// <summary>
        /// Gets or sets the weight of a AutoSize.FillParent view relative to its sibling views
        /// </summary>
        /// <value>The weighting value for this view's size.</value>
        public nfloat Weight
        {
            get;
            set;
        }

        public ColumnDefinition()
        {
            Width = AutoSize.WrapContent;
            Weight = 1;
        }

        internal nfloat XPosition { get; set; }

        internal nfloat CalculatedWidth { get; set; }
    }

    public class GridLayout : ViewGroup
    {

        public IList<RowDefinition> RowDefinitions { get; set; }

        public IList<ColumnDefinition> ColumnDefinitions { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="XibFree.LinearLayout"/> class.
        /// </summary>
        /// <param name="orientation">Specifies the horizontal or vertical orientation of this layout.</param>
        public GridLayout()
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
        public nfloat ColSpacing
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the spacing between stacked subviews
        /// </summary>
        /// <value>The amount of spacing.</value>
        public nfloat RowSpacing
        {
            get;
            set;
        }

        // Overridden to provide layout measurement
        protected override void onMeasure(nfloat parentWidth, nfloat parentHeight)
        {
            Measure(parentWidth, parentHeight);
        }

        private View[,] _arrangedViews;
        // Do measurement when in horizontal orientation
        private void Measure(nfloat parentWidth, nfloat parentHeight)
        {
            // Work out our height
            nfloat layoutWidth = LayoutParameters.TryResolveWidth(this, parentWidth);
            nfloat layoutHeight = LayoutParameters.TryResolveHeight(this, parentHeight);

            // Work out the total fixed size
            var paddings = Padding.TotalWidth();

            _goneViews = new List<View>();
            _arrangedViews = new View[RowDefinitions.Count, ColumnDefinitions.Count];
            var columnWidthFillParentSubviews = new List<View>();

            //calculating columns
            var minWidth = (nfloat)ColumnDefinitions.Sum(x => x.MinWidth);

            foreach (var v in SubViews.Where(x=>!x.Gone))
            {
                _arrangedViews[v.Row, v.Column] = v;
                var columnDefinition = ColumnDefinitions[v.Column];
                var rowDefinition = RowDefinitions[v.Column];

                nfloat width;
                if (columnDefinition.Width > 0)
                    width = columnDefinition.Width;
                else if (columnDefinition.MaxWidth > 0)
                    width = columnDefinition.MaxWidth;
                else
                    width = parentWidth - paddings;

                nfloat height;
                if (rowDefinition.Height > 0)
                    height = rowDefinition.Height;
                else
                    height = adjustLayoutHeight(layoutHeight, v);

                if (v.LayoutParameters.WidthUnits != Units.ParentRatio)
                {
                    v.Measure(width - v.LayoutParameters.Margins.TotalWidth(), height);
                }
                else
                {
                    v._measuredSize = new CGSize(0, 0);
                    v._measuredSizeValid = true;
                    columnWidthFillParentSubviews.Add(v);
                }
            }

            {
                nfloat totalWeight = 0;
                nfloat totalWidth = 0;
                var columnId = -1;
                foreach (var column in ColumnDefinitions)
                {
                    columnId++;
                    column.CalculatedWidth = 0;

                    if (column.Width > 0)
                    {
                        column.CalculatedWidth = column.Width;
                    }
                    else if (column.Width == AutoSize.WrapContent)
                    {
                        
                        for (int rowId = 0; rowId < RowDefinitions.Count; rowId++)
                        {
                            var v = _arrangedViews[rowId, columnId];

                            if (v != null)
                            {
                                column.CalculatedWidth = NMath.Max(column.CalculatedWidth, v.GetMeasuredSize().Width + v.LayoutParameters.Margins.TotalWidth());
                            }
                        }
                    }
                    else if (column.Width == AutoSize.FillParent)
                    {
                        totalWeight += column.Weight;
                    }
                    totalWidth += column.CalculatedWidth;
                }

                var room = layoutWidth - totalWidth;
                foreach (var column in ColumnDefinitions.Where(x => x.Width == AutoSize.FillParent))
                {
                    columnId++;

                    column.CalculatedWidth = room * column.Weight / totalWeight;
                }
            }

            {
                var totalWeight = 0;
                var totalHeight = 0;
                var rowId = -1;
                foreach (var row in RowDefinitions)
                {
                    rowId++;

                    if (row.Height > 0)
                    {
                        row.CalculatedHeight = row.Height;
                        continue;
                    }


                    if (row.Height == AutoSize.WrapContent)
                    {
                        row.CalculatedHeight = 0;
                        for (int columnId = 0; columnId < ColumnDefinitions.Count; columnId++)
                        {
                            var v = _arrangedViews[rowId, columnId];

                            if (v != null)
                            {
                                row.CalculatedHeight = NMath.Max(row.CalculatedHeight, v.GetMeasuredSize().Height);
                            }
                        }
                    }
                }


                var room = layoutHeight - totalHeight;
                foreach (var row in RowDefinitions.Where(x => x.Height == AutoSize.FillParent))
                {
                    row.CalculatedHeight = room * row.Weight / totalWeight;
                }
            }

            CGSize sizeMeasured = CGSize.Empty;
            foreach (var item in ColumnDefinitions)
            {
                sizeMeasured.Width += item.CalculatedWidth;
            }
            sizeMeasured.Width += ColSpacing * (ColumnDefinitions.Count - 1);
            foreach (var item in RowDefinitions)
            {
                sizeMeasured.Height += item.CalculatedHeight;
            }
            sizeMeasured.Height += RowSpacing * (RowDefinitions.Count - 1);

            foreach (var v in columnWidthFillParentSubviews)
            {
                v.Measure(ColumnDefinitions[v.Column].CalculatedWidth, RowDefinitions[v.Row].CalculatedHeight);
            }

            // And finally, set our measure dimensions
            SetMeasuredSize(LayoutParameters.ResolveSize(new CGSize(layoutWidth, layoutHeight), sizeMeasured));
        }



        // Overridden to layout the subviews
        protected override void onLayout(CGRect newPosition, bool parentHidden)
        {
            base.onLayout(newPosition, parentHidden);

            if (!parentHidden && Visible)
            {
                Layout(newPosition);
            }
        }

        private List<View> _goneViews;

        void Layout(CGRect newPosition)
        {
            foreach (var v in _goneViews)
            {
                v.Layout(CGRect.Empty, false);
            }

            var startingY = newPosition.Y;
            for (int row = 0; row < RowDefinitions.Count; row++)
            {
                var rowDefinition = RowDefinitions[row];
                var startingX = newPosition.Left + Padding.Left;
                for (int column = 0; column < ColumnDefinitions.Count; column++)
                {
                    var columnDefinition = ColumnDefinitions[column];
                    View v = _arrangedViews[row, column];
                    if (v != null)
                    {
                        var horizontalGravity = v.LayoutParameters.Gravity & Gravity.HorizontalMask;
                        if (horizontalGravity == Gravity.None)
                            horizontalGravity = Gravity & Gravity.HorizontalMask;
                    
                        nfloat x = startingX;
                        switch (Gravity & Gravity.HorizontalMask)
                        {
                            case Gravity.Right:
                                x = x + columnDefinition.CalculatedWidth - v.GetMeasuredSize().Width;
                                break;

                            case Gravity.CenterHorizontal:
                                x = x + (columnDefinition.CalculatedWidth - v.GetMeasuredSize().Width) / 2;
                                break;
                        }

                        var verticalGravity = v.LayoutParameters.Gravity & Gravity.VerticalMask;
                        if (verticalGravity == Gravity.None)
                            verticalGravity = Gravity & Gravity.VerticalMask;

                        nfloat y;
                        y = startingY;
                        switch (verticalGravity)
                        {
                            case Gravity.Bottom:
                                y = y + rowDefinition.CalculatedHeight - v.GetMeasuredSize().Height;
                                break;

                            case Gravity.CenterVertical:
                                y = y + (rowDefinition.Height - v.GetMeasuredSize().Height) / 2;
                                break;
                        }

                        v.Layout(new CGRect(new CGPoint(x, y), v.GetMeasuredSize()), false);
                    }
                    startingX += columnDefinition.CalculatedWidth + ColSpacing;
                }
                startingY += rowDefinition.CalculatedHeight + RowSpacing;
            }
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

        public Action<GridLayout> Init
        {
            set
            {
                value(this);
            }
        }

        // Fields
        private Orientation _orientation;
        private nfloat _totalWeight;
    }
}

