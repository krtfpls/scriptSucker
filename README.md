HOW TO RUN
===========

Open cmd on Windows OS, go to the directory with scriptSucker, then type:

scriptSucker.exe [python_script_name.py] [ip_adress1] [ip_adress2] [ip_adress3] ...");

application retrieve the username and password for ssh connection,
then executes any specified python script with sudo privileges on remote hosts.

For example, there is the dnsChanger.py script for change DNS on Ubuntu Desktop Hosts using Network Manager CLI

example use:

  scriptSucker.exe dnsChanger.py 192.168.1.11 192.168.1.12 192.168.2.15
</br></br></br>
