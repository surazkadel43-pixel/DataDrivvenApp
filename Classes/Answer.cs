using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Classes
{
    [Serializable]
    public class Answer
    {
        
        private int questionId;
        private int? optionId;
        private string answerText;
        private DateTime answerTime;
        private int answerOrder;  // New field for Answer Order
        private bool dependentAnswers = false;
        private int? optionOrder;
        #region Properties
        public int AnswerOrder  // New property to set and get Answer Order
        {
            get { return answerOrder; }
            set { answerOrder = value; }
        }
       
        // property for dependentAnswers
        public bool DependentAnswers
        {
            get { return dependentAnswers; }
            set { dependentAnswers = value; }
        }
        // Property for questionId
        public int QuestionId
        {
            get { return questionId; }
            set
            {
                if (value == 0)
                    throw new QuestionValidationException("Question ID cannot be zero.");
                questionId = value;
            }
        }

        // Property for optionId
        public int? OptionId
        {
            get { return optionId; }
            set
            {
                
                optionId = value;
            }
        }

        // Property for answerText
        public string AnswerText
        {
            get { return answerText; }
            set
            {
                if (value == null)
                    throw new QuestionValidationException( "Answer text cannot be null");
                answerText = value;
            }
        }

        // Property for answerTime
        public DateTime AnswerTime
        {
            get { return answerTime; }
            set
            {
                if (value == default(DateTime))
                    throw new QuestionValidationException("Answer time cannot be the default value.");

                // Ensure the value is converted to Australia's local time
                TimeZoneInfo australiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time");
                answerTime = TimeZoneInfo.ConvertTime(value.ToUniversalTime(), australiaTimeZone);
            }
        
        }

        public int? OptionOrder { get => optionOrder; set => optionOrder = value; }

        #endregion

        // Default constructor
        public Answer()
        {
            // Set the default date and time to Australia's local time
            TimeZoneInfo australiaTimeZone = TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time");
            this.answerTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, australiaTimeZone);
        }

        
        // Constructor to initialize all fields
       /* public Answer(int questionId, List<int?> optionId, List<string> answerText, DateTime answerTime)
        {
            this.questionId = questionId;
            this.optionId = optionId;
            this.answerText = answerText;
            this.answerTime = answerTime;
        }*/




    }
}