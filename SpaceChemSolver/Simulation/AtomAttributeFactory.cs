using System;
using System.Collections.Generic;

namespace SpaceChemSolver.Simulation
{
    /// <summary>
    /// Factory for creating atom attributes. 
    /// 
    /// Example:
    /// AtomAttributeFactory.Generate("Hydrogen")
    /// </summary>
    public class AtomAttributeFactory
    {
        private static AtomAtrributes[] atomAttributes =
        {
            new AtomAtrributes("H", "Hydrogen", 1),
            new AtomAtrributes("O", "Oxygen", 2),
            new AtomAtrributes("N", "Nitrogen", 4)
        };

        private static Dictionary<string, AtomAtrributes> attributes;

        static AtomAttributeFactory() 
        {
            attributes = new Dictionary<string, AtomAtrributes>();

            foreach(var attribute in atomAttributes)
            {
                attributes.Add(attribute.name, attribute);
                attributes.Add(attribute.fullName, attribute);
            }
        }

        public static AtomAtrributes Generate(string name)
        {
            if(!attributes.ContainsKey(name))
            {
                throw new ArgumentException(
                    string.Format("No atom with name '{0}'", name));
            }

            return attributes[name];
        }
    }
}
