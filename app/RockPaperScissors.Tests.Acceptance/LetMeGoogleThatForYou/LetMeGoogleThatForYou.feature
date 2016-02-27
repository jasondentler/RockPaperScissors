Feature: LetMeGoogleThatForYou
	In order to be a jerk
	As a jerk
	I want to get a link to Let Me Google That For You

Scenario: Get a link to a google search
	Given I am on Let Me Google That For You
	When I search for "How to troll my coworkers"
	Then the link to that search is displayed
	And the copy button is visible
	And the shorten button is visible
	And the preview button is visible

