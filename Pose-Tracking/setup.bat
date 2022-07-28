@echo off
cd /d %~dp0
echo Start setup.
pip install -r requirements.txt

cd src
git clone https://github.com/Massu0921/depthai_blazepose
cd depthai_blazepose
pip install -r requirements.txt

echo Setup done.
