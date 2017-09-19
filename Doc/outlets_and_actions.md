# Replacing Outlets and Actions

Anyone who's used Xib's will be familiar with Interface Builder's Actions and Outlets.  If we're going to get rid of Xib files, we're going to need equivalent functionality.

### Outlets

Outlets are simply a way of wiring up a UIView in the Xib file with an object reference in code.  Since we're defining our layout in code, an outlet becomes nothing more than assigning the UIView to a member variable.  There's a couple of ways to do this.

The simplest way is to simply assign it to a variable inline with layout declaration:

```C#
new NativeView()
{
    View = _labelTitle = new UILabel()
    {
        ...
    },
},
```

Another way would be to use iOS's `ViewWithTag`:

```C#
// Define a tag iID
const int tag_title = 123;

...

new NativeView()
{
    View = new UILabel()
    {
        Tag = tag_title,
    },
},

...

// After hosting the layout, use ViewWithTag
var host = new UILayoutHost(layout);
var label = (UILabel)host.ViewWithTag(tag_title);
```

XibFree also has a version of `ViewWithTag` that can be used to locate view even before it's attached to a host:

```C#
var layout = new LinearLayout(...)...;
var label = layout.ViewWithTag<UILabel>(tag_title);
```

By using `ViewWithTag<NativeView>` we can locate the NativeView associated with a UIView.  This can be useful for changing the layout parameters (or visibility) of a view:

```C#
// Increase the top margin on the label 
var nativeView = layout.ViewWithTag<NativeView>(tag_title);
nativeView.LayoutParameters.Margins.Top  = 100;

// From the NativeView we can get the actual view with the View property
var label = (UILabel)nativeView.View;
Debug.Asset(label.Tag == tag_title);
```

    
XibFree's ViewWithTag can also locate a non-UIView item (such as a LinearLayout) in the view layout:

```C#
    // Since ViewGroups don't have an associated UIView, they have their own Tag property
    // that can be used to locate layout views.
    var layout = new LinearLayout(...)
    {
        ...
        new LinearLayout(Orientation.Vertical)
        {
            Tag = tag_panel,
            ...
        }
    }
    
    // Find the linear layout
    var panel = layout.ViewWithTag<LinearLayout>(id_panel);
```


### Actions

Actions in a Xib file are simply events that are hooked up to handlers in code.  Replicating this with XibFree is pretty simple and again there are a couple of ways to do it.

First, we can use the Init method to hook up an event handler:

```C#
...
new NativeView()
{
    View = new UIButton() {...},
    Init = v =>
    {
        v.As<UIButton>().TouchUpInside += onButtonTap;
    }
}
...

void OnButtonTap(object sender, EventArgs arg)
{
    // Handler code here
}
```

We could also write the handler directly inline if we wanted to:

```C#
...
new NativeView()
{
    View = new UIButton() {...},
    Init = v =>
    {
        v.As<UIButton>().TouchUpInside += (sender, args) =>
        {
            // Handler code here
        };
    }
}
...
```

The other way to hook up action handlers is directly on a view bound to a member variable using any of the methods described above.  eg:

```C#
var button = layout.ViewWithTag<UIButton>(id_button);
button.TouchUpInside += (sender, args) 
{
    // Handler code here
};
```
