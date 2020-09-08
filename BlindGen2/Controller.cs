using System;
using System.Collections.Generic;
using System.Text;

namespace BlindGen2
{
    public class Controller
    {
        public Generateur Generator { get; private set; }
        public Uploader YTBUploader { get; private set; }

        public Controller()
        {
            Generator = new Generateur(new ConfigurationBlindtest { DossierSortie = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) });
            YTBUploader = new Uploader();
        }

        public async void Upload(VideoInfo info)
        {
            await YTBUploader.Upload(info);
        }

        public string Test()
        {
            return Generator.Test();
        }
    }
}
