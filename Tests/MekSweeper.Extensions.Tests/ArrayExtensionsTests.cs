using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace MekSweeper.Extensions.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class ArrayExtensionsTests
    {
        #region 2D Array

        [TestCase(0, 0, "2,5,6")]
        [TestCase(0, 1, "1,2,6,10,9")]
        [TestCase(0, 2, "5,6,10")]
        [TestCase(1, 0, "1,5,6,7,3")]
        [TestCase(1, 1, "1,2,3,7,11,10,9,5")]
        [TestCase(1, 2, "9,5,6,7,11")]
        [TestCase(2, 0, "2,6,7,8,4")]
        [TestCase(2, 1, "2,3,4,8,12,11,10,6")]
        [TestCase(2, 2, "10,6,7,8,12")]
        [TestCase(3, 0, "3,7,8")]
        [TestCase(3, 1, "4,3,7,11,12")]
        [TestCase(3, 2, "11,7,8")]
        public void GetNeighborsTest_2DArray(int x, int y, string neighborsCsv)
        {
            // 9  10  11  12
            // 5  6   7   8
            // 1  2   3   4
            int[,] array = { { 1, 5, 9 }, { 2, 6, 10 }, { 3, 7, 11 }, { 4, 8, 12 } };

            List<int> expectedNeighbors = ParseNeighbors(neighborsCsv);
            List<int> actualNeighbors = array.GetNeighbors(x, y).ToList();

            AssertNeighbors(expectedNeighbors, actualNeighbors);
        }

        [TestCase(0, 0, true, 5)]
        [TestCase(0, 1, true, 9)]
        [TestCase(0, 2, false, null)]
        [TestCase(1, 0, true, 6)]
        [TestCase(1, 1, true, 10)]
        [TestCase(1, 2, false, null)]
        [TestCase(2, 0, true, 7)]
        [TestCase(2, 1, true, 11)]
        [TestCase(2, 2, false, null)]
        [TestCase(3, 0, true, 8)]
        [TestCase(3, 1, true, 12)]
        [TestCase(3, 2, false, null)]
        public void TryGetTopTest_2DArray(int x, int y, bool shouldSucceed, int? expectedValue)
        {
            // 9  10  11  12
            // 5  6   7   8
            // 1  2   3   4
            int[,] array = { { 1, 5, 9 }, { 2, 6, 10 }, { 3, 7, 11 }, { 4, 8, 12 } };
            bool succeeded = array.TryGetTop(x, y, out var result);

            Assert.AreEqual(shouldSucceed, succeeded);
            if (shouldSucceed)
            {
                Assert.AreEqual(expectedValue, result);
            }
        }

        [TestCase(0, 0, false, null)]
        [TestCase(0, 1, false, null)]
        [TestCase(0, 2, false, null)]
        [TestCase(1, 0, true, 5)]
        [TestCase(1, 1, true, 9)]
        [TestCase(1, 2, false, null)]
        [TestCase(2, 0, true, 6)]
        [TestCase(2, 1, true, 10)]
        [TestCase(2, 2, false, null)]
        [TestCase(3, 0, true, 7)]
        [TestCase(3, 1, true, 11)]
        [TestCase(3, 2, false, null)]
        public void TryGetTopLeftTest_2DArray(int x, int y, bool shouldSucceed, int? expectedValue)
        {
            // 9  10  11  12
            // 5  6   7   8
            // 1  2   3   4
            int[,] array = { { 1, 5, 9 }, { 2, 6, 10 }, { 3, 7, 11 }, { 4, 8, 12 } };
            bool succeeded = array.TryGetTopLeft(x, y, out var result);

            Assert.AreEqual(shouldSucceed, succeeded);
            if (shouldSucceed)
            {
                Assert.AreEqual(expectedValue, result);
            }
        }

        [TestCase(0, 0, true, 6)]
        [TestCase(0, 1, true, 10)]
        [TestCase(0, 2, false, null)]
        [TestCase(1, 0, true, 7)]
        [TestCase(1, 1, true, 11)]
        [TestCase(1, 2, false, null)]
        [TestCase(2, 0, true, 8)]
        [TestCase(2, 1, true, 12)]
        [TestCase(2, 2, false, null)]
        [TestCase(3, 0, false, null)]
        [TestCase(3, 1, false, null)]
        [TestCase(3, 2, false, null)]
        public void TryGetTopRightTest_2DArray(int x, int y, bool shouldSucceed, int? expectedValue)
        {
            // 9  10  11  12
            // 5  6   7   8
            // 1  2   3   4
            int[,] array = { { 1, 5, 9 }, { 2, 6, 10 }, { 3, 7, 11 }, { 4, 8, 12 } };
            bool succeeded = array.TryGetTopRight(x, y, out var result);

            Assert.AreEqual(shouldSucceed, succeeded);
            if (shouldSucceed)
            {
                Assert.AreEqual(expectedValue, result);
            }
        }

        [TestCase(0, 0, false, null)]
        [TestCase(0, 1, false, null)]
        [TestCase(0, 2, false, null)]
        [TestCase(1, 0, true, 1)]
        [TestCase(1, 1, true, 5)]
        [TestCase(1, 2, true, 9)]
        [TestCase(2, 0, true, 2)]
        [TestCase(2, 1, true, 6)]
        [TestCase(2, 2, true, 10)]
        [TestCase(3, 0, true, 3)]
        [TestCase(3, 1, true, 7)]
        [TestCase(3, 2, true, 11)]
        public void TryGetLeftTest_2DArray(int x, int y, bool shouldSucceed, int? expectedValue)
        {
            // 9  10  11  12
            // 5  6   7   8
            // 1  2   3   4
            int[,] array = { { 1, 5, 9 }, { 2, 6, 10 }, { 3, 7, 11 }, { 4, 8, 12 } };
            bool succeeded = array.TryGetLeft(x, y, out var result);

            Assert.AreEqual(shouldSucceed, succeeded);
            if (shouldSucceed)
            {
                Assert.AreEqual(expectedValue, result);
            }
        }

        [TestCase(0, 0, true, 2)]
        [TestCase(0, 1, true, 6)]
        [TestCase(0, 2, true, 10)]
        [TestCase(1, 0, true, 3)]
        [TestCase(1, 1, true, 7)]
        [TestCase(1, 2, true, 11)]
        [TestCase(2, 0, true, 4)]
        [TestCase(2, 1, true, 8)]
        [TestCase(2, 2, true, 12)]
        [TestCase(3, 0, false, null)]
        [TestCase(3, 1, false, null)]
        [TestCase(3, 2, false, null)]
        public void TryGetRightTest_2DArray(int x, int y, bool shouldSucceed, int? expectedValue)
        {
            // 9  10  11  12
            // 5  6   7   8
            // 1  2   3   4
            int[,] array = { { 1, 5, 9 }, { 2, 6, 10 }, { 3, 7, 11 }, { 4, 8, 12 } };
            bool succeeded = array.TryGetRight(x, y, out var result);

            Assert.AreEqual(shouldSucceed, succeeded);
            if (shouldSucceed)
            {
                Assert.AreEqual(expectedValue, result);
            }
        }

        [TestCase(0, 0, false, null)]
        [TestCase(0, 1, true, 1)]
        [TestCase(0, 2, true, 5)]
        [TestCase(1, 0, false, null)]
        [TestCase(1, 1, true, 2)]
        [TestCase(1, 2, true, 6)]
        [TestCase(2, 0, false, null)]
        [TestCase(2, 1, true, 3)]
        [TestCase(2, 2, true, 7)]
        [TestCase(3, 0, false, null)]
        [TestCase(3, 1, true, 4)]
        [TestCase(3, 2, true, 8)]
        public void TryGetBottomTest_2DArray(int x, int y, bool shouldSucceed, int? expectedValue)
        {
            // 9  10  11  12
            // 5  6   7   8
            // 1  2   3   4
            int[,] array = { { 1, 5, 9 }, { 2, 6, 10 }, { 3, 7, 11 }, { 4, 8, 12 } };
            bool succeeded = array.TryGetBottom(x, y, out var result);

            Assert.AreEqual(shouldSucceed, succeeded);
            if (shouldSucceed)
            {
                Assert.AreEqual(expectedValue, result);
            }
        }

        [TestCase(0, 0, false, null)]
        [TestCase(0, 1, false, null)]
        [TestCase(0, 2, false, null)]
        [TestCase(1, 0, false, null)]
        [TestCase(1, 1, true, 1)]
        [TestCase(1, 2, true, 5)]
        [TestCase(2, 0, false, null)]
        [TestCase(2, 1, true, 2)]
        [TestCase(2, 2, true, 6)]
        [TestCase(3, 0, false, null)]
        [TestCase(3, 1, true, 3)]
        [TestCase(3, 2, true, 7)]
        public void TryGetBottomLeftTest_2DArray(int x, int y, bool shouldSucceed, int? expectedValue)
        {
            // 9  10  11  12
            // 5  6   7   8
            // 1  2   3   4
            int[,] array = { { 1, 5, 9 }, { 2, 6, 10 }, { 3, 7, 11 }, { 4, 8, 12 } };
            bool succeeded = array.TryGetBottomLeft(x, y, out var result);

            Assert.AreEqual(shouldSucceed, succeeded);
            if (shouldSucceed)
            {
                Assert.AreEqual(expectedValue, result);
            }
        }

        [TestCase(0, 0, false, null)]
        [TestCase(0, 1, true, 2)]
        [TestCase(0, 2, true, 6)]
        [TestCase(1, 0, false, null)]
        [TestCase(1, 1, true, 3)]
        [TestCase(1, 2, true, 7)]
        [TestCase(2, 0, false, null)]
        [TestCase(2, 1, true, 4)]
        [TestCase(2, 2, true, 8)]
        [TestCase(3, 0, false, null)]
        [TestCase(3, 1, false, null)]
        [TestCase(3, 2, false, null)]
        public void TryGetBottomRightTest_2DArray(int x, int y, bool shouldSucceed, int? expectedValue)
        {
            // 9  10  11  12
            // 5  6   7   8
            // 1  2   3   4
            int[,] array = { { 1, 5, 9 }, { 2, 6, 10 }, { 3, 7, 11 }, { 4, 8, 12 } };
            bool succeeded = array.TryGetBottomRight(x, y, out var result);

            Assert.AreEqual(shouldSucceed, succeeded);
            if (shouldSucceed)
            {
                Assert.AreEqual(expectedValue, result);
            }
        }

        #endregion

        #region List of Lists

        [TestCase(0, 0, "2,5,6")]
        [TestCase(0, 1, "1,2,6,10,9")]
        [TestCase(0, 2, "5,6,10")]
        [TestCase(1, 0, "1,5,6,7,3")]
        [TestCase(1, 1, "1,2,3,7,11,10,9,5")]
        [TestCase(1, 2, "9,5,6,7,11")]
        [TestCase(2, 0, "2,6,7,8,4")]
        [TestCase(2, 1, "2,3,4,8,12,11,10,6")]
        [TestCase(2, 2, "10,6,7,8,12")]
        [TestCase(3, 0, "3,7,8")]
        [TestCase(3, 1, "4,3,7,11,12")]
        [TestCase(3, 2, "11,7,8")]
        public void GetNeighborsTest_ListOfLists(int x, int y, string neighborsCsv)
        {
            // 9  10  11  12
            // 5  6   7   8
            // 1  2   3   4
            var array = new List<List<int>>
            {
                new List<int> {1, 5, 9},
                new List<int> {2, 6, 10},
                new List<int> {3, 7, 11},
                new List<int> {4, 8, 12}
            };

            List<int> expectedNeighbors = ParseNeighbors(neighborsCsv);
            List<int> actualNeighbors = array.GetNeighbors(x, y).ToList();

            AssertNeighbors(expectedNeighbors, actualNeighbors);
        }

        [TestCase(0, 0, true, 5)]
        [TestCase(0, 1, true, 9)]
        [TestCase(0, 2, false, null)]
        [TestCase(1, 0, true, 6)]
        [TestCase(1, 1, true, 10)]
        [TestCase(1, 2, false, null)]
        [TestCase(2, 0, true, 7)]
        [TestCase(2, 1, true, 11)]
        [TestCase(2, 2, false, null)]
        [TestCase(3, 0, true, 8)]
        [TestCase(3, 1, true, 12)]
        [TestCase(3, 2, false, null)]
        public void TryGetTopTest_ListOfLists(int x, int y, bool shouldSucceed, int? expectedValue)
        {
            // 9  10  11  12
            // 5  6   7   8
            // 1  2   3   4
            var array = new List<List<int>>
            {
                new List<int> {1, 5, 9},
                new List<int> {2, 6, 10},
                new List<int> {3, 7, 11},
                new List<int> {4, 8, 12}
            };

            bool succeeded = array.TryGetTop(x, y, out var result);

            Assert.AreEqual(shouldSucceed, succeeded);
            if (shouldSucceed)
            {
                Assert.AreEqual(expectedValue, result);
            }
        }

        [TestCase(0, 0, false, null)]
        [TestCase(0, 1, false, null)]
        [TestCase(0, 2, false, null)]
        [TestCase(1, 0, true, 5)]
        [TestCase(1, 1, true, 9)]
        [TestCase(1, 2, false, null)]
        [TestCase(2, 0, true, 6)]
        [TestCase(2, 1, true, 10)]
        [TestCase(2, 2, false, null)]
        [TestCase(3, 0, true, 7)]
        [TestCase(3, 1, true, 11)]
        [TestCase(3, 2, false, null)]
        public void TryGetTopLeftTest_ListOfLists(int x, int y, bool shouldSucceed, int? expectedValue)
        {
            // 9  10  11  12
            // 5  6   7   8
            // 1  2   3   4
            var array = new List<List<int>>
            {
                new List<int> {1, 5, 9},
                new List<int> {2, 6, 10},
                new List<int> {3, 7, 11},
                new List<int> {4, 8, 12}
            };
            bool succeeded = array.TryGetTopLeft(x, y, out var result);

            Assert.AreEqual(shouldSucceed, succeeded);
            if (shouldSucceed)
            {
                Assert.AreEqual(expectedValue, result);
            }
        }

        [TestCase(0, 0, true, 6)]
        [TestCase(0, 1, true, 10)]
        [TestCase(0, 2, false, null)]
        [TestCase(1, 0, true, 7)]
        [TestCase(1, 1, true, 11)]
        [TestCase(1, 2, false, null)]
        [TestCase(2, 0, true, 8)]
        [TestCase(2, 1, true, 12)]
        [TestCase(2, 2, false, null)]
        [TestCase(3, 0, false, null)]
        [TestCase(3, 1, false, null)]
        [TestCase(3, 2, false, null)]
        public void TryGetTopRightTest_ListOfLists(int x, int y, bool shouldSucceed, int? expectedValue)
        {
            // 9  10  11  12
            // 5  6   7   8
            // 1  2   3   4
            var array = new List<List<int>>
            {
                new List<int> {1, 5, 9},
                new List<int> {2, 6, 10},
                new List<int> {3, 7, 11},
                new List<int> {4, 8, 12}
            };
            bool succeeded = array.TryGetTopRight(x, y, out var result);

            Assert.AreEqual(shouldSucceed, succeeded);
            if (shouldSucceed)
            {
                Assert.AreEqual(expectedValue, result);
            }
        }

        [TestCase(0, 0, false, null)]
        [TestCase(0, 1, false, null)]
        [TestCase(0, 2, false, null)]
        [TestCase(1, 0, true, 1)]
        [TestCase(1, 1, true, 5)]
        [TestCase(1, 2, true, 9)]
        [TestCase(2, 0, true, 2)]
        [TestCase(2, 1, true, 6)]
        [TestCase(2, 2, true, 10)]
        [TestCase(3, 0, true, 3)]
        [TestCase(3, 1, true, 7)]
        [TestCase(3, 2, true, 11)]
        public void TryGetLeftTest_ListOfLists(int x, int y, bool shouldSucceed, int? expectedValue)
        {
            // 9  10  11  12
            // 5  6   7   8
            // 1  2   3   4
            var array = new List<List<int>>
            {
                new List<int> {1, 5, 9},
                new List<int> {2, 6, 10},
                new List<int> {3, 7, 11},
                new List<int> {4, 8, 12}
            };
            bool succeeded = array.TryGetLeft(x, y, out var result);

            Assert.AreEqual(shouldSucceed, succeeded);
            if (shouldSucceed)
            {
                Assert.AreEqual(expectedValue, result);
            }
        }

        [TestCase(0, 0, true, 2)]
        [TestCase(0, 1, true, 6)]
        [TestCase(0, 2, true, 10)]
        [TestCase(1, 0, true, 3)]
        [TestCase(1, 1, true, 7)]
        [TestCase(1, 2, true, 11)]
        [TestCase(2, 0, true, 4)]
        [TestCase(2, 1, true, 8)]
        [TestCase(2, 2, true, 12)]
        [TestCase(3, 0, false, null)]
        [TestCase(3, 1, false, null)]
        [TestCase(3, 2, false, null)]
        public void TryGetRightTest_ListOfLists(int x, int y, bool shouldSucceed, int? expectedValue)
        {
            // 9  10  11  12
            // 5  6   7   8
            // 1  2   3   4
            var array = new List<List<int>>
            {
                new List<int> {1, 5, 9},
                new List<int> {2, 6, 10},
                new List<int> {3, 7, 11},
                new List<int> {4, 8, 12}
            };
            bool succeeded = array.TryGetRight(x, y, out var result);

            Assert.AreEqual(shouldSucceed, succeeded);
            if (shouldSucceed)
            {
                Assert.AreEqual(expectedValue, result);
            }
        }

        [TestCase(0, 0, false, null)]
        [TestCase(0, 1, true, 1)]
        [TestCase(0, 2, true, 5)]
        [TestCase(1, 0, false, null)]
        [TestCase(1, 1, true, 2)]
        [TestCase(1, 2, true, 6)]
        [TestCase(2, 0, false, null)]
        [TestCase(2, 1, true, 3)]
        [TestCase(2, 2, true, 7)]
        [TestCase(3, 0, false, null)]
        [TestCase(3, 1, true, 4)]
        [TestCase(3, 2, true, 8)]
        public void TryGetBottomTest_ListOfLists(int x, int y, bool shouldSucceed, int? expectedValue)
        {
            // 9  10  11  12
            // 5  6   7   8
            // 1  2   3   4
            var array = new List<List<int>>
            {
                new List<int> {1, 5, 9},
                new List<int> {2, 6, 10},
                new List<int> {3, 7, 11},
                new List<int> {4, 8, 12}
            };
            bool succeeded = array.TryGetBottom(x, y, out var result);

            Assert.AreEqual(shouldSucceed, succeeded);
            if (shouldSucceed)
            {
                Assert.AreEqual(expectedValue, result);
            }
        }

        [TestCase(0, 0, false, null)]
        [TestCase(0, 1, false, null)]
        [TestCase(0, 2, false, null)]
        [TestCase(1, 0, false, null)]
        [TestCase(1, 1, true, 1)]
        [TestCase(1, 2, true, 5)]
        [TestCase(2, 0, false, null)]
        [TestCase(2, 1, true, 2)]
        [TestCase(2, 2, true, 6)]
        [TestCase(3, 0, false, null)]
        [TestCase(3, 1, true, 3)]
        [TestCase(3, 2, true, 7)]
        public void TryGetBottomLeftTest_ListOfLists(int x, int y, bool shouldSucceed, int? expectedValue)
        {
            // 9  10  11  12
            // 5  6   7   8
            // 1  2   3   4
            var array = new List<List<int>>
            {
                new List<int> {1, 5, 9},
                new List<int> {2, 6, 10},
                new List<int> {3, 7, 11},
                new List<int> {4, 8, 12}
            };
            bool succeeded = array.TryGetBottomLeft(x, y, out var result);

            Assert.AreEqual(shouldSucceed, succeeded);
            if (shouldSucceed)
            {
                Assert.AreEqual(expectedValue, result);
            }
        }

        [TestCase(0, 0, false, null)]
        [TestCase(0, 1, true, 2)]
        [TestCase(0, 2, true, 6)]
        [TestCase(1, 0, false, null)]
        [TestCase(1, 1, true, 3)]
        [TestCase(1, 2, true, 7)]
        [TestCase(2, 0, false, null)]
        [TestCase(2, 1, true, 4)]
        [TestCase(2, 2, true, 8)]
        [TestCase(3, 0, false, null)]
        [TestCase(3, 1, false, null)]
        [TestCase(3, 2, false, null)]
        public void TryGetBottomRightTest_ListOfLists(int x, int y, bool shouldSucceed, int? expectedValue)
        {
            // 9  10  11  12
            // 5  6   7   8
            // 1  2   3   4
            var array = new List<List<int>>
            {
                new List<int> {1, 5, 9},
                new List<int> {2, 6, 10},
                new List<int> {3, 7, 11},
                new List<int> {4, 8, 12}
            };
            bool succeeded = array.TryGetBottomRight(x, y, out var result);

            Assert.AreEqual(shouldSucceed, succeeded);
            if (shouldSucceed)
            {
                Assert.AreEqual(expectedValue, result);
            }
        }

        #endregion

        /// <summary>
        /// Equal as long as they have the same values, not necessarily in the same order
        /// </summary>
        private void AssertNeighbors(List<int> expectedNeighbors, List<int> actualNeighbors)
        {
            if (expectedNeighbors.Count != actualNeighbors.Count)
            {
                Assert.Fail($"Lists were not equal. Expected: {string.Join(", ", expectedNeighbors)} Actual: {string.Join(", ", actualNeighbors)}");
            }

            expectedNeighbors = expectedNeighbors.OrderBy(x => x).ToList();
            actualNeighbors = actualNeighbors.OrderBy(x => x).ToList();

            for (int index = 0; index < expectedNeighbors.Count; index++)
            {
                if (expectedNeighbors[index] != actualNeighbors[index])
                {
                    Assert.Fail($"Lists were not equal. Expected: {string.Join(", ", expectedNeighbors)} Actual: {string.Join(", ", actualNeighbors)}");
                }
            }

            Assert.Pass();
        }

        private List<int> ParseNeighbors(string neighborsCsv)
        {
            return neighborsCsv.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToList();
        }
    }
}
