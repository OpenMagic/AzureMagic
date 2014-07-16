Feature: AzureTableRepository.DeleteEntity

Background: 
	Given Windows Azure Storage Emulator is running
	Given connectionString is 'UseDevelopmentStorage=true;'
	Given tableName is 'unique'
	Given createTableIfNotExists is 'true'

Scenario: when entity is null
	Given entity is 'null'
	When AddEntity is called
	Then ArgumentNullException is thrown for 'entity'

Scenario: when entity does exist
	Given the entity does exist
	When DeleteEntity is called
	Then entity is deleted from the table
	And TableResult is returned
	And TableResult.HttpStatusCode should be 'No Content'

Scenario: when entity does not exist
	Given the entity does not exist
	When DeleteEntity is called
	Then AggregateException is thrown
	And InnerExceptions is AzureTableRepositoryException
	And InnerException is StorageException