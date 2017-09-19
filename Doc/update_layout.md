# Updating the Layout


### Updating the On-screen Layout

There are many things that can be done programatically to a layout that might require it to be recalculated and updated on screen.  XibFree deliberately provides no support for automatically updating the layout in response to such changes - this is by design, and based on the fact that there are things outside of XibFree's scope that can affect layout.

That said, invoking XibFree to update itself it trivial - simply call the standard iOS method `SetNeedsUpdate()` on the UILayoutHost view.

Say for example you update the text of a label that has an `AutoSize.WrapContent` dimension, you would need something like this:

```C#
_label.Text = "New text that may cause the layout to change";
_layoutHost.SetNeedsLayout();
```

### Navigation Extensions

XibFree provides a number of UIView extension methods that help navigate between the UIView hierarchy and XibFree's layout tree.

* GetNativeView() - retrieves the NativeView associated with a UIView
* GetLayoutHost() - retrieves the UILayoutHost hosting a UIView

(for these methods to work, the view must be in a layout hierarchy that is currently hosted by a UILayoutHost)
    
### More Examples

Changing a view's margins:

```C#
_label.GetNativeView().LayoutMargins.Right = 100;
_label.GetLayoutHost().SetNeedsLayout();
```

Toggling a view between Visibility.Visible and Visibility.Gone:

```C#
_label.GetNativeView().Gone = !_label.GetNativeView().Gone;
_label.GetLayoutHost().SetNeedsLayout();
```

Inserting a new view into the view hierarchy:

```C#
_layout.AddSubView(new UILabel(RectangleF.Empty)) { ... };
_layout.GetLayoutHost().SetNeedsLayout();
```

Removing a view from the view hierarchy:

```C#
var host = _label.GetLayoutHost();				// We need to get the host before removing the view
_label.GetNativeView().RemoveFromSuperview();
host.SetNeedsLayout();
```

Adding a layer to a view group.  Although this doesn't actually change the layout, the positioning of a ViewGroup's layer happens as part of the measure/layout cycle.

```C#
_viewGroup.Label = new CAGradientLayer() { ... };
_viewGroup.GetLayoutHost().SetNeedsLayout();
```
