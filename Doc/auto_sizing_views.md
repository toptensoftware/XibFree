# Auto-Sizing Views

When specifying the width or height of a view you specify either an exact size (same units as used by UIView) or you can use one of two special values to have its dimensions calculated at runtime:

* `AutoSize.FillParent`  - the view is to fill all available space provided by the parent view.
* `AutoSize.WrapContent` - the view will be sized to enclose the contained content.

If you're familiar with Android layouts, these will be immediately recognisable and they work pretty much the same way.

View dimensions are specified in the view's `LayoutParameters` property:
    
```C#
new NativeView()
{
    View = new UIView(RectangleF.Empty)...
    LayoutParameters = new LayoutParameters()
    {
        Width = AutoSize.FillParent,
        Height = 50,
    },
},
```

### Width and Height Units

XibFree now supports two properties on the LayoutParameters `WidthUnits` and `HeightUnits` that determine how the `Width` and `Height` property values should be interpreted.  The supported values are:

* `Units.Absolute` - an absolute dimension, specified in pixels (or virtual pixels on Retina devices).
* `Units.ParentRatio` - a ratio of the enclosing parents dimension.
* `Units.ContentRatio` - a ratio of the view's content size.
* `Units.AspectRatio` - a ratio of the adjacent width/height dimension.
* `Units.ScreenRatio` - a ratio of the current device screen size (takes orientation into account).
* `Units.HostRatio` - a ratio of the size of the hosting UILayoutHost or UILayoutHostScrollable view.

Setting a dimension to `AutoSize.FillParent` is a short cut for:

```C#
Width = 1.0f,
WidthUnits = Units.ParentRatio,
```

and similarly `AutoSize.WrapContent` is a short cut for:

```C#
Width = 1.0f,
WidthUnits = Units.ContentRatio,
```

Some examples... making a view half the width of it's parent:

```C#
Width = 0.5,
WidthUnits = Units.ParentRatio,
```

 making a view 15% taller than it's content:
 
```C#
Height = 1.15f,
HeightUnits = Units.ContentRatio,
```

maintaining a aspect ratio on an image:
 
```C#
Width = AutoSize.FillParent,
Height = 3.0f/4.0f,	// Height will always be 3/4 of width
HeightUnits = Units.AspectRatio,
```

making a view that is shorter in landscape mode:
 
```C#
Height = 0.3f,
HeightUnits = Units.ScreenRatio
```
    
### Minimum and Maximum Dimensions

You can restrict the dimensions of a view to a minimum or maximum absolute value using these properties:

* `MinWidth`
* `MaxWidth`
* `MinHeight`
* `MaxHeight`

These properties are applied after all other size calculations and will override autosize and ratio based sizes as described above.

### Things to watch out for

There are a couple of scenarios that you should watch out for when specifying auto-sized layout dimensions.  

* Be careful not to specify a dimension that is a ratio of the parent dimension when the parent dimension is specified as a ratio of the content dimension.  This is a chicken and egg situation and XibFree will try to resolve it by switching the inner dimension to wrap content - but it's bad form, and not well tested.
* Using parent ratios other than 1.0 with linear layout won't give the expected behaviour.  LinearLayout allocates space based on the weight of the subview.  Say you have a subview width a parent ratio of 0.5 and you put it in a linear layout and the layout decides to give the view 100px... the subview will only take 50px and the linear layout will try to share the unclaimed space across other subviews.  It all becomes a bit unpredictable - so just don't do it.
* Unresolvable aspect ratios.  If you use Units.AspectRatio for one dimension, the other adjacent dimension must not.