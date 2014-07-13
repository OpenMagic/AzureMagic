Feature: WindowsAzureStorageEmulatorManager

Scenario: Start emulator when it is not running
	Given the Windows Azure Storage Emulator is not running
	When StartEmulator is called
	Then IsEmulatorRunning returns true

Scenario: Start emulator when it is running
	Given the Windows Azure Storage Emulator is running
	When StartEmulator is called
	Then IsEmulatorRunning returns true

Scenario: Stop emulator when it is not running
	Given the Windows Azure Storage Emulator is not running
	When StopEmulator is called
	Then IsEmulatorRunning returns false

Scenario: Stop emulator when it is running
	Given the Windows Azure Storage Emulator is running
	When StopEmulator is called
	Then IsEmulatorRunning returns false
