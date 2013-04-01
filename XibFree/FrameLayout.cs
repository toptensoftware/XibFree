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
using System.Linq;
using System.Collections.Generic;
using System.Drawing;

namespace XibFree
{
	public class FrameLayout : ViewGroup
	{
		public FrameLayout()
		{
		}

		protected override void onMeasure(float parentWidth, float parentHeight)
		{
			var unresolved = new List<View>();

			// Resolve our width and height as best we can
			var width = LayoutParameters.Width;
			if (width == AutoSize.FillParent)
				width = parentWidth;
			else if (width == AutoSize.WrapContent)
				width = float.MaxValue;

			var height = LayoutParameters.Height;
			if (height == AutoSize.FillParent)
				height = parentHeight;
			else if (height == AutoSize.WrapContent)
				height = float.MaxValue;

			// Remove padding
			if (width!=float.MaxValue)
				width -= Padding.TotalWidth();
			if (height!=float.MaxValue)
				height -= Padding.TotalHeight();


			// Measure all subviews where both dimensions can be resolved
			bool haveResolveSize = false;
			float maxWidth=0, maxHeight=0;
			foreach (var v in SubViews.Where(x=>!x.Gone))
			{
				// Try to resolve subview width
				var subViewWidth = v.LayoutParameters.Width;
				if (subViewWidth == AutoSize.FillParent)
				{
					if (width==float.MaxValue)
					{
						unresolved.Add(v);
						continue;
					}
					else
					{
						subViewWidth = width - v.LayoutParameters.Margins.TotalWidth();
					}
				}
				else if (subViewWidth == AutoSize.WrapContent)
				{
					subViewWidth = float.MaxValue;
				}

				// Try to resolve subview height
				var subViewHeight = v.LayoutParameters.Height;
				if (subViewHeight == AutoSize.FillParent)
				{
					if (height==float.MaxValue)
					{
						unresolved.Add(v);
						continue;
					}
					else
					{
						subViewHeight = height - v.LayoutParameters.Margins.TotalHeight();
					}
				}
				else if (subViewHeight == AutoSize.WrapContent)
				{
					subViewHeight = float.MaxValue;
				}

				// Measure it
				v.Measure(subViewWidth, subViewHeight);

				if (!haveResolveSize)
				{
					maxWidth = v.GetMeasuredSize().Width + v.LayoutParameters.Margins.TotalWidth();
					maxHeight = v.GetMeasuredSize().Height + v.LayoutParameters.Margins.TotalHeight();
				}
				else
				{
					maxWidth = Math.Max(maxWidth, v.GetMeasuredSize().Width + v.LayoutParameters.Margins.TotalWidth());
					maxHeight = Math.Max(maxHeight, v.GetMeasuredSize().Height + v.LayoutParameters.Margins.TotalHeight());
				}
			}

			// Now resolve the unresolved subviews by either using the dimensions of the view
			// that were resolved, or none were, use their natural size
			foreach (var v in unresolved)
			{
				var oldWidth = v.LayoutParameters.Width;
				var oldHeight = v.LayoutParameters.Height;

				// Resolve subview width
				var subViewWidth = v.LayoutParameters.Width;
				if (subViewWidth == AutoSize.FillParent)
				{
					if (haveResolveSize)
						subViewWidth = maxWidth - v.LayoutParameters.Margins.TotalWidth();
					else
					{
						subViewWidth = float.MaxValue;
						v.LayoutParameters.Width = AutoSize.WrapContent;
					}
				}
				else if (subViewWidth == AutoSize.WrapContent)
				{
					subViewWidth = float.MaxValue;
				}
				
				// Resolve subview height
				var subViewHeight = v.LayoutParameters.Height;
				if (subViewHeight == AutoSize.FillParent)
				{
					if (haveResolveSize)
						subViewHeight = maxHeight - v.LayoutParameters.Margins.TotalHeight();
					else
					{
						subViewHeight = float.MaxValue;
						v.LayoutParameters.Height = AutoSize.WrapContent;
					}
				}
				else if (subViewHeight == AutoSize.WrapContent)
				{
					subViewHeight = float.MaxValue;
				}
				
				// Measure it
				v.Measure(subViewWidth, subViewHeight);

				v.LayoutParameters.Width = oldWidth;
				v.LayoutParameters.Height = oldHeight;
			}

			// Now really resolve our width
			width = LayoutParameters.Width;
			if (width == AutoSize.FillParent)
			{
				width = parentWidth;
			}
			else if (width == AutoSize.WrapContent)
			{
				width = SubViews.Max(x=>x.GetMeasuredSize().Width + x.LayoutParameters.Margins.TotalWidth()) + Padding.TotalWidth();
			}

			// And our height
			height = LayoutParameters.Height;
			if (height == AutoSize.FillParent)
			{
				height = parentHeight;
			}
			else if (height == AutoSize.WrapContent)
			{
				height = SubViews.Max(x=>x.GetMeasuredSize().Height + x.LayoutParameters.Margins.TotalHeight()) + Padding.TotalHeight();
			}

			// Done!
			SetMeasuredSize(new SizeF(width, height));
		}

		protected override void onLayout(System.Drawing.RectangleF newPosition, bool parentHidden)
		{
			// Make room for padding
			newPosition = newPosition.ApplyInsets(Padding);

			if (!parentHidden && Visible)
			{
				// Position each view according to it's gravity
				foreach (var v in SubViews)
				{
					if (v.Gone)
					{
						v.Layout(RectangleF.Empty, false);
						continue;
					}

					// If subview has a gravity specified, use it, otherwise use our own
					var g = v.LayoutParameters.Gravity;
					if (g==Gravity.None)
					{
						g = this.Gravity;
					}

					// Get it's size
					var size = v.GetMeasuredSize();

					// Work out it's position by apply margins and gravity
					var subViewPosition = newPosition.ApplyInsets(v.LayoutParameters.Margins).ApplyGravity(size, g);

					// Position it
					v.Layout(subViewPosition, false);
				}
			}
		}

		public Gravity Gravity
		{
			get;
			set;
		}

		public Action<FrameLayout> Init
		{
			set
			{
				value(this);
			}
		}
	}
}

