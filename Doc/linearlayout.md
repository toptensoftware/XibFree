# LinearLayout

The LinearLayout view group is the main workhorse of automatic view layout.  It simply stacks its subviews either vertically or horizontally.  For subviews that have a dimension of `AutoSize.FillParent` it divides the available space between those subviews.

### Orientation

The orientation of a LinearLayout can be specified when it's created

    var layout = new LinearLayout(Orientation.Vertical) ...
    
### Adding Sub-views    

The subviews can be declared using the SubViews property:

```C#
var layout = new LinearLayout(Orientation.Vertical)
{
    ...
    SubViews = new View[]
    {
        new NativeView() { ... }
        new NativeView() { ... }
    }
};
```

LinearLayout's super-class `ViewGroup` also contains methods for programmatically adding and removing subviews.

### Padding

LinearLayout supports padding to control the space around the contained subviews:

```C#
var layout = new LinearLayout(Orientation.Vertical)
{
    Padding = new UIEdgeInsets(10, 10, 10, 10),
    ...
};
```

### Spacing

The Spacing property controls how much space the LinearLayout leaves between each subview.

```C#
var layout = new LinearLayout(Orientation.Vertical)
{
    Spacing = 10,		// 10 pixels between each sub-view
    ...
};
```


### Weighting

By default when a LinearLayout contains multiple views with a dimension of `AutoSize.FillParent`, LinearLayout will divide the available space evenly between them.  You can change this ratio by setting the `Weight` property in the `LayoutParameters` of the subviews:

```C#
new NativeView()
{
	...
	LayoutParameters = new LayoutParameters()
	{
		...
		Weight = 2,		// Make this view twice as wide as the others
	},
},
```

The default weight is 1, so by setting it to 2 as in this example this view will use twice as much space as the other views in this LinearLayout.

### Gravity

By default, LinearLayout will align it's contents in the top-left corner.  For vertical layouts, the subview's will be left-aligned while for horizontal layouts the subviews will be top-aligned.  You can control this alignment with the LinearLayout's Gravity property as well as the Gravity property on the sub-view's LayoutParameters.

The LinearLayout's Gravity property controls how the subviews as a group are aligned within the LinearLayout.  

For example, a vertical layout with 3 views whose total height is less than that of the LinearLayout, the Gravity property can be used to either center them vertically, or place them at the bottom.

```C#
var layout = new LinearLayout(Orientation.Vertical)
{
    Gravity = Gravity.Bottom,		// Place the stack of subviews at the bottom of this linear layout
}
```

For alignment in the adjacent direction of the layout, LinearLayout first looks at the gravity of the subview's LayoutParameters and if not specified, uses the Gravity of the LinearLayout itself.

For example, a vertical layout that contains a subview that is less than the width of the LinearLayout can be aligned to the left, right, or centered using the subview's LayoutParameters.Gravity property:

```C#
var layout = new LinearLayout(Orientation.Vertical)
{
    ...
    SubViews = new View[]
    {
        new NativeView()
        {
            ...
            LayoutParameters = new LayoutParameters()
            {
                Width = 100,
                Gravity = Gravity.Center,	// Center this view horizontally within the vertical LinearLayout
            }
        }
    }
}
```

If the subview doesn't specify a gravity, the gravity of the LinearLayout is used:

```C#
var layout = new LinearLayout(Orientation.Vertical)
{
    // Position the stack of views at the bottom of this LinearLayout, but also center
    // horizontally all views that don't have their own LayoutParameters.Gravity property set.
    Gravity = Gravity.BottomCenter,
}
```

The above examples use a vertical LinearLayout but the same principle applies for horizontal layouts in the adjacent direction.