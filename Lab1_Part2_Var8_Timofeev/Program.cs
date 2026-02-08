using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Лабораторная 1 (часть 2) — вариант 8");
            Console.WriteLine("Функции:");
            Console.WriteLine("  1) Перевод 10 -> 2");
            Console.WriteLine("  2) Макс. подряд одинаковых в массиве");
            Console.WriteLine("  3) Сколько раз число встречается в массиве");
            Console.WriteLine("Процедуры:");
            Console.WriteLine("  4) Матрица случайных чисел");
            Console.WriteLine("  5) Спираль из чисел в матрице");
            Console.WriteLine("0) Выход");
            Console.Write("Выбор: ");
            string choice = Console.ReadLine() ?? "";

            try
            {
                if (choice == "1")
                {
                    Console.Clear();
                    int n = ReadInt("Введите целое число (>=0): ", 0, int.MaxValue);
                    string bin = DecToBin(n);
                    Pause("Двоичное представление: " + bin);
                }
                else if (choice == "2")
                {
                    Console.Clear();
                    int[] arr = InputArray();
                    int maxLen = MaxConsecutiveEqual(arr);
                    Pause("Максимум подряд одинаковых элементов: " + maxLen);
                }
                else if (choice == "3")
                {
                    Console.Clear();
                    int[] arr = InputArray();
                    int x = ReadInt("Какое число ищем? ", int.MinValue, int.MaxValue);
                    int cnt = CountOccurrences(arr, x);
                    Pause("Количество вхождений: " + cnt);
                }
                else if (choice == "4")
                {
                    Console.Clear();
                    int rows = ReadInt("Количество строк: ", 1, 50);
                    int cols = ReadInt("Количество столбцов: ", 1, 50);
                    int min = ReadInt("Min значение: ", -1000000, 1000000);
                    int max = ReadInt("Max значение: ", -1000000, 1000000);
                    if (min > max) { int t = min; min = max; max = t; }

                    int[,] m = CreateRandomMatrix(rows, cols, min, max);
                    PrintMatrix(m);
                    Pause("Готово.");
                }
                else if (choice == "5")
                {
                    Console.Clear();
                    int n = ReadInt("Размер матрицы N (NxN): ", 1, 30);
                    int[,] s = CreateSpiralMatrix(n);
                    PrintMatrix(s);
                    Pause("Готово.");
                }
                else if (choice == "0")
                {
                    return;
                }
                else
                {
                    Pause("Неверный выбор.");
                }
            }
            catch (Exception ex)
            {
                Pause("Ошибка: " + ex.Message);
            }
        }
    }

    // ------------------ ФУНКЦИИ (вариант 8) ------------------

    // 1) Перевести число из десятичной системы в двоичную.
    static string DecToBin(int n)
    {
        if (n == 0) return "0";

        string res = "";
        while (n > 0)
        {
            int bit = n % 2;
            res = bit + res;
            n = n / 2;
        }
        return res;
    }

    // 2) Найти максимальное количество последовательных одинаковых элементов в массиве.
    static int MaxConsecutiveEqual(int[] arr)
    {
        if (arr == null || arr.Length == 0) return 0;

        int best = 1;
        int cur = 1;

        for (int i = 1; i < arr.Length; i++)
        {
            if (arr[i] == arr[i - 1])
                cur++;
            else
                cur = 1;

            if (cur > best) best = cur;
        }

        return best;
    }

    // 3) Определить, сколько раз определенное число встречается в массиве.
    static int CountOccurrences(int[] arr, int x)
    {
        if (arr == null) return 0;

        int cnt = 0;
        for (int i = 0; i < arr.Length; i++)
            if (arr[i] == x) cnt++;

        return cnt;
    }

    // ------------------ ПРОЦЕДУРЫ (вариант 8) ------------------

    // 1) Вывести на экран матрицу заданного размера со случайными числами.
    static int[,] CreateRandomMatrix(int rows, int cols, int min, int max)
    {
        int[,] m = new int[rows, cols];
        Random rnd = new Random();

        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                m[i, j] = rnd.Next(min, max + 1);

        return m;
    }

    // 2) Напечатать «спираль» из чисел в матрице заданного размера.
    static int[,] CreateSpiralMatrix(int n)
    {
        int[,] a = new int[n, n];

        int top = 0, bottom = n - 1, left = 0, right = n - 1;
        int val = 1;

        while (top <= bottom && left <= right)
        {
            for (int j = left; j <= right; j++)
                a[top, j] = val++;
            top++;

            for (int i = top; i <= bottom; i++)
                a[i, right] = val++;
            right--;

            if (top <= bottom)
            {
                for (int j = right; j >= left; j--)
                    a[bottom, j] = val++;
                bottom--;
            }

            if (left <= right)
            {
                for (int i = bottom; i >= top; i--)
                    a[i, left] = val++;
                left++;
            }
        }

        return a;
    }

    static void PrintMatrix(int[,] m)
    {
        int rows = m.GetLength(0);
        int cols = m.GetLength(1);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                Console.Write(m[i, j].ToString().PadLeft(5));
            }
            Console.WriteLine();
        }
    }

    // ------------------ ВВОД/УТИЛИТЫ ------------------

    static int[] InputArray()
    {
        int n = ReadInt("Размер массива: ", 1, 100000);
        int[] arr = new int[n];

        Console.WriteLine("Введите элементы массива:");
        for (int i = 0; i < n; i++)
        {
            arr[i] = ReadInt($"[{i}]: ", int.MinValue, int.MaxValue);
        }

        return arr;
    }

    static int ReadInt(string prompt, int min, int max)
    {
        while (true)
        {
            Console.Write(prompt);
            string s = (Console.ReadLine() ?? "").Trim();

            int v;
            if (int.TryParse(s, out v))
            {
                if (v < min || v > max)
                {
                    Console.WriteLine("Число вне диапазона.");
                    continue;
                }
                return v;
            }

            Console.WriteLine("Введите целое число.");
        }
    }

    static void Pause(string msg)
    {
        Console.WriteLine();
        Console.WriteLine(msg);
        Console.Write("Enter...");
        Console.ReadLine();
    }
}
