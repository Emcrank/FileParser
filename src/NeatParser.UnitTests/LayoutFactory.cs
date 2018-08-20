namespace NeatParser.UnitTests
{
    public static class LayoutFactory
    {
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
            layout.AddColumn(new ColumnDefinition<string>("FileIdentifier"), new FixedLengthSpace(9));
            layout.AddColumn(new ColumnDefinition<string>("FirstData"), new VariableLengthSpace(100, false));
            layout.AddColumn(new ColumnDefinition<string>("SecondData"), new VariableLengthSpace(100, false));
            layout.AddColumn(new ColumnDefinition<string>("ThirdData"), new VariableLengthSpace(3));
            return layout;
        }
    }
}