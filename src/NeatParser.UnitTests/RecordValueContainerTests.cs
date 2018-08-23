using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NeatParser.UnitTests
{
    [TestClass]
    public class RecordValueContainerTests
    {
        [TestMethod]
        public void DoesntThrowExceptionWhenValueNotExisting_ColumnName()
        {
            var recordValues = new Dictionary<string, object>
            {
                {"Key1", "Value1"},
                { "Key2", "Value2"}
            };
            var container = new RecordValueContainer(new Layout(), recordValues);

            var retrievedValue = container["Key3"];

            Assert.IsNull(retrievedValue);
        }

        [TestMethod]
        public void DoesntThrowExceptionWhenValueNotExisting_ColumnIndex()
        {
            var recordValues = new Dictionary<string, object>
            {
                {"Key1", "Value1"},
                { "Key2", "Value2"}
            };
            var container = new RecordValueContainer(new Layout(), recordValues);

            var retrievedValue = container[5];

            Assert.IsNull(retrievedValue);
        }

        [TestMethod]
        public void DoesntThrowExceptionWhenValueNotExisting_ColumnIndex2()
        {
            var recordValues = new Dictionary<string, object>
            {
                {"Key1", "Value1"},
                { "Key2", "Value2"}
            };
            var container = new RecordValueContainer(new Layout(), recordValues);

            var retrievedValue = container[2];

            Assert.IsNull(retrievedValue);
        }

        [TestMethod]
        public void ReturnsLayoutName()
        {
            var container = new RecordValueContainer(new Layout("Foobar"), new Dictionary<string, object>());

            Assert.AreEqual("Foobar", container.LayoutName);
        }
    }
}
