using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Forms;
using MediaPlayer_01.Utilitarias;
using MediaPlayer_01.ControlePersonaizado;
using MediaPlayer_01.Modelos;

namespace MediaPlayer_01
{
    /// <summary>
    /// Interação lógica para MainWindow.xam
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Propriedades

        private bool mute = false;
        private bool dadosCarregados = false;
        public bool mouseMoved { get; set; }
        public bool fullscreen { get; set; }
        private double volume;
        private TimerClass timer = new TimerClass();
        private ControladorDeMidias controladorDeMidias = new ControladorDeMidias();
        private bool IsDispatcherTime = false;
        private static List<MediaInfo> ListaDeMedia { get; set; }
        private string caminho { get; set; }
        private RetornaImagemClass imagem { get; set; }

        #endregion

        #region Main

        public MainWindow()
        {
            InitializeComponent();

            ListaDeMedia = new List<MediaInfo>();
            imagem = new RetornaImagemClass();

        }

        #endregion

        #region Eventos Gerenciados

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog open;
                open = new OpenFileDialog();

                open.AddExtension = true;
                open.DefaultExt = "*.*";
                open.Filter = "Media Files (*.*) | *.*";

                if (open.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    try
                    {
                        Player.Source = new Uri(controladorDeMidias.AdicionarParaLista(open.FileName));
                        caminho = open.FileName;
                    }
                    catch
                    {
                        new NullReferenceException("Error:...");
                    }

                    if (!IsDispatcherTime)
                    {
                        System.Windows.Threading.DispatcherTimer dispatcherTime = new System.Windows.Threading.DispatcherTimer();
                        dispatcherTime.Tick += new EventHandler(timer_Tick);
                        dispatcherTime.Interval = new TimeSpan(0, 0, 1);
                        dispatcherTime.Start();
                        IsDispatcherTime = true;
                    }

                    this.PreviewMouseUp += new MouseButtonEventHandler(sliderVolume_MouseLeftButtonUp);

                    Player.Play();
                    volume = Player.Volume;
                    PreencherListaDeMidia();
                }
            }
            catch (Exception error)
            {
                System.Windows.MessageBox.Show("Open File Error: " + error.Message);
            }
        }

        private void btnPlay_Click(object sender, RoutedEventArgs e)
        {
            Player.Play();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            Player.Stop();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            Player.Pause();
        }

        private void sliderVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Player.Volume = sliderVolume.Value;
            StringFormat();
        }

        private void sliderReproducao_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            try
            {
                TimeSpan ts = TimeSpan.FromSeconds(e.NewValue);
                Player.Position = ts;
            }
            catch (Exception error)
            {
                System.Windows.MessageBox.Show("Erro ao mover ao redefinir posição do vídeo - ValueChanged_SliderReprodução. Error: " + error.Message);
            }
        }

        //Regula a posição do video quando o slider de reprodução é movido
        private void Player_MediaOpened(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Player.NaturalDuration.HasTimeSpan)
                {
                    TimeSpan ts = TimeSpan.FromMilliseconds(Player.NaturalDuration.TimeSpan.TotalMilliseconds);
                    sliderReproducao.Maximum = ts.TotalSeconds;
                }
            }
            catch (Exception error)
            {
                System.Windows.MessageBox.Show("Erro ao mover ao redefinir posição do vídeo - MediaOpened. Error: " + error.Message);
            }
        }

        private void btnVolume_Click(object sender, RoutedEventArgs e)
        {
            ControleMute();
        }

        private void sliderVolume_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sliderVolume.Value > 0)
            {
                volume = Player.Volume;
                mute = true;
                ControleMute();
            }
            else
            {
                mute = false;
                ControleMute();
            }
        }

        private void Player_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ControlFullScreenMode(e);
        }

        private void Container_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            mouseMoved = true;
        }

        private void btnAterior_Click(object sender, RoutedEventArgs e)
        {
            Tocador(controladorDeMidias.Retroceder());
        }

        private void btnProxima_Click(object sender, RoutedEventArgs e)
        {
            Tocador(controladorDeMidias.Avancar());
        }

        private void btnFullScreen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ControlFullScreenMode();
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Fullscreen Error: "+ex.Message);
            }
        }

        #endregion

        #region Métodos de Controle

        void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                sliderReproducao.Value = Player.Position.TotalSeconds;
                CronometroProgressivo();
                ControlVisibleComponents();


                if (Player.NaturalDuration.HasTimeSpan)
                {
                    sliderReproducao.Maximum = Player.NaturalDuration.TimeSpan.TotalSeconds;
                    sliderVolume.Value = Player.Volume;
                    StringFormat();
                }

                Controla_Retroceder_Avancar();
            }
            catch (Exception error)
            {
                System.Windows.MessageBox.Show("Erro no processamento do vídeo. ERROR: " + error.Message);
            }
        }

        private void StringFormat()
        {
            try
            {
                lbVolume.Content = Math.Round(Convert.ToDecimal(sliderVolume.Value), 1) + "%";
                lbReproducao.Content = Player.NaturalDuration;
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show("Erro ao processar textos. ERROR: " + e.Message);
            }
        }

        private BitmapImage ImagemVolume(string caminho)
        {
            return imagem.ImagemVolume(caminho);
        }

        private void ControleMute()
        {
            if (!mute)
            {
                imgVolume.Source = ImagemVolume("/Resources/Sem-Volume-speakers-without-volume.png");
                Player.Volume = 0;
                sliderVolume.Value = 0;
                mute = true;

                if (Player.Volume>0)
                {
                    volume = Player.Volume;
                }
            }
            else
            {
                imgVolume.Source = ImagemVolume("/Resources/speakers-without-volume.png");
                Player.Volume = volume;
                sliderVolume.Value = volume;
                mute = false;
            }
        }

        private void CronometroProgressivo()
        {
            timer.Timer(Player.Position.Hours, Player.Position.Minutes, Player.Position.Seconds);
            lbTimer.Content = timer.toString();
        }

        //Controla a o posicionamento do media element com botão da interface
        private void ControlFullScreenMode()
        {
            if (fullscreen == false)
            {
                this.WindowStyle = WindowStyle.None;
                this.WindowState = WindowState.Maximized;
                fullscreen = true;

                imgFullscreen.Source = ImagemVolume("/Resources/icon exit fullscreen.png");

                Grid.SetColumn(Player, 0);
                Grid.SetColumnSpan(Player, 10);
                Grid.SetRow(Player, 0);
                Grid.SetRowSpan(Player, 6);
            }
            else if (fullscreen == true)
            {
                this.WindowStyle = WindowStyle.SingleBorderWindow;
                this.WindowState = WindowState.Normal;
                fullscreen = false;

                imgFullscreen.Source = ImagemVolume("/Resources/fullscreen-symbol.png");

                Grid.SetColumn(Player, 2);
                Grid.SetColumnSpan(Player, 8);
                Grid.SetRow(Player, 0);
                Grid.SetRowSpan(Player, 1);
            }
        }

        //Controla a o posicionamento do media element com dois cliques do mouse
        private void ControlFullScreenMode(MouseButtonEventArgs e)
        {
            try
            {
                if (e.ClickCount == 2 && fullscreen == false)
                {
                    this.WindowStyle = WindowStyle.None;
                    this.WindowState = WindowState.Maximized;
                    fullscreen = true;

                    imgFullscreen.Source = ImagemVolume("/Resources/icon exit fullscreen.png");

                    Grid.SetColumn(Player, 0);
                    Grid.SetColumnSpan(Player, 10);
                    Grid.SetRow(Player, 0);
                    Grid.SetRowSpan(Player, 6);
                }
                else if (e.ClickCount == 2 && fullscreen == true)
                {
                    this.WindowStyle = WindowStyle.SingleBorderWindow;
                    this.WindowState = WindowState.Normal;
                    fullscreen = false;

                    imgFullscreen.Source = ImagemVolume("/Resources/fullscreen-symbol.png");

                    Grid.SetColumn(Player, 2);
                    Grid.SetColumnSpan(Player, 8);
                    Grid.SetRow(Player, 0);
                    Grid.SetRowSpan(Player, 1);
                }
            }
            catch(Exception er)
            {
                System.Windows.MessageBox.Show("Fullscreen Error: "+er.Message);
            }
        }

        private void ControlVisibleComponents()
        {
            if (fullscreen == true && mouseMoved == false)
            {
                sliderReproducao.Visibility = Visibility.Hidden;
                sliderVolume.Visibility = Visibility.Hidden;
                btnOpenFile.Visibility = Visibility.Hidden;
                btnPause.Visibility = Visibility.Hidden;
                btnPlay.Visibility = Visibility.Hidden;
                btnStop.Visibility = Visibility.Hidden;
                btnVolume.Visibility = Visibility.Hidden;
                lbReproducao.Visibility = Visibility.Hidden;
                lbTimer.Visibility = Visibility.Hidden;
                lbVolume.Visibility = Visibility.Hidden;
                btnAterior.Visibility = Visibility.Hidden;
                btnProxima.Visibility = Visibility.Hidden;
                ScrollLista.Visibility = Visibility.Hidden;
                btnFullScreen.Visibility = Visibility.Hidden;
            }
            else if(fullscreen == true && mouseMoved == true)
            {
                sliderReproducao.Visibility = Visibility.Visible;
                sliderVolume.Visibility = Visibility.Visible;
                btnOpenFile.Visibility = Visibility.Visible;
                btnPause.Visibility = Visibility.Visible;
                btnPlay.Visibility = Visibility.Visible;
                btnStop.Visibility = Visibility.Visible;
                btnVolume.Visibility = Visibility.Visible;
                lbReproducao.Visibility = Visibility.Visible;
                lbTimer.Visibility = Visibility.Visible;
                lbVolume.Visibility = Visibility.Visible;
                btnAterior.Visibility = Visibility.Visible;
                btnProxima.Visibility = Visibility.Visible;
                ScrollLista.Visibility = Visibility.Visible;
                btnFullScreen.Visibility = Visibility.Visible;
            }
            else
            {
                sliderReproducao.Visibility = Visibility.Visible;
                sliderVolume.Visibility = Visibility.Visible;
                btnOpenFile.Visibility = Visibility.Visible;
                btnPause.Visibility = Visibility.Visible;
                btnPlay.Visibility = Visibility.Visible;
                btnStop.Visibility = Visibility.Visible;
                btnVolume.Visibility = Visibility.Visible;
                lbReproducao.Visibility = Visibility.Visible;
                lbTimer.Visibility = Visibility.Visible;
                lbVolume.Visibility = Visibility.Visible;
                btnAterior.Visibility = Visibility.Visible;
                btnProxima.Visibility = Visibility.Visible;
                ScrollLista.Visibility = Visibility.Visible;
                btnFullScreen.Visibility = Visibility.Visible;
            }
            mouseMoved = false;
        }

        private void Tocador(string t)
        {
            try { Player.Source = new Uri(t); }
            catch { new NullReferenceException("Error:..."); }

            Player.Play();
            volume = Player.Volume;
        }

        private void Controla_Retroceder_Avancar()
        {
            if (!fullscreen)
            {
                if (controladorDeMidias.TamanhoDaLista() <= 1)
                {
                    btnAterior.Visibility = Visibility.Hidden;
                    btnProxima.Visibility = Visibility.Hidden;
                }
                else
                {
                    btnAterior.Visibility = Visibility.Visible;
                    btnProxima.Visibility = Visibility.Visible;
                }

                if (controladorDeMidias.MediaAtualRetorno() > 0)
                {
                    btnAterior.Visibility = Visibility.Visible;
                }
                else
                {
                    btnAterior.Visibility = Visibility.Hidden;
                }

                if (controladorDeMidias.MediaAtualRetorno() < controladorDeMidias.TamanhoDaLista() - 1)
                {
                    btnProxima.Visibility = Visibility.Visible;
                }
                else
                {
                    btnProxima.Visibility = Visibility.Hidden;
                }
            }
        }

        //Criar uma Thread para voltar ao método
        private void PreencherListaDeMidia()
        {
            try
            {
                //System.Windows.MessageBox.Show("Entrei no método Preencher Lista de Midia - Fora do If");
                if (Player.NaturalDuration.HasTimeSpan)
                {
                    //System.Windows.MessageBox.Show("Entrei no método Preencher Lista de Midia");
                    StackLista.Children.Clear();

                    ListaDeMedia = controladorDeMidias.ListMediaInfo();

                    for (int i = 0; i < ListaDeMedia.Count; i++)
                    {
                        ButtonMiniaturaVideo mediaInfo = new ButtonMiniaturaVideo();
                        mediaInfo.Name = "media_" + i;

                        //O nome do vídeo é atribuído ao TextBlock
                        mediaInfo.TxtNomeDoVideo.Text = ListaDeMedia[i].Nome;
                        //Captura um frame do vídeo. É passado a duração do video e o caminho para que seja 
                        //capturado um frame aleatório
                        controladorDeMidias.CapturarImagem(Player.NaturalDuration.TimeSpan.TotalSeconds, caminho);
                        //O frame capturado é passado para exibição dentro do media element
                        mediaInfo.MiniaturaMedia.Source = new Uri(ListaDeMedia[i].CaminhoDaImagem);

                        StackLista.Children.Add(mediaInfo);
                    }
                }
            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show("Não foi possível carregar a lista de mídias. ERROR: "+e.Message);
            }
        }

        private void StackLista_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //FrameworkElement feSource = e.Source as FrameworkElement;
                ButtonMiniaturaVideo feSource = e.Source as ButtonMiniaturaVideo;
                Player.Source = new Uri(controladorDeMidias.RetornaMediaSelecionada(feSource.TxtNomeDoVideo.Text.ToString()));
            }
            catch (Exception error)
            {
                System.Windows.MessageBox.Show("Não foi possível carregar a mídia selecionada ERRO: "+error.Message);
            }
            
        }

        #endregion
    }
}
