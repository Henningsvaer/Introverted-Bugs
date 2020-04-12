using System;
using System.Collections.Generic;

namespace IntrovertedBugsConsoleApp
{
    class Program
    {
        /*
         * Жуки не любят находиться рядом друг с другом 
         * и каждый прячется под отдельным камнем и старается 
         * выбирать камни, максимально удаленные от соседей. 
         * Так же жуки любят находится максимально далеко от края. 
         * Как только жук сел за камень, он более не перемещается. 
         * Всего в линии лежат X камней. И туда последовательно 
         * бежит прятаться Y жуков. Найти сколько свободных камней 
         * будет слева и справа от последнего жука.
         * X может быть до 4 млрд.
         * Примеры:

            X=8, Y=1 – ответ 3,4   

            X=8, Y=2 – ответ 1,2   

            X=8, Y=3 – ответ 1,1

           Написать программу вычисления ответа на любом языке.
           Просьба, с инета ответ не списывать-там неправильно.
         */
        static void Main(string[] args)
        {
            while(true)
            {
                Int64 stoneCountX;
                int bugCountY;
                string result = DrawUI(out stoneCountX, out bugCountY);

                if(result.ToLower() == "y")
                {
                    BugsHiddingAlgorithm(stoneCountX, bugCountY);
                }
                Console.WriteLine();
            }
        }

        static string DrawUI(out Int64 stoneCountX, out int bugCountY)
        {
            string result;
            try
            {
                Console.WriteLine("Введите количество камней: ");
                stoneCountX = Int64.Parse(Console.ReadLine());
                Console.WriteLine("Введите количество жуков: ");
                bugCountY = int.Parse(Console.ReadLine());
                result = "y";
            }
            catch
            {
                Console.WriteLine("Ошибка ввода данных...");
                stoneCountX = -1;
                bugCountY = -1;
                result = "n";
            }
            return result;
        }

        // Алгоритм выбора камней жуками.
        static void BugsHiddingAlgorithm(Int64 stoneCountX, int bugCountY)
        {
            Stones stones;
            CreateStones(out stones, stoneCountX);

            // Каждый жук ищет свое место...
            for(int i = 0; i < bugCountY; i++)
            {
                // Вспомогательные данные для определения места для жука.
                var mdbs = new MaxDistanceBetweenStones()
                {
                    IsPart1 = true,
                    A = 0,
                    B = 0,
                    Distance = 0
                };

                Int64 dist = 0;
                int nextA = 0;
                // Сначала в первой части...
                for(int j = 0; j < stones.Part1.Length; j++)
                {
                    if(stones.Part1[j] == false)
                    {
                        dist++;
                        continue;
                    }

                    if(mdbs.Distance > dist)
                    {
                        continue;
                    }

                    // Рассчитываем потенцильное местонахождение наиболее 
                    // благоприятного камня для жука.
                    mdbs.Distance = dist;
                    if(mdbs.B > 0)
                    {
                        mdbs.A = nextA;
                    }
                    mdbs.B = j - 1;
                    nextA = j + 1;
                    dist = 0;
                }
                // ...потом во второй.
                for (int j = 0; j < stones.Part2.Length; j++)
                {
                    if (stones.Part2[j] == false)
                    {
                        dist++;
                        continue;
                    }

                    if (mdbs.Distance > dist)
                    {
                        continue;
                    }

                    // Рассчитываем потенцильное местонахождение наиболее 
                    // благоприятного камня для жука.
                    mdbs.Distance = dist;
                    if (mdbs.B > 0)
                    {
                        mdbs.A = nextA;
                    }
                    mdbs.B = j - 1;
                    nextA = j + 1;
                    dist = 0;
                    mdbs.IsPart1 = false;
                }
                // Сохраняем запись о пренадлежности камня жуку.
            }

        }

        static void CreateStones(out Stones stones, Int64 stoneCountX)
        {
            // Разделяем камни на 2 массива.
            int e = 0;
            if (stoneCountX % 2 != 0)
            {
                e = 1;
            }

            int path1 = (int)stoneCountX / 2 + e;
            int path2 = (int)stoneCountX / 2;

            // Общее количество камней.
            stones = new Stones()
            {
                Part1 = new bool[path1],
                Part2 = new bool[path2]
            };
        }
        struct Stones
        {
            public bool[] Part1 { get; set; }
            public bool[] Part2 { get; set; }

        }
        struct MaxDistanceBetweenStones
        {
            public bool IsPart1 { get; set; }
            public int A { get; set; }
            public int B { get; set; }
            public Int64 Distance { get; set; }
        }
    }
}
