using System;
using System.ComponentModel.DataAnnotations;
using AzureMagic.Tables;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace AzureMagic.Tests.Features.Tables.Steps
{
    [Binding]
    public class AzureTableStorageValidateTableNameSteps
    {
        private string TableName;
        private Exception Exception;

        [Given(@"a valid tableName")]
        public void GivenAValidTableName()
        {
            GivenTableName("abc");
        }

        private void GivenTableName(string tableName)
        {
            TableName = tableName;
        }

        [Given(@"a invalid tableName")]
        public void GivenAInvalidTableName()
        {
            TableName = "a".PadRight(64, 'b');
        }

        [When(@"ValidateTableName is called")]
        public void WhenValidateTableNameIsCalled()
        {
            try
            {
                TableName.ValidateTableName();
            }
            catch (Exception exception)
            {
                Exception = exception;
            }
        }

        [Then(@"an exception is not thrown")]
        public void ThenAnExceptionIsNotThrown()
        {
            Exception.Should().BeNull();
        }

        [Then(@"an exception is thrown")]
        public void ThenAnExceptionIsThrown()
        {
            Exception.Should().BeOfType<ValidationException>();
        }
    }
}
