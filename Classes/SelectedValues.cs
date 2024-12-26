using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Classes
{
    public class SelectedValues
    {
        private int questionId;
        private string userSelectedValues;

        

        // Getter and Setter for questionId
        public int QuestionId
        {
            get { return questionId; }
            set { questionId = value; }
        }

        // Getter and Setter for selectedValues
        public string UserSelectedValues
        {
            get { return this.userSelectedValues; }
            set { this.userSelectedValues = value; }
        }
    }
}