function mostrarPanelExposicion() {
    if ($('.MORAVAL').is(":checked") && !$('.HMA').is(":checked") && !$('.ECA').is(":checked")) {
        $('#panelExposicion').show();
    } else {
        $("#EsDeExposicion").val("false").change();
        mostrarFichaExposicion();
        $('#panelExposicion').hide();
    }
}

function noAdmiteFacturacionElectronica() {
    if ($('#NoAdmiteFacturacionElectronica').is(":checked")) {
        $('#DirEnvioFacturas').show();
        $('#MailDeFacturacion').val("");
        $('#lblMailFacturacion').hide();
        $('#MailDeFacturacion').hide();
    } else {
        $('#DirEnvioFactura').val("");
        $('#DirEnvioFacturas').hide();
        $('#lblMailFacturacion').show();
        $('#MailDeFacturacion').show();
    }
}

function IBANObligatorioSN() {
    if ($('#ExistePedidoEnFirme').val() == "true") {
        $('#lblIBAN').addClass("required");
    } else {
        $('#lblIBAN').removeClass("required");
    }
}

function mostrarAgentes() {
    if ($('.MORAVAL').is(":checked")) {
        $('#lblAgenteMV').show();
        $('#IDAgenteQSMV').show();
    } else {
        $('#lblAgenteMV').hide();
        $('#IDAgenteQSMV').hide();
    }
    if ($('.HMA').is(":checked")) {
        $('#lblAgenteHMA').show();
        $('#IDAgenteQSHMA').show();
    } else {
        $('#lblAgenteHMA').hide();
        $('#IDAgenteQSHMA').hide();
    }
    if ($('.ECA').is(":checked")) {
        $('#lblAgenteECA').show();
        $('#IDAgenteQSECA').show();
    } else {
        $('#lblAgenteECA').hide();
        $('#IDAgenteQSECA').hide();
    }
}

function mostrarFichaExposicion() {
    if ($('#EsDeExposicion').val() == "true") {
        $('#panelSocios').hide();
        $('#TieneSocios').val(0); //Nos aseguramos de que tiene socios tome el valor NO
        $('#panelEmpresasVinculadas').hide();
        $('#TieneEmpresasVinculadas').val(0); //Nos aseguramos de que tiene empresas vinculadas tome el valor NO
        $('#panelContacto').hide();
        $('#panelActividad').hide();
        $('#personaAutorizada').hide();
        $('#Tarifa').val(""); //Nos aseguramos de que el campo tarifa quede vacío
        $('#DatosEmpresariales').hide();
        $('#MailFacturacion').hide();
        $('#PedidoEnFirme').hide();
        $('#lblDtoPP').hide();
        $('#DtoPP').hide();
        $('#lblRecargoFinanciero').hide();
        $('#RecargoFinanciero').hide();
        $('#lblTarifa').hide();
        $('#Tarifa').hide();
        $('#DirEnvioFacturas').hide()
        $('#IBAN').hide();
        $('#Vencimientos').hide();
        $('#ClientesHabituales').hide()
        $('#ProveedoresHabituales').hide()
        $('#Consumo').hide();
        $('#Creditos').hide();
        $('#DatosGerente').hide();
        $('#DatosGrupoEmpresarial').hide();
        $('#fichaLogística').hide();
        $('#TieneFichaLogistica').val(false);
        $('#panelFichaLogistica').hide();
        cargarFormasDePago(true);
    } else {
        $('#DatosEmpresariales').show();
        $('#MailFacturacion').show();
        $('#panelContacto').show();
        $('#panelActividad').show();
        $('#personaAutorizada').show();
        $('#lblDtoPP').show();
        $('#DtoPP').show();
        $('#lblRecargoFinanciero').show();
        $('#RecargoFinanciero').show();
        $('#lblTarifa').show();
        $('#Tarifa').show();
        $('#IBAN').show();
        $('#Vencimientos').show();
        $('#ClientesHabituales').show()
        $('#ProveedoresHabituales').show()
        $('#Consumo').show();
        $('#Creditos').show();
        $('#DatosGerente').show();
        $('#DatosGrupoEmpresarial').show();
        $('#fichaLogística').show();
        cargarFormasDePago(false);
        calcularTarifa();
    }


}

