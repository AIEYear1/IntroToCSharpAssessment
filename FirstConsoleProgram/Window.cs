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

using CRPGNamespace;
using Raylib_cs;
using System;
using System.Numerics;
using static Raylib_cs.Color;
using static Raylib_cs.Raylib;
using static RaylibWindowNamespace.Objects;

namespace RaylibWindowNamespace
{
    public class Window
    {
        /// <summary>
        /// Enum that's only used in this class
        /// </summary>
        enum CombatPhase { START, PLAYERATTACK, PAUSE, ENEMYATTACK, END }

        //Screen width and height, I'm not sorry for using the comma method
        public const int screenWidth = 800, screenHeight = 450;

        //Timer for how long each attack phase is
        public static Timer attackTimer = new Timer(20);

        //This is for my boundries I decided using just ints was annoying
        public static Vector4 playZoneBarrier = new Vector4(borderThickness, borderThickness, 
            screenWidth - borderThickness, screenHeight - borderThickness);

        //The current stage of combat we're on
        CombatPhase stage = CombatPhase.START;

        /// <summary>
        /// Parameter to tell the main program if the Window is open or not
        /// </summary>
        public bool WindowHidden { get { return IsWindowHidden(); } }

        /// <summary>
        /// Constructor for initialization
        /// </summary>
        public Window()
        {
            //Initialize the window
            InitWindow(screenWidth, screenHeight, "Combat");

            SetTargetFPS(60);

            //Initialize characters and textures in the Objects Class
            InitializeCharacters();

            //Hide the window so the player can't see it
            HideWindow();
            //Clear the console for cleanliness sake
            Console.Clear();
        }

        /// <summary>
        /// Begin the attack System
        /// </summary>
        /// <param name="curPlayer">the current player from the main Program</param>
        /// <param name="curMonster">the current monster from the main program</param>
        public void StartAttack(Player curPlayer, Monster curMonster)
        {
            stage = CombatPhase.START;
            player.creature = curPlayer;
            monster.creature = curMonster;

            //Initialize everything
            (player.creature as Player).currentWeapon.WeaponAttack.Start();
            (monster.creature as Monster).enemyAttack.Start();
            player.Start();
            monster.Start();
            Input.Start();

            //Reset timer to three seconds before completion to anticipate messy GetFrameTime()
            attackTimer.Reset(attackTimer.delay - 3);
            UnhideWindow();
        }

        /// <summary>
        /// Clear out the attack system
        /// </summary>
        public void EndAttack()
        {
            player.creature = null;
            monster.creature = null;
            Utils.Print();
            //Hide the window
            HideWindow();
        }

        /// <summary>
        /// Update everything in the windows to play the attack minigames
        /// </summary>
        public void Run()
        {
            BeginDrawing();
            ClearBackground(BLACK);
            switch (stage)
            {
                //1st case "StartPhase", Preloading scene to make Timer count properly
                case CombatPhase.START:
                    DrawText("Loading", 300, 200, 40, RAYWHITE);
                    if (attackTimer.Check())
                    {
                        //Setup enemy healthbar to display correctly
                        Vector2 vec = new Vector2(0, borderThickness + healthBar.Height);
                        healthBorder.Position = vec;
                        vec = Vector2.One * borderThickness;
                        healthBackground.Position = vec;
                        healthBar.Position = vec;
                        healthBar.Width = ((float)monster.creature.currentHP / (float)monster.creature.maximumHP) * healthBackground.Width;

                        stage = CombatPhase.PLAYERATTACK;
                    }
                    break;
                //2nd case "PlayerAttackPhase", runs the players attack against the enemy
                case CombatPhase.PLAYERATTACK:
                    PlayerAttack();
                    break;
                //3rd case "PausePhase", Pause the action and give the player a chance to state when their ready
                case CombatPhase.PAUSE:
                    DrawText("Press Enter to continue", 170, 50, 40, RAYWHITE);
                    //Wait until the player presses Enter
                    if (IsKeyPressed(KeyboardKey.KEY_ENTER))
                    {
                        //Setup player Healthbar to display properly
                        Vector2 vec = new Vector2(0, screenHeight - (borderThickness * 2 + healthBar.Height));
                        healthBorder.Position = vec;
                        vec.X = borderThickness;
                        vec.Y = screenHeight - (borderThickness + healthBar.Height);
                        healthBackground.Position = vec;
                        healthBar.Position = vec;
                        healthBar.Width = ((float)player.creature.currentHP / (float)player.creature.maximumHP) * healthBackground.Width;

                        stage = CombatPhase.ENEMYATTACK;
                    }
                    break;
                //4th case "EnemyAttackPhase", runs the enemys attack against the player 
                case CombatPhase.ENEMYATTACK:
                    EnemyAttack();
                    break;
                //5th case "EndPhase", Ends the Attack system
                case CombatPhase.END:
                    EndAttack();
                    break;
            }
            DrawUI(stage);
            EndDrawing();
        }

        /// <summary>
        /// Draws all the UI elements
        /// </summary>
        /// <param name="phase">the phase of combat we are in</param>
        void DrawUI(CombatPhase phase)
        {
            //Draw the main border
            topBar.Draw();
            bottomBar.Draw();
            leftBar.Draw();
            rightBar.Draw();

            switch (phase)
            {
                //1st case "PausePhase" or "PlayerAttackPhase", draw the enemy healthbar and set the playZoneBarrier accordingly
                case CombatPhase.PAUSE:
                case CombatPhase.PLAYERATTACK:
                    healthBorder.Draw();
                    healthBackground.Draw();
                    healthBar.Draw();
                    playZoneBarrier.Y = borderThickness * 2 + healthBar.Height;
                    break;
                //2nd case "EnemyAttackPhase", draw the player healthbar and set the playZoneBarrier accordingly
                case CombatPhase.ENEMYATTACK:
                    healthBorder.Draw();
                    healthBackground.Draw();
                    healthBar.Draw();
                    playZoneBarrier.W = screenHeight - (borderThickness * 2 + healthBar.Height);
                    break;
                //Overflow, set the playZoneBarrier to the default setup
                default:
                    playZoneBarrier.Y = borderThickness;
                    playZoneBarrier.W = screenHeight - borderThickness;
                    break;
            }
        }

        /// <summary>
        /// Player's attack loop
        /// </summary>
        void PlayerAttack()
        {
            if (attackTimer.Check())
            {
                stage = CombatPhase.PAUSE;
                return;
            }
            (player.creature as Player).currentWeapon.WeaponAttack.Update();
        }
        /// <summary>
        /// Enemy's attack loop
        /// </summary>
        void EnemyAttack()
        {
            if (attackTimer.Check())
            {
                stage = CombatPhase.END;
                return;
            }
            (monster.creature as Monster).enemyAttack.Update();
        }

        /// <summary>
        /// Closes the window
        /// </summary>
        public void Close()
        {
            CloseWindow();
        }
    }
}