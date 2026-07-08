namespace SchoolLearningSystem.Applicationf.Common.Parameters
{
    public class QueryParameters
    {
        // 1. تحديد الحد الأقصى المسموح به في الطلب الواحد
        const int maxPageSize = 50;

        // 2. الصفحة الافتراضية هي 1
        private int _pageNumber = 1;

        // 3. حماية PageNumber: لا تقل عن 1 أبداً
        // (بدون هذي الحماية، PageNumber = 0 أو قيمة سالبة تنتج Skip سالب
        // بالـ Repository وترمي ArgumentException وقت التشغيل)
        public int PageNumber
        {
            get { return _pageNumber; }
            set { _pageNumber = value < 1 ? 1 : value; }
        }

        // 4. الحجم الافتراضي هو 10
        private int _pageSize = 10;

        // 5. حماية الـ PageSize من الحدين معاً:
        //    - لا يتجاوز الحد الأقصى (maxPageSize)
        //    - لا يكون صفر أو سالب (يمنع DivideByZero وكسر Skip/Take)
        public int PageSize
        {
            get { return _pageSize; }
            set
            {
                if (value <= 0)
                    _pageSize = 10;
                else if (value > maxPageSize)
                    _pageSize = maxPageSize;
                else
                    _pageSize = value;
            }
        }

        public string? SearchTerm { get; set; }
    }
}