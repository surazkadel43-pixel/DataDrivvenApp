using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Classes
{
    public class Member
    {
        
        private string givenName;
        private string lastName;
        private DateTime dob;
        private string phone;
        private DateTime registeredAt;
        private int respondentId;

        #region Properties
        // Properties for accessing the private fields
        

        public string GivenName
        {
            get { return givenName; }
            set { givenName = value; }
        }

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        public DateTime DOB
        {
            get { return dob; }
            set { dob = value; }
        }

        public string Phone
        {
            get { return phone; }
            set { phone = value; }
        }

        public DateTime RegisteredAt
        {
            get { return registeredAt; }
            set { registeredAt = value; }
        }

        public int RespondentId
        {
            get { return respondentId; }
            set { respondentId = value; }
        }
        #endregion

        #region Constructer
        // Default constructor
        public Member()
        {
            registeredAt = DateTime.Now; // Default value to match SQL `GETDATE()`
        }

        // Constructor with parameters
        public Member( string givenName, string lastName, DateTime dob, string phone, DateTime registeredAt, int respondentId )
        {
            
            this.givenName = givenName;
            this.lastName = lastName;
            this.dob = dob;
            this.phone = phone;
            this.registeredAt = registeredAt;
            this.respondentId = respondentId;
        }

        // Copy constructor
        public Member(Member other)
        {
            
            this.givenName = other.GivenName;
            this.lastName = other.LastName;
            this.dob = other.DOB;
            this.phone = other.Phone;
            this.registeredAt = other.RegisteredAt;
            this.respondentId = other.RespondentId;
        }

        #endregion
    }
}