# Complete Full-Screen Layout Tutorial

This tutorial shows how to build a complete code-only, full-screen layout for iOS using XibFree.  It demonstrates a simple approach to styling, leverages C# language features to simplify defining the layout, shows how to make a scrollable view and more.  By the end of the tutorial we'll have covered everything to build this view:

![Screen Shot 2013-04-02 at 4.55.02 PM.png](Screen%20Shot%202013-04-02%20at%204.55.02%20PM.png)

In automatically scales for landscape mode, and is scrollable when the content doesn't fit:

![Screen Shot 2013-04-02 at 4.56.10 PM.png](Screen%20Shot%202013-04-02%20at%204.56.10%20PM.png)

This tutorial assumes you've read about the basics of working with XibFree.  If not, please read the [getting started](getting_started.md) page.

### Basic Framework

Lets start by setting up the basic framework in a UIViewController.  Most of this is pretty standard UIViewController and XibFree code and has been explained elsewhere.

```C#
using System;
using MonoTouch.UIKit;
using XibFree;
using System.Drawing;
using MonoTouch.CoreAnimation;
using MonoTouch.Foundation;

namespace Demo
{
	public class FullScreenDemo : UIViewController
	{
		public FullScreenDemo()
		{
			this.Title = "XibFree";
		}

		public override void LoadView()
		{
			// Create the layout
			var layout = new LinearLayout(Orientation.Vertical)
			{
				Padding = new UIEdgeInsets(10,10,10,10),
				Gravity = Gravity.CenterHorizontal,
				SubViews = new View[]
				{
					new NativeView()
					{
						View = new UIImageView()
						{
							Image = UIImage.FromBundle("XibFree_512.png"),
							ContentMode = UIViewContentMode.ScaleAspectFit,
						},
						LayoutParameters = new LayoutParameters()
						{
							Width = 120,
							Height = 120,
							MarginTop = 30,
							MarginBottom = 20,
						}
					},
				}
			};

			// Create a UILayoutHost view to host the layout
			this.View = new UILayoutHost(layout)
			{
				// Yellowish background color
				BackgroundColor = UIColor.FromRGB(0xF1, 0xE8, 0xDC),
			};
		}

		[Obsolete ("Deprecated in iOS6. Replace it with both GetSupportedInterfaceOrientations and PreferredInterfaceOrientationForPresentation")]
		public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}
	}
}
```

So far we have this:

![Screen Shot 2013-04-02 at 5.05.58 PM.png](Screen%20Shot%202013-04-02%20at%205.05.58%20PM.png)

### Creating a stylized label

Since we have a few labels that are all very similar we'll create a new class `Label` that defines these properties and then re-use as necessary.  In this example I'm just declaring it as a nested class in the UIViewController but for a real-world project you'd probably setup classes that can be re-used across multiple views.

By inheriting from NativeView we can encapsulate everything to do with the label and reduce what need to type in the layout definition.

```C#
class Label : NativeView
{
    public Label(string title, UIFont font)
    {
        View = new UILabel(RectangleF.Empty)
        {
            Text = title,
            Font = font,
            BackgroundColor = UIColor.Clear,
            TextColor = UIColor.DarkGray,
        };
            
        LayoutParameters = new LayoutParameters(AutoSize.WrapContent, AutoSize.WrapContent);
    }
}
```

Use it in the layout like this:

```C#
SubViews = new View[]
{
    ...
    new Label("XibFree", UIFont.BoldSystemFontOfSize(24)),
    new Label("Code-only layout for Xamarin.iOS", UIFont.SystemFontOfSize(12)),
    ...
}
```

![Screen Shot 2013-04-02 at 5.10.16 PM.png](Screen%20Shot%202013-04-02%20at%205.10.16%20PM.png)

### Nested LinearLayout and Stylized Buttons

Next are the two horizontally stacked buttons.  Just like the labels, we'll define a class to wrap up the look and feel of the buttons. This class also has an callback handler that we'll invoke when the button is tapped:

