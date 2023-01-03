using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Contract
{
   [DataContract]
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
            br++;
        }
        [DataMember]
        public int Id { get => id; set => id = value; }
        [DataMember]
        public string Region { get => region; set => region = value; }
        [DataMember]
        public string Grad { get => grad; set => grad = value; }
        [DataMember]
        public int Godina { get => godina; set => godina = value; }
        [DataMember]
        public int Potrosnja { get => potrosnja; set => potrosnja = value; }

        public override string ToString()
        {
            return $"{Id};{Region};{Grad};{Godina};{Potrosnja}";
        }
    }
    [DataContract]
    public class modifiedData
    {
        private int id;
        private String region;
        private String grad;
        private int godina;
        private int potrosnja;

        public modifiedData()
        {

        }

        public modifiedData(int id, string region, string grad, int godina, int potrosnja)
        {
            this.Id = id;
            this.Region = region;
            this.Grad = grad;
            this.Godina = godina;
            this.Potrosnja = potrosnja;
        }
        [DataMember]
        public int Id { get => id; set => id = value; }
        [DataMember]
        public string Region { get => region; set => region = value; }
        [DataMember]
        public string Grad { get => grad; set => grad = value; }
        [DataMember]
        public int Godina { get => godina; set => godina = value; }
        [DataMember]
        public int Potrosnja { get => potrosnja; set => potrosnja = value; }

        public override string ToString()
        {
            return $"{Id};{Region};{Grad};{Godina};{Potrosnja}";
        }
    }
}
