using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        // создадим несколько объектов через статический каталог
        try
        {
            LibraryCatalog.AddItem(LibraryCatalog.CreateBook("Clean Code", "Robert C. Martin", 2008));
            LibraryCatalog.AddItem(LibraryCatalog.CreateMagazine("Хакер", 3, 2024));
            LibraryCatalog.AddItem(LibraryCatalog.CreateDvd("Inception", 148, "Sci-Fi"));

            LibraryCatalog.AddItem(LibraryCatalog.CreateBook("CLR via C#", "Jeffrey Richter", 2012));
            LibraryCatalog.AddItem(LibraryCatalog.CreateMagazine("National Geographic", 7, 2023));
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при заполнении каталога: " + ex.Message);
            Pause();
            return;
        }

        while (true)
        {
            Console.Clear();
            Console.WriteLine("Лабораторная 1 (часть 3) — вариант 8 (Библиотека)");
            Console.WriteLine("1) Показать весь каталог");
            Console.WriteLine("2) Найти книгу по автору");
            Console.WriteLine("3) Взять материал (borrow)");
            Console.WriteLine("4) Вернуть материал (return)");
            Console.WriteLine("5) Добавить материал");
            Console.WriteLine("0) Выход");
            Console.Write("Выбор: ");

            string choice = Console.ReadLine() ?? "";

            try
            {
                if (choice == "1")
                {
                    Console.Clear();
                    LibraryCatalog.PrintAll();
                    Pause();
                }
                else if (choice == "2")
                {
                    Console.Clear();
                    Console.Write("Автор: ");
                    string author = (Console.ReadLine() ?? "").Trim();
                    var books = LibraryCatalog.FindBooksByAuthor(author);

                    if (books.Count == 0)
                    {
                        Console.WriteLine("Книги не найдены.");
                    }
                    else
                    {
                        Console.WriteLine("Найдено книг: " + books.Count);
                        foreach (var b in books) Console.WriteLine(b.GetShortInfo());
                    }
                    Pause();
                }
                else if (choice == "3")
                {
                    Console.Clear();
                    Console.Write("Введите ID материала: ");
                    int id = ReadInt(1, int.MaxValue);

                    Console.Write("Кто берет (имя): ");
                    string person = (Console.ReadLine() ?? "").Trim();

                    LibraryCatalog.Borrow(id, person);
                    Console.WriteLine("ОК: материал выдан.");
                    Pause();
                }
                else if (choice == "4")
                {
                    Console.Clear();
                    Console.Write("Введите ID материала: ");
                    int id = ReadInt(1, int.MaxValue);

                    LibraryCatalog.Return(id);
                    Console.WriteLine("ОК: материал возвращен.");
                    Pause();
                }
                else if (choice == "5")
                {
                    AddItemMenu();
                }
                else if (choice == "0")
                {
                    return;
                }
                else
                {
                    Console.WriteLine("Неверный выбор.");
                    Pause();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка: " + ex.Message);
                Pause();
            }
        }
    }

    static void AddItemMenu()
    {
        Console.Clear();
        Console.WriteLine("Добавление материала");
        Console.WriteLine("1) Книга");
        Console.WriteLine("2) Журнал");
        Console.WriteLine("3) DVD");
        Console.WriteLine("0) Назад");
        Console.Write("Выбор: ");
        string ch = Console.ReadLine() ?? "";

        if (ch == "0") return;

        if (ch == "1")
        {
            Console.Write("Название: ");
            string title = (Console.ReadLine() ?? "").Trim();
            Console.Write("Автор: ");
            string author = (Console.ReadLine() ?? "").Trim();
            Console.Write("Год: ");
            int year = ReadInt(0, 3000);

            var item = LibraryCatalog.CreateBook(title, author, year);
            LibraryCatalog.AddItem(item);
            Console.WriteLine("Добавлено: " + item.GetShortInfo());
            Pause();
        }
        else if (ch == "2")
        {
            Console.Write("Название: ");
            string title = (Console.ReadLine() ?? "").Trim();
            Console.Write("Номер выпуска: ");
            int issue = ReadInt(1, 10000);
            Console.Write("Год: ");
            int year = ReadInt(0, 3000);

            var item = LibraryCatalog.CreateMagazine(title, issue, year);
            LibraryCatalog.AddItem(item);
            Console.WriteLine("Добавлено: " + item.GetShortInfo());
            Pause();
        }
        else if (ch == "3")
        {
            Console.Write("Название: ");
            string title = (Console.ReadLine() ?? "").Trim();
            Console.Write("Длительность (мин): ");
            int minutes = ReadInt(1, 10000);
            Console.Write("Жанр: ");
            string genre = (Console.ReadLine() ?? "").Trim();

            var item = LibraryCatalog.CreateDvd(title, minutes, genre);
            LibraryCatalog.AddItem(item);
            Console.WriteLine("Добавлено: " + item.GetShortInfo());
            Pause();
        }
        else
        {
            Console.WriteLine("Неверный выбор.");
            Pause();
        }
    }

    static int ReadInt(int min, int max)
    {
        while (true)
        {
            string s = (Console.ReadLine() ?? "").Trim();
            int v;
            if (int.TryParse(s, out v) && v >= min && v <= max) return v;
            Console.Write("Введите целое число: ");
        }
    }

    static void Pause()
    {
        Console.WriteLine();
        Console.Write("Enter...");
        Console.ReadLine();
    }
}

