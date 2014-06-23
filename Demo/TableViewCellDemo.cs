
using System;
using System.Drawing;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using XibFree;
using System.Collections.Generic;

namespace Demo
{
	public sealed class TableViewCellDemo : UITableViewController
	{
		public TableViewCellDemo() : base(UITableViewStyle.Grouped)
		{
			Title = "TableViewCell";
		}
		
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

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
				_items.Add(item);
			}

			TableView.Source = new Source(this);
			TableView.RowHeight = new DemoTableViewCell().MeasureHeight();
		}

		private class Item
		{
			public string Title;
			public int Count;
			public int Total;
			public int Percentage
			{
				get { return Count * 100 / Total; }
			}
		};

		private readonly List<Item> _items = new List<Item>();

		private sealed class DemoTableViewCell : UITableViewCell
		{
			private readonly ViewGroup _layout;
			private readonly UILabel _labelTitle;
			private readonly UILabel _labelSubTitle;
			private readonly UILabel _labelPercent;

			public DemoTableViewCell() : base(UITableViewCellStyle.Default, "DemoTableViewCell")
			{
				_layout = new LinearLayout(Orientation.Horizontal)
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
							View = new UIImageView(RectangleF.Empty)
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
								}
							}
						},
						new NativeView
						{
							View = _labelPercent = new UILabel
							{
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


				ContentView.Add(new UILayoutHost(_layout, ContentView.Bounds));
				Accessory = UITableViewCellAccessory.DisclosureIndicator;
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
		}

		class Source : UITableViewSource
		{
			private readonly TableViewCellDemo _owner;

			public Source(TableViewCellDemo owner)
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

			#endregion
		}

	}
}

