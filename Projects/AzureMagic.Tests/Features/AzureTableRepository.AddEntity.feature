Feature: AzureTableRepository.AddEntity

Background: 
	Given Windows Azure Storage Emulator is running
	Given connectionString is 'UseDevelopmentStorage=true;'
	Given tableName is 'unique'
	Given createTableIfNotExists is 'true'

Scenario: when entity is null
	Given entity is 'null'
	When AddEntity is called
	Then ArgumentNullException is thrown for 'entity'

Scenario: when entity is valid
	Given entity is 'valid'
	When AddEntity is called
	Then entity is added to the table
	And TableResult is returned
	And TableResult.HttpStatusCode should be 'No Content'

Scenario: when PartitionKey is null
	Given entity has 'null' PartitionKey
	When AddEntity is called
	Then AggregateException is thrown
	And InnerExceptions is AzureTableRepositoryException
	And InnerException is ValidationException
	And entity is not added to the table

Scenario: when PartitionKey is empty
	Given entity has 'empty' PartitionKey
	When AddEntity is called
	Then AggregateException is thrown
	And InnerExceptions is AzureTableRepositoryException
	And InnerException is ValidationException
	And entity is not added to the table

Scenario: when RowKey is null
	Given entity has 'null' RowKey
	When AddEntity is called
	Then AggregateException is thrown
	And InnerExceptions is AzureTableRepositoryException
	And InnerException is ValidationException
	And entity is not added to the table