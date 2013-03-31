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
	/// Gravity flags are used to specify how a subview (or subviews) should be aligned
	/// within a larger container
	/// </summary>
	[Flags]
	public enum Gravity
	{
		None = 0x0000,
		Left = 0x0001,
		Right = 0x0002,
		CenterHorizontal = 0x0004,
		Top = 0x0008,
		Bottom = 0x0010,
		CenterVertical = 0x0020,

		TopLeft = Top | Left,
		TopRight = Top | Right,
		TopCenter = Top | CenterHorizontal,
		BottomLeft = Bottom | Left,
		BottomRight = Bottom | Right,
		BottomCenter = Bottom | CenterHorizontal,
		LeftCenter = Left | CenterVertical,
		RightCenter = Right | CenterVertical,
		Center = CenterVertical | CenterHorizontal,

		HorizontalMask = Left | Right | CenterHorizontal,
		VerticalMask = Top | Bottom | CenterVertical,
	}
}

