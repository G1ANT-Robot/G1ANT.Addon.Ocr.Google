/**
*    Copyright(C) G1ANT Ltd, All rights reserved
*    Solution G1ANT.Addon, Project G1ANT.Addon.Ocr
*    www.g1ant.com
*
*    Licensed under the G1ANT license.
*    See License.txt file in the project root for full license information.
*
*/

using G1ANT.Engine;
using G1ANT.Language;
using System.Drawing;
using NUnit.Framework;
using System.Reflection;
using System;
using G1ANT.Addon.Ocr.Tests.Properties;

namespace G1ANT.Addon.Ocr.Google.Tests
{
    [TestFixture]
    public class OcrGoogleFindTests
    {
        private Scripter scripter;
        private string path;

        [OneTimeSetUp]
        public void Initialize()
        {
            Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
        }

        [SetUp]
        public void TestInit()
        {
            Language.Addon addon = Language.Addon.Load(@"G1ANT.Addon.Ocr.Google.dll");
            path = Assembly.GetExecutingAssembly().UnpackResourceToFile("Resources." + nameof(Resources.testimage), "png");
            scripter = new Scripter();
            scripter.InitVariables.Clear();
            OcrGoogleTests.StartPaint(path);
        }

        [Test, Timeout(OcrGoogleTests.TestTimeout)]
        public void OcrFindCommandTest()
        {
            Rectangle expectedRectangle = new Rectangle(64, 102, 191, 51);
            string script = $@"
            window {SpecialChars.Text + SpecialChars.Search}Paint{SpecialChars.Text + SpecialChars.Search} style maximize
            ocrgoogle.login applicationname G1ANT-Robot jsoncredential {SpecialChars.Variable}credential{SpecialChars.IndexBegin}Ocr:google{SpecialChars.IndexEnd}
            ocrgoogle.find search {SpecialChars.Text}animal{SpecialChars.Text} area (rectangle)110{SpecialChars.Point}184{SpecialChars.Point}564{SpecialChars.Point}488";
            scripter.Text = script;
            scripter.Run();
            var resultRectangle = scripter.Variables.GetVariableValue<Rectangle>("result");
            Assert.IsTrue(AreRectanglesSimilar(expectedRectangle, resultRectangle, 10));
        }

        [TearDown]
        public void TestCleanup()
        {
            OcrGoogleTests.KillAllPaints();
        }

        private bool AreRectanglesSimilar(Rectangle r1, Rectangle r2, int tolerance)
        {
            if (Math.Abs(r1.X - r2.X) > tolerance)
            {
                return false;
            }
            if (Math.Abs(r1.Y - r2.Y) > tolerance)
            {
                return false;
            }
            if (Math.Abs(r1.Width - r2.Width) > tolerance)
            {
                return false;
            }
            if (Math.Abs(r1.Height - r2.Height) > tolerance)
            {
                return false;
            }
            return true;
        }
    }
}
