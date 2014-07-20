using System;
using AzureMagic.Tables;
using Microsoft.WindowsAzure.Storage.Table;

namespace AzureMagic.Tests.Support
{
    [NullGuard.NullGuard(NullGuard.ValidationFlags.None)]
    [EmptyStringGuard.EmptyStringGuard(EmptyStringGuard.ValidationFlags.None)]
    public class DummyTableEntity : TableEntity
    {
        public DummyTableEntity()
            : this(initializeProperties: false)
        {
        }

        public DummyTableEntity(bool initializeProperties)
            : base(Guid.NewGuid().ToPartitionKey(), "")
        {
            if (!initializeProperties)
            {
                return;
            }

            Bytes = new byte[1000];
            Boolean = true;
            DateTime = new DateTime(2000, 1, 2, 3, 4, 5, 6);
            Double = 1.23;
            Guid = Guid.NewGuid();
            Int = 2;
            Long = 3;
            String = "a string";
        }

        public byte[] Bytes { get; set; }
        public bool Boolean { get; set; }
        public DateTime DateTime { get; set; }
        public double Double { get; set; }
        public Guid Guid { get; set; }
        public int Int { get; set; }
        public long Long { get; set; }
        public string String { get; set; }
    }
}