using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BigBattleships
{


    /// <summary>
    /// Game motor takes responsibility over:
    /// - Coordinate other motors
    /// - Show things on screen [now Screen]
    /// - Save information about fleet and maps for each player [ now Player]
    /// </summary>
    internal sealed class GameMotor
    {
        //GLobal variables?
        internal static int mapWidth = 9;
        internal static int mapHeight = 9;

        internal Interface iface = new Interface(mapWidth,mapHeight);
        internal Player P1 = new Player("Player1", mapWidth, mapHeight);
        internal Player P2 = new Player("Player2", mapWidth, mapHeight);
        internal Boat[] Fleet1 = [new Boat(2), new Boat(3), new Boat(3), new Boat(4), new Boat(5)];
        internal Boat[] Fleet2 = [new Boat(2), new Boat(3), new Boat(3), new Boat(4), new Boat(5)];

        //FOR testing adn debugging!!!!!!
        internal void Testing()
        {
            P1 = new Player("Player1", mapWidth, mapHeight);
            P2 = new Player("Player2", mapWidth, mapHeight);

            P1.deployFleet(Fleet1);
            P2.deployFleet(Fleet2);
            for (int i = 0; i < P1.fleet.Length; i++)
            {
                P1.fleet[i].Redeploy(i, i, 0);
                P1.deployBoat(P1.fleet[i]);
                P2.fleet[i].Redeploy(i, i, 1);
                P2.deployBoat(P2.fleet[i]);
            }
        }

        internal void Run()
        {
            //StartGame
            P1 = new Player("Player1", mapWidth, mapHeight);
            P2 = new Player("Player2", mapWidth, mapHeight);
            P1.deployFleet(Fleet1);
            P2.deployFleet(Fleet2);
            iface.updateMaps(P1.FleetMap, P1.EnemyMap);
            //Show on screen
            iface.DrawLogo();
            iface.DrawMaps();
            //Create battlefleet Player 1

            DeployPlayerFleet(P1);
            //Same player 2 or random
            iface.DrawMessage("Is player 2 a human player? Press Space to agree. Press any other key to play against AI.");
            if (Console.ReadKey().Key != ConsoleKey.Spacebar)
            {
                P2.isAI = true;
            }
            DeployPlayerFleet(P2);   ///////HAS TO BE CHANGED TO ALLOW AI PLAYERS
            iface.DrawMessage("Both fleet have been deployed.");

            ///Loop:
            bool playIsOn = true;
            Player activePlayer = new Player("active", 0, 0);
            Player enemyPlayer = new Player("enemy", 0, 0);
            while (playIsOn)
            {
                if (activePlayer == P1)
                {
                    activePlayer = P2;
                    enemyPlayer = P1;
                }
                else
                {
                    activePlayer = P1;
                    enemyPlayer = P2;
                }
                iface.updateMaps(activePlayer.FleetMap, activePlayer.EnemyMap);
                ///Show map and damage; and enemymap and attacks
                ///Choose new attack
                ///Chose new attack? if found mark
                ///Show result
                ///
                bool turnOn = true;
                while (turnOn && enemyPlayer.IsStillFighting())
                {
                    int[] attackCoor = new int[2];
                    if (activePlayer.isAI)
                    {
                        Random rnd = new Random();
                        attackCoor = new int[] { rnd.Next(mapWidth), rnd.Next(mapHeight) };
                    }
                    else
                    {
                        iface.DrawScreen();
                        iface.DrawMessage(activePlayer.name + ", select coordinate to attack.");
                        attackCoor = iface.GetCoordinate(activePlayer.EnemyMap, 1);
                    }
                    turnOn = activePlayer.AttackOut(enemyPlayer, attackCoor[0], attackCoor[1]);
                }
                if (!enemyPlayer.IsStillFighting())
                {
                    playIsOn = false;
                }
                Console.Clear();
                Console.Write("Your mark found only water. Press any key to jump to the next player.");
                Console.ReadKey();
            }
            iface.DrawMessage(activePlayer.name + " won the battle. Congratulations.\nThis simulation is now over. Do you want to try again? Press Space to run the simulation one more time.");
            if (Console.ReadKey().Key == ConsoleKey.Spacebar)
            {
                Run();
            }

        }

        private void DeployPlayerFleet(Player player)
        {
            if (!player.isAI)
            {
                for (int i = 0; i < player.fleet.Length; i++)
                {
                    do
                    {
                        iface.updateMaps(player.FleetMap, player.EnemyMap);
                        iface.DrawMessage(player.name + ". Use arrows to select coordinate for your " + player.fleet[i].Length + " units long boat");

                        int[] boatCoor = iface.GetCoordinate(player.FleetMap, 0);
                        iface.UpdateQuestion("Player. Time to deploy your fleet. How will you deploy your " + player.fleet[i].Length + " long boat?", ["Horizontally", "Vertically"]);
                        int newOrientation = iface.OptionMenu();
                        player.fleet[i].Redeploy(boatCoor[0], boatCoor[1], newOrientation);
                    }
                    while (!player.IsPlaceBoat(player.fleet[i]));
                    player.deployBoat(player.fleet[i]);
                }
                iface.updateMaps(player.FleetMap, player.EnemyMap);
                iface.DrawMaps();
            }
            else
            {
                player.DeployRandomFleet();
            }
            iface.DrawMessage("Your fleet has been deployed. Press any key to continue.");
            Console.ReadKey();
        }
    }
}
