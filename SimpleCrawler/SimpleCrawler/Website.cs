using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using HtmlAgilityPack;

namespace SimpleCrawler
{
    public class Website
    {
        public static List<KeywordGridRow> AnalyseUrl(string url)
        {
            HtmlDocument content;

            if (Uri.IsWellFormedUriString(url, UriKind.RelativeOrAbsolute) && !string.IsNullOrWhiteSpace(url))
            {
                if (TryDownloadUrl(url, out content))
                {                 
                    string[] keywords = FetchKeywords(content);

                    if (keywords.Length > 0)
                    {
                        string siteContent = content.DocumentNode.SelectSingleNode("//body").InnerText;
                        return GetKeywordsRows(keywords, ref siteContent);
                    }
                    else
                    {
                        ReportError("No keywords found!");
                    }                
                }
            }
            else
            {
                ReportError("Given URL is not correct!");
            }

            return null;
        }

        private static bool TryDownloadUrl(string url, out HtmlDocument content)
        {
            using (WebClient client = new WebClient())
            {
                try
                {
                    client.Encoding = Encoding.UTF8;
                    string rawData = client.DownloadString(url);
                    content = new HtmlDocument();
                    content.LoadHtml(rawData);
                    return true;
                }
                catch (WebException e)
                {
                    ReportError(e.Message);
                }
                catch
                {
                    ReportError("Unhandled exeption");
                }

                content = null;
                return false;
            }
        }

        private static string[] FetchKeywords(HtmlDocument htmlDocument)
        {
            string[] result = new string[0];
            HtmlNode keywordsNode = htmlDocument.DocumentNode.SelectSingleNode("//meta[@name='keywords']");

            if (keywordsNode != null)
            {
                string keywordsLine = keywordsNode.GetAttributeValue("content", null);
                result = keywordsLine.Split(',').Select(x => x.Trim()).Where((x) => !string.IsNullOrWhiteSpace(x)).ToArray();
            }

            return result;
        }

        private static List<KeywordGridRow> GetKeywordsRows(string[] keywords, ref string siteContent)
        {
            List<KeywordGridRow> gridRows = new List<KeywordGridRow>();

            for (int i = 0; i < keywords.Length; i++)
            {
                int keywordCount = CountKeywordOccurences(ref siteContent, keywords[i]);
                gridRows.Add(new KeywordGridRow(keywords[i], keywordCount));
            }

            return gridRows;
        }

        private static int CountKeywordOccurences(ref string siteContent, string keyword)
        {
            int count = 0;

            for (int i = siteContent.IndexOf(keyword, StringComparison.InvariantCultureIgnoreCase); i != -1; count++)
            {
                i += keyword.Length;
                i = siteContent.IndexOf(keyword, i, StringComparison.InvariantCultureIgnoreCase);
            }

            return count;
        }
        
        private static void ReportError(string message)
        {
            ErrorManager.Instance.SetErrorText(message);
        }
    }
}