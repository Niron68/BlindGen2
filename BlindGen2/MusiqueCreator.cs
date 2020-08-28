using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace BlindGen2
{
    public class MusiqueCreator
    {
        public string FichierJson { get; private set; }

        public MusiqueCreator()
        {
            FichierJson = "musiques.json";
        }

        public MusiqueInfo addMusic(string oeuvre, string nom, string url, string categorie, string typeMusique)
        {
            if (!File.Exists(FichierJson))
                File.Create(FichierJson).Close();
            List<MusiqueInfo> list = JsonConvert.DeserializeObject<List<MusiqueInfo>>(File.ReadAllText(FichierJson));
            if(list == null)
            {
                list = new List<MusiqueInfo>();
            }
            if(list.Exists(it => it.Nom.Equals(nom) && it.Oeuvre.Equals(oeuvre)))
            {
                return null;
            }
            if(Enum.TryParse<Categorie>(categorie, out Categorie cat) && Enum.TryParse<TypeMusique>(typeMusique, out TypeMusique type))
            {
                MusiqueInfo musiqueInfo = new MusiqueInfo(oeuvre, nom, url, cat, type);
                list.Add(musiqueInfo);
                using(StreamWriter sw = File.CreateText(FichierJson))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(sw, list);
                }
                return musiqueInfo;
            }
            return null;
        }

        public string getFullPath() => Directory.GetParent(FichierJson).FullName;

    }
}
