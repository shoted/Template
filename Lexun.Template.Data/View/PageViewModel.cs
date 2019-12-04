namespace Lexun.Template.Data.View
{
    public class PageViewModel
    {
        #region 公共属性

        public int Page { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public int PageCount { get; set; }
        public bool HasPrevPage { get; set; }
        public bool HasNextPage { get; set; }

        public PageViewModel(int page, int pageSize, int total)
        {
            Page = page;
            PageSize = pageSize;
            Total = total;
        }
        public PageViewModel CalcPageData()
        {
            PageCount = Total % PageSize == 0 ? Total / PageSize : Total / PageSize + 1;
            HasPrevPage = Page > 1;
            HasNextPage = Total > Page * PageSize;
            if (Page == PageCount)
            {
                HasNextPage = false;
            }
            return this;
        }

        #endregion
    }
}
