using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Classes
{
    public class Question
    {

        private int id;
        private string questionText;
        private string questionType;
        private bool disable;
        private int questionOrder;

        #region Properties
        public int Id { get { return this.id; } set { this.id = value; } }
        public string QuestionText
        {
            get { return questionText; }
            set { questionText = value; }
        }

        public string QuestionType
        {
            get { return questionType; }
            set { questionType = value; }
        }

        public bool Disable
        {
            get { return disable; }
            set { disable = value; }
        }
        
        public int QuestionOrder
        {
            get { return questionOrder; }
            set { questionOrder = value; }
        }

        #endregion
       



    }


}