```C#
class Button : NativeView
{
	public Button(string title, Action handler)
	{
		// Setup the button
		var button = new UIButton(UIButtonType.RoundedRect);
		button.SetTitle(title, UIControlState.Normal);
		View = button;

		// Attach an event handler and forward the event
		button.TouchUpInside += (sender, e) => handler();

		// Setup the layout parameters
		LayoutParameters = new LayoutParameters(AutoSize.FillParent, AutoSize.WrapContent);
	}
}
```

and update the layout as follows.  Note we're using a previously unseen property `Spacing` to set the spacing between the buttons:

```C#
new LinearLayout(Orientation.Horizontal)
{
    Spacing = 10,
    SubViews = new View[]
    {
        new Button("Download", () => Alert("Download")),
        new Button("View Samples", () => Alert("Samples")),
    },
    LayoutParameters = new LayoutParameters()
    {
        Width = AutoSize.FillParent,
        Height = AutoSize.WrapContent,
        MarginTop = 50,
    }
},

// Somewhere else in the class, this helper function:
void Alert(string message)
{
    new UIAlertView(message, "",  null, "OK").Show();
}
```

The layout's correct and the buttons work but they look a bit old school:

![Screen Shot 2013-04-02 at 8.59.42 PM.png](Screen%20Shot%202013-04-02%20at%208.59.42%20PM.png)

### Integrating a Custom Control with XibFree

Now we'll create a custom button that'll look a lot better.  We'll use a couple of Core Animation layers to put a nice semi-transparent gradient on the background.

```C#
[Register("GlassButton")]
class GlassButton : UIButton
{
	public GlassButton() : base(RectangleF.Empty)
	{
		// Create a mostly transparent gradient for the button background
		_layerGradient = new CAGradientLayer()
		{
			Colors = new MonoTouch.CoreGraphics.CGColor[]
			{
				new MonoTouch.CoreGraphics.CGColor(1,1,1,0.5f),
				new MonoTouch.CoreGraphics.CGColor(1,1,1,0.1f)
			},
			Locations = new NSNumber[]
			{
				0.0f,
				1.0f
			},
			CornerRadius = 5,
			Frame = this.Bounds,
		};
		
		// Create another mostly transparent layer to darken the button when it's pressed
		_layerDarken = new CALayer()
		{
			BackgroundColor = new MonoTouch.CoreGraphics.CGColor(0,0,0,0.2f),
			CornerRadius = 5,
			Frame = this.Bounds,
			Hidden = true,		// Normally hidden
		};
		
		// Add the sub layers
		Layer.AddSublayer(_layerDarken);
		Layer.AddSublayer(_layerGradient);
		
		// Put on a rounded border
		Layer.BorderWidth = 1;
		Layer.BorderColor = new MonoTouch.CoreGraphics.CGColor(0,0,0,0.2f);
		Layer.CornerRadius = 5;
		
		// Setup the title text color
		SetTitleColor(UIColor.DarkGray, UIControlState.Normal);
	}
	
	public override RectangleF Frame
	{
		set
		{
			base.Frame = value;
			
			// Whenever the button is moved, reposition the layers
			if (_layerDarken!=null)
			{
				_layerDarken.Frame = Bounds;
				_layerGradient.Frame = Bounds;
			}
		}
	}
	
	// SizeThatFits is called by XibFree to measure the layout.  UIButton doesn't include much padding by default so we'll
	// add a bit to height to make it look better
	public override SizeF SizeThatFits(SizeF size)
	{
		return base.SizeThatFits(size) + new SizeF(0,10);
	}
	
	public override bool Highlighted
	{
		set
		{
			// When the button is pressed, show the darkening layer
			_layerDarken.Hidden = !value;
			
			base.Highlighted = value;
		}
	}
	
	
	CALayer _layerGradient;
	CALayer _layerDarken;
}
```

Mostly this is fairly standard Xamarin.iOS stuff but there's a few things to note:

1. This isn't specific to XibFree, but the constructor must not call base(UIButtonType.Custom).  This results in a `UIButton` class being created over in Objective-C land rather than our `GlassButton` class and the virtual Highlighted property won't get called.
2. To measure how big the button should be, XibFree will be calling `SizeThatFits`.  By default, UIButton doesn't include a lot of space around the button. (It does for rounded rect buttons, but not custom ones).  So we just add a bit before returning the size.

