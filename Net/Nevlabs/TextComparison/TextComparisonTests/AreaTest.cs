using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextComparison;

namespace TextComparisonTests
{
    [TestClass]
    public class AreaTest
    {
        [TestMethod]
        public void TestIntersect()
        {
            Area area = new Area(5, 5, 5);
            Area other = new Area(20, 20, 5);

            Assert.IsFalse(area.IntersectByPrimary(other));

            other = new Area(1, 1, 2);

            Assert.IsFalse(area.IntersectByPrimary(other));

            other = new Area(1, 1, 4);

            Assert.IsTrue(area.IntersectByPrimary(other));

            other = new Area(1, 1, 6);

            Assert.IsTrue(area.IntersectByPrimary(other));

            other = new Area(8, 8, 6);

            Assert.IsTrue(area.IntersectByPrimary(other));

            other = new Area(1, 1, 15);

            Assert.IsTrue(area.IntersectByPrimary(other));

            other = new Area(6, 6, 3);

            Assert.IsTrue(area.IntersectByPrimary(other));

            Assert.IsTrue(area.IntersectByPrimary(area));
        }
    }
}
