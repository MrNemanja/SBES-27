using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using Contract;
using System.IO;
using SecurityManager;
using System.Security.Principal;
using System.Security.Permissions;
using System.Threading;

namespace Service
{
    public class ImplDatabaseManag : IDatabaseManagement
    {
        public static Dictionary<String, Dictionary<int, Data>> Databases = new Dictionary<string, Dictionary<int, Data>>();
        public static List<String> ArchivedDatabases = new List<string>();

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
            catch (Exception Ex)
            {
                Console.WriteLine(Ex.ToString());
            }
        }

        public String CreateDatabase(String DatabaseName)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);

            if (Thread.CurrentPrincipal.IsInRole("Create"))
            {
                Audit.AuthorizationSuccess(userName, OperationContext.Current.IncomingMessageHeaders.Action);

                if (!Databases.ContainsKey(DatabaseName))
                {
                    Databases.Add(DatabaseName, new Dictionary<int, Data>());
                    CreateNewTxtFile(DatabaseName);
                    return $"Baza podataka sa imenom '{DatabaseName}' je uspesno kreirana\n";
                }
                else return $"Baza podataka sa imenom '{DatabaseName}' vec postoji\n";
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(userName,
                       OperationContext.Current.IncomingMessageHeaders.Action, "Create method need Admin permission.");
                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Message);
                }

                throw new FaultException("User " + userName + " try to call Create method. Create method need  Admin permission.");
               
            }
        }
        public String DeleteDatabase(String DatabaseName)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);
            if (Thread.CurrentPrincipal.IsInRole("Delete"))
            {
                Audit.AuthorizationSuccess(userName, OperationContext.Current.IncomingMessageHeaders.Action);

                if (Databases.ContainsKey(DatabaseName))
                {
                    Databases.Remove(DatabaseName);
                    DeleteTxtFile(DatabaseName);
                    return $"Baza podataka sa imenom '{DatabaseName}' je uspesno obrisana\n";
                }
                else return $"Baza podataka sa imenom '{DatabaseName}' ne postoji\n";
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(userName,
                       OperationContext.Current.IncomingMessageHeaders.Action, "Delete method need Admin permission.");
                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Message);
                }


                throw new FaultException("User " + userName + " try to call Delete method. Delete method need  Admin permission.");
                
            }
        }

        public string Read(string DatabaseName, string region, string grad)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);

            if (Thread.CurrentPrincipal.IsInRole("Read"))
            {
                Audit.AuthorizationSuccess(userName, OperationContext.Current.IncomingMessageHeaders.Action);
                if (Databases.ContainsKey(DatabaseName))
                {
                    FileStream stream = new FileStream(DatabaseName, FileMode.Open);
                    StreamReader sr = new StreamReader(stream);
                    string line;
                    List<modifiedData> datas = new List<modifiedData>();
                    int sumaPoGradu = 0;
                    int sumaPoRegionu = 0;
                    double brGr = 0;
                    double brR = 0;
                    double prosekPoGradu = 0;
                    double prosekPoRegionu = 0;
                    double max = 0;
                    modifiedData maxPotrosac = new modifiedData();
                    //Uciitavanje reda iz baze i dodavanje u listu
                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] tokens = line.Split(';');
                        modifiedData data = new modifiedData(int.Parse(tokens[0]), tokens[1], tokens[2], int.Parse(tokens[3]), int.Parse(tokens[4]));
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
                    sr.Close();
                    stream.Close();

                    return $"Prosek za region ({region}):{prosekPoRegionu}\nProsek za grad ({grad}):{prosekPoGradu}\nNajveci potrosac za region ({region}):\n{maxPotrosac}";


                }
                else
                {
                    return $"Baza:{DatabaseName} ne postoji";
                }
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(userName,
                       OperationContext.Current.IncomingMessageHeaders.Action, "Read method need Reader permission.");
                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Message);
                }


                throw new FaultException("User " + userName + " try to call Read method. Read method need  Reader permission.");
               
            }
        }
        public string Write(string DatabaseName, string region, string grad, int godina, int potrosnja)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);

            if (Thread.CurrentPrincipal.IsInRole("Write"))
            {
                Audit.AuthorizationSuccess(userName, OperationContext.Current.IncomingMessageHeaders.Action);

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
            else
            {
                try
                {
                    Audit.AuthorizationFailed(userName,
                       OperationContext.Current.IncomingMessageHeaders.Action, "Write method need Writer permission.");
                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Message);
                }


                throw new FaultException("User " + userName + " try to call Write method. Write method need  Writer permission.");
                
            }
        }

        public String ArchiveDatabases(String DatabaseName)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);

            if (Thread.CurrentPrincipal.IsInRole("Archive"))
            {
                Audit.AuthorizationSuccess(userName, OperationContext.Current.IncomingMessageHeaders.Action);

                if (Databases.ContainsKey(DatabaseName))
                {
                    if (File.Exists("Archive.txt") == false)
                    {
                        CreateNewTxtFile("Archive.txt");
                    }

                    FileStream streamDb = new FileStream(DatabaseName, FileMode.Open);
                    StreamReader sr = new StreamReader(streamDb);
                    string line;
                    List<modifiedData> datas = new List<modifiedData>();

                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] tokens = line.Split(';');
                        modifiedData data = new modifiedData(int.Parse(tokens[0]), tokens[1], tokens[2], int.Parse(tokens[3]), int.Parse(tokens[4]));
                        datas.Add(data);
                    }

                    sr.Close();
                    streamDb.Close();

                    FileStream streamArch = new FileStream("Archive.txt", FileMode.Open);
                    streamArch.Seek(0, SeekOrigin.End);
                    StreamWriter sw = new StreamWriter(streamArch);

                    foreach (var d in datas)
                    {
                        sw.WriteLine(d);
                    }

                    sw.Close();
                    streamArch.Close();

                    ArchivedDatabases.Add(DatabaseName);
                    DeleteDatabase(DatabaseName);

                    return $"Baza podataka sa imenom '{DatabaseName}' je uspesno arhivirana\n";
                }
                else return $"Baza podataka sa imenom '{DatabaseName}' ne postoji\n";
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(userName,
                       OperationContext.Current.IncomingMessageHeaders.Action, "Archive method need Admin permission.");
                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Message);
                }


                throw new FaultException("User " + userName + " try to call Archive method. Archive method need  Admin permission.");
                
            }
        }
        public String ModifyData(String DatabaseName, int id, string region, string grad, int godina, int potrosnja)
        {
            CustomPrincipal principal = Thread.CurrentPrincipal as CustomPrincipal;
            string userName = Formatter.ParseName(principal.Identity.Name);

            if (Thread.CurrentPrincipal.IsInRole("Modify"))
            {
                Audit.AuthorizationSuccess(userName, OperationContext.Current.IncomingMessageHeaders.Action);

                modifiedData data = new modifiedData(id, region, grad, godina, potrosnja);

                if (Databases.ContainsKey(DatabaseName))
                {
                    FileStream stream = new FileStream(DatabaseName, FileMode.Open);
                    StreamReader sr = new StreamReader(stream);
                    string line;
                    List<modifiedData> datas = new List<modifiedData>();

                    while ((line = sr.ReadLine()) != null)
                    {
                        string[] tokens = line.Split(';');
                        modifiedData d = new modifiedData(int.Parse(tokens[0]), tokens[1], tokens[2], int.Parse(tokens[3]), int.Parse(tokens[4]));
                        datas.Add(d);
                    }

                    stream.Seek(0, SeekOrigin.Begin);
                    StreamWriter sw = new StreamWriter(stream);

                    foreach (var d in datas)
                    {
                        if (d.Id == data.Id)
                        {

                            sw.WriteLine(data);

                        }
                        else
                        {
                            sw.WriteLine(d);
                        }
                    }
                    sw.Close();
                    stream.Close();
                    return "Podaci su uspesno izmenjeni.\n";
                }
                return "Database not exists.\n";
            }
            else
            {
                try
                {
                    Audit.AuthorizationFailed(userName,
                       OperationContext.Current.IncomingMessageHeaders.Action, "Modify method need Writer permission.");
                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Message);
                }


                throw new FaultException("User " + userName + " try to call Modify method. Modify method need  Writer permission.");
                
            }
        }

        public Dictionary<string, Dictionary<int, Data>> ReadData()
        {
            Dictionary<String, Dictionary<int, Data>> data = new Dictionary<string, Dictionary<int, Data>>();
            Dictionary<int, Data> values = new Dictionary<int, Data>();

            foreach(String name in Databases.Keys)
            {
                foreach(int index in Databases[name].Keys)
                {
                    values.Add(index, Databases[name][index]);
                    data.Add(name, values);
                }
            }

            return data;
        }

        public void WriteData(Dictionary<String, Dictionary<int, Data>> data)
        {
            foreach(String name in data.Keys)
            {
                foreach (int index in data[name].Keys)
                {
                    Databases[name].Add(index, data[name][index]);
                    Databases.Add(name, Databases[name]);
                }
            }

            Console.WriteLine("Podaci replicirani.");
        }
    }
}