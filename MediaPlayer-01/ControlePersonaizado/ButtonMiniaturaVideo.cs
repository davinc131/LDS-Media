using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace MediaPlayer_01.ControlePersonaizado
{
    public class ButtonMiniaturaVideo:Button
    {
        public Grid MeuGrid = new Grid();
        public TextBlock TxtNomeDoVideo { get; set; }
        public MediaElement MiniaturaMedia { get; set; }

        public ButtonMiniaturaVideo()
        {
            TxtNomeDoVideo = new TextBlock();
            MiniaturaMedia = new MediaElement();

            TxtNomeDoVideo.TextWrapping = TextWrapping.Wrap;
            TxtNomeDoVideo.IsManipulationEnabled = false;

            MeuGrid.HorizontalAlignment = HorizontalAlignment.Center;
            MeuGrid.VerticalAlignment = VerticalAlignment.Center;

            ColumnDefinition colDef1 = new ColumnDefinition();

            MeuGrid.ColumnDefinitions.Add(colDef1);

            RowDefinition rowDef1 = new RowDefinition();
            RowDefinition rowDef2 = new RowDefinition();

            MeuGrid.RowDefinitions.Add(rowDef1);
            MeuGrid.RowDefinitions.Add(rowDef2);

            TxtNomeDoVideo.Text = "Novo Controle";
            Grid.SetColumn(TxtNomeDoVideo, 0);
            Grid.SetRow(TxtNomeDoVideo, 1);

            MiniaturaMedia.Source = new Uri(@"C:\\Users\\Public\\Videos\\Sample Videos\\Wildlife.jpg");
            Grid.SetColumn(MiniaturaMedia, 0);
            Grid.SetRow(MiniaturaMedia, 0);

            MeuGrid.Children.Add(TxtNomeDoVideo);
            MeuGrid.Children.Add(MiniaturaMedia);

            this.Content = MeuGrid;
        }
    }
}
