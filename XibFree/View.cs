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
using UIKit;
using CoreGraphics;
using CoreAnimation;
using System.Diagnostics;

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
		public virtual LayoutParameters LayoutParameters
		{
			get;
			set;
		}

		// Internal helper to walk the parent view hierachy and find the view that's hosting this view hierarchy
		public virtual ViewGroup.IHost GetHost()
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


		/// <summary>
		/// Layout the subviews in this view using dimensions calculated during the last measure cycle
		/// </summary>
		/// <param name="newPosition">The new position of this view</param>
		public void Layout(CGRect newPosition, bool parentHidden)
		{
            try
            {
                onLayout(newPosition, parentHidden);
            }
            catch (Exception ex)
            {
                Trace.WriteLine("XibFree Layout error: " + ex.Message);
            }
		}

		/// <summary>
		/// Overridden by view groups to perform the actual layout process
		/// </summary>
		/// <param name="newPosition">New position.</param>
		protected abstract void onLayout(CGRect newPosition, bool parentHidden);

		/// <summary>
		/// Measures the subviews of this view
		/// </summary>
		/// <param name="parentWidth">Available width of the parent view group (might be nfloat.MaxValue).</param>
		/// <param name="parentHeight">Available height of the parent view group(might be nfloat.MaxValue)</param>
		public void Measure(nfloat parentWidth, nfloat parentHeight)
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
		protected abstract void onMeasure(nfloat parentWidth, nfloat parentHeight);

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
		protected void SetMeasuredSize(CGSize size)
		{
			if (LayoutParameters.MinWidth!=0 && size.Width < LayoutParameters.MinWidth)
				size.Width = LayoutParameters.MinWidth;
			if (LayoutParameters.MinHeight!=0 && size.Height < LayoutParameters.MinHeight)
				size.Height = LayoutParameters.MinHeight;
			if (LayoutParameters.MaxWidth!=0 && size.Width > LayoutParameters.MaxWidth)
				size.Width = LayoutParameters.MaxWidth;
			if (LayoutParameters.MaxHeight!=0 && size.Height > LayoutParameters.MaxHeight)
				size.Height = LayoutParameters.MaxHeight;

			_measuredSize = size;
			_measuredSizeValid = true;
		}

		/// <summary>
		/// Retrieve the measured dimensions of this view
		/// </summary>
		/// <returns>The measured size.</returns>
		public CGSize GetMeasuredSize()
		{
			if (!_measuredSizeValid)
				throw new InvalidOperationException("Attempt to use measured size before measurement");
			return _measuredSize;
		}

		internal abstract CALayer GetDisplayLayer();
		internal abstract CALayer FindFirstSublayer();

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

		public bool Gone
		{
			get
			{
				return LayoutParameters.Visibility == Visibility.Gone;
			}
			set
			{
				LayoutParameters.Visibility = value ? Visibility.Gone : Visibility.Visible;
			}
		}

		public bool Visible
		{
			get
			{
				return LayoutParameters.Visibility == Visibility.Visible;
			}
			set
			{
				LayoutParameters.Visibility = value ? Visibility.Visible : Visibility.Invisible;
			}
		}
            
        public void Hide(bool collapsed, bool animate)
        {
            if (LayoutParameters.Visibility != Visibility.Visible)
            {
                // Already hidden or in progress
                // Console.WriteLine("Skipping Hide: already hidden");
                return;
            }
            LayoutParameters.Visibility = (collapsed) ? Visibility.Gone : Visibility.Invisible;
            ShowHide(false, animate);
        }

        public void Show(bool animate)
        {
            if (LayoutParameters.Visibility == Visibility.Visible)
            {
                // Already visible or in progress
                // Console.WriteLine("Skipping Show: already shown");
                return;
            }
            LayoutParameters.Visibility = Visibility.Visible;
            ShowHide(true, animate);
        }

        public void SetVisibility(bool visible, bool animate)
        {
            if (visible)
                Show(animate);
            else
                Hide(true, animate);
        }

        public void SetVisibility(Visibility visibility, bool animate)
        {
            if (LayoutParameters.Visibility != visibility)
            {
                LayoutParameters.Visibility = visibility;
                ShowHide(visibility == Visibility.Visible, animate);
            }
        }
            
        private void ShowHide(bool show, bool animate)
        {
            //Console.WriteLine("ShowHide(show=" + show + ",animate=" + animate + ")");
            double delay = animate ? 0.25f : 0.0;
            //double delay = 0;
            var root = this.FindRootUIView();
            UIView.AnimateNotify(delay, delegate
            {
                foreach (var uiview in this.FindUIViews())
                {
                    uiview.Alpha = (show) ? 1.0f : 0.0f;
                }
            }, (bool completed) =>
            {
                //Console.WriteLine("completed=" + completed);
                //root?.Superview?.SetNeedsLayout();
                root?.SetNeedsLayout();
            });
        }
            
		public void RemoveFromSuperview()
		{
			if (Parent!=null)
			{
				Parent.RemoveSubView(this);
			}
		}


		internal CGSize _measuredSize;
		internal bool _measuredSizeValid;
		internal ViewGroup _parent;
	}
}

