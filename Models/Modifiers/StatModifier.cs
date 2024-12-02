using AroundYou.Models.Enums;
using System;

namespace AroundYou.Models.Modifiers
{
    public class StatModifier : Modifier
    {
        public float Value { get; set; }
        public string StatName { get; set; }
        public EStatCalculationType StatCalculationType { get; set; }

        public StatModifier(string descriptionTemplate, float value, string statName, EStatCalculationType statCalculationType)
            : base(descriptionTemplate)
        {
            Value = value;
            StatName = statName;
            StatCalculationType = statCalculationType;
        }

        public override void Apply(StatsComponent statComponent)
        {
            statComponent.Calculate(StatName, StatCalculationType, Value);
        }

        public override dynamic PreCalculate(StatsComponent statComponent)
        {
            return statComponent.PreCalculate(StatName, StatCalculationType, Value);
        }

        public override dynamic CalculateDifference(StatsComponent statComponent)
        {
            return statComponent.CalculateDifference(StatName, StatCalculationType, Value);
        }

        public override string GenerateDescription()
        {
            return DescriptionTemplate.Replace("#", Value.ToString());
        }
    }
}
