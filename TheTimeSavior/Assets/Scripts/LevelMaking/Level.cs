namespace Assets.Scripts.LevelMaking
{
    public class Level
    {
        public LevelTypes Type { get; set; }
        public int Difficulty { get; set; }
        public int RandomNumber { get; set; }

        public string GetCompleteName()
        {
            return string.Format("LevelsPrefab/{0}_{1}_{2}", Type, Difficulty, RandomNumber);
        }
    }
}
