# Leveraging C# for Layout

One of the best reasons for code-only layouts is the ability to use C# language features to build your layouts.  In most of the XibFree examples I've been quite explicit and somewhat verbose in presenting the layout code in order to make it as clear as possible how the layout engine works.

In practice however there are many techniques we can use to make layouts more concise, more expressive and simply easier to code.

### Styles and Theming

XibFree makes is easy to define a consistant style, or theme for your application.  All we need to do is define our own C# classes for each styled control and then use them directly in layouts.

eg:

```C#
// Our bold label class
class BoldLabel : NativeView
{
    public BoldLabel(string text)
    {
        // Setup the label
        var label = new UILabel();
        label.Text = text;
        label.Font = UIFont.BoldSystemFontOfSize(18);
        label.BackgroundColor = UIColor.Clear;
        label.TextColor = UIColor.DarkGray;
        
        // Setup the layout params
        LayoutParameters = new LayoutParameters()
        {
            Width = AutoSize.FillParent,
            Height = AutoSize.WrapContent,
        }

        // Associate the label with this native view
        this.View = label;
    }
}
```

Now we can use it directly in a layout:

```C#
var layout = new LinearLayout(Orientation.Vertical)
{
    SubViews = new View[]
    {
        new BoldLabel("Apples"),
        new BoldLabel("Pears"),
        new BoldLabel("Oranges"),
    }
}
```

### Code Re-use

Suppose we have a number of places in our app where we need to present a common UI component.  We can define either a function to generate this piece of UI, or just wrap it up in a class.

eg: 

```C#
// A common button definition
class SocialButton : NativeView
{
    public SocialButton(string caption, Action handler)
    {
        // Create the button
        var button = new UIButton();
        button.SetTitle(caption, UIControlState.Normal);
        button.TouchUpInside += (sender, args)
        {
            handler();
        }
        this.View = button;
        
        // Layout
        this.LayoutParameters = new LayoutParameters(AutoSize.FillParent, AutoSize.WrapContent);
    }
}

// A social bar with three buttons horizontally stacked
class SocialBar : LinearLayout
{
    public enum ButtonKind
    {
        Facebook,
        Twitter,
        Email,
    }

    public SocialBar(Action<ButtonKind> handler) : base(Orientation.Horizontal)
    {
        SubViews = new View[]
        {
            new SocialButton("Facebook", () => handler(ButtonKind.Facebook)),
            new SocialButton("Twitter", () => handler(ButtonKind.Twitter)),
            new SocialButton("Email", () => handler(ButtonKind.Email)),
        }
    }
}
```

Now use it:

```C#
    var layout = new LinearLayout(Orientation.Vertical)
    {
        SubViews = new View[]
        {
            ...
            new SocialBar(onSocialButton),
            ...
        }
    }
    
    void onSocialButton(SocialBar.ButtonKind button)
    {
        switch (button)
        {
            case ButtonKind.Facebook:
                // etc...
                ..
                ..
        }
    }
```

### Powerful Development Tools

Both Visual Studio and Xamarin Studio have excellent support for working with code:

* IntelliSense/Code Completion - makes declaring XibFree layouts quick and efficient.
* Refactoring Tools - rename and refactor your layouts without fear of breaking things.
    
### Programmatically Generated Layouts

In most of the other examples on this site, the layouts are quite static - they're defined in a very hierarchical in nature and very similar to the XML layout used in Android development.  Given that this is code though there's no reason why we can't programmatically generate the layout.

### Data Driven Layouts

This is similar to the previous point, but with more of a focus on generating layout based on data either entered by the user, or retrieved from a network server, database etc...
