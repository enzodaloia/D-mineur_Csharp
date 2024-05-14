using System.Timers;
using System;
using System.IO;
using System.Collections.Generic;
using System.Timers;

namespace D_mineur_Csharp
{
    public class Game
    {
        private int nbCoup = 0;
        private string[,] plateau;
        private int[,] plateau2;
        private System.Timers.Timer timer;
        private bool jeuEnCours;
        private int tempsEcoule;
        private int difficulte;
        private string playerName;

        public bool JeuEnCours
        {
            get { return jeuEnCours; }
        }

        public Game()
        {
            plateau = null;
            plateau2 = null;

            timer = new System.Timers.Timer();
            timer.Interval = 1000;
            timer.Elapsed += TimerElapsed;
            jeuEnCours = false;
            tempsEcoule = 0;
        }

        public void ResetGame()
        {
            timer.Stop();
            nbCoup = 0;
            tempsEcoule = 0;
        }

        public int Menu()
        {
            Console.WriteLine("**************************");
            Console.WriteLine("* 1) Voir les règles     *");
            Console.WriteLine("* 2) Facile              *");
            Console.WriteLine("* 3) Moyen               *");
            Console.WriteLine("* 4) Difficile           *");
            Console.WriteLine("* 5) Charger une partie  *");
            Console.WriteLine("* 6) Leaderboard         *");
            Console.WriteLine("* 7) Quitter             *");
            Console.WriteLine("**************************");

            Console.WriteLine("Choisissez un mode de jeu ?");
            int mode = Convert.ToInt32(Console.ReadLine());

            switch (mode)
            {
                case 1:
                    AfficherRegles();
                    break;
                case 2:
                    difficulte = 9;
                    plateau = new string[difficulte, difficulte];
                    plateau2 = new int[difficulte, difficulte];
                    playerName = ChoisirNom();
                    Display();
                    ChoisirCase();
                    break;
                case 3:
                    difficulte = 12;
                    plateau = new string[difficulte, difficulte];
                    plateau2 = new int[difficulte, difficulte];
                    playerName = ChoisirNom();
                    Display();
                    ChoisirCase();
                    break;
                case 4:
                    difficulte = 15;
                    plateau = new string[difficulte, difficulte];
                    plateau2 = new int[difficulte, difficulte];
                    playerName = ChoisirNom();
                    Display();
                    ChoisirCase();
                    break;
                case 5:
                    ChargerPartie();
                    break;
                case 6:
                    AfficherLeaderboard();
                    break;
                case 7:
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Option invalide. Veuillez choisir une option valide.");
                    Menu();
                    break;
            }
            return mode;
        }

        private string ChoisirNom()
        {
            Console.WriteLine("Entrez votre nom :");
            return Console.ReadLine();
        }

        public void PlacerBombes()
        {
            Random rnd = new Random();
            int bombesPlacées = 0;

            while (bombesPlacées < difficulte)
            {
                int ligne = rnd.Next(0, plateau2.GetLength(0));
                int colonne = rnd.Next(0, plateau2.GetLength(1));

                if (plateau2[ligne, colonne] != -1)
                {
                    plateau2[ligne, colonne] = -1;
                    bombesPlacées++;
                }
            }
        }

        public void Display()
        {
            jeuEnCours = true;
            timer.Start();
            PlacerBombes();

            Console.Write("   ");
            for (int i = 0; i < plateau.GetLength(1); i++)
            {
                Console.Write($" {i} ");
            }
            Console.WriteLine();

            for (int i = 0; i < plateau.GetLength(0); i++)
            {
                if (i < 10)
                {
                    Console.Write($" 0{i} ");
                }
                else
                {
                    Console.Write($" {i} ");
                }
                for (int j = 0; j < plateau.GetLength(1); j++)
                {
                    Console.Write(" * ");
                }
                Console.WriteLine();
            }
        }

        public void DisplayGame()
        {
            jeuEnCours = true;
            Console.Clear();
            Console.Write("   ");
            for (int i = 0; i < plateau.GetLength(1); i++)
            {
                Console.Write($" {i} ");
            }
            Console.WriteLine();

            for (int i = 0; i < plateau.GetLength(0); i++)
            {
                Console.Write($" {i} ");
                for (int j = 0; j < plateau.GetLength(1); j++)
                {
                    if (string.IsNullOrEmpty(plateau[i, j]))
                    {
                        Console.Write(" * ");
                    }
                    else
                    {
                        Console.Write($" {plateau[i, j]} ");
                    }
                }
                Console.WriteLine();
            }
        }

