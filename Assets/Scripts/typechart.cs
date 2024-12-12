using System;
using System.Collections;
using System.Collections.Generic;

public class typechart
{
    // create type list
    public enum TYPE {
        NORMAL = 1,
        FIRE = 2,
        WATER = 3,
        ELECTRIC = 4,
        GRASS = 5,
        ICE = 6,
        FIGHTING = 7,
        POISON = 8,
        GROUND = 9,
        FLYING = 10,
        PSYCHIC = 11,
        BUG = 12,
        ROCK = 13,
        GHOST = 14,
        DRAGON = 15,
        DARK = 16,
        STEEL = 17,
        FAIRY = 18,
        NA = 19
    }

    private TYPE type1 = TYPE.NA;
    private TYPE type2 = TYPE.NA;
    public TYPE getType1() { return type1; }
    public TYPE getType2() { return type2; }

    // constructors for new pokemon types
    public typechart(string primary) { type1 = GetTypeFromString(primary); }
    public typechart(string primary, string secondary) { 
        type1 = GetTypeFromString(primary);
        type2 = GetTypeFromString(secondary);
        if ((type2 != TYPE.NA) && (type1 == TYPE.NA)) { // probably won't happen but just in case
            type1 = type2;
            type2 = TYPE.NA;
        }
    }
    private TYPE GetTypeFromString(string typeString) { // attempt to assign type, return NA if impossible
        if (Enum.TryParse(typeString.ToUpper(), out TYPE result)) return result;
        else return TYPE.NA;
    }

    // find damage of attack after finding effectiveness
    public int FindEffectiveness(int damage, TYPE attackingType, TYPE defendingType1, TYPE defendingType2) {
        return (int)(damage * chart(attackingType, defendingType1) * chart(attackingType, defendingType2));
    }

