using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using raygamecsharp;
using static raygamecsharp.Objects;

namespace CRPGThing
{
    class World
    {
        #region Object Lists
        public static readonly List<Item> Items = new List<Item>();
        public static readonly List<NPC> NPCs = new List<NPC>();
        public static readonly List<Monster> Monsters = new List<Monster>();
        public static readonly List<Location> Locations = new List<Location>();
        public static readonly List<Quest> Quests = new List<Quest>();
        #endregion

        #region Object IDs
        public const int ITEM_ID_STICK = 0;
        public const int ITEM_ID_CLOTHES = 1;
        public const int ITEM_ID_FANG = 2;
        public const int ITEM_ID_MERMAIDSPEAR = 3;
        public const int ITEM_ID_BANDITGARB = 4;

        public const int NPC_ID_STEVESHOP = 0;

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
        #endregion

        static World()
        {
            PopulateItems();
            PopulateNPCs();
            PopulateQuests();
            PopulateMonsters();
            PopulateLocations();
        }


        #region Population
        private static void PopulateItems()
        {
            //Parameters: Name,Plural Name,Description,Weight(, Quest it's connected to, Objective Marker it's related to) < Only for Quest Items
            //Weapons: Max Damage Min Damage, Parameters
            //Armor: AC, Parameters
            Items.Add(new Weapon("Stick", "Sticks", "Long narrow stick with a stick like texture", 3, stickAttack));
            Items.Add(new Armor("Clothes", "Clothes", "Some pretty normal clothes, without these you'd be naked!", 5, 2));
            Items.Add(new Item("Wolf Fang", "Wolf Fangs", "The fang of a wolf, pretty useless", 1));
            Items.Add(new Weapon("Mermaid Spear", "Mermaid Spears", "A simple yet elegant spear", 7, mermaidSpearAttack));
            Items.Add(new Armor("Bandit Garb", "Bandit Garb", "A simple set of thrown together armor", 10, 5));

            //string[] lines = File.ReadAllLines("items.csv");

            //for (int x = 1; x < lines.Length; x++)
            //{
            //    string[] lineVals = lines[x].Split(',');

            //    switch (lineVals[0])
            //    {
            //        case "W":
            //            Weapon tmpWeapon = new Weapon(lineVals[1], lineVals[2], lineVals[3]);
            //            int.TryParse(lineVals[4], out tmpWeapon.value);
            //            int.TryParse(lineVals[5], out tmpWeapon.maxDamage);
            //            int.TryParse(lineVals[6], out tmpWeapon.minDamage);
            //            Items.Add(tmpWeapon);
            //            break;
            //        case "A":
            //            Armor tmpArmor = new Armor(lineVals[1], lineVals[2], lineVals[3]);
            //            int.TryParse(lineVals[4], out tmpArmor.value);
            //            int.TryParse(lineVals[7], out tmpArmor.ac);
            //            Items.Add(tmpArmor);
            //            break;
            //        default:
            //            Item tmpItem = new Item(lineVals[1], lineVals[2], lineVals[3]);
            //            int.TryParse(lineVals[4], out tmpItem.value);
            //            Items.Add(tmpItem);
            //            break;
            //    }
            //}
        }

        private static void PopulateNPCs()
        {
            List<InventoryItem> shopStock = new List<InventoryItem>();
            foreach (Item item in Items)
            {
                shopStock.Add(item);
            }
            shopStock.Add(ItemByID(ITEM_ID_STICK));
            shopStock.Add(ItemByID(ITEM_ID_STICK));
            shopStock.Add(ItemByID(ITEM_ID_STICK));
            shopStock.Add(ItemByID(ITEM_ID_STICK));
            NPCs.Add(new Shop(new Name("Steve"), "'ello", shopStock, 2));
        }

        private static void PopulateMonsters()
        {
            //Parameters: Name, Description, Health, Max Damage, Min Damage, Reward XP, Reward Gold(, Quest it's connected to, Objective Marker it's related to) < Only for Quest Monsters
            Monster wolf = new Monster(new Name("Wolf"), "A lone wolf prowling", 12, WolfAttack, 11, 5);
            wolf.lootTable.Add(new LootItem(ItemByID(ITEM_ID_FANG), 100, true));
            Monster mermaid = new Monster(new Name("Mermaid"), "a mermaid sitting on the edge of the lake", 15, mermaidAttack, 23, 3);
            mermaid.lootTable.Add(new LootItem(ItemByID(ITEM_ID_MERMAIDSPEAR), 100, true));
            Monster looter = new Monster(new Name("Looter"), "A looter hiding in the bushes", 20, looterAttack, 18, 7);
            looter.lootTable.Add(new LootItem(ItemByID(ITEM_ID_BANDITGARB), 100, true));
            QuestMonster troll = new QuestMonster(new Name("Troll"), "A troll trying to attack the town", 30, trollAttack, 32, 10, QuestByID(QUEST_ID_TUTORIALQUEST), 1);

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
        #endregion


        #region GetByID
        public static Item ItemByID(int ID)
        {
            if (ID >= Items.Count)
            {
                Utils.Add("Invalid Item ID");
                return null;
            }

            return Items[ID];
        }

        public static NPC NPCByID(int ID)
        {
            if (ID >= NPCs.Count)
            {
                Utils.Add("Invalid Item ID");
                return null;
            }

            return NPCs[ID];
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
        #endregion

        public static void Help()
        {
            Utils.Add("Possible Commands: ");
            Utils.Add("\twho am i: tells you your name");
            Utils.Add("\tlook: lets you look at things\n\tSubCommands: \n\t\tNorth; East; South; West: lets you look at locations in that direction" +
                "\n\t\there: lets you look here and tells you of everything in this area\n\t\tItem Names: lets you look at the specified item" +
                "\n\t\tMonster Names: lets you look at the specified monster\n\t\tQuest Names: lets you look at the specified quest");
            Utils.Add("\tstats: shows you your current stats");
            Utils.Add("\tinventory: shows you all the items in your inventory");
            Utils.Add("\tquests: shows you your current quests");
            Utils.Add("\tmove: lets you move in a specified direction");
            Utils.Add("\tequip: lets you equip a specified item");
            Utils.Add("\tattack: attacks the current monster in the area");
            Utils.Add("\tquit: quits the game");
        }
    }
}
