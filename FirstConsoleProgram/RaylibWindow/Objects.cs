using Raylib_cs;
using System.Numerics;
using static Raylib_cs.Color;
using static Raylib_cs.Raylib;

namespace RaylibWindowNamespace
{
    /// <summary>
    /// Stores Raylib related objects
    /// </summary>
    public class Objects
    {
        /// <summary>
        /// Textures for player and enemy
        /// </summary>
        public static Texture2D playerTexture, enemyTexture;

        /// <summary>
        /// Player Character
        /// </summary>
        public static Character player;
        /// <summary>
        /// Monster AI
        /// </summary>
        public static AI monster;

        //All of the Weapon Attack
        /// <summary>
        /// Attack for the Stick Weapon
        /// </summary>
        public static WeaponAttack stickAttack = new WeaponAttack(3, 7, "You chase the enemy around with 'wasd' and attempt to smack them", WeaponAttackIndex.STICKATTACK);
        /// <summary>
        /// Attack for the Mermaid Spear Attack
        /// </summary>
        public static WeaponAttack mermaidSpearAttack = new WeaponAttack(1, 4, "You hold down left mouse button and release in the direction you wish to throw your spear.\nYou have 15 spears", WeaponAttackIndex.MERMAIDSPEARATTACK);

        //All of the Enemy Attack
        /// <summary>
        /// Attack for the Wolf Enemy
        /// </summary>
        public static EnemyAttack WolfAttack = new EnemyAttack(2, 5, "Wolves appear out of nowhere and and attempt to pounce on you", EnemyAttackIndex.WOLFATTACK);
        /// <summary>
        /// Attack for the Looter Enemy
        /// </summary>
        public static EnemyAttack looterAttack = new EnemyAttack(4, 7, "Looter attempts to sneak up behind you hiding in your blind spots", EnemyAttackIndex.LOOTERATTACK);
        /// <summary>
        /// Attack for the Mermaid Enemy
        /// </summary>
        public static EnemyAttack mermaidAttack = new EnemyAttack(5, 10, "Mermaid throws spears at you", EnemyAttackIndex.MERMAIDATTACK);
        /// <summary>
        /// Attack for the Troll Attack
        /// </summary>
        public static EnemyAttack trollAttack = new EnemyAttack(9, 21, "Troll attacks a random area every few seconds", EnemyAttackIndex.TROLLATTACK);

        /// <summary>
        /// Loads the textures and assigns the player and monster instances, done because you can't LoadTexture cannot be called before InitWindow()
        /// </summary>
        public static void InitializeCharacters()
        {
            playerTexture = LoadTexture(@"Pictures\Template.png");
            enemyTexture = LoadTexture(@"Pictures\Rogue.png");

            player = new Character(playerTexture, new Vector2(Window.screenWidth / 2, Window.screenHeight / 2), WHITE, 16, Vector2.One * 4, 20);
            monster = new AI(enemyTexture, new Vector2(Window.screenWidth / 2, Window.screenHeight / 2), WHITE, 16, Vector2.One * 4, 20);
        }

        /// <summary>
        /// Thickness of the borders
        /// </summary>
        public const float borderThickness = 5;

        //Health UI
        public static RectangleSprite healthBar = new RectangleSprite(Vector2.One * borderThickness, Window.screenWidth - borderThickness * 2, 20, GREEN);
        public static RectangleSprite healthBackground = new RectangleSprite(Vector2.One * borderThickness, Window.screenWidth - borderThickness * 2, 20, RED);
        public static RectangleSprite healthBorder = new RectangleSprite(new Vector2(0, borderThickness + 20), Window.screenWidth, borderThickness, DARKGRAY);

        //Main borders
        public static RectangleSprite topBar = new RectangleSprite(Vector2.Zero, Window.screenWidth, borderThickness, DARKGRAY);
        public static RectangleSprite leftBar = new RectangleSprite(Vector2.Zero, borderThickness, Window.screenHeight, DARKGRAY);
        public static RectangleSprite bottomBar = new RectangleSprite(new Vector2(0, Window.screenHeight - borderThickness), Window.screenWidth, borderThickness, DARKGRAY);
        public static RectangleSprite rightBar = new RectangleSprite(new Vector2(Window.screenWidth - borderThickness, 0), borderThickness, Window.screenHeight, DARKGRAY);
    }
}
