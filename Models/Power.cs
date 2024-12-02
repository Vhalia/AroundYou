using System.Collections.Generic;
using AroundYou.Models.Modifiers;

namespace AroundYou.Models
{
    public class Power
    {
        public string Name { get; set; }
        public List<Modifier> Modifiers { get; set; }

        public static Power Init(string name)
        {
            return new Power()
            {
                Name = name,
                Modifiers = new List<Modifier>()
            };
        }

        public Power AddModifier(Modifier modifier)
        {
            Modifiers.Add(modifier);
            return this;
        }

        public void ApplyModifiers(StatsComponent statsComponent)
        {
            foreach (Modifier modifier in Modifiers)
            {
                modifier.Apply(statsComponent);
            }
        }

        public Dictionary<string, dynamic> PreCalculateModifiers(StatsComponent statsComponent)
        {
            Dictionary<string, dynamic> preCalculatedValues = new();
            foreach (Modifier modifier in Modifiers)
            {
                if (modifier is StatModifier)
                {
                    var statModifier = modifier as StatModifier;
                    if (preCalculatedValues.TryGetValue(statModifier.StatName, out var existingValue))
                    {
                        preCalculatedValues[statModifier.StatName] = existingValue + modifier.PreCalculate(statsComponent);
                    }
                    else
                    {
                        preCalculatedValues.Add(statModifier.StatName, modifier.PreCalculate(statsComponent));
                    }
                }
            }

            return preCalculatedValues;
        }

        public Dictionary<string, dynamic> CalculateDifferenceModifiers(StatsComponent statsComponent)
        {
            Dictionary<string, dynamic> differences = new();
            foreach (Modifier modifier in Modifiers)
            {
                if (modifier is StatModifier)
                {
                    var statModifier = modifier as StatModifier;
                    if (differences.TryGetValue(statModifier.StatName, out var existingValue))
                    {
                        differences[statModifier.StatName] = existingValue + modifier.CalculateDifference(statsComponent);
                    }
                    else
                    {
                        differences.Add(statModifier.StatName, modifier.CalculateDifference(statsComponent));
                    }
                }
            }

            return differences;
        }

        public string GenerateDescription()
        {
            var result = string.Empty;

            foreach(var modifier in Modifiers)
            {
                result += modifier.GenerateDescription() + ",\n";
            }

            return result;
        }
    }
}
