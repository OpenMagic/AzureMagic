Feature: AzureExtensions

Background: 
	Given Windows Azure Storage Emulator is running
	Given connectionString is 'UseDevelopmentStorage=true;'
	Given tableName is 'unique'
	Given createTableIfNotExists is 'true'
	Given table has rows

Scenario: ExecuteAsync
	When I call ExecuteAsync
	Then all partition keys are returned
