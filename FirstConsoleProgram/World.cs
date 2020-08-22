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

        public const int QUEST_ID_TUTORIALQUEST = 0;

        static World()
        {
            PopulateItems();
            PopulateQuests();
            PopulateMonsters();
            PopulateLocations();
        }



        private static void PopulateItems()
        {
            //Name, Plural Name, Description, Weight(, Quest it's connected to, Objective Marker it's related to) < Only for Quest Items
            Items.Add(new Weapon(5, 1, "Stick", "Sticks", "Long narrow stick with a stick like texture", 1));
            Items.Add(new Armor(2, "Clothes", "Clothes", "Some pretty normal clothes, without these you'd be naked!", 3));
            Items.Add(new Item("Wolf Fang", "Wolf Fangs", "The fang of a wolf, pretty useless", 3));
        }

        private static void PopulateMonsters()
        {
            //Name, Description, Health, Max Damage, Min Damage, Reward XP, Reward Gold(, Quest it's connected to, Objective Marker it's related to) < Only for Quest Monsters
            Monster wolf = new Monster(new Name("Wolf"), "A lone wolf prowling", 12, 7, 3, 10, 5);
            wolf.lootTable.Add(new LootItem(ItemByID(ITEM_ID_FANG), 100, true));
            Monsters.Add(wolf);
        }

        private static void PopulateLocations()
        {
            //Name, Description(, Quest it's connected to, Objective Marker it's related to) < Only for Quest Locations
            QuestLocation clearing = new QuestLocation("Clearing", "A small clearing, forest surrounds you", QuestByID(QUEST_ID_TUTORIALQUEST), -1);
            clearing.monsterLivingHere = World.MonsterByID(World.MONSTER_ID_WOLF);

            Location path1 = new Location("Path", "A small path from the clearing, where does it lead?");

            clearing.locationToNorth = path1;

            path1.locationToSouth = clearing;

            Locations.Add(clearing);
            Locations.Add(path1);
        }

        public static void PopulateQuests()
        {
            List<string> objectives = new List<string>();

            #region tutorial objectives
            objectives.Add("Figure out where you are");
            #endregion
            Quest tutorialQuest = new Quest("Lost in the Forest!", "You've no idea where you are, you should start looking around for anything familiar", objectives, 7, 20, "You wake up in a clearing, where is this place? You should figure out where you are.", "well, looks like it's time to adventure");
            objectives.Clear();

            Quests.Add(tutorialQuest);
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
