using System;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;

namespace bdd_cs
{
    public class Order : List<Order>
    {
        public int __id = default;
        public int __clientId = default;
        public string __date = "";

        public Order(int id, int clientId, string date)
        {
            __clientId = clientId;
            __id = id;
            __date = date;
        }

        public static void GetOrders(DataManager dataManager)

        {
            Console.Clear();

            Console.WriteLine($" {"order-id",-10} | {"client-id",-10} | {"date",-10} ");

            var rdr = dataManager.fetchAll("GetAllOrders");

            List<Order> orders = new List<Order>();

            while (rdr.Read())
            {
                if (!orders.Exists(x => x.__id == rdr.GetInt32(0)))
                {
                    orders.Add(new Order(rdr.GetInt32(0), rdr.GetInt32(2), rdr.GetString(1)));
                }
                Console.WriteLine($" {rdr.GetInt32(0),-10} | {rdr.GetInt32(2),-10} | {rdr.GetString(1),-20}");
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
                    AddOrder(dataManager);
                    break;
                case "2":
                    DeleteOrder(dataManager);
                    break;
                case "3":
                    Menu.MainMenu(dataManager);
                    break;
                case "4":
                    UpdateOrder(dataManager, orders);
                    break;
            }
        }

        public static void DeleteOrder(DataManager dataManager)
        {
            Console.WriteLine("");
            Console.Write("Quel commande voulez vous supprimer ? (En vous basant de son ID) :");
            string id = Console.ReadLine();

            var rdr = dataManager.deleteItem("Orders", "id", id);

            while (rdr.Read())
            {
                Console.WriteLine(rdr);
            }
            rdr.Dispose();
            Order.GetOrders(dataManager);
        }

        public static void AddOrder(DataManager dataManager)
        {

            Console.Write("date: ");
            string date = Console.ReadLine();
            Console.WriteLine("");

            Console.Write("ClientId: ");
            string clientId = Console.ReadLine();

            string[] values = { "date", "client_id"};

            string[] datas = { date, clientId };

            var rdr = dataManager.addItem("Orders", values, datas);
            rdr.Dispose();

            Order.GetOrders(dataManager);
        }


        public static void UpdateOrder(DataManager dataManager, List<Order> orderData)
        {
            Console.WriteLine("");
            Console.Write("Quel Commande voulez vous modifier ? (id)");
            int id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("");


            Order order = orderData.Find(order => order.__id == id);

            Console.Write("Client (" + order.__clientId + "):");
            string clientId = Console.ReadLine();
            Console.Write("Date (" + order.__date + "):");
            string date = Console.ReadLine();


            string data = $"Orders.client_id = '{(clientId == "" ? order.__clientId : clientId)}' " +
                $", Orders.date = '{(date == "" ? order.__date : date)}' ";
                

            var rdr = dataManager.updateItem("Orders", data, "id", id.ToString());

            rdr.Dispose();

            Order.GetOrders(dataManager);
        }
    };
}

