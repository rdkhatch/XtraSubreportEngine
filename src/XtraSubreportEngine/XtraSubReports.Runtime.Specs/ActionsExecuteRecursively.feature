Feature: Actions
	In order to have clean seperation bethween the report designer and the developer
	As a developer
	I want to write .net actions which can be invoked at runtime

@mytag
Scenario: Change text on a label in a subreport
	Given A report exists 
	And the report has an XRLabel named ChangeMe in the header
	And ChangeMe's text property has a value of Brodie
	And the report has another XRLabel named DontChangeMe in header
	And DontChangeMe's text property has a value of GreenBayPackers
	And an action exists against an XRLabel named ChangeMe to change the name to Camp and increment a counter
	And the xtrasubreport engine is initialized
	When the report engine runs
	Then ChangeMe's text property should have a value of Camp
	And DontChangeMe's text property should have a value of GreenBayPackers
	And the counter incremented by the action should have a count of 1


