using System.Drawing;

using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.CoreAnimation;

using XibFree;

namespace Demo
{
	public sealed class VisibilityDemo : UIViewController
	{
		public VisibilityDemo()
		{
			Title = "Visibility";

			// Custom initialization
		}

		public override void LoadView()
		{
			View panel;
			var layout = new LinearLayout(Orientation.Vertical)
			{
				SubViews = new View[] 
				{
					new NativeView
					{
						View = new UIView
						{
							BackgroundColor = UIColor.Blue
						},
						LayoutParameters = new LayoutParameters
						{
							Width = Dimension.FillParent,
							Height = Dimension.Absolute(50),
						},
					},
					new LinearLayout(Orientation.Vertical)
					{
						Padding = new UIEdgeInsets(10,10,10,10),
						Layer = new CAGradientLayer
						{
							Colors = new[]
							{
								new MonoTouch.CoreGraphics.CGColor(0.9f, 0.9f, 0.9f, 1f),
								new MonoTouch.CoreGraphics.CGColor(0.7f, 0.7f, 0.7f, 1f)
							},
							Locations = new NSNumber[]
							{
								0.0f,
								1.0f
							},
							CornerRadius = 5,
						},
						SubViews = new[]
						{
							new NativeView
							{
								View = new UILabel(RectangleF.Empty)
								{
									Text="Hello World",
									Font = UIFont.SystemFontOfSize(24),
									BackgroundColor = UIColor.Clear,
								}
							},
							panel = new NativeView
							{
								View = new UILabel(RectangleF.Empty)
								{
									Text="Goodbye",
									Font = UIFont.SystemFontOfSize(24),
									BackgroundColor = UIColor.Clear,
								}
							}
						},
						LayoutParameters = new LayoutParameters
						{
							Width = Dimension.FillParent,
							Height = Dimension.WrapContent,
							Margins = new UIEdgeInsets(10,10,10,10),
						},
					},
					new NativeView
					{
						View = new UIView
						{
							BackgroundColor = UIColor.Blue
						},
						LayoutParameters = new LayoutParameters
						{
							Width = Dimension.FillParent,
							Height = Dimension.Absolute(50),
						},
					},
					new NativeView
					{
						View = new UIButton(UIButtonType.RoundedRect),
						LayoutParameters = new LayoutParameters
						{
							Width = Dimension.FillParent,
							Height = Dimension.FillParent,
						},
						Init = v =>
						{
							v.As<UIButton>().SetTitle("Change Visibility", UIControlState.Normal);
							v.As<UIButton>().TouchUpInside += (sender, e) => 
							{
								switch (panel.LayoutParameters.Visibility)
								{
									case Visibility.Gone:
										panel.LayoutParameters.Visibility = Visibility.Visible;
										break;
									case Visibility.Visible:
										panel.LayoutParameters.Visibility = Visibility.Invisible;
										break;
									case Visibility.Invisible:
										panel.LayoutParameters.Visibility = Visibility.Gone;
										break;
								}

								View.SetNeedsLayout();
							};
						}
					}
				},
			};

			// We've now defined our layout, to actually use it we simply create a UILayoutHost control and pass it the layout
			View = new UILayoutHost(layout);
			View.BackgroundColor = UIColor.Gray;
		}
	}
}
