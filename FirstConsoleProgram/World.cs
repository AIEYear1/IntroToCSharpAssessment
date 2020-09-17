using System;
using System.Collections;
using System.Collections.Generic;
using static RaylibWindowNamespace.Objects;

namespace CRPGNamespace
{
    static class World
    {
        #region Object Lists
        public static readonly List<Item> Items = new List<Item>();
        public static readonly List<NPC> NPCs = new List<NPC>();
        public static readonly List<Monster> Monsters = new List<Monster>();
        public static readonly List<Location> Locations = new List<Location>();
        public static readonly List<Quest> Quests = new List<Quest>();
        #endregion

        #region Object IDs
        public const int ITEM_ID_STICK = 1<<0;
        public const int ITEM_ID_CLOTHES = 1<<1;
        public const int ITEM_ID_FANG = 1<<2;
        public const int ITEM_ID_MERMAIDSPEAR = 1<<3;
        public const int ITEM_ID_BANDITGARB = 1<<4;
        public const int ITEM_ID_LESSERHEALINGPOTION = 1<<5;
        public const int ITEM_ID_HEALINGPOTION = 1<<6;

        public const int NPC_ID_STEVE = 0;
        public const int NPC_ID_KVORKYSINN = 1;
        public const int NPC_ID_KVORKYSSHOP = 2;
        public const int NPC_ID_KVORKYSFOLKS = 3;

        public const int MONSTER_ID_WOLF = 0;
        public const int MONSTER_ID_MERMAID = 1;
        public const int MONSTER_ID_LOOTER = 2;
        public const int MONSTER_ID_TROLL = 3;

        public const int LOCATION_ID_CLEARING = 1<<0;
        public const int LOCATION_ID_PATH = 1<<1;
        public const int LOCATION_ID_BUSHES = 1<<2;
        public const int LOCATION_ID_LAKE = 1<<3;
        public const int LOCATION_ID_FORESTEDGE = 1<<4;
        public const int LOCATION_ID_PAVEDROAD = 1<<5;
        public const int LOCATION_ID_TOWNENTRANCE = 1<<6;
        public const int LOCATION_ID_TOWNSHOP = 1<<7;
        public const int LOCATION_ID_TOWNINN = 1<<8;
        public const int LOCATION_ID_TOWNSQUARE = 1<<9;
        public const int LOCATION_ID_ROADNORTH = 1<<10;

        public const int QUEST_ID_TUTORIALQUEST = 1<<0;
        public const int QUEST_ID_MAINQUEST = 1<<1;
        #endregion

        static World()
        {
            PopulateItems();
            PopulateQuests();
            PopulateNPCs();
            PopulateMonsters();
            PopulateLocations();
        }
        public static void Reload()
        {
            PopulateItems();
            PopulateQuests();
            PopulateNPCs();
            PopulateMonsters();
            PopulateLocations();
        }

        #region Population
        private static void PopulateItems()
        {
            Items.Clear();
            //Parameters: Name,Plural Name,Description,Weight(, Quest it's connected to, Objective Marker it's related to) < Only for Quest Items
            //Weapons: Parameters, Weapon Attack
            //Armor: Parameters, AC
            Items.Add(new Weapon(ITEM_ID_STICK, "Stick", "Sticks", "Long narrow stick with a stick like texture", 3, stickAttack));
            Items.Add(new Armor(ITEM_ID_CLOTHES, "Clothes", "Clothes", "Some pretty normal clothes, without these you'd be naked!", 5, 2));
            Items.Add(new Item(ITEM_ID_FANG, "Wolf Fang", "Wolf Fangs", "The fang of a wolf, pretty useless", 1));
            Items.Add(new Weapon(ITEM_ID_MERMAIDSPEAR, "Mermaid Spear", "Mermaid Spears", "A simple yet elegant spear", 7, mermaidSpearAttack));
            Items.Add(new Armor(ITEM_ID_BANDITGARB, "Bandit Garb", "Bandit Garb", "A simple set of thrown together armor", 10, 5));
            Items.Add(new HealingPotion(ITEM_ID_LESSERHEALINGPOTION, "Lesser Healing Potion", "Lesser Healing Potions", "A cheap healing potion", 25, 4));
            Items.Add(new HealingPotion(ITEM_ID_HEALINGPOTION, "Healing Potion", "Healing Potions", "A decent healing potion", 50, 10));
        }

