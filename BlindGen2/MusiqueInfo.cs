using System;

namespace BlindGen2
{
    public class MusiqueInfo
    {
        public string Oeuvre { get; set; }
        public string Nom { get; set; }
        public string Url { get; set; }
        public Categorie Categorie { get; set; }
        public TypeMusique TypeMusique { get; set; }

        public MusiqueInfo(string oeuvre, string nom, string url, Categorie categorie, TypeMusique typeMusique)
        {
            (Oeuvre, Nom, Url, Categorie, TypeMusique) = (oeuvre, nom, url, categorie, typeMusique);
        }

        public bool Equals(MusiqueInfo obj) => obj.Nom.Equals(Nom) && obj.Oeuvre.Equals(Oeuvre);
    }
}
