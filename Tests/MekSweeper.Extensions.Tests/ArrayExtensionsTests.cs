using NUnit.Framework;

namespace MekSweeper.Extensions.Tests
{
    [TestFixture]
    [Parallelizable(ParallelScope.All)]
    public class ArrayExtensionsTests
    {
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
        public void TryGetTopTest(int x, int y, bool shouldSucceed, int? expectedValue)
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
        public void TryGetTopLeftTest(int x, int y, bool shouldSucceed, int? expectedValue)
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
        public void TryGetTopRightTest(int x, int y, bool shouldSucceed, int? expectedValue)
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
        public void TryGetLeftTest(int x, int y, bool shouldSucceed, int? expectedValue)
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
        public void TryGetRightTest(int x, int y, bool shouldSucceed, int? expectedValue)
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
        public void TryGetBottomTest(int x, int y, bool shouldSucceed, int? expectedValue)
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
        public void TryGetBottomLeftTest(int x, int y, bool shouldSucceed, int? expectedValue)
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
        public void TryGetBottomRightTest(int x, int y, bool shouldSucceed, int? expectedValue)
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
    }
}
