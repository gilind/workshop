using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextComparison.Collections;

namespace TextComparisonTests
{
    [TestClass]
    public class OwnedItemCollectionTest
    {
        private int _count;

        private void ItemRemoved(object sender, EventArgs e)
        {
            _count++;
        }

        [TestMethod]
        public void TestItemRemoveEvent()
        {
            OwnedItemCollection<OwnedItem> numbers = new OwnedItemCollection<OwnedItem>();

            OwnedItem item = new OwnedItem();
            item.Removed += ItemRemoved;

            numbers.Add(item);

            _count = 0;

            item.Remove();

            Assert.AreEqual(_count, 1);
        }

        [TestMethod]
        public void TestCollectionRemoveEvent()
        {
            OwnedItemCollection<OwnedItem> numbers = new OwnedItemCollection<OwnedItem>();

            OwnedItem item = new OwnedItem();
            item.Removed += ItemRemoved;

            numbers.Add(item);

            _count = 0;

            numbers.Remove(item);

            Assert.AreEqual(_count, 1);
        }

        private class MyItem : OwnedItem
        {
            public string Name { get; }

            public MyItem(string name)
            {
                Name = name;
            }
        }

        [TestMethod]
        public void TestNextPreviousItem()
        {
            Assert.AreEqual((new MyItem("")).Index, -1);
            Assert.IsNull((new MyItem("")).Next);
            Assert.IsNull((new MyItem("")).Previous);

            OwnedItemCollection<MyItem> numbers = new OwnedItemCollection<MyItem>();

            numbers.Add(new MyItem("First"));
            numbers.Add(new MyItem("Second"));

            Assert.AreEqual(((MyItem)numbers[0].Next).Name, "Second");

            Assert.IsNull(numbers[0].Previous);

            Assert.AreEqual(((MyItem)numbers[1].Previous).Name, "First");

            Assert.IsNull(numbers[1].Next);
        }
    }
}
