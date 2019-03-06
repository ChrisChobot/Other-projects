namespace SimpleCrawler
{
    public struct KeywordGridRow
    {
        public string Keyword { get; }
        public string Count { get; }

        public KeywordGridRow(string keyword, int count)
        {
            Keyword = keyword;
            Count = count.ToString();
        }
    }
}