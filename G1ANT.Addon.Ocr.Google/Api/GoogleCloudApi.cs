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
using GoogleOCR = Google.Apis.Vision.v1.Data;
using System;
using System.Collections.Generic;
using System.Drawing;

namespace G1ANT.Language.Ocr.Google
{
    public class GoogleCloudApi
    {
        private static string _jsonCredential;
        public static string JsonCredential
        {
            get
            {
                return _jsonCredential;
            }
            set
            {
                try
                {
                    _jsonCredential = value;
                    googleCredential = CreateCredential(_jsonCredential);
                }
                catch
                {
                    googleCredential = null;
                    _jsonCredential = null;
                    throw new Exception("Invalid json credential. Cannot connect to the Google Cloud Service");
                }
            }
        }
        private static GoogleCredential googleCredential;


        public GoogleCloudApi()
        {
            if (googleCredential == null)
                throw new Exception("Before using this command, you need to login to Google Cloud Service. Please, use ocr.login command first");
        }

        private static GoogleCredential CreateCredential(string jsonCredential)
        {
            using (var stream = jsonCredential.ConvertStringToStream())
            {
                string[] scopes = { VisionService.Scope.CloudPlatform };
                var credential = GoogleCredential.FromStream(stream);
                return credential.CreateScoped(scopes);
            }
        }

        private VisionService CreateService()
        {
            var service = new VisionService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = googleCredential,
                ApplicationName = "G1ANT-Robot",
                GZipEnabled = true,
            });
            return service;
        }

        public string RecognizeText(Bitmap image, List<string> languages, int timeout)
        {
            BatchAnnotateImagesRequest batchRequest = new BatchAnnotateImagesRequest();
            batchRequest.Requests = new List<AnnotateImageRequest>();

            batchRequest.Requests.Add(new AnnotateImageRequest()
            {

                Features = new List<Feature>() { new Feature() { Type = "TEXT_DETECTION", MaxResults = 1 }, },
                ImageContext = new ImageContext() { LanguageHints = languages },

                Image = new GoogleOCR.Image()
                { Content = Convert.ToBase64String(image.ImageToBytes()) },
            });

            string output = string.Empty;
            using (var visioService = this.CreateService())
            {
                visioService.HttpClient.Timeout = new TimeSpan(0, 0, 0, 0, timeout);
                var annotate = visioService.Images.Annotate(batchRequest);
                BatchAnnotateImagesResponse batchAnnotateImagesResponse = annotate.Execute();
                var annotations = batchAnnotateImagesResponse.Responses;
                if (annotations.Count > 0 &&
                    annotations[0].TextAnnotations != null &&
                    annotations[0].TextAnnotations.Count > 0 &&
                    annotations[0].TextAnnotations[0].Description != null)
                {
                    output = annotations[0].TextAnnotations[0].Description.TrimEnd().TrimStart();
                }
            }
            return output;
        }

        public Rectangle RecognizeText(Bitmap image, string searchword, List<string> languages, int timeout)
        {
            Rectangle rect = new Rectangle();
            BatchAnnotateImagesRequest batchRequest = new BatchAnnotateImagesRequest();
            batchRequest.Requests = new List<AnnotateImageRequest>();
            batchRequest.Requests.Add(new AnnotateImageRequest()
            {
                Features = new List<Feature>() { new Feature() { Type = "TEXT_DETECTION", MaxResults = 1 }, },
                ImageContext = new ImageContext() { LanguageHints = languages },
                Image = new GoogleOCR.Image() { Content = Convert.ToBase64String(image.ImageToBytes()) },
            });

            string output = string.Empty;
            using (var visioService = this.CreateService())
            {

                visioService.HttpClient.Timeout = new TimeSpan(0, 0, 0, 0, timeout);
                var annotate = visioService.Images.Annotate(batchRequest);
                BatchAnnotateImagesResponse batchAnnotateImagesResponse = annotate.Execute();
                var annotations = batchAnnotateImagesResponse.Responses;
                if (annotations.Count > 0 &&
                    annotations[0].TextAnnotations != null &&
                    annotations[0].TextAnnotations.Count > 0 &&
                    annotations[0].TextAnnotations[0].Description != null)
                {
                    output = annotations[0].TextAnnotations[0].Description.TrimEnd().TrimStart();
                }

                if (annotations.Count > 0 &&
                    annotations[0].TextAnnotations != null &&
                    annotations[0].TextAnnotations.Count > 0)
                {
                    for (int i = 0; i < annotations[0].TextAnnotations.Count; i++)
                    {
                        var itemDescription = annotations[0].TextAnnotations[i].Description.ToLower();
                        var item = annotations[0].TextAnnotations[i].BoundingPoly.Vertices;
                        if (itemDescription.Equals(searchword.ToLower()))
                        {
                            rect = new Rectangle(item[0].X.Value, item[0].Y.Value, item[1].X.Value - item[0].X.Value, item[2].Y.Value - item[1].Y.Value);
                            break;
                        }
                        else
                        {

                            rect = new Rectangle(-1, -1, -1, -1);
                        }
                    }
                }
            }
            return rect;
        }
    }
}
