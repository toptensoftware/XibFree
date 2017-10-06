using System;
using CoreGraphics;
using System.Collections.Generic;

using UIKit;
using Foundation;

using XibFree;

namespace Demo
{
    public partial class LinearLayoutDemo2 : UITableViewController
    {
        public LinearLayoutDemo2()
        {
            Title = "LinearLayout2";

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
                Padding = new UIEdgeInsets(10, 10, 10, 10),
                Gravity = Gravity.CenterVertical,
                SubViews = new View[]
                {
                    new NativeView()
                    {
                        // This time we're showing a UILabel
                        View = new UILabel()
                        {
                            BackgroundColor = UIColor.Purple,
                            Text = "Hello World, this is a test to see if things wrap and measure correctly",
                            Lines = 0,
                            TextAlignment = UITextAlignment.Center,
                            TextColor = UIColor.White,
                        },

                        LayoutParameters = new LayoutParameters()
                        {
                            Width = AutoSize.WrapContent,
                            Height = AutoSize.WrapContent,
                        },
                    },

                    new LinearLayout(Orientation.Horizontal)
                    {
                        LayoutParameters = new LayoutParameters(AutoSize.FillParent, AutoSize.WrapContent),
                        Spacing = 5,
                        SubViews = new View[]
                        {
                            new NativeView()
                            {
                                LayoutParameters = new LayoutParameters(AutoSize.WrapContent, AutoSize.WrapContent)
                                {
                                },
                                View = new UILabel()
                                {
                                    Text = "asd as dasd asd as da fkasdhf aksdh fkljashd fklahsdfk lahsdlkf haslkdhf klashdfkasd",
                                    Lines = 0,
                                },
                            },
                            new NativeView()
                            {
                                LayoutParameters = new LayoutParameters(AutoSize.WrapContent, AutoSize.WrapContent)
                                {
                                    MinWidth = 50,
                                },
                                View = View = new UILabel()
                                {
                                    Text = "asd as"
                                },
                            },
                        },
                    },

                    new LinearLayout(Orientation.Horizontal)
                    {
                        LayoutParameters = new LayoutParameters(AutoSize.WrapContent, AutoSize.WrapContent),
                        Spacing = 5,
                        SubViews = new View[]
                        {
                            new NativeView()
                            {
                                LayoutParameters = new LayoutParameters(AutoSize.WrapContent, AutoSize.WrapContent)
                                {
                                },
                                View = new CustomButton("234 asdf asdf a sdfs"),
                            },
                            new NativeView()
                            {
                                LayoutParameters = new LayoutParameters(AutoSize.WrapContent, AutoSize.WrapContent)
                                {
                                },
                                View = new CustomButton("xcv"),
                            },
                            new NativeView()
                            {
                                LayoutParameters = new LayoutParameters(AutoSize.WrapContent, AutoSize.WrapContent)
                                {
                                },
                                View = new CustomButton("asd"),
                            },

                            new NativeView()
                            {
                                LayoutParameters = new LayoutParameters(30, 50),
                                View = new UIView() { BackgroundColor = UIColor.Green },
                            }
                        },
                    },

                    new LinearLayout(Orientation.Horizontal)
                    {
                        // How to layout this linear layout within the outer one
                        LayoutParameters = new LayoutParameters()
                        {
                            Height = AutoSize.WrapContent,
                            Width = AutoSize.FillParent,
                        },

                        // Sub view collection
                        SubViews = new View[]
                        {
                            new NativeView()
                            {
                                // This time we're showing a UILabel
                                View = new UILabel()
                                {
                                    BackgroundColor = UIColor.Purple,
                                    Text = "Hello World, this is a test to see if things wrap and measure correctly",
                                    Lines = 0,
                                    TextAlignment = UITextAlignment.Center,
                                    TextColor = UIColor.White
                                },

                                LayoutParameters = new LayoutParameters()
                                {
                                    Width = AutoSize.WrapContent,
                                    Height = AutoSize.WrapContent,		// Height calculated automatically based on text content!
                                },
                            },
                            new NativeView()
                            {
                                // This time we're showing a UILabel
                                View = new UIView()
                                {
                                    BackgroundColor = UIColor.Yellow,
                                },

                                LayoutParameters = new LayoutParameters()
                                {
                                    Width = 60,
                                    Height = AutoSize.FillParent,		// Height calculated automatically based on text content!
                                    MinWidth = 60,
                                },
                            },
                        }
                    },
					
                    // Here we're nesting a horizontal linear layout inside the outer vertical linear layout
                    new LinearLayout(Orientation.Horizontal)
                    {
                        // How to layout this linear layout within the outer one
                        LayoutParameters = new LayoutParameters()
                        {
                            Height = AutoSize.WrapContent,
                            Width = AutoSize.FillParent,
                        },

                        // Sub view collection
                        SubViews = new View[]
                        {
                            new NativeView()
                            {
                                // This time we're showing a UILabel
                                View = new UILabel()
                                {
                                    BackgroundColor = UIColor.Purple,
                                    Text = "Hello World, this is a test to see if things wrap and measure correctly",
                                    Lines = 0,
                                    TextAlignment = UITextAlignment.Center,
                                    TextColor = UIColor.White
                                },

                                LayoutParameters = new LayoutParameters()
                                {
                                    Width = AutoSize.WrapContent,
                                    Height = AutoSize.WrapContent,		// Height calculated automatically based on text content!
                                    MinWidth = 50,
                                },
                            },
                            new NativeView()
                            {
                                // This time we're showing a UILabel
                                View = new UIView()
                                {
                                    BackgroundColor = UIColor.Purple,
                                },

                                LayoutParameters = new LayoutParameters()
                                {
                                    Width = 20,
                                    MinWidth = 20,
                                    Height = AutoSize.FillParent,		// Height calculated automatically based on text content!
                                },
                            },
                            new NativeView()
                            {
                                // This time we're showing a UILabel
                                View = new UILabel()
                                {
                                    BackgroundColor = UIColor.Purple,
                                    Text = "Hello World, this is a test to see if things wrap and measure correctly",
                                    Lines = 0,
                                    TextAlignment = UITextAlignment.Center,
                                    TextColor = UIColor.White
                                },

                                LayoutParameters = new LayoutParameters()
                                {
                                    Width = AutoSize.WrapContent,
                                    Height = AutoSize.WrapContent,		// Height calculated automatically based on text content!
                                    MinWidth = 50,
                                },
                            },
                        }
                    },

                    new LinearLayout(Orientation.Horizontal)
                    {
                        // How to layout this linear layout within the outer one
                        LayoutParameters = new LayoutParameters()
                        {
                            Height = AutoSize.WrapContent,
                            Width = AutoSize.FillParent,
                        },

                        // Sub view collection
                        SubViews = new View[]
                        {
                            new NativeView()
                            {
                                // This time we're showing a UILabel
                                View = new UILabel()
                                {
                                    BackgroundColor = UIColor.Purple,
                                    Text = "123 12 3123",
                                    Lines = 0,
                                    TextAlignment = UITextAlignment.Center,
                                    TextColor = UIColor.White
                                },

                                LayoutParameters = new LayoutParameters(100, AutoSize.WrapContent)
                                {
                                },
                            },
                            new NativeView()
                            {
                                // This time we're showing a UILabel
                                View = new UILabel()
                                {
                                    BackgroundColor = UIColor.Purple,
                                    Text = "Hello World, this is a test to see if things wrap and measure correctly",
                                    Lines = 0,
                                    TextAlignment = UITextAlignment.Center,
                                    TextColor = UIColor.White
                                },

                                LayoutParameters = new LayoutParameters()
                                {
                                    Width = AutoSize.WrapContent,
                                    Height = AutoSize.WrapContent,      // Height calculated automatically based on text content!
                                    MinWidth = 50,
                                },
                            },
                            new NativeView()
                            {
                                LayoutParameters = new LayoutParameters(AutoSize.FillParent, AutoSize.WrapContent)
                                {
                                    MarginLeft = 4,
                                },
                                View = new UILabel()
                                {
                                    Text = "",
                                },
                            }
                        }
                    },
                    
                },
            };

