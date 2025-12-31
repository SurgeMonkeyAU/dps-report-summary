#load "PlayerMetaData.csx"

public class ReportSummary
{
    public Dictionary<string,HashSet<PlayerMetaData>> PlayersAttendance = [];

    public Dictionary<string, int> SummarizePlayerCounts()
    {
        var playerCounts = new Dictionary<string, int>();

        foreach (var (date, players) in PlayersAttendance)
        {
            var uniquePlayers = new HashSet<string>();

            foreach (var player in players)
            {
                uniquePlayers.Add(player.Display_Name);
            }

            foreach (var uniquePlayer in uniquePlayers)
            {
                if (playerCounts.ContainsKey(uniquePlayer))
                {
                    playerCounts[uniquePlayer]++;
                }
                else
                {
                    playerCounts[uniquePlayer] = 1;
                }
            }
        }

        return playerCounts;
    }

    public Tuple<DateTime, DateTime> GetFirstAndLastDate()
    {
        if (PlayersAttendance == null || PlayersAttendance.Count == 0)
        {
            throw new InvalidOperationException("PlayersAttendance dictionary is empty or null.");
        }

        // Parse the keys to DateTime objects
        var dates = PlayersAttendance.Keys.Select(date => DateTime.Parse(date)).ToList();

        // Find the first and last dates
        DateTime firstDate = dates.Min();
        DateTime lastDate = dates.Max();

        return Tuple.Create(firstDate, lastDate);
    }
}