using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SimpleCrawler;

namespace SimpleCrawlerTests
{
   

    [TestClass]
    public class TestWebsite
    {
        const string NonExistingFile = @"C:\Users\\SimpleCrawler\FileThatWillNeverEverExist.nonExistable";
        const string NoKeywordTag = @"C:\Users\\SimpleCrawler\NoKeywordTagSite.html";
        const string EmptyKeywordContent = @"C:\Users\\SimpleCrawler\EmptyKeywordContentSite.html";
        const string SingleKeywordTag = @"C:\Users\\SimpleCrawler\SingleKeywordTagSite.html";

        [TestMethod]
        public void TestNonExistingFile()
        {
            Website website = new Website();

            List<KeywordGridRow> gridRows = Website.AnalyseUrl(NonExistingFile);
            Debug.Assert(gridRows == null);
        }

        [TestMethod]
        public void TestEmptyKeywordAnalysys()
        {
            
            Website website = new Website();

            List<KeywordGridRow> gridRows = Website.AnalyseUrl(NoKeywordTag);
            Debug.Assert(gridRows == null);

            gridRows = Website.AnalyseUrl(EmptyKeywordContent);
            Debug.Assert(gridRows == null);
        }

        [TestMethod]
        public void TestSingleKeyword()
        {
            const int keywordCount = 3;
            const string keywordPhrase = "This is";
            Website website = new Website();

            List<KeywordGridRow> gridRows = Website.AnalyseUrl(SingleKeywordTag);
            Debug.Assert(gridRows.Count == 1);
            Debug.Assert(gridRows[0].Count == keywordCount.ToString());
            Debug.Assert(gridRows[0].Keyword == keywordPhrase);
        }
    }
}