// -------------------- ИНТЕРФЕЙС --------------------
interface IBorrowable
{
    bool IsBorrowed { get; }
    string Borrower { get; }
    DateTime? BorrowedAt { get; }

    void Borrow(string person);
    void Return();
}

// -------------------- АБСТРАКТНЫЙ КЛАСС --------------------
abstract class LibraryItem
{
    public int Id { get; private set; }
    public string Title { get; private set; }
    public int Year { get; private set; }

    protected LibraryItem(int id, string title, int year)
    {
        if (id <= 0) throw new ArgumentException("Id должен быть > 0");
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Пустое название");
        if (year < 0 || year > 3000) throw new ArgumentException("Некорректный год");

        Id = id;
        Title = title.Trim();
        Year = year;
    }

    public abstract string GetShortInfo();
}

// -------------------- НАСЛЕДНИКИ --------------------
class Book : LibraryItem, IBorrowable
{
    public string Author { get; private set; }

    public bool IsBorrowed { get; private set; }
    public string Borrower { get; private set; } = "";
    public DateTime? BorrowedAt { get; private set; }

    public Book(int id, string title, string author, int year) : base(id, title, year)
    {
        if (string.IsNullOrWhiteSpace(author)) throw new ArgumentException("Пустой автор");
        Author = author.Trim();
    }

    public override string GetShortInfo()
    {
        string status = IsBorrowed ? $"(выдано: {Borrower})" : "(в наличии)";
        return $"[Book] ID={Id}, {Title}, {Author}, {Year} {status}";
    }

    public void Borrow(string person)
    {
        if (IsBorrowed) throw new InvalidOperationException("Уже выдано");
        if (string.IsNullOrWhiteSpace(person)) throw new ArgumentException("Пустое имя");

        IsBorrowed = true;
        Borrower = person.Trim();
        BorrowedAt = DateTime.Now;
    }

    public void Return()
    {
        if (!IsBorrowed) throw new InvalidOperationException("Материал не был выдан");

        IsBorrowed = false;
        Borrower = "";
        BorrowedAt = null;
    }
}

class Magazine : LibraryItem, IBorrowable
{
    public int IssueNumber { get; private set; }

    public bool IsBorrowed { get; private set; }
    public string Borrower { get; private set; } = "";
    public DateTime? BorrowedAt { get; private set; }

    public Magazine(int id, string title, int issueNumber, int year) : base(id, title, year)
    {
        if (issueNumber <= 0) throw new ArgumentException("Номер выпуска должен быть > 0");
        IssueNumber = issueNumber;
    }

    public override string GetShortInfo()
    {
        string status = IsBorrowed ? $"(выдано: {Borrower})" : "(в наличии)";
        return $"[Magazine] ID={Id}, {Title}, №{IssueNumber}, {Year} {status}";
    }

    public void Borrow(string person)
    {
        if (IsBorrowed) throw new InvalidOperationException("Уже выдано");
        if (string.IsNullOrWhiteSpace(person)) throw new ArgumentException("Пустое имя");

        IsBorrowed = true;
        Borrower = person.Trim();
        BorrowedAt = DateTime.Now;
    }

