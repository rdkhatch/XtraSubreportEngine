Feature: Subreport recieves datasource without path traversal
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario: Subreport inside of a header band
	Given A parent report exists
	And the parent report has a datasource of three items
	And a subreport exists as a file
	And a XRSubreport container exists in the parent report's header band
	And the XRSubreport container references the subreport's filename
	And the xtrasubreport engine is initialized
	When the report engine runs
	Then the subreport's datasource should be the same as the parent report's datasource
	And the subreport's datasource should not be null
	And the subreport action should have been fired 1 time(s)

Scenario: Subreport inside of a footer band
	Given A parent report exists
	And the parent report has a datasource of three items
	And a subreport exists as a file
	And a XRSubreport container exists in the parent report's footer band
	And the XRSubreport container references the subreport's filename
	And the xtrasubreport engine is initialized
	When the report engine runs
	Then the subreport's datasource should be the same as the parent report's datasource
	And the subreport's datasource should not be null
	And the subreport action should have been fired 1 time(s)

Scenario: Subreport inside of a detail band
	Given A parent report exists
	And the parent report has a datasource of three items
	And a subreport exists as a file
	And a XRSubreport container exists in the parent report's detail band
	And the XRSubreport container references the subreport's filename
	And the xtrasubreport engine is initialized with datasource tracking
	When the report engine runs
	Then each item in the parent's datasource should have been set once
	And the subreport action should have been fired 3 time(s)



