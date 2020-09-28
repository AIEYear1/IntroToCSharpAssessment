namespace CRPGNamespace
{
    class NestLocation : Location
    {
        /// <summary>
        /// MonsterThat will appear everytime you enter this area
        /// </summary>
        public Monster monsterToLiveHere;

        /// Parameters
        /// <param name="iD">ID reference for finding specific locations</param>
        /// <param name="name">Name of the location</param>
        /// <param name="description">Description of the location</param>
        /// <param name="knownNoun">Whether the noun is Specific or abstract</param>
        /// <param name="properNoun">Whether the noun is proper or normal</param>
        public NestLocation(int iD, string name, string description, bool knownNoun = false, bool properNoun = false) : base(iD, name, description, knownNoun, properNoun)
        {

        }

        public void SpawnMonster()
        {
            monsterLivingHere = monsterToLiveHere;
        }
    }
}
