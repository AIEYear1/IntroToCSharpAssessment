using Raylib_cs;
using System.ComponentModel.Design;
using System.Numerics;
using static Raylib_cs.Color;
using static Raylib_cs.Raylib;

namespace RaylibWindowNamespace
{
    public struct Objects
    {
        public static Texture2D playerTexture;
        public static Texture2D enemyTexture;


        public static Character player;
        public static AI monster;

        public static WeaponAttack stickAttack = new WeaponAttack(3, 7, "", WeaponAttackIndex.STICKATTACK);
        public static WeaponAttack mermaidSpearAttack = new WeaponAttack(2, 6, "", WeaponAttackIndex.MERMAIDSPEARATTACK);

        public static EnemyAttack WolfAttack = new EnemyAttack(1, 4, "", EnemyAttackIndex.WOLFATTACK);
        public static EnemyAttack looterAttack = new EnemyAttack(1, 5, "", EnemyAttackIndex.LOOTERATTACK);
        public static EnemyAttack mermaidAttack = new EnemyAttack(3, 10, "", EnemyAttackIndex.MERMAIDATTACK);
        public static EnemyAttack trollAttack = new EnemyAttack(9, 19, "", EnemyAttackIndex.TROLLATTACK);

        public static void InitializeCharacters()
        {
            playerTexture = LoadTexture(@"Pictures\Template.png");
            enemyTexture = LoadTexture(@"Pictures\Rogue.png");

            player = new Character(playerTexture, new Vector2(Window.screenWidth / 2, Window.screenHeight / 2), WHITE, 16, Vector2.One * 4, 20);
            monster = new AI(enemyTexture, new Vector2(Window.screenWidth / 2, Window.screenHeight / 2), WHITE, 16, Vector2.One * 4, 20);
        }

        public static RectangleSprite healthBar = new RectangleSprite(Vector2.One * borderThickness, Window.screenWidth - borderThickness * 2, 20, GREEN);
        public static RectangleSprite healthBackground = new RectangleSprite(Vector2.One * borderThickness, Window.screenWidth - borderThickness * 2, 20, RED);
        public static RectangleSprite healthBorder = new RectangleSprite(new Vector2(0, borderThickness+20), Window.screenWidth, borderThickness, DARKGRAY);

        public const float borderThickness = 5;
        public static RectangleSprite topBar = new RectangleSprite(Vector2.Zero, Window.screenWidth, borderThickness, DARKGRAY);
        public static RectangleSprite leftBar = new RectangleSprite(Vector2.Zero, borderThickness, Window.screenHeight, DARKGRAY);
        public static RectangleSprite bottomBar = new RectangleSprite(new Vector2(0, Window.screenHeight - borderThickness), Window.screenWidth, borderThickness, DARKGRAY);
        public static RectangleSprite rightBar = new RectangleSprite(new Vector2(Window.screenWidth - borderThickness, 0), borderThickness, Window.screenHeight, DARKGRAY);
    }
}
