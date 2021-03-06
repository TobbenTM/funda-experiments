namespace FE.Domain.Models
{
    /// <summary>
    /// This is the summary of the top ten real estate agents
    /// </summary>
    public class TopTen
    {
        public long TotalAdsToCalculate { get; set; }

        public long TotalAdsCalculated { get; set; }

        public bool DoneCalculating => TotalAdsCalculated == TotalAdsToCalculate;

        public Ranking[] Leaderboard { get; set; }
    }
}
