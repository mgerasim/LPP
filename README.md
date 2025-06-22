dotnet publish LPP.csproj -c Release --runtime linux-x64 --self-contained

tar -czf bin/LPP.tar.gz -C bin/Release/net8.0/linux-x64/publish/ .

scp bin/LPP.tar.gz root@flexess.ru:/home/root/apps/LPP/


# на сервере
cd /home/root/apps/LPP
tar -xzvf /home/root/apps/LPP/LPP.tar.gz

# инстал€ци€

sudo chmod u+x /home/root/apps/LPP/LPP
sudo systemctl stop LPP.service
sudo cp /home/root/apps/LPP/LPP.service /etc/systemd/system/
sudo systemctl daemon-reload
sudo systemctl enable LPP.service
sudo systemctl start LPP.service
sudo systemctl status LPP.service