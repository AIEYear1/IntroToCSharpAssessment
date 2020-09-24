namespace CRPGNamespace
{
    /// <summary>
    /// Abstract Consumable class that branches into anything the player can use one time
    /// </summary>
    public abstract class Consumable : Item
    {
        public Consumable(int iD, string name, string namePlural, string description, int value) : base(iD, name, namePlural, description, value)
        {

        }

        public abstract void Consume(Player player);
    }
}
