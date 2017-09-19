# UILayoutHost

UILayoutHost is UIView capable of hosting a XibFree layout.

### Layout Property

Sets or retrieves the View object that is the root of the view hierarchy to be displayed in this UILayoutHost.

### SizeThatFits (Overridden Method)

SizeThatFits is implemented by UILayoutHost to measure the internally hosted view hierarchy and returns its enclosing size.  

### LayoutSubviews (Overridden Method)

Remeasures and lays out the hosted views.

### FindNativeView() method

Finds the `NativeView` object associated with a specified `UIView`.

