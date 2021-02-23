using RiotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LCTB
{
    class Balancer
    {
        private IRiotClient client;

        public delegate void StateArgs(string message);
        public event StateArgs ProgressUpdate;

        public Balancer()
        {
            client = new RiotClient(new RiotClientSettings
            {
                ApiKey = StaticValues.ApiKey
            });
        }

        public async Task Process(string inputFile, string outputFile, string server)
        {
            List<SummonerData> summoners = new List<SummonerData>();
            List<string> unresolved = new List<string>();
            var names = File.ReadAllLines(inputFile);

            for (int i = 0; i < names.Length; i++)
            {
                ProgressUpdate?.Invoke($"{names[i]} ({i + 1}/{names.Length})");
                var summoner = await GetSummoner(names[i], server);

                if (summoner == null)
                    unresolved.Add(names[i]);
                else
                    summoners.Add(summoner);
            }
            /*
            List<string> outStrings = new List<string>();
            foreach (var summoner in summoners)
            {
                var outString = $"{summoner.Name} = {summoner.Elo} ELO.";
                if (!string.IsNullOrEmpty(summoner.Info))
                    outString += $" ({summoner.Info})";
                outStrings.Add(outString);
            }
            */

            ProgressUpdate?.Invoke($"Balancing...");

            int totalAvg;
            int reserveCount = summoners.Count % 5;
            List<SummonerData> reserve = new List<SummonerData>();

            summoners = summoners.OrderBy(x => x.Elo).ToList();
            totalAvg = summoners.Sum(s => s.Elo) / summoners.Count;

            Random rnd = new Random();
            for (int i = 0; i < reserveCount; i++)
            {
                int index = rnd.Next(0, summoners.Count - 1);
                reserve.Add(summoners[index]);
                summoners.RemoveAt(index);
            }
            var teams = Balance(summoners);

            OutputToFile(teams, reserve, unresolved, totalAvg, outputFile);
        }
        async Task<SummonerData> GetSummoner(string name, string server)
        {
            SummonerData summonerData = new SummonerData();
            int elo;
            string info = "";

            DateTime start = DateTime.Now;

            var summoner = await client.GetSummonerBySummonerNameAsync(name, server);
            if (summoner == null)
                return null;
            var leagueEntries = await client.GetLeagueEntriesBySummonerIdAsync(summoner.Id, server);
            var leagueEntry = leagueEntries.Where(l => l.QueueType == "RANKED_SOLO_5x5").FirstOrDefault();
            if (leagueEntry == null)
            {
                var matchList = await client.GetMatchListByAccountIdAsync(summoner.AccountId, platformId: server);
                if (matchList == null)
                {
                    elo = StaticValues.DefaultElo;
                    info = "No match list. Default value.";
                }
                else
                {
                    var match = await client.GetMatchAsync(matchList.Matches[0].GameId, server);
                    if (match == null)
                    {
                        elo = StaticValues.DefaultElo;
                        info = "Can't get last match. Default value.";
                    }
                    else
                    {
                        info = "Based on last match.";
                        var participant = match.Participants.Where(p => p.ParticipantId == match.ParticipantIdentities.Where(i => i.Player.SummonerName == summoner.Name).FirstOrDefault().ParticipantId).FirstOrDefault();
                        var tier = participant.HighestAchievedSeasonTier;
                        if (tier == null || tier == "UNRANKED")
                        {
                            elo = StaticValues.DefaultElo;
                            info += " (Unranked)";
                        }
                        else
                        {
                            elo = RankToPoints($"{tier} 2");
                            info += $" ({tier})";
                        }
                    }
                }
            }
            else
            {
                elo = RankToPoints($"{leagueEntry.Tier} {RomanToArabic(leagueEntry.Rank)}") + leagueEntry.LeaguePoints;
                info = $"{leagueEntry.Tier} {leagueEntry.Rank}";
            }
            var timeout = 3000 - (DateTime.Now - start).TotalMilliseconds;
            if (timeout > 0)
                Thread.Sleep((int)timeout);

            summonerData.Name = summoner.Name;
            summonerData.Info = info;
            summonerData.Elo = elo;
            return summonerData;
        }
        List<Team> Balance(List<SummonerData> summonersList)
        {
            Stack<SummonerData> summoners = new Stack<SummonerData>(summonersList);

            int median = (summoners.Sum(s => s.Elo) / summoners.Count) / 2;
            List<Team> teamsList = new List<Team>();
            for (int i = 0; i < summoners.Count / 5; i++)
            {
                teamsList.Add(new Team());
            }

            while (summoners.Count > 0)
            {
                var summoner = summoners.Pop();
                int index = 0;
                teamsList = teamsList.OrderBy(t => t.Summoners.Sum(s => s.Elo)).ToList();
                while (teamsList[index].Summoners.Count == 5)//&& teamsList[index].Summoners.Sum(s => s.Elo) >= median
                {
                    index++;
                }
                teamsList[index].Summoners.Add(summoner);
            }
            foreach (var team in teamsList)
            {
                team.Avg = team.Summoners.Sum(s => s.Elo) / team.Summoners.Count;
            }

            return teamsList;
        }
        void OutputToFile(List<Team> teams, List<SummonerData> reserve, List<string> unresolved, int avg, string outputFile)
        {
            Random rnd = new Random();
            int strLength = 90;
            List<string> outputStrings = new List<string>();

            teams = teams.OrderBy(t => rnd.Next(0, 100)).ToList();

            outputStrings.Add($"Average ELO over all summoners: {avg}");
            if (unresolved.Count > 0)
            {
                outputStrings.Add("");
                outputStrings.Add($"Unresolved names: {unresolved.Count}");
                foreach (var name in unresolved)
                {
                    outputStrings.Add($"{name}");
                }
            }
            if (reserve.Count > 0)
            {
                outputStrings.Add("");
                outputStrings.Add($"Reserve summoners: {reserve.Count}");
                foreach (var summoner in reserve)
                {
                    var strStart = $"{summoner.Name} | {summoner.Elo} ELO";
                    var strEnd = summoner.Info;

                    strEnd = strEnd.PadLeft(strLength - strStart.Length, '.');

                    outputStrings.Add($"{strStart}{strEnd}");
                }
            }
            outputStrings.Add("");
            outputStrings.Add($"Balanced into {teams.Count} teams.");
            int maxAvg = teams.Max(t => t.Avg);
            int minAvg = teams.Min(t => t.Avg);

            outputStrings.Add($"Team avg. max difference: {maxAvg - minAvg}");

            for (int i = 0; i < teams.Count; i++)
            {
                string adj = StaticValues.Adjectives[rnd.Next(0, StaticValues.Adjectives.Length - 1)];
                string champ = StaticValues.Champions[rnd.Next(0, StaticValues.Champions.Length - 1)];
                if (champ.EndsWith("x") || champ.EndsWith("s") || champ.EndsWith("z"))
                    champ += "es";
                else
                    champ += "s";

                outputStrings.Add("");
                outputStrings.Add(PadBoth($"\"{adj} {champ}\" ({teams[i].Avg} Average)", strLength));
                foreach (var summoner in teams[i].Summoners)
                {
                    var strStart = $"{summoner.Name} | {summoner.Elo} ELO";
                    var strEnd = summoner.Info;

                    strEnd = strEnd.PadLeft(strLength - strStart.Length, '.');

                    outputStrings.Add($"{strStart}{strEnd}");
                }
            }
            File.WriteAllLines(outputFile, outputStrings);
        }
        public string PadBoth(string source, int length)
        {
            int spaces = length - source.Length;
            int padLeft = spaces / 2 + source.Length;
            return source.PadLeft(padLeft, '=').PadRight(length, '=');

        }
        public int RomanToArabic(string roman)
        {
            switch (roman)
            {
                case "I":
                    return 1;
                case "II":
                    return 2;
                case "III":
                    return 3;
                case "IV":
                    return 4;
            }
            return -1;
        }
        public int RankToPoints(string rank)
        {
            rank = rank.ToLower();
            int mult = 0;
            if (rank.Contains("iron"))
            {
                mult = (int)Char.GetNumericValue(rank[rank.Length - 1]);
                switch (mult)
                {
                    case 1:
                        return 300;
                    case 2:
                        return 200;
                    case 3:
                        return 100;
                    case 4:
                        return 0;
                }
            }
            if (rank.Contains("bronze"))
            {
                mult = (int)Char.GetNumericValue(rank[rank.Length - 1]);
                switch (mult)
                {
                    case 1:
                        return 700;
                    case 2:
                        return 600;
                    case 3:
                        return 500;
                    case 4:
                        return 400;
                }
            }
            else if (rank.Contains("silver"))
            {
                mult = (int)Char.GetNumericValue(rank[rank.Length - 1]);
                switch (mult)
                {
                    case 1:
                        return 1100;
                    case 2:
                        return 1000;
                    case 3:
                        return 900;
                    case 4:
                        return 800;
                }
            }
            else if (rank.Contains("gold"))
            {
                mult = (int)Char.GetNumericValue(rank[rank.Length - 1]);
                switch (mult)
                {
                    case 1:
                        return 1500;
                    case 2:
                        return 1400;
                    case 3:
                        return 1300;
                    case 4:
                        return 1200;
                }
            }
            else if (rank.Contains("platinum"))
            {
                mult = (int)Char.GetNumericValue(rank[rank.Length - 1]);
                switch (mult)
                {
                    case 1:
                        return 1900;
                    case 2:
                        return 1800;
                    case 3:
                        return 1700;
                    case 4:
                        return 1600;
                }
            }
            else if (rank.Contains("diamond"))
            {
                mult = (int)Char.GetNumericValue(rank[rank.Length - 1]);
                switch (mult)
                {
                    case 1:
                        return 2300;
                    case 2:
                        return 2200;
                    case 3:
                        return 2100;
                    case 4:
                        return 2000;
                }
            }
            else if (rank.Contains("master"))
            {
                if (rank.Contains("grand"))
                {
                    return 2500;
                }
                else
                    return 2400;
            }
            else if (rank.Contains("challenger"))
            {
                return 2600;
            }

            return 0;
        }
    }
}
