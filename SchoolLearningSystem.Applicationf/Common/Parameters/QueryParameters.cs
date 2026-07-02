namespace SchoolLearningSystem.Application.Common.Parameters
{
    public class QueryParameters
    {
        // 1. تحديد الحد الأقصى المسموح به في الطلب الواحد
        const int maxPageSize = 50;

        // 2. الصفحة الافتراضية هي 1
        public int PageNumber { get; set; } = 1;

        // 3. الحجم الافتراضي هو 10
        private int _pageSize = 10;

        // 4. حماية الـ PageSize لكي لا يتجاوز الـ Max
        public int PageSize
        {
            get { return _pageSize; }
            set { _pageSize = (value > maxPageSize) ? maxPageSize : value; }
        }

        public string? SearchTerm { get; set; }
    }
}