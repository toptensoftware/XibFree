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
	/// UILayoutHost is the native UIView that hosts that XibFree layout
	/// </summary>
	public class UILayoutHost : UIView, ViewGroup.IHost
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="XibFree.UILayoutHost"/> class.
		/// </summary>
		/// <param name="layout">Root of the view hierarchy to be hosted by this layout host</param>
		public UILayoutHost(ViewGroup layout, RectangleF frame) : base(frame)
		{
			this.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
			Layout = layout;
		}

		public UILayoutHost() 
		{
			this.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
		}

		public UILayoutHost(ViewGroup layout) 
		{
			this.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
			Layout = layout;
		}


		/// <summary>
		/// The ViewGroup declaring the layout to hosted
		/// </summary>
		/// <value>The ViewGroup.</value>
		public ViewGroup Layout
		{
			get
			{
				return _layout;
			}

			set
			{
				if (_layout!=null)
					_layout.SetHost(null);

				_layout = value;

				if (_layout!=null)
					_layout.SetHost(this);
			}
		}

		/// <summary>
		/// Finds the NativeView associated with a UIView
		/// </summary>
		/// <returns>The native view.</returns>
		/// <param name="view">View.</param>
		public NativeView FindNativeView(UIView view)
		{
			return _layout.FindNativeView(view);
		}

		public override SizeF SizeThatFits(SizeF size)
		{
			if (_layout==null)
				return new SizeF(0,0);

			// Measure the layout
			_layout.Measure(size.Width, size.Height);
			return _layout.GetMeasuredSize();
		}


		/// <Docs>Lays out subviews.</Docs>
		/// <summary>
		/// Called by iOS to update the layout of this view
		/// </summary>
		public override void LayoutSubviews()
		{
			if (_layout!=null)
			{
				// Remeasure
				_layout.Measure(Bounds.Width, Bounds.Height);
				// Apply layout
				_layout.Layout(Bounds, false);
			}
		}

		#region IHost implementation

		/// <summary>
		/// Provide the hosting view
		/// </summary>
		UIView ViewGroup.IHost.GetUIView()
		{
			return this;
		}

		#endregion


		private ViewGroup _layout;
	}
}

