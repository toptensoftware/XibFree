using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Demo
{
	public sealed class MainViewController : UITableViewController
	{
		public MainViewController() : base(UITableViewStyle.Grouped)
		{
			Title = "XibFree Demos";
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			TableView.Source = new Source(this);
		}

		private class Source : UITableViewSource
		{
			private readonly MainViewController _owner;

			public Source(MainViewController owner)
			{
				_owner = owner;
			}

			private class Demo
			{
				public string Title;
				public Type Class;
			};

			private readonly Demo[] _demos = new[]
			{
				new Demo { Title = "#1 Basics", Class = typeof(Demo1) },
				new Demo { Title = "LinearLayout", Class = typeof(LinearLayoutDemo) },
				new Demo { Title = "FrameLayout", Class = typeof(FrameLayoutDemo) },
				new Demo { Title = "Nested Hosts", Class = typeof(NestedDemo) },
				new Demo { Title = "ViewGroup Layers", Class = typeof(ViewGroupLayerDemo) },
				new Demo { Title = "TableViewCell", Class = typeof(TableViewCellDemo) },
				new Demo { Title = "TableViewCell Variable", Class = typeof(TableViewCellDemo2) },
				new Demo { Title = "Visibility", Class = typeof(VisibilityDemo) },
				new Demo { Title = "Recalculate Layout", Class = typeof(RecalculateLayoutDemo) }
			};

			#region implemented abstract members of UITableViewSource
			public override int RowsInSection(UITableView tableview, int section)
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

				return cell;
			}

			public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
			{
				var vc = (UIViewController)Activator.CreateInstance(_demos[indexPath.Row].Class);
				_owner.NavigationController.PushViewController(vc, true);
			}
			#endregion
		}

	}
}

