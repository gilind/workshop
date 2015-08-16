using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextComparison;

namespace TextComparisonTests
{
    [TestClass]
    public class TextLineTests
    {
        [TestMethod]
        public void TestTextLineCompare()
        {
            TextLine line1 = new TextLine("abc");
            TextLine line2 = new TextLine("abc");
            Assert.AreEqual(line1.CompareTo(line2), 0);

            line2 = new TextLine("  abc  ");
            Assert.AreEqual(line1.CompareTo(line2), 0);

            line2 = new TextLine(" \t  \t abc \t  \t ");
            Assert.AreEqual(line1.CompareTo(line2), 0);
        }
    }
}