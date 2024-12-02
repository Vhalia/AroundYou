using System;

namespace AroundYou.Models.Modifiers
{
    public abstract class Modifier
    {
        public string DescriptionTemplate { get; set; }

        protected Modifier(string descriptionTemplate)
        {
            DescriptionTemplate = descriptionTemplate;
        }

        public abstract void Apply(StatsComponent statComponent);

        public abstract dynamic PreCalculate(StatsComponent statComponent);

        public abstract dynamic CalculateDifference(StatsComponent statComponent);

        public abstract string GenerateDescription();

    }
}