Now we just need to update our XibFree `Button` class to use `GlassButton` instead of `UIButton`:

```C#
class Button : NativeView
{
    ...
    //var button = new UIButton(UIButtonType.RoundedRect);             <-- replace this with below
    var button = new GlassButton();
    ...
}
```

This looks much better:

![Screen Shot 2013-04-02 at 9.12.54 PM.png](Screen%20Shot%202013-04-02%20at%209.12.54%20PM.png)

### Finishing off the layout

The rest of the layout doesn't need any explanation - it's just a plain UIView for the horizontal line and another label:

```C#
new NativeView()
{
	View = new UIView()
	{
		BackgroundColor = UIColor.FromRGBA(0, 0, 0, 10),
	},
	LayoutParameters = new LayoutParameters()
	{
		Width = AutoSize.FillParent,
		Height = 2,
		MarginTop = 20,
		MarginBottom = 20,
	}
},
new Label("Step away from the mouse, build your UI in code!", UIFont.SystemFontOfSize(12)),
```

### Making it Scrollable

So that's it for the layout code - it looks pretty good, and the layout recalculates and updates when the device is rotated to landscape orientation but:  it no longer fits on the screen.  Lets make it scrollable by replacing `UILayoutHost` with `UILayoutHostScrollable`:

```C#
// Create a UILayoutHost view to host the layout
// this.View = new UILayoutHost(layout)    <-- Replace this with below
this.View = new UILayoutHostScrollable(layout)
```

The above will make the view scrollable, but the scroll range will be way too big and you'll be able to scroll a *long* way.  The reason for this is that the root view of the layout has a layout height of `AutoSize.FillParent`.  For scrolling to work that root view should have a height of `AutoSize.WrapContent`.  Let's fix that:

```C#
// Create the layout
var layout = new LinearLayout(Orientation.Vertical)
{
	...
	LayoutParameters = new LayoutParameters(AutoSize.FillParent, AutoSize.WrapContent),
	...
```

That's it!  We now have everything in place:

![Screen Shot 2013-04-02 at 4.56.10 PM.png](Screen%20Shot%202013-04-02%20at%204.56.10%20PM.png)

### Full Source Code

Here's the full source code for everything described here:

