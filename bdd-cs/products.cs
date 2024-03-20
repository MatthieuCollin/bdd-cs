using System;
using MySqlX.XDevAPI;

namespace bdd_cs
{
    public class Product : List<Product>
    {
        public int __price = default;
        public int __stock = default;
        public string __preference = "";
        public string __designation = "";

        public Product(string preference, string designation, int stock, int price)
        {
            __designation = designation;
            __preference = preference;
            __stock = stock;
            __price = price;
        }

        public static void GetProducts(DataManager dataManager)

        {
            Console.Clear();


            Console.WriteLine($" {"réference",-10} | {"Désignation",-30} | {"Stock",-10} | {"Prix",-10}");

            var rdr = dataManager.fetchAll("GetAllProducts");

            List<Product> products = new List<Product>();

            while (rdr.Read())
            {
                if (!products.Exists(x => x.__preference == rdr.GetString(0)))
                {
                    products.Add(new Product(rdr.GetString(0), rdr.GetString(1), rdr.GetInt32(2), rdr.GetInt32(3)));
                }
                Console.WriteLine($" {rdr.GetString(0),-10} | {rdr.GetString(1),-30} | {rdr.GetInt32(2),-10} | {rdr.GetInt32(3),-30}");
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
                    AddProduct(dataManager);
                    break;
                case "2":
                    DeleteProduct(dataManager);
                    break;
                case "3":
                    Menu.MainMenu(dataManager);
                    break;
                case "4":
                    UpdateProduct(dataManager, products);
                    break;  
            }
        }

        public static void DeleteProduct(DataManager dataManager)
        {
            Console.WriteLine("");
            Console.Write("Quel produit voulez vous supprimer ? (En vous basant de sa préference) :");
            string product = Console.ReadLine();

            var rdr = dataManager.deleteItem("products", "reference", product);

            while (rdr.Read())
            {
                Console.WriteLine(rdr);
            }

            rdr.Dispose();

            GetProducts(dataManager);
        }

        public static void AddProduct(DataManager dataManager)
        {
            Console.WriteLine("");
            Console.Write("référence: ");
            string preference = Console.ReadLine();
            Console.WriteLine("");

            Console.Write("Désignation: ");
            string designation = Console.ReadLine();
            Console.WriteLine("");

            Console.Write("stock: ");
            string quantity = Console.ReadLine();
            Console.WriteLine("");

            Console.Write("prix: ");
            string price = Console.ReadLine();

            string[] values = { "reference", "designation", "price", "stock" };

            string[] datas = { preference, designation, price, quantity };

            var rdr = dataManager.addItem("products", values, datas);

            rdr.Dispose();

            Menu.MainMenu(dataManager);
        }

        public static void UpdateProduct(DataManager dataManager, List<Product> productData)
        {
            Console.WriteLine("");
            Console.Write("Quel Produit voulez vous modifier ? (référence)");
            string id = Console.ReadLine();
            Console.WriteLine("");


            Product product = productData.Find(product => product.__preference == id);

            Console.Write("Désignation (" + product.__designation + "):");
            string designation = Console.ReadLine();
            Console.Write("Référence (" + product.__preference + "):");
            string reference = Console.ReadLine();
            Console.Write("Stock (" + product.__stock + "):");
            string stock = Console.ReadLine();
            Console.Write("Prix (" + product.__price + "):");
            string price = Console.ReadLine();

            string data = $"products.designation = '{(designation == "" ? product.__designation : designation)}' " +
                $", products.reference = '{(reference == "" ? product.__preference: reference)}' " +
                $", products.price = '{(price == "" ? product.__price : price)}' " +
                $", products.stock = '{(stock == "" ? product.__stock : stock   )}'";

            var rdr = dataManager.updateItem("products", data, "reference", id);

            rdr.Dispose();

            Product.GetProducts(dataManager);
        }
    };
}