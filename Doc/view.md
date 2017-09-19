# View Class


The `View` class is the base class for all elements in a XibFree view hierarchy.

### Parent Property

The `Parent` property returns the Parent view of this view, or `null`.  This property can't be set - it's automatically updated by XibFree when the view is added to a ViewGroup.

### GetMeasuredSize() Method

Returns the last measured size of this view as a `SizeF`

### ViewWithTag&lt;T&gt;() Method

Locates a view with the specified tag.  Depending on the type passed for T, this method can return either the NativeView or UIView for a view.

### Gone Property

This is a convenience property that delegates to the view's `LayoutParameters.Visibility` and returns true if the Visibility is set to Visibility.Gone, otherwise false.  It can also be assigned to set the visibility to either Gone or Visible.

### Visible Property

This is a convenience property that delegates to the view's `LayoutParameters.Visibility` and returns true if the Visibility is set to Visibility.Visible, otherwise false.  It can also be assigned to set the visibility to either Visible or Invisible.



