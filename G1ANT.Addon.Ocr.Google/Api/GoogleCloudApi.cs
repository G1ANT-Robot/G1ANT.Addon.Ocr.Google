/**
*    Copyright(C) G1ANT Ltd, All rights reserved
*    Solution G1ANT.Addon, Project G1ANT.Addon.Ocr
*    www.g1ant.com
*
*    Licensed under the G1ANT license.
*    See License.txt file in the project root for full license information.
*
*/

using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Vision.v1;
using Google.Apis.Vision.v1.Data;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Drawing;
using Image = Google.Apis.Vision.v1.Data.Image;

namespace G1ANT.Language.Ocr.Google
{
    public class GoogleCloudApi
    {
        private static BaseClientService.Initializer initializer;

        public GoogleCloudApi()
        {
            if (initializer == null)
                throw new Exception("Before using this command, you need to login to Google Cloud Service. Please, use ocrgoogle.login command first");
        }

        public static void InitializeJsonCredential(string applicationName, string jsonCredential)
        {
            try
            {
                using (var stream = jsonCredential.ConvertStringToStream())
                {
                    initializer = new BaseClientService.Initializer
                    {
                        ApplicationName = applicationName,
                        HttpClientInitializer = GoogleCredential.FromStream(stream).CreateScoped(VisionService.Scope.CloudPlatform),
                        GZipEnabled = true,
                    };
                }
            }
            catch
            {
                throw new Exception("Invalid json credential");
            }
        }

        public static void InitializeApiKeyCredential(string applicationName, string apiKey)
        {
            initializer = new BaseClientService.Initializer
            {
                ApplicationName = applicationName,
                ApiKey = apiKey,
                GZipEnabled = true,
            };
        }

        public static void ClearCredential()
        {
            initializer = null;
        }

        public string RecognizeText(Bitmap image, List<string> languages, int timeout)
        {
            try
            {
                return DoWithVisionService(image, languages, timeout, annotations =>
                {
                    return annotations.First().TextAnnotations.First().Description?.Trim();
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not recognize text. Message: {ex.Message}", ex);
            }
        }

        public Rectangle? FindTextPosition(Bitmap image, string searchWord, List<string> languages, int timeout)
        {
            try
            {
                return DoWithVisionService(image, languages, timeout, annotations =>
                {
                    var rectangle = default(Rectangle?);
                    var annotation = annotations.First().TextAnnotations.FirstOrDefault(a => string.Equals(a.Description, searchWord, StringComparison.InvariantCultureIgnoreCase));

                    if (annotation != null)
                        rectangle = GetBoundingRectangle(annotation.BoundingPoly);

                    return rectangle;
                });
            }
            catch (Exception ex)
            {
                throw new Exception($"Could not find text position. Message: {ex.Message}", ex);
            }
            
        }

        private T DoWithVisionService<T>(Bitmap image, List<string> languages, int timeout, Func<IList<AnnotateImageResponse>, T> func)
        {            
            using (var visionService = new VisionService(initializer))
            {
                visionService.HttpClient.Timeout = TimeSpan.FromMilliseconds(timeout);

                var result = default(T);
                var batchRequest = GetBatchAnnotateImagesRequest(image, languages);
                var annotations = visionService.Images.Annotate(batchRequest).Execute().Responses;

                if (IsAnyTextAnnotatations(annotations))
                {
                    result = func.Invoke(annotations);
                }

                return result;
            }
        }

        private BatchAnnotateImagesRequest GetBatchAnnotateImagesRequest(Bitmap image, List<string> languages)
        {
            return new BatchAnnotateImagesRequest
            {
                Requests = new List<AnnotateImageRequest>
                {
                    new AnnotateImageRequest()
                    {
                        Features = new List<Feature> { new Feature() { Type = "TEXT_DETECTION", MaxResults = 1 } },
                        ImageContext = new ImageContext { LanguageHints = languages },
                        Image = new Image { Content = Convert.ToBase64String(image.ImageToBytes()) },
                    }
                }
            };
        }

        private bool IsAnyTextAnnotatations(IList<AnnotateImageResponse> annotations)
        {
            return annotations != null && 
                annotations.Any() && 
                annotations.First().TextAnnotations != null &&
                annotations.First().TextAnnotations.Any();
        }

        private Rectangle GetBoundingRectangle(BoundingPoly boundingPoly)
        {
            return new Rectangle(
                boundingPoly.Vertices[0].X.Value,
                boundingPoly.Vertices[0].Y.Value,
                boundingPoly.Vertices[1].X.Value - boundingPoly.Vertices[0].X.Value,
                boundingPoly.Vertices[2].Y.Value - boundingPoly.Vertices[1].Y.Value);
        }
    }
}
