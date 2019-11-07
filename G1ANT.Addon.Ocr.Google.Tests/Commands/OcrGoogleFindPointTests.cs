/**
*    Copyright(C) G1ANT Ltd, All rights reserved
*    Solution G1ANT.Addon, Project G1ANT.Addon.Ocr
*    www.g1ant.com
*
*    Licensed under the G1ANT license.
*    See License.txt file in the project root for full license information.
*
*/
using G1ANT.Addon.Ocr.Google.Tests;
using G1ANT.Addon.Ocr.Tests.Properties;
using G1ANT.Engine;
using G1ANT.Language;
using NUnit.Framework;
using System;
using System.Drawing;
using System.Reflection;

namespace G1ANT.Addon.Ocr.Tests
{
    [TestFixture]
    public class OcrGoogleFindPointTests
    {
        private Scripter scripter;
        private string path;

        [OneTimeSetUp]
        public void Initialize()
        {
            Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
        }

        [SetUp]
        public void OcrGoogleFromScreenInitialize()
        {
            Language.Addon addon = Language.Addon.Load(@"G1ANT.Addon.Ocr.Google.dll");
            path = Assembly.GetExecutingAssembly().UnpackResourceToFile("Resources." + nameof(Resources.testimage), "png");
            scripter = new Scripter();
            scripter.InitVariables.Clear();
            OcrGoogleTests.StartPaint(path);
        }

        [Test, Timeout(OcrGoogleTests.TestTimeout)]
        public void OcrGoogleFromScreenTest()
        {
            Point expectedPoint = new Point(181, 294);
            string script = $@"
            window {SpecialChars.Text + SpecialChars.Search}Paint{SpecialChars.Text + SpecialChars.Search} style maximize
            ocrgoogle.login applicationname G1ANT-Robot jsoncredential {SpecialChars.Variable}credential{SpecialChars.IndexBegin}Ocr:google{SpecialChars.IndexEnd}
            ocrgoogle.findpoint search {SpecialChars.Text}animal{SpecialChars.Text} area (rectangle)68{SpecialChars.Point}162{SpecialChars.Point}767{SpecialChars.Point}528";
            scripter.Text = script;
            scripter.Run();
            var resultPoint = scripter.Variables.GetVariableValue<Point>("result");
            Assert.IsTrue(ArePointsSimilar(expectedPoint, resultPoint, 10));
        }

        [TearDown]
        public void OcrGoogleFromScreenCleanup()
        {
            OcrGoogleTests.KillAllPaints();
        }

        private bool ArePointsSimilar(Point p1, Point p2, int tolerance)
        {
            if (Math.Abs(p1.X - p2.X) > tolerance)
            {
                return false;
            }
            if (Math.Abs(p1.Y - p2.Y) > tolerance)
            {
                return false;
            }
            return true;
        }
    }
}
