using System.Drawing;
using MonoTouch.UIKit;
using XibFree;

namespace Demo
{
	public sealed class NestedDemo : UIViewController
	{
		public NestedDemo()
		{
			Title = "Nested UILayoutViews";

			// Custom initialization
		}

		public override void LoadView()
		{
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
					new NativeView
					{
						View = new UILayoutHost
						{
							BackgroundColor = UIColor.Yellow,
							Layout = new LinearLayout(Orientation.Vertical)
							{
								Padding = new UIEdgeInsets(3,3,3,3),
								SubViews = new View[]
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
									new NativeView
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
						},
						Init = v =>
						{
							v.View.Layer.CornerRadius = 5;
							v.View.Layer.MasksToBounds = true;
						}
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
						}
					}
				},
			};

			// We've now defined our layout, to actually use it we simply create a UILayoutHost control and pass it the layout
			View = new UILayoutHost(layout);
			View.BackgroundColor=UIColor.Gray;
		}
	}
}
