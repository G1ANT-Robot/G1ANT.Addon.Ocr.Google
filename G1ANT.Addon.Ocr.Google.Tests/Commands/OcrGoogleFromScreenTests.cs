/**
*    Copyright(C) G1ANT Ltd, All rights reserved
*    Solution G1ANT.Addon, Project G1ANT.Addon.Ocr
*    www.g1ant.com
*
*    Licensed under the G1ANT license.
*    See License.txt file in the project root for full license information.
*
*/

using G1ANT.Addon.Ocr.Tests.Properties;
using G1ANT.Engine;
using G1ANT.Language;
using G1ANT.Language.Ocr.Google;
using NUnit.Framework;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;

namespace G1ANT.Addon.Ocr.Google.Tests
{
    [TestFixture]
    public class OcrGoogleFromScreenTests
    {
        private Scripter scripter;
        private string path;

        [OneTimeSetUp]
        public void Initialize()
        {
            System.Environment.CurrentDirectory = TestContext.CurrentContext.TestDirectory;
            //Window window = new Window(); //TODO  was G1ANT.Language.Core.Commands.Window i think do wyjebania
        }

        [SetUp]
        public void OcrFromScreenInitialize()
        {
            Language.Addon addon = Language.Addon.Load(@"G1ANT.Addon.Ocr.Google.dll");
            path = Assembly.GetExecutingAssembly().UnpackResourceToFile("Resources." + nameof(Resources.testimage), "png");
            scripter = new Scripter();
            scripter.InitVariables.Clear();
            OcrGoogleTests.StartPaint(path);
        }

        [Test, Timeout(OcrGoogleTests.TestTimeout)]
        public void GoogleOcrTest()
        {
            string expectedString = "animal";
            string script = $@"window {SpecialChars.Text + SpecialChars.Search}Paint{SpecialChars.Text + SpecialChars.Search} style maximize
                            ocrgoogle.login {SpecialChars.Variable}credential{SpecialChars.IndexBegin}Ocr:google{SpecialChars.IndexEnd}
                            ocrgoogle.fromscreen area (rectangle)68{SpecialChars.Point}162{SpecialChars.Point}767{SpecialChars.Point}528";
            scripter.Text = script;
            scripter.Run();
            var resultOutput = scripter.Variables.GetVariableValue<string>("result");
            Assert.AreEqual(expectedString, resultOutput);
        }

        [Test, Timeout(OcrGoogleTests.TestTimeout)]
        public void OcrTestGoogleApiTest()
        {
            GoogleCloudApi.JsonCredential = (string)scripter.Variables.GetVariable("credential").GetValue("Ocr:google").Object;
            var bitmapWithTestText = Resources.testimage;
            var expectedRectangle = new Rectangle(167, 142, 192, 51);
            var languages = new List<string>() { "en" };
            var timeout = 10000;
            GoogleCloudApi googleApi = new GoogleCloudApi();
            Rectangle foundRectangle = googleApi.RecognizeText(bitmapWithTestText, "animal", languages, timeout);
            Assert.AreEqual(expectedRectangle, foundRectangle);
        }

        [TearDown]
        public void OcrGoogleFromScreenCleanup()
        {
            OcrGoogleTests.KillAllPaints();
        }
    }
}