        public void ChoisirCase()
        {
            while (true)
            {
                Console.WriteLine("Entrez la ligne de la case ou 's' pour sauvegarder la partie ou 'q' pour quitter :");
                string input = Console.ReadLine();
                if (input.ToLower() == "s")
                {
                    SauvegarderPartie();
                    continue;
                }
                if (input.ToLower() == "q")
                {
                    Quit();
                }

                if (int.TryParse(input, out int ligne) && ligne >= 0 && ligne < difficulte)
                {
                    Console.WriteLine("Entrez la colonne de la case :");
                    if (int.TryParse(Console.ReadLine(), out int colonne) && colonne >= 0 && colonne < difficulte)
                    {
                        nbCoup++;
                        if (plateau2[ligne, colonne] == -1)
                        {
                            plateau[ligne, colonne] = "!";
                            DisplayGame();
                            Console.WriteLine("Boom ! Vous avez touché une bombe !");
                            Quit();
                        }
                        else
                        {
                            plateau[ligne, colonne] = "X";
                            DisplayGame();
                            Console.WriteLine($"La case ({ligne}, {colonne}) est sûre.");
                            Console.WriteLine($"Nombre de coups : {nbCoup}");

                            if (CheckWinCondition())
                            {
                                Console.WriteLine("Félicitations ! Vous avez gagné !");
                                RecordScore();
                                Quit();
                            }
                            else
                            {
                                ChoisirCase();
                            }
                        }
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Coordonnée de colonne invalide. Veuillez choisir une colonne valide.");
                    }
                }
                else
                {
                    Console.WriteLine("Coordonnée de ligne invalide. Veuillez choisir une ligne valide.");
                }
            }
        }


        private bool CheckWinCondition()
        {
            for (int i = 0; i < plateau.GetLength(0); i++)
            {
                for (int j = 0; j < plateau.GetLength(1); j++)
                {
                    if (string.IsNullOrEmpty(plateau[i, j]) && plateau2[i, j] != -1)
                    {
                        return false;
                    }
                }
            }
            return true;
        }


        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            tempsEcoule++;
        }

        public void Quit()
        {
            int minutes = tempsEcoule / 60;
            int secondes = tempsEcoule % 60;
            Console.WriteLine($"{minutes}:{secondes}");
            Console.WriteLine("Au revoir !");
            ResetGame();
            Menu();
        }