        private static void PopulateNPCs()
        {
            NPCs.Clear();
            //Parameters: Name, Talk Line, Description
            //QueryNPC: Parameters
            List<InventoryItem> shopStock = new List<InventoryItem>();
            NPCs.Add(new NPC(new Name("Steve"), "'ello, welcome to Kvorkys", "The entrance guard to Kvorkys", false, true));
            NPCs.Add(new Inn(new Name("Mileena"), "'ello", "The town inn", "Would you like to rent a room for the night?", 10, false, true));
            #region Kvorkys Stock
            shopStock.Add(new InventoryItem(ItemByID(ITEM_ID_LESSERHEALINGPOTION), 5));
            shopStock.Add(new InventoryItem(ItemByID(ITEM_ID_HEALINGPOTION), 5));
            #endregion
            NPCs.Add(new Shop(new Name("Markus"), "'ello", "the town shop", "Buyin' or sellin'?", shopStock, 1.02f, false, true));
            shopStock.Clear();
            NPCs.Add(new QuestNPC(new Name("Townsfolk"), "'ello", "was there anything else you needed?", "Townsfolk of Kvorkys, hangin' out in the town square", QuestByID(QUEST_ID_TUTORIALQUEST), 2, true));
        }

        private static void PopulateMonsters()
        {
            Monsters.Clear();
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
            Locations.Clear();
            //Parameters: Name, Description(, Quest it's connected to, Objective Marker it's related to) < Only for Quest Locations
            QuestLocation clearing = new QuestLocation(LOCATION_ID_CLEARING, "Clearing", "A small clearing, forest surrounds you", QuestByID(QUEST_ID_TUTORIALQUEST), -1)
            {
                monsterLivingHere = MonsterByID(MONSTER_ID_WOLF)
            };
            Location path = new Location(LOCATION_ID_PATH, "Path", "A small path from the clearing, where does it lead?");
            Location bushes = new Location(LOCATION_ID_BUSHES, "Rustling Bushes", "Some rustling bushes", false, true)
            {
                monsterLivingHere = MonsterByID(MONSTER_ID_LOOTER)
            };
            Location smallLake = new Location(LOCATION_ID_LAKE, "Lake", "A small lake, seems peaceful")
            {
                monsterLivingHere = MonsterByID(MONSTER_ID_MERMAID)
            };
            LockedLocation forestEdge = new LockedLocation(LOCATION_ID_FORESTEDGE, "Forest Edge", "You can see the forest give way to what looks like grasslands", LockedLocationIndex.FORESTEDGE, "Something tells you, you need to be stronger before you can move on");
            QuestLocation pavedRoad = new QuestLocation(LOCATION_ID_PAVEDROAD, "Paved Road", "A paved road the first sign of civilization", QuestByID(QUEST_ID_TUTORIALQUEST), 0)
            {
                monsterLivingHere = MonsterByID(MONSTER_ID_TROLL)
            };
            Location townEntrance = new Location(LOCATION_ID_TOWNENTRANCE, "Town Entrance", "Entrance to the town of Kvorkys")
            {
                npcLivingHere = NPCByID(NPC_ID_STEVE)
            };
            Location townShop = new Location(LOCATION_ID_TOWNSHOP, "Town Shop", "Town's gneral store")
            {
                npcLivingHere = NPCByID(NPC_ID_KVORKYSSHOP)
            };
            Location townInn = new Location(LOCATION_ID_TOWNINN, "Town Inn", "Town's Inn")
            {
                npcLivingHere = NPCByID(NPC_ID_KVORKYSINN)
            };
            Location townSquare = new Location(LOCATION_ID_TOWNSQUARE, "Town Square", "The Town Square, where a lot of people spend their time")
            {
                npcLivingHere = NPCByID(NPC_ID_KVORKYSFOLKS)
            };
            LockedLocation roadNorth = new LockedLocation(LOCATION_ID_ROADNORTH, "Road North", "A road leading north", LockedLocationIndex.NORTH, "You should head to the town first");


            #region Intro Forest
            clearing.locationToNorth = path;

            path.locationToNorth = forestEdge;
            path.locationToEast = bushes;
            path.locationToWest = smallLake;
            path.locationToSouth = clearing;

            bushes.locationToWest = path;

            smallLake.locationToEast = path;

            forestEdge.locationToSouth = path;
            forestEdge.locationToNorth = pavedRoad;
            #endregion

            pavedRoad.locationToNorth = roadNorth;
            pavedRoad.locationToEast = townEntrance;

            #region Kvorkys Town
            townEntrance.locationToWest = pavedRoad;
            townEntrance.locationToNorth = townShop;
            townEntrance.locationToSouth = townInn;
            townEntrance.locationToEast = townSquare;

            townShop.locationToSouth = townEntrance;

            townInn.locationToNorth = townEntrance;

            townSquare.locationToWest = townEntrance;
            #endregion


            Locations.Add(clearing);
            Locations.Add(path);
            Locations.Add(bushes);
            Locations.Add(smallLake);
            Locations.Add(forestEdge);
            Locations.Add(pavedRoad);
            Locations.Add(townEntrance);
            Locations.Add(townShop);
            Locations.Add(townInn);
            Locations.Add(townSquare);
            Locations.Add(roadNorth);
        }

