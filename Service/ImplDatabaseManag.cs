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
    }
}
