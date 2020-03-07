#!/bin/bash

git clone https://github.com/ant-design/ant-design.git
git clone https://github.com/ElderJames/ant-design-blazor.git

cd ant-design

LAST_VERSION=$(git describe --abbrev=0 --tags | sed 's/* //'  )
echo "Lastest Version of ant-design: ${LAST_VERSION}"

git checkout ${LAST_VERSION}

find ./components/ -name '*.less' -exec cp --parents -a '{}' '../ant-design-blazor/' ';'

cd ../ant-design-blazor

TOTAL_MODIFIED=$(git status -s | wc -l)

if [ "$TOTAL_MODIFIED" -eq "0" ]; then 
    echo "nothing modified" 
    exit 0
fi

echo "modified: ${TOTAL_MODIFIED}"

git config --global user.name 'ElderJames'
git config --global user.email 'shunjiey@hotmail.com'

git add -A
git commit -m "chore: sync ant-design v${LAST_VERSION}"
git push https://github.com/ElderJames/ant-design-blazor.git master:sync-style/${LAST_VERSION}
