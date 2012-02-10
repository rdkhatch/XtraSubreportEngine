Feature: Subreport Inside of a Detail Report Band is passed the correct Datasource
	In order to easily build reports without writing any code
	As a report designer
	I want my sub reports to share datasources automatically, even in detail report bands

@mytag
Scenario: Subreport inside header of detail report band
	Given A parent report exists
	And the parent report has a datasource of three items
	And a subreport exists as a file
	And the parent report has a detail report band with a datamember of dogs
	And the detail report band contains a subreport in its header band
	And the XRSubreport container references the subreport's filename
	And the xtrasubreport engine is initialized
	When the report engine runs
	Then the subreport should have the same datasource as the containing group's datasource collection


Scenario: Subreport inside footer of detail report band
	Given A parent report exists
	And the parent report has a datasource of three items
	And a subreport exists as a file
	And the parent report has a detail report band with a datamember of dogs
	And the detail report band contains a subreport in its footer band
	And the XRSubreport container references the subreport's filename
	And the xtrasubreport engine is initialized
	When the report engine runs
	Then the subreport should have the same datasource as the containing group's datasource collection

Scenario: Subreport inside detail of detail report band
	Given A parent report exists
	And the parent report has a datasource of three items
	And a subreport exists as a file
	And the parent report has a detail report band with a datamember of dogs
	And the detail report band contains a subreport in its detail band
	And the XRSubreport container references the subreport's filename
	And the xtrasubreport engine is initialized
	When the report engine runs
	Then each subreport should have a datasource containing a single item
	And each subreport datasource contains the same datasource as the containing group's detail band