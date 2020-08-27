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
        public const int ITEM_ID_MERMAIDSPEAR = 3;
        public const int ITEM_ID_BANDITGARB = 4;

        public const int MONSTER_ID_WOLF = 0;
        public const int MONSTER_ID_MERMAID = 1;
        public const int MONSTER_ID_LOOTER = 2;
        public const int MONSTER_ID_TROLL = 3;

        public const int LOCATION_ID_CLEARING = 0;
        public const int LOCATION_ID_PATH = 1;
        public const int LOCATION_ID_BUSHES = 2;
        public const int LOCATION_ID_LAKE = 3;
        public const int LOCATION_ID_FORESTEDGE = 4;
        public const int LOCATION_ID_PAVEDROAD = 5;

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
            //Parameters: Name, Plural Name, Description, Weight(, Quest it's connected to, Objective Marker it's related to) < Only for Quest Items
            //Weapons: Max Damage Min Damage, Parameters
            //Armor: AC, Parameters
            Items.Add(new Weapon(4, 1, "Stick", "Sticks", "Long narrow stick with a stick like texture", 3));
            Items.Add(new Armor(2, "Clothes", "Clothes", "Some pretty normal clothes, without these you'd be naked!", 5));
            Items.Add(new Item("Wolf Fang", "Wolf Fangs", "The fang of a wolf, pretty useless", 1));
            Items.Add(new Weapon(10, 3, "Mermaid Spear", "Mermaid Spears", "A simple yet elegant spear", 7));
            Items.Add(new Armor(5, "Bandit Garb", "Bandit Garb", "A simple set of thrown together armor", 10));
        }

        private static void PopulateMonsters()
        {
            //Parameters: Name, Description, Health, Max Damage, Min Damage, Reward XP, Reward Gold(, Quest it's connected to, Objective Marker it's related to) < Only for Quest Monsters
            Monster wolf = new Monster(new Name("Wolf"), "A lone wolf prowling", 12, 7, 3, 11, 5);
            wolf.lootTable.Add(new LootItem(ItemByID(ITEM_ID_FANG), 100, true));
            Monster mermaid = new Monster(new Name("Mermaid"), "a mermaid sitting on the edge of the lake", 15, 10, 3, 23, 3);
            mermaid.lootTable.Add(new LootItem(ItemByID(ITEM_ID_MERMAIDSPEAR), 100, true));
            Monster looter = new Monster(new Name("Looter"), "A looter hiding in the bushes", 20, 5, 1, 18, 7);
            looter.lootTable.Add(new LootItem(ItemByID(ITEM_ID_BANDITGARB), 100, true));
            QuestMonster troll = new QuestMonster(new Name("Troll"), "A troll trying to attack the town", 30, 19, 9, 32, 10, QuestByID(QUEST_ID_TUTORIALQUEST), 1);

            Monsters.Add(wolf);
            Monsters.Add(mermaid);
            Monsters.Add(looter);
            Monsters.Add(troll);
        }

        private static void PopulateLocations()
        {
            //Parameters: Name, Description(, Quest it's connected to, Objective Marker it's related to) < Only for Quest Locations
            QuestLocation clearing = new QuestLocation("Clearing", "A small clearing, forest surrounds you", QuestByID(QUEST_ID_TUTORIALQUEST), -1);
            clearing.monsterLivingHere = World.MonsterByID(World.MONSTER_ID_WOLF);
            Location path = new Location("Path", "A small path from the clearing, where does it lead?");
            Location bushes = new Location("Rustling Bushes", "Some rustling bushes", false, true);
            bushes.monsterLivingHere = MonsterByID(MONSTER_ID_LOOTER);
            Location smallLake = new Location("Lake", "a small lake, seems peaceful");
            smallLake.monsterLivingHere = MonsterByID(MONSTER_ID_MERMAID);
            Location forestEdge = new Location("Forest Edge", "you can see the forest give way to what looks like grasslands", true);
            QuestLocation pavedRoad = new QuestLocation("Paved Road", "A paved road the first sign of civilization", QuestByID(QUEST_ID_TUTORIALQUEST), 0);
            pavedRoad.monsterLivingHere = MonsterByID(MONSTER_ID_TROLL);


            clearing.locationToNorth = path;

            path.locationToNorth = forestEdge;
            path.locationToEast = bushes;
            path.locationToWest = smallLake;
            path.locationToSouth = clearing;

            bushes.locationToWest = path;

            smallLake.locationToEast = path;

            forestEdge.locationToSouth = path;
            forestEdge.locationToNorth = pavedRoad;


            Locations.Add(clearing);
            Locations.Add(path);
            Locations.Add(bushes);
            Locations.Add(smallLake);
            Locations.Add(forestEdge);
            Locations.Add(pavedRoad);
        }

        public static void PopulateQuests()
        {
            //Parameters: Name, Description, Objectives, Reward Gold, Reward XP, Text played when quest gained, Text played when quest completed, quest following current quest-presumed null, Whther quest is main quest-presumed false, Quest complete-presumed false
            List<(string, string)> objectives = new List<(string, string)>();

            #region tutorial objectives
            objectives.Add(("Figure out where you are", "As you walk along the road you see a city being attacked by a troll!\nDefeat the troll and protect the villagers!"));
            objectives.Add(("Defeat the troll", "With the troll defeated the town is now safe, perhaps there you can find answers there"));
            objectives.Add(("Talk with the townsfolk", ""));
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
