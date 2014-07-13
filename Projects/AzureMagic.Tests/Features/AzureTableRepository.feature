Feature: AzureTableRepository

Background: 
	Given Windows Azure Storage Emulator is running
	Given connectionString is 'UseDevelopmentStorage=true;'
	Given tableName is 'unique'
	Given createTableIfNotExists is 'true'

Scenario: Constructor when connectionString is null
	Given connectionString is 'null'
	When constructor is called
	Then ArgumentNullException is thrown for 'connectionString'

Scenario: Constructor when connectionString is empty
	Given connectionString is 'empty'
	When constructor is called
	Then ArgumentException is thrown for 'connectionString'

Scenario: Constructor when tableName is null
	Given tableName is 'null'
	When constructor is called
	Then ArgumentNullException is thrown for 'tableName'

Scenario: Constructor when tableName is empty
	Given tableName is 'empty'
	When constructor is called
	Then ArgumentException is thrown for 'tableName'

Scenario: Constructor when createTableIfNotExists is true and table does not exist
	Given createTableIfNotExists is 'true'
	And table does not exist
	When constructor is called
	Then the table is created

Scenario: Constructor when createTableIfNotExists is true and table does exist
	Given createTableIfNotExists is 'true'
	And table does exist
	When constructor is called
	Then the table remains intact

Scenario: Constructor when createTableIfNotExists is false and table does not exist
	Given createTableIfNotExists is 'false'
	And table does not exist
	When constructor is called
	Then the table does not exist

Scenario: Constructor when createTableIfNotExists is false and table does exist
	Given createTableIfNotExists is 'false'
	And table does exist
	When constructor is called
	Then the table remains intact

