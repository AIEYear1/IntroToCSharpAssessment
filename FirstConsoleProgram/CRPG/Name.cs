namespace CRPGNamespace
{
    /// <summary>
    /// Struct which contains a first middle and last name
    /// </summary>
    public struct Name
    {
        /// <summary>
        /// Player's first name
        /// </summary>
        public string FirstName;
        /// <summary>
        /// Player's middle name
        /// </summary>
        public string MiddleName;
        /// <summary>
        /// Player's last name
        /// </summary>
        public string LastName;

        /// <summary>
        /// Player's full name with middle initial
        /// </summary>
        public string FullName
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
