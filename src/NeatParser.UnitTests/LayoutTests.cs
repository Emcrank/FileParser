using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NeatParser.UnitTests
{
    [TestClass]
    public class LayoutTests
    {
        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void CannotSetDelimiterWhenColumnsHaveAlreadyBeenAdded()
        {
            var layout = new Layout();
            layout.AddColumn(new DummyColumn(), new FixedLengthSpace(1));
            layout.SetDelimiter(",");
        }

        [ExpectedException(typeof(InvalidOperationException))]
        [TestMethod]
        public void CannotSetDelimiterTwice()
        {
            var layout = new Layout();
            layout.SetDelimiter(",");
            layout.SetDelimiter("|");
        }

        [ExpectedException(typeof(ArgumentNullException))]
        [TestMethod]
        public void CannotSetEmptyDelimiter()
        {
            new Layout().SetDelimiter(string.Empty);
        }
    }
}