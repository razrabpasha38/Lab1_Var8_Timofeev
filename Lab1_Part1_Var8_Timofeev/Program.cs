using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Лабораторная 1 (часть 1) — вариант 8");
            Console.WriteLine("1) Задание 1: прибыль (массив + файл)");
            Console.WriteLine("2) Задание 2: инвентаризация (List + Dictionary + файл)");
            Console.WriteLine("0) Выход");
            Console.Write("Выбор: ");
            string choice = Console.ReadLine() ?? "";

            if (choice == "1") Task1();
            else if (choice == "2") Task2();
            else if (choice == "0") return;
            else Pause("Неверный выбор.");
        }
    }

    // ---------- ЗАДАНИЕ 1 (вариант 8): прибыль ----------
    // Массив прибыли за несколько лет + средняя прибыль и средний темп изменения (%)
    static void Task1()
    {
        const string fileName = "profits.txt";
        double[] profits = new double[0];

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Задание 1: Расчет нормы прибыли");
            Console.WriteLine("Файл: " + fileName);
            Console.WriteLine("1) Ввести данные");
            Console.WriteLine("2) Загрузить из файла");
            Console.WriteLine("3) Показать расчет");
            Console.WriteLine("4) Сохранить в файл");
            Console.WriteLine("0) Назад");
            Console.Write("Выбор: ");
            string choice = Console.ReadLine() ?? "";

            try
            {
                if (choice == "1")
                {
                    profits = InputProfits();
                    Pause("Данные введены.");
                }
                else if (choice == "2")
                {
                    profits = LoadProfits(fileName);
                    Pause("Загружено значений: " + profits.Length);
                }
                else if (choice == "3")
                {
                    ShowProfitCalc(profits);
                }
                else if (choice == "4")
                {
                    SaveProfits(fileName, profits);
                    Pause("Сохранено.");
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

    static double[] InputProfits()
    {
        int n = ReadInt("Сколько лет (кол-во значений)? ", 1, 100);
        double[] arr = new double[n];

        for (int i = 0; i < n; i++)
        {
            arr[i] = ReadDouble("Прибыль за год #" + (i + 1) + ": ");
        }
        return arr;
    }

    static void ShowProfitCalc(double[] profits)
    {
        Console.Clear();

        if (profits == null || profits.Length == 0)
        {
            Pause("Нет данных. Сначала введите или загрузите.");
            return;
        }

        Console.WriteLine("Прибыль по годам:");
        for (int i = 0; i < profits.Length; i++)
            Console.WriteLine("Год #" + (i + 1) + ": " + profits[i]);

        double sum = 0;
        for (int i = 0; i < profits.Length; i++)
            sum += profits[i];

        double avgProfit = sum / profits.Length;

        // Средний темп изменения (% к предыдущему году)
        // Если предыдущий год = 0, изменение не считаем
        double sumRates = 0;
        int cntRates = 0;

        if (profits.Length >= 2)
        {
            for (int i = 1; i < profits.Length; i++)
            {
                double prev = profits[i - 1];
                if (Math.Abs(prev) < 0.0000001) continue;

                double rate = (profits[i] - prev) / prev * 100.0;
                sumRates += rate;
                cntRates++;
            }
        }

        Console.WriteLine();
        Console.WriteLine("Средняя прибыль: " + avgProfit.ToString("F2"));

        if (cntRates > 0)
        {
            double avgRate = sumRates / cntRates;
            Console.WriteLine("Средний темп изменения прибыли: " + avgRate.ToString("F2") + "%");
        }
        else
        {
            Console.WriteLine("Средний темп изменения прибыли: недостаточно данных.");
        }

        Pause("Готово.");
    }

    static double[] LoadProfits(string fileName)
    {
        if (!File.Exists(fileName))
            throw new FileNotFoundException("Файл не найден: " + fileName);

        string[] lines = File.ReadAllLines(fileName);
        List<double> values = new List<double>();

        foreach (string raw in lines)
        {
            string s = (raw ?? "").Trim();
            if (s.Length == 0) continue;

            s = s.Replace(',', '.'); // на всякий
            double v;
            if (!double.TryParse(s, System.Globalization.NumberStyles.Float,
                                 System.Globalization.CultureInfo.InvariantCulture, out v))
                throw new FormatException("Некорректное число в файле: " + raw);

            values.Add(v);
        }

        return values.ToArray();
    }

    static void SaveProfits(string fileName, double[] profits)
    {
        if (profits == null || profits.Length == 0)
            throw new InvalidOperationException("Нет данных для сохранения.");

        string[] lines = new string[profits.Length];
        for (int i = 0; i < profits.Length; i++)
            lines[i] = profits[i].ToString(System.Globalization.CultureInfo.InvariantCulture);

        File.WriteAllLines(fileName, lines);
    }

    // ---------- ЗАДАНИЕ 2 (вариант 8): инвентаризация ----------
    // List оборудования + Dictionary(name -> state) + подсчет по состояниям + файл
    static void Task2()
    {
        const string fileName = "equipment.txt";

        List<string> items = new List<string>();
        Dictionary<string, string> states = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        // Попытка автозагрузки
        try
        {
            if (File.Exists(fileName))
                LoadEquipment(fileName, items, states);
        }
        catch
        {
            // если файл кривой — просто начнем с пустого
            items.Clear();
            states.Clear();
        }

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Задание 2: Инвентаризация оборудования");
            Console.WriteLine("Файл: " + fileName);
            Console.WriteLine("1) Добавить оборудование");
            Console.WriteLine("2) Изменить состояние");
            Console.WriteLine("3) Показать список");
            Console.WriteLine("4) Подсчет по состояниям");
            Console.WriteLine("5) Сохранить в файл");
            Console.WriteLine("6) Загрузить из файла (перезаписать)");
            Console.WriteLine("0) Назад");
            Console.Write("Выбор: ");
            string choice = Console.ReadLine() ?? "";

            try
            {
                if (choice == "1") AddItem(items, states);
                else if (choice == "2") UpdateState(items, states);
                else if (choice == "3") ShowItems(items, states);
                else if (choice == "4") CountStates(items, states);
                else if (choice == "5") { SaveEquipment(fileName, items, states); Pause("Сохранено."); }
                else if (choice == "6")
                {
                    items.Clear();
                    states.Clear();
                    LoadEquipment(fileName, items, states);
                    Pause("Загружено.");
                }
                else if (choice == "0") return;
                else Pause("Неверный выбор.");
            }
            catch (Exception ex)
            {
                Pause("Ошибка: " + ex.Message);
            }
        }
    }

    static void AddItem(List<string> items, Dictionary<string, string> states)
    {
        Console.Clear();
        Console.Write("Название оборудования: ");
        string name = (Console.ReadLine() ?? "").Trim();
        if (name.Length == 0)
        {
            Pause("Пустое название.");
            return;
        }

        if (states.ContainsKey(name))
        {
            Pause("Такое оборудование уже есть.");
            return;
        }

        Console.Write("Состояние (например: в работе / требует ремонта / на складе): ");
        string state = (Console.ReadLine() ?? "").Trim();
        if (state.Length == 0)
        {
            Pause("Пустое состояние.");
            return;
        }

        items.Add(name);
        states[name] = state;

        Pause("Добавлено.");
    }

    static void UpdateState(List<string> items, Dictionary<string, string> states)
    {
        Console.Clear();
        if (items.Count == 0)
        {
            Pause("Список пуст.");
            return;
        }

        ShowItems(items, states, false);

        Console.Write("Введите название оборудования: ");
        string name = (Console.ReadLine() ?? "").Trim();

        if (!states.ContainsKey(name))
        {
            Pause("Не найдено.");
            return;
        }

        Console.Write("Новое состояние: ");
        string state = (Console.ReadLine() ?? "").Trim();
        if (state.Length == 0)
        {
            Pause("Пустое состояние.");
            return;
        }

        states[name] = state;
        Pause("Изменено.");
    }

    static void ShowItems(List<string> items, Dictionary<string, string> states, bool pause = true)
    {
        Console.Clear();
        if (items.Count == 0)
        {
            if (pause) Pause("Список пуст.");
            return;
        }

        Console.WriteLine("Оборудование:");
        for (int i = 0; i < items.Count; i++)
        {
            string name = items[i];
            string state = states.ContainsKey(name) ? states[name] : "неизвестно";
            Console.WriteLine((i + 1) + ") " + name + " — " + state);
        }

        if (pause) Pause("Показано.");
    }

    static void CountStates(List<string> items, Dictionary<string, string> states)
    {
        Console.Clear();
        if (items.Count == 0)
        {
            Pause("Список пуст.");
            return;
        }

        Dictionary<string, int> count = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        foreach (string name in items)
        {
            string st = states.ContainsKey(name) ? states[name] : "неизвестно";
            if (!count.ContainsKey(st)) count[st] = 0;
            count[st]++;
        }

        Console.WriteLine("Количество по состояниям:");
        foreach (var kv in count)
            Console.WriteLine(kv.Key + ": " + kv.Value);

        Pause("Готово.");
    }

    // Формат файла: одна строка = "название;состояние"
    static void SaveEquipment(string fileName, List<string> items, Dictionary<string, string> states)
    {
        if (items.Count == 0)
            throw new InvalidOperationException("Нет данных для сохранения.");

        string[] lines = new string[items.Count];
        for (int i = 0; i < items.Count; i++)
        {
            string name = items[i];
            string state = states.ContainsKey(name) ? states[name] : "";

            // Чтобы не ломать разделитель:
            name = name.Replace(";", ",");
            state = state.Replace(";", ",");

            lines[i] = name + ";" + state;
        }

        File.WriteAllLines(fileName, lines);
    }

    static void LoadEquipment(string fileName, List<string> items, Dictionary<string, string> states)
    {
        if (!File.Exists(fileName))
            throw new FileNotFoundException("Файл не найден: " + fileName);

        string[] lines = File.ReadAllLines(fileName);

        foreach (string raw in lines)
        {
            string s = (raw ?? "").Trim();
            if (s.Length == 0) continue;

            string[] parts = s.Split(';');
            if (parts.Length < 2) throw new FormatException("Некорректная строка: " + raw);

            string name = (parts[0] ?? "").Trim();
            string state = (parts[1] ?? "").Trim();

            if (name.Length == 0) continue;

            if (!states.ContainsKey(name))
                items.Add(name);

            if (state.Length == 0) state = "неизвестно";
            states[name] = state;
        }
    }

    // ---------- Утилиты ----------
    static int ReadInt(string prompt, int min, int max)
    {
        while (true)
        {
            Console.Write(prompt);
            string s = (Console.ReadLine() ?? "").Trim();
            int v;
            if (int.TryParse(s, out v) && v >= min && v <= max)
                return v;

            Console.WriteLine("Введите целое число от " + min + " до " + max + ".");
        }
    }

    static double ReadDouble(string prompt)
    {
        while (true)
        {
            Console.Write(prompt);
            string s = (Console.ReadLine() ?? "").Trim().Replace(',', '.');

            double v;
            if (double.TryParse(s, System.Globalization.NumberStyles.Float,
                                System.Globalization.CultureInfo.InvariantCulture, out v))
                return v;

            Console.WriteLine("Введите число (например 123.45).");
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
