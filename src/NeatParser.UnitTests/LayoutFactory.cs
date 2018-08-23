namespace NeatParser.UnitTests
{
    public static class LayoutFactory
    {
        internal const string SortCodeColumnName = "BeneficiaryCreditInstitution";
        internal const string Narrative1ColumnName = "OriginatingCustomerAccountName";
        internal const string Narrative2ColumnName = "BeneficiaryCustomerAccountName";
        internal const string AmountColumnName = "Amount";
        internal const string PaymentDateColumnName = "SettlementDate";
        internal const string AccountNumberColumnName = "BeneficiaryCustomerAccountNumber";
        internal const string ColumnAssignedNumber = "ColumnNumber";

        public static Layout GetDelimitedLayout()
        {
            var layout = new Layout();
            layout.SetDelimiter(",");
            layout.AddColumn(new StringColumn("1"), new FixedLengthSpace(8));
            layout.AddColumn(new StringColumn("2"), new FixedLengthSpace(8));
            layout.AddColumn(new StringColumn("3"), new FixedLengthSpace(8));
            layout.AddColumn(new StringColumn("4"), new FixedLengthSpace(8));
            layout.AddColumn(new StringColumn("5"), new FixedLengthSpace(8));
            return layout;
        }

        public static Layout GetBacsDetailLayout()
        {
            var layout = new Layout();
            layout.AddColumn(new ColumnDefinition<string>("SortCode"), new FixedLengthSpace(6));
            layout.AddColumn(new ColumnDefinition<string>("AccountNumber"), new FixedLengthSpace(8));
            layout.AddColumn(new DummyColumn("TlaCode"), new FixedLengthSpace(1));
            layout.AddColumn(new ColumnDefinition<string>("TransactionCode"), new FixedLengthSpace(2));
            layout.AddColumn(new DummyColumn("OriginatorSortCode"), new FixedLengthSpace(6));
            layout.AddColumn(new DummyColumn("OriginatorAccountNumber"), new FixedLengthSpace(8));
            layout.AddColumn(new DummyColumn("OriginatorReference"), new FixedLengthSpace(4));
            layout.AddColumn(new ColumnDefinition<int>("Amount"), new FixedLengthSpace(11));
            layout.AddColumn(new ColumnDefinition<string>("RemittersName"), new FixedLengthSpace(18));
            layout.AddColumn(new ColumnDefinition<string>("RemittersReferenceNumber"), new FixedLengthSpace(18));
            return layout;
        }

        public static Layout GetVariableLayout()
        {
            var layout = new Layout();
            var column = new ColumnDefinition<string>("FileIdentifier");
            layout.AddColumn(column, new FixedLengthSpace(9));
            layout.AddColumn(new ColumnDefinition<string>("FirstData"), new VariableLengthSpace(100, false));
            layout.AddColumn(new ColumnDefinition<string>("SecondData"), new VariableLengthSpace(100, false));
            layout.AddColumn(new ColumnDefinition<string>("ThirdData"), new VariableLengthSpace(3));
            return layout;
        }

        public static Layout CreateLayoutEditorLayout()
        {
            var layout = new Layout();
            layout.AddColumn(
                new LayoutEditorColumn(new HexadecimalLayoutEditor(1, 64)).AddMetadata(ColumnAssignedNumber, 0),
                new FixedLengthSpace(16));
            layout.AddColumn(
                new LayoutEditorColumn(new HexadecimalLayoutEditor(65, 128)).AddMetadata(ColumnAssignedNumber, 1),
                new FixedLengthSpace(16));
            layout.AddColumn(
                new DummyColumn("ProcessingCode").AddMetadata(ColumnAssignedNumber, 3),
                new FixedLengthSpace(6));
            layout.AddColumn(
                new DummyColumn("OriginalAmount").AddMetadata(ColumnAssignedNumber, 4),
                new FixedLengthSpace(14));
            layout.AddColumn(
                new ColumnDefinition<int>(AmountColumnName).AddMetadata(ColumnAssignedNumber, 6),
                new FixedLengthSpace(14));
            layout.AddColumn(
                new DummyColumn("ExchangeRate").AddMetadata(ColumnAssignedNumber, 10),
                new FixedLengthSpace(12));
            layout.AddColumn(
                new DummyColumn("DateSent").AddMetadata(ColumnAssignedNumber, 12),
                new FixedLengthSpace(8));
            layout.AddColumn(
                new DateTimeColumn(PaymentDateColumnName, "yyyyMMdd").AddMetadata(ColumnAssignedNumber, 15),
                new FixedLengthSpace(8));
            layout.AddColumn(
                new DummyColumn("ActionCode").AddMetadata(ColumnAssignedNumber, 26),
                new FixedLengthSpace(4));
            layout.AddColumn(
                new DummyColumn("ProcessedAsynchronously").AddMetadata(ColumnAssignedNumber, 27),
                new FixedLengthSpace(1));
            layout.AddColumn(
                new DummyColumn("SettlementCycleId").AddMetadata(ColumnAssignedNumber, 29),
                new FixedLengthSpace(3));
            layout.AddColumn(
                new DummyColumn("TransactionReferenceNumber").AddMetadata(ColumnAssignedNumber, 31),
                new VariableLengthSpace(2));
            layout.AddColumn(
                new DummyColumn("SubmittingMember").AddMetadata(ColumnAssignedNumber, 32),
                new VariableLengthSpace(2));
            layout.AddColumn(
                new StringColumn(AccountNumberColumnName).AddMetadata(ColumnAssignedNumber, 35),
                new VariableLengthSpace(2));
            layout.AddColumn(
                new DummyColumn("OriginatingCreditInstitution").AddMetadata(ColumnAssignedNumber, 42),
                new FixedLengthSpace(11));
            layout.AddColumn(
                new DummyColumn("OriginatingCustomerAccountNumber").AddMetadata(ColumnAssignedNumber, 43),
                new VariableLengthSpace(2));
            layout.AddColumn(
                new DummyColumn("ChargingInformation").AddMetadata(ColumnAssignedNumber, 46),
                new VariableLengthSpace(3));
            layout.AddColumn(
                new DummyColumn("OriginalCurrency").AddMetadata(ColumnAssignedNumber, 49),
                new FixedLengthSpace(3));
            layout.AddColumn(
                new DummyColumn("Currency").AddMetadata(ColumnAssignedNumber, 51),
                new FixedLengthSpace(3));
            layout.AddColumn(
                new DummyColumn("PaymentData").AddMetadata(ColumnAssignedNumber, 61),
                new VariableLengthSpace(3));
            layout.AddColumn(
                new DummyColumn("EndToEndReference").AddMetadata(ColumnAssignedNumber, 62),
                new VariableLengthSpace(3));
            layout.AddColumn(
                new DummyColumn("NumericReference").AddMetadata(ColumnAssignedNumber, 71),
                new FixedLengthSpace(4));
            layout.AddColumn(
                new DummyColumn("FileId").AddMetadata(ColumnAssignedNumber, 72),
                new VariableLengthSpace(3));
            layout.AddColumn(
                new StringColumn(SortCodeColumnName).AddMetadata(ColumnAssignedNumber, 95),
                new VariableLengthSpace(2));
            layout.AddColumn(
                new DummyColumn("SendingFPSInstitution").AddMetadata(ColumnAssignedNumber, 98),
                new FixedLengthSpace(11));
            layout.AddColumn(
                new DummyColumn("ReceivingMember").AddMetadata(ColumnAssignedNumber, 99),
                new VariableLengthSpace(2));
            layout.AddColumn(
                new StringColumn(Narrative1ColumnName).AddMetadata(ColumnAssignedNumber, 116),
                new VariableLengthSpace(2));
            layout.AddColumn(
                new DummyColumn("OriginatingCustomerAccountAddress").AddMetadata(ColumnAssignedNumber, 117),
                new VariableLengthSpace(3));
            layout.AddColumn(
                new StringColumn(Narrative2ColumnName).AddMetadata(ColumnAssignedNumber, 118),
                new VariableLengthSpace(3));
            layout.AddColumn(
                new DummyColumn("BeneficiaryCustomerAccountAddress").AddMetadata(ColumnAssignedNumber, 119),
                new VariableLengthSpace(3));
            layout.AddColumn(
                new DummyColumn("ReferenceInformation").AddMetadata(ColumnAssignedNumber, 120),
                new VariableLengthSpace(3));
            layout.AddColumn(
                new DummyColumn("RemittanceInformation").AddMetadata(ColumnAssignedNumber, 121),
                new VariableLengthSpace(3));
            layout.AddColumn(
                new DummyColumn("RegulatoryReporting").AddMetadata(ColumnAssignedNumber, 122),
                new VariableLengthSpace(3));
            layout.AddColumn(
                new DummyColumn("PaymentReturnCode").AddMetadata(ColumnAssignedNumber, 126),
                new VariableLengthSpace(3));
            layout.AddColumn(
                new DummyColumn("ReturnedPaymentFPID").AddMetadata(ColumnAssignedNumber, 127),
                new VariableLengthSpace(3));
            layout.AddColumn(
                new DummyColumn("MessageAuthenticationCode").AddMetadata(ColumnAssignedNumber, 128),
                new FixedLengthSpace(16));
            return layout;
        }

        public static Layout CreateEditLayoutZeroData()
        {
            var layout = new Layout();
            layout.AddColumn(new LayoutEditorColumn(new HexadecimalLayoutEditor(1, 4)), new FixedLengthSpace(1));
            layout.AddColumn(new ColumnDefinition<int>("1", true).AddMetadata(ColumnAssignedNumber, 1),
                new FixedLengthSpace(2));
            layout.AddColumn(new StringColumn("2").AddMetadata(ColumnAssignedNumber, 2), new FixedLengthSpace(2));
            layout.AddColumn(new ColumnDefinition<int>("3").AddMetadata(ColumnAssignedNumber, 3),
                new FixedLengthSpace(2));
            layout.AddColumn(new StringColumn("4").AddMetadata(ColumnAssignedNumber, 4), new FixedLengthSpace(2));
            return layout;
        }
    }
}