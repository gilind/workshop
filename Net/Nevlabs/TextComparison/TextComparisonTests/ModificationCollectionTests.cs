using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextComparison;
using TextComparison.Experiments;

namespace TextComparisonTests
{
    [TestClass]
    public class ModificationCollectionTests
    {
        [TestMethod]
        public void TestFindModificationByPrimaryIndex()
        {
            //ModyCollection modifications = new ModyCollection();

            //modifications.Add(Mody.CreateNoChanged(0, 0, 2));
            //modifications.Add(Mody.CreateReplaced(2, 0, 2));
            //modifications.Add(Mody.CreateNoChanged(4, 0, 2));

            //Mody wanted = modifications.FindModificationByPrimaryIndex(1);
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