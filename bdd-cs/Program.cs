namespace bdd_cs;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI.Relational;

class Program
{
    static void Main(string[] args)
    {
        Menu.Login();
    }
}



public class DataManager
{
    public MySqlConnection con = null!;

    public string values = "";
    public string data = "";

    public void Login(string username, string password)
    {
        string cs = $"server=localhost;userid={username};password={password};database=CSI";

        con = new MySqlConnection(cs);

        try
        {
            con.Open();
            Console.WriteLine("Vous êtes Connecté");
        }
        catch
        {
            Console.WriteLine("Identifiant Incorrect");
            Menu.Login();
        }
    }

    public void Logout()
    {
        try
        {
            con.Close();
            Console.WriteLine("Vous êtes déconnecté");
            Console.WriteLine("");
            Console.WriteLine("Appuyez sur entrée pour quitter le logiciel");

            Console.ReadLine();

        }
        catch
        {
            Console.WriteLine("Erreur lors de la déconnection");
            Menu.Login();
        }
    }

    public MySqlDataReader fetchAll(string table)
    {
        string query = $"CALL `{table}`(@p0);";
        using var cmd = new MySqlCommand(query, con);

        return cmd.ExecuteReader();
    }

    public MySqlDataReader deleteItem(string table, string column, string id)
    {
        var query = $"DELETE FROM {table} WHERE {table}.{column} = {id}";

        try
        {
            using var cmd = new MySqlCommand(query, con);
            return cmd.ExecuteReader();

        }
        catch
        {
            using var cmd = new MySqlCommand($"SELECT * FROM {table}", con);
            Console.WriteLine("une erreur est survenue, veuillez réssayer");
            Console.WriteLine("Appuyez sur entrée pour continuer");

            Console.ReadLine();
            return cmd.ExecuteReader();

        }


    }


    public MySqlDataReader deleteItemPrecisely(string table, string column1, string id1, string column2, string id2)
    {
        var query = $"DELETE FROM {table} WHERE {table}.{column1} = {id1} AND {table}.{column2} = {id2}";

        using var cmd = new MySqlCommand(query, con);


        return cmd.ExecuteReader();
    }

    public MySqlDataReader addItem(string table, string[] columns, string[] datas)
    {
        for (int i = 0 ; i < columns.Length; i++)
        {
            if (i == columns.Length -1)
            {
                values += columns[i];

            }
            else
            {
                values += columns[i] + ",";
            }
        }

        for (int i = 0; i < datas.Length; i++)
        {
            if (i == datas.Length -1)
            {
                data += $"'{datas[i]}'";

            }
            else
            {
                data += $"'{datas[i]}',";
            }
        }

        var query = $"INSERT INTO {table}({values}) VALUES ({data});";
        data = "";
        values = "";

        try
        {
            using var cmd = new MySqlCommand(query, con);
            return cmd.ExecuteReader();
        }
        catch(MySqlException em) 
        {
            using var cmd = new MySqlCommand($"SELECT * FROM {table}",con);
            Console.WriteLine("une erreur est survenue, veuillez réssayer");
            Console.WriteLine("Appuyez sur entrée pour continuer");

            Console.ReadLine();
            return cmd.ExecuteReader();
        }
    }

    public MySqlDataReader updateItem(string table, string datas, string column, string id)
    {
       
        var query = $"UPDATE {table} SET {datas} WHERE {table}.{column} = '{id}'";

        using var cmd = new MySqlCommand(query, con);


       return cmd.ExecuteReader();

    }
}

public class Menu
{

    public static void Login()
    {
        Console.Write("Nom d'utilisateur : ");
        string username = Console.ReadLine();

        Console.Write("Mot de passe : ");
        string password = Console.ReadLine();

        DataManager dataManager = new DataManager();

        dataManager.Login(username, password);

        MainMenu(dataManager);
    }

    public static void MainMenu(DataManager dataManager)
    {
        Console.Clear();

        Console.WriteLine("Bienvenue sur l'application de gestion de la boutique de demo");
        Console.WriteLine("");
        Console.WriteLine("1: Voir la liste de vos clients");
        Console.WriteLine("2: Voir la liste des produits disponibles");
        Console.WriteLine("3: Voir la liste des commandes");
        Console.WriteLine("4: Voir la liste des lines");
        Console.WriteLine("");
        Console.WriteLine("5: Se déconnecter");
        Console.WriteLine("");
        Console.Write("Veuillez effectuer une action : ");
        string action = Console.ReadLine();

        Console.WriteLine(action);
        switch (action)
        {
            case "1":
                Client.GetClients(dataManager);
                break;
            case "2":
                Product.GetProducts(dataManager);
                break;
            case "3":
                Order.GetOrders(dataManager);
                break;
            case "4":
                Lines.GetLines(dataManager);
                break;
            case "5":
                dataManager.Logout();
                break;
        }
    }
}