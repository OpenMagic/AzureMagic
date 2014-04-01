using System;
using System.Diagnostics;
using AzureMagic.Tools;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace AzureMagic.Tests.Features.Tools.Steps
{
    [Binding]
    public class AzureStorageTableEmulatorSteps
    {
        private static readonly object ThisLock = new Object();
        
        [Given(@"the Azure Storage Emulator is not running")]
        public void GivenTheAzureStorageEmulatorIsNotRunning()
        {
            StopEmulator();
        }

        private static void StopEmulator()
        {
            lock (ThisLock)
            {
                var sw = Stopwatch.StartNew();

                while (sw.Elapsed.TotalSeconds < 1 && AzureStorageEmulatorManager.IsProcessStarted())
                {
                    AzureStorageEmulatorManager.StopProcess();
                }

                if (AzureStorageEmulatorManager.IsProcessStarted())
                {
                    throw new Exception("Could not stop Azure Storage Emulator.");
                }
            }
        }

        private static void StartEmulator()
        {
            lock (ThisLock)
            {
                var sw = Stopwatch.StartNew();

                while (sw.Elapsed.TotalSeconds < 1 && !AzureStorageEmulatorManager.IsProcessStarted())
                {
                    AzureStorageEmulatorManager.StartProcess();
                }

                if (!AzureStorageEmulatorManager.IsProcessStarted())
                {
                    throw new Exception("Could not start Azure Storage Emulator.");
                }
            }
        }

        [When(@"StartProcess is called")]
        public void WhenStartProcessIsCalled()
        {
            StartEmulator();
        }

        [Then(@"IsProcessStarted returns true")]
        public void ThenIsProcessStartedReturnsTrue()
        {
            AzureStorageEmulatorManager.IsProcessStarted().Should().BeTrue();
        }

        [Given(@"the Azure Storage Emulator is running")]
        public void GivenTheAzureStorageEmulatorIsRunning()
        {
            StartEmulator();
        }

        [When(@"StopProcess is called")]
        public void WhenStopProcessIsCalled()
        {
            StopEmulator();
        }

        [Then(@"IsProcessStarted returns false")]
        public void ThenIsProcessStartedReturnsFalse()
        {
            AzureStorageEmulatorManager.IsProcessStarted().Should().BeFalse();
        }
    }
}
