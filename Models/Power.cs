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

        public void ApplyModifiers()
        {
            foreach (Modifier modifier in Modifiers)
            {
                modifier.Apply();
            }
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
