using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
   public class Data
   {
        private int id;
        private String region;
        private String grad;
        private int godina;
        private double[] potrosnja;

        public Data()
        {

        }

        public Data(int id, string region, string grad, int godina, double[] potrosnja)
        {
            this.Id = id;
            this.Region = region;
            this.Grad = grad;
            this.Godina = godina;
            this.Potrosnja = potrosnja;
        }

        public int Id { get => id; set => id = value; }
        public string Region { get => region; set => region = value; }
        public string Grad { get => grad; set => grad = value; }
        public int Godina { get => godina; set => godina = value; }
        public double[] Potrosnja { get => potrosnja; set => potrosnja = value; }
    }
}
