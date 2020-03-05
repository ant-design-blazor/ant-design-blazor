mkdir -p ./tmp
cd tmp
git clone https://github.com/ant-design/ant-design.git
git clone https://github.com/ElderJames/ant-design-blazor.git
cd ant-design
find ./components/ -name '*.less' -exec cp --parents -av '{}' '../ant-design-blazor/' ';'