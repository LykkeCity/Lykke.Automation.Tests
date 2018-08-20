using AzureStorage;
using AzureStorage.Blob;
using Lykke.SettingsReader;
using System;
using System.Threading.Tasks;

namespace XUnitTestData.Repositories.AlgoStore
{
    public class AlgoBlobRepository
    {
        IBlobStorage blobStorage;
        private const string BlobContainer = "algo-store-binary";

        public AlgoBlobRepository(IReloadingManager<string> connectionStringManager, TimeSpan? maxExecutionTimeout = null)
        {
            blobStorage = AzureBlobStorage.Create(connectionStringManager, maxExecutionTimeout);
        }

        public async Task<bool> CheckIfBlobExists(string blobName, string binaryType)
        {
            return await blobStorage.HasBlobAsync(BlobContainer, blobName+binaryType);
        }
    }
}