    public void Return()
    {
        if (!IsBorrowed) throw new InvalidOperationException("Материал не был выдан");

        IsBorrowed = false;
        Borrower = "";
        BorrowedAt = null;
    }
}

class DVD : LibraryItem, IBorrowable
{
    public int DurationMinutes { get; private set; }
    public string Genre { get; private set; }

    public bool IsBorrowed { get; private set; }
    public string Borrower { get; private set; } = "";
    public DateTime? BorrowedAt { get; private set; }

    public DVD(int id, string title, int durationMinutes, string genre, int year) : base(id, title, year)
    {
        if (durationMinutes <= 0) throw new ArgumentException("Длительность должна быть > 0");
        if (string.IsNullOrWhiteSpace(genre)) throw new ArgumentException("Пустой жанр");

        DurationMinutes = durationMinutes;
        Genre = genre.Trim();
    }

    public override string GetShortInfo()
    {
        string status = IsBorrowed ? $"(выдано: {Borrower})" : "(в наличии)";
        return $"[DVD] ID={Id}, {Title}, {DurationMinutes} мин, {Genre}, {Year} {status}";
    }

    public void Borrow(string person)
    {
        if (IsBorrowed) throw new InvalidOperationException("Уже выдано");
        if (string.IsNullOrWhiteSpace(person)) throw new ArgumentException("Пустое имя");

        IsBorrowed = true;
        Borrower = person.Trim();
        BorrowedAt = DateTime.Now;
    }

    public void Return()
    {
        if (!IsBorrowed) throw new InvalidOperationException("Материал не был выдан");

        IsBorrowed = false;
        Borrower = "";
        BorrowedAt = null;
    }
}

// -------------------- СТАТИЧЕСКИЙ КЛАСС (каталог/менеджер) --------------------
static class LibraryCatalog
{
    static List<LibraryItem> items = new List<LibraryItem>();
    static int nextId = 1;

    public static void AddItem(LibraryItem item)
    {
        if (item == null) throw new ArgumentNullException("item");

        // запретим одинаковые ID
        foreach (var it in items)
            if (it.Id == item.Id) throw new InvalidOperationException("Дублирование ID");

        items.Add(item);
        if (item.Id >= nextId) nextId = item.Id + 1;
    }

    public static Book CreateBook(string title, string author, int year)
    {
        return new Book(nextId++, title, author, year);
    }

    public static Magazine CreateMagazine(string title, int issueNumber, int year)
    {
        return new Magazine(nextId++, title, issueNumber, year);
    }

    public static DVD CreateDvd(string title, int minutes, string genre)
    {
        // год можно поставить текущий, чтобы не спрашивать отдельно
        int year = DateTime.Now.Year;
        return new DVD(nextId++, title, minutes, genre, year);
    }

    public static void PrintAll()
    {
        if (items.Count == 0)
        {
            Console.WriteLine("Каталог пуст.");
            return;
        }

        foreach (var it in items)
            Console.WriteLine(it.GetShortInfo());
    }

    public static List<Book> FindBooksByAuthor(string author)
    {
        var res = new List<Book>();
        if (string.IsNullOrWhiteSpace(author)) return res;

        string a = author.Trim();

        foreach (var it in items)
        {
            if (it is Book b)
            {
                if (b.Author.IndexOf(a, StringComparison.OrdinalIgnoreCase) >= 0)
                    res.Add(b);
            }
        }

        return res;
    }

    public static void Borrow(int id, string person)
    {
        var it = FindById(id);
        var b = it as IBorrowable;
        if (b == null) throw new InvalidOperationException("Этот материал нельзя выдать");

        b.Borrow(person);
    }

    public static void Return(int id)
    {
        var it = FindById(id);
        var b = it as IBorrowable;
        if (b == null) throw new InvalidOperationException("Этот материал нельзя вернуть");

        b.Return();
    }

    static LibraryItem FindById(int id)
    {
        foreach (var it in items)
            if (it.Id == id) return it;

        throw new KeyNotFoundException("Материал с таким ID не найден");
    }
}
