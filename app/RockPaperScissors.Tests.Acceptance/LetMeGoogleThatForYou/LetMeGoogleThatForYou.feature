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

Scenario: Navigate to link
	Given I am on Let Me Google That For You
	And I have searched for "How to troll my coworkers"
	When I click Preview
	Then the url is "https://www.google.com/search?btnG=1&pws=0&q=How+to+troll+my+coworkers&gws_rd=ssl"
