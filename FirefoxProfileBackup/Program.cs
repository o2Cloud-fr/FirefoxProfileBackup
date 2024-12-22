using System;
using System.IO;
using System.IO.Compression;

namespace FirefoxProfileBackup
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Détecter le chemin du profil Firefox
                string userProfile = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                string firefoxPath = Path.Combine(userProfile, "Mozilla", "Firefox", "Profiles");

                if (!Directory.Exists(firefoxPath))
                {
                    Console.WriteLine("Le chemin du profil Firefox n'a pas été trouvé.");
                    return;
                }

                Console.WriteLine("Chemin du profil Firefox détecté : " + firefoxPath);

                // Lister les dossiers de profils
                string[] profiles = Directory.GetDirectories(firefoxPath);
                if (profiles.Length == 0)
                {
                    Console.WriteLine("Aucun profil Firefox trouvé.");
                    return;
                }

                Console.WriteLine("\nProfils Firefox disponibles :");
                for (int i = 0; i < profiles.Length; i++)
                {
                    Console.WriteLine($"{i + 1}. {profiles[i]}");
                }

                // Sauvegarder tous les profils ou un profil spécifique ?
                Console.WriteLine("\nVoulez-vous sauvegarder tous les profils (taper 'all') ou un seul profil (taper son numéro) ?");
                string choix = Console.ReadLine();

                // Définir le chemin à sauvegarder
                string cheminASauvegarder;
                if (choix.ToLower() == "all")
                {
                    cheminASauvegarder = firefoxPath;
                }
                else if (int.TryParse(choix, out int index) && index > 0 && index <= profiles.Length)
                {
                    cheminASauvegarder = profiles[index - 1];
                }
                else
                {
                    Console.WriteLine("Choix invalide.");
                    return;
                }

                // Demander le nom du fichier ZIP
                Console.WriteLine("\nEntrez le nom du fichier ZIP (sans l'extension) :");
                string zipName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(zipName))
                {
                    Console.WriteLine("Nom du fichier invalide.");
                    return;
                }

                // Définir le chemin de destination sur le Bureau
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string zipFilePath = Path.Combine(desktopPath, zipName + ".zip");

                // Créer le fichier ZIP
                Console.WriteLine("\nCréation de l'archive ZIP en cours...");
                ZipFile.CreateFromDirectory(cheminASauvegarder, zipFilePath, CompressionLevel.Optimal, true);

                Console.WriteLine($"\nLe fichier ZIP a été créé avec succès : {zipFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Une erreur est survenue : {ex.Message}");
            }

            Console.WriteLine("\nAppuyez sur une touche pour quitter...");
            Console.ReadKey();
        }
    }
}
