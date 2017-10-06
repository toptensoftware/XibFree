using System;
using CoreGraphics;
using System.Collections.Generic;

using UIKit;
using Foundation;

using XibFree;

namespace Demo
{
    public partial class WrapLayoutDemo : UITableViewController
    {
        public WrapLayoutDemo()
        {
            Title = "WrapLayout";

            // Custom initialization
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        public override void LoadView()
        {
            // This is a simple vertical LinearLayout.   ViewGroups are not implemented as UIViews - they're simply scaffolding for 
            // the layout of the contained NativeViews
            var layout = new LinearLayout(Orientation.Vertical)
            {
                Padding = new UIEdgeInsets(50, 10, 10, 10),
                Gravity = Gravity.CenterVertical,
                SubViews = new View[]
                {
                    new NativeView()
                    {
                        // This is the UIView
                        View = new UILabel()
                        {
                            Text = "above",
                            Lines = 1,
                        },
                    },
                    new WrapLayout()
                    {
                        LayoutParameters = new LayoutParameters(AutoSize.FillParent, AutoSize.WrapContent),
                                Gravity = Gravity.Right,
                        SubViews = new View[]
                        {
                            // A NativeView contains an iOS UIView
                            new NativeView()
                            {
                                // This is the UIView
                                View = new UIView(CGRect.Empty)
                                {
                                    // Set properties here
                                    BackgroundColor = UIColor.Red,
                                },

                                // This controls how it's laid out by its parent view group (in this case the outer linear layout)
                                LayoutParameters = new LayoutParameters()
                                {
                                    Width = 50,
                                    Height = 50,
                                },
                            },
                            new NativeView()
                            {
                                // This is the UIView
                                View = new UILabel()
                                {
                                    Text = "sada asd asd asd",
                                    Lines = 1,
                                },
                            },
                            new NativeView()
                            {
                                // This is the UIView
                                View = new UILabel()
                                {
                                    Text = "123 456 789 101112",
                                    Lines = 1,
                                },
                            },
                            new NativeView()
                            {
                                // This is the UIView
                                View = new UIView(CGRect.Empty)
                                {
                                    // Set properties here
                                    BackgroundColor = UIColor.Red,
                                },

                                // This controls how it's laid out by its parent view group (in this case the outer linear layout)
                                LayoutParameters = new LayoutParameters()
                                {
                                    Width = 200,
                                    Height = 50,
                                },
                            },
                            new NativeView()
                            {
                                // This is the UIView
                                View = new UILabel()
                                {
                                    Text = "456 45 6",
                                    Lines = 1,
                                },
                                            LayoutParameters = new LayoutParameters() {
                                                Gravity = Gravity.Bottom,
                                            },
                            },
                        },
                    },
                    new NativeView()
                    {
                        // This is the UIView
                        View = new UILabel()
                        {
                            Text = "below",
                            Lines = 1,
                        },
                    },
                },
            };

            // We've now defined our layout, to actually use it we simply create a UILayoutHost control and pass it the layout
            this.View = new XibFree.UILayoutHost(layout);
            this.View.BackgroundColor = UIColor.Gray;
        }
    }
}
