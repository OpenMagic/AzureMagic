using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using AzureMagic.Exceptions;
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
        private DummyTableEntity[] DummyEntities;
        private DummyTableEntity DummyEntity;
        private TableResult TableResult;

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
            DummyEntities = new[]
            {
                new DummyTableEntity(true)
            };

            var table = AzureStorage.GetTable(ConnectionString, TableName);

            table.Create();

            foreach (var fakeRow in DummyEntities)
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
            var table = GetTable();

            table.Exists().Should().BeTrue();

            var rows = FindAllEntities();

            rows.ShouldAllBeEquivalentTo(DummyEntities, EntityEquivalencyOptions);
        }

        [Then(@"the table does not exist")]
        public void ThenTheTableDoesNotExist()
        {
            AzureStorage.GetTable(ConnectionString, TableName).Exists().Should().BeFalse();
        }

        [Given(@"entity is '(.*)'")]
        public void GivenEntityIs(string entityState)
        {
            switch (entityState)
            {
                case "null":
                    DummyEntity = null;
                    break;

                case "valid":
                    DummyEntity = new DummyTableEntity(initializeProperties: true);
                    break;

                case "invalid":
                    DummyEntity = new DummyTableEntity { PartitionKey = null };
                    break;

                default:
                    throw new ArgumentOutOfRangeException("entityState", entityState, string.Format("Cannot handle entityState: {0}", entityState));
            }
        }

        [When(@"AddEntity is called")]
        public void WhenAddEntityIsCalled()
        {
            var repository = CreateRepository();

            try
            {
                TableResult = repository.AddEntity(DummyEntity).Result;
            }
            catch (Exception exception)
            {
                Exception = exception;
            }
        }

        [Then(@"entity is added to the table")]
        public void ThenEntityIsAddedToTheTable()
        {
            var actualEntity = FindAllEntities().Single();

            actualEntity.ShouldBeEquivalentTo(DummyEntity, EntityEquivalencyOptions);
        }

        [Then(@"entity is not added to the table")]
        public void ThenEntityIsNotAddedToTheTable()
        {
            var entities = FindAllEntities().ToArray();

            entities.Any().Should().BeFalse();
        }

        [Then(@"AzureTableRepositoryException is thrown")]
        public void ThenAzureTableRepositoryExceptionIsThrown()
        {
            Exception.Should().BeOfType<AggregateException>();
            ((AggregateException)Exception).InnerExceptions.Single().Should().BeOfType<AzureTableRepositoryException>();
        }

        [Then(@"TableResult is returned")]
        public void ThenTableResultIsReturned()
        {
            TableResult.Should().NotBeNull();
        }

        [Then(@"TableResult\.HttpStatusCode should be '(.*)'")]
        public void ThenTableResult_HttpStatusCodeShouldBe(string statusCode)
        {
            var expected = GetHttpStatusCode(statusCode);
            var actual = (HttpStatusCode)TableResult.HttpStatusCode;

            actual.Should().Be(expected);
        }

        [Given(@"entity has '(.*)' PartitionKey")]
        public void GivenEntityHasPartitionKey(string partitionKey)
        {
            partitionKey = GetValue(partitionKey);

            DummyEntity = new DummyTableEntity(true) { PartitionKey =  partitionKey };
        }

        [Given(@"entity has '(.*)' RowKey")]
        public void GivenEntityHasRowKey(string rowKey)
        {
            rowKey = GetValue(rowKey);

            DummyEntity = new DummyTableEntity(true) { RowKey = rowKey };
        }

        [Then(@"AggregateException is thrown")]
        public void ThenAggregateExceptionIsThrown()
        {
            Exception.Should().BeOfType<AggregateException>();
        }

        [Then(@"InnerExceptions is AzureTableRepositoryException")]
        public void ThenInnerExceptionsIsAzureTableRepositoryException()
        {
            AggregateException.InnerExceptions.Single().Should().BeOfType<AzureTableRepositoryException>();
        }

        [Then(@"InnerException is ValidationException")]
        public void ThenInnerExceptionIsValidationException()
        {
            AggregateException.InnerExceptions.Single().InnerException.Should().BeOfType<ValidationException>();
        }

        #region " Helpers "

        private AggregateException AggregateException { get { return (AggregateException)Exception; } }

        private AzureTableRepository<DummyTableEntity> CreateRepository()
        {
            return new AzureTableRepository<DummyTableEntity>(ConnectionString, TableName, CreateTableIfNotExists);
        }

        private EquivalencyAssertionOptions<DummyTableEntity> EntityEquivalencyOptions(EquivalencyAssertionOptions<DummyTableEntity> options)
        {
            var excludedEntityProperties = new[] { "CompiledWrite", "CompiledRead" };

            return options.Excluding(subjectInfo => excludedEntityProperties.Contains(subjectInfo.PropertyInfo.Name));
        }

        private IEnumerable<DummyTableEntity> FindAllEntities()
        {
            return FindAllEntities(GetTable());
        }

        private static IEnumerable<DummyTableEntity> FindAllEntities(CloudTable table)
        {
            var rows = table.CreateQuery<DummyTableEntity>().Execute();

            return rows;
        }

        private static HttpStatusCode GetHttpStatusCode(string statusCode)
        {
            switch (statusCode)
            {
                case "No Content":
                    return HttpStatusCode.NoContent;

                default:
                    throw new ArgumentOutOfRangeException("statusCode", statusCode, "Cannot handle given status code.");
            }
        }

        private CloudTable GetTable()
        {
            return AzureStorage.GetTable(ConnectionString, TableName);
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

        #endregion

}
