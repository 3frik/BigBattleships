using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BigBattleships
{
    internal class Player
    {
        internal string name = "Player";
        int height = 10;
        int width = 10;
        internal bool isAI = false;
        internal Boat[] fleet;
        internal string[,] FleetMap;
        internal string[,] EnemyMap;

        //Chars for map
        string emptyCell = "  ";
        string boatCell = "[]";
        string markedCell = "XX";
        string waterCell = "--";

        public Player(string name, int width, int height, bool AI = false)
        {
            this.name = name;
            this.width = width;
            this.height = height;
            this.isAI = AI;
            FleetMap = new string[width, height];
            EnemyMap = new string[width, height];
            ClearMap(FleetMap);
            ClearMap(EnemyMap);
        }

        /// <summary>
        /// Fills the map with empty spaces
        /// </summary>
        /// <param name="map"></param>
        private void ClearMap(string[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    map[i, j] = emptyCell;
                }
            }
        }

        public void deployFleet(Boat[] theFleet)
        {
            fleet = theFleet;
        }

        public void deployBoat(Boat aBoat)
        {
            int xModifier = 0;
            int yModifier = 0;
            if (aBoat.isHorizontal)
            {
                xModifier = 1;
            }
            else
            {
                yModifier = 1;
            }
            for (int i = 0; i < aBoat.Length; i++)
            {
                int nextX = aBoat.Xpos + i * xModifier;
                int nextY = aBoat.Ypos + i * yModifier;
                FleetMap[nextX, nextY] = boatCell;
            }
        }

        internal void DeployRandomFleet()
        {
            for (int i = 0; i < fleet.Length; i++)
            {
                Random rnd = new Random();
                while (!IsPlaceBoat(fleet[i]))
                {
                    fleet[i].Xpos = rnd.Next(width);
                    fleet[i].Ypos = rnd.Next(height);
                }
                deployBoat(fleet[i]);
            }
        }

        /// <summary>
        /// Try to place a boat on the player map
        /// </summary>
        /// <param name="boat"></param>
        /// <returns>wether the boat can be placed</returns>
        internal bool IsPlaceBoat(Boat boat)
        {
            bool answer = true;
            int xModifier = 0;
            int yModifier = 0;
            if (boat.isHorizontal)
            {
                xModifier = 1;
            }
            else
            {
                yModifier = 1;
            }
            for (int i = 0; i < boat.Length; i++)
            {
                int nextX = boat.Xpos + i * xModifier;
                int nextY = boat.Ypos + i * yModifier;
                if (nextX >= width || nextY >= height || FleetMap[nextX, nextY] != "  ")
                {
                    answer = false;
                    break;
                }
            }

            return answer;
        }

        internal bool AttackIn(int xCoor, int yCoor)
        {
            bool result = false;
            if (FleetMap[xCoor, yCoor] == boatCell)
            {
                result = true;
                FleetMap[xCoor, yCoor] = markedCell;
            }
            return result;
        }

        internal bool AttackOut(Player enemy, int xCoor, int yCoor)
        {
            bool result = enemy.AttackIn(xCoor, yCoor);
            if (result)
            {
                EnemyMap[xCoor, yCoor] = markedCell;
            }
            else
            {
                EnemyMap[xCoor, yCoor] = waterCell;
            }
            return result;
        }

        internal bool IsStillFighting()
        {
            bool isStillFighting = false;
            for (int i = 0; i < FleetMap.GetLength(0); i++)
            {
                for (int j = 0; j < FleetMap.GetLength(1); j++)
                {

                    if (FleetMap[i, j] == boatCell)
                    {
                        isStillFighting = true;
                    }
                }
            }
            return isStillFighting;

        }


        /*
        //Create the boats, try to put them on the map
        internal void GatherFleet(int[] boatSizes)
        {
            Boat[] theFleet = new Boat[boatSizes.Length];
            for (int i = 0; i < boatSizes.Length; i++)
            {
                int newY = 0;
                int newX = 0;
                bool newHorizontal = true;
                bool rightanswer = false;
                bool rightplace = false;
                bool fitsX = newX + boatSizes[i] <= width;
                bool fitsY = newY + boatSizes[i] <= height;
                do
                {

                    while (!rightplace)
                    {
                        Console.Clear();

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.BackgroundColor = ConsoleColor.Black;
                        Console.WriteLine($"Where do you want to deploy your {boatSizes[i]} units long boat on the battlefield?");
                        while (!rightanswer)
                        {
                            Console.WriteLine($"X position (MAX {width}): ");
                            rightanswer = int.TryParse(Console.ReadLine(), out newX);
                            if (!rightanswer || newX >= width)
                            {
                                Console.WriteLine("That is not a valid coordinate");
                                rightanswer = false;
                            }
                        }
                        while (rightanswer)
                        {
                            Console.WriteLine($"Y position (MAX {height}): ");
                            rightanswer = !int.TryParse(Console.ReadLine(), out newY);
                            if (rightanswer || newY >= height)
                            {
                                Console.WriteLine("That is not a valid coordinate");
                                rightanswer = true;
                            }
                        }
                        fitsX = newX + boatSizes[i] <= width;
                        fitsY = newY + boatSizes[i] <= height;
                        rightplace = fitsX || fitsY;
                    }
                    while (!rightanswer)
                    {
                        Console.WriteLine("How shall the boat be deployed?");
                        if (!fitsX)
                        {
                            Console.WriteLine("The only alternative is to deploy north to south.");
                            newHorizontal = false;
                            rightanswer = true;
                        }
                        else if (!fitsY)
                        {
                            Console.WriteLine("The only alternative is to deploy west to east.");
                            newHorizontal = true;
                            rightanswer = true;
                        }
                        else
                        {
                            Console.WriteLine("1. North to south.\n2. West to east.");
                            string answer = Console.ReadLine();
                            if (answer == "1")
                            {
                                rightanswer = true;
                                newHorizontal = false;
                            }
                            else if (answer == "2")
                            {
                                rightanswer = true;
                                newHorizontal = true;
                            }
                            else
                            {
                                Console.WriteLine("Not a valid answer. Try again.");
                            }
                        }
                    }
                    Boat newBoat = new Boat(boatSizes[i], newX, newY, newHorizontal);
                    rightplace = IsPlaceBoat(newBoat);
                    if (rightplace)
                    {
                        theFleet[i] = newBoat;
                        string facing = newHorizontal ? "east" : "south";
                        Console.WriteLine($"The boat has been deployed at ({newX},{newY}), facing {facing}.");
                    }
                    else
                    {
                        Console.WriteLine($"That position seems to be already occupied.");

                    }
                    rightanswer = false;
                } while (!rightplace);
            }
            deployFleet(theFleet);
        }
        */

    }
}