function mostrarPanelMails() {
    if ($('#TieneMail').val() == "true") {
        $('#panelMails').show();
    } else {
        $('#panelMails').hide();
    }
}

function cargarDatosCP() {
    var v = $('#CP').val();
    var url = "/Clientes/Clientes/getJsonLocalizaciones";
    var lastProv = "";
    var lastPais = "";
    var newProv = "";
    var newPais = "";
    $.getJSON(url, { cp: v }, function (municipios) {
        $('#Municipio').empty();
        $('#IDProvinciaQS').empty();
        $('#IDPaisQS').empty();

        //Cargamos los Municipios relacionados con el CP
        $defaultMun = $('<option />');
        $defaultMun.attr("value", "").text("--Seleccione un valor--");
        $('#Municipio').append($defaultMun);
        $defaultProv = $('<option />');
        $defaultProv.attr("value", "").text("--Seleccione un valor--");
        $('#IDProvinciaQS').append($defaultProv);
        $defaultPais = $('<option />');
        $defaultPais.attr("value", "").text("--Seleccione un valor--");
        $('#IDPaisQS').append($defaultPais);

        $(municipios).each(function () {
            // Añadimos el municipio al desplegable de municipios
            var $municipio = $('<option />');
            $municipio.attr("value", this.Municipio).text(this.Municipio);
            if (this.CP == v) {
                $municipio.attr("selected", "selected");
                $('#IDMunicipio').val(this.IDMunicipio);
                newProv = this.IDProvincia;
                newPais = this.IDPais;
            }

            $("#Municipio").append($municipio);

            //Añadimos las provincias al desplegable de provincias
            if (this.IDProvincia != lastProv) {
                var $provincia = $('<option />');
                $provincia.attr("value", this.IDProvincia).text(this.Provincia);
                $('#IDProvinciaQS').append($provincia);

                lastProv = this.IDProvincia;
            }

            //Añadimos los paises.
            if (this.IDPais != lastPais) {
                var $pais = $('<option />');
                $pais.attr("value", this.IDPais).text(this.Pais);
                $('#IDPaisQS').append($pais);

                lastPais = this.IDPais;
            }
        });

        //Seleccionamos la Provincia y el pais correspondiente
        if (newProv != "") {
            $(('#IDProvinciaQS option[value=').concat(newProv, ']')).attr("selected", "selected");
        }
        if (newPais != "") {
            $(('#IDPaisQS option[value=').concat(newPais, ']')).attr("selected", "selected");
        }

        //Calculamos la zona que le corresponde al cliente
        calcularZona()
    });
}

function calcularZona() {
    setTimeout(function () {
        var mun = $('#IDMunicipio').val();
        var prov = $('#IDProvinciaQS').val();
        var pais = $('#IDPaisQS').val();
        var url = '/Clientes/Clientes/getJsonZonas';

        //Dejo el desplegable sin ningún valor seleccionado
        $("#Zona option").prop("selected", false);

        if (mun != "" && prov != "" && pais != "") {
            //Si tengo un municipio, una provincia y un pais, calculo la ruta
            $.getJSON(url, { IDMun: mun, IDProv: prov, IDPais: pais }, function (data) {
                if (data) {
                    $(('#Zona option[value=').concat(data.IDZona.toString(), ']')).attr("selected", "selected");
                } else {
                    $("#Zona option[value='']").attr("selected", "selected");
                }
            });
        } else {
            $("#Zona option[value='']").attr("selected", "selected");
        }
    }, 500);
}

