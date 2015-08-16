using System;
using System.Collections.Generic;

namespace TextComparison
{
    public class DiffEngine
    {
        private TextFile _primary;
        private TextFile _secondary;

        public DiffEngine()
        {
            _primary = null;
            _secondary = null;
        }

        /// <summary>
        /// Найти длину совпадения.
        /// </summary>
        private int GetPrimaryMatchLength(int primaryIndex, int secondaryIndex, int maxLength)
        {
            int matchCount;
            for (matchCount = 0; matchCount < maxLength; matchCount++)
            {
                if (_secondary[secondaryIndex + matchCount].CompareTo(_primary[primaryIndex + matchCount]) != 0)
                {
                    break;
                }
            }
            return matchCount;
        }

        /// <summary>
        /// Найти наибольшее совпадение.
        /// </summary>
        private Match GetLongestPrimaryMatch(int primaryStart, int primaryEnd, int secondaryStart, int secondaryEnd)
        {
            int maxSecondaryLength = secondaryEnd - secondaryStart + 1;
            int bestIndex = -1;
            int bestLength = 0;
            

            for (int primaryIndex = primaryStart; primaryIndex <= primaryEnd; primaryIndex++)
            {
                var maxLength = Math.Min(maxSecondaryLength, primaryEnd - primaryIndex + 1);

                if (maxLength <= bestLength)
                {
                    break;
                }

                var currentLength = GetPrimaryMatchLength(primaryIndex, secondaryStart, maxLength);

                if (currentLength > bestLength)
                {
                    bestIndex = primaryIndex;
                    bestLength = currentLength;
                }
                
                primaryIndex += bestLength;
            }
            
            return bestIndex == -1 ? null : new Match(bestIndex, bestLength);
        }

        /// <summary>
        /// Обработать диапазоны и найти отличия.
        /// </summary>
        private void ProcessRange(int primaryStart, int primaryEnd, int secondaryStart, int secondaryEnd, IList<Difference> differences)
        {
            int bestIndex = -1;
            int bestLength = -1;
            Match bestMatch = null;

            for (int secondaryIndex = secondaryStart; secondaryIndex <= secondaryEnd; secondaryIndex++)
            {
                var maxPossibleSecondaryLength = (secondaryEnd - secondaryIndex) + 1;

                if (maxPossibleSecondaryLength <= bestLength)
                {
                    break;
                }

                var match = GetLongestPrimaryMatch(primaryStart, primaryEnd, secondaryIndex, secondaryEnd);

                if (match != null)
                {
                    if (match.Length > bestLength)
                    {
                        bestIndex = secondaryIndex;
                        bestLength = match.Length;
                        bestMatch = match;
                    }

                    break;
                }
            }

            if (bestIndex < 0 || bestMatch == null)
            {
                return;
            }

            int primaryIndex = bestMatch.StartIndex;
            differences.Add(Difference.CreateNoChanged(primaryIndex, bestIndex, bestLength));

            if (secondaryStart < bestIndex)
            {
                if (primaryStart < primaryIndex)
                {
                    ProcessRange(primaryStart, primaryIndex - 1, secondaryStart, bestIndex - 1, differences);
                }
            }

            int upperSecondaryStart = bestIndex + bestLength;
            int upperPrimaryStart = primaryIndex + bestLength;
            if (secondaryEnd > upperSecondaryStart)
            {
                if (primaryEnd > upperPrimaryStart)
                {
                    ProcessRange(upperPrimaryStart, primaryEnd, upperSecondaryStart, secondaryEnd, differences);
                }
            }
        }

        public IList<Difference> Compare(TextFile primary, TextFile secondary)
        {
            _primary = primary;
            _secondary = secondary;

            List<Difference> result = new List<Difference>();

            if (_primary.LineCount == 0)
            {
                if (_secondary.LineCount > 0)
                {
                    result.Add(Difference.CreateAdded(0, _secondary.LineCount));
                }

                return result;
            }

            if (_secondary.LineCount == 0)
            {
                if (_primary.LineCount > 0)
                {
                    result.Add(Difference.CreateDeleted(0, _primary.LineCount));
                }

                return result;
            }

            List<Difference> advanceDifferences = new List<Difference>();

            if (_primary.LineCount > 0 && _secondary.LineCount > 0)
            {
                ProcessRange(0, _primary.LineCount - 1, 0, _secondary.LineCount - 1, advanceDifferences);
            }

            advanceDifferences.Sort();

            int curPrimary = 0;
            int curSecondary = 0;
            Difference last = null;

            //Process each match record
            foreach (Difference difference in advanceDifferences)
            {
                if (AddChanges(result, curPrimary, difference.PrimaryIndex, curSecondary, difference.SecondaryIndex) ||
                    last == null)
                {
                    result.Add(difference);
                }
                else
                {
                    last.Length += difference.Length;
                }

                curSecondary = difference.SecondaryIndex + difference.Length;
                curPrimary = difference.PrimaryIndex + difference.Length;
                last = difference;
            }

            //Process any tail end data
            AddChanges(result, curPrimary, _primary.LineCount, curSecondary, _secondary.LineCount);

            return result;
        }

        private bool AddChanges(List<Difference> report, int curPrimary, int nextPrimary, int curSecondary, int nextSecondary)
        {
            bool retval = false;
            int diffSecondary = nextSecondary - curSecondary;
            int diffPrimary = nextPrimary - curPrimary;
            if (diffSecondary > 0)
            {
                if (diffPrimary > 0)
                {
                    var minDiff = Math.Min(diffSecondary, diffPrimary);
                    report.Add(Difference.CreateReplaced(curPrimary, curSecondary, minDiff));
                    if (diffSecondary > diffPrimary)
                    {
                        curSecondary += minDiff;
                        report.Add(Difference.CreateAdded(curSecondary, diffSecondary - diffPrimary));
                    }
                    else
                    {
                        if (diffPrimary > diffSecondary)
                        {
                            curPrimary += minDiff;
                            report.Add(Difference.CreateDeleted(curPrimary, diffPrimary - diffSecondary));
                        }
                    }
                }
                else
                {
                    report.Add(Difference.CreateAdded(curSecondary, diffSecondary));
                }
                retval = true;
            }
            else
            {
                if (diffPrimary > 0)
                {
                    report.Add(Difference.CreateDeleted(curPrimary, diffPrimary));
                    retval = true;
                }
            }
            return retval;
        }
    }
}