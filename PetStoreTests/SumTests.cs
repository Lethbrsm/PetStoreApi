using System;
using PetStore.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace PetStoreTests
{
    [TestClass]
    public class SumTests
    {
        [TestMethod]
        public void SumPrices_ValidAmounts_QuantityOne()
        {
            List<Item> testList = new List<Item>();
            Item item1 = new Item();
            Item item2 = new Item();
            Item item3 = new Item();
            item1.price = 13.89;
            item1.quantity = 1;
            item2.price = 4.30;
            item2.quantity = 1;
            item3.price = 9.99;
            item3.quantity = 1;
            double expected = 28.18;
            testList.Add(item1);
            testList.Add(item2);
            testList.Add(item3);

            double actual = PriceInfo.TotalPrice(testList);

            Assert.AreEqual(expected, actual, 0.001, "Amounts are unequal");
        }
        [TestMethod]
        public void SumPrices_ValidAmounts_QuantityMultiple()
        {
            List<Item> testList = new List<Item>();
            Item item1 = new Item();
            Item item2 = new Item();
            Item item3 = new Item();
            item1.price = 13.89;
            item1.quantity = 73;
            item2.price = 4.30;
            item2.quantity = 5;
            item3.price = 9.99;
            item3.quantity = 900;
            double expected = 10026.47;
            testList.Add(item1);
            testList.Add(item2);
            testList.Add(item3);

            double actual = PriceInfo.TotalPrice(testList);

            Assert.AreEqual(expected, actual, 0.001, "Amounts are unequal");
        }
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SumPrices_ZeroQuantity()
        {
            List<Item> testList = new List<Item>();
            Item item1 = new Item();
            Item item2 = new Item();
            Item item3 = new Item();
            item1.price = 13.89;
            item1.quantity = 73;
            item2.price = 4.30;
            item2.quantity = 5;
            item3.price = 9.99;
            item3.quantity = 0;
            testList.Add(item1);
            testList.Add(item2);
            testList.Add(item3);

            double actual = PriceInfo.TotalPrice(testList);
        }

    }
}
