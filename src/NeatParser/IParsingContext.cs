namespace NeatParser
{
    public interface IParsingContext
    {
        /// <summary>
        /// Represents a number for records read. (Not including skipped).
        /// </summary>
        int ActualRecordNumber { get; }

        /// <summary>
        /// Represents a number for records read. (Including skipped).
        /// </summary>
        int PhysicalRecordNumber { get; }
    }
}