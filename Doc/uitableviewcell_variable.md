# UITableViewCell (Variable Height)

In this example we look at how to enhance the [previous UITableViewCell example](uitableviewcell_fixed.md) to support variable height rows.  We'll add another view to the centre stack and let it wrap over multiple lines.

The final result is this:

![Screen Shot 2013-03-31 at 3.02.40 PM.png](Screen%20Shot%202013-03-31%20at%203.02.40%20PM.png)

and this:

![Screen Shot 2013-03-31 at 3.03.01 PM.png](Screen%20Shot%202013-03-31%20at%203.03.01%20PM.png)

### Updating the Layout

First we'll update the layout to include the new UILabel.  Note the `Lines = 0` to switch the label into multi-line mode.

```C#
new NativeView()
{
	View = _labelLongText = new UILabel()
	{
		BackgroundColor = UIColor.Clear,
		Font = UIFont.SystemFontOfSize(12),
		TextColor = UIColor.DarkGray,
		HighlightedTextColor = UIColor.White,
		Lines = 0,
	},
	LayoutParameters = new LayoutParameters()
	{
		Width = AutoSize.FillParent,
		Height = AutoSize.WrapContent,
	}
}
```

### Updating the MeasureHeight function

Next we need to update the MeasureHeight function.  Since the height is now variable, and dependant on the width of the view we need both the item that's going to be displayed and the width of the table view.

```C#
public float MeasureHeight(UITableView tableView, Item i)
{
	// Initialize the view's so they have the correct content for height calculations
	Init(i);

	// Remeasure the layout using the tableView width, allowing for grouped table view margins
	// and the disclosure indicator
	Layout.Measure(tableView.Bounds.Width - 20 - 18, float.MaxValue);

	// Grab the measured height
	return Layout.GetMeasuredSize().Height;
}
```

### Supplying the Row Height for each cell

The `UITableView.RowHeight` property is only useful for fixed height rows so we can't rely on this anymore.  Rather we need to override `GetHeightForRow` on the UITableViewSource:

```C#
public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
{
    return _prototype.MeasureHeight(tableView, _owner._items[indexPath.Row]);
}
```

The variable `_prototype` is a member variable that holds a single DemoTableViewCell reference that we use for height calculations and nothing else:
 
```C#
DemoTableViewCell _prototype = new DemoTableViewCell();
```

### Full Source Code

The rest of the example is the same as before, but here it is for completeness:


```C#
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using XibFree;
using System.Collections.Generic;

namespace Demo
{
	public partial class TableViewCellDemo2 : UITableViewController
	{
		public TableViewCellDemo2() : base(UITableViewStyle.Grouped)
		{
			this.Title = "TableViewCell";
		}
		
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			string[] messages = new string[] 
			{
				"Short message",
				"A medium length message",
				"A somewhat longer message that may wrap",
				"A really long message that really really should wrap.  This will allow us to properly test text wrapping when used inside a variable height table view cell",
			};

			// Create some items
			var r = new Random();
			for (int i=0; i<100; i++)
			{
				var item = new Item();
				item.Title =  string.Format("Item {0}", i+1);
				item.Total = r.Next(1000);
				item.Count = r.Next(item.Total);
				item.LongText = messages[r.Next(messages.Length)];
				_items.Add(item);
			}

			// Setup the datasource/delegate
			this.TableView.Source = new Source(this);
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
			public string LongText;
		};

		List<Item> _items = new List<Item>();


		class DemoTableViewCell : UITableViewCell
		{
			public DemoTableViewCell() : base(UITableViewCellStyle.Default, "DemoTableViewCell")
			{
				Layout = new LinearLayout(Orientation.Horizontal)
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
							View  = new UIImageView(RectangleF.Empty)
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
										Text = "Title",
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
										Text = "SubTitle",
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
								new NativeView()
								{
									View = _labelLongText = new UILabel()
									{
										BackgroundColor = UIColor.Clear,
										Font = UIFont.SystemFontOfSize(12),
										TextColor = UIColor.DarkGray,
										HighlightedTextColor = UIColor.White,
										Lines = 0,
									},
									LayoutParameters = new LayoutParameters()
									{
										Width = AutoSize.FillParent,
										Height = AutoSize.WrapContent,
									}
								}
							}
						},
						new NativeView()
						{
							View = _labelPercent = new UILabel()
							{
								Text = "20%",
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


				this.ContentView.Add(new UILayoutHost(Layout, this.ContentView.Bounds));
				this.Accessory = UITableViewCellAccessory.DisclosureIndicator;
			}

			public void Init(Item i)
			{
				_labelTitle.Text = i.Title;
				_labelSubTitle.Text = string.Format("{0} of {1}", i.Count, i.Total);
				_labelPercent.Text = string.Format("{0}%", i.Percentage);
				_labelLongText.Text = i.LongText;
			}

			public float MeasureHeight(UITableView tableView, Item i)
			{
				// Initialize the view's so they have the correct content for height calculations
				Init(i);

				// Remeasure the layout using the tableView width, allowing for grouped table view margins
				// and the disclosure indicator
				Layout.Measure(tableView.Bounds.Width - 20 - 18, float.MaxValue);

				// Grab the measured height
				return Layout.GetMeasuredSize().Height;
			}

			public ViewGroup Layout
			{
				get;
				set;
			}

			UILabel _labelTitle;
			UILabel _labelSubTitle;
			UILabel _labelPercent;
			UILabel _labelLongText;
		}

		class Source : UITableViewSource
		{
			public Source(TableViewCellDemo2 owner)
			{
				_owner = owner;
			}

			TableViewCellDemo2 _owner;

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

			public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
			{
			}

			public override float GetHeightForRow(UITableView tableView, NSIndexPath indexPath)
			{
				return _prototype.MeasureHeight(tableView, _owner._items[indexPath.Row]);
			}
			#endregion
		}
	}
}

```

