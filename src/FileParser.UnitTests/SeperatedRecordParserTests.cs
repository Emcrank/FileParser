using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NeatParser.UnitTests
{
    [TestClass]
    public class SeperatedRecordParserTests
    {
        private static string InvalidTestData => "Invalid\r\nInvalid";

        private static string BacsTestData => new StringBuilder()
            .AppendLine(@"VOL1623187                           ****999999                                3                                                ")
            .AppendLine(@"HDR1 A999999S   999999                 F     01 18164              18173                                                        ")
            .AppendLine(@"UHL1 18164623187    00000000         000              CLIENTNAME                                                                ")
            .AppendLine(@"6231878014026209960708010025847000000000016216JP157084A DWP SMI 999999-9999999    12345678           27358901509F6F             ")
            .AppendLine(@"6231878014026209960708010025847000000000032428JM071783D DWP SMI 888888-88888-08                      27358901509F70             ")
            .Append(@"UTL28932398748732835                                                                                                            ")
            .ToString();

        private static string VariableTestData => new StringBuilder()
                .AppendLine(@"FILE#0047001A025FAKE DATA - 1234567898451012MOREFAKEDATA")
                .Append(@"FILE#0010001B021FAKE DATA - 123456789006MOREFA")
            .ToString();

        [TestMethod]
        public void Parser_CanSkip_UsingOptions()
        {
            using(var reader = new StringReader(BacsTestData))
            {
                var options = new SeperatedRecordParserOptions
                {
                    RecordSeperator = Environment.NewLine,
                    SkipFirst = 3
                };

                var parser = new SeperatedRecordParser(reader, LayoutFactory.GetBacsDetailLayout(), options);
                Assert.IsTrue(parser.Read());

                string sortCode = parser.Take()[0];
                Assert.AreEqual("623187", sortCode);
            }
        }

        [TestMethod]
        public void Parser_CanReadRecords_FixedWidth()
        {
            using(var reader = new StringReader(BacsTestData))
            {
                var options = new SeperatedRecordParserOptions
                {
                    RecordSeperator = Environment.NewLine,
                    SkipFirst = 3
                };

                var parser = new SeperatedRecordParser(reader, LayoutFactory.GetBacsDetailLayout(), options);
                Assert.IsTrue(parser.Read());

                var values = parser.Take();

                Assert.AreEqual("623187", values["SortCode"]);
                Assert.AreEqual("80140262", values["AccountNumber"]);
                Assert.AreEqual("99", values[2]);
                Assert.AreEqual(16216, values["Amount"]);
                Assert.AreEqual("JP157084A DWP SMI", values["RemittersName"]);
                Assert.AreEqual("999999-9999999", values["RemittersReferenceNumber"]);
            }
        }

        [TestMethod]
        public void Parser_CanSkip_UsingEventHandler()
        {
            using(var reader = new StringReader(BacsTestData))
            {
                var toSkipPrefix = new List<string> {"VOL", "HDR", "UHL"};

                var parser = new SeperatedRecordParser(reader, LayoutFactory.GetBacsDetailLayout());
                parser.OnRecordRead += (s, e) => { e.ShouldSkip = toSkipPrefix.Any(l => e.LineData.StartsWith(l)); };

                Assert.IsTrue(parser.Read());
                Assert.IsTrue(parser.Read());
                var values = parser.Take();

                Assert.AreEqual("623187", values["SortCode"]);
                Assert.AreEqual("80140262", values["AccountNumber"]);
                Assert.AreEqual("99", values[2]);
                Assert.AreEqual(32428, values["Amount"]);
                Assert.AreEqual("JM071783D DWP SMI", values["RemittersName"]);
                Assert.AreEqual("888888-88888-08", values["RemittersReferenceNumber"]);
            }
        }

        [TestMethod]
        public void Parser_CanReadUntilTheEnd()
        {
            using (var reader = new StringReader(BacsTestData))
            {
                var toSkipPrefix = new List<string> { "VOL", "HDR", "UHL", "UTL" };

                var parser = new SeperatedRecordParser(reader, LayoutFactory.GetBacsDetailLayout());
                parser.OnRecordRead += (s, e) => { e.ShouldSkip = toSkipPrefix.Any(l => e.LineData.StartsWith(l)); };
                parser.OnRecordParseError += (s,e) => { throw new InvalidDataException("Failed to parse " + e.LineData, e.Cause); };

                var rowData = new List<RecordValueContainer>();
                Assert.IsTrue(parser.Read());
                rowData.Add(parser.Take());

                Assert.IsTrue(parser.Read());
                rowData.Add(parser.Take());

                var values = rowData[0];
                Assert.AreEqual("623187", values["SortCode"]);
                Assert.AreEqual("80140262", values["AccountNumber"]);
                Assert.AreEqual("99", values[2]);
                Assert.AreEqual(16216, values["Amount"]);
                Assert.AreEqual("JP157084A DWP SMI", values["RemittersName"]);
                Assert.AreEqual("999999-9999999", values["RemittersReferenceNumber"]);

                values = rowData[1];
                Assert.AreEqual("623187", values["SortCode"]);
                Assert.AreEqual("80140262", values["AccountNumber"]);
                Assert.AreEqual("99", values[2]);
                Assert.AreEqual(32428, values["Amount"]);
                Assert.AreEqual("JM071783D DWP SMI", values["RemittersName"]);
                Assert.AreEqual("888888-88888-08", values["RemittersReferenceNumber"]);

                Assert.IsFalse(parser.Read());
            }
        }

        [TestMethod]
        public void Parser_CanReadRecords_MixedWidths()
        {
            using(var reader = new StringReader(VariableTestData))
            {
                var parser = new SeperatedRecordParser(reader, LayoutFactory.GetVariableLayout());
                parser.OnRecordParseError += (s, e) => { throw new InvalidDataException("Error parsing " + e.LineData, e.Cause); };

                Assert.IsTrue(parser.Read());
                var values = parser.Take();
                Assert.AreEqual("FILE#0047", values[0]);
                Assert.AreEqual("A", values[1]);
                Assert.AreEqual("FAKE DATA - 1234567898451", values[2]);
                Assert.AreEqual("MOREFAKEDATA", values[3]);

                Assert.IsTrue(parser.Read());
                values = parser.Take();
                Assert.AreEqual("FILE#0010", values[0]);
                Assert.AreEqual("B", values[1]);
                Assert.AreEqual("FAKE DATA - 123456789", values[2]);
                Assert.AreEqual("MOREFA", values[3]);
            }
        }

        [TestMethod]
        public void Parser_FiresEventWhenRecordErrorExists()
        {
            using (var reader = new StringReader(InvalidTestData))
            {
                var parser = new SeperatedRecordParser(reader, LayoutFactory.GetBacsDetailLayout());

                bool hasFired = false;
                parser.OnRecordParseError += (s, e) =>
                {
                    hasFired = true;
                    Assert.IsNotNull(e.LineData);
                    Assert.IsNotNull(e.Cause);
                };

                Assert.IsTrue(parser.Read());
                Assert.IsTrue(hasFired);
            }
        }

        [TestMethod]
        public void Parser_CompletesFullRead()
        {
            using (var reader = new StringReader(BacsTestData))
            {
                var toSkipPrefix = new List<string> { "VOL", "HDR", "UHL", "UTL" };

                var parser = new SeperatedRecordParser(reader, LayoutFactory.GetBacsDetailLayout());
                parser.OnRecordRead += (s, e) => { e.ShouldSkip = toSkipPrefix.Any(l => e.LineData.StartsWith(l)); };
                parser.OnRecordParseError += (s, e) => { throw new InvalidDataException("Error parsing " + e.LineData, e.Cause); };

                var rowData = new List<RecordValueContainer>();

                while(parser.Read())
                {
                    rowData.Add(parser.Take());
                }

                Assert.AreEqual(2, rowData.Count);
                Assert.AreEqual(6, rowData[0].RecordValues.Count);
                Assert.AreEqual(6, rowData[1].RecordValues.Count);
                
                foreach(var value in rowData[0].RecordValues)
                    Assert.IsNotNull(value, "rowData[0] value != null");

                foreach (var value in rowData[1].RecordValues)
                    Assert.IsNotNull(value, "rowData[1] value != null");
            }
        }
    }
}