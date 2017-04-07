/*
 * 
 * Narayana's Cows
 *
 *  1989
 *  Narayana was an Indian mathematician in the 14th century, who proposed the following problem: 
 *  A cow produces one calf every year. 
 *  Beginning in its fourth year, each calf produces one calf at the beginning of each year. 
 *  How many cows are there altogether after, for example, 17 years?
 *
**/

using System;
using System.Collections.Generic;
using System.Linq;

namespace Narayana
{
    class Program
    {
        static void Main(string[] args)
        {
            var cow = new Cow(CowTypes.Cow, null);

            for (var year = 0; year < 17; year++)
            {
                cow.AddYear();
            }

            var cows = Cow.GetCows(cow).ToList();
            var numbersPerGeneration = GetNumberOfCowsPerGeneration(cows);

            foreach (var npg in numbersPerGeneration)
            {
                Console.WriteLine($"Cow generation {npg.Key} has {npg.Value} calves");
            }
            Console.WriteLine($"For a total number of cows of {cows.Count}");
            Console.ReadKey();
        }

        private static Dictionary<int, int> GetNumberOfCowsPerGeneration(IEnumerable<Cow> cows)
        {
            var numbersPerGeneration = new Dictionary<int, int>();

            foreach (var c in cows)
            {
                if (numbersPerGeneration.ContainsKey(c.Generation))
                    numbersPerGeneration[c.Generation]++;
                else
                    numbersPerGeneration.Add(c.Generation, 1);
            }

            return numbersPerGeneration;
        }
    }

    internal class Cow
    {
        private Cow ParentCow { get; set; }
        public CowTypes CowType;

        public Cow(CowTypes cowType, Cow parentCow)
        {
            CowType = cowType;
            ParentCow = parentCow;
            if(parentCow == null)
                Calves.Add(new Cow(CowTypes.Calf, this)); // first year starts with 1 calf

            var tempCow = this;
            Generation = 1; // start at 1
            while (tempCow.ParentCow != null)
            {
                Generation++;
                tempCow = tempCow.ParentCow;
            }
        }

        public int Generation { get; set; }

        public int Age { get; set; }
        public List<Cow> Calves { get; } = new List<Cow>();

        public int NumberOfCalves => Calves.Count;

        public void AddYear()
        {
            foreach (var calf in Calves)
            {
                calf.AddYear();
            }

            if (Age++ == 3)
            {
                CowType = CowTypes.Cow;
            }

            if (CowType == CowTypes.Cow)
                Calves.Add(new Cow(CowTypes.Calf, this));
        }

        public static IEnumerable<Cow> GetCows(Cow cow)
        {
            if (cow == null)
            {
                yield break;
            }
            yield return cow;
            foreach (var calf in cow.Calves)
            {
                foreach (var innerCalf in GetCows(calf))
                {
                    yield return innerCalf;
                }
            }
        }
    }

    public enum CowTypes
    {
        Cow,
        Calf
    }
}