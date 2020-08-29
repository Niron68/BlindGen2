using System;
using System.Collections.Generic;
using System.Linq;
using BlindGen2;

namespace ConsoleBlindGen
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Controller controller = new Controller();
                controller.Generator.Downloader.Update();
                Console.WriteLine("Que voulez-vous faire : (add/create/quit)");
                string action;
                bool redo;
                do
                {
                    action = Console.ReadLine().ToLower();
                    switch(action)
                    {
                        case "add":
                            redo = false;
                            break;
                        case "create":
                            redo = false;
                            break;
                        case "quit":
                            redo = false;
                            break;
                        default:
                            redo = true;
                            break;
                    }
                } while (redo);
                if (action.Equals("quit"))
                    return;
                if (action.Equals("create"))
                {
                    Console.Clear();
                    Console.WriteLine("Combien d'extrait ?");
                    bool notNumber;
                    int number;
                    do
                    {
                        string text = Console.ReadLine();
                        notNumber = !Int32.TryParse(text, out number);
                    } while (notNumber);
                    Console.WriteLine("Voulez vous spécifier une ou plusieurs catégorie spécifique : (y/n)");
                    if(Console.ReadLine().ToLower().First() == 'y')
                    {
                        List<Categorie> categories = new List<Categorie>();
                        string catChoice;
                        do
                        {
                            Console.WriteLine("Veuillez choisir une categorie ou taper quit pour terminer le choix :");
                            catChoice = Console.ReadLine().ToUpper();
                            if (Enum.TryParse<Categorie>(catChoice, out Categorie categorie) && !categories.Contains(categorie))
                            {
                                categories.Add(categorie);
                                Console.WriteLine("Catégorie ajouté avec succès !");
                            }
                            else
                            {
                                Console.WriteLine("Nom de catégorie invalide !");
                            }
                        } while (!catChoice.Equals("QUIT"));
                        controller.Generator.GenerateBlindtest(number, categories);
                    }
                    else
                    {
                        controller.Generator.GenerateBlindtest(number);
                    }
                }else if (action.Equals("add"))
                {
                    Console.WriteLine("Oeuvre :");
                    string oeuvre = Console.ReadLine();
                    Console.WriteLine("Nom :");
                    string nom = Console.ReadLine();
                    Console.WriteLine("Lien youtube :");
                    string url = Console.ReadLine();
                    if (controller.Generator.ContainsMusique(url))
                    {
                        Console.WriteLine("L'extrait a déjà été ajouter");
                        Console.ReadKey();
                    }
                    else
                    {
                        Console.WriteLine("Categorie : (Film, serie, anime, ...)");
                        string categorie = Console.ReadLine().ToUpper();
                        if(Enum.TryParse<Categorie>(categorie, out Categorie categorie1))
                        {
                            Console.WriteLine("Type de musique : (Opening, Ending, Ost)");
                            string type = Console.ReadLine().ToUpper();
                            if(Enum.TryParse<TypeMusique>(type, out TypeMusique typeMusique))
                            {
                                controller.Generator.addMusic(oeuvre, nom, url, categorie, type);
                                Console.WriteLine("Musique ajouté avec succés !");
                                Console.ReadKey();
                                Console.Clear();
                            }
                            else
                            {
                                Console.WriteLine("Veuillez entrer une catégorie valide");
                                Console.ReadKey();
                            }
                        }
                        else
                        {
                            Console.WriteLine("Veuillez entrer une catégorie valide");
                            Console.ReadKey();
                        }
                    }
                }
            }
        }
    }
}
