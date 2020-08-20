using System;
using System.Collections.Generic;
using System.Linq;

namespace CRPGThing
{
    class Program
    {
        public static bool running = true;
        public static Player player = new Player(null, 0, 0, 1, (Weapon)World.ItemByID(World.ITEM_ID_STICK), 
            (Armor)World.ItemByID(World.ITEM_ID_CLOTHES), 15);

        static void Main(string[] args)
        {
            Console.WriteLine("What is your name");
            Console.Write(">");
            string input = Console.ReadLine();
            string[] nameDets = new string[3];

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
                        player.Attack(World.MonsterByID(0));
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