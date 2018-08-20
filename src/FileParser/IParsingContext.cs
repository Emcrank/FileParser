namespace FileParser
{
    public interface IParsingContext
    {
        /// <summary>
        /// Represents a number for records read. (Not including skipped).
        /// </summary>
        int LogicalRecordNumber { get; }

        /// <summary>
        /// Represents a number for records read. (Including skipped).
        /// </summary>
        int PhysicalRecordNumber { get; }
    }
}