        public static void PopulateQuests()
        {
            Quests.Clear();
            //Parameters: Name, Description, Objectives, Reward Gold, Reward XP, Text played when quest gained, Text played when quest completed, quest following current quest-presumed null, Whther quest is main quest-presumed false, Quest complete-presumed false
            List<(string, string)> objectives = new List<(string, string)>();

            #region tutorial objectives
            objectives.Add(("Figure out where you are", "As you walk along the road you see a city being attacked by a troll!\nDefeat the troll and protect the villagers!"));
            objectives.Add(("Defeat the troll", "With the troll defeated the town is now safe, perhaps there you can find answers there"));
            objectives.Add(("Talk with the townsfolk", "You're lost? Oh well this is Kvorkys a small town in the southern portion of Bjork.\nYou may be able to find out more at the capital, if you head out of the town you should see a road north,\nthat'll lead you to the capital"));
            #endregion
            Quest tutorialQuest = new Quest(QUEST_ID_TUTORIALQUEST, "Lost in the Forest!", "You've no idea where you are, you should start looking around for anything familiar", objectives, 7, 20, "You wake up in a clearing, where is this place? You should figure out where you are.", "well, looks like it's time to adventure");
            objectives.Clear();

            #region Main Objectives
            objectives.Add(("Make your way to the capital", ""));
            #endregion
            Quest mainQuest = new Quest(QUEST_ID_MAINQUEST, "To the Capital!", "In order to figure out more about where you are and where you're going you need to head to the capital", objectives, 25, 56, "Alright, now you have a location, head out on a journey to reach the capital", "", true);

            tutorialQuest.followUpQuest = mainQuest;

            Quests.Add(tutorialQuest);
            Quests.Add(mainQuest);
        }
        #endregion


        #region GetByID
        public static Item ItemByID(int ID)
        {
            for(int x = 0; x < Items.Count; x++)
            {
                if(Items[x].ID == ID)
                {
                    return Items[x];
                }
            }
            Utils.Add("Invalid Item ID");
            return null;
        }

        public static NPC NPCByID(int ID)
        {
            if (ID >= NPCs.Count)
            {
                Utils.Add("Invalid NPC ID");
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
            for (int x = 0; x < Locations.Count; x++)
            {
                if (Locations[x].ID == ID)
                {
                    return Locations[x];
                }
            }
            Utils.Add("Invalid Location ID");
            return null;
        }

        public static Quest QuestByID(int ID)
        {
            for (int x = 0; x < Quests.Count; x++)
            {
                if (Quests[x].ID == ID)
                {
                    return Quests[x];
                }
            }
            Utils.Add("Invalid Quest ID");
            return null;
        }
        #endregion


        #region Help
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
            Utils.Add("\thelp color: explains the text color scheme");
        }
        public static void HelpColor()
        {
            Utils.Add("Color Reference:");
            Utils.Add("\tWhite: normal text");
            Utils.Add("\t" + Utils.ColorText("Red", TextColor.RED) + ": Monster");
            Utils.Add("\t" + Utils.ColorText("Magenta", TextColor.MAGENTA) + ": quest objective");
            Utils.Add("\t" + Utils.ColorText("Green", TextColor.GREEN) + ": XP");
            Utils.Add("\t" + Utils.ColorText("Yellow", TextColor.YELLOW) + ": Gold");
            Utils.Add("\t" + Utils.ColorText("Blue", TextColor.BLUE) + ": Damage");
            Utils.Add("\t" + Utils.ColorText("Salmon", TextColor.SALMON) + ": Weapon");
            Utils.Add("\t" + Utils.ColorText("Light Blue", TextColor.LIGHTBLUE) + ": Armor");
            Utils.Add("\t" + Utils.ColorText("Gold", TextColor.GOLD) + ": General Item");
            Utils.Add("\t" + Utils.ColorText("Dark Red", TextColor.DARKRED) + ": player death");
            Utils.Add("\t" + Utils.ColorText("Pink", TextColor.PINK) + ": Consumable");
            Utils.Add("\t" + Utils.ColorText("Sea Green", TextColor.SEAGREEN) + ": NPC");
        }
        #endregion
    }
}
