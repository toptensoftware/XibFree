# ViewGroup Class

The ViewGroup class is the base class for all LayoutContainers - specifically LinearLayout and FrameLayout. It contains methods and attributes for managing the subviews of a layout container.

### SubViews Property

Gets or sets the subviews of this ViewGroup. Implemented as an IEnumerable<View> when assigned, the ViewGroup will take a copy of the array and store it in its own internal List.

### InsertSubView()/AddSubView() Methods

Various overloads for inserting a view into the view hierarchy.

### RemoveSubView()

Various overloads for removing a view from the view hierarchy.

### Padding Property

Sets or retrieves a UIEdgeInsets that defines a space that will be reserved around the outside of the container.

### Tag Property

An integer "tag" value that can be used with the View.ViewWithTag to locate a view group in the view hierarchy.

### FindNativeView() Method

Finds the NativeView wrapper for a specified UIView instance.

### Layer Property

Sets or retrieves a CALayer object that will be positioned as the background for this ViewGroup.

 