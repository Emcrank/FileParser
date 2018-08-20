using FileParser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FileParser.UnitTests
{
    [TestClass]
    public class ColumnDefinitionTests
    {
        [TestMethod]
        public void Parse_DoesntThrowExceptionWhenInvalid()
        {
            const string value = "100";

            Assert.IsFalse((bool)new ColumnDefinition<bool>().Parse(value));
        }

        [TestMethod]
        public void Parse_ParsesCorrectly()
        {
            const string value = "Foobar";

            Assert.AreEqual("Foobar", new ColumnDefinition<string>().Parse(value));
        }

        [TestMethod]
        public void Parse_ParsesCorrectly_False_Lowercase()
        {
            const string value = "false";

            Assert.IsFalse((bool)new ColumnDefinition<bool>().Parse(value));
        }

        [TestMethod]
        public void Parse_ParsesCorrectly_False_Uppercase()
        {
            const string value = "FALSE";

            Assert.IsFalse((bool)new ColumnDefinition<bool>().Parse(value));
        }

        [TestMethod]
        public void Parse_ParsesCorrectly_True_Lowercase()
        {
            const string value = "true";

            Assert.IsTrue((bool)new ColumnDefinition<bool>().Parse(value));
        }

        [TestMethod]
        public void Parse_ParsesCorrectly_True_Uppercase()
        {
            const string value = "TRUE";

            Assert.IsTrue((bool)new ColumnDefinition<bool>().Parse(value));
        }

        [TestMethod()]
        public void Parse_ReturnsNull_OnEmptyString()
        {
            const string value = "";

            Assert.IsNull(new ColumnDefinition<string>().Parse(value));
        }
    }
}