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
                nameDets = input.Trim().Split(' ');
                name = new Name((nameDets.Length >= 1) ? nameDets[0] : "", (nameDets.Length >= 3) ? nameDets[1] : "", (nameDets.Length >= 3) ? nameDets[2] : (nameDets.Length >= 2) ? nameDets[1] : "");

                Console.WriteLine("Welcome " + name.FirstName);
            }

            while (running)
            {
                Console.Write(">");
                input = Console.ReadLine();
                input = input.Trim().ToLower();

                if (input == "who am i")
                {
                    if (name.FirstName != "")
                    {
                        Console.WriteLine(name.FullName);
                    }
                    else
                    {
                        Console.WriteLine("You did not give us your name would you like to? Yes or No");
                        Console.Write(">");
                        input = Console.ReadLine();
                        input = input.Trim().ToLower();

                        if (input == "yes")
                        {
                            Console.WriteLine("What is your name");
                            Console.Write(">");
                            input = Console.ReadLine();

                            nameDets = input.Trim().Split(' ');
                            name = new Name((nameDets.Length >= 1) ? nameDets[0] : "", (nameDets.Length >= 3) ? nameDets[1] : "", (nameDets.Length >= 3) ? nameDets[2] : (nameDets.Length >= 2) ? nameDets[1] : "");
                            Console.WriteLine("Welcome " + name.FirstName);
                        }
                        else
                        {
                            Console.WriteLine("Uh, alright then");
                        }
                    }
                }
                else if (input == "quit")
                {
                    running = false;
                }
            }
        }
    }

    class Name
    {
        public string FirstName;
        public string MiddleName;
        public string LastName;

        public string FullName
        {
            get
            {
                return FirstName + ((MiddleName != "") ? " " + MiddleName.ToUpper()[0] + "." : "") + ((LastName != "") ? " " + LastName : "");
            }
        }

        public Name(string firstName, string middleName, string lastName)
        {
            FirstName = firstName;
            MiddleName = middleName;
            LastName = lastName;
        }
    }
}
