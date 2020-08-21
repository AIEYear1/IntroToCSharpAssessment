using System;
using System.Collections.Generic;
using System.Text;

namespace CRPGThing
{
    class World
    {
        public static readonly List<Item> Items = new List<Item>();
        public static readonly List<Monster> Monsters = new List<Monster>();
        public static readonly List<Location> Locations = new List<Location>();
        public static readonly List<Quest> Quests = new List<Quest>();

        public const int ITEM_ID_STICK = 0;
        public const int ITEM_ID_CLOTHES = 1;
        public const int ITEM_ID_FANG = 2;

        public const int MONSTER_ID_WOLF = 0;

        public const int LOCATION_ID_STARTINGLOCATION = 0;

        static World()
        {
            PopulateItems();
            PopulateMonsters();
            PopulateLocations();
            PopulateQuests();
        }



        private static void PopulateItems()
        {
            Items.Add(new Weapon(5, 1, "Stick", "Sticks", "Long narrow stick with a stick like texture", 1));
            Items.Add(new Armor(2, "Clothes", "Clothes", "Some pretty normal clothes, without these you'd be naked!", 3));
            Items.Add(new Item("Wolf Fang", "Wolf Fangs", "The fang of a wolf, pretty useless", 3));
        }

        private static void PopulateMonsters()
        {
            Monster wolf = new Monster(new Name("Wolf"), "A lone wolf prowling", 7, 3, 5, 10, 12);
            wolf.lootTable.Add(new LootItem(ItemByID(ITEM_ID_FANG), 100, true));
            Monsters.Add(wolf);
        }

        private static void PopulateLocations()
        {
            Location startingLocation = new Location("Starting Loc, Will be changed Later", "No description as of now");
            startingLocation.monsterLivingHere = World.MonsterByID(World.MONSTER_ID_WOLF);

            Locations.Add(startingLocation);
        }

        public static void PopulateQuests()
        {

        }



        public static Item ItemByID(int ID)
        {
            if (ID >= Items.Count)
            {
                Utils.Add("Invalid Item ID");
                return null;
            }

            return Items[ID];
        }

        public static Monster MonsterByID(int ID)
        {
            if (ID >= Monsters.Count)
            {
                Utils.Add("Invalid Monster ID");
                return null;
            }

            return Monsters[ID];
        }

        public static Location LocationByID(int ID)
        {
            if (ID >= Locations.Count)
            {
                Utils.Add("Invalid Location ID");
                return null;
            }

            return Locations[ID];
        }

        public static Quest QuestByID(int ID)
        {
            if (ID >= Quests.Count)
            {
                Utils.Add("Invalid Location ID");
                return null;
            }

            return Quests[ID];
        }
    }
}
