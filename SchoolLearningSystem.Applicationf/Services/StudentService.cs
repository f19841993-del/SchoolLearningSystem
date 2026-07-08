using AutoMapper;
using SchoolLearningSystem.Applicationf.DTOs.Student;
using SchoolLearningSystem.Applicationf.Exceptions;
using SchoolLearningSystem.Applicationf.Interfaces;
using SchoolLearningSystem.Applicationf.Services.Base;
using SchoolLearningSystem.Domain.Entities;
using SchoolLearningSystem.Domain.Enums;
using SchoolLearningSystem.Domain.Interfaces;

namespace SchoolLearningSystem.Applicationf.Services
{
    // ==================================================================================
    // 📌 دور هذا الـ Service:
    // يدير بيانات "الطالب" نفسه (الملف الشخصي، المرحلة الدراسية، سجل التقدم).
    // أي بيانات تخص علاقة الطالب بكيان آخر (كورساته، نتائجه) لها Service مستقل
    // مسؤول عنها (ICourseStudentService، IResultService على التوالي).
    // ==================================================================================
    public class StudentService
        : BaseService<Student, StudentReadDto, StudentCreateDto, StudentUpdateDto>, IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository, IMapper mapper)
            : base(studentRepository, mapper)
        {
            _studentRepository = studentRepository;
        }

        // 🔹 CRUD الأساسي موروث من BaseService

        // ============================================================================
        // 🎯 Use Case: "الأدمن يفتح تقرير كل طلاب صف معيّن (مثلاً كل طلاب الصف الرابع)
        //              لأغراض إدارية أو لتوزيعهم على كورسات جديدة"
        // ============================================================================
        public async Task<IEnumerable<StudentReadDto>> GetStudentsByGradeLevelAsync(GradeLevel gradeLevel)
        {
            var students = await _studentRepository.GetByGradeLevelAsync(gradeLevel);
            return _mapper.Map<IEnumerable<StudentReadDto>>(students);
        }

        // ============================================================================
        // 🎯 Use Case: "الطالب يفتح لوحة أدائه الشخصية، والنظام الذكي (SRS) يحتاج
        //              يعرف بالضبط أي الأسئلة يتقنها الطالب وأيها يحتاج مراجعة،
        //              عشان يقرر متى يعيد له نفس السؤال (التكرار المتباعد)"
        //
        // مين يستدعيها: محرك الـ SRS (Spaced Repetition System) الداخلي، أو صفحة
        //               "تقدمي" بواجهة الطالب.
        //
        // 💡 ليش ما نرجع null هنا (خلافاً لبعض الدوال السابقة)؟ لأن طلب "تقدم طالب"
        //    بدون وجود الطالب أصلاً هو حالة خطأ حقيقية (مو حالة عمل طبيعية)،
        //    فنمط Fail Fast هنا هو الأنسب.
        // ============================================================================
        public async Task<StudentReadDto> GetStudentWithProgressAsync(int studentId)
        {
            var student = await _studentRepository.GetStudentWithProgressAsync(studentId)
                ?? throw new NotFoundException($"الطالب برقم {studentId} غير موجود.");

            return _mapper.Map<StudentReadDto>(student);
        }
    }
}