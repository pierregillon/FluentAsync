﻿using System;
using System.Collections.Generic;

namespace FluentAsync.Tests.Tasks
{
    public static class MathExtensions
    {
        private static int Partition<T>(this IList<T> list, int start, int end, Random rnd = null) where T : IComparable<T>
        {
            if (rnd != null) {
                list.Swap(end, rnd.Next(start, end + 1));
            }

            var pivot = list[end];
            var lastLow = start - 1;
            for (var i = start; i < end; i++) {
                if (list[i].CompareTo(pivot) <= 0) {
                    list.Swap(i, ++lastLow);
                }
            }

            list.Swap(end, ++lastLow);
            return lastLow;
        }

        /// <summary>
        /// Returns Nth smallest element from the list. Here n starts from 0 so that n=0 returns minimum, n=1 returns 2nd smallest element etc.
        /// Note: specified list would be mutated in the process.
        /// Reference: Introduction to Algorithms 3rd Edition, Corman et al, pp 216
        /// </summary>
        public static T NthOrderStatistic<T>(this IList<T> list, int n, Random rnd = null) where T : IComparable<T> 
            => NthOrderStatistic(list, n, 0, list.Count - 1, rnd);

        private static T NthOrderStatistic<T>(this IList<T> list, int n, int start, int end, Random rnd) where T : IComparable<T>
        {
            while (true) {
                var pivotIndex = list.Partition(start, end, rnd);
                if (pivotIndex == n) {
                    return list[pivotIndex];
                }

                if (n < pivotIndex) {
                    end = pivotIndex - 1;
                }
                else {
                    start = pivotIndex + 1;
                }
            }
        }

        public static void Swap<T>(this IList<T> list, int i, int j)
        {
            if (i == j) //This check is not required but Partition function may make many calls so its for perf reason
            {
                return;
            }

            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

        /// <summary>
        /// Note: specified list would be mutated in the process.
        /// </summary>
        public static T Median<T>(this IList<T> list) where T : IComparable<T> => list.NthOrderStatistic((list.Count - 1) / 2);
    }
}