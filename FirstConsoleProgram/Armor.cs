using CsvHelper.Configuration;

namespace CRPGNamespace
{
    public class Armor : Item
    {
        public int ac = 0;

        public Armor(int iD, string name, string namePlural, string description, int value, int ac) : base(iD, name, namePlural, description, value)
        {
            this.ac = ac;
        }

        public override void Look()
        {
            Utils.Add(Name);
            Utils.Add($"\tProtection Level: {ac}");
            Utils.Add(Description);
        }
    }
}
