using System;
using System.Collections.Generic;
using System.Text;

namespace BlindGen2
{
    public class Controller
    {
        public Generateur Generator { get; private set; }

        public Controller()
        {
            Generator = new Generateur(new ConfigurationBlindtest { DossierSortie = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) });
        }

        public string Test()
        {
            return Generator.Test();
        }
    }
}
