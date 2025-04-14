using System.Diagnostics; 
using GeneticSharp.Domain.Chromosomes; 
using GeneticSharp.Domain.Mutations; 
using GeneticSharp.Runner.UnityApp.Car; 
using GeneticSharp.Domain.Randomizations;
using System;


public class Mutation : IMutation 
{
    public bool IsOrdered { get; private set; } // Indica se o operador é ordenado, ou seja, se pode manter a ordem do cromossoma
    private double std = 1; // Desvio padrão da distribuição gaussiana

    // Construtor que recebe o desvio padrão como parâmetro
    public Mutation(double std)
    {
        IsOrdered = true; //operador é ordenado
        this.std = std; //desvio padrão
    }

    //define o desvio padrão como 1 por padrão
    public Mutation()
    {
        IsOrdered = true; //operador é ordenado.
    }

    //aplicar a mutação em um cromossoma com uma certa probabilidade
    public void Mutate(IChromosome chromosome, float probability)
    { 
        for(int i = 0; i < chromosome.Length; i++){  //comprimento do cromossoma
            if(RandomizationProvider.Current.GetDouble() <= probability){
                double geneValue = ApplyGaussianMutation((double) chromosome.GetGene(i).Value, std);
                chromosome.ReplaceGene(i, new Gene(geneValue));
            }
        }
    }

    //aplicar uma mutação gaussiana a um valor de gene
    protected double ApplyGaussianMutation(double mean, double std)
    {
        // Aplica uma mutação gaussiana ao valor do gene
        double x1 = RandomizationProvider.Current.GetDouble(0, 1);
        double x2 = RandomizationProvider.Current.GetDouble(0, 1);
        double y1 = Math.Sqrt(-2.0 * Math.Log(x1)) * Math.Cos(2.0 * Math.PI * x2);  //default
        return y1 * std + mean;
    }
}