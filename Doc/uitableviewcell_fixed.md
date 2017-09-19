# UITableViewCell (Fixed Height)

XibFree is a great way to define custom layouts for UITableViewCells.  This guide shows how to setup a simple UITableViewCell with an image on the left, two vertically stacked labels in the middle (that stretch to fill available space) and a large label on the right.  The final effect is this:

![Screen Shot 2013-03-31 at 2.30.06 PM.png](<Screen Shot 2013-03-31 at 2.30.06 PM.png>)

And of course it works in landscape too:

![Screen Shot 2013-03-31 at 2.55.24 PM.png](<Screen Shot 2013-03-31 at 2.55.24 PM.png>)

### Declare a data model

We'll start by defining an object to represent the data items being displayed.  This isn't part of XibFree's requirements - it's simply to support the following example:

```C#
class Item
{
    public string Title;
    public int Count;
    public int Total;
    public int Percentage
    {
        get
        {
            return Count * 100 / Total;
        }
    }
};
```

### Define a custom UITableViewCell class

Next we'll implement the custom UITableViewCell class.  Note the following:

* DemoTableViewCell will derive from UITableViewCell
* It defines the layout using two nested LinearLayouts and the required UIViews.
* References to the UIView's are stored as member variables as the layout is declared (eg: `View = _labelTitle = new UILabel() { ... }`.
* The layout is hosted in a UILayoutHost and added as a subview to the UITableViewCell's `ContentView`.

Here's how it looks so far:

```C#
class DemoTableViewCell : UITableViewCell
{
	public DemoTableViewCell() : base(UITableViewCellStyle.Default, "DemoTableViewCell")
	{
		_layout = new LinearLayout(Orientation.Horizontal)
		{
			Padding = new UIEdgeInsets(5,5,5,5),
			LayoutParameters = new LayoutParameters()
			{
				Width = AutoSize.FillParent,
				Height = AutoSize.WrapContent,
			},
			SubViews = new View[]
			{
				new NativeView()
				{
					View = new UIImageView(RectangleF.Empty)
					{
						Image = UIImage.FromBundle("tts512.png"),
					},
					LayoutParameters = new LayoutParameters()
					{
						Width = 40,
						Height = 40,
						Margins = new UIEdgeInsets(0,0,0,10),
					}
				},
				new LinearLayout(Orientation.Vertical)
				{
					LayoutParameters = new LayoutParameters()
					{
						Width = AutoSize.FillParent,
						Height = AutoSize.WrapContent,
					},
					SubViews = new View[]
					{
						new NativeView()
						{
							View = _labelTitle = new UILabel()
							{
								BackgroundColor = UIColor.Clear,
								Font = UIFont.BoldSystemFontOfSize(18),
								HighlightedTextColor = UIColor.White,
							},
							LayoutParameters = new LayoutParameters()
							{
								Width = AutoSize.FillParent,
								Height = AutoSize.WrapContent,
							}
						},
						new NativeView()
						{
							View = _labelSubTitle = new UILabel()
							{
								BackgroundColor = UIColor.Clear,
								Font = UIFont.SystemFontOfSize(12),
								TextColor = UIColor.DarkGray,
								HighlightedTextColor = UIColor.White,
							},
							LayoutParameters = new LayoutParameters()
							{
								Width = AutoSize.FillParent,
								Height = AutoSize.WrapContent,
							}
						},
					}
				},
				new NativeView()
				{
					View = _labelPercent = new UILabel()
					{
						BackgroundColor = UIColor.Clear,
						TextColor = UIColor.FromRGB(51,102,153),
						HighlightedTextColor = UIColor.White,
						Font = UIFont.BoldSystemFontOfSize(24),
						TextAlignment = UITextAlignment.Right,
					},
					LayoutParameters = new LayoutParameters()
					{
						Width = 50,
						Height = AutoSize.FillParent,
						Margins = new UIEdgeInsets(0, 10, 0, 0),
					}
				}
			}
		};

		this.ContentView.Add(new UILayoutHost(_layout, this.ContentView.Bounds));
		this.Accessory = UITableViewCellAccessory.DisclosureIndicator;
	}

	public void Init(Item i)
	{
		_labelTitle.Text = i.Title;
		_labelSubTitle.Text = string.Format("{0} of {1}", i.Count, i.Total);
		_labelPercent.Text = string.Format("{0}%", i.Percentage);
	}

	// Helper to get the required height for a cell
	public float MeasureHeight()
	{
		// Get the layout to measure itself, and then retrieve the measured height
		_layout.Measure(float.MaxValue, float.MaxValue);
		return _layout.GetMeasuredSize().Height;
	}

	ViewGroup _layout;
	UILabel _labelTitle;
	UILabel _labelSubTitle;
	UILabel _labelPercent;
}
```

### Setting up the RowHeight

If you look carefully at the implementation of DemoTableViewCell just above, you'll notice another method: `MeasureHeight()`. This helper function uses the XibFree layout to work out the required height for the cell.  

The most efficient way to set the height of a UITableView's rows is by using its `RowHeight` property, so in ViewDidLoad() we create a temporary prototype cell and use it to measure its height:

```C#
this.TableView.RowHeight = new DemoTableViewCell().MeasureHeight();
```

### Providing cells to the Table View

Next we need to implement UITableViewSource's GetCell method.  First we try to re-use an existing cell, otherwise we create a new one.  We then simply call its `Init` method to setup the cell and then return it.

