# NeatParser
A library for parsing files.




<h1 style="text-align: center;"><strong>Quick Start Guide</strong></h1>
<p>&nbsp;</p>
<h2 style="padding-left: 60px;"><span style="text-decoration: underline;"><strong>Layouts</strong></span></h2>
<p style="padding-left: 60px;">A layout is basically telling the library what a record in the file looks like.</p>
<p style="padding-left: 60px;">For example:</p>
<p style="padding-left: 60px;">&nbsp;</p>
<p style="padding-left: 60px;"><strong>Given an example delimited file</strong></p>
<p style="padding-left: 60px;">```</p>
<p style="padding-left: 60px;">HEADER</p>
<p style="padding-left: 60px;">1COLUMN1,1COLUMN2,1COLUMN3,1COLUMN4,1COLUMN5,</p>
<p style="padding-left: 60px;">2COLUMN1,2COLUMN2,2COLUMN3,2COLUMN4,2COLUMN5,</p>
<p style="padding-left: 60px;">3COLUMN1,3COLUMN2,3COLUMN3,3COLUMN4,3COLUMN5,</p>
<p style="padding-left: 60px;">```</p>
<p style="padding-left: 60px;"><strong>To configure a layout that met the specification of this file, it would be:</strong></p>
<p style="padding-left: 60px;">```</p>
<p style="padding-left: 60px;">internal static Layout CreateExampleLayout()</p>
<p style="padding-left: 60px;">{</p>
<p style="text-align: left; padding-left: 60px;">var layout = new Layout("ExampleLayout");</p>
<p style="text-align: left; padding-left: 60px;">layout.SetDelimiter(",");</p>
<p style="text-align: left; padding-left: 60px;">layout.AddColumn(new StringColumn("ColumnName1"), new FixedLengthSpace(8));</p>
<p style="text-align: left; padding-left: 60px;">layout.AddColumn(new StringColumn("ColumnName2"), new FixedLengthSpace(8));</p>
<p style="text-align: left; padding-left: 60px;">layout.AddColumn(new StringColumn("ColumnName3"), new FixedLengthSpace(8));</p>
<p style="text-align: left; padding-left: 60px;">layout.AddColumn(new StringColumn("ColumnName4"), new FixedLengthSpace(8));</p>
<p style="text-align: left; padding-left: 60px;">layout.AddColumn(new StringColumn("ColumnName5"), new FixedLengthSpace(8));</p>
<p style="text-align: left; padding-left: 60px;">return layout;</p>
<p style="padding-left: 60px;">}</p>
<p style="padding-left: 60px;">&nbsp;</p>
<p style="padding-left: 60px;">```</p>
<p style="padding-left: 60px;"><strong>To then read each record from that file you would construct a NeatParser instance and pass the layout like so:</strong></p>
<p style="padding-left: 60px;">```</p>
<p style="padding-left: 60px;">var options = new NeatParserOptions()</p>
<p style="padding-left: 60px;">{</p>
<p style="padding-left: 90px;">// Default record seperator is Environment.NewLine.</p>
<p style="padding-left: 90px;">// This can be changed by setting the RecordSeperator property on the options instance.</p>
<p style="padding-left: 90px;">// SkipFirst = 1 is to let the parser know to the first record - being the header.</p>
<p style="padding-left: 90px;">SkipFirst = 1</p>
<p style="padding-left: 60px;">};</p>
<p style="padding-left: 60px;">&nbsp;</p>
<p style="padding-left: 60px;">using(var exampleFileReader = new&nbsp;StreamReader("exampleFile.csv"))</p>
<p style="padding-left: 60px;">{</p>
<p style="padding-left: 90px;">var layout =&nbsp;CreateExampleLayout();</p>
<p style="padding-left: 90px;">var parser = new NeatParser(exampleFileReader,&nbsp;layout, options);</p>
<p style="padding-left: 90px;">while(parser.Next())</p>
<p style="padding-left: 90px;">{</p>
<p style="padding-left: 120px;">// Retrieve the values for the record by calling parser.Take();</p>
<p style="padding-left: 120px;">var recordValueContainer = parser.Take();</p>
<p style="padding-left: 120px;">// Access the values by column name defined in the layout or the RecordValues dictionary attached to the RecordValueContainer.</p>
<p style="padding-left: 120px;">&nbsp;</p>
<p style="padding-left: 120px;">// Example 1 -&nbsp;No type casting is required using this method as the value is returned using the dynamic keyword.</p>
<p style="padding-left: 120px;">string column1 = recordValueContainer["ColumnName1"];</p>
<p style="padding-left: 120px;">string column1 =&nbsp;recordValueContainer["ColumnName2"];</p>
<p style="padding-left: 120px;">&nbsp;</p>
<p style="padding-left: 120px;">// Example 2 - using this method you must pass the type as a type parameter.</p>
<p style="padding-left: 120px;">string column1 = recordValueContainer.Get&lt;string&gt;("ColumnName1");</p>
<p style="padding-left: 120px;">string column2 =&nbsp;recordValueContainer.Get&lt;string&gt;("ColumnName2");</p>
<p style="padding-left: 90px;">}</p>
<p style="padding-left: 60px;">}</p>
<p style="padding-left: 60px;">&nbsp;</p>
<p style="padding-left: 60px;">```</p>
<p style="padding-left: 60px;">&nbsp;</p>
