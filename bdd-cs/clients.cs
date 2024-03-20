namespace bdd_cs

{
    public class Client : List<Client>
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

    public static void GetClients(DataManager dataManager)

        {

            Console.Clear();

            var rdr = dataManager.fetchAll("GetAllClients");

            Console.WriteLine($" {"id",-10} | {"Prénom",-10} | {"Nom",-10} | {"Ville",-30} | {"Addresse",-20}");


            List<Client> clients = new List<Client>();

            while (rdr.Read())
            {
                if(!clients.Exists(x => x.__id == rdr.GetInt32(0)))
                {
                    clients.Add(new Client(rdr.GetInt32(0), rdr.GetString(1), rdr.GetString(2), rdr.GetString(3), rdr.GetString(4)));
                }
                Console.WriteLine($" {rdr.GetInt32(0),-10} | {rdr.GetString(1),-10} | {rdr.GetString(2),-10} | {rdr.GetString(3)} | {rdr.GetString(4)}");

            }

            rdr.Dispose();

            Console.WriteLine("");
            Console.WriteLine("1: Ajouter");
            Console.WriteLine("2: Supprimer");
            Console.WriteLine("3: Retour");
            Console.WriteLine("4: Modifier");
            Console.WriteLine("");
            Console.Write("Veuillez effectuer une action : ");
            string action = Console.ReadLine();

            switch (action)
            {
                case "1":
                    AddClient(dataManager);
                    break;
                case "2":
                    Client.DeleteClient(dataManager);
                    break;
                case "3":
                    Menu.MainMenu(dataManager);
                    break;
                case "4":
                    Client.UpdateClient(dataManager, clients);
                    break;
            }
        }

        public static void DeleteClient(DataManager dataManager)
        {
            Console.WriteLine("");
            Console.Write("Quel Client voulez vous supprimer ? (En vous basant de son ID :");
            string client = Console.ReadLine();

            var rdr = dataManager.deleteItem("clients", "id", client);

            while (rdr.Read())
            {
                Console.WriteLine(rdr);
            }

            rdr.Dispose();
            Client.GetClients(dataManager);
        }

        public static void AddClient(DataManager dataManager)
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



            string[] columns = { "firstname", "lastname", "address", "city" };

            string[] values = { firstname, lastname, address, city };

            var rdr = dataManager.addItem("clients", columns, values);

            rdr.Dispose();

            Client.GetClients(dataManager);
        }

        public static void UpdateClient(DataManager dataManager, List<Client> userData)
        {
            Console.WriteLine("");
            Console.Write("Quel utilisateur voulez vous modifier ? (id)");
            int id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("");


            Client client = userData.Find(user => user.__id == id);

            Console.Write("Prénom (" + client.__firstname + "):");
            string firstname = Console.ReadLine();
            Console.Write("Nom (" + client.__lastname + "):");
            string lastname = Console.ReadLine();
            Console.Write("Adresse (" + client.__address + "):");
            string address = Console.ReadLine();
            Console.Write("Ville (" + client.__city + "):");
            string city = Console.ReadLine();

            string data = $"clients.firstname = '{(firstname == "" ? client.__firstname : firstname)}' " +
                $", clients.lastname = '{(lastname == "" ? client.__lastname : lastname)}' " +
                $", clients.address = '{(address == "" ? client.__address : address)}' " +
                $", clients.city = '{(city == "" ? client.__city : city)}'";

            var rdr = dataManager.updateItem("clients", data, "id", id.ToString());

            rdr.Dispose();

            Client.GetClients(dataManager);
        }
    };
}

