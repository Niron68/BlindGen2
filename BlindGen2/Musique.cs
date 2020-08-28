using System;
using System.Collections.Generic;
using System.Text;

namespace BlindGen2
{
    public class Musique
    {
        public MusiqueInfo Info { get; private set; }
        public string Chemin
        {
            get => "Video\\" + Info.Oeuvre + "_" + Info.TypeMusique.ToString() + "_" + Info.Nom + ".mp4";
        }
        
        public string CheminTemp
        {
            get => "Temp" + Chemin;
        }

        public string CheminSon
        {
            get => "Son\\" + Info.Oeuvre + "_" + Info.TypeMusique.ToString() + "_" + Info.Nom + ".mp3";
        }

        public string CheminExtrait
        {
            get => "Extrait\\" + Info.Oeuvre + "_" + Info.TypeMusique.ToString() + "_" + Info.Nom + ".mp4";
        }

        public Musique(MusiqueInfo info)
        {
            Info = info;
        }

        public override bool Equals(object obj)
        {
            if(obj is Musique m){
                return m.Info.Equals(Info);
            }
            return false;
        }

        public bool SameOeuvre(Musique mus) => Info.Oeuvre.Equals(mus.Info.Oeuvre);
    }
}
