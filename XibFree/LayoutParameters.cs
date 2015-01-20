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

namespace XibFree
{
	public enum Units
	{
		Absolute,			// Absolute pixel dimension
		ParentRatio,		// Ratio of parent size
		ContentRatio,		// Ratio of content size
		AspectRatio,		// Ratio of adjacent dimension size
		ScreenRatio,		// Ratio of the current screen size
		HostRatio,			// Ratio of the current UIViewHost window size
	}


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

		/// <summary>
		/// Initializes a new instance of the <see cref="XibFree.LayoutParameters"/> class.
		/// </summary>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
		/// <param name="weight">Weight.</param>
		public LayoutParameters(nfloat width, nfloat height, double weight=1.0)
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
		public nfloat Width
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the height for this view
		/// </summary>
		/// <value>The height in pixels, or one of the AutoSize constants.</value>
		public nfloat Height
		{
			get;
			set;
		}

		Units _widthUnits;

		/// <summary>
		/// Gets or sets the width units.
		/// </summary>
		/// <value>The width units.</value>
		public Units WidthUnits
		{
			get
			{
				if (_widthUnits == Units.Absolute)
				{
					if (Width == AutoSize.FillParent)
						return Units.ParentRatio;
					if (Width == AutoSize.WrapContent)
						return Units.ContentRatio;
				}
				return _widthUnits;
			}
			set
			{
				_widthUnits = value;
			}
		}
		
		Units _heightUnits;
		/// <summary>
		/// Gets or sets the height units.
		/// </summary>
		/// <value>The height units.</value>
		public Units HeightUnits
		{
			get
			{
				if (_heightUnits == Units.Absolute)
				{
					if (Height == AutoSize.FillParent)
						return Units.ParentRatio;
					if (Height == AutoSize.WrapContent)
						return Units.ContentRatio;
				}
				return _heightUnits;
			}
			set
			{
				_heightUnits = value;
			}
		}

		internal nfloat HeightRatio
		{
			get
			{
				if (_heightUnits == Units.Absolute)
				{
					return 1;
				}
				else
				{
					return Height;
				}
			}
		}

		internal nfloat WidthRatio
		{
			get
			{
				if (_widthUnits == Units.Absolute)
				{
					return 1;
				}
				else
				{
					return Width;
				}
			}
		}

		/// <summary>
		/// Gets or sets the weight of a AutoSize.FillParent view relative to its sibling views
		/// </summary>
		/// <value>The weighting value for this view's size.</value>
		public double Weight
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
		public nfloat MarginLeft
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
		public nfloat MarginRight
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
		public nfloat MarginTop
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
		public nfloat MarginBottom
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

		/// <summary>
		/// Gets or sets the minimum width.
		/// </summary>
		/// <value>The minimum width.</value>
		public nfloat MinWidth
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the maximum width.
		/// </summary>
		/// <value>The maximum width.</value>
		public nfloat MaxWidth
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the minimum height.
		/// </summary>
		/// <value>The minimum height.</value>
		public nfloat MinHeight
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the max height.
		/// </summary>
		/// <value>The maximum height</value>
		public nfloat MaxHeight
		{
			get;
			set;
		}

		static nfloat TryResolve(Units units, nfloat size, nfloat ratio, nfloat parentSize)
		{
			switch (units)
			{
				case Units.Absolute:
					return size;
					
				case Units.ParentRatio:
					return parentSize==nfloat.MaxValue ? nfloat.MaxValue : parentSize * ratio;

				default:
					return nfloat.MaxValue;
			}
		}

		static CGSize GetScreenSize()
		{
			var orientation = UIApplication.SharedApplication.StatusBarOrientation;
			if (orientation == UIInterfaceOrientation.Portrait || orientation == UIInterfaceOrientation.PortraitUpsideDown)
			{
				return UIScreen.MainScreen.Bounds.Size;
			}
			else
			{
				var temp = UIScreen.MainScreen.Bounds.Size;
				return new CGSize(temp.Height, temp.Width);
			}
		}

		internal CGSize GetHostSize(View view)
		{
			// Get the host
			var host = view.GetHost();
			if (host==null)
				return GetScreenSize();

			var hostView = host.GetUIView();

			// Use outer scroll view if present
			var parent = hostView.Superview;
			if (parent is UIScrollView)
				hostView = parent;

			// Return size
			return hostView.Bounds.Size;
		}

		internal nfloat TryResolveWidth(View view, nfloat parentWidth)
		{
			if (WidthUnits==Units.HostRatio)
			{
				return GetHostSize(view).Width * WidthRatio;
			}

			if (WidthUnits==Units.ScreenRatio)
			{
				return GetScreenSize().Width * WidthRatio;
			}

			return TryResolve(WidthUnits, Width, WidthRatio, parentWidth);
		}
	
		internal nfloat TryResolveHeight(View view, nfloat parentHeight)
		{
			if (HeightUnits==Units.HostRatio)
			{
				return GetHostSize(view).Height * HeightRatio;
			}
			
			if (HeightUnits==Units.ScreenRatio)
			{
				return GetScreenSize().Height * HeightRatio;
			}

			return TryResolve(HeightUnits, Height, HeightRatio, parentHeight);
		}

		internal CGSize ResolveSize(CGSize size, CGSize sizeMeasured)
		{
			// Resolve measured size
			if (size.Width == nfloat.MaxValue)
				size.Width = sizeMeasured.Width;
			if (size.Height == nfloat.MaxValue)
				size.Height = sizeMeasured.Height;

			// Finally, resolve aspect ratios
			if (WidthUnits == Units.AspectRatio)
			{
				size.Width = size.Height * WidthRatio;
			}
			if (HeightUnits == Units.AspectRatio)
			{
				size.Height = size.Width * HeightRatio;
			}

			return size;
		}

		UIEdgeInsets _margins;
	}
}

