using Newtonsoft.Json;
using Questao2;

public class Program
{
    public static void Main()
    {
        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team "+ teamName +" scored "+ totalGoals.ToString() + " goals in "+ year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }

    public static int getTotalScoredGoals(string team, int year)
    {
        RetornoApi? retornoApiTeam1 = new ();
        RetornoApi? retornoApiTeam2 = new();
        HttpResponseMessage response;
        int gols = 0;
        int totalPage;
        string result;
        using (HttpClient httpClient = new())
        {
            response = httpClient.GetAsync($"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team1={team}").GetAwaiter().GetResult();
            result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            retornoApiTeam1 = JsonConvert.DeserializeObject<RetornoApi>(result.ToString());

            response = httpClient.GetAsync($"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team2={team}").GetAwaiter().GetResult();
            result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
            retornoApiTeam2 = JsonConvert.DeserializeObject<RetornoApi>(result.ToString());

            if (retornoApiTeam1 != null)
            {
                totalPage = retornoApiTeam1.Total_Pages;
                for (int i = retornoApiTeam1.Page; i <= totalPage; i++)
                {
                    response = httpClient.GetAsync($"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team1={team}&page={i}").GetAwaiter().GetResult();
                    result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    retornoApiTeam1 = JsonConvert.DeserializeObject<RetornoApi>(result.ToString());

                    if (retornoApiTeam1 != null)
                    {
                        foreach (var jogos in retornoApiTeam1.Data)
                        {
                            gols += Convert.ToInt32(jogos.Team1goals);
                        }
                    }
                }
            }

            if (retornoApiTeam2 != null)
            {
                totalPage = retornoApiTeam2.Total_Pages;
                for (int i = retornoApiTeam2.Page; i <= totalPage; i++)
                { 
                    response = httpClient.GetAsync($"https://jsonmock.hackerrank.com/api/football_matches?year={year}&team2={team}&page={i}").GetAwaiter().GetResult();
                    result = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    retornoApiTeam2 = JsonConvert.DeserializeObject<RetornoApi>(result.ToString());
                    

                    if (retornoApiTeam2 != null)
                    {
                        foreach (var jogos in retornoApiTeam2.Data)
                        {
                            gols += Convert.ToInt32(jogos.Team2goals);
                        }
                    }
                }
            }
        }

        return gols;
    }

}