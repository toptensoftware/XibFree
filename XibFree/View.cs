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
	/// Abstract base class for any item in the layout view hierarchy
	/// </summary>
	public abstract class View
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="XibFree.View"/> class.
		/// </summary>
		public View()
		{
			LayoutParameters = new LayoutParameters();
		}

		/// <summary>
		/// Gets or sets this view's parent view
		/// </summary>
		/// <value>A reference to the parent view (or null)</value>
		public ViewGroup Parent
		{
			get
			{
				return _parent;
			}
			internal set
			{
				if (_parent!=value)
				{
					// Detach old host
					ViewGroup.IHost host = GetHost();
					if (host!=null)
						onDetach();

					// Store new parent
					_parent = value;

					// Attach to new host
					host = GetHost();
					if (host!=null)
						onAttach(host);
				}
			}
		}

		/// <summary>
		/// Gets or sets the layout parameters for this view
		/// </summary>
		/// <value>The layout parameters.</value>
		public LayoutParameters LayoutParameters
		{
			get;
			set;
		}

		// Internal helper to walk the parent view hierachy and find the view that's hosting this view hierarchy
		protected virtual ViewGroup.IHost GetHost()
		{
			if (_parent==null)
				return null;

			return _parent.GetHost();
		}

		// Internal notification that this view has been attached to a hosting view
		internal virtual void onAttach(ViewGroup.IHost host)
		{
		}

		// Internal notification that this view has been detached from a hosting view
		internal virtual void onDetach()
		{
		}

		bool _hidden;

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="XibFree.View"/> is hidden.  Hidden views
		/// are collapsed and not included in layout.
		/// </summary>
		/// <value><c>true</c> if hidden; otherwise, <c>false</c>.</value>
		/// <description>To hide a view, but not collapse the space it uses, use the hidden attribute of the
		/// native view (ie: UIView.Hidden) and leave this property set to true</description>
		public bool Hidden
		{
			get
			{
				return _hidden;
			}
			set
			{
				if (_hidden!=value)
				{
					_hidden = value;
					onHiddenChanged();
				}
			}
		}

		protected virtual void onHiddenChanged()
		{
		}

		public bool ResolveHidden
		{
			get
			{
				if (_hidden)
					return _hidden;
				if (_parent!=null)
					return _parent.ResolveHidden;
				return false;
			}
		}


		/// <summary>
		/// Layout the subviews in this view using dimensions calculated during the last measure cycle
		/// </summary>
		/// <param name="newPosition">The new position of this view</param>
		public void Layout(RectangleF newPosition)
		{
			onLayout(newPosition);
		}

		/// <summary>
		/// Overridden by view groups to perform the actual layout process
		/// </summary>
		/// <param name="newPosition">New position.</param>
		protected abstract void onLayout(RectangleF newPosition);

		/// <summary>
		/// Measures the subviews of this view
		/// </summary>
		/// <param name="parentWidth">Available width of the parent view group (might be float.MaxValue).</param>
		/// <param name="parentHeight">Available height of the parent view group(might be float.MaxValue)</param>
		public void Measure(float parentWidth, float parentHeight)
		{
			_measuredSizeValid = false;
			onMeasure(parentWidth, parentHeight);
			if (!_measuredSizeValid)
			{
				throw new InvalidOperationException("onMeasure didn't set measurement before returning");
			}
		}

		/// <summary>
		/// Overridden by view groups to perform the actual layout measurement
		/// </summary>
		/// <param name="parentWidth">Parent width.</param>
		/// <param name="parentHeight">Parent height.</param>
		protected abstract void onMeasure(float parentWidth, float parentHeight);

		// Mark the measurement of this view as invalid
		public void InvalidateMeasure()
		{
			_measuredSizeValid = false;
		}

		/// <summary>
		/// Called by derived implementations of onMeasure to store the measured dimensions
		/// of this view
		/// </summary>
		/// <param name="size">Size.</param>
		protected void SetMeasuredSize(SizeF size)
		{
			_measuredSize = size;
			_measuredSizeValid = true;
		}

		/// <summary>
		/// Retrieve the measured dimensions of this view
		/// </summary>
		/// <returns>The measured size.</returns>
		public SizeF GetMeasuredSize()
		{
			if (!_measuredSizeValid)
				throw new InvalidOperationException("Attempt to use measured size before measurement");
			return _measuredSize;
		}

		/// <summary>
		/// Overridden to locate a UIView 
		/// </summary>
		/// <returns>The view with tag.</returns>
		/// <param name="tag">Tag.</param>
		internal abstract UIView UIViewWithTag(int tag);

		/// <summary>
		/// Overridden to locate a layout hierarchy view
		/// </summary>
		/// <returns>The view with tag.</returns>
		/// <param name="tag">Tag.</param>
		internal abstract View LayoutViewWithTag(int tag);

		/// <summary>
		/// Locates a view in either the layout or GUI hierarchy
		/// </summary>
		/// <returns>The view with tag.</returns>
		/// <param name="tag">Tag.</param>
		/// <typeparam name="T">The type of view to return</typeparam>
		public T ViewWithTag<T>(int tag)
		{
			if (typeof(UIView).IsAssignableFrom(typeof(T)))
				return (T)(object)UIViewWithTag(tag);
			else
			    return (T)(object)LayoutViewWithTag(tag);
		}

		public abstract NativeView FindNativeView(UIView v);


		internal SizeF _measuredSize;
		internal bool _measuredSizeValid;
		internal ViewGroup _parent;
	}
}

