using Raylib_cs;
using System.ComponentModel.Design;
using System.Numerics;
using static Raylib_cs.Color;
using static Raylib_cs.Raylib;

namespace raygamecsharp
{
    public struct Objects
    {
        public static Texture2D playerTexture;
        public static Texture2D enemyTexture;


        public static Character player;
        public static AI monster;

        public static WeaponAttack stickAttack = new WeaponAttack(1, 4, "", WeaponAttackIndex.STICKATTACK);
        public static WeaponAttack mermaidSpearAttack = new WeaponAttack(2, 6, "", WeaponAttackIndex.MERMAIDSPEARATTACK);

        public static EnemyAttack WolfAttack = new EnemyAttack(3, 7, "", EnemyAttackIndex.WOLFATTACK);
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
    }
}
