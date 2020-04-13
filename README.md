# Introverted-Bugs

## Решение проблемы с 4млрд записями
Так как стандартный массив может иметь не более чем int.MaxValue индексов,
я принял решение разделить все "камни" на 2 булевых массива, что в сумме
позволяет хранить >4млрд элементов.

## Основные методы.
```
static Int64 GetMaxLengthFreeDistance(bool[] array1, bool[] array2, ref Int64 rightBorderIndex)
```
* Возвращает максимальную длину свободного расстояния между жуками.

```
static void CreateStones(out Stones stones, Int64 stoneCountX)
```
* Инициализация структуры Stones.

```
static void OutputLastBugFreeSpaceAround(Int64 X, int Y, Int64 index, bool[] part1, bool[] part2)
```
* Алгорит поиска и вывода на экран свободного "пространства" возле последнего жука.

```
static string DrawUI(out Int64 stoneCountX, out int bugCountY)
```
* Консольный UI.

```
static void BugsHiddingAlgorithm(Int64 stoneCountX, int bugCountY)
```
* Алгоритм выбора камней жуками.
