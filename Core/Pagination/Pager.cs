namespace Core.Pagination
{
    public class Pager
    {
        public Pager(decimal pageNumber, decimal pageSize, decimal totalRecords)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecords = totalRecords;
        }

        public decimal PageNumber { get; set; }
        public decimal PageSize { get; set; }
        public decimal TotalPages
        {
            get
            {
                return Convert.ToInt32(Math.Ceiling(((double)TotalRecords / (PageSize == 0 ? 1 : (double)PageSize))));
            }
        }
        public decimal TotalRecords { get; set; }
    }
}