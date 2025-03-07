namespace MVC_Project.Contract
{
    public class Pagination
    {
        public int PageNumber { set; get; }
        public int PageSize { set; get; }
        public bool SkipPagination { set; get; }
        public string? SortColumn { set; get; }
        public bool SortDesc { set; get; }
    }
}
