/**
*    Copyright(C) G1ANT Ltd, All rights reserved
*    Solution G1ANT.Addon, Project G1ANT.Addon.Ocr
*    www.g1ant.com
*
*    Licensed under the G1ANT license.
*    See License.txt file in the project root for full license information.
*
*/

namespace G1ANT.Language.Ocr.Google
{
    [Command(Name = "ocrgoogle.login",
        Tooltip = "This command logs in to the Google Cloud text recognition service")]

    public class OcrGoogleLoginCommand : Command
    {
        public class Arguments : CommandArguments
        {
            [Argument(Required = true, Tooltip = "JSON credential obtained from the Google Cloud text recognition service")]
            public TextStructure JsonCredential { get; set; }
        }

        public OcrGoogleLoginCommand(AbstractScripter scripter) : base(scripter) { }
        
        public void Execute(Arguments arguments)
        {
            GoogleCloudApi.JsonCredential = arguments.JsonCredential.Value;

            OnScriptEnd = () =>
            {
                try
                {
                    GoogleCloudApi.JsonCredential = null;
                }
                catch
                {

                }
            };
        }
    }
}
