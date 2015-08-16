using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextComparison;
using TextComparison.Modifications;

namespace TextComparisonTests
{
    [TestClass]
    public class ModificationCollectionTests
    {
        [TestMethod]
        public void TestFindModificationByPrimaryIndex()
        {
            TextFile primary = new TextFile();
            primary.AddLines(new[] {"a","b","c","d","e","f"});

            TextFile secondary = new TextFile();
            secondary.AddLines(new[] { "1", "2", "3", "4", "5", "6" });

            ModificationCollection modifications = new ModificationCollection(primary, secondary);

            modifications.AddNoChanged(0, 0, 2);
            modifications.AddReplaced(2, 0, 2);
            modifications.AddNoChanged(4, 0, 2);

            Modification wanted = modifications.FindModificationByPrimaryIndex(1);
            Assert.AreEqual(wanted.Index, 0);

            wanted = modifications.FindModificationByPrimaryIndex(3);
            Assert.AreEqual(wanted.Index, 1);

            wanted = modifications.FindModificationByPrimaryIndex(4);
            Assert.AreEqual(wanted.Index, 2);

            wanted = modifications.FindModificationByPrimaryIndex(5);
            Assert.AreEqual(wanted.Index, 2);
        }
    }
}