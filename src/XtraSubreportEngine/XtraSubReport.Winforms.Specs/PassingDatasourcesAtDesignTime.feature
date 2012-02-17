Feature: Passing datasources at design time
	In order simplify report creation
	As a user
	I want a default root folder to be created for me automatically in my Documents

@mytag
Scenario: Passing Datasource Using Traversal
	Given The design runtime is ready
	And a datasource exists called DogTime
	And PersonReport exists with a subreport called DogReport in a detail report
	And PersonReport loads the DogTime datasource
	And a new report instance exists
	When A ReportActivatedBySubreportMessage occurs which contains the new report instance
	Then the new report instance's datasource should be the first dog of the first person from PersonReport's datasource


