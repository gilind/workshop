using System;
using System.Collections.Generic;
using TextComparison.Modifications;

namespace TextComparison
{
    /// <summary>
    /// ���������� ��� ����� � ������� ������ �����������
    /// </summary>
    public class FileComparer
    {
        /// <summary>
        /// ����� ����� ����������.
        /// </summary>
        private static int GetPrimaryMatchLength(TextFile primary, int primaryIndex, TextFile secondary, int secondaryIndex, int maxLength)
        {
            int matchCount;
            for (matchCount = 0; matchCount < maxLength; matchCount++)
            {
                if (secondary[secondaryIndex + matchCount].CompareTo(primary[primaryIndex + matchCount]) != 0)
                {
                    break;
                }
            }
            return matchCount;
        }

        /// <summary>
        /// ����� ���������� ����������.
        /// </summary>
        private Area GetLongestPrimaryMatch(int primaryStart, int primaryEnd, int secondaryStart, int secondaryEnd)
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

                var currentLength = GetPrimaryMatchLength(_primary, primaryIndex, _secondary, secondaryStart, maxLength);

                if (currentLength > bestLength)
                {
                    bestIndex = primaryIndex;
                    bestLength = currentLength;
                }
                
                primaryIndex += bestLength;
            }
            
            return bestIndex == -1 ? null : new Area(bestIndex, secondaryStart, bestLength);
        }

        /// <summary>
        /// ����������� ��������� �������� ��������.
        /// </summary>
        private void DivideIntoAreas(int primaryStart, int primaryEnd, int secondaryStart, int secondaryEnd, IList<Area> areas)
        {
            Area bestMatch = null;

            // todo: ����������� ���������� �� primary
            for (int secondaryIndex = secondaryStart; secondaryIndex <= secondaryEnd; secondaryIndex++)
            {
                var maxPossibleSecondaryLength = secondaryEnd - secondaryIndex + 1;

                if (bestMatch != null && maxPossibleSecondaryLength <= bestMatch.Length)
                {
                    break;
                }

                var match = GetLongestPrimaryMatch(primaryStart, primaryEnd, secondaryIndex, secondaryEnd);

                if (match == null)
                {
                    continue;
                }

                if (bestMatch == null)
                {
                    bestMatch = match;
                }
                else
                {
                    if (match.Length > bestMatch.Length)
                    {
                        bestMatch = match;
                    }
                }
            }

            if (bestMatch == null)
            {
                return;
            }

            areas.Add(bestMatch);

            if (secondaryStart < bestMatch.SecondaryIndex && primaryStart < bestMatch.PrimaryIndex)
            {
                DivideIntoAreas(primaryStart, bestMatch.PrimaryIndex - 1, secondaryStart, bestMatch.SecondaryIndex - 1, areas);
            }

            int upperSecondaryStart = bestMatch.SecondaryIndex + bestMatch.Length;
            int upperPrimaryStart = bestMatch.PrimaryIndex + bestMatch.Length;

            if (secondaryEnd > upperSecondaryStart && primaryEnd >= upperPrimaryStart)
            {
                DivideIntoAreas(upperPrimaryStart, primaryEnd, upperSecondaryStart, secondaryEnd, areas);
            }
        }

        private bool DetermineModification(int primaryStart, int primaryEnd, int secondaryStart, int secondaryEnd, ModificationCollection modifications)
        {
            bool result = false;
            int primaryLength = primaryEnd - primaryStart;
            int secondaryLength = secondaryEnd - secondaryStart;

            if (secondaryLength > 0)
            {
                if (primaryLength > 0)
                {
                    var minLength = Math.Min(secondaryLength, primaryLength);
                    modifications.AddReplaced(primaryStart, secondaryStart, minLength);

                    if (secondaryLength > primaryLength)
                    {
                        secondaryStart += minLength;
                        modifications.AddAdded(secondaryStart, secondaryLength - primaryLength);
                    }
                    else
                    {
                        if (primaryLength > secondaryLength)
                        {
                            primaryStart += minLength;
                            modifications.AddRemoved(primaryStart, primaryLength - secondaryLength);
                        }
                    }
                }
                else
                {
                    modifications.AddAdded(secondaryStart, secondaryLength);
                }
                result = true;
            }
            else
            {
                if (primaryLength > 0)
                {
                    modifications.AddRemoved(primaryStart, primaryLength);
                    result = true;
                }
            }
            return result;
        }

        private TextFile _primary;
        private TextFile _secondary;

        public ModificationCollection Compare(TextFile primary, TextFile secondary)
        {
            _primary = primary;
            _secondary = secondary;

            ModificationCollection result = new ModificationCollection(primary, secondary);

            if (primary.LineCount == 0 && secondary.LineCount == 0)
            {
                // ��� ������ �����
                _primary = null;
                _secondary = null;
                return result;
            }

            if (primary.LineCount == 0)
            {
                if (secondary.LineCount > 0)
                {
                    // ������ ���� ������, � ������ ����� ������
                    result.AddAdded(0, secondary.LineCount);
                }

                _primary = null;
                _secondary = null;
                return result;
            }

            if (secondary.LineCount == 0)
            {
                if (primary.LineCount > 0)
                {
                    // ������ ���� ����� ������, � ������ ������
                    result.AddRemoved(0, primary.LineCount);
                }

                _primary = null;
                _secondary = null;
                return result;
            }

            List<Area> areas = new List<Area>();

            DivideIntoAreas(0, primary.LineCount - 1, 0, secondary.LineCount - 1, areas);

            areas.Sort(Area.SecondaryIndexComparer);

            int primaryIndex = 0;
            int secondaryIndex = 0;
            Area last = null;

            foreach (Area area in areas)
            {
                if (DetermineModification(primaryIndex, area.PrimaryIndex, secondaryIndex, area.SecondaryIndex, result) ||
                    last == null)
                {
                    result.AddNoChanged(area.PrimaryIndex, area.SecondaryIndex, area.Length);
                }

                primaryIndex = area.PrimaryIndex + area.Length;
                secondaryIndex = area.SecondaryIndex + area.Length;
                last = area;
            }

            // ���� ����� ������ ��������, ����� ����� ���������� ������
            // ��� ��������� ��������, �������� ����������� ���
            DetermineModification(primaryIndex, primary.LineCount, secondaryIndex, secondary.LineCount, result);

            return result;
        }
    }
}