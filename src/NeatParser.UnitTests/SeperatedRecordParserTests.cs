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

        private static string DelimitedTestDataNoTrailingDelimiter =>
            @"TESTDATA 1COLUMN1,1COLUMN2,1COLUMN3,1COLUMN4,1COLUMN5 2COLUMN1,2COLUMN2,2COLUMN3,2COLUMN4,2COLUMN5";

        private static string DelimitedTestDataWithTrailingDelimiter =>
            @"TESTDATA 1COLUMN1,1COLUMN2,1COLUMN3,1COLUMN4,1COLUMN5, 2COLUMN1,2COLUMN2,2COLUMN3,2COLUMN4,2COLUMN5,";

        private static string BacsTestData => new StringBuilder()
            .AppendLine(
                @"VOL1623187                           ****999999                                3                                                ")
            .AppendLine(
                @"HDR1 A999999S   999999                 F     01 18164              18173                                                        ")
            .AppendLine(
                @"UHL1 18164623187    00000000         000              CLIENTNAME                                                                ")
            .AppendLine(
                @"6231878014026209960708010025847000000000016216JP157084A DWP SMI 999999-9999999    12345678           27358901509F6F             ")
            .AppendLine(
                @"6231878014026209960708010025847000000000032428JM071783D DWP SMI 888888-88888-08                      27358901509F70             ")
            .Append(
                @"UTL28932398748732835                                                                                                            ")
            .ToString();

        private static string VariableTestData => new StringBuilder()
            .AppendLine(@"FILE#0047001A025FAKE DATA - 1234567898451012MOREFAKEDATA")
            .Append(@"FILE#0010001B021FAKE DATA - 123456789006MOREFA")
            .ToString();

        private static string LayoutEditorTestData =>
            @"A412002B2060200000000002600015013010100000000000540020180807201808061003184000000003827894950710000040800000199110799     080045603682606134001110799     080100000607S YZXTH018XXXXYYYY & XILJLPK01017469176080000000000000000|A412002B2060200000000002600015013010100000000001250020180807201808061003186000000003777399610710000040800027303309912     080023097582606134001309912     080100000606E POIU016XYXYXYXY ZZZXXXX0131315838910-020000000000000000|A412002B20602004000000026000154130101000000000011030201808072018080610031800156280632BBFHDCF0710000110832315907090128     085687823882601800156280632BBFHDCF06134013090128     080100000617POIUY CONSTERDINE012BBG MORT A/C012013/32315907017000000000000000000000000000000000|A412002B2060200000000002600015013010100000000000632220180807201808061003181000000003763492230710000040898401510309233     080266130182606134013309233     080100000610J A YUUOIU018XYXYXYXY & ZZZXXXX009JA YUUOIU0000000000000000";

        private static string LayoutEditorIntegerTestData => @"7TT00BA";

        private static string DelimitedTestData => new StringBuilder()
            .AppendLine(@"HEADER----------------ROW")
            .AppendLine(@"  TEST DATA 47|  TEST DATA 24|  TEST DATA 12|")
            .AppendLine(@"  TEST DATA 14|  TEST DATA 11|  TEST DATA 06|")
            .ToString();

        [TestMethod]
        public void Parse_CanReadDelimitedData()
        {
            using(var reader = new StringReader(DelimitedTestData))
            {
                var options = new NeatParserOptions {SkipFirst = 1};
                var layout = new Layout("testLayout", "|");
                layout.AddColumn(new StringColumn("1") {TrimOption = TrimOptions.None}, new FixedLengthSpace(14));
                layout.AddColumn(new StringColumn("2") {TrimOption = TrimOptions.None}, new FixedLengthSpace(14));
                layout.AddColumn(new StringColumn("3") {TrimOption = TrimOptions.None}, new FixedLengthSpace(14));

                var parser = new NeatParser(reader, layout, options);
                var values = parser.TakeNext();

                Assert.AreEqual("  TEST DATA 47", values["1"]);
                Assert.AreEqual("  TEST DATA 24", values["2"]);
                Assert.AreEqual("  TEST DATA 12", values["3"]);

                values = parser.TakeNext();

                Assert.AreEqual("  TEST DATA 14", values["1"]);
                Assert.AreEqual("  TEST DATA 11", values["2"]);
                Assert.AreEqual("  TEST DATA 06", values["3"]);

                Assert.IsNull(parser.TakeNext());
            }
        }

        [TestMethod]
        public void Parse_CanReadDelimitedData_WithoutTrailingDelimiter()
        {
            using(var reader = new StringReader(DelimitedTestDataNoTrailingDelimiter))
            {
                var options = new NeatParserOptions {RecordSeperator = " ", SkipFirst = 1};
                var layout = LayoutFactory.GetDelimitedLayout();
                layout.RecordsHaveTrailingDelimiter = false;

                var parser = new NeatParser(reader, layout, options);
                var values = parser.TakeNext();

                Assert.AreEqual("1COLUMN1", values["1"]);
                Assert.AreEqual("1COLUMN2", values["2"]);
                Assert.AreEqual("1COLUMN3", values["3"]);
                Assert.AreEqual("1COLUMN4", values["4"]);
                Assert.AreEqual("1COLUMN5", values["5"]);

                values = parser.TakeNext();

                Assert.AreEqual("2COLUMN1", values["1"]);
                Assert.AreEqual("2COLUMN2", values["2"]);
                Assert.AreEqual("2COLUMN3", values["3"]);
                Assert.AreEqual("2COLUMN4", values["4"]);
                Assert.AreEqual("2COLUMN5", values["5"]);

                Assert.IsNull(parser.TakeNext());
            }
        }

        [TestMethod]
        public void Parse_CanReadDelimitedData_WithTrailingDelimiter()
        {
            using(var reader = new StringReader(DelimitedTestDataWithTrailingDelimiter))
            {
                var options = new NeatParserOptions {RecordSeperator = " ", SkipFirst = 1};
                var parser = new NeatParser(reader, LayoutFactory.GetDelimitedLayout(), options);

                var values = parser.TakeNext();

                Assert.AreEqual("1COLUMN1", values["1"]);
                Assert.AreEqual("1COLUMN2", values["2"]);
                Assert.AreEqual("1COLUMN3", values["3"]);
                Assert.AreEqual("1COLUMN4", values["4"]);
                Assert.AreEqual("1COLUMN5", values["5"]);

                values = parser.TakeNext();

                Assert.AreEqual("2COLUMN1", values["1"]);
                Assert.AreEqual("2COLUMN2", values["2"]);
                Assert.AreEqual("2COLUMN3", values["3"]);
                Assert.AreEqual("2COLUMN4", values["4"]);
                Assert.AreEqual("2COLUMN5", values["5"]);

                Assert.IsNull(parser.TakeNext());
            }
        }

        [ExpectedException(typeof(NeatParserException))]
        [TestMethod]
        public void IsRequiredThrowsExceptionWhenNotFound()
        {
            using(var reader = new StringReader(LayoutEditorIntegerTestData))
            {
                var parser = new NeatParser(reader, LayoutFactory.CreateEditLayoutZeroData());

                Assert.IsTrue(parser.Next());
                var values = parser.Take();

                Assert.AreEqual(null, values["1"]);
            }
        }

        [TestMethod]
        public void IntegerParsesCorrectly()
        {
            using(var reader = new StringReader(LayoutEditorIntegerTestData))
            {
                var parser = new NeatParser(reader, LayoutFactory.CreateEditLayoutZeroData());

                Assert.IsTrue(parser.Next());
                var values = parser.Take();

                Assert.AreEqual("TT", values["2"]);
                Assert.AreEqual(0, values["3"]);
                Assert.AreEqual("BA", values["4"]);
            }
        }

        [TestMethod]
        public void CanParseRecordsWithLayoutEditorColumn()
        {
            using(var reader = new StringReader(LayoutEditorTestData))
            {
                var options = new NeatParserOptions {RecordSeperator = "|"};
                var parser = new NeatParser(reader, LayoutFactory.CreateLayoutEditorLayout(), options);

                Assert.IsTrue(parser.Next());
                var values = parser.Take();
                Assert.AreEqual(5400, values[LayoutFactory.AmountColumnName]);
                Assert.AreEqual(new DateTime(2018, 8, 6), values[LayoutFactory.PaymentDateColumnName]);
                Assert.AreEqual("00000199", values[LayoutFactory.AccountNumberColumnName]);
                Assert.AreEqual("134001", values[LayoutFactory.SortCodeColumnName]);
                Assert.AreEqual("S YZXTH", values[LayoutFactory.Narrative1ColumnName]);
                Assert.AreEqual("1746917608", values[LayoutFactory.Narrative2ColumnName]);

                Assert.IsTrue(parser.Next());
                values = parser.Take();
                Assert.AreEqual(12500, values[LayoutFactory.AmountColumnName]);
                Assert.AreEqual(new DateTime(2018, 8, 6), values[LayoutFactory.PaymentDateColumnName]);
                Assert.AreEqual("00027303", values[LayoutFactory.AccountNumberColumnName]);
                Assert.AreEqual("134001", values[LayoutFactory.SortCodeColumnName]);
                Assert.AreEqual("E POIU", values[LayoutFactory.Narrative1ColumnName]);
                Assert.AreEqual("1315838910-02", values[LayoutFactory.Narrative2ColumnName]);
            }
        }

        [TestMethod]
        public void Parser_CanSkip_UsingOptions()
        {
            using(var reader = new StringReader(BacsTestData))
            {
                var options = new NeatParserOptions
                {
                    RecordSeperator = Environment.NewLine,
                    SkipFirst = 3
                };

                var parser = new NeatParser(reader, LayoutFactory.GetBacsDetailLayout(), options);
                Assert.IsTrue(parser.Next());

                string sortCode = parser.Take()["SortCode"];
                Assert.AreEqual("623187", sortCode);
            }
        }

        [TestMethod]
        public void Parser_CanReadRecords_FixedWidth()
        {
            using(var reader = new StringReader(BacsTestData))
            {
                var options = new NeatParserOptions
                {
                    RecordSeperator = Environment.NewLine,
                    SkipFirst = 3
                };

                var parser = new NeatParser(reader, LayoutFactory.GetBacsDetailLayout(), options);
                Assert.IsTrue(parser.Next());

                var values = parser.Take();

                Assert.AreEqual("623187", values["SortCode"]);
                Assert.AreEqual("80140262", values["AccountNumber"]);
                Assert.AreEqual("99", values["TransactionCode"]);
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

                var parser = new NeatParser(reader, LayoutFactory.GetBacsDetailLayout());
                parser.OnRecordRead += (s, e) => { e.ShouldSkip = toSkipPrefix.Any(l => e.LineData.StartsWith(l)); };

                Assert.IsTrue(parser.Next());
                Assert.IsTrue(parser.Next());
                var values = parser.Take();

                Assert.AreEqual("623187", values["SortCode"]);
                Assert.AreEqual("80140262", values["AccountNumber"]);
                Assert.AreEqual("99", values["TransactionCode"]);
                Assert.AreEqual(32428, values["Amount"]);
                Assert.AreEqual("JM071783D DWP SMI", values["RemittersName"]);
                Assert.AreEqual("888888-88888-08", values["RemittersReferenceNumber"]);
            }
        }

        [TestMethod]
        public void Parser_CanReadUntilTheEnd()
        {
            using(var reader = new StringReader(BacsTestData))
            {
                var toSkipPrefix = new List<string> {"VOL", "HDR", "UHL", "UTL"};

                var parser = new NeatParser(reader, LayoutFactory.GetBacsDetailLayout());
                parser.OnRecordRead += (s, e) => { e.ShouldSkip = toSkipPrefix.Any(l => e.LineData.StartsWith(l)); };
                parser.OnRecordParseError += (s, e) =>
                {
                    throw new InvalidDataException("Failed to parse " + e.LineData, e.Cause);
                };

                var rowData = new List<RecordValueContainer>();
                Assert.IsTrue(parser.Next());
                rowData.Add(parser.Take());

                Assert.IsTrue(parser.Next());
                rowData.Add(parser.Take());

                var values = rowData[0];
                Assert.AreEqual("623187", values["SortCode"]);
                Assert.AreEqual("80140262", values["AccountNumber"]);
                Assert.AreEqual("99", values["TransactionCode"]);
                Assert.AreEqual(16216, values["Amount"]);
                Assert.AreEqual("JP157084A DWP SMI", values["RemittersName"]);
                Assert.AreEqual("999999-9999999", values["RemittersReferenceNumber"]);

                values = rowData[1];
                Assert.AreEqual("623187", values["SortCode"]);
                Assert.AreEqual("80140262", values["AccountNumber"]);
                Assert.AreEqual("99", values["TransactionCode"]);
                Assert.AreEqual(32428, values["Amount"]);
                Assert.AreEqual("JM071783D DWP SMI", values["RemittersName"]);
                Assert.AreEqual("888888-88888-08", values["RemittersReferenceNumber"]);

                Assert.IsFalse(parser.Next());
            }
        }

        [TestMethod]
        public void Parser_CanReadRecords_MixedWidths()
        {
            using(var reader = new StringReader(VariableTestData))
            {
                var parser = new NeatParser(reader, LayoutFactory.GetVariableLayout());
                parser.OnRecordParseError += (s, e) =>
                {
                    throw new InvalidDataException("Error parsing " + e.LineData, e.Cause);
                };

                Assert.IsTrue(parser.Next());
                var values = parser.Take();
                Assert.AreEqual("FILE#0047", values["FileIdentifier"]);
                Assert.AreEqual("A", values["FirstData"]);
                Assert.AreEqual("FAKE DATA - 1234567898451", values["SecondData"]);
                Assert.AreEqual("MOREFAKEDATA", values["ThirdData"]);

                Assert.IsTrue(parser.Next());
                values = parser.Take();
                Assert.AreEqual("FILE#0010", values["FileIdentifier"]);
                Assert.AreEqual("B", values["FirstData"]);
                Assert.AreEqual("FAKE DATA - 123456789", values["SecondData"]);
                Assert.AreEqual("MOREFA", values["ThirdData"]);
            }
        }

        [TestMethod]
        public void Parser_FiresEventWhenRecordErrorExists()
        {
            using(var reader = new StringReader(InvalidTestData))
            {
                var parser = new NeatParser(reader, LayoutFactory.GetBacsDetailLayout());

                bool hasFired = false;
                parser.OnRecordParseError += (s, e) =>
                {
                    hasFired = true;
                    Assert.IsNotNull(e.LineData);
                    Assert.IsNotNull(e.Cause);
                    e.UserHandled = true;
                };

                Assert.IsTrue(parser.Next());
                Assert.IsTrue(hasFired);
            }
        }

        [ExpectedException(typeof(NeatParserException))]
        [TestMethod]
        public void Parser_RethrowsExceptionIfNotUserHandled()
        {
            using(var reader = new StringReader(InvalidTestData))
            {
                var parser = new NeatParser(reader, LayoutFactory.GetBacsDetailLayout());

                bool hasFired = false;
                parser.OnRecordParseError += (s, e) =>
                {
                    hasFired = true;
                    Assert.IsNotNull(e.LineData);
                    Assert.IsNotNull(e.Cause);
                    e.UserHandled = false;
                };

                Assert.IsTrue(parser.Next());
                Assert.IsTrue(hasFired);
            }
        }

        [TestMethod]
        public void Parser_CompletesFullRead()
        {
            using(var reader = new StringReader(BacsTestData))
            {
                var toSkipPrefix = new List<string> {"VOL", "HDR", "UHL", "UTL"};

                var parser = new NeatParser(reader, LayoutFactory.GetBacsDetailLayout());
                parser.OnRecordRead += (s, e) => { e.ShouldSkip = toSkipPrefix.Any(l => e.LineData.StartsWith(l)); };
                parser.OnRecordParseError += (s, e) =>
                {
                    throw new InvalidDataException("Error parsing " + e.LineData, e.Cause);
                };

                var rowData = new List<RecordValueContainer>();

                while(parser.Next()) rowData.Add(parser.Take());

                Assert.AreEqual(2, rowData.Count);
                Assert.AreEqual(6, rowData[0].RecordValues.Count);
                Assert.AreEqual(6, rowData[1].RecordValues.Count);

                foreach(var value in rowData[0].RecordValues)
                    Assert.IsNotNull(value, "rowData[0] value != null");

                foreach(var value in rowData[1].RecordValues)
                    Assert.IsNotNull(value, "rowData[1] value != null");
            }
        }
    }
}