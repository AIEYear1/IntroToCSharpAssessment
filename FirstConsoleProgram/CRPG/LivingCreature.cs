namespace CRPGNamespace
{
    /// <summary>
    /// Base class for Player and Monster
    /// </summary>
    public abstract class LivingCreature
    {
        /// <summary>
        /// Name of the creature
        /// </summary>
        public Name name;
        /// <summary>
        /// Creature's current HP
        /// </summary>
        public int currentHP;
        /// <summary>
        /// Creatures Maximum HP
        /// </summary>
        public int maximumHP;
        /// <summary>
        /// Whether noun is specific or abstract
        /// </summary>
        public bool knownNoun;
        /// <summary>
        /// Whether noun is Proper or normal
        /// </summary>
        public bool properNoun;

        /// Parameters
        /// <param name="name">Name of the creature</param>
        /// <param name="HP">Health of the creature</param>
        /// <param name="knownNoun">Whether the noun is specific or abstract</param>
        /// <param name="properNoun">Whether the noun is Proper or normal</param>
        public LivingCreature(Name name, int HP, bool knownNoun, bool properNoun)
        {
            this.name = name;
            this.currentHP = HP;
            this.maximumHP = HP;
            this.knownNoun = knownNoun;
            this.properNoun = properNoun;
        }

        /// Parameters
        /// <param name="HP">Health of the creature</param>
        /// <param name="knownNoun">Whether the noun is specific or abstract</param>
        /// <param name="properNoun">Whether the noun is Proper or normal</param>
        public LivingCreature(int HP, bool knownNoun, bool properNoun)
        {
            this.currentHP = HP;
            this.maximumHP = HP;
            this.knownNoun = knownNoun;
            this.properNoun = properNoun;
        }

        public LivingCreature()
        {

        }

        /// <summary>
        /// Creature takes damage
        /// </summary>
        /// <param name="damage">amount of damage taken</param>
        public abstract void TakeDamage(int damage);
    }
}
