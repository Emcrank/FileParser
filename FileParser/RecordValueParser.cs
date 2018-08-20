using System;
using System.Collections.Generic;
using System.Text;
using static System.FormattableString;

namespace FileParser
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
        internal static IReadOnlyList<object> ParseValues(Layout layout, StringBuilder dataBuffer)
        {
            var parsedValues = new List<object>();

            for (int columnIndex = 0; columnIndex < layout.TotalColumns; columnIndex++)
            {
                var column = layout[columnIndex];

                if (ProcessDummyColumn(column, dataBuffer))
                    continue;

                if (ProcessLayoutEditorColumn(column, dataBuffer))
                    continue;

                parsedValues.Add(ParseValue(column, dataBuffer));
            }

            return parsedValues;
        }

        private static object ParseValue(Column column, StringBuilder dataBuffer)
        {
            try
            {
                string snippedData = column.Space.SnipData(column.Layout, dataBuffer);
                return column.Parse(snippedData);
            }
            catch (ArgumentException ex)
            {
                throw new FileParserException(
                    Invariant($"An error occured parsing value for column {column.Definition.ColumnName}."), ex);
            }
        }

        private static bool ProcessDummyColumn(Column column, StringBuilder dataBuffer)
        {
            if (!column.Definition.IsDummy)
                return false;

            try
            {
                column.Space.SnipData(column.Layout, dataBuffer);
            }
            catch (ArgumentException ex)
            {
                throw new FileParserException(
                    Invariant($"An error occured parsing value for dummy column {column.Definition.ColumnName}."), ex);
            }

            return true;
        }

        private static bool ProcessLayoutEditorColumn(Column column, StringBuilder dataBuffer)
        {
            if (!column.Definition.IsLayoutEditor)
                return false;

            string layoutEditorArgs;

            try
            {
                string snippedData = column.Space.SnipData(column.Layout, dataBuffer);
                layoutEditorArgs = column.Parse(snippedData).ToString();
            }
            catch (FileParserException ex)
            {
                throw new FileParserException(
                    Invariant(
                        $"An error occured parsing value for layout editor column {column.Definition.ColumnName}."),
                    ex);
            }

            try
            {
                column.Layout.Edit(column.Definition.LayoutEditor, layoutEditorArgs);
            }
            catch (FileParserException ex)
            {
                throw new FileParserException(
                    Invariant(
                        $"An error occured editing layout using layout editor column {column.Definition.ColumnName}."),
                    ex);
            }

            return true;
        }
    }
}