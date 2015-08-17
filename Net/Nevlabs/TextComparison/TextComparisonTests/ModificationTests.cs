using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextComparison;
using TextComparison.Modifications;

namespace TextComparisonTests
{
    [TestClass]
    public class ModificationTests
    {
        [TestMethod]
        public void TestStartIndex()
        {
            Modification modification = Modification.CreateNoChanged(new List<string> {"aaa", "bbb", "ccc"});

            Assert.AreEqual(modification.Primary.StartIndex, 0);
            Assert.AreEqual(modification.Secondary.StartIndex, 0);

            ModificationCollection modifications = new ModificationCollection();

            modifications.Add(modification);

            modification = Modification.CreateAdded(new List<string> { "ddd", "eee", "fff" });

            modifications.Add(modification);

            Assert.AreEqual(modification.Primary.StartIndex, 3);
            Assert.AreEqual(modification.Primary.Length, 0);
            Assert.AreEqual(modification.Secondary.StartIndex, 3);
            Assert.AreEqual(modification.Secondary.Length, 3);

            modification = Modification.CreateAdded(new List<string> { "ggg", "hhh", "iii" });

            modifications.Add(modification);

            Assert.AreEqual(modification.Primary.StartIndex, 3);
            Assert.AreEqual(modification.Primary.Length, 0);
            Assert.AreEqual(modification.Secondary.StartIndex, 6);
            Assert.AreEqual(modification.Secondary.Length, 3);
        }

        [TestMethod]
        public void TestSplit()
        {
            TextFile primary = new TextFile();
            primary.AddLines(new[] { "a", "b", "c", "d", "e", "f" });

            TextFile secondary = new TextFile();
            secondary.AddLines(new[] { "1", "2", "3", "4", "5", "6" });

            ModificationCollection modifications = new ModificationCollection(primary, secondary);

            modifications.AddNoChanged(0, 2);
            modifications.AddReplaced(2, 0, 2);
            modifications.AddAdded(4, 2);

            Modification[] splited = modifications[1].Split(1); // неверный индекс
            Assert.AreEqual(splited.Length, 1);
            Assert.AreSame(splited[0], modifications[1]);

            splited = modifications[1].Split(2); // граничный индекс
            Assert.AreEqual(splited.Length, 1);
            Assert.AreSame(splited[0], modifications[1]);

            splited = modifications[1].Split(3); // индекс в середине
            Assert.AreEqual(splited.Length, 2);
        }
    }
}