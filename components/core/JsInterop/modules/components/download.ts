function downloadFile(url,fileName) { 
        const anchorElement = document.createElement('a'); 
       anchorElement.href = url; 
     
        if (fileName) { 
            anchorElement.download = fileName; 
        } 
     
        anchorElement.click(); 
       anchorElement.remove(); 
    };
