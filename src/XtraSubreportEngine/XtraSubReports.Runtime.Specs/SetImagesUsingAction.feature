Feature: Images Should be Set By Action
	In order to easily build reports without writing any code
	As a report designer
	I want images to be set automatically

@mytag
Scenario: Insert image from file
	Given a report exists
	And an image exists as a file
	And the report contains an image placeholder
	And an action exists to place the image into the placeholder 
	When the report runs
	Then the image should be placed into the report


