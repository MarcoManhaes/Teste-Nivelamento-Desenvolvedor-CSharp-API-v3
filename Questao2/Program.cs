using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Questao2;

public class Program
{
    public static async Task Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = await GetTotalScoredGoalsAsync(teamName, year);

        Console.WriteLine("Team "+ teamName +" scored "+ totalGoals.ToString() + " goals in "+ year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = await GetTotalScoredGoalsAsync(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public static async Task<int> GetTotalScoredGoalsAsync(string team, int year)
    {
        int totalGoals = 0;
        int currentPage = 1;
        HttpClient client = new HttpClient();

        // Process team1 goals
        while (true)
        {
            string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team1={team}&page={currentPage}";
            var response = await client.GetStringAsync(url);
            var data = JsonConvert.DeserializeObject<ApiResponse>(response);

            foreach (var match in data.Data)
            {
                totalGoals += int.Parse(match.Team1Goals);
            }

            if (currentPage >= data.TotalPages)
                break;

            currentPage++;
        }

        currentPage = 1;

        // Process team2 goals
        while (true)
        {
            string url = $"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team2={team}&page={currentPage}";
            var response = await client.GetStringAsync(url);
            var data = JsonConvert.DeserializeObject<ApiResponse>(response);

            foreach (var match in data.Data)
            {
                totalGoals += int.Parse(match.Team2Goals);
            }

            if (currentPage >= data.TotalPages)
                break;

            currentPage++;
        }

        return totalGoals;
    }
}

