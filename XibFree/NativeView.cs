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
using MonoTouch.CoreAnimation;

namespace XibFree
{
	/// <summary>
	/// NativeView provides a wrapper around a native view control allowing it to partipate
	/// it the XibFree's layout logic
	/// </summary>
	public class NativeView : View
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="XibFree.NativeView"/> class.
		/// </summary>
		public NativeView()
		{

		}

		/// <summary>
		/// Initializes a new instance of the <see cref="XibFree.NativeView"/> class.
		/// </summary>
		/// <param name="view">The view to be hosted.</param>
		/// <param name="lp">The view's layout parameters.</param>
		public NativeView(UIView view, LayoutParameters lp)
		{
			_view = view;
			this.LayoutParameters = lp;
		}

		public override LayoutParameters LayoutParameters
		{
			get
			{
				var nestedHost = _view as UILayoutHost;
				if (nestedHost!=null && nestedHost.Layout!=null)
				{
					return nestedHost.Layout.LayoutParameters;
				}
				return base.LayoutParameters;
			}
		}

		/// <summary>
		/// Gets or sets the native view
		/// </summary>
		/// <value>The view.</value>
		public UIView View
		{
			get
			{
				return _view;
			}
			set
			{
				if (_view!=value)
				{
					// Detach old view from host
					ViewGroup.IHost host = GetHost();
					if (host!=null)
					{
						onDetach();
					}

					// Store the new view
					_view = value;

					// Turn off auto-resizing, we'll take care of that thanks
					_view.AutoresizingMask = UIViewAutoresizing.None;

					// Attach the new view to the host
					if (host!=null)
					{
						onAttach(host);
					}
				}
			}
		}

		/// <summary>
		/// Get the hosted view, casting to the specified type
		/// </summary>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public T As<T>() where T:UIView
		{
			return _view as T;
		}


		/// <summary>
		/// Overridden to set the position of the native view
		/// </summary>
		/// <param name="newPosition">New position.</param>
		protected override void onLayout(RectangleF newPosition, bool parentHidden)
		{
			// Simple, just reposition the view!
			if (_view!=null)
			{
				_view.Hidden = parentHidden || !Visible;
				_view.Frame = newPosition;
			}
		}

		/// <summary>
		/// Overridden to provide measurement support for this native view
		/// </summary>
		/// <param name="parentWidth">Parent width.</param>
		/// <param name="parentHeight">Parent height.</param>
		protected override void onMeasure(float parentWidth, float parentHeight)
		{
			// Resolve width for absolute and parent ratio
			float width = LayoutParameters.TryResolveWidth(this, parentWidth);
			float height = LayoutParameters.TryResolveHeight(this, parentHeight);

			// Do we need to measure our content?
			SizeF sizeMeasured = SizeF.Empty;
			if (width == float.MaxValue || height == float.MaxValue)
			{
				SizeF sizeToFit = new SizeF(width, height);
				sizeMeasured = Measurer!=null ? Measurer(_view, sizeToFit) : _view.SizeThatFits(sizeToFit);
			}

			// Set the measured size
			SetMeasuredSize(LayoutParameters.ResolveSize(new SizeF(width, height), sizeMeasured));
		}

		/// <summary>
		/// Overridden to add this native view to the parent native view
		/// </summary>
		/// <param name="host">Host.</param>
		internal override void onAttach(ViewGroup.IHost host)
		{
			// If we have a view, attach to the hosting view by adding as a subview
			if (_view!=null)
			{
				host.GetUIView().Add(_view);
			}
		}

		/// <summary>
		/// Overridden to remove this native view from the parent native view
		/// </summary>
		internal override void onDetach()
		{
			// If we have a view, remove from the hosting view by removing it from the superview
			if (_view!=null)
			{
				_view.RemoveFromSuperview();
			}
		}

		/// Delegate for a plugin measurement support
		public delegate SizeF NativeMeasurer(UIView native, SizeF constraint);

		public NativeMeasurer Measurer
		{
			get;
			set;
		}

		/// <summary>
		/// Sets a an action to be immediately called.  Provided to allowing execution of code inline
		/// with the layout of the view hierarchy.  See examples.
		/// </summary>
		/// <value>An Action to be called immediately</value>
		public Action<NativeView> Init
		{
			set
			{
				value(this);
			}
		}

		internal override UIView UIViewWithTag(int tag)
		{
			if (_view!=null)
				return _view.ViewWithTag(tag);
			return null;
		}

		internal override View LayoutViewWithTag(int tag)
		{
			if (_view!=null && _view.Tag==tag)
				return this;
			return null;
		}

		public override NativeView FindNativeView(UIView v)
		{
			if (_view == v)
				return this;
			else
				return null;
		}

		internal override CALayer GetDisplayLayer()
		{
			return _view.Layer;
		}

		internal override CALayer FindFirstSublayer()
		{
			return null;
		}



		// The hosted native view
		private UIView _view;
	}
}

