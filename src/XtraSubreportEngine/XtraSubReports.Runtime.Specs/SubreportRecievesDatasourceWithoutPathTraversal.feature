Feature: Subreport recieves datasource without path traversal
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario: Subreport inside of a header band
	Given A parent report exists
	And the parent report has a datasource
	And the parent report has a subreport in the report header
	When the report engine runs
	Then the subreport should have the same datasource as the parent as a collection

Scenario: Subreport inside of a footer band
	Given A parent report exists
	And the parent report has a datasource
	And the parent report has a subreport in the report footer
	When the report engine runs
	Then the subreport should have the same datasource as the parent as a collection

Scenario: Subreport inside of a detail band
	Given A parent report exists
	And the parent report has a datasource
	And the parent report has a subreport in the detail band
	When the report engine runs
	Then each subreport instance should have a collection datasource containing only one item
	And each subreport instance's datasource contains the same datasource as the containing detail band