```C#
public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
{
	var cell = (DemoTableViewCell)tableView.DequeueReusableCell("DemoTableViewCell");
	if (cell==null)
	{
		cell = new DemoTableViewCell();
	}

	var item = _owner._items[indexPath.Row];
	cell.Init(item);

	return cell;
}
```


### Full Source Code

The rest of this example is a standard MonoTouch view controller, here's the full source code:

```C#
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using XibFree;
using System.Collections.Generic;

namespace Demo
{
	public partial class TableViewCellDemo : UITableViewController
	{
		public TableViewCellDemo() : base(UITableViewStyle.Grouped)
		{
			this.Title = "TableViewCell";
		}
		
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			// Create some items
			var r = new Random();
			for (int i=0; i<100; i++)
			{
				var item = new Item();
				item.Title =  string.Format("Item {0}", i+1);
				item.Total = r.Next(1000);
				item.Count = r.Next(item.Total);
				_items.Add(item);
			}

			this.TableView.Source = new Source(this);
			this.TableView.RowHeight = new DemoTableViewCell().MeasureHeight();
		}

		class Item
		{
			public string Title;
			public int Count;
			public int Total;
			public int Percentage
			{
				get
				{
					return Count * 100 / Total;
				}
			}
		};

		List<Item> _items = new List<Item>();

		class DemoTableViewCell : UITableViewCell
		{
			public DemoTableViewCell() : base(UITableViewCellStyle.Default, "DemoTableViewCell")
			{
				_layout = new LinearLayout(Orientation.Horizontal)
				{
					Padding = new UIEdgeInsets(5,5,5,5),
					LayoutParameters = new LayoutParameters()
					{
						Width = AutoSize.FillParent,
						Height = AutoSize.WrapContent,
					},
					SubViews = new View[]
					{
						new NativeView()
						{
							View = new UIImageView(RectangleF.Empty)
							{
								Image = UIImage.FromBundle("tts512.png"),
							},
							LayoutParameters = new LayoutParameters()
							{
								Width = 40,
								Height = 40,
								Margins = new UIEdgeInsets(0,0,0,10),
							}
						},
						new LinearLayout(Orientation.Vertical)
						{
							LayoutParameters = new LayoutParameters()
							{
								Width = AutoSize.FillParent,
								Height = AutoSize.WrapContent,
							},
							SubViews = new View[]
							{
								new NativeView()
								{
									View = _labelTitle = new UILabel()
									{
										BackgroundColor = UIColor.Clear,
										Font = UIFont.BoldSystemFontOfSize(18),
										HighlightedTextColor = UIColor.White,
									},
									LayoutParameters = new LayoutParameters()
									{
										Width = AutoSize.FillParent,
										Height = AutoSize.WrapContent,
									}
								},
								new NativeView()
								{
									View = _labelSubTitle = new UILabel()
									{
										BackgroundColor = UIColor.Clear,
										Font = UIFont.SystemFontOfSize(12),
										TextColor = UIColor.DarkGray,
										HighlightedTextColor = UIColor.White,
									},
									LayoutParameters = new LayoutParameters()
									{
										Width = AutoSize.FillParent,
										Height = AutoSize.WrapContent,
									}
								},
							}
						},
						new NativeView()
						{
							View = _labelPercent = new UILabel()
							{
								BackgroundColor = UIColor.Clear,
								TextColor = UIColor.FromRGB(51,102,153),
								HighlightedTextColor = UIColor.White,
								Font = UIFont.BoldSystemFontOfSize(24),
								TextAlignment = UITextAlignment.Right,
							},
							LayoutParameters = new LayoutParameters()
							{
								Width = 50,
								Height = AutoSize.FillParent,
								Margins = new UIEdgeInsets(0, 10, 0, 0),
							}
						}
					}
				};


				this.ContentView.Add(new UILayoutHost(_layout, this.ContentView.Bounds));
				this.Accessory = UITableViewCellAccessory.DisclosureIndicator;
			}

			public void Init(Item i)
			{
				_labelTitle.Text = i.Title;
				_labelSubTitle.Text = string.Format("{0} of {1}", i.Count, i.Total);
				_labelPercent.Text = string.Format("{0}%", i.Percentage);
			}

			// Helper to get the required height for a cell
			public float MeasureHeight()
			{
				// Get the layout to measure itself, and then retrieve the measured height
				_layout.Measure(float.MaxValue, float.MaxValue);
				return _layout.GetMeasuredSize().Height;
			}

			ViewGroup _layout;
			UILabel _labelTitle;
			UILabel _labelSubTitle;
			UILabel _labelPercent;
		}

		class Source : UITableViewSource
		{
			public Source(TableViewCellDemo owner)
			{
				_owner = owner;
			}

			TableViewCellDemo _owner;

			DemoTableViewCell _prototype = new DemoTableViewCell();

			#region implemented abstract members of UITableViewSource

			public override int RowsInSection(UITableView tableview, int section)
			{
				return _owner._items.Count;
			}

			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{
				var cell = (DemoTableViewCell)tableView.DequeueReusableCell("DemoTableViewCell");
				if (cell==null)
				{
					cell = new DemoTableViewCell();
				}

				var item = _owner._items[indexPath.Row];
				cell.Init(item);

				return cell;
			}

			#endregion
		}

	}
}
```