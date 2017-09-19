# LayoutParameters

Every View object has an associated LayoutParameters instance that defines how the view should be laid out.  

(Unlike Android where each ViewGroup has its own derived implementation of LayoutParameters, XibFree uses the one implementation for both LinearLayout and FrameLayout.  The approach of having this as a separate object attached by a property is used however as this will allow expansion of additional properties for other ViewGroup if necessary).

### Width Property

Sets or returns the desired width of this View.  Can be a pixel dimension, a ratio or one of the [AutoSize](sizing_views.md) constants.

### Height Property

Sets or returns the desired height of this View.  Can be a pixel dimension, a ratio or one of the [AutoSize](sizing_views.md) constants.

### WidthUnits/HeightUnits

Sets or returns the units of the associated width/height. Can be one of the following values:

* `Units.Absolute` - an absolute dimension, specified in pixels (or virtual pixels on Retina devices).
* `Units.ParentRatio` - a ratio of the enclosing parents dimension.
* `Units.ContentRatio` - a ratio of the view's content size.
* `Units.AspectRatio` - a ratio of the adjacent width/height dimension.
* `Units.ScreenRatio` - a ratio of the current device screen size (takes orientation into account).
* `Units.HostRatio` - a ratio of the size of the hosting UILayoutHost or UILayoutHostScrollable view.

See [Auto-sizing Views](auto_sizing_views.md) for more.

### MinWidth/MinHeight/MaxWidth/MaxHeight

Sets or returns the minimum/maximum width or height for a view.  Applied after all other calculations and is specified as an absolute pixel dimension.

### Weight Property

Used by LinearLayout to calculate the ratio used in dividing available space between multiple views that have an auto-size dimension of `AutoSize.FillParent`.

### Margins Property

Defines the space the should be left clear around the view.  Specified as a UIEdgeInsets, in pixel units.

### Margin*XXX* Properties

In addition to the Margins property which is supports settings all 4 margins at once, there are also individual properties MarginTop, MarginLeft, MarginRight, MarginBottom.

### Gravity Property

Defines how this view should be aligned within the parent container.  

For views contained in a FrameLayout this determines which edge of the parent view it should be aligned.  

For views contained in a LinearLayout this controls the orientation of the view in the direction adjacent to the direction of the linear layout (eg: the horizontal alignment for a view in a vertical linear layout).

### Visibility Property

Controls the visibility of the view.  Three modes are available:

* Visible - view is visible.
* Invisible - view is hidden, but still takes part in layout calculations causing it to "leave space" in the layout.
* Gone - as if the view has been removed from the layout - the space it would normally consume is collapsed.

See also the [View](view.md) properties `Gone` and `Visible`.