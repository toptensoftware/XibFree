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
using MonoTouch.UIKit;
using System.Drawing;

namespace XibFree
{
	/// <summary>
	/// UILayoutHostScrollable is the native UIView that hosts that XibFree layout
	/// </summary>
	public class UILayoutHostScrollable : UIScrollView
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="XibFree.UILayoutHostScrollable"/> class.
		/// </summary>
		/// <param name="layout">Root of the view hierarchy to be hosted by this layout host</param>
		public UILayoutHostScrollable(ViewGroup layout, RectangleF frame) : base(frame)
		{
			_layoutHost = new UILayoutHost(layout);
			_layoutHost.AutoresizingMask = UIViewAutoresizing.None;
			this.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
			this.AddSubview(_layoutHost);
		}

		public UILayoutHostScrollable() : this(null, RectangleF.Empty)
		{
		}

		public UILayoutHostScrollable(ViewGroup layout) : this(layout, RectangleF.Empty)
		{
		}

		UILayoutHost _layoutHost;

		/// <summary>
		/// The ViewGroup declaring the layout to hosted
		/// </summary>
		/// <value>The ViewGroup.</value>
		public ViewGroup Layout
		{
			get
			{
				return _layoutHost.Layout;
			}

			set
			{
				_layoutHost.Layout = Layout;
				SetNeedsLayout();
			}
		}

		public override SizeF SizeThatFits(SizeF size)
		{
			return _layoutHost.SizeThatFits(size);
		}


		/// <Docs>Lays out subviews.</Docs>
		/// <summary>
		/// Called by iOS to update the layout of this view
		/// </summary>
		public override void LayoutSubviews()
		{
			if (Layout!=null)
			{
				// Remeasure the layout
				Layout.Measure(Bounds.Width, float.MaxValue);

				var size = Layout.GetMeasuredSize();

				// Reposition the layout host
				_layoutHost.Frame = new RectangleF(PointF.Empty, size);

				// Update the scroll view content
				ContentSize = size;
			}
		}
	}
}

