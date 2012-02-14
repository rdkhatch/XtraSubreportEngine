Feature: Addition
	In order simplify report creation
	As a user
	I want a default root folder to be created for me automatically in my Documents

@mytag
Scenario: Folder does not yet exist
	Given the root path folder does not exist
	When The application starts up
	Then The folder should be created
