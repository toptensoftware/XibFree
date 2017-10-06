
using System;
using CoreGraphics;

using Foundation;
using UIKit;
using XibFree;

namespace Demo
{
	public partial class MainViewController : UITableViewController
	{
		public MainViewController() : base(UITableViewStyle.Grouped)
		{
			this.Title = "XibFree Demos";
		}
		
		public override void DidReceiveMemoryWarning()
		{
			// Releases the view if it doesn't have a superview.
			base.DidReceiveMemoryWarning();
			
			// Release any cached data, images, etc that aren't in use.
		}
		
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			this.TableView.Source = new Source(this);
		}

		class Source : UITableViewSource
		{
			public Source(MainViewController owner)
			{
				_owner = owner;
			}

			MainViewController _owner;

			class Demo
			{
				public string Title;
				public Type TClass;
			};

			Demo[] _demos = new Demo[]
			{
				new Demo() { Title = "#1 Basics", TClass = typeof(Demo1) },
				new Demo() { Title = "LinearLayout", TClass = typeof(LinearLayoutDemo) },
				new Demo() { Title = "LinearLayout2", TClass = typeof(LinearLayoutDemo2) },
				new Demo() { Title = "GridLayout", TClass = typeof(GridLayoutDemo) },
				new Demo() { Title = "FrameLayout", TClass = typeof(FrameLayoutDemo) },
				new Demo() { Title = "Nested Hosts", TClass = typeof(NestedDemo) },
                new Demo() { Title = "Nested Hosts Visibility Bug", TClass = typeof(NestedDemoVisibilityBug) },
				new Demo() { Title = "ViewGroup Layers", TClass = typeof(ViewGroupLayerDemo) },
				new Demo() { Title = "TableViewCell", TClass = typeof(TableViewCellDemo) },
				new Demo() { Title = "TableViewCell Variable", TClass = typeof(TableViewCellDemo2) },
				new Demo() { Title = "Visibility", TClass = typeof(VisibilityDemo) },
				new Demo() { Title = "Recalculate Layout", TClass = typeof(RecalculateLayoutDemo) },
                    new Demo() { Title = "Wrap Layout", TClass = typeof(WrapLayoutDemo) },
			};

			#region implemented abstract members of UITableViewSource
			public override nint RowsInSection(UITableView tableview, nint section)
			{
				return _demos.Length;
			}
			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{
				var cell = tableView.DequeueReusableCell("cell");
				if (cell==null)
				{
					cell = new UITableViewCell(UITableViewCellStyle.Default, "cell");
					cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
				}

				cell.TextLabel.Text = _demos[indexPath.Row].Title;
                cell.AccessibilityIdentifier = _demos[indexPath.Row].Title;
                cell.AccessibilityLabel = _demos[indexPath.Row].Title;
				return cell;
			}

			public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
			{
				var vc = (UIViewController)Activator.CreateInstance(_demos[indexPath.Row].TClass);
				_owner.NavigationController.PushViewController(vc, true);
			}
			#endregion
		}

	}
}

