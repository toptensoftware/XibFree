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

namespace XibFree
{
	/// <summary>
	/// AutoSize defines special float constants to indicate that views dimensions should
	/// be automatically determined by wrapping the views content, or matching the parent view's size
	/// </summary>
	public static class AutoSize
	{
		/// <summary>
		/// Specifies that a view's width or height should fill the available space in the parent view group.
		/// </summary>
		public const float FillParent = -1;

		/// <summary>
		/// Specifies that a view's width or height should wrap the content contained within it.
		/// </summary>
		public const float WrapContent = -2;
	}
}

