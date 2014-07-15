Feature: AzureTableRepository.Query

Background: 
	Given Windows Azure Storage Emulator is running
	Given connectionString is 'UseDevelopmentStorage=true;'
	Given tableName is 'unique'
	Given createTableIfNotExists is 'true'

Scenario: Query
	Given table has entities
	When Query is called
	Then TableQuery<TEntity> is returned
	And entities can be read