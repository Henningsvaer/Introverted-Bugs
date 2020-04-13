using System;

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

        // Просто UI.
        static string DrawUI(out Int64 stoneCountX, out int bugCountY)
        {
            string result;
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("X--Введите количество камней: ");
                stoneCountX = Int64.Parse(Console.ReadLine());
                Console.WriteLine("Y--Введите количество жуков: ");
                bugCountY = int.Parse(Console.ReadLine());
                result = "y";
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
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
            Console.ForegroundColor = ConsoleColor.Yellow;
            Stones stones;
            CreateStones(out stones, stoneCountX);

            Int64 rightBorderIndex = -1;
            Int64 dist = -1;
            Int64 index = -1;

            // Каждый жук последовательно выбирает себе камень.
            for (int i = 0; i < bugCountY; i++)
            {
                rightBorderIndex = -1;
                dist = GetMaxLengthFreeDistance(stones.Part1, stones.Part2, ref rightBorderIndex);
                Console.WriteLine($"---Индекс правой границы: {rightBorderIndex}");

                index = rightBorderIndex - GetBugIndex(dist);
                Console.WriteLine($"---Индекс жука: {index}");
                Console.WriteLine();

                if (index < stones.Part1.Length)
                {
                    stones.Part1[index] = true;
                }
                else // if(index >= stones.Part1.Length)
                {
                    stones.Part2[index - stones.Part1.Length] = true;
                }
            }

            OutputLastBugFreeSpaceAround(stoneCountX, bugCountY, index, 
                stones.Part1, stones.Part2);
        }

        // Пример вывода: X=8, Y=1 – ответ 3,4.
        static void OutputLastBugFreeSpaceAround(Int64 X, int Y, 
            Int64 index, bool[] part1, bool[] part2)
        {
            Console.ForegroundColor = ConsoleColor.White;

            int left = 0;
            int right = 0;

            // Если индекс жука находится в первой части массива...
            if(index < part1.Length)
            {
                // ...то ищем там все пустые камни слева от жука.
                for(int i = 1; i < part1.Length; i++)
                {
                    if(part1[i] == true)
                    {
                        break;
                    }

                    left++;
                }
                // Возможно пустые камни есть во второй части массива.
                bool hasInPart2 = true;
                // Ищем пустые камни справа от жука.
                for (int i = left + 2; i < part1.Length; i++)
                {
                    if (part1[i] == true)
                    {
                        // Находим жука справа и прекращаем поиск.
                        hasInPart2 = false;
                        break;
                    }

                    right++;
                }
                if(hasInPart2)
                {
                    for(int i = 0; i < part2.Length - 1; i++)
                    {
                        if (part2[i] == true)
                        {
                            break;
                        }

                        right++;
                    }
                }

            }
            // Если индекс жука находится во второй части массива...
            else
            {
                // ...то ищем там все пустые камни справа от жука.
                for (int i = part2.Length - 2; i > 0; i--)
                {
                    if (part2[i] == true)
                    {
                        break;
                    }

                    right++;
                }
                // Возможно пустые камни есть во первой части массива.
                bool hasInPart1 = true;
                // Ищем пустые камни слева от жука.
                for (int i = part2.Length - 2; i > right; i--)
                {
                    if (part2[i] == true)
                    {
                        // Находим жука справа и прекращаем поиск.
                        hasInPart1 = false;
                        break;
                    }

                    left++;
                }
                if (hasInPart1)
                {
                    for (int i = part1.Length - 1; i > 1; i--)
                    {
                        if (part1[i] == true)
                        {
                            break;
                        }

                        left++;
                    }
                }
            }
            // Пример вывода: 
            // X=8, Y=1 – ответ 3,4.
            // X=8, Y=2 – ответ 1,2
            // X=8, Y=3 – ответ 1,1
            Console.WriteLine($"X={X}, Y={Y} - ответ {left},{right}");
        }

        static Int64 GetMaxLengthFreeDistance(bool[] array1, bool[] array2, 
            ref Int64 rightBorderIndex)
        {
            Int64 dist = 0;
            Int64 maxDist = 0;

            for (int i = 0; i < array1.Length;i++)
            {
                if(array1[i] == false)
                {
                    dist++;
                    continue;
                }
                if(maxDist < dist)
                {
                    rightBorderIndex = i;
                    maxDist = dist;
                }
                dist = 0;
            }

            for (int i = 0; i < array2.Length; i++)
            {
                if (array2[i] == false)
                {
                    dist++;
                    continue;
                }
                if (maxDist < dist)
                {
                    rightBorderIndex = i + array1.Length - 1;
                    maxDist = dist;
                }
                dist = 0;
            }

            if (maxDist == 0 && dist > 0)
            {
                maxDist = dist;
            }

            return maxDist;
        }

        // Индекс жука относительно дистанции с поправкой на четность.
        static Int64 GetBugIndex(Int64 distance)
        {
            int e = 0;
            if (distance % 2 != 0)
            {
                e = 1;
            }
            return (Int64)(distance / 2 + e);
        }

        static void CreateStones(out Stones stones, Int64 stoneCountX)
        {
            // Разделяем камни на 2 массива.
            int e = 0;
            if (stoneCountX % 2 != 0)
            {
                e = 1;
            }

            int path1 = (int)(stoneCountX / 2 + e);
            int path2 = (int)(stoneCountX / 2);

            // Общее количество камней.
            stones = new Stones()
            {
                Part1 = new bool[path1 + 1],
                Part2 = new bool[path2 + 1]
            };
            stones.Part1[0] = true;
            stones.Part2[stones.Part2.Length - 1] = true;
        }

        // Разделение нужно, потому что CLR не позволяет 
        // создавать массивы размерностью > int.MaxValue. (~ 0.5 от 4млрд.)
        struct Stones
        {
            public bool[] Part1 { get; set; }
            public bool[] Part2 { get; set; }

        }
        struct MaxDistanceBetweenStones
        {
            public Int64 RightBorderIndex { get; set; }
            public Int64 IndexForNewBug { get; set; }
            public Int64 MaxDistance { get; set; }
        }
    }
}