    // calculate effectiveness with each type matchup
    private float chart(TYPE attack, TYPE defense) {
        // handle special cases
        if (attack == TYPE.NA) return 0; // immune if attacking type error
        if (defense == TYPE.NA) return 1; // could be empty secondary type
        
        // check each type
        switch (attack) {
            case TYPE.NORMAL:
                if (defense == TYPE.GHOST) return 0; // immunities
                else if (defense == TYPE.STEEL || defense == TYPE.ROCK) return 0.5f; // resistances
                else return 1;
            case TYPE.FIRE:
                if (defense == TYPE.FIRE || defense == TYPE.WATER || defense == TYPE.DRAGON || defense == TYPE.ROCK) return 0.5f; // resistances
                else if (defense == TYPE.GRASS || defense == TYPE.ICE || defense == TYPE.BUG || defense == TYPE.STEEL) return 2; // weaknesses
                else return 1;
            case TYPE.WATER:
                if (defense == TYPE.GRASS || defense == TYPE.WATER || defense == TYPE.DRAGON) return 0.5f; // resistances
                else if (defense == TYPE.FIRE || defense == TYPE.GROUND || defense == TYPE.ROCK) return 2; // weaknesses
                else return 1;
            case TYPE.ELECTRIC:
                if (defense == TYPE.GROUND) return 0; // immunities
                else if (defense == TYPE.GRASS || defense == TYPE.ELECTRIC || defense == TYPE.DRAGON) return 0.5f; // resistances
                else if (defense == TYPE.WATER || defense == TYPE.FLYING) return 2; // weaknesses
                else return 1;
            case TYPE.GRASS:
                if (defense == TYPE.FIRE || defense == TYPE.GRASS || defense == TYPE.POISON || defense == TYPE.FLYING || defense == TYPE.BUG || defense == TYPE.DRAGON || defense == TYPE.STEEL) return 0.5f; // resistances
                else if (defense == TYPE.WATER || defense == TYPE.GROUND || defense == TYPE.ROCK) return 2; // weaknesses
                else return 1;
            case TYPE.ICE:
                if (defense == TYPE.FIRE || defense == TYPE.WATER || defense == TYPE.ICE || defense == TYPE.STEEL) return 0.5f; // resistances
                else if (defense == TYPE.GRASS || defense == TYPE.GROUND || defense == TYPE.FLYING || defense == TYPE.DRAGON) return 2; // weaknesses
                else return 1;
            case TYPE.FIGHTING:
                if (defense == TYPE.GHOST) return 0; // immunities
                else if (defense == TYPE.POISON || defense == TYPE.FLYING || defense == TYPE.PSYCHIC || defense == TYPE.BUG) return 0.5f; // resistances
                else if (defense == TYPE.NORMAL || defense == TYPE.ROCK || defense == TYPE.ICE || defense == TYPE.DARK || defense == TYPE.STEEL) return 2; // weaknesses
                else return 1;
            case TYPE.POISON:
                if (defense == TYPE.STEEL) return 0; // immunities
                else if (defense == TYPE.POISON || defense == TYPE.GROUND || defense == TYPE.ROCK || defense == TYPE.GHOST) return 0.5f; // resistances
                else if (defense == TYPE.GRASS || defense == TYPE.FAIRY) return 2; // weaknesses
                else return 1;
            case TYPE.GROUND:
                if (defense == TYPE.FLYING) return 0; // immunities
                else if (defense == TYPE.GRASS || defense == TYPE.BUG) return 0.5f; // resistances
                else if (defense == TYPE.FIRE || defense == TYPE.ELECTRIC || defense == TYPE.POISON || defense == TYPE.ROCK || defense == TYPE.STEEL) return 2; // weaknesses
                else return 1;
            case TYPE.FLYING:
                if (defense == TYPE.ELECTRIC || defense == TYPE.ROCK || defense == TYPE.STEEL) return 0.5f; // resistances
                else if (defense == TYPE.GRASS || defense == TYPE.FIGHTING || defense == TYPE.BUG) return 2; // weaknesses
                else return 1;
            case TYPE.PSYCHIC:
                if (defense == TYPE.DARK) return 0; // immunities
                else if (defense == TYPE.PSYCHIC || defense == TYPE.STEEL) return 0.5f; // resistances
                else if (defense == TYPE.FIGHTING || defense == TYPE.POISON) return 2; // weaknesses
                else return 1;
            case TYPE.BUG:
                if (defense == TYPE.FIRE || defense == TYPE.FIGHTING || defense == TYPE.POISON || defense == TYPE.FLYING || defense == TYPE.GHOST || defense == TYPE.FAIRY || defense == TYPE.STEEL) return 0.5f; // resistances
                else if (defense == TYPE.GRASS || defense == TYPE.PSYCHIC || defense == TYPE.DARK) return 2; // weaknesses
                else return 1;
            case TYPE.ROCK:
                if (defense == TYPE.FIGHTING || defense == TYPE.GROUND || defense == TYPE.STEEL) return 0.5f; // resistances
                else if (defense == TYPE.FIRE || defense == TYPE.ICE || defense == TYPE.BUG || defense == TYPE.FLYING) return 2; // weaknesses
                else return 1;
            case TYPE.GHOST:
                if (defense == TYPE.NORMAL) return 0; // immunities
                else if (defense == TYPE.PSYCHIC || defense == TYPE.GHOST) return 0.5f; // resistances
                else if (defense == TYPE.DARK) return 2; // weaknesses
                else return 1;
            case TYPE.DRAGON:
                if (defense == TYPE.FAIRY) return 0; // immunities
                else if (defense == TYPE.DRAGON) return 0.5f; // resistances
                else if (defense == TYPE.STEEL) return 2; // weaknesses
                else return 1;
            case TYPE.DARK:
                if (defense == TYPE.FIGHTING || defense == TYPE.DARK || defense == TYPE.FAIRY) return 0.5f; // resistances
                else if (defense == TYPE.GHOST || defense == TYPE.PSYCHIC) return 2; // weaknesses
                else return 1;
            case TYPE.STEEL:
                if (defense == TYPE.FIRE || defense == TYPE.WATER || defense == TYPE.ELECTRIC || defense == TYPE.STEEL) return 0.5f; // resistances
                else if (defense == TYPE.ICE || defense == TYPE.ROCK || defense == TYPE.FAIRY) return 2; // weaknesses
                else return 1;
            case TYPE.FAIRY:
                if (defense == TYPE.FIRE || defense == TYPE.POISON || defense == TYPE.STEEL) return 0.5f; // resistances
                else if (defense == TYPE.FIGHTING || defense == TYPE.DRAGON || defense == TYPE.DARK) return 2; // weaknesses
                else return 1;
            default:
                return 0;
        }
    }
}
