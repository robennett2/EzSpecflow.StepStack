Feature: StackSample
	Simple calculator for adding two numbers

@mytag
Scenario: Fail as the stack is not retried
	Given I have a number 0
	Given I add 10
	Given I subtract 9
	Then I throw if less than 3 without retry
	
@mytag
Scenario: Pass as the step Given I add 2 until 10 is retried
	Given I have a number 0
	Given I add 2 until 10
	Given I subtract 4
	Then I throw if less than 5 with stack retry
	
@mytag
Scenario: Pass as the stack is retried
	Given I have a number 0
	Given I add 10
	Given I subtract 9
	Then I throw if less than 5 with stack retry