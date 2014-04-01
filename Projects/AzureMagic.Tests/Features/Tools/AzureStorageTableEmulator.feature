# These tests individual pass but in same run all 4 passing is rare. Hence why they are ignored.

Feature: AzureStorageTableEmulator

@ignore
Scenario: Start emulator when it's not running
	Given the Azure Storage Emulator is not running
	When StartProcess is called
	Then IsProcessStarted returns true

@ignore
Scenario: Start emulator when it's running
	Given the Azure Storage Emulator is running
	When StartProcess is called
	Then IsProcessStarted returns true

@ignore
Scenario: Stop emulator when it's not running
	Given the Azure Storage Emulator is not running
	When StopProcess is called
	Then IsProcessStarted returns false

@ignore
Scenario: Stop emulator when it's running
	Given the Azure Storage Emulator is running
	When StopProcess is called
	Then IsProcessStarted returns false
