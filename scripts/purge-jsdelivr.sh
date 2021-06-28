#! /bin/bash

ignoreDir=(
less
)


read_dir(){
    for file in `ls $1`       
    do
        if [ -d $1"/"$file ]; then
            if [[ ! "${ignoreDir[@]}"  =~ "${file}" ]]; then
                read_dir $1"/"$file $2
            fi
        else
            filePath=$1"/"$file
            filePath=${filePath/$2\//}  
            echo ''
            echo 'purging '${filePath}'...'
            curl -s https://purge.jsdelivr.net/gh/$3/$filePath
        fi
    done
}

read_dir $1 $1 $2