/*******************************************************************************************
*
*   raylib [core] example - Basic window
*
*   Welcome to raylib!
*
*   To test examples, just press F6 and execute raylib_compile_execute script
*   Note that compiled executable is placed in the same folder as .c file
*
*   You can find all basic examples on C:\raylib\raylib\examples folder or
*   raylib official webpage: www.raylib.com
*
*   Enjoy using raylib. :)
*
*   This example has been created using raylib 1.0 (www.raylib.com)
*   raylib is licensed under an unmodified zlib/libpng license (View raylib.h for details)
*
*   Copyright (c) 2013-2016 Ramon Santamaria (@raysan5)
*
********************************************************************************************/

using Raylib_cs;
using System.Numerics;
using static RaylibWindowNamespace.Objects;
using static Raylib_cs.Color;
using static Raylib_cs.Raylib;
using CRPGNamespace;
using System.IO.Compression;

namespace RaylibWindowNamespace
{
    public class Window
    {
        enum CombatPhase { START, PLAYERATTACK, PAUSE, ENEMYATTACK, END }
        public const int screenWidth = 800;
        public const int screenHeight = 450;

        public static Timer attackTimer = new Timer(20);

        public static Vector4 playZoneBarrier = new Vector4(borderThickness, borderThickness, 
            screenWidth - borderThickness, screenHeight - borderThickness);

        CombatPhase stage = CombatPhase.START;

        //Player curPlayer;
        //Monster curMonster;

        public bool WindowHidden { get { return IsWindowHidden(); } }

        public Window()
        {
            InitWindow(screenWidth, screenHeight, "Combat");

            SetTargetFPS(60);

            InitializeCharacters();

            HideWindow();
        }

        public void StartAttack(Player curPlayer, Monster curMonster)
        {
            stage = CombatPhase.START;
            player.creature = curPlayer;
            monster.creature = curMonster;

            (player.creature as Player).currentWeapon.weaponAttack.Start();
            (monster.creature as Monster).enemyAttack.Start();

            attackTimer.Reset(7);
            UnhideWindow();
        }

        void EndAttack()
        {
            player.creature = null;
            monster.creature = null;
            Utils.Print();
            HideWindow();
        }

        public void Run()
        {
            BeginDrawing();
            ClearBackground(BLACK);
            DrawUI(stage);
            switch (stage)
            {
                case CombatPhase.START:
                    DrawText("Loading", 300, 200, 40, RAYWHITE);
                    if (attackTimer.Check())
                    {
                        Vector2 vec = new Vector2(0, borderThickness + healthBar.height);
                        healthBorder.position = vec;
                        vec = Vector2.One * borderThickness;
                        healthBackground.position = vec;
                        healthBar.position = vec;
                        healthBar.width = ((float)monster.creature.currentHP / (float)monster.creature.maximumHP) * healthBackground.width;

                        stage = CombatPhase.PLAYERATTACK;
                    }
                    break;
                case CombatPhase.PLAYERATTACK:
                    PlayerAttack();
                    break;
                case CombatPhase.PAUSE:
                    DrawText("Press Enter to continue", 170, 50, 40, RAYWHITE);
                    if (IsKeyPressed(KeyboardKey.KEY_ENTER))
                    {
                        Vector2 vec = new Vector2(0, screenHeight - (borderThickness * 2 + healthBar.height));
                        healthBorder.position = vec;
                        vec.X = borderThickness;
                        vec.Y = screenHeight - (borderThickness + healthBar.height);
                        healthBackground.position = vec;
                        healthBar.position = vec;
                        healthBar.width = ((float)player.creature.currentHP / (float)player.creature.maximumHP) * healthBackground.width;

                        stage = CombatPhase.ENEMYATTACK;
                    }
                    break;
                case CombatPhase.ENEMYATTACK:
                    EnemyAttack();
                    break;
                case CombatPhase.END:
                    EndAttack();
                    break;
            }
            EndDrawing();
        }

        void DrawUI(CombatPhase phase)
        {
            topBar.Draw();
            bottomBar.Draw();
            leftBar.Draw();
            rightBar.Draw();

            switch (phase)
            {
                case CombatPhase.PLAYERATTACK:
                    healthBorder.Draw();
                    healthBackground.Draw();
                    healthBar.Draw();
                    playZoneBarrier.Y = borderThickness * 2 + healthBar.height;
                    break;
                case CombatPhase.ENEMYATTACK:
                    healthBorder.Draw();
                    healthBackground.Draw();
                    healthBar.Draw();
                    playZoneBarrier.W = screenHeight - (borderThickness * 2 + healthBar.height);
                    break;
                default:
                    playZoneBarrier.Y = borderThickness;
                    playZoneBarrier.W = screenHeight - borderThickness;
                    break;
            }
        }

        void PlayerAttack()
        {
            if (attackTimer.Check())
            {
                stage = CombatPhase.PAUSE;
                return;
            }
            (player.creature as Player).currentWeapon.weaponAttack.Update();
        }
        void EnemyAttack()
        {
            if (attackTimer.Check())
            {
                stage = CombatPhase.END;
                return;
            }
            (monster.creature as Monster).enemyAttack.Update();
        }

        public void Close()
        {
            CloseWindow();
        }
    }
}