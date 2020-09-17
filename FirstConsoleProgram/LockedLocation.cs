using System;
using System.Collections.Generic;
using System.Text;

enum LockedLocationIndex
{
    FORESTEDGE,
    NORTH
}
namespace CRPGNamespace
{
    class LockedLocation : Location
    {
        bool canEnter = false;
        readonly LockedLocationIndex index;
        readonly string lockedText;

        public LockedLocation(int iD, string name, string description, bool canEnter, LockedLocationIndex locationIndex, string lockedText) : base(iD, name, description)
        {
            this.canEnter = canEnter;
            this.index = locationIndex;
            this.lockedText = lockedText;
        }

        public bool Enter()
        {
            switch (index)
            {
                case LockedLocationIndex.FORESTEDGE:
                    if(Program.player.Level < 2)
                        break;

                    canEnter = true;
                    Utils.Add("You unlock " + Utils.PrefixNoun(name, properNoun, knownNoun));
                    break;
                case LockedLocationIndex.NORTH:

                    break;
            }

            if (!canEnter)
            {
                Utils.Add(lockedText);
                return false;
            }

            return true;
        }
    }
}
