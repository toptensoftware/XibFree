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
	}
}
