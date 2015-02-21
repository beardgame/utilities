using System;
using OpenTK;

namespace Bearded.Utilities.Algorithms
{
    /// <summary>
    /// The Hungarian algorithm is a cubic algorithm that solves the assignment problem.
    /// The algorithm calculates a maximum matching between workers and jobs, while minimising the total cost.
    /// </summary>
    /// <remarks>The original implementation was by Kevin L. Stern.</remarks>
    public sealed class HungarianAlgorithm
    {
        #region Public static interface
        /// <summary>
        /// Evaluates a minimal cost matching using the given cost matrix.
        /// </summary>
        /// <param name="costMatrix">A matrix of costs of assigning jobs to workers.</param>
        /// <returns>The minimal cost matching based on the specified cost matrix. A value of -1 means the worker is not assigned.</returns>
        public static int[] Run(float[,] costMatrix)
        {
            var instance = new HungarianAlgorithm(costMatrix);
            return instance.execute();
        }

        /// <summary>
        /// Evaluates a minimal cost matching using the given metric.
        /// </summary>
        /// <typeparam name="TWorker">The type representing the workers.</typeparam>
        /// <typeparam name="TJob">The type representing the jobs.</typeparam>
        /// <param name="workers">The elements representing the workers.</param>
        /// <param name="jobs">The elements representing the jobs.</param>
        /// <param name="getCost">A function that calculates the cost of assigning a job to a worker.</param>
        /// <returns>The minimal cost matching based on the specified metric. A value of -1 means the worker is not assigned.</returns>
        public static int[] Run<TWorker, TJob>(TWorker[] workers, TJob[] jobs, Func<TWorker, TJob, float> getCost)
        {
            var costMatrix = new float[workers.Length, jobs.Length];
            for (int t = 0; t < jobs.Length; t++)
                for (int s = 0; s < workers.Length; s++)
                    costMatrix[s, t] = getCost(workers[s], jobs[t]);

            return HungarianAlgorithm.Run(costMatrix);
        }

        /// <summary>
        /// Evaluates a minimal cost matching on two-dimensional vectors using the least-squares metric.
        /// </summary>
        /// <param name="from">The source vectors.</param>
        /// <param name="to">The destination vectors.</param>
        /// <returns>The minimal cost matching based on the least-squares metric. A value of -1 means the source vector is not assigned.</returns>
        public static int[] Run(Vector2[] from, Vector2[] to)
        {
            return HungarianAlgorithm.Run(from, to, (u, v) => (v - u).LengthSquared);
        }

        /// <summary>
        /// Evaluates a minimal cost matching on two-dimensional vectors using the least-squares metric.
        /// </summary>
        /// <param name="from">The source vectors.</param>
        /// <param name="to">The destination vectors.</param>
        /// <returns>The minimal cost matching based on the least-squares metric. A value of -1 means the source vector is not assigned.</returns>
        public static int[] Run(Vector3[] from, Vector3[] to)
        {
            return HungarianAlgorithm.Run(from, to, (u, v) => (v - u).LengthSquared);
        }
        #endregion

        #region Implementation
        #region Fields
        private readonly float[,] costMatrix;
        private readonly int rows, cols, dim;
        private readonly float[] labelSources;
        private readonly float[] labelDests;
        private readonly int[] minSlackDestBySource;
        private readonly float[] minSlackValueBySource;
        private readonly int[] sourceMatches;
        private readonly int[] destMatches;
        private readonly int[] parentSourceByCommittedDest;
        private readonly bool[] matchedSources;
        #endregion

        #region Initializaion
        /// <summary>
        /// Construct an instance of the algorithm.
        /// </summary>
        /// <param name="costMatrix">the cost matrix, where matrix[i][j] holds the cost of assigning worker i to job j, for all i, j.</param>
        private HungarianAlgorithm(float[,] costMatrix)
        {
            this.costMatrix = costMatrix;
            this.rows = this.costMatrix.GetLength(0);
            this.cols = this.costMatrix.GetLength(1);
            this.dim = System.Math.Max(this.rows, this.cols);
            this.labelSources = new float[this.dim];
            this.labelDests = new float[this.dim];
            this.minSlackDestBySource = new int[this.dim];
            this.minSlackValueBySource = new float[this.dim];
            this.matchedSources = new bool[this.dim];
            this.parentSourceByCommittedDest = new int[this.dim];
            this.sourceMatches = new int[this.dim];
            this.destMatches = new int[this.dim];

            for (int i = 0; i < this.dim; i++)
            {
                this.sourceMatches[i] = -1;
                this.destMatches[i] = -1;
            }
        }
        #endregion

