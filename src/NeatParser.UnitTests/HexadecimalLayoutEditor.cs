using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using static System.FormattableString;

namespace NeatParser.UnitTests
{
    public sealed class HexadecimalLayoutEditor : ILayoutEditor
    {
        private readonly int startNumber;
        private readonly int endNumber;

        public HexadecimalLayoutEditor(int startNumber, int endNumber)
        {
            this.startNumber = startNumber;
            this.endNumber = endNumber;
        }

        /// <summary>
        ///     takes a string of Hexadecimal characters which will be converted to binary
        ///     representation to determine which columns are available in the layout.
        ///     e.g 0101001 would mean columns 2, 4 and 7 were present.
        /// </summary>
        /// <param name="layout">Column this layout editor is attached to</param>
        /// <param name="args">Editor arguments.</param>
        /// <returns>Updated list of columns</returns>
        public IList<Column> Edit(Layout layout, string args)
        {
            if(layout == null)
                throw new ArgumentNullException(nameof(layout));

            if(!layout.DefinedColumns.Any())
                throw new ArgumentException("No columns defined for this layout.");

            if(string.IsNullOrWhiteSpace(args))
                throw new ArgumentNullException(nameof(args));

            return EditInternal(layout, args);
        }

        private IList<Column> EditInternal(Layout layout, string args)
        {
            string binaryString = GetBinaryFromHexString(args);
            ValidateBitmap(binaryString);

            var columnsAvailable = layout.CurrentColumns;
            int columnIndex = startNumber;

            foreach(char c in binaryString)
            {
                var currentColumn = GetColumn(columnsAvailable, columnIndex++);
                if(currentColumn == null)
                    continue;

                if(c == '1')
                    continue;

                columnsAvailable.Remove(currentColumn);
            }

            return columnsAvailable;
        }

        private static Column GetColumn(IEnumerable<Column> columns, int number)
        {
            return columns.FirstOrDefault(c => c.Definition.Metadata.ContainsKey(LayoutFactory.ColumnAssignedNumber) && 
                c.Definition.Metadata[LayoutFactory.ColumnAssignedNumber].Equals(number));
        }

        private static string GetBinaryFromHexString(string hexadecimalString)
        {
            try
            {
                return string.Join(string.Empty, hexadecimalString.Select(HexCharToBinary));
            }
            catch(Exception ex) when(ex is ArgumentException || ex is OverflowException || ex is FormatException)
            {
                throw new NeatParserException(
                    Invariant($"Unable to get the binary from the string '{hexadecimalString}'."), ex);
            }
        }

        private static string HexCharToBinary(char character)
        {
            return Convert.ToString(Convert.ToInt32(character.ToString(), 16), 2).PadLeft(4, '0');
        }

        private void ValidateBitmap(string binaryString)
        {
            int difference = endNumber - startNumber + 1;
            if (binaryString.Length != difference)
                throw new InvalidDataException(Invariant(
                    $"The binary string value retrieved is not long enough for the endNumber provided. endNumber: {endNumber}, binaryString: '{binaryString}'"));
        }
    }
}