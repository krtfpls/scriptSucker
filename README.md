HOW TO RUN
===========

Open cmd on Windows OS, go to the directory with scriptSucker, then type:

scriptSucker.exe [python_script_name.py] [ip_adress1] [ip_adress2] [ip_adress3] ...");

the application ask you for the username and password for ssh connection,
then executes any specified python script with sudo privileges (if the user has sudo privileges) on remote hosts.
For exampple, there is the dnsChanger.py script for change DNS on Ubuntu Desktop Hosts using Network Manager CLI

ex.:
  scriptSucker.exe dnsChanger.py 192.168.1.11 192.168.1.12 192.168.2.15

