curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --version 9.0.0
dotnet workload install wasm-tools
npm install
dotnet build

dotnet publish ./site/AntDesign.Docs.Wasm -c Release -o _site
cp -rf ./site/AntDesign.Docs.Wasm/bin/Debug/net10.0/*.dll _site/wwwroot/_framework
find ./_site/wwwroot -type f -name '*.html' -exec bash -c 'if [ $(grep -o "<title>" "$1" | wc -l) -gt 1 ]; then perl -0777 -i -pe "s/<title>.*?<\/title>//s" "$1"; fi' _ {} \;

