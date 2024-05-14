using System;
using D_mineur_Csharp;

class Program
{
    static void Main(string[] args)
    {
        Game game = new Game();

        game.Menu();
    }
}

/*
using System;
using System.IO;

class Program
{
    static void Main()
    {
        CreateLeaderboardFile();
    }

    static void CreateLeaderboardFile()
    {
        string leaderboardFilePath = "/Users/enzo/Documents/Github/D-mineur_Csharp/leaderboard_demineur.txt";

        try
        {
            if (!File.Exists(leaderboardFilePath))
            {
                using (StreamWriter writer = new StreamWriter(leaderboardFilePath))
                {
                    writer.WriteLine("Leaderboard du Démineur :");
                    writer.WriteLine("1. ");
                    writer.WriteLine("2. ");
                    writer.WriteLine("3. ");
                }
                Console.WriteLine($"Le fichier leaderboard a été créé : {leaderboardFilePath}");
            }
            else
            {
                Console.WriteLine("Le fichier leaderboard existe déjà.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Une erreur s'est produite : {ex.Message}");
        }
    }
}
*/