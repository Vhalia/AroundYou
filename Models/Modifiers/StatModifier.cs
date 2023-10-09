using System;

namespace AroundYou.Models.Modifiers
{
    public class StatModifier : Modifier
    {
        public dynamic Value { get; set; }
        public Action<dynamic> AddToStat { get; set; }

        public StatModifier(string descriptionTemplate, dynamic value, Action<dynamic> setStat)
            : base(descriptionTemplate)
        {
            Value = value;
            AddToStat = setStat;
        }

        public override void Apply()
        {
            AddToStat(Value);
        }

        public override string GenerateDescription()
        {
            return DescriptionTemplate.Replace("#", Value.ToString());
        }
    }
}
