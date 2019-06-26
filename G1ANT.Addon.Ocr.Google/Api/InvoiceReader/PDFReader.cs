/**
*    Copyright(C) G1ANT Ltd, All rights reserved
*    Solution G1ANT.Addon, Project G1ANT.Addon.Ocr
*    www.g1ant.com
*
*    Licensed under the G1ANT license.
*    See License.txt file in the project root for full license information.
*
*/
using System.Collections.Generic;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;

namespace G1ANT.Language.Ocr.Google
{
    public class PDFReader
    {
        public PDFReader()
        { }

        public PDFReader(string path)
        {
            ReadText = GetTextFromPDF(path);

        }

        public string ReadText { get; }



        public static string GetTextFromPDF(string path)
        {
            StringBuilder text = new StringBuilder();
            using (PdfReader reader = new PdfReader(path))
            {
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    text.Append(PdfTextExtractor.GetTextFromPage(reader, i));
                }
            }

            return text.ToString();
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
        //public static List<string> GetBetween1(string strSource, string strStart, string strEnd)
        //{
        //    List<string> list = new List<string>();
        //    int Start, End;
        //    if (!strSource.Contains(strStart) || !strSource.Contains(strEnd))
        //    {
        //        return list;
        //    }
        //    else
        //    {
        //        while (strSource.Contains(strStart) && strSource.Contains(strEnd))
        //        {
        //            Start = strSource.IndexOf(strStart, 0) + strStart.Length;
        //            End = strSource.IndexOf(strEnd, Start);
        //            list.Add(strSource.Substring(Start, End - Start));
        //            Start=Start + Start;
        //            End = End+End;
                    
        //            //return strSource.Substring(Start, End - Start);
        //        }
        //        return list;
        //    }
        //}


        public static List<string> lGetTextFromPDF(string path)
        {
            List<string> text = new List<string>();
            using (PdfReader reader = new PdfReader(path))
            {
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    
                    text.Add(PdfTextExtractor.GetTextFromPage(reader, i));
                }
            }

            return text;
        }

    }
}
