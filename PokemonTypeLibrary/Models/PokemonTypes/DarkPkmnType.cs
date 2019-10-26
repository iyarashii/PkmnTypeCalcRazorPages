﻿namespace PokemonTypeLibrary.Models
{
    public class DarkPkmnType : PkmnType
    {
        public override string TypeName => "DARK";

        public override string TypeColor => "#705848";

        public override double CalculateDmgMultiplierForASingleType(IPkmnType pkmnType)
        {
            double output;
            if (pkmnType is GhostPkmnType || pkmnType is PsychicPkmnType)
            {
                output = 2.0;
            }
            else if (pkmnType is FightingPkmnType || pkmnType is DarkPkmnType || pkmnType is FairyPkmnType)
            {
                output = 0.5;
            }
            else
            {
                output = 1.0;
            }
            return output;
        }
    }
}
