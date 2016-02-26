using System;
using CoreGraphics;
using System.Collections.Generic;

using UIKit;
using Foundation;

using XibFree;

namespace Demo
{
    public partial class NestedDemoVisibilityBug : UIViewController
    {
        public NestedDemoVisibilityBug()
        {
            Title = "Nested UILayoutViews Bug";

            // Custom initialization
        }

        private NativeView GetNestedHost()
        {
            var layout = new LinearLayout(Orientation.Vertical)
            {
                SubViews = new[]
                {
                    new NativeView
                    {
                        View = new UILabel()
                        {
                            Text = "nested object",
                        }
                    },
                },
            };

            return new NativeView
            {
                View = new UILayoutHost(layout)
                {
                    BackgroundColor = UIColor.Gray,
                },
            };
        }

        private View _container;

        public override void LoadView()
        {
             
            var layout = new LinearLayout(Orientation.Vertical)
            {
                SubViews = new View[]
                {
                    _container = new LinearLayout(Orientation.Vertical)
                    {
                        SubViews = new[]
                        {
                            GetNestedHost(),
                        },
                        LayoutParameters = new LayoutParameters(AutoSize.FillParent, AutoSize.WrapContent),
                    },
                    new NativeView()
                    {
                        View = new UILabel()
                        {
                            Text = "123",
                        }
                    },
                    new NativeView()
                    {
                        View = new UIButton()
                        {
                            Font = UIFont.SystemFontOfSize(24),
                            BackgroundColor = UIColor.Clear,
                                        AccessibilityIdentifier = "Hide/Show",
                        },
                        Init = v =>
                        {
                            v.As<UIButton>().BackgroundColor = UIColor.Black;
                            v.As<UIButton>().SetTitle("Hide/Show", UIControlState.Normal);
                            v.As<UIButton>().TouchUpInside += (sender, e) =>
                            {
                                _container.Gone = !_container.Gone;
                                View.LayoutSubviews();
                            };
                        }
                    },
                    new NativeView()
                    {
                        View = new UIView()	{ BackgroundColor = UIColor.Blue },
                        LayoutParameters = new LayoutParameters(AutoSize.FillParent, AutoSize.FillParent),
                    },
                },
            };

            // We've now defined our layout, to actually use it we simply create a UILayoutHost control and pass it the layout
            this.View = new XibFree.UILayoutHost(layout);
            this.View.BackgroundColor = UIColor.Gray;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            EdgesForExtendedLayout = UIRectEdge.None;
        }
    }
}
