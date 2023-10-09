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

        public abstract void Apply();

        public abstract string GenerateDescription();

    }
}
