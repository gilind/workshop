using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TextComparison;

namespace TextComparisonTests
{
    [TestClass]
    public class FileComparerTests
    {
        private static void ModificationAreEqual(
            Modification modification,
            ModificationType modificationType,
            int primaryIndex,
            int secondaryIndex,
            int length)
        {
            Assert.AreEqual(modification.Type, modificationType);
            //Assert.AreEqual(modification.PrimaryIndex, primaryIndex);
            //Assert.AreEqual(modification.SecondaryIndex, secondaryIndex);
            //Assert.AreEqual(modification.Length, length);
        }

        [TestMethod]
        public void TestAdded()
        {
            TextFile file1 = new TextFile();
            file1.AddLine("1");
            file1.AddLine("3");

            TextFile file2 = new TextFile();
            file2.AddLine("1");
            file2.AddLine("2");
            file2.AddLine("3");

            FileComparer fileComparer = new FileComparer();
            IList<Modification> report = fileComparer.Compare(file1, file2);

            ModificationAreEqual(report[0], ModificationType.NoChanged, 0, 0, 1);
            ModificationAreEqual(report[1], ModificationType.Added, -1, 1, 1);
            ModificationAreEqual(report[2], ModificationType.NoChanged, 1, 2, 1);
        }

        [TestMethod]
        public void TestReplaced()
        {
            TextFile file1 = new TextFile();
            file1.AddLine("1");
            file1.AddLine("4");
            file1.AddLine("3");

            TextFile file2 = new TextFile();
            file2.AddLine("1");
            file2.AddLine("2");
            file2.AddLine("3");

            FileComparer fileComparer = new FileComparer();
            IList<Modification> report = fileComparer.Compare(file1, file2);

            ModificationAreEqual(report[0], ModificationType.NoChanged, 0, 0, 1);
            ModificationAreEqual(report[1], ModificationType.Replaced, 1, 1, 1);
            ModificationAreEqual(report[2], ModificationType.NoChanged, 2, 2, 1);
        }

        [TestMethod]
        public void TestReplaced2()
        {
            TextFile file1 = new TextFile();
            file1.AddLine("1");
            file1.AddLine("4");
            file1.AddLine("3");

            TextFile file2 = new TextFile();
            file2.AddLine("2");

            FileComparer fileComparer = new FileComparer();
            IList<Modification> report = fileComparer.Compare(file1, file2);

            ModificationAreEqual(report[0], ModificationType.Replaced, 0, 0, 1);
            ModificationAreEqual(report[1], ModificationType.Removed, 1, -1, 2);
        }

        //[TestMethod]
        //public void TestRemoved()
        //{
        //    TextFile file1 = new TextFile();
        //    file1.AddLine("1");
        //    file1.AddLine("2");
        //    file1.AddLine("3");

        //    TextFile file2 = new TextFile();
        //    //file2.AddLine("1");
        //    //file2.AddLine("3");

        //    FileComparer merger = new FileComparer();
        //    IList<Modification> report = merger.Compare(file1, file2);

        //    ModificationAreEqual(report[0], ModificationType.NoChanged, 0, 0, 1);
        //    ModificationAreEqual(report[1], ModificationType.Added, -1, 1, 1);
        //    ModificationAreEqual(report[2], ModificationType.NoChanged, 1, 2, 1);
        //}

        [TestMethod]
        public void TestMethod1()
        {
            TextFile file1 = new TextFile();
            file1.AddLine("public class MyClass");
            file1.AddLine("{");
            file1.AddLine("    public MyClass()");
            file1.AddLine("    { }");
            file1.AddLine();
            file1.AddLine("    public string StringProperty");
            file1.AddLine("    {");
            file1.AddLine("        get;");
            file1.AddLine("        set;");
            file1.AddLine("    }");
            file1.AddLine("}");

            TextFile file2 = new TextFile();
            file2.AddLine("public class MyClass");
            file2.AddLine("{");
            file2.AddLine("    public MyClass()");
            file2.AddLine("    { }");
            file2.AddLine();
            file2.AddLine("    public int MyMethod(int value)");
            file2.AddLine("    {");
            file2.AddLine("        Console.WriteLine(value);");
            file2.AddLine("    }");
            file2.AddLine("}");

            FileComparer fileComparer = new FileComparer();
            IList<Modification> report = fileComparer.Compare(file1, file2);

            ModificationAreEqual(report[0], ModificationType.NoChanged, 0, 0, 5);
            ModificationAreEqual(report[1], ModificationType.Replaced, 5, 5, 1);
            ModificationAreEqual(report[2], ModificationType.NoChanged, 6, 6, 1);
            ModificationAreEqual(report[3], ModificationType.Replaced, 7, 7, 1);
            ModificationAreEqual(report[4], ModificationType.Removed, 8, -1, 1);
            ModificationAreEqual(report[5], ModificationType.NoChanged, 9, 8, 2);
        }

        //[TestMethod]
        //public void TestMethod2()
        //{
        //    TextFile file1 = new TextFile();
        //    file1.AddLine("1");
        //    file1.AddLine("2");
        //    //file1.AddLine("3");
        //    //file1.AddLine("4");
        //    //file1.AddLine("5");

        //    TextFile file2 = new TextFile();
        //    file2.AddLine("1");
        //    file2.AddLine();
        //    file2.AddLine();
        //    file2.AddLine();
        //    file2.AddLine("2");
        //    //file2.AddLine("3");
        //    //file2.AddLine();
        //    //file2.AddLine("4");
        //    //file2.AddLine("5");
        //    //file2.AddLine();

        //    FileComparer merger = new FileComparer();
        //    IList<Modification> report = merger.Compare(file1, file2);

        //    //ModificationAreEqual(report[0], ModificationType.NoChanged, 0, 0, 1);
        //    //ModificationAreEqual(report[1], ModificationType.Added, -1, 1, 1);
        //    //ModificationAreEqual(report[2], ModificationType.NoChanged, 1, 2, 2);
        //    //ModificationAreEqual(report[3], ModificationType.Added, -1, 4, 1);
        //    //ModificationAreEqual(report[4], ModificationType.NoChanged, 3, 5, 2);
        //    //ModificationAreEqual(report[5], ModificationType.Added, -1, 7, 1);
        //}

        [TestMethod]
        public void TestEmpty()//todo: переименовать
        {
            TextFile emptyFile = new TextFile();

            TextFile file2 = new TextFile();
            file2.AddLine("public class MyClass");
            file2.AddLine("{");
            file2.AddLine("}");

            FileComparer fileComparer = new FileComparer();

            IList<Modification> report = fileComparer.Compare(emptyFile, file2);
            ModificationAreEqual(report[0], ModificationType.Added, -1, 0, 3);

            report = fileComparer.Compare(file2, emptyFile);
            ModificationAreEqual(report[0], ModificationType.Removed, 0, -1, 3);

            report = fileComparer.Compare(emptyFile, emptyFile);
            Assert.AreEqual(report.Count, 0);
        }
    }
}
