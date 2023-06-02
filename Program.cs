using Renci.SshNet;
using System.Text;
using System.Net;
using System.Text.RegularExpressions;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine();
        Console.WriteLine("############################################################################");
        Console.WriteLine();
        Console.WriteLine("               -- ScriptSucker ver. 1.0 by krtfpls@gmail.com --              ");
        Console.WriteLine();
        Console.WriteLine("############################################################################");
        Console.WriteLine("Aplikacja do wgrywania skryptów pythonowych po ssh z uprawnieniami sudo");
        Console.WriteLine("                                                                     sucker :)");
        Console.WriteLine("#############################################################################");
        Console.WriteLine();
        Console.WriteLine();

        if (args.Length < 2)
        {
            Console.WriteLine("Podaj argumenty w formacie: scriptSucker.exe [nazwa_pliku_skryptu.py] [adres_ip1] [adres_ip2] [adres_ip3] ...");
            return;
        }

        string scriptFileName = args[0];


        List<string> ipAddresses = new List<string>(args.Skip(1));

        if (!ValidateIPAddresses(ipAddresses))
        {
            Console.WriteLine("Nieprawidłowy format adresów IP.");
            return;
        }

        string scriptPath = Path.Combine(Directory.GetCurrentDirectory(), scriptFileName);
        if (!File.Exists(scriptPath))
        {
            Console.WriteLine("Plik skryptu nie istnieje.");
            return;
        }

        Console.WriteLine("Podaj nazwę użytkownika (login):");
        string username = Console.ReadLine();

        Console.WriteLine("Podaj hasło do połączenia SSH:");
        string password = ReadPassword();

        foreach (string ipAddress in ipAddresses)
        {
            using (var client =   new SshClient(ipAddress, username, password))
            {
                try
                {
                    client.Connect();

                    if (client.IsConnected)
                    {

                        string remoteScriptFileName = Path.GetFileName(scriptPath);

                        using (var scp = new ScpClient(ipAddress, username, password))
                        {
                            scp.Connect();
                            scp.Upload(new FileInfo(scriptPath), remoteScriptFileName);
                            scp.Disconnect();
                        }

                       var command = client.CreateCommand("echo \"" + password + $"\" | sudo -S python3 {remoteScriptFileName}");
                        string output = command.Execute();
                        client.RunCommand($"rm {remoteScriptFileName}");
                        Console.WriteLine();
                        Console.WriteLine("=============================================================================");
                        Console.WriteLine($"Wynik skryptu na hoscie {ipAddress}:\n{output}");
                        Console.WriteLine("=============================================================================");
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine($"Nie można połączyć się z adresem IP: {ipAddress}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Błąd podczas nawiązywania połączenia SSH z adresem IP: {ipAddress}");
                    Console.WriteLine($"Wiadomość błędu: {ex.Message}");
                }
                finally
                {
                    client.Disconnect();
                }
            }
        }

        Console.WriteLine("Zakończono wykonanie skryptów na wszystkich hostach.");
    }

    static bool ValidateIPAddresses(List<string> ipAddresses)
    {
        foreach (string ipAddress in ipAddresses)
        {
            if (!IPAddress.TryParse(ipAddress, out IPAddress parsedAddress))
            {
                return false;
            }
        }

        return true;
    }

    static string ReadPassword()
    {
        StringBuilder password = new StringBuilder();
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(intercept: true);

            if (key.Key != ConsoleKey.Enter)
            {
                password.Append(key.KeyChar);
            }
        }
        while (key.Key != ConsoleKey.Enter);

        return password.ToString();
    }
}
