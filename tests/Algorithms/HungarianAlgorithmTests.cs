using System.Runtime.Remoting.Messaging;
using Bearded.Utilities.Algorithms;
using OpenTK;
using Xunit;

namespace Bearded.Utilities.Tests.Algorithms
{
    public class HungarianAlgorithmTests
    {
        [Fact]
        public void Run_EmptyCostMatrix_DoesntFail()
        {
            Assert.Empty(HungarianAlgorithm.Run(new float[0, 0]));
        }

        [Fact]
        public void Run_OneWorker_AssignsWorker()
        {
            Assert.Equal(HungarianAlgorithm.Run(new[,] {{5f}}), new[] {0});
        }

        [Fact]
        public void Run_TwoWorkers_AssignsLowestCostWorker()
        {
            Assert.Equal(HungarianAlgorithm.Run(new[,] {{1f, 5f}, {5f, 1f}}), new[] {0, 1});
        }

        [Fact]
        public void Run_NoJobs_LeavesWorkerUnassigned()
        {
            Assert.Equal(HungarianAlgorithm.Run(new[] {Vector2.Zero}, new Vector2[0]), new[] {-1});
        }

        [Fact]
        public void Run_MoreWorkersThanJobs_LeavesWorkersUnassigned()
        {
            Assert.Equal(
                HungarianAlgorithm.Run(
                    new [] {Vector2.Zero, Vector2.UnitX, Vector2.UnitY},
                    new [] {.2f * Vector2.UnitY, .8f * Vector2.UnitY}),
                new [] {0, 1, -1});
        }
    }
}