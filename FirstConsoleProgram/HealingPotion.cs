using System;
using System.Collections.Generic;
using System.Text;

namespace CRPGThing
{
    class HealingPotion : Consumable
    {
        public int amountToHeal;

        public HealingPotion(int amountToHeal, string name, string namePlural, string description, int value) : base(name, namePlural, description, value)
        {
            this.amountToHeal = amountToHeal;
        }

        public override void Effect(Player player)
        {
            player.currentHP = (int)MathF.Min(player.currentHP + amountToHeal, player.maximumHP);
        }
    }
}
