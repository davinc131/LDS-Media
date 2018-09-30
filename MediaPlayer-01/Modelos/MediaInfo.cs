using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MediaPlayer_01.Modelos
{
    public class MediaInfo
    {
        public BitmapImage ImageMiniatura { get; set; }
        public string CaminhoDaImagem { get; set; }
        public string Nome { get; set; }
        public string CaminhoDaMidia { get; set; }
        public string DescricaoDaMidia { get; set; }
    }
}
