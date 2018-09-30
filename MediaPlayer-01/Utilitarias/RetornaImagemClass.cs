using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace MediaPlayer_01.Utilitarias
{
    /// <summary>
    /// Retorna um bitmapimage
    /// </summary>
    public class RetornaImagemClass
    {
        public BitmapImage ImagemVolume(string caminho)
        {
            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri(caminho, UriKind.Relative);
            bi3.EndInit();

            return bi3;
        }
    }
}
