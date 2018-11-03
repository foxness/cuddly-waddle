using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TSPApp
{
    class TSP
    {
        private static readonly Random random = new Random();
        private List<List<int>> population;
        private List<float[]> vertices;
        private readonly int startEndVertex;
        private readonly int populationSize;
        private readonly int mutationCount;
        private readonly double mutationProbability;

        public int CurrentGeneration { get; private set; }

        public TSP(List<float[]> vertices, int startEndVertex, int populationSize, int mutationCount, double mutationProbability)
        {
            this.vertices = vertices;
            this.startEndVertex = startEndVertex;
            this.populationSize = populationSize;
            this.mutationCount = mutationCount;
            this.mutationProbability = mutationProbability;
        }

        public void InitWithRandomPopulation()
        {
            CurrentGeneration = 0;
            population = GetRandomPopulation();
        }

        public void NextGeneration()
        {
            CurrentGeneration++;
            population = Selected(population);
            population = Crossovered(population);
            population = Mutated(population);
        }

        public (List<int>, double) GetBestState() => GetBestState(population);

        //public void Run(int generationCount)
        //{


        //    //var (bestState, bestFitness) = GetBestState(population);
        //    //Console.WriteLine(InfoToString(0, bestState, bestFitness));

        //    for (int t = 0; t < generationCount; ++t)
        //    {
        //        population = Selected(population);
        //        population = Crossovered(population);
        //        population = Mutated(population);

        //        if ((t + 1) % (1000 / populationSize) == 0 || t == generationCount - 1)
        //        {
        //            //(bestState, bestFitness) = GetBestState(population);
        //            //onReportGeneration(bestState);
        //            //Console.WriteLine(InfoToString(t + 1, bestState, bestFitness));
        //        }
        //    }
        //}

        private (List<int>, double) GetBestState(List<List<int>> population)
        {
            return population.Select(x => (x, GetFitness(x))).OrderByDescending(x => x.Item2).First();
        }

        private List<List<int>> Selected(List<List<int>> population)
        {
            var gradient = Gradient(population.Count);
            var sortedPopulation = FitSorted(population);
            var newPopulation = new List<List<int>>();

            for (int i = 0; i < sortedPopulation.Count; ++i)
            {
                if (gradient[i])
                {
                    newPopulation.Add(sortedPopulation[i]);
                }
            }

            return newPopulation;
        }

        private List<List<int>> Crossovered(List<List<int>> population)
        {
            var offspring = new List<List<int>>();

            while (population.Count + offspring.Count < populationSize)
            {
                var parents = Enumerable.Range(0, 2).Select(x => population.RandomElement(random)).ToList();
                var child = Mate(parents);
                offspring.Add(child);
            }

            return population.Concat(offspring).ToList();
        }

        private List<List<int>> Mutated(List<List<int>> population)
        {
            var newPopulation = FitSorted(population);
            var gradient = Gradient(newPopulation.Count);

            for (int i = 0; i < newPopulation.Count; ++i)
            {
                var shouldNotMutate = gradient[i];
                if (!shouldNotMutate)
                {
                    newPopulation[i] = MutatedState(newPopulation[i]);
                }
            }

            return newPopulation;
        }

        private List<int> Swapped(List<int> list, int i, int j)
        {
            var newList = new List<int>(list);

            var tmp = newList[i];
            newList[i] = newList[j];
            newList[j] = tmp;

            return newList;
        }

        private List<int> MutatedState(List<int> state)
        {
            var mutated = state;

            for (int i = 0; i < mutationCount; ++i)
            {
                if (random.NextDouble() < mutationProbability)
                {
                    var randomLeftIndex = random.Next(1, mutated.Count - 2);
                    mutated = Swapped(mutated, randomLeftIndex, randomLeftIndex + 1);
                }
            }

            return mutated;
        }

        private List<int> Mate(List<List<int>> parents_)
        {
            var parents = Copy(parents_);

            foreach (var parent in parents)
            {
                parent.RemoveAt(0);
                parent.RemoveAt(parent.Count - 1);
            }

            var adjacencyMatrices = new List<Dictionary<int, List<int>>>();

            foreach (var parent in parents)
            {
                var adjacencyMatrix = new Dictionary<int, List<int>>();

                for (int i = 0; i < parent.Count; ++i)
                {
                    var neighbors = new List<int>();

                    for (int j = -1; j <= 1; j += 2)
                    {
                        int ni = i + j;
                        if (ni >= 0 && ni < parent.Count)
                        {
                            neighbors.Add(parent[ni]);
                        }
                    }

                    adjacencyMatrix.Add(parent[i], neighbors);
                }

                adjacencyMatrices.Add(adjacencyMatrix);
            }

            var unionMatrix = new Dictionary<int, List<int>>();
            foreach (var key in adjacencyMatrices[0].Keys)
            {
                var united = adjacencyMatrices[0][key].Union(adjacencyMatrices[1][key]).ToList();
                unionMatrix.Add(key, united);
            }

            var child = new List<int>();
            var node = parents.RandomElement(random)[0];

            while (true)
            {
                child.Add(node);

                if (child.Count == parents[0].Count)
                {
                    break;
                }

                foreach (var key in unionMatrix.Keys)
                {
                    unionMatrix[key].Remove(node);
                }

                var newNode = 0;
                if (unionMatrix[node].Count > 0)
                {
                    var minNeighborCount = unionMatrix[node].Min(x => unionMatrix[x].Count);
                    var minNeighbors = new List<int>();
                    for (int i = 0; i < unionMatrix[node].Count; ++i)
                    {
                        if (unionMatrix[unionMatrix[node][i]].Count == minNeighborCount)
                        {
                            minNeighbors.Add(unionMatrix[node][i]);
                        }
                    }

                    newNode = minNeighbors.RandomElement(random);
                }
                else
                {
                    newNode = parents[0].Where(x => !child.Contains(x)).ToList().RandomElement(random);
                }

                node = newNode;
            }

            child.Insert(0, startEndVertex);
            child.Add(startEndVertex);

            return child;
        }

        private List<List<int>> Copy(List<List<int>> list)
        {
            var newList = new List<List<int>>();

            foreach (var element in list)
                newList.Add(new List<int>(element));

            return newList;
        }

        private List<List<int>> FitSorted(List<List<int>> population)
        {
            return population.OrderByDescending(state => GetFitness(state)).ToList();
        }

        private double GetFitness(List<int> state)
        {
            double fitness = 0;

            for (int i = 0; i < state.Count - 1; ++i)
            {
                var a = vertices[state[i]];
                var b = vertices[state[i + 1]];
                var distance = Math.Sqrt(Math.Pow(a[0] - b[0], 2) + Math.Pow(a[1] - b[1], 2));
                fitness -= distance;
            }

            return fitness;
        }

        private List<List<int>> GetRandomPopulation()
        {
            return Enumerable.Range(0, populationSize).Select(x => GetRandomState()).ToList();
        }

        private double Ease(double x) => (Math.Sin(Math.PI * (x - 0.5)) + 1) / 2;

        private List<bool> Gradient(int length)
        {
            return Enumerable.Range(0, length).Select(i => Ease((double)i / length) < random.NextDouble()).ToList();
        }

        private List<int> GetRandomState()
        {
            var shuffled = Enumerable.Range(0, vertices.Count).Where(x => x != startEndVertex).ToList().Shuffled(random);
            shuffled.Insert(0, startEndVertex);
            shuffled.Add(startEndVertex);
            return shuffled;
        }
    }
}
