using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace NeatParser
{
    /// <summary>
    /// Static class that will parse a record using a defined layout.
    /// </summary>
    internal static class RecordValueParser
    {
        /// <summary>
        /// Parses the string into the expected layout columns.
        /// </summary>
        /// <param name="layout">layout to use</param>
        /// <param name="dataBuffer">String data.</param>
        /// <returns></returns>
        internal static IReadOnlyDictionary<string, object> ParseValues(Layout layout, StringBuilder dataBuffer)
        {
            var parsedValues = new Dictionary<string, object>();

            for (int columnIndex = 0; columnIndex < layout.TotalCurrentColumns; columnIndex++)
            {
                var column = layout[columnIndex];

                if (ProcessDummyColumn(column, dataBuffer))
                    continue;

                if (ProcessLayoutEditorColumn(column, dataBuffer))
                    continue;

                parsedValues.Add(column.Definition.ColumnName, ParseValue(column, dataBuffer));
            }
            
            return parsedValues;
        }

        private static object ParseValue(Column column, StringBuilder dataBuffer)
        {
            try
            {
                string snippedData = column.Space.SnipData(column, dataBuffer);
                return column.Parse(snippedData);
            }
            catch (NeatParserException ex)
            {
                throw new NeatParserException(
                    FormattableString.Invariant($"An error occured parsing value for column {column.Definition.ColumnName}."), ex);
            }
        }

        private static bool ProcessDummyColumn(Column column, StringBuilder dataBuffer)
        {
            if (!column.Definition.IsDummy)
                return false;

            try
            {
                column.Space.SnipData(column, dataBuffer);
            }
            catch (NeatParserException ex)
            {
                throw new NeatParserException(
                    FormattableString.Invariant($"An error occured parsing value for dummy column {column.Definition.ColumnName}."), ex);
            }

            return true;
        }

        private static bool ProcessLayoutEditorColumn(Column column, StringBuilder dataBuffer)
        {
            if (!column.Definition.IsLayoutEditor)
                return false;

            string layoutEditorArgs = ParseValue(column, dataBuffer).ToString();

            try
            {
                column.Layout.Edit(column.Definition.LayoutEditor, layoutEditorArgs);
            }
            catch (NeatParserException ex)
            {
                throw new NeatParserException(
                    FormattableString.Invariant(
                        $"An error occured editing the layout using layout editor from column {column.Definition.ColumnName}."),
                    ex);
            }

            return true;
        }
    }
}