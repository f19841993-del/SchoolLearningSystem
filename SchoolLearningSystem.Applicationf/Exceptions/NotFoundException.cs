using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolLearningSystem.Applicationf.Exceptions
{
    // داخل مشروع Application
    public class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message) { }
    }
}
