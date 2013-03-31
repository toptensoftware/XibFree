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
		/// <param name="root">Root of the view hierarchy to be hosted by this layout host</param>
		public UILayoutHost(ViewGroup root, RectangleF frame) : base(frame)
		{
			this.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
			Root = root;
		}

		public UILayoutHost(ViewGroup root) 
		{
			this.AutoresizingMask = UIViewAutoresizing.FlexibleDimensions;
			Root = root;
		}


		/// <summary>
		/// The root ViewGroup hosted by this native view host
		/// </summary>
		/// <value>The root.</value>
		public ViewGroup Root
		{
			get
			{
				return _root;
			}

			set
			{
				if (_root!=null)
					_root.SetHost(null);

				_root = value;

				if (_root!=null)
					_root.SetHost(this);
			}
		}


		/// <Docs>Lays out subviews.</Docs>
		/// <summary>
		/// Called by iOS to update the layout of this view
		/// </summary>
		public override void LayoutSubviews()
		{
			if (_root!=null)
			{
				// Remeasure
				_root.Measure(Bounds.Width, Bounds.Height);
				// Apply layout
				_root.Layout(Bounds);
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

		private ViewGroup _root;
	}
}

