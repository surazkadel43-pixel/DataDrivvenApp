using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Classes
{
    public class Respondent
    {
        private int id;
        private DateTime registeredDateTime;
        private string macAddress;

        #region Properties
        // Public properties for accessing private fields
        public int Id
        {
            get { return id; }
            set { id = value; }
        }

        public DateTime RegisteredDateTime
        {
            get { return registeredDateTime; }
            set { registeredDateTime = value; }
        }

        

        public string MacAddress
        {
            get { return macAddress; }
            set { macAddress = value; }
        }

        #endregion


        #region Constructer 

        // Default constructor
        public Respondent()
        {
            this.RegisteredDateTime = DateTime.Now;
        }

        // Constructor with parameters
        public Respondent(int id, DateTime registeredDateTime,  string macAddress)
        {
            this.id = id;
            this.registeredDateTime = registeredDateTime;
            
            this.macAddress = macAddress;
        }

        // Copy constructor
        public Respondent(Respondent other)
        {
            this.id = other.Id;
            this.registeredDateTime = other.RegisteredDateTime;
            
            this.macAddress = other.MacAddress;
        }
        #endregion
    }

}