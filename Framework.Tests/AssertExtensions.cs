using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using System.Linq;

namespace Framework.Tests
{
    [DebuggerStepThrough]
    public static class AssertExtensions
    {
        private const string Empty = "";

        public static void ShouldBeEqualTo<T>(this T actual, T expected, string message = Empty)
        {
            Assert.AreEqual(expected, actual, message);
        }

        public static void ShouldBeNotEqualTo<T>(this T actual, T expected, string message = Empty)
        {
            Assert.AreNotEqual(expected, actual, message);
        }

        public static void ShouldBeGreaterThen<T>(this T actual, T expected, string message = Empty) where T : IComparable
        {
            Assert.IsTrue(actual.CompareTo(expected) > 0, message);
        }

        public static void ShouldBeLessThen<T>(this T actual, T expected, string message = Empty) where T : IComparable
        {
            Assert.IsTrue(actual.CompareTo(expected) < 0, message);
        }

        public static void ShouldBeTrue(this bool actual, string message = Empty)
        {
            Assert.IsTrue(actual, message);
        }

        public static void ShouldBeFalse(this bool actual, string message = Empty)
        {
            Assert.IsFalse(actual, message);
        }

        public static void ShouldBeNull<T>(this T actual, string message = Empty)
        {
            Assert.IsNull(actual, message);
        }

        public static void ShouldBeNotNull<T>(this T actual, string message = Empty)
        {
            Assert.IsNotNull(actual, message);
        }

        public static void ShouldBeInstanceOf<TInstance>(this object actual, string message = Empty)
        {
            Assert.IsInstanceOfType(actual, typeof(TInstance), message);
        }

        public static void ShouldBeInstanceOf(this object actual, Type expectedType, string message = Empty)
        {
            Assert.IsInstanceOfType(actual, expectedType, message);
        }

        public static void ShouldBeEqualTo<T>(this ICollection<T> actual, ICollection<T> expected, string message = Empty)
        {
            CollectionAssert.AreEqual((ICollection)expected, (ICollection)actual, message);
        }

        public static void ShouldBeNotEqualTo<T>(this ICollection<T> actual, ICollection<T> expected, string message = Empty)
        {
            CollectionAssert.AreNotEqual((ICollection)expected, (ICollection)actual, message);
        }

        public static void ShouldBeEquivalentTo<T>(this ICollection<T> actual, ICollection<T> expected, string message = Empty)
        {
            CollectionAssert.AreEquivalent((ICollection)expected, (ICollection)actual, message);
        }

        public static void ShouldBeNotEquivalentTo<T>(this ICollection<T> actual, ICollection<T> expected, string message = Empty)
        {
            CollectionAssert.AreNotEquivalent((ICollection)expected, (ICollection)actual, message);
        }

        public static void ShouldContain<T>(this IEnumerable<T> actual, T expected, string message = Empty)
        {
            CollectionAssert.Contains((ICollection)actual, expected, message);
        }

        public static void ShouldContain<T>(this IEnumerable<T> actual, Func<T, bool> condition, string message = Empty)
        {
            ShouldBeNotNull(actual, message);
            actual.Any(condition).ShouldBeTrue(message);
        }

        public static void ShouldNotContain<T>(this IEnumerable<T> actual, T expected, string message = Empty)
        {
            CollectionAssert.DoesNotContain((ICollection)actual, expected, message);
        }

        public static void ShouldNotContain<T>(this IEnumerable<T> actual, Func<T, bool> condition, string message = Empty)
        {
            ShouldBeNotNull(actual, message);
            actual.Any(condition).ShouldBeFalse(message);
        }

        public static void ShouldBeEmpty(this string actual, string message = Empty)
        {
            Assert.AreEqual(string.Empty, actual, message);
        }

        public static void ShouldAllMeetTheCondition<T>(this IEnumerable<T> collection, Func<T, bool> condition)
        {
            var errors = new List<string>();

            foreach (var element in collection)
            {
                if (!condition(element)) errors.Add(element.ToString());
            }

            errors.Count.ShouldBeEqualTo(0, $"Condition failed for elements <{typeof(T)}> :" + string.Join(",", errors));
        }
    }
}