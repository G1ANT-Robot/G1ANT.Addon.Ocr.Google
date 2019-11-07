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

namespace G1ANT.Language.Ocr.Google
{
    [Command(Name = "ocrgoogle.login", Tooltip = "This command logs in to the Google Cloud text recognition service")]
    public class OcrGoogleLoginCommand : Command
    {
        public class Arguments : CommandArguments
        {
            [Argument(Required = true, Tooltip = "Application name obtained from the Google Cloud text recognition service")]
            public TextStructure ApplicationName { get; set; }

            [Argument(Tooltip = "JSON credential obtained from the Google Cloud text recognition service")]
            public TextStructure JsonCredential { get; set; }

            [Argument(Tooltip = "API key obtained from the Google Cloud text recognition service")]
            public TextStructure ApiKey { get; set; }
        }

        public OcrGoogleLoginCommand(AbstractScripter scripter) : base(scripter) { }
        
        public void Execute(Arguments arguments)
        {
            if (!string.IsNullOrEmpty(arguments.ApiKey?.Value))
                GoogleCloudApi.InitializeApiKeyCredential(arguments.ApplicationName.Value, arguments.ApiKey.Value);
            else if (!string.IsNullOrEmpty(arguments.JsonCredential?.Value))
                GoogleCloudApi.InitializeJsonCredential(arguments.ApplicationName.Value, arguments.JsonCredential.Value);
            else
                throw new ArgumentException($"You must provide {nameof(arguments.ApiKey)} or {nameof(arguments.JsonCredential)} argument to log in to the Google Cloud Service");

            OnScriptEnd = () => GoogleCloudApi.ClearCredential();
        }
    }
}