        #region Execute
        /// <summary>
        /// Executes the algorithm.
        /// </summary>
        /// <returns>The minimum cost matching of workers to jobs based upon the provided cost matrix. A matching value of -1 indicates that the corresponding worker is unassigned.</returns>
        private int[] execute()
        {
            /*
             * Heuristics to improve performance: Reduce rows and columns by their
             * smallest element, compute an initial non-zero dual feasible solution and
             * create a greedy matching from workers to jobs of the cost matrix.
             **/
            this.reduce();
            this.computeInitialFeasibleSolution();
            this.greedyMatch();

            int t = this.firstUnmatchedSource();
            while (t < dim)
            {
                this.initializePhase(t);
                this.executePhase();
                t = this.firstUnmatchedSource();
            }

            var result = new int[rows];
            Array.Copy(this.sourceMatches, result, this.rows);
            for (int i = 0; i < result.Length; i++)
                if (result[i] >= this.cols)
                    result[i] = -1;

            return result;
        }
        #endregion

        #region Pre-processing
        /// <summary>
        /// Reduces the cost matrix by subtracting the smallest element of each row from
        /// all elements of the row as well as the smallest element of each column from
        /// all elements of the column.
        /// Note that an optimal assignment for a reduced cost matrix is optimal for the
        /// original cost matrix.
        /// </summary>
        private void reduce()
        {
            for (int s = 0; s < this.dim; s++)
            {
                float min = float.PositiveInfinity;
                for (int t = 0; t < this.dim; t++)
                {
                    if (this.costMatrix[s, t] < min)
                    {
                        min = this.costMatrix[s, t];
                    }
                }
                for (int t = 0; t < this.dim; t++)
                {
                    this.costMatrix[s, t] -= min;
                }
            }

            var mins = new float[dim];
            for (int t = 0; t < this.dim; t++)
            {
                mins[t] = float.PositiveInfinity;
            }
            for (int s = 0; s < this.dim; s++)
            {
                for (int t = 0; t < this.dim; t++)
                {
                    if (this.costMatrix[s, t] < mins[t])
                    {
                        mins[t] = this.costMatrix[s, t];
                    }
                }
            }
            for (int s = 0; s < this.dim; s++)
            {
                for (int t = 0; t < this.dim; t++)
                {
                    this.costMatrix[s, t] -= mins[t];
                }
            }
        }

        /// <summary>
        /// Compute an initial feasible solution by assigning zero labels to the workers and by assigning to each job a label equal to the minimum cost among its incident edges.
        /// </summary>
        private void computeInitialFeasibleSolution()
        {
            for (int t = 0; t < this.dim; t++)
                this.labelDests[t] = float.PositiveInfinity;

            for (int s = 0; s < this.dim; s++)
                for (int t = 0; t < this.dim; t++)
                    if (costMatrix[s, t] < this.labelDests[t])
                        this.labelDests[t] = this.costMatrix[s, t];
        }

        /// <summary>
        /// Greedily find a matching to start the algorithm with.
        /// </summary>
        private void greedyMatch()
        {
            for (int s = 0; s < this.dim; s++)
            {
                for (int t = 0; t < this.dim; t++)
                {
                    if (this.sourceMatches[s] == -1 && this.destMatches[t] == -1
                        && this.costMatrix[s, t] - this.labelSources[s] - this.labelDests[t] == 0)
                    {
                        this.match(s, t);
                    }
                }
            }
        }
        #endregion

        #region Phase
        /// <summary>
        /// Initializes the next phase of the algorithm by clearing the committed
        /// workers and jobs sets and by initializing the slack arrays to the values
        /// corresponding to the specified root worker.
        /// </summary>
        /// <param name="s">The worker at which to root the next phase.</param>
        private void initializePhase(int s)
        {
            for (int i = 0; i < this.matchedSources.Length; i++)
            {
                this.matchedSources[i] = false;
                this.parentSourceByCommittedDest[i] = -1;
            }

            this.matchedSources[s] = true;
            for (int t = 0; t < dim; t++)
            {
                this.minSlackValueBySource[t] = this.costMatrix[s, t] - this.labelSources[s]
                    - this.labelDests[t];
                this.minSlackDestBySource[t] = s;
            }
        }

