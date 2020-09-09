using System;
using System.Collections.Generic;
using System.Text;

namespace CRPGNamespace
{
    public struct Name
    {
        public string FirstName;    //Player's first name
        public string MiddleName;   //Player's middle name
        public string LastName;     //Player's last name

        public string FullName      //Player's full name with middle initial
        {
            get
            {
                return FirstName + ((MiddleName != "") ? " " + MiddleName.ToUpper()[0] + "." : "") + ((LastName != "") ? " " + LastName : "");
            }
        }

        public Name(string firstName, string lastName = "", string middleName = "")
        {
            if (firstName != "")
                FirstName = firstName[0].ToString().ToUpper() + firstName.Remove(0, 1);
            else
                FirstName = "";

            if (middleName != "")
                MiddleName = middleName[0].ToString().ToUpper() + middleName.Remove(0, 1);
            else
                MiddleName = "";

            if (lastName != "")
                LastName = lastName[0].ToString().ToUpper() + lastName.Remove(0, 1);
            else
                LastName = "";
        }
    }
}
