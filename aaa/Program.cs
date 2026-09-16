using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {

        CreateSampleFileIfNotExists();

        while (true)
        {
            Console.Clear();
            Console.WriteLine("================ / МЕНЮ ЗАВДАНЬ  ================");
            Console.WriteLine("1. Завдання 1: Кількість унікальних значень (PLINQ)");
            Console.WriteLine("2. Завдання 2: Максимальна зростаюча послідовність (PLINQ)");
            Console.WriteLine("3. Завдання 3: Максимальна послідовність додатних чисел (PLINQ)");
            Console.WriteLine("4. Завдання 4: Таблиця множення (Parallel.For)");
            Console.WriteLine("0. Вихід");
            Console.WriteLine("==================================================");
            Console.Write("Оберіть номер завдання (0-4): ");

            string choice = Console.ReadLine();
            Console.WriteLine();

            switch (choice)
            {
                case "1":
                    RunTask1();
                    break;
                case "2":
                    RunTask2();
                    break;
                case "3":
                    RunTask3();
                    break;
                case "4":
                    RunTask4();
                    break;
                case "0":
                    return;
                default:
                    Console.WriteLine("Невірний вибір. Спробуйте ще раз.");
                    break;
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу, щоб повернутися в меню...");
            Console.ReadKey();
        }
    }

    static void CreateSampleFileIfNotExists()
    {
        string filePath = "numbers.txt";
        if (!File.Exists(filePath))
        {
            File.WriteAllLines(filePath, new[] { "1", "2", "8", "-1", "4", "2", "7", "9", "15", "5" });
        }
    }

    static void RunTask1()
    {
        Console.WriteLine("--- Завдання 1: Підрахунок унікальних значень ---");
        string filePath = "numbers.txt";

        List<int> numbers = File.ReadAllLines(filePath)
                              .Select(int.Parse)
                              .ToList();

        int uniqueCount = numbers.AsParallel()
                                 .Distinct()
                                 .Count();

        Console.WriteLine($"Файл містить числа: {string.Join(", ", numbers)}");
        Console.WriteLine($"Кількість унікальних значень: {uniqueCount}");
    }

    static void RunTask2()
    {
        Console.WriteLine("--- Завдання 2: Максимальна зростаюча послідовність ---");
        string filePath = "numbers.txt";

        List<int> numbers = File.ReadAllLines(filePath)
                              .Select(int.Parse)
                              .ToList();

        Console.WriteLine($"Файл містить числа: {string.Join(", ", numbers)}");

        int currentLength = 1;
        int maxLengthSeq = 1;
        int startIndex = 0;
        int bestStartIndex = 0;

        for (int i = 1; i < numbers.Count; i++)
        {
            if (numbers[i] > numbers[i - 1])
            {
                currentLength++;
            }
            else
            {
                if (currentLength > maxLengthSeq)
                {
                    maxLengthSeq = currentLength;
                    bestStartIndex = startIndex;
                }
                currentLength = 1;
                startIndex = i;
            }
        }
        if (currentLength > maxLengthSeq)
        {
            maxLengthSeq = currentLength;
            bestStartIndex = startIndex;
        }

        Console.WriteLine($"Довжина найбільшої зростаючої послідовності: {maxLengthSeq}");
        Console.Write("Послідовність: ");
        for (int i = bestStartIndex; i < bestStartIndex + maxLengthSeq; i++)
        {
            Console.Write(numbers[i] + " ");
        }
        Console.WriteLine();
    }

    static void RunTask3()
    {
        Console.WriteLine("--- Завдання 3: Максимальна послідовність додатних чисел ---");
        string filePath = "numbers.txt";

        List<int> numbers = File.ReadAllLines(filePath)
                              .Select(int.Parse)
                              .ToList();

        Console.WriteLine($"Файл містить числа: {string.Join(", ", numbers)}");

        int maxLen = 0;
        int currentLen = 0;
        int bestStart = 0;
        int currentStart = 0;

        for (int i = 0; i < numbers.Count; i++)
        {
            if (numbers[i] > 0)
            {
                if (currentLen == 0) currentStart = i;
                currentLen++;

                if (currentLen > maxLen)
                {
                    maxLen = currentLen;
                    bestStart = currentStart;
                }
            }
            else
            {
                currentLen = 0;
            }
        }

        Console.WriteLine($"Довжина найбільшої додатної послідовності: {maxLen}");
        Console.Write("Послідовність: ");
        for (int i = bestStart; i < bestStart + maxLen; i++)
        {
            Console.Write(numbers[i] + " ");
        }
        Console.WriteLine();
    }

    static void RunTask4()
    {
        Console.WriteLine("--- Завдання 4: Таблиця множення (Parallel.For) ---");
        Console.Write("Введіть початкове значення діапазону: ");
        if (!int.TryParse(Console.ReadLine(), out int start)) return;

        Console.Write("Введіть кінцеве значення діапазону: ");
        if (!int.TryParse(Console.ReadLine(), out int end)) return;

        string filePath = "multiplication_table.txt";
        ConcurrentBag<string> results = new ConcurrentBag<string>();

        Parallel.For(start, end + 1, i =>
        {
            for (int j = 1; j <= 10; j++)
            {
                results.Add($"{i} * {j} = {i * j}");
            }
            results.Add("-------");
        });

        File.WriteAllLines(filePath, results);

        Console.WriteLine($"\nТаблицю множення успішно записано у файл: {filePath}");
        Console.WriteLine("Перші кілька рядків з файлу:");

        foreach (var line in File.ReadLines(filePath).Take(15))
        {
            Console.WriteLine(line);
        }
        Console.WriteLine("...");
    }
}