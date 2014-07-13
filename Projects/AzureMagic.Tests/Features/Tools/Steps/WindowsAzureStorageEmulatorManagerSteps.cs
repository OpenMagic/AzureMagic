using System.Threading;
using AzureMagic.Tools;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace AzureMagic.Tests.Features.Tools.Steps
{
    [Binding]
    public class WindowsAzureStorageEmulatorManagerSteps
    {
        [Given(@"the Windows Azure Storage Emulator is not running")]
        public void GivenTheAzureStorageEmulatorIsNotRunning()
        {
            WindowAzureStorageEmulatorManager.StopEmulator();
            Sleep();
        }

        [When(@"StartEmulator is called")]
        public void WhenStartEmulatorIsCalled()
        {
            WindowAzureStorageEmulatorManager.StartEmulator();
            Sleep();
        }

        [Then(@"IsEmulatorRunning returns true")]
        public void ThenIsEmulatorRunningReturnsTrue()
        {
            WindowAzureStorageEmulatorManager.IsEmulatorRunning().Should().BeTrue();
        }

        [Given(@"the Windows Azure Storage Emulator is running")]
        public void GivenTheAzureStorageEmulatorIsRunning()
        {
            WindowAzureStorageEmulatorManager.StartEmulator();
            Sleep();
        }

        [When(@"StopEmulator is called")]
        public void WhenStopEmulatorIsCalled()
        {
            WindowAzureStorageEmulatorManager.StopEmulator();
            Sleep();
        }

        [Then(@"IsEmulatorRunning returns false")]
        public void ThenIsEmulatorRunningReturnsFalse()
        {
            WindowAzureStorageEmulatorManager.IsEmulatorRunning().Should().BeFalse();
        }

        private static void Sleep()
        {
            // A little sleep after each Given & When ensures the Then will pass when all 4 scenarios are run.
            Thread.Sleep(75);
        }
    }
}