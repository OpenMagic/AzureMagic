Feature: AzureTableStorage.ValidateTableName

Scenario: ValidateTableName when tableName is valid
	Given a valid tableName
	When ValidateTableName is called
	Then an exception is not thrown

Scenario: ValidateTableName when tableName is invalid
	Given a invalid tableName
	When ValidateTableName is called
	Then an exception is thrown
