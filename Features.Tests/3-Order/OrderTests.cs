namespace Features.Tests
{
    [TestCaseOrderer("Features.Tests.PriorityOrderer", "Features.Tests")]
    public class OrderTests
    {
        public static bool Test1Call;
        public static bool Test2Call;
        public static bool Test3Call;
        public static bool Test4Call;

        [Fact(DisplayName = "Test 4"), TestPriority(3)]
        [Trait("Category", "Test Ordering")]
        public void Test4()
        {
            Test4Call = true;

            Assert.True(Test3Call);
            Assert.True(Test1Call);
            Assert.False(Test2Call);
        }

        [Fact(DisplayName = "Test 1"), TestPriority(2)]
        [Trait("Category", "Test Ordering")]
        public void Test1()
        {
            Test1Call = true;

            Assert.True(Test3Call);
            Assert.False(Test4Call);
            Assert.False(Test2Call);
        }

        [Fact(DisplayName = "Test 3"), TestPriority(1)]
        [Trait("Category", "Test Ordering")]
        public void Test3()
        {
            Test3Call = true;

            Assert.False(Test1Call);
            Assert.False(Test2Call);
            Assert.False(Test4Call);
        }

        [Fact(DisplayName = "Test 2"), TestPriority(4)]
        [Trait("Category", "Test Ordering")]
        public void Test2()
        {
            Test2Call = true;

            Assert.True(Test1Call);
            Assert.True(Test3Call);
            Assert.True(Test4Call);
        }
    }
}
