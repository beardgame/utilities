﻿using System;
using System.Linq;
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
        /// <param name="costMatrix">The cost matrix, where matrix[i, j] holds the cost of assigning worker i to job j,
        ///     for all i, j. Needs to be square.</param>
        /// <returns>The minimal cost matching based on the specified cost matrix.</returns>
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
        /// <returns>The minimal cost matching based on the specified metric.</returns>
        public static int[] Run<TWorker, TJob>(TWorker[] workers, TJob[] jobs, Func<TWorker, TJob, float> getCost)
        {
            var costMatrix = new float[workers.Length, jobs.Length];
            for (var t = 0; t < jobs.Length; t++)
                for (var s = 0; s < workers.Length; s++)
                    costMatrix[s, t] = getCost(workers[s], jobs[t]);

            return Run(costMatrix);
        }

        /// <summary>
        /// Evaluates a minimal cost matching on two-dimensional vectors using the least-squares metric.
        /// </summary>
        /// <param name="from">The source vectors.</param>
        /// <param name="to">The destination vectors.</param>
        /// <returns>The minimal cost matching based on the least-squares metric.</returns>
        public static int[] Run(Vector2[] from, Vector2[] to)
        {
            return Run(from, to, (u, v) => (v - u).LengthSquared);
        }

        /// <summary>
        /// Evaluates a minimal cost matching on two-dimensional vectors using the least-squares metric.
        /// </summary>
        /// <param name="from">The source vectors.</param>
        /// <param name="to">The destination vectors.</param>
        /// <returns>The minimal cost matching based on the least-squares metric.</returns>
        public static int[] Run(Vector3[] from, Vector3[] to)
        {
            return Run(from, to, (u, v) => (v - u).LengthSquared);
        }
        #endregion

        #region Implementation
        private readonly float[,] costMatrix;
        private readonly int dim;
        private readonly float[] labelSources;
        private readonly float[] labelDests;
        private readonly int[] minSlackDestBySource;
        private readonly float[] minSlackValueBySource;
        private readonly int[] sourceMatches;
        private readonly int[] destMatches;
        private readonly int[] parentSourceByCommittedDest;
        private readonly bool[] matchedSources;

        private HungarianAlgorithm(float[,] costMatrix)
        {
            if (costMatrix.GetLength(0) != costMatrix.GetLength(1))
                throw new ArgumentException("Hungarian algorithm requires a square cost matrix.", nameof(costMatrix));
            if (costMatrix.Cast<float>().Any(f => float.IsInfinity(f) || float.IsNaN(f)))
                throw new ArgumentException("Only valid finite costs allowed.", nameof(costMatrix));
            this.costMatrix = costMatrix;
            dim = costMatrix.GetLength(0);
            labelSources = new float[dim];
            labelDests = new float[dim];
            minSlackDestBySource = new int[dim];
            minSlackValueBySource = new float[dim];
            matchedSources = new bool[dim];
            parentSourceByCommittedDest = new int[dim];
            sourceMatches = new int[dim];
            destMatches = new int[dim];

            for (var i = 0; i < dim; i++)
            {
                sourceMatches[i] = -1;
                destMatches[i] = -1;
            }
        }

        private int[] execute()
        {
            /*
             * Heuristics to improve performance: Reduce rows and columns by their
             * smallest element, compute an initial non-zero dual feasible solution and
             * create a greedy matching from workers to jobs of the cost matrix.
             **/
            reduce();
            computeInitialFeasibleSolution();
            greedyMatch();

            var t = firstUnmatchedSource();
            while (t < dim)
            {
                initializePhase(t);
                executePhase();
                t = firstUnmatchedSource();
            }

            var result = new int[dim];
            Array.Copy(sourceMatches, result, dim);
            for (var i = 0; i < result.Length; i++)
                if (result[i] >= dim)
                    result[i] = -1;

            return result;
        }

        /// <summary>
        /// Reduces the cost matrix by subtracting the smallest element of each row from
        /// all elements of the row as well as the smallest element of each column from
        /// all elements of the column.
        /// Note that an optimal assignment for a reduced cost matrix is optimal for the
        /// original cost matrix.
        /// </summary>
        private void reduce()
        {
            for (var s = 0; s < dim; s++)
            {
                var min = float.PositiveInfinity;
                for (var t = 0; t < dim; t++)
                {
                    if (costMatrix[s, t] < min)
                    {
                        min = costMatrix[s, t];
                    }
                }
                for (var t = 0; t < dim; t++)
                {
                    costMatrix[s, t] -= min;
                }
            }

            var mins = new float[dim];
            for (var t = 0; t < dim; t++)
            {
                mins[t] = float.PositiveInfinity;
            }
            for (var s = 0; s < dim; s++)
            {
                for (var t = 0; t < dim; t++)
                {
                    if (costMatrix[s, t] < mins[t])
                    {
                        mins[t] = costMatrix[s, t];
                    }
                }
            }
            for (var s = 0; s < dim; s++)
            {
                for (var t = 0; t < dim; t++)
                {
                    costMatrix[s, t] -= mins[t];
                }
            }
        }

        /// <summary>
        /// Compute an initial feasible solution by assigning zero labels to the workers and by assigning to each job a
        /// label equal to the minimum cost among its incident edges.
        /// </summary>
        private void computeInitialFeasibleSolution()
        {
            for (var t = 0; t < dim; t++)
                labelDests[t] = float.PositiveInfinity;

            for (var s = 0; s < dim; s++)
                for (var t = 0; t < dim; t++)
                    if (costMatrix[s, t] < labelDests[t])
                        labelDests[t] = costMatrix[s, t];
        }

        /// <summary>
        /// Greedily find a matching to start the algorithm with.
        /// </summary>
        private void greedyMatch()
        {
            for (var s = 0; s < dim; s++)
            {
                for (var t = 0; t < dim; t++)
                {
                    if (sourceMatches[s] == -1 && destMatches[t] == -1
                        // ReSharper disable once CompareOfFloatsByEqualityOperator
                        && costMatrix[s, t] - labelSources[s] - labelDests[t] == 0)
                    {
                        match(s, t);
                    }
                }
            }
        }

        /// <summary>
        /// Initializes the next phase of the algorithm by clearing the committed
        /// workers and jobs sets and by initializing the slack arrays to the values
        /// corresponding to the specified root worker.
        /// </summary>
        /// <param name="s">The worker at which to root the next phase.</param>
        private void initializePhase(int s)
        {
            for (var i = 0; i < matchedSources.Length; i++)
            {
                matchedSources[i] = false;
                parentSourceByCommittedDest[i] = -1;
            }

            matchedSources[s] = true;
            for (var t = 0; t < dim; t++)
            {
                minSlackValueBySource[t] = costMatrix[s, t] - labelSources[s]
                    - labelDests[t];
                minSlackDestBySource[t] = s;
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
                var minSlackValue = float.PositiveInfinity;

                for (int t = 0; t < dim; t++)
                {
                    if (parentSourceByCommittedDest[t] != -1) continue;
                    if (!(minSlackValueBySource[t] < minSlackValue)) continue;

                    minSlackValue = minSlackValueBySource[t];
                    minSlackSource = minSlackDestBySource[t];
                    minSlackDest = t;
                }

                if (minSlackValue > 0)
                    updateLabeling(minSlackValue);

                parentSourceByCommittedDest[minSlackDest] = minSlackSource;
                if (destMatches[minSlackDest] == -1)
                {
                    // An augmenting path has been found.
                    var committedJob = minSlackDest;
                    var parentWorker = parentSourceByCommittedDest[committedJob];
                    while (true)
                    {
                        var temp = sourceMatches[parentWorker];
                        match(parentWorker, committedJob);
                        committedJob = temp;
                        if (committedJob == -1)
                        {
                            break;
                        }
                        parentWorker = parentSourceByCommittedDest[committedJob];
                    }
                    return;
                }

                // Update slack values since we increased the size of the committed workers set.
                var worker = destMatches[minSlackDest];
                matchedSources[worker] = true;
                for (var j = 0; j < dim; j++)
                {
                    if (parentSourceByCommittedDest[j] != -1) continue;

                    var slack = costMatrix[worker, j] - labelSources[worker] - labelDests[j];

                    if (!(minSlackValueBySource[j] > slack)) continue;

                    minSlackValueBySource[j] = slack;
                    minSlackDestBySource[j] = worker;
                }
            }
        }

        /// <summary>
        /// Returns the first unmatched source (or dim if none).
        /// </summary>
        /// <returns></returns>
        private int firstUnmatchedSource()
        {
            int s;
            for (s = 0; s < dim; s++)
            {
                if (sourceMatches[s] == -1)
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
            sourceMatches[s] = t;
            destMatches[t] = s;
        }

        /// <summary>
        /// Updates labels with the specified slack by adding the slack value for
        /// committed workers and by subtracting the slack value for committed jobs. In
        /// addition, update the minimum slack values appropriately.
        /// </summary>
        /// <param name="slack"></param>
        private void updateLabeling(float slack)
        {
            for (var s = 0; s < dim; s++)
            {
                if (matchedSources[s])
                {
                    labelSources[s] += slack;
                }
            }

            for (var t = 0; t < dim; t++)
            {
                if (parentSourceByCommittedDest[t] != -1)
                {
                    labelDests[t] -= slack;
                }
                else
                {
                    minSlackValueBySource[t] -= slack;
                }
            }
        }
        #endregion
    }
}