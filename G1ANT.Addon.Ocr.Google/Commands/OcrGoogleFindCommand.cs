/**
*    Copyright(C) G1ANT Ltd, All rights reserved
*    Solution G1ANT.Addon, Project G1ANT.Addon.Ocr
*    www.g1ant.com
*
*    Licensed under the G1ANT license.
*    See License.txt file in the project root for full license information.
*
*/

using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace G1ANT.Language.Ocr.Google
{
    [Command(Name = "ocrgoogle.find", Tooltip = "This command finds a specified text on the active screen and returns its position in a rectangle format")]
    public class OcrGoogleFindCommand : Command
    {
        public class Arguments : CommandArguments
        { 
            [Argument(Required = true,Tooltip = "Text to be found on the screen (the fewer words, the better results)")]
            public TextStructure Search { get; set; }

            [Argument(Tooltip = "Area on the screen to find text in, specified in `x0⫽y0⫽x1⫽y1` format, where `x0⫽y0` are the coordinates of the top left and `x1⫽y1` are the coordinates of the right bottom corner of the area")]
            public RectangleStructure Area { get; set; } = new RectangleStructure(SystemInformation.VirtualScreen);

            [Argument(Tooltip = "Determines whether the `area` argument is specified with absolute coordinates (top left corner of the screen) or refers to the currently opened window (its top left corner)")]
            public BooleanStructure Relative { get; set; } = new BooleanStructure(true);

            [Argument(Tooltip = "Name of a variable where the command's result will be stored")]
            public VariableStructure Result { get; set; } = new VariableStructure("result");

            [Argument(DefaultVariable = "timeoutOcr", Tooltip = "Specifies time in milliseconds for G1ANT.Robot to wait for the command to be executed")]
            public  override TimeSpanStructure Timeout { get; set; }

            [Argument(Tooltip = "List of languages you want to use to recognize text on the screen")]
            public TextStructure Languages { get; set; } = new TextStructure("en");
        }

        public OcrGoogleFindCommand(AbstractScripter scripter) : base(scripter) { }

        public void Execute(Arguments arguments)
        {
            var screenArea = !arguments.Relative.Value ? arguments.Area.Value : arguments.Area.Value.ToAbsoluteCoordinates(); 
            var screenImage = RobotWin32.GetPartOfScreen(screenArea);
            var timeout = (int)arguments.Timeout.Value.TotalMilliseconds;
            var languages = arguments.Languages.Value.Split(',').ToList();
            var search = arguments.Search.Value;
            var googleApi = new GoogleCloudApi();
            var rectangle = googleApi.FindTextPosition(screenImage, search, languages, timeout);

            if (rectangle == null)
                throw new NullReferenceException("Ocr was unable to find text position");

            Scripter.Variables.SetVariableValue(arguments.Result.Value, new RectangleStructure(rectangle.Value));
        }
    }
}
