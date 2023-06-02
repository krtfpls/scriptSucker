import subprocess
import re
dns_servers = ["194.204.159.1", "8.8.4.4"]

print()
print("############################################################")
print("--== Ubuntu Desktop DNS changer ver. 1.0  by krtfpls@gmail.com ==--")
print("############################################################")
print()

def change_dns(connection_name, dns_servers):
    dns_servers_str = ",".join(dns_servers)
    command = f"nmcli connection modify '{connection_name}' ipv4.dns '{dns_servers_str}'"
    subprocess.run(command, shell=True)

    # Restart the network manager service for the changes to take effect
    subprocess.run("systemctl restart NetworkManager", shell=True)

def get_dns_servers(connection_name):
    command = f"nmcli connection show '{connection_name}' | grep -ie dns"
    process = subprocess.Popen(command, shell=True, stdout=subprocess.PIPE)
    output, _ = process.communicate()
    output = output.decode("utf-8")
    
    dns_get = re.findall(r'\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b', output)
    
    return dns_get

def get_internet_connection():
    command = "nmcli -t -f NAME,TYPE c show --active"
    process = subprocess.Popen(command.split(), stdout=subprocess.PIPE)
    output, _ = process.communicate()
    output = output.decode("utf-8")
    lines = output.split("\n")

    for line in lines:
        parts = line.split(":")
        if len(parts) >= 2:
            connection_name = parts[0]
            connection_type = parts[1]
            if connection_type == "802-3-ethernet":
                return connection_name

    return None

internet_connection = get_internet_connection()

if internet_connection is not None:
    print("Główne połączenie z dostępem do internetu:")
    print(internet_connection)

    print("Zmiana adresów DNS...")
    change_dns(internet_connection, dns_servers)
    print("Zmieniono adresy DNS na:", dns_servers)

    print("Sprawdzanie ustawionych adresów DNS...")
    current_dns_servers = get_dns_servers(internet_connection)

    if current_dns_servers == dns_servers:
        print("Adresy DNS są zgodne lub zmieniono poprawnie:")
        for dns in current_dns_servers:
            print(dns)
    else:
        print("Ustawione adresy DNS są różne od oczekiwanych:")
        for dns in current_dns_servers:
            print(dns)

else:
    print("Nie znaleziono głównego połączenia z dostępem do internetu.")

