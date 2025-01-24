using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BigBattleships
{
    internal class Interface
    {
        string question = "";
        string[] options = { "" };
        string[,] map1;
        string[,] map2;

        //Chars for map
        string emptyCell = "  ";
        string boatCell = "[]";
        string markedCell = "XX";
        string waterCell = "--";

        //Coordinates for Printing areas
        int messageAreaX = 4;   //X coor for messages start
        int messageAreaY = 17;  //Y coor for messages start
        int mapAreaX = 10;
        int mapAreaY = 7;

        internal void DrawScreen()
        {
            DrawLogo();
            DrawMaps();
        }

        internal void DrawLogo()
        {
            Console.SetCursorPosition(0, 0);

            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();

            // Ställ in bakgrundsfärg
            Console.BackgroundColor = ConsoleColor.Blue;

            // Skriv översta raden (vågor)
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");

            // Skriv rubriken
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("|| ");
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write("BATTLESHIPS: PREPARE FOR COMBAT!");
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("  ^         ||");

            // Skriv skeppen och havet
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("||      __      __      __        __/ \\__      ||");
            Console.WriteLine("||  ___/  \\____/  \\____/  \\______/       \\___  ||");

            // Skriv nedersta raden (vågor)
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");

            // Återställ färgerna
            Console.ForegroundColor = ConsoleColor.Green;
            Console.BackgroundColor = ConsoleColor.Black;
        }

        public void updateMaps(string[,] mapOne, string[,] mapTwo)
        {
            map1 = mapOne;
            map2 = mapTwo;
        }

        internal void DrawMaps()
        {
            DrawMap(0, map1);
            DrawMap(1, map2);
        }

        internal void DrawMap(int index, string[,] Map, int[] activeCoor = null)
        {
            int xMap = mapAreaX + index * 20;
            int yMap = mapAreaY;
            Console.SetCursorPosition(xMap, yMap - 1);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("A B C D E F G H I");
            for (int i = 0; i < Map.GetLength(1); i++)
            {
                Console.SetCursorPosition(xMap - 1, yMap + i);
                Console.BackgroundColor = ConsoleColor.Black;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write(i);
                for (int j = 0; j < Map.GetLength(0); j++)
                {
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    if (activeCoor != null)
                    {
                        if(activeCoor[0] == j && activeCoor[1] == i)
                        {
                            Console.BackgroundColor = ConsoleColor.DarkBlue;
                        }else if (Map[j, i] == markedCell)
                        {
                            Console.BackgroundColor = ConsoleColor.Red;
                        }else if (Map[j, i] == boatCell)
                        {
                            Console.BackgroundColor = ConsoleColor.Gray;
                        }else if (Map[j, i] == waterCell)
                        {
                            Console.BackgroundColor = ConsoleColor.White;
                        }
                    }
                    Console.Write(Map[j, i]);
                }
                Console.WriteLine();
            }

        }

        internal void UpdateQuestion(string newQuestion, string[] newOptions)
        {
            question = newQuestion.Trim();
            options = newOptions;
        }

        internal int OptionMenu()
        {
            bool rightAnswer = false;
            int answer = 0;
            ClearMessageArea();
            while (!rightAnswer)
            {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.BackgroundColor = ConsoleColor.Black;
                Console.SetCursorPosition(messageAreaX, messageAreaY);
                Console.WriteLine(question);
                for (int i = 0; i < options.Length; i++)
                {
                    if(i == answer)
                    {
                        Console.BackgroundColor=ConsoleColor.Gray;
                        Console.ForegroundColor=ConsoleColor.Black;
                    }
                    else
                    {
                        Console.ForegroundColor= ConsoleColor.Green;
                        Console.BackgroundColor=ConsoleColor.Black;
                    }
                    Console.WriteLine(i + ". " + options[i]);
                }
                ConsoleKey pressedKey = Console.ReadKey().Key;
                switch (pressedKey)
                {
                    case ConsoleKey.UpArrow:
                        answer--;
                        if (answer< 0)
                        {
                            answer= options.Length - 1;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        answer++;
                        if (answer== options.Length)
                        {
                            answer = 0;
                        }
                        break;
                    case ConsoleKey.Enter:
                        rightAnswer = true; ;
                        break;
                }
            }
            return answer;
        }

        internal void DrawMessage(string message)
        {
            ClearMessageArea();
            Console.SetCursorPosition(messageAreaX, messageAreaY);
            Console.ForegroundColor = ConsoleColor.Green;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.WriteLine(message);
        }

        private void ClearMessageArea()
        {
            Console.Clear();
            DrawLogo();
            DrawMaps();
        }

        internal int[] GetCoordinate(string[,] map, int mapSlot)
        {
            int xActive = 0;
            int yActive = 0;
            bool choosing = true;
            while (choosing)
            {
                DrawMap(mapSlot, map, new int[] { xActive, yActive });
                ConsoleKey userKey = Console.ReadKey().Key;
                switch (userKey)
                {
                    case ConsoleKey.RightArrow:
                        xActive++;
                        if (xActive == map.GetLength(0))
                        {
                            xActive = 0;
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        xActive--;
                        if (xActive < 0)
                        {
                            xActive = map.GetLength(0) - 1;
                        }
                        break;
                    case ConsoleKey.UpArrow:
                        yActive--;
                        if (yActive < 0)
                        {

                            yActive = map.GetLength(1) - 1;
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        yActive++;
                        if (yActive == map.GetLength(1))
                        {
                            yActive = 0;
                        }
                        break;
                    case ConsoleKey.Enter:
                        choosing = false;
                        break;
                }
            }
            return [xActive, yActive];
        }

        /*
        internal void DrawBoat(Boat boat, int xMap, int yMap)
        {
            int xModificator = 0;
            int yModificator = 0;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.BackgroundColor = ConsoleColor.Cyan;
            for (int i = 0; i < boat.Length; i++)
            {
                if (boat.isHorizontal)
                {
                    xModificator = 1;
                }
                else
                {
                    yModificator = 1;
                }
                if (boat.damaged[i])
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                }
                Console.SetCursorPosition(boat.Xpos * 2 + xMap + 1 + i * xModificator * 2, boat.Ypos + yMap + (i * yModificator) + 1);
                Console.Write("[]");
                Console.BackgroundColor = ConsoleColor.Cyan;

            }
        }

        internal void DrawBoats(Boat[] theFleet, int xMap, int yMap)
        {
            for (int i = 0; i < theFleet.Length; i++)
            {
                if (theFleet[i] != null)
                {
                    DrawBoat(theFleet[i], xMap, yMap);
                }
            }
        }

        internal void DrawIntell(int[,] intell, int xMap, int yMap)
        {
            
        }
        */
    }
}
