
var fNumber = {
    sepMil: ".", // separador para los miles
    sepDec: ',', // separador para los decimales
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

function control_session(Ruta) {
    swal({
        title: "No se encontraron los datos de autenticación en la sesión!",
        text: "ingrese nuevamente!",
        icon: "warning",                
    })
    .then((willDelete) => {
        window.location.assign(Ruta);
    });
}