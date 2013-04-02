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
			get
			{
				return _margins;
			}

			set
			{
				_margins = value;
			}
		}


		/// <summary>
		/// Gets or sets the left margin.
		/// </summary>
		/// <value>The left margin size.</value>
		public float MarginLeft
		{
			get
			{
				return Margins.Left;
			}
			set
			{
				_margins.Left = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the right margin.
		/// </summary>
		/// <value>The right margin size.</value>
		public float MarginRight
		{
			get
			{
				return Margins.Right;
			}
			set
			{
				_margins.Right = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the top margin.
		/// </summary>
		/// <value>The top margin size.</value>
		public float MarginTop
		{
			get
			{
				return Margins.Top;
			}
			set
			{
				_margins.Top = value;
			}
		}
		
		/// <summary>
		/// Gets or sets the bottom margin.
		/// </summary>
		/// <value>The bottom margin size.</value>
		public float MarginBottom
		{
			get
			{
				return Margins.Bottom;
			}
			set
			{
				_margins.Bottom = value;
			}
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

		/// <summary>
		/// Gets or sets the visibility of this view
		/// </summary>
		/// <value>One of the Visibility constants</value>
		public Visibility Visibility
		{
			get;
			set;
		}

	
		UIEdgeInsets _margins;
	}
}

