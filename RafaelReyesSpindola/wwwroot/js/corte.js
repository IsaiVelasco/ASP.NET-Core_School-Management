
function obtenerValor() {
    var pagado = parseFloat(document.getElementById("pagado").value)
    var totalPagar = parseFloat(document.formu2.totalpagar.value)
    var restante = 0.0;
    if (isNaN(pagado)) {
        //if (e.keyCode == 8) alert('Delete Key Pressed')
        pagado = 0;
    }
    restante = pagado - totalPagar;
    document.getElementById("restante").value = restante;
    if (restante < 0) {
        document.getElementById("restante").style.color = 'red';
    } else if (restante > 0) {
        document.getElementById("restante").style.color = '#F38C1B';
    } else {
        document.getElementById("restante").style.color = '#28A745';
    }
}
