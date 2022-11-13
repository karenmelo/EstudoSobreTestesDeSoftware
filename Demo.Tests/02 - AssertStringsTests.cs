namespace Demo.Tests
{
    public class AssertStringsTests
    {
        [Fact]
        public void StringsTools_UniteNames_ReturnFullName()
        {
            //Arrange
            var sut = new StringsTools();

            //Act
            var fullName = sut.Join("Karen", "Melo");

            //Assert
            Assert.Equal("Karen Melo", fullName);
        }

        [Fact]
        public void StringsTools_UniteNames_MustIgnoreCase()
        {
            //Arrange
            var sut = new StringsTools();

            //Act
            var fullName = sut.Join("Karen", "Melo");

            //Assert
            Assert.Equal("karen melo", fullName, true);
        }

        [Fact]
        public void StringsTools_UniteNames_MustContainExcerpt()
        {
            //Arrange
            var sut = new StringsTools();

            //Act
            var fullName = sut.Join("Karen", "Melo");

            //Assert
            Assert.Contains("aren", fullName);
        }

        [Fact]
        public void StringsTools_UniteNames_MustStartWith()
        {
            //Arrange
            var sut = new StringsTools();

            //Act
            var fullName = sut.Join("Karen", "Melo");

            //Assert
            Assert.StartsWith("Kar", fullName);

        }

        [Fact]
        public void StringsTools_UniteNames_MustEndWith()
        {
            //Arrange
            var sut = new StringsTools();

            //Act
            var fullName = sut.Join("Karen", "Melo");

            //Assert
            Assert.EndsWith("elo", fullName);

        }


        [Fact]
        public void StringsTools_UniteNames_ValidateRegularExpression()
        {
            //Arrange
            var sut = new StringsTools();

            //Act
            var fullName = sut.Join("Karen", "Melo");

            //Assert
            Assert.Matches("[A-Z]{1}[a-z]+ [A-Z]{1}[a-z]+", fullName);

        }
    }
}
