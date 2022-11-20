namespace Features.Tests._8_Skip
{
    public class TestNotPassingSpecificReason
    {
        [Fact(DisplayName = "New Client 2.0", Skip = "New version 2.0 breaking")]
        [Trait("Category", "Skip the Tests")]
        public void Test_NotPassing_NewVersionNotCompatible()
        {
            Assert.True(false);
        }
    }
}
