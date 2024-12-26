using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Classes
{
    public class QuestionValidationException : Exception
    {
        // Default constructor
        public QuestionValidationException()
            : base("An error occurred while validating the question.") { }

        // Constructor that takes a custom message
        public QuestionValidationException(string message)
            : base(message) { }

        // Constructor that takes a custom message and an inner exception
        public QuestionValidationException(string message, Exception innerException)
            : base(message, innerException) { }

        // Optionally, you can include additional properties or methods if needed
    }
}