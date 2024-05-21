using NoireRules.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoireRules.Logic
{
    public class Mover
    {
        public static void Move(PlayingField playingField)
        {
            Console.WriteLine("Выберите в каком направлении сдвинуть колоду");
            var key = Console.ReadKey();
            int pointX;
            int pointY;
            Card card;

            switch (key.Key)
            {
                case ConsoleKey.UpArrow:

                    Console.WriteLine("Выбирите ряд, который будете сдвигать");
                    pointX = int.Parse(Console.ReadLine()!);
                    pointX -= 1;
                    card = playingField.CardsSuspect![0, pointX];

                    for (int i = 0; i < playingField.CardsSuspect.GetLength(0); i++)
                    {
                        if (i <= 0)
                        {
                            playingField.CardsSuspect[i, pointX].PointY = playingField.CardsSuspect.GetLength(0) - 1;
                        }
                        else
                        {
                            playingField.CardsSuspect[i, pointX].PointY = i - 1;
                        }
                        if (i >= 1)
                        {
                            playingField.CardsSuspect[i - 1, pointX] = playingField.CardsSuspect[i, pointX];
                        }
                    }
                    playingField.CardsSuspect[playingField.CardsSuspect.GetLength(1) - 1, pointX] = card;

                    break;

                case ConsoleKey.DownArrow:

                    Console.WriteLine("Выбирите ряд, который будете сдвигать");
                    pointX = int.Parse(Console.ReadLine()!);
                    pointX -= 1;
                    card = playingField.CardsSuspect![playingField.CardsSuspect.GetLength(0) - 1, pointX];

                    for (int i = playingField.CardsSuspect.GetLength(0) - 1; i >= 0; i--)
                    {
                        if (i >= playingField.CardsSuspect.GetLength(1) - 1)
                        {
                            playingField.CardsSuspect[i, pointX].PointY = 0;
                        }
                        else
                        {
                            playingField.CardsSuspect[i, pointX].PointY = i + 1;
                        }
                        if (i <= playingField.CardsSuspect.GetLength(0) - 2)
                        {
                            playingField.CardsSuspect[i + 1, pointX] = playingField.CardsSuspect[i, pointX];
                        }
                    }
                    playingField.CardsSuspect[0, pointX] = card;

                    break;

                case ConsoleKey.LeftArrow:

                    Console.WriteLine("Выбирите столбец, который будете сдвигать");
                    pointY = int.Parse(Console.ReadLine()!);
                    pointY -= 1;
                    card = playingField.CardsSuspect![pointY, 0];

                    for (int i = 0; i < playingField.CardsSuspect.GetLength(1); i++)
                    {
                        if (i <= 0)
                        {
                            playingField.CardsSuspect[pointY, i].PointX = playingField.CardsSuspect.GetLength(1) - 1;
                        }
                        else
                        {
                            playingField.CardsSuspect[pointY, i].PointX = i - 1;
                        }
                        if (i >= 1)
                        {
                            playingField.CardsSuspect[pointY, i - 1] = playingField.CardsSuspect[pointY, i];
                        }
                    }
                    playingField.CardsSuspect[pointY, playingField.CardsSuspect.GetLength(1) - 1] = card;

                    break;

                case ConsoleKey.RightArrow:

                    Console.WriteLine("Выбирите столбец, который будете сдвигать");
                    pointY = int.Parse(Console.ReadLine()!);
                    pointY -= 1;
                    card = playingField.CardsSuspect![pointY, playingField.CardsSuspect.GetLength(1) - 1];

                    for (int i = playingField.CardsSuspect.GetLength(1) - 1; i >= 0; i--)
                    {
                        if (i >= playingField.CardsSuspect.GetLength(1) - 1)
                        {
                            playingField.CardsSuspect[pointY, i].PointX = 0;
                        }
                        else
                        {
                            playingField.CardsSuspect[pointY, i].PointX = i + 1;
                        }
                        if (i <= playingField.CardsSuspect.GetLength(1) - 2)
                        {
                            playingField.CardsSuspect[pointY, i + 1] = playingField.CardsSuspect[pointY, i];
                        }
                    }
                    playingField.CardsSuspect[pointY, 0] = card;

                    break;

                default:
                    Console.WriteLine("Такой команды нет");
                    break;
            }
        }
    }
}
