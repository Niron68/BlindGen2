using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace BlindGen2
{
    public class YTBDownloader
    {
        private Process YoutubeDL { get; set; }

        public YTBDownloader()
        {
            YoutubeDL = new Process();
            YoutubeDL.StartInfo.FileName = "youtube-dl.exe";
            YoutubeDL.StartInfo.CreateNoWindow = false;
            YoutubeDL.StartInfo.UseShellExecute = true;
            YoutubeDL.StartInfo.RedirectStandardOutput = false;
        }

        public string Update()
        {
            YoutubeDL.StartInfo.Arguments = "-U";
            YoutubeDL.Start();
            YoutubeDL.WaitForExit();
            return "";
        }

        public string downloadVideo(MusiqueInfo info)
        {
            YoutubeDL.StartInfo.Arguments = "-f mp4 --recode-video mp4 -o \"Video\\" + info.Oeuvre + "_" + info.TypeMusique.ToString() + "_" + info.Nom + ".mp4\" " + info.Url;
            YoutubeDL.Start();
            YoutubeDL.WaitForExit();
            return YoutubeDL.StartInfo.Arguments + "\n";
        }
    }
}
