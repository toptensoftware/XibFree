using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using XibFree;
using System.Collections.Generic;

namespace Demo
{
	public sealed class TableViewCellDemo2 : UITableViewController
	{
		private readonly List<Item> _items = new List<Item>();

		public TableViewCellDemo2() : base(UITableViewStyle.Grouped)
		{
			Title = "TableViewCell";
		}
			
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			var messages = new[] 
			{
				"Short message",
				"A medium length message",
				"A somewhat longer message that may wrap",
				"A really long message that really really should wrap.  This will allow us to properly test text wrapping when used inside a variable height table view cell"
			};

			// Create some items
			var r = new Random();
			for (var i=0; i<100; i++)
			{
				var item = new Item
				{
					Title = string.Format("Item {0}", i + 1), 
					Total = r.Next(1000)
				};
				item.Count = r.Next(item.Total);
				item.LongText = messages[r.Next(messages.Length)];
				_items.Add(item);
			}

			// Setup the datasource/delegate
			TableView.Source = new Source(this);
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



		private sealed class DemoTableViewCell : UITableViewCell
		{
			private readonly UILabel _labelTitle;
			private readonly UILabel _labelSubTitle;
			private readonly UILabel _labelPercent;
			private readonly UILabel _labelLongText;

			private ViewGroup Layout { get; set; }

			public DemoTableViewCell() : base(UITableViewCellStyle.Default, "DemoTableViewCell")
			{
				Layout = new LinearLayout(Orientation.Horizontal)
				{
					Padding = new UIEdgeInsets(5,5,5,5),
					LayoutParameters = new LayoutParameters
					{
						Width = Dimension.FillParent,
						Height = Dimension.WrapContent,
					},
					SubViews = new View[]
					{
						new NativeView
						{
							View  = new UIImageView(RectangleF.Empty)
							{
								Image = UIImage.FromBundle("tts512.png"),
							},
							LayoutParameters = new LayoutParameters
							{
								Width = Dimension.Absolute(40),
								Height = Dimension.Absolute(40),
								Margins = new UIEdgeInsets(0,0,0,10),
							}
						},
						new LinearLayout(Orientation.Vertical)
						{
							LayoutParameters = new LayoutParameters
							{
								Width = Dimension.FillParent,
								Height = Dimension.WrapContent,
							},
							SubViews = new View[]
							{
								new NativeView
								{
									View = _labelTitle = new UILabel
									{
										Text = "Title",
										BackgroundColor = UIColor.Clear,
										Font = UIFont.BoldSystemFontOfSize(18),
										HighlightedTextColor = UIColor.White,
									},
									LayoutParameters = new LayoutParameters
									{
										Width = Dimension.FillParent,
										Height = Dimension.WrapContent,
									}
								},
								new NativeView
								{
									View = _labelSubTitle = new UILabel
									{
										Text = "SubTitle",
										BackgroundColor = UIColor.Clear,
										Font = UIFont.SystemFontOfSize(12),
										TextColor = UIColor.DarkGray,
										HighlightedTextColor = UIColor.White,
									},
									LayoutParameters = new LayoutParameters
									{
										Width = Dimension.FillParent,
										Height = Dimension.WrapContent,
									}
								},
								new NativeView
								{
									View = _labelLongText = new UILabel
									{
										BackgroundColor = UIColor.Clear,
										Font = UIFont.SystemFontOfSize(12),
										TextColor = UIColor.DarkGray,
										HighlightedTextColor = UIColor.White,
										Lines = 0,
									},
									LayoutParameters = new LayoutParameters
									{
										Width = Dimension.FillParent,
										Height = Dimension.WrapContent,
									}
								}
							}
						},
						new NativeView
						{
							View = _labelPercent = new UILabel
							{
								Text = "20%",
								BackgroundColor = UIColor.Clear,
								TextColor = UIColor.FromRGB(51,102,153),
								HighlightedTextColor = UIColor.White,
								Font = UIFont.BoldSystemFontOfSize(24),
								TextAlignment = UITextAlignment.Right,
							},
							LayoutParameters = new LayoutParameters
							{
								Width = Dimension.Absolute(50),
								Height = Dimension.FillParent,
								Margins = new UIEdgeInsets(0, 10, 0, 0),
							}
						}
					}
				};


				ContentView.Add(new UILayoutHost(Layout, ContentView.Bounds));
				Accessory = UITableViewCellAccessory.DisclosureIndicator;
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
		}

		private class Source : UITableViewSource
		{
			private readonly TableViewCellDemo2 _owner;

			private readonly DemoTableViewCell _prototype = new DemoTableViewCell();

			public Source(TableViewCellDemo2 owner)
			{
				_owner = owner;
			}

			#region implemented abstract members of UITableViewSource
			public override int RowsInSection(UITableView tableview, int section)
			{
				return _owner._items.Count;
			}
			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{
				var cell = (DemoTableViewCell)tableView.DequeueReusableCell("DemoTableViewCell") ?? new DemoTableViewCell();

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