function mostrarDatosFormaDePago() {
    setTimeout(function () {
        var v = $("#FormaDePago").val();
        if (v == "") {
            if ($("#EsDeExposicion").val() == "false") {
                $('#trFormaDePagoSolicitada').show();
                $('#PedidoEnFirme').show();
                $("#PedidoEnFirme option[value='false']").attr("selected", "selected");
                $("#IBAN").hide();
                $("#Creditos").hide();
                $("#EsSepa").val(0);
                $("#lblDtoPP").hide();
                $("#DtoPP").hide();
                $("#lblRecargoFinanciero").hide();
                $("#RecargoFinanciero").hide();
            } else {
                $('#trFormaDePagoSolicitada').hide();
            }
        } else {
            $('#FormaDePagoSolicitada').val('');
            $('#trFormaDePagoSolicitada').hide();
            var url = "/Clientes/Clientes/getJsonDatosFormaDePago";
            $.getJSON(url, { id: v }, function (data) {
                $("#DtoPP").val(data.DtoPP.toString().replace('.',','));
                $("#RecargoFinanciero").val(data.RecargoFinanciero.toString().replace('.', ','));
                if (data.DtoPP > 0) {
                    $('#lblDtoPP').show();
                    $("#DtoPP").show();
                } else {
                    $('#lblDtoPP').hide();
                    $("#DtoPP").hide();
                }
                if (data.RecargoFinanciero > 0) {
                    $("#lblRecargoFinanciero").show();
                    $("#RecargoFinanciero").show();
                } else {
                    $("#lblRecargoFinanciero").hide();
                    $("#RecargoFinanciero").hide();
                }
                if (data.EsSEPA) {
                    $('#PedidoEnFirme').show();
                    $("#IBAN").show();
                    $("#Creditos").show();
                    $("#EsSepa").val(1);
                } else {
                    $("#IBAN").hide();
                    $('#PedidoEnFirme').hide();
                    $('#PedidoEnFirme').val(false);
                    $("#Creditos").hide();
                    $("#EsSepa").val(0);
                }
                
            });
        }
        IBANObligatorioSN();
    }, 500);
}

function cargarFormasDePago(SoloExposicion) {
    var v = SoloExposicion;
    var url = "/Clientes/Clientes/getJsonFormasDePago";
    $.getJSON(url, { blnSoloExposicion: v }, function (data) {
        var fPago = $("#FormaDePago").val();
        $("#FormaDePago").empty();
        var $option = $("<option />");
        if ($('#EsDeExposicion').val() == "true") {
            $option.attr("value", "").text("--Seleccione un valor--");
        } else {
            $option.attr("value", "").text("Otra (solicitar autorización)");
        }
        $('#FormaDePago').append($option);

        $(data).each(function () {
            // Create option
            var $option = $("<option />");
            // Add value and text to option
            $option.attr("value", this.Value).text(this.Text);
            if (this.Value == fPago) {
                $option.attr("selected", "selected");
            }
            // Add option to drop down list
            $("#FormaDePago").append($option);
        });
    });
    $("#DtoPP").val(0);
    $("#RecargoFinanciero").val(0);
    mostrarDatosFormaDePago();
}

function calcularTarifa() {
    if ($('.MORAVAL').is(":checked")) {
        var url = '/Clientes/Clientes/getJsonTarifa';
        var tipoCli = $('#TipoCliente').val();
        var actividad = $('#IDActividadQS').val();
        $.getJSON(url, { strTipoCliente: tipoCli, strActividad: actividad }, function (data) {
            if (data) {
                $('#Tarifa').val(data);
            } else {
                $('#Tarifa').val('');
            }
        });
    } else {
        $('#Tarifa').val(''); //Para HIERROS y ECA la tarifa es vacía
    }
}
function mostrarVtosFijos() {
    if ($('#NoTieneVtosFijos').is(":checked")) {
        $('#lblDiasVtoFijo').hide();
        $('#DiaVtoFijo1').hide();
        $('#DiaVtoFijo2').hide();
        $('#DiaVtoFijo3').hide();
    } else {
        $('#lblDiasVtoFijo').show();
        $('#DiaVtoFijo1').show();
        $('#DiaVtoFijo2').show();
        $('#DiaVtoFijo3').show();
    }
}

