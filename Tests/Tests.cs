using System;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Xamarin.UITest;
using Xamarin.UITest.iOS;
using Xamarin.UITest.Queries;

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
                ScreenshotAndCompare("NestedHosts_VisibilityBug_1");

                app.Tap(x=>x.Button("Hide/Show"));
                ScreenshotAndCompare("NestedHosts_VisibilityBug_2");

                app.Tap(x=>x.Button("Hide/Show"));
                ScreenshotAndCompare("NestedHosts_VisibilityBug_3");
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
                ScreenshotAndCompare("RecalculateLayout_1");

                app.Tap(x=>x.Button("Change"));
                ScreenshotAndCompare("RecalculateLayout_2");

                app.Tap(x=>x.Button("Change"));
                ScreenshotAndCompare("RecalculateLayout_3");

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
                var bytes1 = File.ReadAllBytes(file1);
                var bytes2 = File.ReadAllBytes(file2);


                if ((new FileInfo(file1).Length) != (new FileInfo(file2).Length)) 
                {
                    throw new InvalidOperationException("images don't match");
                }
            }
            catch (Exception ex)
            {
                var newName = file1.Replace(".png", "_current.png");
                File.Delete(newName);
                File.Copy(file2, newName);

                Assert.Fail(ex.ToString());
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

