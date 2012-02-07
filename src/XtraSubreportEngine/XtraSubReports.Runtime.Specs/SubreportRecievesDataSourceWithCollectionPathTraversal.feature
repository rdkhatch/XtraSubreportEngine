Feature: Subreport Recieves Datasource With Collection Path Traversal
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario: Subreport inside of a group header band
	Given A parent report exists
	And the parent report has a datasource
	And the parent report has a group traversal on a collection inside a detail band
	And the group contains a subreport in its header
	When the report engine runs
	Then the subreport should have the same datasource as the containing group's datasource collection

Scenario: Subreport inside of a group footer band
	Given A parent report exists
	And the parent report has a datasource
	And the parent report has a group traversal on a collection inside a detail band
	And the group contains a subreport in its footer
	When the report engine runs
	Then the subreport should have the same datasource as the containing group's datasource collection

Scenario: Subreport inside of a group detail band
	Given A parent report exists
	And the parent report has a datasource
	And the parent report has a group traversal on a collection inside a detail band
	And the group's detail band has a subreport
	When the report engine runs
	Then each subreport instance should have a collection datasource containing only one item
	And each subreport instance's datasource contains the same datasource as the containing group's detail band