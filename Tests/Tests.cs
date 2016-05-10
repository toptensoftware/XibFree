using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.iOS;
using Xamarin.UITest.Queries;
using System.Drawing;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        private static iOSApp app;

        [SetUp]
        public void BeforeEachTest()
        {
            if (app == null)
            {
                app = ConfigureApp
				.iOS
                .EnableLocalScreenshots()
				.StartApp();
                
            }
        }

        /*
        [Test]
        public void AppLaunches()
        {
            var path = app.Screenshot("s1");
            app.Repl();
        }*/

        [Test]
        public void Basics()
        {
            TestScreen("#1 Basics");
        }

        [Test]
        public void LinearLayout()
        {
            TestScreen("LinearLayout");
        }

        [Test]
        public void LinearLayout2()
        {
            TestScreen("LinearLayout2");
        }
        [Test]
        public void GridLayout()
        {
            TestScreen("GridLayout");
        }
        [Test]
        public void FrameLayout()
        {
            TestScreen("FrameLayout");
        }
        [Test]
        public void NestedHosts()
        {
            TestScreen("Nested Hosts");
        }
        [Test]
        public void NestedHosts_VisibilityBug()
        {
            TestScreen("Nested Hosts Visibility Bug", () => {
                app.Tap(x=>x.Button("Hide/Show"));
                ScreenshotAndCompare("Nested Hosts Visibility Bug 1");

                app.Tap(x=>x.Button("Hide/Show"));
                ScreenshotAndCompare("Nested Hosts Visibility Bug 2");

                app.Tap(x=>x.Button("Hide/Show"));
                ScreenshotAndCompare("Nested Hosts Visibility Bug 3");
            });
        }
        [Test]
        public void ViewGroup_Layers()
        {
            TestScreen("ViewGroup Layers");
        }
        [Test]
        public void TableViewCell()
        {
            TestScreen("TableViewCell");
        }
        [Test]
        public void TableViewCell_Variable()
        {
            TestScreen("TableViewCell Variable");
        }

        [Test]
        public void WrapLayout()
        {
            TestScreen("Wrap Layout");
        }

        [Test]
        public void Visibility()
        {
            TestScreen("Visibility", ()=>{
                app.Tap(x=>x.Button("ChangeVisibility"));
                ScreenshotAndCompare("Visibility_1");

                app.Tap(x=>x.Button("ChangeVisibility"));
                ScreenshotAndCompare("Visibility_2");

                app.Tap(x=>x.Button("ChangeVisibility"));
                ScreenshotAndCompare("Visibility_3");

            });
        }
        [Test]
        public void RecalculateLayout()
        {
            TestScreen("Recalculate Layout", () => {
                app.Tap(x=>x.Button("Change"));
                ScreenshotAndCompare("Recalculate Layout 1");

                app.Tap(x=>x.Button("Change"));
                ScreenshotAndCompare("Recalculate Layout 2");

                app.Tap(x=>x.Button("Change"));
                ScreenshotAndCompare("Recalculate Layout 3");

            });
        }

        private void TestScreen(string name, Action testAction = null) {
            SwitchTo(name);

            try
            {
                ScreenshotAndCompare(name);

                if (testAction != null) {
                    testAction();
                }
            }
            finally
            {
                GoBack();   
            }
        }

        private void ScreenshotAndCompare(string name) 
        {
            var file = app.Screenshot(name);

            CompareFiles("../../ReferenceImages/"+name+".png", file.FullName);
        }
        /*
        private string GetReferenceImage(string name) {
            return Path.Combine("", name + ".png");
        }*/
        private void CompareFiles(string file1, string file2) {
            
            try
            {
                var image1 = Bitmap.FromFile(file1);
                var image2 = Bitmap.FromFile(file2);

                byte[] image1Bytes;
                byte[] image2Bytes;

                using(var mstream = new MemoryStream())
                {
                    image1.Save(mstream, image1.RawFormat);
                    image1Bytes = mstream.ToArray();
                }

                using(var mstream = new MemoryStream())
                {
                    image2.Save(mstream, image2.RawFormat);
                    image2Bytes = mstream.ToArray();
                }

                if (image1Bytes.Length != image2Bytes.Length)
                    throw new InvalidOperationException("Size is different, images don't match");

                for (int i = 0; i < image1Bytes.Length-1; i++) {
                    if (image1Bytes[i] != image2Bytes[i])
                        throw new InvalidOperationException("Images don't match at byte " + i);
                }

                var image164 = Convert.ToBase64String(image1Bytes);
                var image264 = Convert.ToBase64String(image2Bytes);

                if (!string.Equals(image164, image264)) 
                {
                    throw new InvalidOperationException("images don't match");
                }
            }
            catch (Exception ex)
            {
                var newName = file1.Replace(".png", "_current.png");
                File.Delete(newName);
                File.Copy(file2, newName);

                Assert.Fail(file1 + " " + ex.ToString());
            }

        }

        private void SwitchTo(string screen) {
            app.WaitForElement(x => x.Class("UINavigationItemView").Child().Text("XibFree Demos"));

            var limit = 5;
            var elements = app.Query(x => x.Marked(screen).Class("UITableViewCell"));
            while (!elements.Any() && limit > 0)
            {
                app.ScrollDown(x => x.Class("UIScrollView"));
                elements = app.Query(x => x.Marked(screen).Class("UITableViewCell"));
                limit--;
            }
            app.Tap(x=>x.Marked(screen).Class("UITableViewCell"));
            //app.Tap(x => x.Marked(screen).Class("UITableViewCell"));
            //app.WaitForElement(x => x.Class("UINavigationItemView").Child().Text(screen));
        }

        private void GoBack() {
            app.Tap(x => x.Class("UINavigationItemButtonView"));
            app.WaitForElement(x => x.Class("UINavigationItemView").Child().Text("XibFree Demos"));
        }
    }
}