function mostrarFichaLogistica() {
    if ($('#TieneFichaLogistica').val() == "true") {
        $('#panelFichaLogistica').show();
        mostrarCAE();
    } else {
        $('#panelFichaLogistica').hide();
    }

}

function mostrarInstrumentosDePesaje() {
    if ($('#PesaElMaterial').val() == "true") {
        $('#instrumentosDePesaje').show();
    } else {
        $('#instrumentosDePesaje').hide();
    }
}

function mostrarCAE() {
    if ($('#CAEFirmada').val() == "true") {
        $('#panelFicheroCAE').show();
        $('#panelNoFirma').hide();
    } else {
        $('#panelFicheroCAE').hide();
        $('#panelNoFirma').show();
    }
}

function mostrarSocios() {
    if ($('#TieneSocios').val() == "1") {
        $('#panelSocios').show();
    } else {
        $('#panelSocios').hide();
    }
}


function añadirSocio() {
    var $tableBody = $('#panelSocios').find("tbody"),
                $trLast = $tableBody.find("tr:last"),
                $trNew = $trLast.clone();
    $trLast.after($trNew);
    $trLast = $tableBody.find("tr:last");
    $trLast.find('input:text').val('');
    //Regenero los ID
    $prevRow = ($('#panelSocios tr').length - 2).toString();
    $rows = ($('#panelSocios tr').length - 1).toString();
    $trLast.find(('#arrCIFSocio').concat($prevRow)).attr("id", ("arrCIFSocio").concat($rows));
    $trLast.find(('#arrNombreSocio').concat($prevRow)).attr("id", ("arrNombreSocio").concat($rows));
    $trLast.find(('#arrPorcentaSocio').concat($prevRow)).attr("id", ("arrPorcentaSocio").concat($rows));
}

function añadirClienteHab() {
    var $tableBody = $('#panelClientesHab').find("tbody"),
                $trLast = $tableBody.find("tr:last"),
                $trNew = $trLast.clone();
    $trLast.after($trNew);
    $trLast = $tableBody.find("tr:last");
    $trLast.find('input:text').val('');
}

function añadirProveedorHab() {
    var $tableBody = $('#panelProveedoresHab').find("tbody"),
                $trLast = $tableBody.find("tr:last"),
                $trNew = $trLast.clone();
    $trLast.after($trNew);
    $trLast = $tableBody.find("tr:last");
    $trLast.find('input:text').val('');
}

function añadirBanco() {
    var $tableBody = $('#panelBancos').find("tbody"),
                $trLast = $tableBody.find("tr:last"),
                $trNew = $trLast.clone();
    $trLast.after($trNew);
    $trLast = $tableBody.find("tr:last");
    $trLast.find('input:text').val('');
}
function mostrarEmpresasVinculadas() {
    if ($('#TieneEmpresasVinculadas').val() == "1") {
        $('#panelEmpresasVinculadas').show();
    } else {
        $('#panelEmpresasVinculadas').hide();
    }
}

function añadirEmpresaVinculada() {
    var $tableBody = $('#panelEmpresasVinculadas').find("tbody"),
                $trLast = $tableBody.find("tr:last"),
                $trNew = $trLast.clone();
    $trLast.after($trNew);
    $trLast = $tableBody.find("tr:last");
    $trLast.find('input:text').val('');
    $prevRow = ($('#panelEmpresasVinculadas tr').length - 2).toString();
    $rows = ($('#panelEmpresasVinculadas tr').length - 1).toString();
    $trLast.find(('#arrCIFVinc').concat($prevRow)).attr("id", ("arrCIFVinc").concat($rows));
    $trLast.find(('#arrEmpVinc').concat($prevRow)).attr("id", ("arrEmpVinc").concat($rows));
}