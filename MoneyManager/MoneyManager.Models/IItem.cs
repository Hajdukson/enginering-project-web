using System;

namespace MoneyManager.Models
{
    public interface IItem
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Price { get; set; }
        public ItemType Type { get; }
    }

    public enum ItemType 
    {
        Income,
        Outcome,
    }
}
