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
        public static bool running = true;
        public static Player player = new Player(null, 0, 0, 1, new Weapon(5, 1, "Stick", "Sticks", "Long narrow stick with a stick like texture", 1), null, 15);

        static void Main(string[] args)
        {
            Monster wolf = new Monster(new Name("Wolf"), 3, 0, 5, 12, 12);

            Console.WriteLine("What is your name");
            Console.Write(">");
            string input = Console.ReadLine();
            string[] nameDets;

            if (input.Trim().ToLower() == "quit")
                running = false;
            else
            {
                nameDets = input.Trim().ToLower().Split(' ');

                player.name = new Name((nameDets.Length >= 1) ? nameDets[0] : "", (nameDets.Length >= 3) ? nameDets[1] : "", (nameDets.Length >= 3) ? nameDets[2] : (nameDets.Length >= 2) ? nameDets[1] : "");

                Console.WriteLine("Welcome " + player.name.FirstName);
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
                        if (player.name.FirstName != "")
                        {
                            Console.WriteLine(player.name.FullName);
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
                            player.name = new Name((nameDets.Length >= 1) ? nameDets[0] : "", (nameDets.Length >= 3) ? nameDets[1] : "", (nameDets.Length >= 3) ? nameDets[2] : (nameDets.Length >= 2) ? nameDets[1] : "");

                            Console.WriteLine("Welcome " + player.name.FirstName);
                        }
                        else
                        {
                            Console.WriteLine("Uh, alright then");
                        }
                        break;
                    case "attack":
                        player.Attack(wolf);
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
}