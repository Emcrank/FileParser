using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NeatParser.UnitTests
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

        [TestMethod]
        public void Parse_ReturnsCorrectDate()
        {
            const string value = "20180807";

            var parsedValue = new DateTimeColumn("yyyyMMdd").Parse(value);

            Assert.AreEqual(new DateTime(2018, 08, 07), parsedValue);
        }

        [TestMethod]
        public void Parse_ReturnsCorrectDateTime()
        {
            const string value = "20201210 17:47:09";

            var parsedValue = new DateTimeColumn("yyyyMMdd HH:mm:ss").Parse(value);

            Assert.AreEqual(new DateTime(2020, 12, 10, 17, 47, 09), parsedValue);
        }
    }
}