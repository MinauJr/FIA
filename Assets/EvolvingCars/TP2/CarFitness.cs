using GeneticSharp.Domain.Fitnesses;
using GeneticSharp.Domain.Chromosomes;
using System.Threading;
using UnityEngine;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System;
using System.Linq;

namespace GeneticSharp.Runner.UnityApp.Car
{
    public class CarFitness : IFitness
    {
        public CarFitness()
        {
            ChromosomesToBeginEvaluation = new BlockingCollection<CarChromosome>();
            ChromosomesToEndEvaluation = new BlockingCollection<CarChromosome>();
        }

        public BlockingCollection<CarChromosome> ChromosomesToBeginEvaluation { get; private set; }
        public BlockingCollection<CarChromosome> ChromosomesToEndEvaluation { get; private set; }
        public double Evaluate(IChromosome chromosome)
        {
            var c = chromosome as CarChromosome;
            ChromosomesToBeginEvaluation.Add(c);

            float fitness = 0; 
            do
            {
                Thread.Sleep(1000);
                
                float Distance = c.Distance;
                float EllapsedTime = c.EllapsedTime;
                float NumberOfWheels = c.NumberOfWheels;
                float CarMass = c.CarMass;
                int RoadCompleted = c.RoadCompleted ? 1 : 0;

                List<float> Velocities = c.Velocities;
                float SumVelocities = c.SumVelocities;
                
                List<float> Accelerations = c.Accelerations;
                float SumAccelerations = c.SumAccelerations;

                List<float> Forces = c.Forces;
                float SumTotalForces = c.SumForces;

                /*YOUR CODE HERE*/
                /*Note que é executado ao longo da simulação*/

                float maxVelocity = Velocities.Max();
                float avgVelocity = c.Velocities.Average();             
                
                int fitnessFunction = 3;
                switch (fitnessFunction)
                {
                    // Gap Road
                    case 1:
                        //fitness = (maxVelocity * Distance) / CarMass;
                        fitness = CarMass*CarMass*CarMass * Distance*Distance;
                        //fitness = (float)Math.Pow((Distance * 0.9),3) + RoadCompleted * 500000 + maxVelocity * 65 - CarMass * 30;
                        break;

                    // Hill Road
                    case 2:
                        //fitness = (RoadCompleted * 10000) + (Distance * maxVelocity) - (SumTotalForces * CarMass);
                        //fitness = Distance * maxVelocity + RoadCompleted * 10000;  
                        fitness = (float)Math.Pow(Distance,3) + RoadCompleted * 1500 + maxVelocity * 75 + CarMass * 50;
                        break;

                    // Rocky Hill Road
                    case 3:
                        fitness = Distance*Distance*Distance * maxVelocity * CarMass; 
                        break;

                    //Meta 1
                    default:
                        if (RoadCompleted == 1)
                        {
                            //extra por chegar a meta e rapido
                            fitness = 10000 + (Distance / EllapsedTime) + maxVelocity;  //velocidade media para incentivar a rapidez e maxVelocity para sublinhar carros que atinjam altas velocidades 
                        }
                        else
                        {
                            fitness = (Distance * maxVelocity) / (1 + EllapsedTime);  //quanto mais tempo demora, menor o fitness
                        }
                        
                        break;
                }

                /*END OF YOUR CODE*/

                c.Fitness = fitness;

            } while (!c.Evaluated);

            ChromosomesToEndEvaluation.Add(c);


            do
            {
                Thread.Sleep(1000);
            } while (!c.Evaluated);

            /*O valor da variável fitness é o valor de aptidão do indivíduo*/

            return fitness;
        }
    }
}