using MoneyManager.Services;
using MoneyManager.Services.Interfeces;
using MoneyManager.Models;
using NUnit.Framework;
using System.Collections.Generic;

namespace MoneyManager.Tests
{
    public class CalculationTests
    {
        ICalculator _incomeCalculator;
        List<IItem> _items;

        [SetUp]
        public void SetUp()
        {
            _incomeCalculator = new ExpenseCalculator();

            var incomes = new List<Income> // 364.75
            {
                new Income() {Price = 100.99m},
                new Income() {Price= 70.99m},
                new Income() {Price= 40.99m},
                new Income() {Price= 60.79m},
                new Income() {Price= 90.99m},
            };

            var outcomes = new List<Outcome> // 146.00
            {
                new Outcome() {Price = 20.0m},
                new Outcome() {Price = 111.0m},
                new Outcome() {Price = 10.0m},
                new Outcome() {Price = 5.0m},
            };

            _items = new List<IItem>();
            _items.AddRange(outcomes);
            _items.AddRange(incomes);

            // 364.75 - 146.00 = 218.75
        }
        [Test]
        public void IncomeSumTest()
        {
            var expected = 364.75m;
            var actual = _incomeCalculator.CalculateIncome(_items);

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void OutcomeSumTest()
        {
            var expected = 146.00m;
            var actual = _incomeCalculator.CalculateOutcome(_items);

            Assert.AreEqual(expected, actual);
        }
        [Test]
        public void BalanceTest()
        {
            var expected = 218.75m;
            var actual = _incomeCalculator.CalculateBalance(_items);

            Assert.AreEqual(expected, actual);
        }
    }
}
