namespace CRPGNamespace
{
    /// <summary>
    /// Objective marker for quests
    /// </summary>
    public struct Objective
    {
        /// <summary>
        /// Name of the Objective
        /// </summary>
        public string Name;
        /// <summary>
        /// Whether the objective is complete
        /// </summary>
        public bool Complete;
        /// <summary>
        /// Marker point in quest line
        /// </summary>
        public int Marker;
        /// <summary>
        /// Text displayed on objective completion
        /// </summary>
        public string CompletionText;

        /// Parameters
        /// <param name="name">Name of the Objective</param>
        /// <param name="marker">0 based marker point in the quest line</param>
        /// <param name="completionText">Text displayed on Objective completion</param>
        public Objective(string name, int marker, string completionText)
        {
            Name = name;
            Complete = false;
            Marker = marker;
            CompletionText = completionText;
        }
    }
}
