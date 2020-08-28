using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace BlindGen2
{
    public class Monteur
    {
        private Process ffmpeg { get; set; }

        public Monteur()
        {
            ffmpeg = new Process();
            ffmpeg.StartInfo.FileName = "ffmpeg.exe";
            ffmpeg.StartInfo.UseShellExecute = true;
            ffmpeg.StartInfo.CreateNoWindow = false;
        }

        public void CreateBlindtest(List<Musique> musiques, string pause)
        {
            string name = DateTime.Now.Ticks.ToString();
            string args = "-i " + pause;
            foreach(Musique mus in musiques)
            {
                args += " -i \"" + mus.Chemin + "\" -i \"" + mus.CheminTemp + "\"";
            }
            args += " -filter_complex \"[0:v]scale=1280:720:force_original_aspect_ratio=1[v0];";
            for(int i = 0; i < musiques.Count; i++)
            {
                args += String.Format(" [{0}:v]scale=1280:720:force_original_aspect_ratio=1[v{0}];", (i * 2) + 2);
            }
            for(int i = 0; i < musiques.Count; i++)
            {
                args += String.Format("[v0][v{1}][{0}:a]", (i*2)+1, (i*2)+2);
            }
            args += "concat=n=" + musiques.Count + ":v=2:a=1[outv][outa]\" -map \"[outv]\" -map \"[outa]\" " + name + ".mp4";

        }

        public void CutVideo(string input, string output, int debut, int fin)
        {
            int duration = fin - debut;
            ffmpeg.StartInfo.Arguments = "-i \"" + input + "\" -n -ss " + debut + " -t " + duration + " \"" + output + "\"";
            ffmpeg.Start();
            ffmpeg.WaitForExit();
        }

        public void CreateExtract(Musique musique, string pause, int fadeout = 14)
        {
            string textFilter = "drawtext=fontfile='OpenSans-Bold.ttf':text='" + musique.Info.Oeuvre + "':fontcolor=white:fontsize=48:borderw=2:x=0:y=(h-text_h)";
            ffmpeg.StartInfo.Arguments = "-i \"" + pause + "\"" + " -i \"" + musique.CheminTemp + "\" -i \"" + musique.CheminSon + "\" -r 30 -n -filter_complex \"[0:v]scale=1280:720,fade=t=out:d=1:st=" + fadeout + "[v0]; [1:v]scale=1280:720," + textFilter + "[v1]; [2:a]afade=t=out:d=1:st=" + fadeout + "[outa]; [v0][v1]concat=n=2:v=1:a=0[outv]\" -map [outv] -map [outa] \"" + musique.CheminExtrait + "\"";
            ffmpeg.Start();
            ffmpeg.WaitForExit();
        }
        
        public void ConcatMusique(List<Musique> musiques, string outputFolder = "Blindtest")
        {
            string name = Path.Combine(outputFolder, DateTime.Now.Ticks.ToString());
            string args = "";
            foreach(Musique mus in musiques)
            {
                args += "-i \"" + mus.CheminExtrait + "\" ";
            }
            args += "-filter_complex ";
            for(int i = 0; i < musiques.Count; i++)
            {
                args += String.Format("[{0}:v][{0}:a]", i);
            }
            args += "concat=unsafe=1:n=" + musiques.Count + ":v=1:a=1[outv][outa] -map [outv] -map [outa] \"" + name + ".mp4\"";
            ffmpeg.StartInfo.Arguments = args;
            ffmpeg.Start();
            ffmpeg.WaitForExit();
        }

    }
}
