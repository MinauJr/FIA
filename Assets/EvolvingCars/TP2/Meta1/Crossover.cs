using GeneticSharp.Domain.Chromosomes;
using System;
using System.Linq;
using UnityEngine;
using GeneticSharp.Domain.Randomizations;
using System.Collections.Generic;
using GeneticSharp.Domain.Crossovers;

namespace GeneticSharp.Runner.UnityApp.Commons
{
    public class Crossover : ICrossover
    {
        public int ParentsNumber { get; private set; }

        public int ChildrenNumber { get; private set; }

        public int MinChromosomeLength { get; private set; }

        public bool IsOrdered { get; private set; } 

        protected float crossoverProbability;

        public Crossover(float crossoverProbability) : this(2, 2, 2, true)
        {
            this.crossoverProbability = crossoverProbability;
        }

        public Crossover(int parentsNumber, int offSpringNumber, int minChromosomeLength, bool isOrdered)
        {
            ParentsNumber = parentsNumber;
            ChildrenNumber = offSpringNumber;
            MinChromosomeLength = minChromosomeLength;
            IsOrdered = isOrdered;
        }

        public IList<IChromosome> Cross(IList<IChromosome> parents)
        {
            IChromosome parent1 = parents[0];
            IChromosome parent2 = parents[1];
            IChromosome offspring1 = parent1.Clone();
            IChromosome offspring2 = parent2.Clone();

            /* YOUR CODE HERE */
            /*REPLACE THESE LINES BY YOUR CROSSOVER IMPLEMENTATION*/
            var rng = RandomizationProvider.Current;
            
            if (rng.GetDouble() <= crossoverProbability) {
                int chromosomeLength = parent1.Length;
                
                // Escolhe random os pontos de crossover
                int crossoverPoint1 = rng.GetInt(0, chromosomeLength);
                int crossoverPoint2 = rng.GetInt(0, chromosomeLength);
                
                // Garante que crossoverPoint2 é maior que crossoverPoint1
                if (crossoverPoint1 > crossoverPoint2)
                {
                    int temp = crossoverPoint1;
                    crossoverPoint1 = crossoverPoint2;
                    crossoverPoint2 = temp;
                }

                // Troca das seções entre os pontos de crossover
                for (int geneIndex = crossoverPoint1; geneIndex < crossoverPoint2; geneIndex++) {
                    offspring1.ReplaceGene(geneIndex, parent2.GetGene(geneIndex)); //mete o segmento do pai2 no filho 1
                    offspring2.ReplaceGene(geneIndex, parent1.GetGene(geneIndex)); //mete o segmento do pai1 no filho 2
                }
            }
            /*END OF YOUR CODE*/

            return new List<IChromosome> { offspring1, offspring2 };
            
        }
    }
}