using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaPlayer_01.Utilitarias
{
    public class TimerClass
    {
        private double Horas { get; set; }
        private double Minutos { get; set; }
        private double Segundos { get; set; }

        public void Timer(double horas, double minutos, double segudos)
        {
            Horas = horas;
            Minutos = minutos;
            Segundos = segudos;
        }

        public string toString()
        {
            return String.Format("{0}:{1}:{2}", Horas, Minutos, Segundos);
        }
    }
}
