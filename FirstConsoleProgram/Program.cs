using System;

namespace FirstConsoleProgram
{
    class Program
    {
        //(string firstName, string middleName, string lastName) Name 
        //{
        //    get
        //    {
        //        return Name.firstName + " " + Name.middleName.ToUpper()[0] + " " + Name.lastName;
        //    }
        //}

        static void Main(string[] args)
        {
            bool running = true;

            Console.WriteLine("What is your name");
            Console.Write(">");
            string input = Console.ReadLine();
            string[] nameDets;
            Name name = null;

            if (input.Trim().ToLower() == "quit")
                running = false;
            else
            {
                nameDets = input.Trim().ToLower().Split(' ');
                name = new Name((nameDets.Length >= 1) ? nameDets[0] : "", (nameDets.Length >= 3) ? nameDets[1] : "", (nameDets.Length >= 3) ? nameDets[2] : (nameDets.Length >= 2) ? nameDets[1] : "");

                Console.WriteLine("Welcome " + name.FirstName);
            }

            //Loop Start
            while (running)
            {
                Console.Write(">");               //Get player Input
                input = Console.ReadLine();       //Get player Input
                input = input.Trim().ToLower();   //Get player Input


                switch (input)
                {
                    case "who am i":                //1st case "who am i"
                        if (name.FirstName != "")
                        {
                            Console.WriteLine(name.FullName);
                            continue;
                        }

                        Console.WriteLine("You did not give us your name would you like to? Yes or No");
                        Console.Write(">");
                        input = Console.ReadLine();
                        input = input.Trim().ToLower();

                        if (input == "yes")
                        {
                            Console.WriteLine("What is your name");
                            Console.Write(">");
                            input = Console.ReadLine();

                            if (input.Trim().ToLower() == "quit")
                            {
                                running = false;
                                break;
                            }

                            nameDets = input.Trim().ToLower().Split(' ');
                            name = new Name((nameDets.Length >= 1) ? nameDets[0] : "", (nameDets.Length >= 3) ? nameDets[1] : "", (nameDets.Length >= 3) ? nameDets[2] : (nameDets.Length >= 2) ? nameDets[1] : "");

                            Console.WriteLine("Welcome " + name.FirstName);
                        }
                        else
                        {
                            Console.WriteLine("Uh, alright then");
                        }
                        break;
                    case "quit":                    //2nd case "quit"
                        running = false;
                        break;
                    default:                        //Overflow
                        Console.WriteLine("I- I don- I don't understand");
                        break;
                }
            }
        }
    }

    class Name
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

        public Name(string firstName, string middleName, string lastName)
        {
            if (firstName != "")
                FirstName = firstName[0].ToString().ToUpper() + firstName.Remove(0, 1);
            else
                FirstName = "";

            if (middleName != "")
                MiddleName = middleName[0].ToString().ToUpper() + middleName.Remove(0, 1);
            else
                MiddleName = "";

            if(lastName != "")
                LastName = lastName[0].ToString().ToUpper() + lastName.Remove(0, 1);
            else
                LastName = "";
        }
    }
}