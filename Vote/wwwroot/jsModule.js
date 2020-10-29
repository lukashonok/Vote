window.functions = {
    returnMarksAsync: () => {
        DotNet.invokeMethodAsync('Vote', 'ReturnMarksAsync')
            .then(data => {
                console.log(data);
            });
    }
}