function Cambia(txek, valor) {
    var cadAct = document.formu.totalpagar.value;
    var valAct = parseFloat(cadAct);
    var valNvo = parseFloat(valor)
    if (txek.checked) {
        var total = valAct + valNvo;
        document.formu.totalpagar.value = total;
    }
    else {
        var total = valAct - valNvo;
        document.formu.totalpagar.value = total;
    }
    obtenerValorTP();
}
function obtenerValorTP() {
    var pagado = parseFloat(document.getElementById("pagado").value)
    var totalPagar = parseFloat(document.formu.totalpagar.value)
    var restante = 0.0;
    if (isNaN(pagado)) {
        //if (e.keyCode == 8) alert('Delete Key Pressed')
        pagado = 0;
    }
    restante = totalPagar - pagado;
    document.getElementById("restante").value = restante;
}
function AplicarRecargo(event) {
    var elementoRecargo = document.getElementById("montoP")
    var addRecargo = parseFloat(elementoRecargo.value);
    if (!isNaN(addRecargo) & addRecargo > 0) {
        var cadAct = document.formu.totalpagar.value;
        var valAct = parseFloat(cadAct);
        var totalPlus = valAct + addRecargo;
        document.formu.totalpagar.value = totalPlus;
        document.formu.recargo.value = addRecargo;
        elementoRecargo.readOnly = true;
        document.getElementById("quitar").disabled = false;
        document.getElementById("aplicar").disabled = true;
    }

    obtenerValorTP();
}
function QuitarRecargo() {
    var elementoRecargo = document.getElementById("montoP")
    var addRecargo = parseFloat(elementoRecargo.value);
    if (!isNaN(addRecargo) & addRecargo > 0) {
        var cadAct = document.formu.totalpagar.value;
        var valAct = parseFloat(cadAct);
        var totalPlus = valAct - addRecargo;
        document.formu.totalpagar.value = totalPlus;
        document.formu.recargo.value = addRecargo;
        document.getElementById("montoP").readOnly = false;
        document.getElementById("quitar").disabled = true;
        document.getElementById("aplicar").disabled = false;
    }
    obtenerValorTP();
}
function AddConcept(btn) {
    btn.value = parseFloat(btn.value) + 1;
}