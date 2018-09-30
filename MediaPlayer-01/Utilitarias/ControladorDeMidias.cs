using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediaPlayer_01.Modelos;
using System.Windows.Media.Imaging;

namespace MediaPlayer_01.Utilitarias
{
    /// <summary>
    /// Esta classe controla as midias que serão tocadas
    /// Ao selecionar a o video na sua pasta ele é adcionado a lista de midias
    /// E a midia atual selecionada será exibida no tocador.
    /// </summary>
    public class ControladorDeMidias
    {
        public static List<MediaInfo> ListaDeMedia { get; set; }
        private ExtractImage extractImage { get; set; }
        private RetornaImagemClass RetonaImagem { get; set; }
        public int MediaAtual { get; set; }
        public bool IsExiste { get; set; }

        public ControladorDeMidias()
        {
            ListaDeMedia = new List<MediaInfo>();
            extractImage = new ExtractImage();
            RetonaImagem = new RetornaImagemClass();
        }

        public string AdicionarParaLista(string caminhoDaMidia)
        {
            if(ListaDeMedia.Count > 0)
            {
                for (int i = 0; i < ListaDeMedia.Count; i++)
                {
                    if (ListaDeMedia[i].CaminhoDaMidia == caminhoDaMidia)
                    {
                        MediaAtual = i;
                        IsExiste = true;
                    }
                }

                if (IsExiste == true)
                {
                    IsExiste = false;
                    return ListaDeMedia[MediaAtual].CaminhoDaMidia;
                }
                else
                {
                    MediaInfo novaMidia = new MediaInfo();
                    novaMidia.CaminhoDaMidia = caminhoDaMidia;

                    CapturaNome(caminhoDaMidia, novaMidia);

                    ListaDeMedia.Add(novaMidia);

                    MediaAtual = ListaDeMedia.Count - 1;

                    return ListaDeMedia[MediaAtual].CaminhoDaMidia;
                }
            }
            else
            {
                MediaInfo novaMidia = new MediaInfo();
                novaMidia.CaminhoDaMidia = caminhoDaMidia;

                CapturaNome(caminhoDaMidia, novaMidia);

                ListaDeMedia.Add(novaMidia);

                MediaAtual = ListaDeMedia.Count - 1;

                return ListaDeMedia[MediaAtual].CaminhoDaMidia;
            }
        }

        public void CapturarImagem(double tempo, string caminho)
        {
            ListaDeMedia[ListaDeMedia.Count - 1].CaminhoDaImagem = extractImage.ExtrairImagem(tempo, caminho);
            ListaDeMedia[ListaDeMedia.Count - 1].ImageMiniatura = CarregarImagem(ListaDeMedia[ListaDeMedia.Count - 1].CaminhoDaImagem);
        }

        public BitmapImage CarregarImagem(string c)
        {
            BitmapImage b = RetonaImagem.ImagemVolume(c);
            return b;
        }

        public int TamanhoDaLista()
        {
            return ListaDeMedia.Count;
        }

        public int MediaAtualRetorno()
        {
            return MediaAtual;
        }

        public string Avancar()
        {
            MediaAtual++;
            int proxima = MediaAtual;
            int ultima = ListaDeMedia.Count - 1;

            if (proxima <= ultima)
            {
                MediaAtual = proxima;
                return ListaDeMedia[MediaAtual].CaminhoDaMidia;
            }
            else
            {
                return null;
            }
        }

        public string Retroceder()
        {
            MediaAtual--;

            int anterior = MediaAtual;

            if(anterior >= 0)
            {
                MediaAtual = anterior;
                return ListaDeMedia[MediaAtual].CaminhoDaMidia;
            }
            else
            {
                return null;
            }
        }

        public List<MediaInfo> ListMediaInfo()
        {
            return ListaDeMedia;
        }

        private MediaInfo CapturaNome(string n, MediaInfo m)
        {
            string[] cadeia = n.Split('\\');
            m.Nome = cadeia[cadeia.Length - 1];

            return m;
        }

        public string RetornaMediaSelecionada(string nome)
        {
            for(int i = 0; i < ListaDeMedia.Count; i++)
            {
                if(ListaDeMedia[i].Nome == nome)
                {
                    return ListaDeMedia[i].CaminhoDaMidia;
                }
            }

            return null;
        }
    }
}
