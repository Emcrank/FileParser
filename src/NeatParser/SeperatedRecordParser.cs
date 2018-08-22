using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using static System.FormattableString;

namespace NeatParser
{
    /// <summary>
    /// Parser class that should be used to parse files that have records with a seperator. Default
    /// seperator is Environment.NewLine.
    /// </summary>
    public class SeperatedRecordParser
    {
        /// <summary>
        /// Gets the IParsingContext which provides contextual information regarding the parsers execution.
        /// </summary>
        public IParsingContext Context => context;

        /// <summary>
        /// Gets the layout currently in use.
        /// </summary>
        public Layout CurrentLayout => layoutDecider.Current;

        private bool IsEndOfReader => reader.Peek() < 0;
        private readonly ParsingContext context = new ParsingContext();
        private readonly LayoutDecider layoutDecider;
        private readonly SeperatedRecordParserOptions options;
        private readonly TextReader reader;
        private IReadOnlyDictionary<string, object> recordValues;

        public event EventHandler<RecordParseErrorEventArgs> OnRecordParseError;

        public event EventHandler<RecordReadEventArgs> OnRecordRead;

        /// <summary>
        /// Constructs a new instance of the <see cref="SeperatedRecordParser"/> using a
        /// <see cref="LayoutDecider"/> instance.
        /// </summary>
        /// <param name="reader">Reader</param>
        /// <param name="layoutDecider">Layout decider</param>
        /// <param name="options">Parser options</param>
        public SeperatedRecordParser(TextReader reader, LayoutDecider layoutDecider,
            SeperatedRecordParserOptions options)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            if (layoutDecider == null)
                throw new ArgumentNullException(nameof(layoutDecider));

            if (options == null)
                throw new ArgumentNullException(nameof(options));

            this.reader = reader;
            this.layoutDecider = layoutDecider;
            this.options = options;

            for (int i = 1; i <= options.SkipFirst; i++) SkipRecord();
        }

        /// <summary>
        /// Constructs a new instance of the <see cref="SeperatedRecordParser"/> using a
        /// <see cref="Layout"/> instance.
        /// </summary>
        /// <param name="reader">Reader</param>
        /// <param name="layout">layout to use</param>
        /// <param name="options">Parser options</param>
        public SeperatedRecordParser(TextReader reader, Layout layout, SeperatedRecordParserOptions options)
            : this(reader, new LayoutDecider(layout), options) { }

        /// <summary>
        /// Constructs a new instance of the <see cref="SeperatedRecordParser"/> using a
        /// <see cref="Layout"/> instance using the default options.
        /// </summary>
        /// <param name="reader">Reader</param>
        /// <param name="layout">layout to use</param>
        public SeperatedRecordParser(TextReader reader, Layout layout)
            : this(reader, new LayoutDecider(layout), new SeperatedRecordParserOptions()) { }

        /// <summary>
        /// Constructs a new instance of the <see cref="SeperatedRecordParser"/> using a
        /// <see cref="LayoutDecider"/> instance using the default options.
        /// </summary>
        /// <param name="reader">Reader</param>
        /// <param name="layoutDecider">layout decider</param>
        public SeperatedRecordParser(TextReader reader, LayoutDecider layoutDecider)
            : this(reader, layoutDecider, new SeperatedRecordParserOptions()) { }

        /// <summary>
        /// Advances the reader to the next record.
        /// </summary>
        /// <returns>
        /// True if record has been read. False if has reached end of reader or an incomplete record.
        /// </returns>
        public bool Next()
        {
            try
            {
                return NextInternal();
            }
            catch (Exception ex) when (ex is IOException)
            {
                throw new NeatParserException(ex);
            }
        }

        /// <summary>
        /// Takes the values for the current record being processed.
        /// </summary>
        /// <returns>A <see cref="RecordValueContainer"/> instance in which the values will exist.</returns>
        public RecordValueContainer Take()
        {
            return new RecordValueContainer(layoutDecider.Current, recordValues ?? new Dictionary<string, object>());
        }

        private void GetRecordValues(StringBuilder recordDataBuffer)
        {
            context.ActualRecordNumber++;

            try
            {
                recordValues = RecordValueParser.ParseValues(layoutDecider.Current, recordDataBuffer);
            }
            catch(NeatParserException ex)
            {
                throw new NeatParserException(Invariant($"An error occured parsing record number {context.ActualRecordNumber}."), ex);
            }
        }

        private RecordParseErrorEventArgs InvokeOnRecordParseError(StringBuilder lineData, Exception ex)
        {
            var eventArgs = new RecordParseErrorEventArgs(lineData.ToString(), ex);
            OnRecordParseError?.Invoke(this, eventArgs);
            return eventArgs;
        }

        private RecordReadEventArgs InvokeOnRecordReadHandler(StringBuilder lineData)
        {
            var eventArgs = new RecordReadEventArgs(lineData.ToString());
            OnRecordRead?.Invoke(this, eventArgs);
            return eventArgs;
        }

        private bool NextInternal()
        {
            return !IsEndOfReader && ReadNextRecord();
        }

        private bool ReadNextRecord()
        {
            var recordDataBuffer = ReadUntilNextSeperator();

            if(recordDataBuffer.Length == 0)
                return false;
            if(string.IsNullOrWhiteSpace(recordDataBuffer.ToString()))
                return false;
            
            if (InvokeOnRecordReadHandler(recordDataBuffer).ShouldSkip)
                return ReadNextRecord();

            layoutDecider.Decide(recordDataBuffer);

            try
            {
                GetRecordValues(recordDataBuffer);
            }
            catch (NeatParserException ex)
            {
                var eventArgs = InvokeOnRecordParseError(recordDataBuffer, ex);
                if(!eventArgs.UserHandled)
                    throw;
            }

            return true;
        }

        private StringBuilder ReadUntilNextSeperator()
        {
            context.PhysicalRecordNumber++;
            var seperatorBuffer = new CircularCharBuffer(options.RecordSeperator.Length);
            int partialLength = options.RecordSeperator.Length - 1;
            var currentStringBuffer = new StringBuilder();

            while (!IsEndOfReader)
            {
                char nextChar = (char)reader.Read();

                if (seperatorBuffer.PushAndMatch(nextChar, options.RecordSeperator))
                    return currentStringBuffer.Remove(currentStringBuffer.Length - partialLength, partialLength);

                currentStringBuffer.Append(nextChar);
            }

            return currentStringBuffer;
        }

        private void SkipRecord()
        {
            ReadUntilNextSeperator();
        }
    }
}