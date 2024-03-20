using System;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;

namespace bdd_cs
{
    public class Lines : List<Lines>
    {
        public int __orderId = default;
        public string __productReference = "";
        public int __quantity = default;

        public Lines(int orderId, string productReference, int quantity)
        {
            __orderId = orderId;
            __productReference = productReference;
            __quantity = quantity;
        }

        public static void GetLines(DataManager dataManager)

        {
            Console.Clear();


            var rdr = dataManager.fetchAll("GetAllLinesOrders");

            Console.WriteLine($" {"orderId",-10} | {"référence produit",-10} | {"quantity",-10}");

            List<Lines> lines = new List<Lines>();

            while (rdr.Read())
            {
                if (!lines.Exists(x => x.__orderId == rdr.GetInt32(0) && x.__productReference == rdr.GetString(1)))
                {
                    lines.Add(new Lines(rdr.GetInt32(0), rdr.GetString(1), rdr.GetInt32(2)));
                }
                Console.WriteLine($" {rdr.GetInt32(0),-10} | {rdr.GetString(1),-10} | {rdr.GetInt32(2),-20}");
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
                    AddLines(dataManager);
                    break;
                case "2":
                    DeleteLines(dataManager);
                    break;
                case "3":
                    Menu.MainMenu(dataManager);
                    break;
                case "4":
                    Lines.UpdateLines(dataManager, lines);
                    break;
            }
        }

        public static void DeleteLines(DataManager dataManager)
        {
            Console.WriteLine("");
            Console.Write("Quel lines voulez vous supprimer ? (En vous basant de son id) :");
            string id = Console.ReadLine();
            Console.WriteLine("");
            Console.Write("Veuillez indiquer la réference du produit :");
            string reference = Console.ReadLine();

            var rdr = dataManager.deleteItemPrecisely("linesProduct", "order_id", id, "product_reference", reference);

            while (rdr.Read())
            {
                Console.WriteLine(rdr);
            }
            rdr.Dispose();
            Order.GetOrders(dataManager);
        }

        public static void AddLines(DataManager dataManager)
        {

            Console.Write("reference produit: ");
            string reference = Console.ReadLine();
            Console.WriteLine("");

            Console.Write("orderId: ");
            string clientId = Console.ReadLine();

            var query = $"INSERT INTO linesProduct(order_id, reference_produit) VALUES ('{clientId}', '{reference}')";

            string[] columns = { "order_id", "product_reference" };
            string[] values = { clientId, reference };
            
            var rdr = dataManager.addItem("linesProduct", columns, values);
            rdr.Dispose();

            Order.GetOrders(dataManager);
        }

        public static void UpdateLines(DataManager dataManager, List<Lines> linesData)
        {
            Console.WriteLine("");
            Console.Write("Quel Lines voulez vous modifier ? (OrderId)");
            int id = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("");
            Console.Write("Quel Lines voulez vous modifier ? (product_reference)");
            string reference = Console.ReadLine();
            Console.WriteLine("");


            Lines line = linesData.Find(line => line.__orderId == id && line.__productReference == reference);

            Console.Write("OrderId (" + line.__orderId + "):");
            string orderId = Console.ReadLine();
            Console.Write("Référence produit (" + line.__productReference + "):");
            string referenceI = Console.ReadLine();
            Console.Write("Quantité (" + line.__quantity + "):");
            string stock = Console.ReadLine();


            string data = $"linesProduct.order_id = '{(orderId == "" ? line.__orderId : orderId)}' " +
                $", linesProduct.product_reference = '{(reference == "" ? line.__productReference : reference)}' " +
                $", linesProduct.quantity = '{(stock == "" ? line.__quantity : stock)}' ";
                

            var rdr = dataManager.updateItem("linesProduct", data, $"order_id = {id.ToString()} AND linesProduct.product_reference ", reference);

            rdr.Dispose();

            Lines.GetLines(dataManager);
        }
    };
}