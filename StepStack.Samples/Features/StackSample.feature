Feature: StackSample
	Simple calculator for adding two numbers

@mytag
Scenario: Fail as the stack is not retried
	Given I am using the sample retry policy
	Given I have a number 0
	Given I add 10
	Given I subtract 9
	Then I throw if less than 3 without retry
	
@mytag
Scenario: Fail as using the test retry policy
	Given I am using the test retry policy
	Given I have a number 0
	Given I add 2 until 10
	Given I subtract 4
	Then I throw if less than 5 with frame retry
	
@mytag
Scenario: Pass as it reverts to the default retry policy
	Given I am using the test retry policy
	Given I want to use the default retry policy
	Given I have a number 0
	Given I add 2 until 10
	Given I subtract 4
	Then I throw if less than 5 with frame retry
	
@mytag
Scenario: Pass as it defaults to a known retry policy
	Given I am using the unknown retry policy
	Given I have a number 0
	Given I add 2 until 10
	Given I subtract 4
	Then I throw if less than 5 with frame retry
	
@mytag
Scenario: Pass as the step Given I add 2 until 10 is retried
	Given I am using the sample retry policy
	Given I have a number 0
	Given I add 2 until 10
	Given I subtract 4
	Then I throw if less than 5 with frame retry
	
@mytag
Scenario: Pass as the frame is retried
	Given I am using the sample retry policy
	Given I have a number 0
	Given I add 10
	Given I subtract 9
	Then I throw if less than 5 with frame retry
	
@mytag
Scenario: Pass as the stack is retried
	Given I am using the sample retry policy
	Given I have a number 0
	Given I add 2
	Given I multiply by 2
	Given I subtract 1
	Then I throw if less than 11 with frame retry