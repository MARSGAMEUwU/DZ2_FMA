using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASD_Dz2_FMA
{
    public class Shelf
    {
        public char Name { get; }
        private readonly string?[] slots;

        public Shelf(char name, int slotCount)
        {
            Name = name;
            slots = new string?[slotCount];
        }

        public int SlotCount => slots.Length;

        public bool IsSlotEmpty(int slotIndex) => slots[slotIndex] == null;

        public string? GetItem(int slotIndex) => slots[slotIndex];

        public bool PlaceItem(int slotIndex, string itemName)
        {
            if (slots[slotIndex] != null)
                return false;
            slots[slotIndex] = itemName;
            return true;
        }

        public string? TakeItem(int slotIndex)
        {
            var item = slots[slotIndex];
            slots[slotIndex] = null;
            return item;
        }

        public void Display()
        {
            Console.Write($"Полка {Name}: ");
            for (int i = 0; i < slots.Length; i++)
            {
                var content = slots[i] ?? "пусто";
                Console.Write($"[{i + 1}] {content}  ");
            }
            Console.WriteLine();
        }
    }
}
