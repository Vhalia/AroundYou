using System.Collections.Generic;

namespace AroundYou.Models
{
    public class Power
    {
        public string Name { get; set; }
        public string DescriptionTemplate { get; set; }
        public List<float> Values { get; set; }
        public StatModifier StatModifier { get; set; }

        public string GenerateDescription()
        {
        }
    }
}
