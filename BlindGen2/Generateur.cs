using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlindGen2
{
    public class Generateur
    {
        public ConfigurationBlindtest Config { get; private set; }
        public YTBDownloader Downloader { get; private set; }
        public List<Musique> Musiques { get; private set; }
        public Monteur Renderer { get; private set; }
        public MusiqueCreator Creator { get; private set; }
        public int NombreOeuvre
        {
            get
            {
                int res = 0;
                List<string> oeuvres = new List<string>();
                foreach(Musique mus in Musiques)
                {
                    if (!oeuvres.Contains(mus.Info.Oeuvre))
                    {
                        res++;
                        oeuvres.Add(mus.Info.Oeuvre);
                    }
                }
                return res;
            }
        }

        public Generateur(ConfigurationBlindtest config)
        {
            Directory.CreateDirectory("TempVideo");
            Directory.CreateDirectory("Son");
            Directory.CreateDirectory("Extrait");
            Directory.CreateDirectory("Blindtest");
            Config = config;
            Downloader = new YTBDownloader();
            Creator = new MusiqueCreator();
            Renderer = new Monteur();
            Musiques = new List<Musique>();
            if (File.Exists("musiques.json"))
            {
                List<MusiqueInfo> musiqueInfos = JsonConvert.DeserializeObject<List<MusiqueInfo>>(File.ReadAllText("musiques.json"));
                if(musiqueInfos != null)
                {
                    foreach(MusiqueInfo info in musiqueInfos)
                    {
                        Musiques.Add(new Musique(info));
                    }
                }
            }
        }

        public string Test()
        {
            Downloader.Update();
            //addMusic("Stranger Things", "Opening", "https://www.youtube.com/watch?v=-RcPZdihrp4", "SERIE", "OPENING");
            //foreach(Musique mus in Musiques)
            //{
            //    if (!File.Exists(mus.Chemin))
            //    {
            //        Downloader.downloadVideo(mus.Info);
            //    }
            //}
            //Renderer.CutVideo(Musiques[0].Chemin, Musiques[0].CheminTemp, 10, 12);
            List<Musique> randomList = createRandomList(10);
            string res = "";
            foreach(Musique mus in randomList)
            {
                res += mus.Chemin + "\n";
                Renderer.CutVideo(mus.Chemin, mus.CheminTemp, 10, 15);
                Renderer.CutVideo(mus.Chemin, mus.CheminSon, 0, 15);
                Renderer.CreateExtract(mus, "Video\\decompte.mp4");
            }
            Renderer.ConcatMusique(randomList);
            //Renderer.CreateBlindtest(randomList, "Video\\decompte.mp4");
            return res + "\n" + Creator.getFullPath();
        }

        public void addMusic(string oeuvre, string nom, string url, string categorie, string typeMusique)
        {
            MusiqueInfo info = Creator.addMusic(oeuvre, nom, url, categorie, typeMusique);
            if(info != null)
            {
                Musiques.Add(new Musique(info));
            }
        }

        public List<Musique> createRandomList(int nb)
        {
            List<Musique> res = new List<Musique>();
            Random rand = new Random();
            int max = nb;
            if (nb > NombreOeuvre)
                max = NombreOeuvre;
            for(int i = 0; i < max; i++)
            {
                bool stop;
                Musique musique;
                do
                {
                    int r = rand.Next(Musiques.Count);
                    musique = Musiques[r];
                    stop = res.Exists(mus => mus.SameOeuvre(musique));
                } while (stop);
                res.Add(musique);
            }
            return res;
        }

        public int ToDownload(List<Musique> musiques)
        {
            int res = 0;
            foreach(Musique mus in musiques)
            {
                if (!File.Exists(mus.Chemin))
                {
                    res++;
                }
            }
            return res;
        }

        public void DownloadMusiques(List<Musique> musiques)
        {
            foreach (Musique mus in musiques)
            {
                if (!File.Exists(mus.Chemin))
                {
                    Downloader.downloadVideo(mus.Info);
                }
            }
        }

        public void GenerateBlindtest(int nb)
        {
            List<Musique> musiques = createRandomList(nb);
            DownloadMusiques(musiques);
            foreach (Musique mus in musiques)
            {
                Renderer.CutVideo(mus.Chemin, mus.CheminTemp, 10, 15);
                Renderer.CutVideo(mus.Chemin, mus.CheminSon, 0, 15);
                Renderer.CreateExtract(mus, "Video\\decompte.mp4");
            }
            Renderer.ConcatMusique(musiques);
        }

        public bool ContainsMusique(string url) => Musiques.Exists(mus => mus.Info.Url.Equals(url));

    }
}
