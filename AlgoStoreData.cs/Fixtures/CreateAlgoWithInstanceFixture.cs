using AlgoStoreData.DTOs.InstanceData;
using NUnit.Framework;
using System.Threading.Tasks;
using XUnitTestData.Enums;

namespace AlgoStoreData.Fixtures
{
    [TestFixture]
    public partial class CreateAlgoWithInstanceFixture : AlgoStoreTestDataFixture
    {
        public AlgoInstanceType instanceType = AlgoInstanceType.Live;

        private string message;
        
        protected InstanceDataDTO instanceForAlgo;

        [OneTimeSetUp]
        public void Initialize()
        {
            PrepareTestData().Wait();
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            
        }

        private async Task PrepareTestData()
        {
            await CreateAlgoAndStartInstance(1);
            
            // Wait for all instances to be started // If a lot intances are created this could take a long time
            foreach (var instanceData in instancesList)
            {
                await WaitAlgoInstanceToStart(instanceData.InstanceId);
            }
        }

        public async Task CreateAlgoAndStartInstance(int numberOfInstances)
        {
            for (int i = 0; i < numberOfInstances; i++)
            {
                // Create algo
                algoData = await CreateAlgo();

                // Create instance
                InstanceDataDTO instanceData = await SaveInstance(algoData, instanceType);
                postInstanceData = instanceData;

                // Deploy the instance
                await DeployInstance(instanceData);
            }
        }
    }
}
