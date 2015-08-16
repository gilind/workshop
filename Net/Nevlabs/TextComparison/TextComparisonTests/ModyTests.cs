using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextComparison;
using TextComparison.Experiments;

namespace TextComparisonTests
{
    [TestClass]
    public class ModyTests
    {
        [TestMethod]
        public void TestStartIndex()
        {
            Mody mody = Mody.CreateNoChanged(new List<string> {"aaa", "bbb", "ccc"});

            Assert.AreEqual(mody.Primary.StartIndex, 0);
            Assert.AreEqual(mody.Secondary.StartIndex, 0);

            ModyCollection modifications = new ModyCollection();

            modifications.Add(mody);

            mody = Mody.CreateAdded(new List<string> { "ddd", "eee", "fff" });

            modifications.Add(mody);

            Assert.AreEqual(mody.Primary.StartIndex, 4);
            Assert.AreEqual(mody.Primary.Length, 0);

            Assert.AreEqual(mody.Secondary.StartIndex, 4);
            Assert.AreEqual(mody.Secondary.Length, 3);
        }
    }
}