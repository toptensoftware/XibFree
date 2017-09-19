# XibFree - A lightweight, code-only layout engine for iOS/MonoTouch

## Sorry :( This Project is not longer under development or maintenance.  The repo is kept here for posterity only.

XibFree is a simple layout engine for code-only layouts in Xamarin.iOS (aka MonoTouch)

* Code only layouts - no more Xib files.
* Leverages C# language features to define layouts in a concise, flexible and powerful way.
* Supports LinearLayout - the workhorse of automatic layout.
* Supports FrameLayout - for overlaid views.
* Deliberately uses Android terminology and concepts make it instantly familiar for cross-platform developers.
* Extremely light weight, uses UIViews directly without any re-wrapping or property delegation.
* Easily integrated into any existing MonoTouch project - it's not a framework so you can use it as much or as little as you like!

### A Quick Example

The following is a really simple example to show what a typical XibFree layout looks like:

```C#
var layout = new LinearLayout(Orientation.Horizontal)
{
	Padding = new UIEdgeInsets(10, 10, 10, 10),
	LayoutParameters = new LayoutParameters()
	{
		Width = AutoSize.FillParent,
		Height = AutoSize.WrapContent,
	},
	SubViews = new View[]
	{
		new NativeView()
		{
			View = new UILabel()
			{
				Text = "Hello World, from XibFree",
				TextColor = UIColor.White,
				BackgroundColor = UIColor.Clear,
			},
			LayoutParameters = new LayoutParameters()
			{
				Width = AutoSize.FillParent,
				Height = AutoSize.FillParent,
			}
		},
		new NativeView()
		{
			View = new UIImageView()
			{
				Image = UIImage.FromBundle("tts512.png"),
				ContentMode = UIViewContentMode.ScaleAspectFit,
			},
			LayoutParameters = new LayoutParameters()
			{
				Width = 40,
				Height = 40,
			}
		}
	},
};
```

Which results in this:

![Screen Shot 2013-03-31 at 5.52.28 PM.png](<Doc/Screen%20Shot%202013-03-31%20at%205.52.28%20PM.png>)


### Why Code-Only Layout?

 Xamarin's Michael James says it pretty concisely on the [Xamarin iOS Forum](http://forums.xamarin.com/discussion/1164/ios-layout-in-c#):

> *... code-only layouts have a number of other advantages. To start with, many properties and behaviors of iOS layout are not exposed in Xcode’s Interface designer. Additionally, keeping layouts in C# also allows for easier management and tracking of UI changes for development teams and makes it easier to merge changes which can be extremely time consuming when developing with xibs.*

### Guide

* [Getting Started](Doc/getting_started.md)
* [Tutorial](Doc/tutorial.md)
* [Auto Sizing Views](Doc/auto_sizing_views.md)
* [Replacing Outlets and Actions](Doc/outlets_and_actions.md)
* [Updating the Layout](Doc/update_layout.md)
* [Nesting UILayoutHost](Doc/nesting_uilayouthost.md)
* [ViewGroup Layers](Doc/viewgroup_layers.md)
* [Leveraging C#](Doc/leveraging_csharp.md)

### Reference

* [View](Doc/view.md)
* [Viewgroup](Doc/viewgroup.md)
* [LinearLayout](Doc/linearlayout.md)
* [FrameLayout](Doc/framelayout.md)
* [NativeView](Doc/nativeview.md)
* [UILayoutHost](Doc/uilayouthost.md)
* [LayoutParameters](Doc/layoutparameters.md)

### Examples

* [Custom UITableViewCell (Fixed Height)](Doc/uitableviewcell_fixed.md)
* [Custom UITableViewCell (Variable Height)](Doc/uitableviewcell_variable.md)
* [Linear Layouts](Doc/example_1.md)
* [Frame Layouts](Doc/example_2.md)

### Download

Source code, including examples:

* <http://github.com/toptensoftware/xibfree>

Compiled DLL:

* [XibFree.dll](https://github.com/toptensoftware/XibFree/blob/master/out/XibFree.dll?raw=true)

### License

XibFree
Copyright 2013 Topten Software

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this product except in compliance with the License.
You may obtain a copy of the License at

<http://www.apache.org/licenses/LICENSE-2.0>

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.

 


