using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NeatParser.UnitTests
{
    [TestClass]
    public class RecordValueContainerTests
    {
        [ExpectedException(typeof(NeatParserException))]
        [TestMethod]
        public void ThrowsExceptionWhenColumnNotDefinedInLayout()
        {
            var layout = new Layout();
            layout.AddColumn(new StringColumn("Key1"), new FixedLengthSpace(1));
            layout.AddColumn(new StringColumn("Key2"), new FixedLengthSpace(1));

            var recordValues = new Dictionary<string, object>
            {
                {"Key1", "Value1"},
                {"Key2", "Value2"}
            };
            var container = new RecordValueContainer(layout, recordValues);

            var retrievedValue = container["Key3"];

            Assert.IsNull(retrievedValue);
        }

        [TestMethod]
        public void DoesntThrowExceptionWhenValueNotExistingButDefined()
        {
            var layout = new Layout();
            layout.AddColumn(new StringColumn("Key1"), new FixedLengthSpace(1));
            layout.AddColumn(new StringColumn("Key2"), new FixedLengthSpace(1));
            layout.AddColumn(new StringColumn("Key3"), new FixedLengthSpace(1));

            var recordValues = new Dictionary<string, object>
            {
                {"Key1", "Value1"},
                {"Key2", "Value2"}
            };
            var container = new RecordValueContainer(layout, recordValues);

            var retrievedValue = container["Key3"];

            Assert.IsNull(retrievedValue);
        }

        [TestMethod]
        public void GetValueReturnsCorrectValue()
        {
            var layout = new Layout();
            layout.AddColumn(new StringColumn("Key1"), new FixedLengthSpace(1));
            layout.AddColumn(new StringColumn("Key2"), new FixedLengthSpace(1));

            var recordValues = new Dictionary<string, object>
            {
                {"Key1", "Value1"},
                {"Key2", 45}
            };
            var container = new RecordValueContainer(layout, recordValues);

            int retrievedValue = container.Get<int>("Key2");

            Assert.AreEqual(45, retrievedValue);
        }

        [TestMethod]
        public void DoesntThrowExceptionWhenValueNullAndNonNullableType()
        {
            var layout = new Layout();
            layout.AddColumn(new StringColumn("Key1"), new FixedLengthSpace(1));
            layout.AddColumn(new StringColumn("Key2"), new FixedLengthSpace(1));

            var recordValues = new Dictionary<string, object>
            {
                {"Key1", "Value1"},
                {"Key2", null}
            };
            var container = new RecordValueContainer(layout, recordValues);

            var retrievedValue = container.Get<DateTime>("Key2");

            Assert.AreEqual(default(DateTime), retrievedValue);
        }

        [TestMethod]
        public void ReturnsLayoutName()
        {
            var container = new RecordValueContainer(new Layout("Foobar"), new Dictionary<string, object>());

            Assert.AreEqual("Foobar", container.LayoutName);
        }

        [TestMethod]
        public void ThrowsExceptionWhenRequiredFieldNotPresent()
        {
            var layout = new Layout();
            layout.AddColumn(new StringColumn("Key1"), new FixedLengthSpace(1));
            layout.AddColumn(new StringColumn("Key2"), new FixedLengthSpace(1));

            var recordValues = new Dictionary<string, object>
            {
                {"Key1", "Value1"},
                {"Key2", 45}
            };
            var container = new RecordValueContainer(layout, recordValues);

            int retrievedValue = container.Get<int>("Key2");

            Assert.AreEqual(45, retrievedValue);
        }
    }
}