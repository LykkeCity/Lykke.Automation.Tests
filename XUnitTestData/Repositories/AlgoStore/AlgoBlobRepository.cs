using AzureStorage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using XUnitTestData.Domains.AlgoStore;

namespace XUnitTestData.Repositories.AlgoStore
{
    public class AlgoBlobRepository : AzureStorage.Blob.AzureBlobStorage 
    {
        private const string BlobContainer = "algo-store-binary";

        public AlgoBlobRepository(string connectionString, TimeSpan? maxExecutionTimeout = null) : base(connectionString, maxExecutionTimeout)
        {
        }

        public async Task<bool> CheckIfBlobExists(string blobName)
        {
            return await HasBlobAsync(BlobContainer, blobName);
          
        }

    }
}
