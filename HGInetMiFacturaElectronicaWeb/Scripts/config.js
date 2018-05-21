

//Controla el formato de numeros Ejemplo: $ 1.222.333,90
var fNumber = {
    sepMil: ",", // separador para los miles
    sepDec: '.', // separador para los decimales
    simbol: "$" || '', //Simbolo de modena
    formatear: function (num) {
        num += '';
        var splitStr = num.split('.');
        var splitLeft = splitStr[0];
        var splitRight = splitStr.length > 1 ? this.sepDec + splitStr[1] : '';
        var regx = /(\d+)(\d{3})/;
        while (regx.test(splitLeft)) {
            splitLeft = splitLeft.replace(regx, '$1' + this.sepMil + '$2');
        }
        return this.simbol + splitLeft + splitRight;
    },
    go: function (num) {
        return this.formatear(num);
    }
}

//Controla los mensajes de Session
function control_session(Ruta) {
    
    swal({
        title: "Tu sesión ha expirado",
        icon: "warning"        
    })
    .then((willDelete) => {
        window.location.assign(Ruta);
    });
}

//Formato de fecha para los grid.
function convertDateFormat(string) {
    string = string.substr(0, 10);
    var info = string.split('/');
    var part1 = info[1].length;
    var part0 = info[0].length;
    if (part1 == 1) { info[1] = '0' + info[1] }
    if (part0 == 1) { info[0] = '0' + info[0] }
    return info[2] + '/' + info[0] + '/' + info[1];
}

