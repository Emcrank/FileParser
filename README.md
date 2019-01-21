# NeatParser
A library for parsing files.

## Parsing a delimited file

 1. Define the file layout using the layout class.
 2. Instantiate parser options (Optional).
 3. Instantiate parser and read values.

**For Example, given a file in the below format:**

    HEADER
   	1COLUMN1,1COLUMN2,1COLUMN3,1COLUMN4,1COLUMN5,
   	2COLUMN1,2COLUMN2,2COLUMN3,2COLUMN4,2COLUMN5,
   	3COLUMN1,3COLUMN2,3COLUMN3,3COLUMN4,3COLUMN5,

**To configure a layout that met the specification of this file, it would be:**
```csharp
internal static Layout CreateExampleLayout()
{
	var layout = new Layout("ExampleLayout");
	layout.SetDelimiter(",");
	layout.AddColumn(new StringColumn("ColumnName1"), new FixedLengthSpace(8));
	layout.AddColumn(new StringColumn("ColumnName2"), new FixedLengthSpace(8));
	layout.AddColumn(new StringColumn("ColumnName3"), new FixedLengthSpace(8));
	layout.AddColumn(new StringColumn("ColumnName4"), new FixedLengthSpace(8));
	layout.AddColumn(new StringColumn("ColumnName5"), new FixedLengthSpace(8));
	return layout;
}
```

**To then read each record from that file:**
**Instantiate an instance of `NeatParserOptions` if required.**

```csharp
var options = new NeatParserOptions()
{
	// Default record seperator is Environment.NewLine.
	// This can be changed by setting the RecordSeperator property on the options instance.
	// SkipFirst = 1 is to let the parser know to the first record - being the header.
	SkipFirst = 1
};
```
**Next create the `NeatParser` instance and pass the required parameters including the options and layout.**
If you do not require custom options, use 
`new NeatParser(exampleFileReader, layout);` constructor instead.

```csharp
using(var exampleFileReader = new StreamReader("exampleFile.csv"))
{
	var layout = CreateExampleLayout();
	var parser = new NeatParser(exampleFileReader, layout, options);
	
	while(parser.Next())
	{
		// Retrieve the values for the record by calling parser.Take();
		var recordValueContainer = parser.Take();
		// Access the values by column name defined in the layout or the RecordValues dictionary attached to the RecordValueContainer.
		// Example 1 - No type casting is required using this method as the value is returned using the dynamic keyword.
		string column1 = recordValueContainer["ColumnName1"];
		string column1 = recordValueContainer["ColumnName2"];

		// Example 2 - using this method you must pass the type as a type parameter.
		string column1 = recordValueContainer.Get<string>("ColumnName1");
		string column2 = recordValueContainer.Get<string>("ColumnName2");
	}
}
```
Using the `NeatParser.Take()` method, you can retrieve the values for each line.
If you wish not to use a loop with `while(NeatParser.Next())`, there is also the `NeatParser.TakeNext()` method which will take the values for the current line and advance the reader by one record.
