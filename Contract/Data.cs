using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
   public class Data
   {
        static int br = 0;
        private int id;
        private String region;
        private String grad;
        private int godina;
        private int potrosnja;

        public Data()
        {

        }

        public Data(string region, string grad, int godina, int potrosnja)
        {
            this.Id = br;
            this.Region = region;
            this.Grad = grad;
            this.Godina = godina;
            this.Potrosnja = potrosnja;
        }

        public int Id { get => id; set => id = value; }
        public string Region { get => region; set => region = value; }
        public string Grad { get => grad; set => grad = value; }
        public int Godina { get => godina; set => godina = value; }
        public int Potrosnja { get => potrosnja; set => potrosnja = value; }

        public override string ToString()
        {
            return $"{Id};{Region};{Grad};{Godina};{Potrosnja}";
        }
    }
}
