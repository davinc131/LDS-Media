using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MediaPlayer_01.Utilitarias
{
    /// <summary>
    /// Esta classe captura um frame de um video e armazena em uma pasta do sistema
    /// </summary>
    public class ExtractImage
    {
        public string ExtrairImagem(double t, string s)
        {
            //Este bloco desmonta o caminho da matriz para obter apenas o nome do video
            string[] cadeia = s.Split('\\');
            string qq = cadeia[cadeia.Length - 1];
            string[] cadeia1 = qq.Split('.');
            string nn = cadeia1[cadeia1.Length - 2];

            string caminhoImagem = "";

            //Este bloco irá remontar o caminho para que a imagem extraida do video posso
            //ser armazenada
            for(int i = 0; i <= cadeia.Length - 2; i++)
            {
                caminhoImagem += cadeia[i] + "\\";
            }

            var inputFile = new MediaFile { Filename = s };
            var outputFile = new MediaFile { Filename = caminhoImagem + nn+".jpg" };
            Random random = new Random();
            int randomNumber = random.Next(0, (int)t);

            using (var engine = new Engine())
            {
                engine.GetMetadata(inputFile);

                // Saves the frame located on the 15th second of the video.
                var options = new ConversionOptions { Seek = TimeSpan.FromSeconds(randomNumber) };
                engine.GetThumbnail(inputFile, outputFile, options);
            }

            return outputFile.Filename;
        }
    }
}
