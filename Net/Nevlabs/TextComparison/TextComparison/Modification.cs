using System.Collections.Generic;
using System.Linq;

namespace TextComparison
{
    /// <summary>
    /// Изменение, является областью и содержит тип изменения
    /// </summary>
    //public class Modification : Area//, IComparable
    //{
    //    private class SecondaryComparer : IComparer<Modification>
    //    {
    //        public int Compare(Modification x, Modification y)
    //        {
    //            return x.SecondaryIndex.CompareTo(y.SecondaryIndex);
    //        }
    //    }

    //    private class PrimaryComparer : IComparer<Modification>
    //    {
    //        public int Compare(Modification x, Modification y)
    //        {
    //            return x.PrimaryIndex.CompareTo(y.PrimaryIndex);
    //        }
    //    }

    //    //public static IComparer<Modification> SecondaryIndexComparer
    //    //{
    //    //    get { return new SecondaryComparer(); }
    //    //}

    //    public static IComparer<Modification> PrimaryIndexComparer
    //    {
    //        get { return new PrimaryComparer(); }
    //    }

    //    private const int UnknownIndex = -1;

    //    public ModificationType Type { get; }

    //    //public IList<string> NewLines { get; }// todo: может не потребуется

    //    public bool HasConflict { get; set; }// todo: может не потребуется

    //    protected Modification(ModificationType type, int primaryIndex, int secondaryIndex, int length/*, IList<string> newLines*/)
    //        : base(primaryIndex, secondaryIndex, length)
    //    {
    //        Type = type;
    //        //NewLines = newLines;
    //        HasConflict = false;
    //    }

    //    public static Modification CreateNoChanged(int primaryIndex, int secondaryIndex, int length)
    //    {
    //        return new Modification(ModificationType.NoChanged, primaryIndex, secondaryIndex, length/*, new List<string>()*/);
    //    }

    //    public static Modification CreateNoChanged(Area area)
    //    {
    //        return new Modification(ModificationType.NoChanged, area.PrimaryIndex, area.SecondaryIndex, area.Length/*, new List<string>()*/);
    //    }

    //    public static Modification CreateReplaced(int primaryIndex, int secondaryIndex, int length/*, IList<string> newLines*/)
    //    {
    //        return new Modification(ModificationType.Replaced, primaryIndex, secondaryIndex, length/*, newLines*/);
    //    }

    //    public static Modification CreateRemoved(int primaryIndex, int length)
    //    {
    //        return new Modification(ModificationType.Removed, primaryIndex, UnknownIndex, length/*, new List<string>()*/);
    //    }

    //    public static Modification CreateAdded(int primaryIndex, int secondaryIndex, int length/*, IList<string> newLines*/)
    //    {
    //        return new Modification(ModificationType.Added, primaryIndex, secondaryIndex, length/*, newLines*/);
    //    }

    //    public override string ToString()
    //    {
    //        return string.Format("{0}, ({1}, {2}, {3}), {4}",
    //            Type,
    //            PrimaryIndex,
    //            SecondaryIndex,
    //            Length,
    //            HasConflict);
    //    }

    //    public bool Contains(int index)
    //    {
    //        return PrimaryIndex <= index && (PrimaryIndex + Length) > index;
    //    }

    //    public Modification[] Split(int splitIndex)
    //    {
    //        IList<Modification> result = new List<Modification>();

    //        if (splitIndex < PrimaryIndex ||
    //            splitIndex >= PrimaryIndex + Length)
    //        {
    //            return result.ToArray();
    //        }

    //        if (splitIndex == PrimaryIndex)
    //        {
    //            result.Add(this);
    //            return result.ToArray();
    //        }

    //        result.Add(new Modification(Type, PrimaryIndex, SecondaryIndex, splitIndex - PrimaryIndex));
    //        result.Add(new Modification(Type, splitIndex, SecondaryIndex, PrimaryIndex + Length - splitIndex));

    //        return result.ToArray();
    //    }
    //}
}
