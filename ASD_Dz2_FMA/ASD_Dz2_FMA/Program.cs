using ASD_Dz2_FMA;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

class Program
{
    private const int slots = 5;
    private static Shelf shelfA = new Shelf('A', slots);
    private static Shelf shelfB = new Shelf('B', slots);

    private static Journal<PlacedEvent> placedJournal = new Journal<PlacedEvent>();
    private static Journal<TakenEvent> takenJournal = new Journal<TakenEvent>();
    private static Journal<MovedEvent> movedJournal = new Journal<MovedEvent>();

    static void Main()
    {
        LoadJournals();

        while (true)
        {
            Console.WriteLine("\n=== Склад ===");
            shelfA.Display();
            shelfB.Display();

            Console.WriteLine("\n1 - Положить товар");
            Console.WriteLine("2 - Забрать товар");
            Console.WriteLine("3 - Перенести товар");
            Console.WriteLine("4 - Показать журналы");
            Console.WriteLine("5 - Выход");
            Console.Write("Ваш выбор: ");

            string? input = Console.ReadLine();

            if (!int.TryParse(input, out int choice) || choice < 1 || choice > 5)
            {
                Console.WriteLine("Введите число от 1 до 5.");
                continue;
            }

            switch (choice)
            {
                case 1: PlaceItem(); break;
                case 2: TakeItem(); break;
                case 3: MoveItem(); break;
                case 4: ShowJournals(); break;
                case 5:
                    SaveJournals();
                    Console.WriteLine("Журналы сохранены. До свидания!");
                    return;
            }
        }
    }

    private static void PlaceItem()
    {
        Console.Write("Полка (A или B): ");
        string? shelfInput = Console.ReadLine()?.ToUpper();
        if (shelfInput != "A" && shelfInput != "B") { Console.WriteLine("Неверная полка."); return; }
        char shelfChar = shelfInput[0];
        Shelf shelf = shelfChar == 'A' ? shelfA : shelfB;

        Console.Write($"Номер слота (1-{slots}): ");
        if (!int.TryParse(Console.ReadLine(), out int slot) || slot < 1 || slot > slots) { Console.WriteLine("Неверный номер слота."); return; }
        int slotIndex = slot - 1;

        Console.Write("Название товара: ");
        string? itemName = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(itemName)) { Console.WriteLine("Название не может быть пустым."); return; }

        if (!shelf.PlaceItem(slotIndex, itemName!))
        {
            Console.WriteLine("Нельзя положить: слот уже занят.");
            return;
        }

        placedJournal.Add(new PlacedEvent(shelfChar, slot, itemName!));
        Console.WriteLine("Операция выполнена.");
    }

    private static void TakeItem()
    {
        Console.Write("Полка (A или B): ");
        string? shelfInput = Console.ReadLine()?.ToUpper();
        if (shelfInput != "A" && shelfInput != "B") { Console.WriteLine("Неверная полка."); return; }
        char shelfChar = shelfInput[0];
        Shelf shelf = shelfChar == 'A' ? shelfA : shelfB;

        Console.Write($"Номер слота (1-{slots}): ");
        if (!int.TryParse(Console.ReadLine(), out int slot) || slot < 1 || slot > slots) { Console.WriteLine("Неверный номер слота."); return; }
        int slotIndex = slot - 1;

        var item = shelf.TakeItem(slotIndex);
        if (item == null)
        {
            Console.WriteLine("Нельзя забрать: слот пуст.");
            return;
        }

        takenJournal.Add(new TakenEvent(shelfChar, slot, item));
        Console.WriteLine($"Забран товар: {item}");
    }

    private static void MoveItem()
    {
        Console.Write("Полка-источник (A или B): ");
        string? srcShelfInput = Console.ReadLine()?.ToUpper();
        if (srcShelfInput != "A" && srcShelfInput != "B") { Console.WriteLine("Неверная полка."); return; }
        char srcShelfChar = srcShelfInput[0];
        Shelf srcShelf = srcShelfChar == 'A' ? shelfA : shelfB;

        Console.Write($"Слот-источник (1-{slots}): ");
        if (!int.TryParse(Console.ReadLine(), out int srcSlot) || srcSlot < 1 || srcSlot > slots) { Console.WriteLine("Неверный номер слота."); return; }
        int srcSlotIndex = srcSlot - 1;

        Console.Write("Полка-назначение (A или B): ");
        string? destShelfInput = Console.ReadLine()?.ToUpper();
        if (destShelfInput != "A" && destShelfInput != "B") { Console.WriteLine("Неверная полка."); return; }
        char destShelfChar = destShelfInput[0];
        Shelf destShelf = destShelfChar == 'A' ? shelfA : shelfB;

        Console.Write($"Слот-назначение (1-{slots}): ");
        if (!int.TryParse(Console.ReadLine(), out int destSlot) || destSlot < 1 || destSlot > slots) { Console.WriteLine("Неверный номер слота."); return; }
        int destSlotIndex = destSlot - 1;

        var item = srcShelf.GetItem(srcSlotIndex);
        if (item == null) { Console.WriteLine("Нельзя перенести: слот-источник пуст."); return; }
        if (!destShelf.PlaceItem(destSlotIndex, item)) { Console.WriteLine("Нельзя перенести: слот-назначение занят."); return; }

        srcShelf.TakeItem(srcSlotIndex);
        movedJournal.Add(new MovedEvent(srcShelfChar, srcSlot, destShelfChar, destSlot, item));
        Console.WriteLine("Операция выполнена.");
    }

    private static void ShowJournals()
    {
        Console.WriteLine("\n--- Размещения ---");
        foreach (var e in placedJournal.GetAll()) Console.WriteLine(e.ToScreenLine());

        Console.WriteLine("\n--- Изъятия ---");
        foreach (var e in takenJournal.GetAll()) Console.WriteLine(e.ToScreenLine());

        Console.WriteLine("\n--- Переносы ---");
        foreach (var e in movedJournal.GetAll()) Console.WriteLine(e.ToScreenLine());
    }

    private static void SaveJournals()
    {
        SaveJournal("placed.log", placedJournal.GetAll(), e => e.ToLogLine());
    }

    private static void SaveJournal<T>(string path, IEnumerable<T> entries, Func<T, string> toLogLine)
    {
        var lines = new List<string>();
        foreach (var entry in entries) lines.Add(toLogLine(entry));
        File.WriteAllLines(path, lines, Encoding.UTF8);
    }

    private static void LoadJournals()
    {
        if (File.Exists("placed.log"))
        {
            var lines = File.ReadAllLines("placed.log", Encoding.UTF8);
            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                    placedJournal.Add(PlacedEvent.FromLogLine(line));
            }
        }
    }
}