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

namespace MediaPlayer_01.ControlePersonaizado
{
    /// <summary>
    /// Interação lógica para MiniMediaInfo.xam
    /// </summary>
    public partial class MiniMediaInfo : UserControl
    {
        #region Construtor do controle

        public MiniMediaInfo()
        {
            InitializeComponent();
        }

        #endregion

        #region Propriedades do controle

        public ImageSource CustonMiniatura
        {
            get { return ImgMiniatura.Source; }
            set { ImgMiniatura.Source = value; }
        }

        public string CustonNome
        {
            get { return LbNome.Content.ToString(); }
            set { LbNome.Content = value; }
        }

        #endregion

        #region Controlador de eventos do controle

        //// Create a custom routed event by first registering a RoutedEventID
        //// This event uses the bubbling routing strategy
        //public static readonly RoutedEvent TapEvent = EventManager.RegisterRoutedEvent(
        //    "Tap", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MiniMediaInfo));

        //// Provide CLR accessors for the event
        //public event RoutedEventHandler Tap
        //{
        //    add { AddHandler(TapEvent, value); }
        //    remove { RemoveHandler(TapEvent, value); }
        //}

        //// This method raises the Tap event
        //void RaiseTapEvent()
        //{
        //    RoutedEventArgs newEventArgs = new RoutedEventArgs(MiniMediaInfo.TapEvent);
        //    RaiseEvent(newEventArgs);
        //}
        //// For demonstration purposes we raise the event when the MyButtonSimple is clicked
        //protected override void OnClick()
        //{
        //    RaiseTapEvent();
        //}

        public static readonly RoutedEvent AddClickEvent = EventManager.RegisterRoutedEvent("AddClick", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(MiniMediaInfo));

        public event RoutedEventHandler AddClick
        {
            add { AddHandler(AddClickEvent, value); }
            remove { RemoveHandler(AddClickEvent, value); }
        }

        void RaiseAddClickEvent()
        {
            RoutedEventArgs newEventArgs = new RoutedEventArgs(MiniMediaInfo.AddClickEvent);
        }

        protected void OnAddClick()
        {
            //RaiseAddClickEvent();
            RaiseEvent(new RoutedEventArgs(AddClickEvent));
        }

        //objects events

        private void btnAdd_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //RaiseAddClickEvent();
            RaiseEvent(new RoutedEventArgs(AddClickEvent));
        }
        #endregion
    }
}
