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
using System.Collections.Generic;

namespace XibFree
{
	public static class PublicExtensions
	{
		// Find the UILayoutHost associated with a UIView
		public static UILayoutHost GetLayoutHost(this UIView view)
		{
			if (view==null)
				return null;

			var host = view.Superview as UILayoutHost;
			if (host==null)
				throw new InvalidOperationException("Unable to find UILayoutHost for view - either the view is not part of a XibFree layout, or the layout hasn't been hosted by a UILayoutHost yet");

			return host;
		}

		// Find the NativeView associated with a UIView
		public static NativeView GetNativeView(this UIView view)
		{
			var host = view.GetLayoutHost();
			return host.FindNativeView(view);
		}

		// Get the root layout containing this UIView
		public static View GetLayoutRoot(this UIView view)
		{
			var host = view.GetLayoutHost();
			return host.Layout;
		}

        /// <summary>
        /// Returns the top-level ViewGroup that this view is
        /// a part of. Returns null if it is not under a group
        /// yet. If view is the root, it is returned.
        /// </summary>
        public static ViewGroup FindRootGroup(this View view)
        {
            if (view.Parent == null && view is ViewGroup)
            {
                return (ViewGroup)view;
            }

            ViewGroup parent = null;
            while (view.Parent != null)
            {
                view = parent = view.Parent;
            }
            return parent;
        }

        /// <summary>
        /// Returns the top-level UIView that this view is being hosted under.
        /// Returns null if not hosted.
        /// </summary>
        public static UIView FindRootUIView(this View view)
        {
            return FindRootGroup(view)?.GetHost()?.GetUIView();
        }

        /// <summary>
        /// Returns the nearest parent ViewGroup that this view is
        /// a part of. Returns null if it is not under a group
        /// yet. Returns the view passed if it is actually a
        /// ViewGroup.
        /// </summary>
        public static ViewGroup FindNearestGroup(this View view)
        {
            while (true)
            {
                if (view is ViewGroup)
                {
                    return (ViewGroup)view;
                }
                view = view.Parent;
                if (view == null)
                {
                    return null;
                }
            }
        }

        /// <summary>
        /// Returns the UIView associated with nearest parent ViewGroup 
        /// that this view is a part of. Returns null if it is not under 
        /// a group yet. Returns the UIView for the view passed if it is 
        /// actually a ViewGroup.
        /// </summary>
        public static UIView FindNearestGroupUIView(this View view)
        {
            return view.FindNearestGroup()?.GetHost()?.GetUIView();
        }

        /// <summary>
        /// Returns all UIViews at or under the passed view.
        /// </summary>
        public static IEnumerable<UIView> FindUIViews(this View view)
        {
            var views = new List<UIView>();
            if (view is NativeView)
            {
                views.Add((view as NativeView).View);
            }
            else if (view is ViewGroup)
            {
                foreach (var child in (view as ViewGroup).SubViews)
                {
                    views.AddRange(FindUIViews(child));
                }
            }
            return views;
        }
	}
}