        /// <summary>
        /// Execute a single phase of the algorithm. A phase of the Hungarian algorithm
        /// consists of building a set of committed workers and a set of committed jobs
        /// from a root unmatched worker by following alternating unmatched/matched
        /// zero-slack edges. If an unmatched job is encountered, then an augmenting
        /// path has been found and the matching is grown. If the connected zero-slack
        /// edges have been exhausted, the labels of committed workers are increased by
        /// the minimum slack among committed workers and non-committed jobs to create
        /// more zero-slack edges (the labels of committed jobs are simultaneously
        /// decreased by the same amount in order to maintain a feasible labeling).
        /// </summary>
        /// <remarks>
        /// The runtime of a single phase of the algorithm is O(n^2), where n is the
        /// dimension of the internal square cost matrix, since each edge is visited at
        /// most once and since increasing the labeling is accomplished in time O(n) by
        /// maintaining the minimum slack values among non-committed jobs. When a phase
        /// completes, the matching will have increased in size.
        /// </remarks>
        private void executePhase()
        {
            while (true)
            {
                int minSlackSource = -1, minSlackDest = -1;
                float minSlackValue = float.PositiveInfinity;

                for (int t = 0; t < dim; t++)
                {
                    if (this.parentSourceByCommittedDest[t] != -1) continue;
                    if (!(this.minSlackValueBySource[t] < minSlackValue)) continue;

                    minSlackValue = this.minSlackValueBySource[t];
                    minSlackSource = this.minSlackDestBySource[t];
                    minSlackDest = t;
                }

                if (minSlackValue > 0)
                    this.updateLabeling(minSlackValue);

                this.parentSourceByCommittedDest[minSlackDest] = minSlackSource;
                if (this.destMatches[minSlackDest] == -1)
                {
                    // An augmenting path has been found.
                    int committedJob = minSlackDest;
                    int parentWorker = this.parentSourceByCommittedDest[committedJob];
                    while (true)
                    {
                        int temp = this.sourceMatches[parentWorker];
                        match(parentWorker, committedJob);
                        committedJob = temp;
                        if (committedJob == -1)
                        {
                            break;
                        }
                        parentWorker = this.parentSourceByCommittedDest[committedJob];
                    }
                    return;
                }

                // Update slack values since we increased the size of the committed workers set.
                int worker = this.destMatches[minSlackDest];
                this.matchedSources[worker] = true;
                for (int j = 0; j < this.dim; j++)
                {
                    if (this.parentSourceByCommittedDest[j] != -1) continue;

                    float slack = this.costMatrix[worker, j] - this.labelSources[worker]
                        - this.labelDests[j];

                    if (!(this.minSlackValueBySource[j] > slack)) continue;

                    this.minSlackValueBySource[j] = slack;
                    this.minSlackDestBySource[j] = worker;
                }
            }
        }
        #endregion

        #region Helpers
        /// <summary>
        /// Returns the first unmatched source (or dim if none).
        /// </summary>
        /// <returns></returns>
        private int firstUnmatchedSource()
        {
            int s;
            for (s = 0; s < dim; s++)
            {
                if (this.sourceMatches[s] == -1)
                {
                    break;
                }
            }
            return s;
        }

        /// <summary>
        /// Matches source s to destination t.
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        private void match(int s, int t)
        {
            this.sourceMatches[s] = t;
            this.destMatches[t] = s;
        }

        /// <summary>
        /// Updates labels with the specified slack by adding the slack value for
        /// committed workers and by subtracting the slack value for committed jobs. In
        /// addition, update the minimum slack values appropriately.
        /// </summary>
        /// <param name="slack"></param>
        private void updateLabeling(float slack)
        {
            for (int s = 0; s < dim; s++)
            {
                if (this.matchedSources[s])
                {
                    this.labelSources[s] += slack;
                }
            }

            for (int t = 0; t < dim; t++)
            {
                if (this.parentSourceByCommittedDest[t] != -1)
                {
                    this.labelDests[t] -= slack;
                }
                else
                {
                    this.minSlackValueBySource[t] -= slack;
                }
            }
        }
        #endregion
        #endregion
    }
}