using System;
using System.Collections.Generic;
using System.Linq;
using FluentValidation.Results;

namespace SchoolLearningSystem.Applicationf.Exceptions
{
    public class CustomValidationException : Exception
    {
        public List<string> Errors { get; }

        public CustomValidationException(IEnumerable<ValidationFailure> failures)
            : base("حدث خطأ في التحقق من البيانات.")
        {
            Errors = failures.Select(e => e.ErrorMessage).ToList();
        }
    }
}