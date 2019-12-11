
using System;
using CoreGraphics;

using Foundation;
using UIKit;
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
				item.Total = r.Next(999)+1;
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
							View = new UIImageView(CGRect.Empty)
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
								Font = UIFont.BoldSystemFontOfSize(20),
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
			public nfloat MeasureHeight()
			{
				// Get the layout to measure itself, and then retrieve the measured height
				_layout.Measure(nfloat.MaxValue, nfloat.MaxValue);
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

			#region implemented abstract members of UITableViewSource

			public override nint RowsInSection(UITableView tableview, nint section)
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

