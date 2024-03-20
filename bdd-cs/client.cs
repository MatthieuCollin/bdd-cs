using System;
using MySql.Data.MySqlClient;

using bdd_cs;

namespace bdd
{
    public class Client
    {
        public int __id = default;
        public string __firstname = "";
        public string __lastname = "";
        public string __address = "";
        public string __city = "";

        public Client(int id, string firstname, string lastname, string address, string city)
        {
            __id = id;
            __firstname = firstname;
            __lastname = lastname;
            __address = address;
            __city = city;
        }

        public static void GetClients()

        {
            Console.Clear();
            string cs = @"server=localhost;userid=root;password=;database=CSI";

            using var con = new MySqlConnection(cs);
            con.Open();

            var query = "SELECT * FROM clients";

            using var cmd = new MySqlCommand(query, con);

            using MySqlDataReader rdr = cmd.ExecuteReader();
            Console.WriteLine($" {"id",-10} | {"Prénom",-10} | {"Nom",-10} | {"Ville",-30} | {"Addresse",-20}");



            while (rdr.Read())
            {
                Client obj = new Client(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4));
                Console.WriteLine($" {obj.__id,-10} | {obj.__firstname,-10} | {obj.__lastname,-10} | {obj.__address,-30} | {obj.__city,-20}");
            }

            Console.WriteLine("");
            Console.WriteLine("1: Ajouter");
            Console.WriteLine("2: Supprimer");
            Console.WriteLine("3: Retour");
            Console.WriteLine("");
            Console.Write("Veuillez effectuer une action : ");
            string action = Console.ReadLine();

            switch (action)
            {
                case "1":
                    AddClient();
                    break;
                case "2":
                    Client.DeleteClient();
                    break;
                case "3":
                    Menu.MainMenu();
                    break;
            }
        }

        public static void DeleteClient()
        {
            Console.WriteLine("");
            Console.Write("Quel Client voulez vous supprimer ? (En vous basant de son ID :");
            int client = Convert.ToInt32(Console.ReadLine());

            string cs = @"server=localhost;userid=root;password=;database=CSI";

            using var con = new MySqlConnection(cs);
            con.Open();

            var query = $"DELETE  FROM clients WHERE clients.id = {client}";

            using var cmd = new MySqlCommand(query, con);

            using MySqlDataReader rdr = cmd.ExecuteReader();

            while (rdr.Read())
            {
                Console.WriteLine(rdr);
            }
            Client.GetClients();
        }

        public static void AddClient()
        {
            Console.WriteLine("");
            Console.Write("Prénom: ");
            string firstname = Console.ReadLine();
            Console.WriteLine("");

            Console.Write("Nom: ");
            string lastname = Console.ReadLine();
            Console.WriteLine("");

            Console.Write("Adresse: ");
            string address = Console.ReadLine();
            Console.WriteLine("");

            Console.Write("Ville: ");
            string city = Console.ReadLine();

            string cs = @"server=localhost;userid=root;password=;database=CSI";

            using var con = new MySqlConnection(cs);
            con.Open();

            var query = $"INSERT INTO clients(firstname, lastname, address, city) VALUES ('{firstname}', '{lastname}', '{address}', '{city}')";

            using var cmd = new MySqlCommand(query, con);

            using MySqlDataReader rdr = cmd.ExecuteReader();

            Client.GetClients();
        }
    };
}

