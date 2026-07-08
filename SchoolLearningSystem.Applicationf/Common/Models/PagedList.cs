namespace SchoolLearningSystem.Applicationf.Common.Models
{
    public class PagedList<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }

        // خصائص محسوبة تسهّل على الفرونت بناء أزرار الترقيم بدون حساب يدوي
        public bool HasPreviousPage => CurrentPage > 1;
        public bool HasNextPage => CurrentPage < TotalPages;

        public PagedList(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            Items = items;
            TotalCount = count;
            CurrentPage = pageNumber;
            PageSize = pageSize;

            // حماية دفاعية (Defense in Depth): لو استُدعي هذا الكلاس مستقبلاً
            // من مكان لا يمر عبر QueryParameters (اللي يضمن pageSize > 0)،
            // نتجنب هنا أي قسمة على صفر أو Math.Ceiling على قيمة غير منطقية.
            TotalPages = pageSize > 0
                ? (int)Math.Ceiling(count / (double)pageSize)
                : 0;
        }
    }
}