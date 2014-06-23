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
using System.Collections.Generic;
using System.Linq;
using MonoTouch.CoreAnimation;
using System.Drawing;

namespace XibFree
{
	/// <summary>
	/// Base class for all views that can layout a set of subviews
	/// </summary>
	public abstract class ViewGroup : View
	{
		// Fields
		private readonly List<View> _subViews = new List<View>();
		private CALayer _layer;
		private IHost _host;

		/// <summary>
		/// Gets or sets the padding that should be applied around the subviews contained in this view group
		/// </summary>
		/// <value>The padding.</value>
		public UIEdgeInsets Padding { get; set; }

		public int Tag { get; set; }

		/// <summary>
		/// Gets or sets all the subviews of this view group
		/// </summary>
		/// <value>The sub views.</value>
		public IEnumerable<View> SubViews
		{
			get { return _subViews; }
			set
			{
				// Check that none of the child subviews already have parents
				if (value.Any(c => c.Parent != null))
				{
					throw new InvalidOperationException("View is already a child of another ViewGroup");
				}

				foreach (var c in value) c.Parent = this;

				// Remove self as parent from current subviews and then replace them
				foreach (var c in _subViews) c.Parent = null;
				_subViews.Clear();

				_subViews.AddRange(value);
			}
		}

		/// <summary>
		/// Insert a new subview at a specified position
		/// </summary>
		/// <param name="position">Zero-based index of where to insert the new subview.</param>
		/// <param name="view">The native subview to insert.</param>
		/// <param name="lp">Layout parameters for the subview.</param>
		public void InsertSubView(int position, UIView view, LayoutParameters lp)
		{
			InsertSubView(-1, new NativeView(view, lp));
		}

		/// <summary>
		/// Insert a new subview at the end of the subview collection
		/// </summary>
		/// <param name="view">The native subview to insert.</param>
		/// <param name="lp">Layout parameters for the subview.</param>
		public void AddSubView(UIView view, LayoutParameters lp)
		{
			InsertSubView(-1, new NativeView(view, lp));
		}

		/// <summary>
		/// Insert a new subview at the end of the subview collection
		/// </summary>
		/// <param name="view">The subview to add</param>
		public void AddSubView(View view)
		{
			InsertSubView(-1, view);
		}

		/// <summary>
		/// Remove a subview from the subview collection
		/// </summary>
		/// <param name="view">The subview to remove.</param>
		public void RemoveSubView(UIView view)
		{
			for (var i=0; i<_subViews.Count; i++)
			{
				var nv = _subViews[i] as NativeView;
				if (nv!=null && nv.View == view)
				{
					RemoveSubViewAt(i);
				}
			}
		}

		/// <summary>
		/// Insert a new subview at a specified position
		/// </summary>
		/// <param name="position">Zero-based index of where to insert the new subview.</param>
		/// <param name="view">The subview to add.</param>
		public void InsertSubView(int position, View view)
		{
			if (view.Parent != null) throw new InvalidOperationException("View is already a child of another ViewGroup");
			view.Parent = this;

			if (position < 0) position = _subViews.Count;
			_subViews.Insert(position, view);
		}
	
		/// <summary>
		/// Remove the specified subview
		/// </summary>
		/// <param name="view">The subview to remove</param>
		public void RemoveSubView(View view)
		{
			RemoveSubViewAt(_subViews.IndexOf(view));
		}

		/// <summary>
		/// Remove the subview at a specified position
		/// </summary>
		/// <param name="index">The zero-based index of the view to remove.</param>
		public void RemoveSubViewAt(int index)
		{
			_subViews[index].Parent = null;
			_subViews.RemoveAt(index);
		}

		/// <summary>
		/// Sets the native host for this view hierachy
		/// </summary>
		/// <param name="host">A reference to the host.</param>
		public void SetHost(IHost host)
		{
			if (_host != null) OnDetach();

			_host = host;

			if (_host != null) OnAttach(_host);
		}

		/// <summary>
		/// Overridden to locate the parent host for this view hierarchy
		/// </summary>
		/// <returns>The host.</returns>
		internal override IHost GetHost()
		{
			// If this view group has been parented into an actual UIView, we'll have a IHost reference
			// that acts as the host for all views in the hierarchy.  If not, ask our parent
			return _host ?? base.GetHost();
		}

		/// <summary>
		/// We've been attached to a hosting view, notify all subviews
		/// </summary>
		/// <param name="host">The Host.</param>
		internal override void OnAttach(IHost host)
		{
			// Add the layer
			if (_layer!=null) host.GetUIView().Layer.AddSublayer(_layer);

			// Forward on to all children
			foreach (var c in _subViews) c.OnAttach(host);
		}

		/// <summary>
		/// We've been detached from a hosting view, notify all subviews
		/// </summary>
		internal override void OnDetach()
		{
			// Remove from layer
			if (_layer != null) _layer.RemoveFromSuperLayer();

			// Forward on to all children
			foreach (var c in _subViews) c.OnDetach();
		}

		protected override void OnLayout(RectangleF newPosition, bool parentHidden)
		{
			// Reposition the layer
			if (_layer != null)
			{
				var newHidden = parentHidden || !Visible;
				if (newHidden != _layer.Hidden)
				{
					// If we're changing the visibility, disable animations since
					// the old rectangle for the position was probably wrong, resulting in 
					// weird animation as it's repositioned.
					CATransaction.Begin();
					CATransaction.DisableActions = true;
					_layer.Hidden = newHidden;
					_layer.Frame = newPosition;
					CATransaction.Commit();
				}
				else
				{
					if (!_layer.Hidden) _layer.Frame = newPosition;
				}
			}

			// Hide all subviews
			if (!parentHidden && Visible) return;

			foreach (var v in SubViews) v.Layout(RectangleF.Empty, false);
		}

		internal override View LayoutViewWithTag(int tag)
		{
			if (Tag == tag) return this;

			return _subViews.Select(v => v.LayoutViewWithTag(tag)).FirstOrDefault(result => result != null);
		}

		internal override UIView UIViewWithTag(int tag)
		{
			return _subViews.Select(v => v.UIViewWithTag(tag)).FirstOrDefault(result => result != null);
		}

		public override NativeView FindNativeView(UIView view)
		{
			return _subViews.Select(v => v.FindNativeView(view)).FirstOrDefault(result => result != null);
		}

		internal override CALayer FindFirstSublayer()
		{
			foreach (var v in SubViews)
			{
				var layer = v.GetDisplayLayer();
				if (layer != null) return layer;

				layer = v.FindFirstSublayer();
				if (layer != null) return layer;
			}

			return null;
		}

		internal override CALayer GetDisplayLayer()
		{
			return _layer;
		}

		public CALayer Layer
		{
			get { return _layer; }
			set
			{
				// Remove old layer
				if (_layer != null) _layer.RemoveFromSuperLayer();

				// Store it
				_layer = value;

				// If we're attached, add the layer to our host
				if (_layer == null) return;

				var host = GetHost();
				if (host == null) return;

				var hostView = host.GetUIView();
				var nextSubLayer = FindFirstSublayer();

				if (nextSubLayer != null) hostView.Layer.InsertSublayerBelow(_layer, nextSubLayer);
				else hostView.Layer.AddSublayer(_layer);
			}
		}
	}
}

