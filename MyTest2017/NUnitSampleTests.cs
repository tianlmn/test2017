using NUnit.Framework;

namespace MyTest2017 {
    [TestFixture]
    public class NUnitSampleTests {
        [Test]
        public void SomePassingTest() {
            Assert.AreEqual(5, 5);
        }

        [Test]
        public void SomeFailingTest() {
            Assert.Greater(5, 7);
        }
    }
}
