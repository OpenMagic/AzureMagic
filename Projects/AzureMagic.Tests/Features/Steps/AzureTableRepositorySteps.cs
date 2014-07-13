using System;
using System.Linq;
using AzureMagic.Tests.Support;
using AzureMagic.Tools;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Microsoft.WindowsAzure.Storage.Table;
using TechTalk.SpecFlow;

namespace AzureMagic.Tests.Features.Steps
{
    [Binding]
    public class AzureTableRepositorySteps : StepsBase
    {
        private string ConnectionString;
        private string TableName;
        private bool CreateTableIfNotExists;
        private Exception Exception;
        private AzureTableRepository<DummyTableEntity> Repository;
        private DummyTableEntity[] DummyRows;

        [Given(@"Windows Azure Storage Emulator is running")]
        public void GivenWindowsAzureStorageEmulatorIsRunning()
        {
            WindowAzureStorageEmulatorManager.StartEmulator();
        }

        [Given(@"connectionString is '(.*)'")]
        public void GivenConnectionStringIs(string connectionString)
        {
            ConnectionString = GetValue(connectionString);
        }

        [Given(@"tableName is '(.*)'")]
        public void GivenTableNameIs(string tableName)
        {
            TableName = GetTableName(tableName);
        }

        [Given(@"createTableIfNotExists is '(.*)'")]
        public void GivenCreateTableIfNotExistsIs(bool createTableIfNotExists)
        {
            CreateTableIfNotExists = createTableIfNotExists;
        }

        [When(@"constructor is called")]
        public void WhenConstructorIsCalled()
        {
            try
            {
                Repository = new AzureTableRepository<DummyTableEntity>(ConnectionString, TableName, CreateTableIfNotExists);
            }
            catch (Exception exception)
            {
                Exception = exception;
            }
        }

        [Then(@"ArgumentNullException is thrown for '(.*)'")]
        public void ThenArgumentNullExceptionIsThrownFor(string paramName)
        {
            Exception.Should().BeOfType<ArgumentNullException>();
            Exception.Message.Should().Contain(string.Format("Parameter name: {0}", paramName));
        }

        [Then(@"ArgumentException is thrown for '(.*)'")]
        public void ThenArgumentExceptionIsThrownFor(string paramName)
        {
            Exception.Should().BeOfType<ArgumentException>();
            Exception.Message.Should().Contain(string.Format("Parameter name: {0}", paramName));
        }

        [Given(@"table does not exist")]
        public void GivenTableDoesNotExist()
        {
            // table hasn't been created so nothing to do.
        }

        [Given(@"table does exist")]
        public void GivenTableDoesExist()
        {
            DummyRows = new[]
            {
                new DummyTableEntity(true)
            };

            var table = AzureStorage.GetTable(ConnectionString, TableName);

            table.Create();

            foreach (var fakeRow in DummyRows)
            {
                table.Execute(TableOperation.Insert(fakeRow));
            }
        }

        [Then(@"the table is created")]
        public void ThenTheTableIsCreated()
        {
            AzureStorage.GetTable(ConnectionString, TableName).Exists().Should().BeTrue();
        }

        [Then(@"the table remains intact")]
        public void ThenTheTableRemainsIntact()
        {
            var excludedEntityProperties = new[] { "CompiledWrite", "CompiledRead" };
            var table = AzureStorage.GetTable(ConnectionString, TableName);

            table.Exists().Should().BeTrue();

            var rows = table.CreateQuery<DummyTableEntity>().ToArray();

            rows.ShouldAllBeEquivalentTo(DummyRows, options => options.Excluding(subjectInfo => excludedEntityProperties.Contains(subjectInfo.PropertyInfo.Name)));
        }

        [Then(@"the table does not exist")]
        public void ThenTheTableDoesNotExist()
        {
            AzureStorage.GetTable(ConnectionString, TableName).Exists().Should().BeFalse();
        }

        protected override void Cleanup()
        {
            try
            {
                AzureStorage.GetTable(ConnectionString, TableName).DeleteIfExists();
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception)
            {
                // ignoring all exceptions.
            }

            base.Cleanup();
        }
    }
}
