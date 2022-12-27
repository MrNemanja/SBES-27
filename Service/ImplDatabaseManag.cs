using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Contract;
using System.IO;

namespace Service
{
    public class ImplDatabaseManag : IDatabaseManagement
    {
        public static Dictionary<String, Dictionary<int, Data>> Databases = new Dictionary<string, Dictionary<int, Data>>();

        public void CreateNewTxtFile(String txtFileName)
        {
            try
            {
                // Create a new file     
                FileStream fs = File.Create(txtFileName);
                fs.Dispose();

            }
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }

        public void DeleteTxtFile(String txtFileName)
        {
            try
            {
                // Delete txt file
                File.Delete(txtFileName);
            }
            catch(Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }

        public String CreateDatabase(String DatabaseName)
        {
            if (!Databases.ContainsKey(DatabaseName))
            {
                Databases.Add(DatabaseName, new Dictionary<int, Data>());
                CreateNewTxtFile(DatabaseName);
                return $"Baza podataka sa imenom '{DatabaseName}' je uspesno kreirana\n";
            }
            else return $"Baza podataka sa imenom '{DatabaseName}' vec postoji\n";
        }

        public String DeleteDatabase(String DatabaseName)
        {
            if (Databases.ContainsKey(DatabaseName))
            {
                Databases.Remove(DatabaseName);
                DeleteTxtFile(DatabaseName);
                return $"Baza podataka sa imenom '{DatabaseName}' je uspesno obrisana\n";
            }
            else return $"Baza podataka sa imenom '{DatabaseName}' ne postoji\n";
        }
        public string Read(string DatabaseName, string region, string grad)
        {
            if (Databases.ContainsKey(DatabaseName))
            {
                FileStream stream = new FileStream(DatabaseName, FileMode.Open);
                StreamReader sr = new StreamReader(stream);
                string line;
                List<Data> datas = new List<Data>();
                int sumaPoGradu = 0;
                int sumaPoRegionu = 0;
                double brGr = 0;
                double brR = 0;
                double prosekPoGradu = 0;
                double prosekPoRegionu = 0;
                double max = 0;
                Data maxPotrosac = new Data();
                //Uciitavanje reda iz baze i dodavanje u listu
                while ((line = sr.ReadLine()) != null)
                {
                    string[] tokens = line.Split(';');
                    Data data = new Data(tokens[1], tokens[2], int.Parse(tokens[3]), int.Parse(tokens[4]));
                    datas.Add(data);
                }

                //Prosek po regionu
                foreach (var d in datas)
                {
                    if (d.Region.Equals(region))
                    {
                        sumaPoRegionu += d.Potrosnja;
                        brR++;
                    }
                }
                if (!(sumaPoRegionu == 0 && brR == 0))
                {
                    prosekPoRegionu = sumaPoRegionu / brR;
                }


                //Prosek po gradu
                foreach (var d in datas)
                {

                    if (d.Grad.Equals(grad))
                    {
                        sumaPoGradu += d.Potrosnja;
                        brGr++;
                    }
                }
                if (!(sumaPoGradu == 0 && brGr == 0))
                {
                    prosekPoGradu = sumaPoGradu / brGr;
                }
                //Pronalazak najveceg potrosaca za odredjeni region
                foreach (var d in datas)
                {
                    if (d.Region.Equals(region))
                    {
                        if (d.Potrosnja > max)
                        {
                            max = d.Potrosnja;
                        }
                        maxPotrosac = d;
                    }
                }

                return $"Prosek za region ({region}):{prosekPoRegionu}\nProsek za grad ({grad}):{prosekPoGradu}\nNajveci potrosac za region ({region}):\n{maxPotrosac}";


            }
            else
            {
                return $"Baza:{DatabaseName} ne postoji";
            }

        }

        public string Write(string DatabaseName, string region, string grad, int godina, int potrosnja)
        {
            if (Databases.ContainsKey(DatabaseName))
            {
                Data data = new Data(region, grad, godina, potrosnja);
                FileStream stream = new FileStream(DatabaseName, FileMode.Append);
                StreamWriter sw = new StreamWriter(stream);
                sw.WriteLine(data);
                sw.Close();
                stream.Close();
                return $"Upis u bazu:{DatabaseName} je uspesno odradjen";
            }
            else
            {
                return $"Baza:{DatabaseName} ne postoji";
            }

        }
    }
}
