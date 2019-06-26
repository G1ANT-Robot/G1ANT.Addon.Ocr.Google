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
    public static class InvoiceManger
    {
        //public InvoiceManger(string path, string type)
        //{
        //    ReadInvoice(path, type);
        //}
        public static string ReadText { get; private set; }
        public static PDFReader pdf;
        public static  ImageReader image;      
		
        public static void ReadInvoice (string path, string type)
        {
            if (type.Equals("Ocr"))
            {
                image = new ImageReader(path);
                ReadText = image.ReadText;
            } else
            {
				pdf = new PDFReader(path);
				ReadText = pdf.ReadText;
			}
        }

        public static string getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }
            else
            {
                return "";
            }
        }
    }
}
