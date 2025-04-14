using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using GeneticSharp.Domain.Chromosomes;
using GeneticSharp.Domain.Populations;
using GeneticSharp.Domain.Randomizations;
using GeneticSharp.Domain.Selections;
using GeneticSharp.Infrastructure.Framework.Texts;
using GeneticSharp.Runner.UnityApp.Car;
using UnityEngine;

public class ParentSelection : SelectionBase{
    public ParentSelection() : base(2)
    {
    }
    protected override IList<IChromosome> PerformSelectChromosomes(int n, Generation generation)
    {
        //converte os cromossomas da geração atual para o tipo CarChromosome
        IList<CarChromosome> population = generation.Chromosomes.Cast<CarChromosome>().ToList();
        //lista de cromossomas para guardar os pais escolhidos
        IList<IChromosome> parents = new List<IChromosome>();
        
        double sumFitness = 0.0; //soma total de fitness da população

        //calcula a soma total de fitness de todos os cromossomas na população
        foreach (var chromosome in population){
            sumFitness += chromosome.Fitness;
        }

        //escolhe n pais com base no seu fitness (método da roleta)
        for (int i = 0; i < n; i++){
            //valor aleatório entre 0 e a soma total de fitness
            double randomValue = RandomizationProvider.Current.GetDouble() * sumFitness; 
            double cumulativeFitness = 0.0; //para acumular o fitness

            //itera sobre a população para selecionar um cromossoma com base no valor aleatório
            foreach (var chromosome in population){
                cumulativeFitness += chromosome.Fitness;
                //se o fitness acumulado for >= valor aleatório, adiciona esse cromossoma
                if (cumulativeFitness >= randomValue){
                    parents.Add(chromosome);
                    break;
                }
            }
        }
        return parents; //lista de cromossomas pais escolhidos
    }
}
