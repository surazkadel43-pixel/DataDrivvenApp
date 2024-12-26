using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Classes
{
    public class DependentQuestion
    {


        private int id;
        private int questionId;
        private int optionID;

        public DependentQuestion(int id, int questionId, int optionID)
        {
            this.id = id;
            this.questionId = questionId;
            this.optionID = optionID;
        }

        public int Id { get => this.id; set => this.id = value; }
        public int QuestionId { get => this.questionId; set => this.questionId = value; }
        public int OptionID { get => this.optionID; set => this.optionID = value; }
    }
}