namespace MyFinance.Client.Models
{
    public class PagedResultDto<T>
    {
        public IList<T> Items { get; set; } = new List<T>();
        public int TotalCount { get; set; }
    }
}