            // We've now defined our layout, to actually use it we simply create a UILayoutHost control and pass it the layout
            this.View = new XibFree.UILayoutHost(layout);
            this.View.BackgroundColor = UIColor.Gray;
        }

        public class CustomButton : UIButton
        {
            private UILayoutHost _host;

            public CustomButton(string title)
            {
                _host = new UILayoutHost()
                {
                    Layout = new LinearLayout(Orientation.Horizontal)
                    {
                        Padding = new UIEdgeInsets(3, 2, 3, 5),
                        LayoutParameters = new LayoutParameters(AutoSize.WrapContent, 56)
                        {
                        },
                        SubViews = new[]
                        {
                            new NativeView()
                            {
                                LayoutParameters = new LayoutParameters(30, 30),
                                View = new UIView() { BackgroundColor = UIColor.Red },
                            },
                            new NativeView()
                            {
                                LayoutParameters = new LayoutParameters(AutoSize.WrapContent, AutoSize.FillParent)
                                {
                                    MarginLeft = 4,
                                    MarginRight = 2,
                                    Gravity = Gravity.CenterVertical,
                                    MaxWidth = 90,
                                },
                                View = new UILabel() { Text = title, Lines = 0 },
                            },
                        }
                    },
                };
                AddSubview(_host);
            }

            public override CGSize SizeThatFits(CGSize size)
            {
                return _host.SizeThatFits(size);
            }

            public override CGRect Frame
            {
                get
                {
                    return base.Frame;
                }
                set
                {
                    base.Frame = value;
                    if (_host != null)
                    {
                        _host.Frame = Bounds;
                    }
                }
            }
        }
    }
}