        public void AfficherRegles()
        {
            string filePath = "/Users/enzo/Documents/Github/D-mineur_Csharp/regles_demineur.txt";
            try
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    Console.WriteLine(line);
                }
                Menu();
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Le fichier des règles est introuvable.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite : {ex.Message}");
            }
        }

        public void RecordScore()
        {
            string filePath = "/Users/enzo/Documents/Github/D-mineur_Csharp/leaderboard_demineur.txt";
            try
            {
                List<string> scores = new List<string>();
                if (File.Exists(filePath))
                {
                    scores.AddRange(File.ReadAllLines(filePath));
                }

                string newScore = $"{playerName} - {nbCoup} coups en {tempsEcoule / 60}:{tempsEcoule % 60} - Difficulté {difficulte} bombes";
                scores.Add(newScore);

                scores.Sort((a, b) =>
                {
                    int coupsA = ExtractCoups(a);
                    int coupsB = ExtractCoups(b);
                    int cmp = coupsA.CompareTo(coupsB);
                    if (cmp == 0)
                    {
                        int tempsA = ExtractTemps(a);
                        int tempsB = ExtractTemps(b);
                        cmp = tempsA.CompareTo(tempsB);
                    }
                    return cmp;
                });

                if (scores.Count > 3)
                {
                    scores = scores.GetRange(0, 3);
                }

                File.WriteAllLines(filePath, scores);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de l'enregistrement du score : {ex.Message}");
            }
        }

        private int ExtractCoups(string score)
        {
            var parts = score.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 3) return int.MaxValue;

            var coupsPart = parts[1].Trim().Split(' ')[0];
            if (int.TryParse(coupsPart, out int coups))
            {
                return coups;
            }
            return int.MaxValue;
        }

        private int ExtractTemps(string score)
        {
            var parts = score.Split(new[] { '-' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 3) return int.MaxValue;

            var tempsPart = parts[2].Trim().Split(' ')[0];
            if (int.TryParse(tempsPart, out int temps))
            {
                return temps;
            }
            return int.MaxValue;
        }


        public void AfficherLeaderboard()
        {
            string filePath = "/Users/enzo/Documents/Github/D-mineur_Csharp/leaderboard_demineur.txt";
            try
            {
                if (File.Exists(filePath))
                {
                    string[] scores = File.ReadAllLines(filePath);
                    foreach (string score in scores)
                    {
                        Console.WriteLine(score);
                    }
                }
                else
                {
                    Console.WriteLine("Le fichier du leaderboard est introuvable.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite : {ex.Message}");
            }
            Menu();
        }

        public void SauvegarderPartie()
        {
            string filePath = "/Users/enzo/Documents/Github/D-mineur_Csharp/partie_en_cours.txt";
            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    writer.WriteLine($"Difficulte: {difficulte}");
                    writer.WriteLine($"NbCoup: {nbCoup}");
                    writer.WriteLine($"TempsEcoule: {tempsEcoule}");
                    writer.WriteLine($"JeuEnCours: {jeuEnCours}");

                    // Sauvegarde du plateau
                    writer.WriteLine("Plateau:");
                    for (int i = 0; i < plateau.GetLength(0); i++)
                    {
                        for (int j = 0; j < plateau.GetLength(1); j++)
                        {
                            if (!string.IsNullOrEmpty(plateau[i, j]))
                            {
                                writer.Write($"{plateau[i, j]} ");
                            }
                            else
                            {
                                writer.Write("* ");
                            }
                        }
                        writer.WriteLine();
                    }

                    // Sauvegarde du plateau2
                    writer.WriteLine("Plateau2:");
                    for (int i = 0; i < plateau2.GetLength(0); i++)
                    {
                        for (int j = 0; j < plateau2.GetLength(1); j++)
                        {
                            writer.Write($"{plateau2[i, j]} ");
                        }
                        writer.WriteLine();
                    }
                }

                Console.WriteLine("La partie en cours a été sauvegardée avec succès.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors de la sauvegarde de la partie en cours : {ex.Message}");
            }
        }


        public void ChargerPartie()
        {
            string filePath = "/Users/enzo/Documents/Github/D-mineur_Csharp/partie_en_cours.txt";
            try
            {
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("Aucune partie en cours n'a été trouvée.");
                    return;
                }

                using (StreamReader reader = new StreamReader(filePath))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("Difficulte:"))
                        {
                            difficulte = int.Parse(line.Split(':')[1].Trim());
                            plateau = new string[difficulte, difficulte];
                            plateau2 = new int[difficulte, difficulte];
                        }
                        else if (line.StartsWith("NbCoup:"))
                        {
                            nbCoup = int.Parse(line.Split(':')[1].Trim());
                        }
                        else if (line.StartsWith("TempsEcoule:"))
                        {
                            tempsEcoule = int.Parse(line.Split(':')[1].Trim());
                        }
                        else if (line.StartsWith("JeuEnCours:"))
                        {
                            jeuEnCours = bool.Parse(line.Split(':')[1].Trim());
                        }
                        else if (line.Equals("Plateau:"))
                        {
                            for (int i = 0; i < difficulte; i++)
                            {
                                string[] values = reader.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                                if (values.Length != difficulte)
                                {
                                    Console.WriteLine("Erreur : le nombre de valeurs lues ne correspond pas à la taille du tableau1.");
                                    return;
                                }
                                for (int j = 0; j < difficulte; j++)
                                {
                                    plateau[i, j] = values[j];
                                }
                            }
                        }
                        else if (line.Equals("Plateau2:"))
                        {
                            for (int i = 0; i < difficulte; i++)
                            {
                                string[] values = reader.ReadLine().Split(' ', StringSplitOptions.RemoveEmptyEntries);
                                if (values.Length != difficulte)
                                {
                                    Console.WriteLine("Erreur : le nombre de valeurs lues ne correspond pas à la taille du tableau2.");
                                    return;
                                }
                                for (int j = 0; j < difficulte; j++)
                                {
                                    plateau2[i, j] = int.Parse(values[j]);
                                }
                            }
                        }
                    }
                }

                Console.WriteLine("Partie chargée avec succès !");
                DisplayGame();
                ChoisirCase();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur s'est produite lors du chargement de la partie en cours : {ex.Message}");
            }
        }

    }
}
