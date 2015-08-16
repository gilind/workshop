using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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

            Assert.AreEqual(modification.Primary.StartIndex, 4);
            Assert.AreEqual(modification.Primary.Length, 0);

            Assert.AreEqual(modification.Secondary.StartIndex, 4);
            Assert.AreEqual(modification.Secondary.Length, 3);
        }
    }
}