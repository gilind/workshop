using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextComparison;

namespace TextComparisonTests
{
    [TestClass]
    public class ModificationCollectionTests
    {
        [TestMethod]
        public void TestFindModificationByPrimaryIndex()
        {
            //ModificationCollection modifications = new ModificationCollection();

            //modifications.Add(Modification.CreateNoChanged(0, 0, 2));
            //modifications.Add(Modification.CreateReplaced(2, 0, 2));
            //modifications.Add(Modification.CreateNoChanged(4, 0, 2));

            //Modification wanted = modifications.FindModificationByPrimaryIndex(1);
            //Assert.AreEqual(wanted.Index, 0);

            //wanted = modifications.FindModificationByPrimaryIndex(3);
            //Assert.AreEqual(wanted.Index, 1);

            //wanted = modifications.FindModificationByPrimaryIndex(4);
            //Assert.AreEqual(wanted.Index, 2);

            //wanted = modifications.FindModificationByPrimaryIndex(5);
            //Assert.AreEqual(wanted.Index, 2);
        }
    }
}