```C#
using System;
using MonoTouch.UIKit;
using XibFree;
using System.Drawing;
using MonoTouch.CoreAnimation;
using MonoTouch.Foundation;

namespace Demo
{
	public class FullScreenDemo : UIViewController
	{
		public FullScreenDemo()
		{
			this.Title = "XibFree";
		}

		[Register("GlassButton")]
		class GlassButton : UIButton
		{
			public GlassButton() : base(RectangleF.Empty)
			{
				// Create a mostly transparent gradient for the button background
				_layerGradient = new CAGradientLayer()
				{
					Colors = new MonoTouch.CoreGraphics.CGColor[]
					{
						new MonoTouch.CoreGraphics.CGColor(1,1,1,0.5f),
						new MonoTouch.CoreGraphics.CGColor(1,1,1,0.1f)
					},
					Locations = new NSNumber[]
					{
						0.0f,
						1.0f
					},
					CornerRadius = 5,
					Frame = this.Bounds,
				};
				
				// Create another mostly transparent layer to darken the button when it's pressed
				_layerDarken = new CALayer()
				{
					BackgroundColor = new MonoTouch.CoreGraphics.CGColor(0,0,0,0.2f),
					CornerRadius = 5,
					Frame = this.Bounds,
					Hidden = true,		// Normally hidden
				};
				
				// Add the sub layers
				Layer.AddSublayer(_layerDarken);
				Layer.AddSublayer(_layerGradient);
				
				// Put on a rounded border
				Layer.BorderWidth = 1;
				Layer.BorderColor = new MonoTouch.CoreGraphics.CGColor(0,0,0,0.2f);
				Layer.CornerRadius = 5;
				
				// Setup the title text color
				SetTitleColor(UIColor.DarkGray, UIControlState.Normal);
			}
			
			public override RectangleF Frame
			{
				set
				{
					base.Frame = value;
					
					// Whenever the button is moved, reposition the layers
					if (_layerDarken!=null)
					{
						_layerDarken.Frame = Bounds;
						_layerGradient.Frame = Bounds;
					}
				}
			}
			
			// SizeThatFits is called by XibFree to measure the layout.  UIButton doesn't include much padding by default so we'll
			// add a bit to height to make it look better
			public override SizeF SizeThatFits(SizeF size)
			{
				return base.SizeThatFits(size) + new SizeF(0,10);
			}
			
			public override bool Highlighted
			{
				set
				{
					// When the button is pressed, show the darkening layer
					_layerDarken.Hidden = !value;
					
					base.Highlighted = value;
				}
			}
			
			
			CALayer _layerGradient;
			CALayer _layerDarken;
		}
		
		class Label : NativeView
		{
			public Label(string title, UIFont font)
			{
				View = new UILabel(RectangleF.Empty)
				{
					Text = title,
					Font = font,
					BackgroundColor = UIColor.Clear,
					TextColor = UIColor.DarkGray,
				};
				
				LayoutParameters = new LayoutParameters(AutoSize.WrapContent, AutoSize.WrapContent);
			}
		}
		
		class Button : NativeView
		{
			public Button(string title, Action handler)
			{
				// Setup the button
				//var button = new UIButton(UIButtonType.RoundedRect);
				var button = new GlassButton();
				button.SetTitle(title, UIControlState.Normal);
				View = button;

				// Attach an event handler and forward the event
				button.TouchUpInside += (sender, e) => handler();

				// Setup the layout parameters
				LayoutParameters = new LayoutParameters(AutoSize.FillParent, AutoSize.WrapContent);
			}
		}

		public override void LoadView()
		{
			// Create the layout
			var layout = new LinearLayout(Orientation.Vertical)
			{
				Padding = new UIEdgeInsets(10,10,10,10),
				Gravity = Gravity.CenterHorizontal,
				LayoutParameters = new LayoutParameters(AutoSize.FillParent, AutoSize.WrapContent),
				SubViews = new View[]
				{
					new NativeView()
					{
						View = new UIImageView()
						{
							Image = UIImage.FromBundle("XibFree_512.png"),
							ContentMode = UIViewContentMode.ScaleAspectFit,
						},
						LayoutParameters = new LayoutParameters()
						{
							Width = 120,
							Height = 120,
							MarginTop = 30,
							MarginBottom = 20,
						}
					},
					new Label("XibFree", UIFont.BoldSystemFontOfSize(24)),
					new Label("Code-only layout for Xamarin.iOS", UIFont.SystemFontOfSize(12)),
					new LinearLayout(Orientation.Horizontal)
					{
						Spacing = 10,
						SubViews = new View[]
						{
							new Button("Download", () => Alert("Download")),
							new Button("View Samples", () => Alert("Samples")),
						},
						LayoutParameters = new LayoutParameters()
						{
							Width = AutoSize.FillParent,
							Height = AutoSize.WrapContent,
							MarginTop = 50,
						}
					},
					new NativeView()
					{
						View = new UIView()
						{
							BackgroundColor = UIColor.FromRGBA(0, 0, 0, 10),
						},
						LayoutParameters = new LayoutParameters()
						{
							Width = AutoSize.FillParent,
							Height = 2,
							MarginTop = 20,
							MarginBottom = 20,
						}
					},
					new Label("Step away from the mouse, build your UI in code!", UIFont.SystemFontOfSize(12)),
				}
			};

			// Create a UILayoutHost view to host the layout
			this.View = new UILayoutHostScrollable(layout)
			{
				// Yellowish background color
				BackgroundColor = UIColor.FromRGB(0xF1, 0xE8, 0xDC),
			};
		}

		void Alert(string message)
		{
			new UIAlertView(message, "",  null, "OK").Show();
		}
		


		[Obsolete ("Deprecated in iOS6. Replace it with both GetSupportedInterfaceOrientations and PreferredInterfaceOrientationForPresentation")]
		public override bool ShouldAutorotateToInterfaceOrientation(UIInterfaceOrientation toInterfaceOrientation)
		{
			return true;
		}
	}
}

```
