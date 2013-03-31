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

namespace XibFree
{
	/// <summary>
	/// Base class for all views that can layout a set of subviews
	/// </summary>
	public abstract class ViewGroup : View
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="XibFree.ViewGroup"/> class.
		/// </summary>
		public ViewGroup()
		{
			LayoutParameters.Width = AutoSize.FillParent;
			LayoutParameters.Height = AutoSize.FillParent;
		}

		/// <summary>
		/// Gets or sets all the subviews of this view group
		/// </summary>
		/// <value>The sub views.</value>
		public IEnumerable<View> SubViews
		{
			get
			{
				return _subViews;
			}
			set
			{
				// Check none of the child already have parents
				foreach (var c in value)
				{
					if (c.Parent!=null)
						throw new InvalidOperationException("View is already a child of another ViewGroup");
				}

				foreach (var c in value)
				{
					c.Parent = this;
				}

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
		/// <param name="child">The subview to add</param>
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
			for (int i=0; i<_subViews.Count; i++)
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
			if (view.Parent!=null)
				throw new InvalidOperationException("View is already a child of another ViewGroup");
			view.Parent = this;

			if (position<0)
				position = _subViews.Count;
			_subViews.Insert(position, view);
		}
	
		/// <summary>
		/// Remove the specified subview
		/// </summary>
		/// <param name="view">The subview to remove</param>
		public void RemoveSubView(ViewGroup view)
		{
			RemoveSubViewAt(_subViews.IndexOf(view));
		}

		/// <summary>
		/// Remove the subview at a specified position
		/// </summary>
		/// <param name="index">The zero-based index of the view to remove.</param>
		public void RemoveSubViewAt(int index)
		{
			_subViews.RemoveAt(index);
		}

		/// <summary>
		/// Gets or sets the padding that should be applied around the subviews contained in this view group
		/// </summary>
		/// <value>The padding.</value>
		public UIEdgeInsets Padding
		{
			get
			{
				return _padding;
			}
			set
			{
				_padding = value;
			}
		}

		public interface IHost
		{
			UIView GetUIView();
		}


		/// <summary>
		/// Sets the native host for this view hierachy
		/// </summary>
		/// <param name="host">A reference to the host.</param>
		public void SetHost(IHost host)
		{
			if (_host!=null)
			{
				onDetach();
			}

			_host = host;

			if (_host!=null)
			{
				onAttach(_host);
			}
		}

		/// <summary>
		/// Overridden to locate the parent host for this view hierarchy
		/// </summary>
		/// <returns>The host.</returns>
		protected override ViewGroup.IHost GetHost()
		{
			// If this view group has been parented into an actual UIView, we'll have a IHost reference
			// that acts as the host for all views in the hierarchy.  If not, ask our parent
			if (_host==null)
				return base.GetHost();
			else
				return _host;
		}

		/// <summary>
		/// We've been attached to a hosting view, notify all subviews
		/// </summary>
		/// <param name="host">The Host.</param>
		internal override void onAttach(IHost host)
		{
			// Forward on to all children
			foreach (var c in _subViews)
				c.onAttach(host);
		}

		/// <summary>
		/// We've been detached from a hosting view, notify all subviews
		/// </summary>
		internal override void onDetach()
		{
			// Forward on to all children
			foreach (var c in _subViews)
				c.onDetach();
		}

		// Fields
		List<View> _subViews = new List<View>();
		UIEdgeInsets _padding = UIEdgeInsets.Zero;
		IHost _host;
	}
}

