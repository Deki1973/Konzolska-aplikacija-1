using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace ConsoleApp1
{
    class Zapis
    {
        public string strConnString= @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=NewDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public string strKomanda;
        public SqlConnection sqlConn;
    }

    class Program

    {

        static void Main(string[] args)
        {
            bool blnPonovo = true;

        do
        {
            string strMeni = "Izaberite opciju:" + "\n";
            strMeni += "1 - Izlistaj ceo adresar" + "\n";
            strMeni += "2 - Pronadji zapis" + "\n";
            strMeni += "3 - Unesi nov zapis" + "\n";
            strMeni += "4 - Izmeni zapis za kontakt" + "\n";
            strMeni += "5 - Obrisi zapis" + "\n";
            Console.WriteLine(strMeni);

                string strIzbor=Console.ReadLine();
                switch (strIzbor)
                {
                    case "1":
                        IzlistajZapise(); break;
                    case "2":
                        TrazenjeZapisa();break;
                    case "3":
                        UnosNovogZapisa(); break;
                    case "4":
                        IzmenaZapisa(); break;
                    case "5":
                        BrisanjeZapisa(); break;
                    default:
                        Console.Write("UNELI STE NEVAZECU OPCIJU. PROBAJTE PONOVO.\n");break;
                }
            
                Console.Write("POKRENUTI PROGRAM PONOVO? (d/n)");
                strIzbor = Console.ReadLine();
                if (strIzbor.ToUpper() != "D") { blnPonovo = false; Console.WriteLine("KRAJ PROGRAMA"); }
                else { blnPonovo = true; }

            } while (blnPonovo == true);
        }

        static void UnosNovogZapisa()
        {
            Zapis zap1 = new Zapis();
            zap1.strKomanda = "INSERT INTO [dbo].[tabImenik] (chrIme,chrTelefon,chrAdresa) VALUES (@parIme,@parTelefon,@parAdresa)";
            string strIme;
            string strTelefon;
            string strAdresa;
            Console.Write("NOV ZAPIS:\n");
            strIme = Console.ReadLine().Trim();
            //Console.Write(strIme);
            Console.Write("Unesite telefon za " + strIme + ": ");
            strTelefon = Console.ReadLine().Trim();
            Console.Write("Unesite adresu za " + strIme + ": ");
            strAdresa = Console.ReadLine().Trim();

            try
            {
                SqlConnection con1 = new SqlConnection(zap1.strConnString);
                SqlCommand cmd1 = new SqlCommand(zap1.strKomanda, con1);
                cmd1.Parameters.AddWithValue("@parIme", strIme);
                cmd1.Parameters.AddWithValue("@parTelefon", strTelefon);
                cmd1.Parameters.AddWithValue("@parAdresa", strAdresa);
                con1.Open();
                cmd1.ExecuteNonQuery();
                con1.Close();
            }

            catch (Exception ex)
            {
                string strPoruka = "Ups! Nesto je poslo kako ne treba!\n";
                strPoruka += ex.Message;
                Console.Write(strPoruka + "\n");
            }
        }

        static void IzmenaZapisa()
        {
            string strPoruka = "Unesite ime kontakta cije podatke hocete da izmenite: ";
            Console.Write(strPoruka);
            string strIme = Console.ReadLine().ToString().Trim();
            Console.Write("Unesite nov telefon za " + strIme + ": ");
            string strNoviTelefon = Console.ReadLine().ToString().Trim();
            Console.Write("Unesite novu adresu za " + strIme + ": ");
            string strNovaAdresa = Console.ReadLine().ToString().Trim();
            Zapis zap1 = new Zapis();
            zap1.sqlConn = new SqlConnection(zap1.strConnString);
            zap1.strKomanda = "UPDATE [dbo].[tabImenik] SET chrTelefon=@IzmenjenTelefon,chrAdresa=@IzmenjenaAdresa WHERE chrIme='" + strIme + "'";
            SqlCommand cmd1 = new SqlCommand(zap1.strKomanda, zap1.sqlConn);
            cmd1.Parameters.AddWithValue("@IzmenjenTelefon", strNoviTelefon);
            cmd1.Parameters.AddWithValue("@IzmenjenaAdresa", strNovaAdresa);
            zap1.sqlConn.Open();
            int intIzmenjenoSlogova = cmd1.ExecuteNonQuery();
            if (intIzmenjenoSlogova == 1) { Console.Write("Telefon i adresa za kontakt " + strIme + " uspesno izmenjeni."); }
            else { Console.Write("U tabeli ne postoji zapis za kontakt " + strIme + ". \nProverite podatke i probajte ponovo.\nPritisnite bilo koje dugme za povratak u glavni meni.\n"); }
            zap1.sqlConn.Close();
            //Console.Write(cmd1.CommandText.ToString() + "\n"); - Ovu sintaksu sam koristio tokom pisanja za kontrolu uspesnosti izvrsenja koda.
            Console.ReadLine();
        }

        static void IzlistajZapise()
        {

            string strConnString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=NewDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            string strQuery = "SELECT * FROM tabImenik ORDER BY chrIme ASC";
            Console.Write("ADRESAR - Pritisnite bilo koje dugme za nastavak." + "\n");
            string strIzbor = Console.ReadLine();
            SqlConnection conn1 = new SqlConnection(strConnString);
            SqlCommand cmd1 = new SqlCommand(strQuery, conn1);
            conn1.Open();
            SqlDataReader rdr1 = cmd1.ExecuteReader();

            while (rdr1.Read())
            {
                Console.WriteLine(rdr1[0].ToString() + ", " + rdr1[1] + "," + rdr1[2] + ";");
            }
            conn1.Close();
        }

        static void BrisanjeZapisa()
        {
            Console.Write("Unesite ime koje zelite da obrisete iz adresara:");
            string strIme = Console.ReadLine().Trim();
            Console.Write("DA LI STE SIGURNI DA ZELITE DA OBRISETE IZ ADRESARA KONTAKT " + strIme + "? (D/N)");
            string strOdgovor = Console.ReadLine().Trim().ToUpper();
            if (strOdgovor == "D")
            {
                Zapis zapBrisanje = new Zapis();
                SqlConnection conn1 = new SqlConnection(zapBrisanje.strConnString);
                zapBrisanje.strKomanda = "DELETE FROM [dbo].[tabImenik] WHERE chrIme='" + strIme + "'";
                SqlCommand cmdBrisanje = new SqlCommand(zapBrisanje.strKomanda, conn1);
                conn1.Open();
                int intObrisanoZapisa = cmdBrisanje.ExecuteNonQuery();
                conn1.Close();
                if (intObrisanoZapisa != 0)
                { Console.Write("Iz tabele je uspesno obrisano " + intObrisanoZapisa.ToString() + " zapisa.\n"); }
                else
                { Console.Write("U tabeli ne postoji zapis sa imenom " + strIme + ".\n"); }
            }
        }

        static void TrazenjeZapisa()
        {
            string strPoruka = "Izaberite parametar po koje hocete da uradite pretragu:\n";
            strPoruka += "1 - Ime\n";
            strPoruka += "2 - Telefonski broj\n";
            strPoruka += "3 - Adresa\n";
            Console.Write(strPoruka);
            string strIzbor = Console.ReadLine();
            string strKolona = "";

            if (strIzbor == "1") { strKolona = "chrIme"; }
            if (strIzbor == "2") { strKolona = "chrTelefon"; }
            if (strIzbor == "3") { strKolona = "chrAdresa"; }

            strPoruka = "Unesite vrednost parametra: ";
            Console.Write(strKolona);
            string strVrednostParametra = Console.ReadLine().Trim();
            string strKomanda = "SELECT * FROM [dbo].[tabImenik] WHERE " + strKolona + "='" + strVrednostParametra + "'";

            SqlConnection conn1 = new SqlConnection(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=NewDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            SqlCommand cmdTrazi = new SqlCommand(strKomanda, conn1);
            Console.Write("\n" + cmdTrazi.CommandText + "\n");

            SqlDataAdapter sqlAdap1 = new SqlDataAdapter(strKomanda, conn1);
            conn1.Open();
            SqlDataReader rdr1 = cmdTrazi.ExecuteReader();
            while (rdr1.Read())
            {
                Console.WriteLine(rdr1[0].ToString().TrimEnd() + ", " + rdr1[1].ToString().TrimEnd() + "," + rdr1[2] + ";");
            }
            conn1.Close();
        }
    }
}

