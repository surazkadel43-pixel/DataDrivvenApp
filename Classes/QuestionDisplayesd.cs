using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Classes
{
    public class QuestionDisplayesd
    {
        
        private int questionId;
       
        private int questionOrderDatabase;
        private int questionOrderDisplayed;
        private int relatedQuestion;
        private int questionOrder;
        private int totalNumberOFQuestionInDatabase;

        public QuestionDisplayesd()
        {
            
        }
        public QuestionDisplayesd(int questionOrderDisplayed,  int relatedQuestion, int questionOrder)
        {
            this.questionOrderDisplayed = questionOrderDisplayed;
            this.relatedQuestion = relatedQuestion;
            this.questionOrder = questionOrder;
            
        }


        #region Properties
        public int QuestionId
        {
            get { return questionId; }
            set { questionId = value; }
        }

        public int QuestionOrderDatabase
        {
            get { return questionOrderDatabase; }
            set { questionOrderDatabase = value; }
        }

        public int QuestionOrderDisplayed
        {
            get { return questionOrderDisplayed; }
            set { questionOrderDisplayed = value; }
        }

        public int RelatedQuestion { get => relatedQuestion; set => relatedQuestion = value; }
        public int QuestionOrder { get => questionOrder; set => questionOrder = value; }
        public int TotalNumberOFQuestionInDatabase { get => totalNumberOFQuestionInDatabase; set => totalNumberOFQuestionInDatabase = value; }

        #endregion


    }
}