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
	/// <summary>
	/// LayoutParameters declare how a view should be laid out by it's parent view group.
	/// </summary>
	public class LayoutParameters
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="XibFree.LayoutParameters"/> class.
		/// </summary>
		public LayoutParameters()
		{
			Width = AutoSize.WrapContent;
			Height = AutoSize.WrapContent;
			Margins = UIEdgeInsets.Zero;
			Weight = 1;
			Gravity = Gravity.None;
		}

		public LayoutParameters(float width, float height, float weight=1)
		{
			Width = width;
			Height = height;
			Margins = UIEdgeInsets.Zero;
			Weight = 1;
			Gravity = Gravity.None;
		}

		/// <summary>
		/// Gets or sets the width for this view
		/// </summary>
		/// <value>The width in pixels, or one of the AutoSize constants.</value>
		public float Width
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the height for this view
		/// </summary>
		/// <value>The height in pixels, or one of the AutoSize constants.</value>
		public float Height
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the weight of a AutoSize.FillParent view relative to its sibling views
		/// </summary>
		/// <value>The weighting value for this view's size.</value>
		public float Weight
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the whitepsace margins that should be left around a view
		/// </summary>
		/// <value>The margins sizes</value>
		public UIEdgeInsets Margins
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the gravity for this view within it's parent subview
		/// </summary>
		/// <value>One of the gravity constants</value>
		public Gravity Gravity
		{
			get;
			set;
		}
	}
}

