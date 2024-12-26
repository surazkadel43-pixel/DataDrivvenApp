using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Classes
{
    [Serializable] // Marking the class as serializable
    public class RealQuestion
    {
        private int questionId;
        private string questionText;
        private List<int?> optionId;
        private List<string> optionText;
        private string questionType;
        private int questionDisable;
        private int questionOrder;
        private int optionAllowed;            // New field for optionAllowed
        private List<bool> relatedQuestion;    // New field for relatedQuestion
        private bool questionRequired;
        public bool QuestionRequired { get => questionRequired; set => questionRequired = value; }

        // Constructor
        public RealQuestion()
        {
            OptionId = new List<int?>();
            optionText = new List<string>();
            relatedQuestion = new List<bool>(); // Ensure initialization
        }
        #region Properties
        // Property for questionId
        public int QuestionId
        {
            get { return questionId; }
            set
            {
                if (value == 0)
                {
                    throw new ArgumentNullException(nameof(value), "QuestionId cannot be null.");
                }
                questionId = value;
            }
        }

        // Property for optionId
        public List<int?> OptionId { get; set; }
        // Property for QuestionText
        public string QuestionText
        {
            get { return questionText; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new QuestionValidationException("Question text cannot be null or empty.");
                questionText = value;
            }
        }

        // Property for OptionText
        public List<string> OptionText
        {
            get { return optionText; }
            set
            {
                if (value == null || value.Count == 0)
                    throw new QuestionValidationException("Option list cannot be null or empty.");
                optionText = value;
            }
        }

        // Property for QuestionType
        public string QuestionType
        {
            get { return questionType; }
            set
            {
                if (string.IsNullOrEmpty(value))
                    throw new QuestionValidationException("Question type cannot be null or empty.");
                questionType = value;
            }
        }

        // Property for QuestionDisable
        public int QuestionDisable
        {
            get { return questionDisable; }
            set { questionDisable = value; } // Boolean will always have a value (false by default)
        }

        // Property for QuestionOrder
        public int QuestionOrder
        {
            get { return questionOrder; }
            set
            {
                if (value < 0)
                    throw new QuestionValidationException("Question order cannot be negative.");
                questionOrder = value;
            }
        }

        public int OptionAllowed  // New property for optionAllowed
        {
            get { return optionAllowed; }
            set
            {
                if (value < 0)
                {
                    throw new QuestionValidationException("OptionAllowed cannot be negative.");
                }
                optionAllowed = value;
            }
        }

        public List<bool> RelatedQuestion  // New property for relatedQuestion
        {
            get { return relatedQuestion; }
            set
            {
                if (value == null)
                {
                    throw new QuestionValidationException("RelatedQuestion list cannot be null.");
                }
                relatedQuestion = value;
            }
        }
    }
    #endregion
    // Constructor
    
}


