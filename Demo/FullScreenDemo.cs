using System;
using MonoTouch.UIKit;
using XibFree;
using System.Drawing;
using MonoTouch.CoreAnimation;
using MonoTouch.Foundation;

namespace Demo
{
	public sealed class FullScreenDemo : UIViewController
	{
		public FullScreenDemo()
		{
			Title = "XibFree";
		}

		[Register("GlassButton")]
		private sealed class GlassButton : UIButton
		{
			private readonly CALayer _layerGradient;
			private readonly CALayer _layerDarken;

			public GlassButton() : base(RectangleF.Empty)
			{
				// Create a mostly transparent gradient for the button background
				_layerGradient = new CAGradientLayer
				{
					Colors = new[]
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
					Frame = Bounds,
				};
				
				// Create another mostly transparent layer to darken the button when it's pressed
				_layerDarken = new CALayer
				{
					BackgroundColor = new MonoTouch.CoreGraphics.CGColor(0,0,0,0.2f),
					CornerRadius = 5,
					Frame = Bounds,
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
		}
		
		private class Label : NativeView
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

				LayoutParameters = new LayoutParameters
				{
					Width = Dimension.WrapContent,
					Height = Dimension.WrapContent,
				};
			}
		}
		
		private class Button : NativeView
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
				LayoutParameters = new LayoutParameters
				{
					Width = Dimension.FillParent,
					Height = Dimension.WrapContent,
					MaxWidth = 160,
				};
			}
		}

		public override void LoadView()
		{
			// Create the layout
			var layout = new LinearLayout(Orientation.Vertical)
			{
				Padding = new UIEdgeInsets(10,10,10,10),
				Gravity = Gravity.CenterHorizontal,
				LayoutParameters = new LayoutParameters
				{
					Width = Dimension.FillParent,
					Height = Dimension.WrapContent,
				},
				SubViews = new View[]
				{
					new NativeView
					{
						View = new UIImageView
						{
							Image = UIImage.FromBundle("XibFree_512.png"),
							ContentMode = UIViewContentMode.ScaleAspectFit,
						},
						LayoutParameters = new LayoutParameters
						{
							Width = Dimension.Absolute(120),
							Height = Dimension.Absolute(120),
							MarginTop = 30,
							MarginBottom = 20,
						}
					},
					new Label("XibFree", UIFont.BoldSystemFontOfSize(24)),
					new Label("Code-only layout for Xamarin.iOS", UIFont.SystemFontOfSize(12)),
					new LinearLayout(Orientation.Horizontal)
					{
						Spacing = 10,
						Gravity = Gravity.CenterHorizontal,
						SubViews = new View[]
						{
							new Button("Download", () => Alert("Download")),
							new Button("View Samples", () => Alert("Samples"))
						},
						LayoutParameters = new LayoutParameters
						{
							Width = Dimension.FillParent,
							Height = Dimension.WrapContent,
							MarginTop = 50,
						}
					},
					new NativeView
					{
						View = new UIView
						{
							BackgroundColor = UIColor.FromRGBA(0, 0, 0, 10),
						},
						LayoutParameters = new LayoutParameters
						{
							Width = Dimension.FillParent,
							Height = Dimension.Absolute(2),
							MarginTop = 20,
							MarginBottom = 20,
						}
					},
					new Label("Step away from the mouse, build your UI in code!", UIFont.SystemFontOfSize(12))
				}
			};

			// Create a UILayoutHost view to host the layout
			View = new UILayoutHostScrollable(layout)
			{
				// Yellowish background color
				BackgroundColor = UIColor.FromRGB(0xF1, 0xE8, 0xDC),
			};
		}

		private static void Alert(string message)
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

