function mostrarPanelExposicion() {
    if (!$('.ECA').is(":checked")) {
        $('#panelExposicion').show();
    } else {
        $("#EsDeExposicion").val("false").change();
        mostrarFichaExposicion();
        $('#panelExposicion').hide();
    }
}

function noAdmiteFacturacionElectronica() {
    if ($('#NoAdmiteFacturacionElectronica').is(":checked")) {
        if ($('#EsDeExposicion').val() == "false") {
            //Si es un cliente de exposición no mostramos la dirección postal
            $('#ApartadoDeCorreos').show();
            if ($('#TipoDeViaFacturacion').val() == "") {
                $('#TieneApartadoPostalFacturacion').val("true");
                $('#TipoDeViaFacturacion').val("");
                $('#DomicilioFacturacion').val("");
                $('#NumeroFacturacion').val("");
                $('#PisoFacturacion').val("");
            }
            mostrarDirEnvioFacturas();
            $('#CPFacturacion').val($('#CP').val());
            cargarDatosCPFacturacion();
        } else {
            $('#EnvioPostal').hide();
        }
        $('#MailDeFacturacion').val("");
        $('#lblMailFacturacion').hide();
        $('#MailDeFacturacion').hide();
    } else {
        $('#EnvioPostal').hide();
        $('#lblMailFacturacion').show();
        $('#MailDeFacturacion').show();
    }
}

function mostrarDirEnvioFacturas() {
    $('#EnvioPostal').show();
    if ($('#TieneApartadoPostalFacturacion').val() == "false") {
        $('#DirEnvioFacturas').show();
        $('#lblApartadoPostal').hide();
        $('#ApatadoDeCorreosFacturacion').hide();
        if ($('#DomicilioFacturacion').val() == "") {
            $('#TipoDeViaFacturacion').val($('#TipoDeVia').val());
            $('#DomicilioFacturacion').val($('#Domicilio').val());
            $('#NumeroFacturacion').val($('#Numero').val());
            $('#PisoFacturacion').val($('#Piso').val());
            $('#CPFacturacion').val($('#CP').val());
            cargarDatosCPFacturacion();
        }
    } else {
        $('#DirEnvioFacturas').hide();
        $('#lblApartadoPostal').show();
        $('#ApatadoDeCorreosFacturacion').show();
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
        $('#panelRecogen').show();
        $('#panelParticular').show();
        $('#panelSocios').hide();
        $('#TieneSocios').val(0); //Nos aseguramos de que tiene socios tome el valor NO
        $('#panelEmpresasVinculadas').hide();
        $('#TieneEmpresasVinculadas').val(0); //Nos aseguramos de que tiene empresas vinculadas tome el valor NO
        $('#panelContacto').hide();
        $('#panelActividad').hide();
        $('#personaAutorizada').hide();
        $('#Tarifa').val(""); //Nos aseguramos de que el campo tarifa quede vacío
        $('#DatosEmpresariales').hide();
        $('#PedidoEnFirme').hide();
        $('#lblDtoPP').hide();
        $('#DtoPP').hide();
        $('#lblRecargoFinanciero').hide();
        $('#RecargoFinanciero').hide();
        $('#lblTarifa').hide();
        $('#Tarifa').hide();
        $('#ApartadoDeCorreos').hide();
        $('#DirEnvioFacturas').hide()
        $('#IBAN').hide();
        $('#Vencimientos').hide();
        $('#ClientesHabituales').hide()
        $('#ProveedoresHabituales').hide()
        $('#Consumo').hide();
        $('#Creditos').hide();
        $('#DatosGerente').hide();
        $('#DatosGrupoEmpresarial').hide();
        $('#lblMailFacturacion').text("Mail de envío factura");
        $('#lblNoAdmiteFacturacionElectronica').text("El cliente no quiere que se le envíe la factura");
        cargarFormasDePago(true);
    } else {
        $('#panelRecogen').hide();
        $('#panelParticular').hide();
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
        $('#lblMailFacturacion').text("Mail de facturación electrónica");
        $('#lblNoAdmiteFacturacionElectronica').text("El cliente no admite facturación electrónica");
        cargarFormasDePago(false);
        calcularTarifa();
    }
    mostrarCAE();
    noAdmiteFacturacionElectronica();

}

function asignarTipoDocumento(){
    if ($('#radioNIF').attr("checked")) {
        $('#lblNombre').text("Nombre y apellidos");
    } else {
        $('#lblNombre').text("Razón social");
    }
}

function asignarDatosPrevencion() {
    if ($('#EsClienteParticular').val() == "true") {
        if ($('#RecogeEnNuestrasInstalaciones').val() == "true") {
            //Si es cliente particular y recoge en nuestras instalaciones NO mostramos la ficha logística.
            $('#IDCausaNoFirmaCAE').val("2  ").attr("selected", "selected"); //RECOGEN
            $('#panelFichaLogistica').hide();
        } else if ($('#RecogeEnNuestrasInstalaciones').val() == "false") {
            //Si es cliente particular y no recoge en nuestras instalaciones ocultamos los requerimientos especiales y asignamos como causa de no firma "Cliente Particular"
            $('#panelFichaLogistica').show();
            $('#IDCausaNoFirmaCAE').val("1  ").attr("selected","selected"); //PARTICULAR
            $('#requerimientosCalidad').hide();
            $('#requerimientosPrevencion').hide();
        }
    } else {
        $('#panelFichaLogistica').show();
        $('#requerimientosCalidad').show();
        $('#requerimientosPrevencion').show();
        if ($('#RecogeEnNuestrasInstalaciones').val() == "true") {
            //Si no es particular y recoge en nuestras instalaciones, indicaremos como motivo de no firma por defecto "Recoge en nuestras instalaciones"
            $('#IDCausaNoFirmaCAE').val("2  ").attr("selected", "selected"); //RECOGEN
        }
    }
}

function mostrarPanelMails() {
    if ($('#TieneMail').val() == "true") {
        $('#panelMails').show();
    } else {
        $('#panelMails').hide();
    }
}

function mostrarNumero() {
    if ($('#SinNumero').is(":checked")) {
        $('#lblNumero').hide();
        $('#Numero').hide();
    } else {
        $('#lblNumero').show();
        $('#Numero').show();
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
                $('#IDMunicipioQS').val(this.IDMunicipio);
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
        var mun = $('#IDMunicipioQS').val();
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

function mostrarNumeroFacturacion() {
    if ($('#SinNumeroFacturacion').is(":checked")) {
        $('#lblNumeroFacturacion').hide();
        $('#NumeroFacturacion').hide();
    } else {
        $('#lblNumeroFacturacion').show();
        $('#NumeroFacturacion').show();
    }
}

function cargarDatosCPFacturacion() {
    var v = $('#CPFacturacion').val();
    var url = "/Clientes/Clientes/getJsonLocalizaciones";
    var lastProv = "";
    var lastPais = "";
    var newProv = "";
    var newPais = "";
    $.getJSON(url, { cp: v }, function (municipios) {
        $('#MunicipioFacturacion').empty();
        $('#IDProvinciaQSFacturacion').empty();
        $('#IDPaisQSFacturacion').empty();

        //Cargamos los Municipios relacionados con el CP
        $defaultMun = $('<option />');
        $defaultMun.attr("value", "").text("--Seleccione un valor--");
        $('#MunicipioFacturacion').append($defaultMun);
        $defaultProv = $('<option />');
        $defaultProv.attr("value", "").text("--Seleccione un valor--");
        $('#IDProvinciaQSFacturacion').append($defaultProv);
        $defaultPais = $('<option />');
        $defaultPais.attr("value", "").text("--Seleccione un valor--");
        $('#IDPaisQSFacturacion').append($defaultPais);

        $(municipios).each(function () {
            // Añadimos el municipio al desplegable de municipios
            var $municipio = $('<option />');
            $municipio.attr("value", this.Municipio).text(this.Municipio);
            if (this.CP == v) {
                $municipio.attr("selected", "selected");
                $('#IDMunicipioQSFacturacion').val(this.IDMunicipio);
                newProv = this.IDProvincia;
                newPais = this.IDPais;
            }

            $("#MunicipioFacturacion").append($municipio);

            //Añadimos las provincias al desplegable de provincias
            if (this.IDProvincia != lastProv) {
                var $provincia = $('<option />');
                $provincia.attr("value", this.IDProvincia).text(this.Provincia);
                $('#IDProvinciaQSFacturacion').append($provincia);

                lastProv = this.IDProvincia;
            }

            //Añadimos los paises.
            if (this.IDPais != lastPais) {
                var $pais = $('<option />');
                $pais.attr("value", this.IDPais).text(this.Pais);
                $('#IDPaisQSFacturacion').append($pais);

                lastPais = this.IDPais;
            }
        });

        //Seleccionamos la Provincia y el pais correspondiente
        if (newProv != "") {
            $(('#IDProvinciaQSFacturacion option[value=').concat(newProv, ']')).attr("selected", "selected");
        }
        if (newPais != "") {
            $(('#IDPaisQSFacturacion option[value=').concat(newPais, ']')).attr("selected", "selected");
        }
    });
}

function mostrarNumeroDirEnv(campo) {
    var pos = campo.attr("id").replace(/\D/g, '');
    if (campo.is(":checked")) {
        $('#arrlblNumeroDirEnv' + pos).hide();
        $('#arrNumeroDirEnv' + pos).hide();
    } else {
        $('#arrlblNumeroDirEnv' + pos).show();
        $('#arrNumeroDirEnv' + pos).show();
    }
}

function cargarDatosCPDirEnv(campo){
    var pos = campo.attr("id").replace(/\D/g, '');
    var v = campo.val();
    var url = "/Clientes/Clientes/getJsonLocalizaciones";
    var lastProv = "";
    var lastPais = "";
    var newProv = "";
    var newPais = "";
    $.getJSON(url, { cp: v }, function (municipios) {
        $('#arrMunicipioDirEnv' + pos).empty();
        $('#arrIDProvinciaQSDirEnv' + pos).empty();
        $('#arrIDPaisQSDirEnv' + pos).empty();

        //Cargamos los Municipios relacionados con el CP
        $defaultMun = $('<option />');
        $defaultMun.attr("value", "").text("--Seleccione un valor--");
        $('#arrMunicipioDirEnv' + pos).append($defaultMun);
        $defaultProv = $('<option />');
        $defaultProv.attr("value", "").text("--Seleccione un valor--");
        $('#arrIDProvinciaQSDirEnv' + pos).append($defaultProv);
        $defaultPais = $('<option />');
        $defaultPais.attr("value", "").text("--Seleccione un valor--");
        $('#arrIDPaisQSDirEnv' + pos).append($defaultPais);
        $(municipios).each(function () {
            // Añadimos el municipio al desplegable de municipios
            var $municipio = $('<option />');
            $municipio.attr("value", this.Municipio).text(this.Municipio);
            if (this.CP == v) {
                $municipio.attr("selected", "selected");
                $('#arrIDMunicipioQSDirEnv' + pos).val(this.IDMunicipio);
                newProv = this.IDProvincia;
                newPais = this.IDPais;
            }

            $("#arrMunicipioDirEnv" + pos).append($municipio);

            //Añadimos las provincias al desplegable de provincias
            if (this.IDProvincia != lastProv) {
                var $provincia = $('<option />');
                $provincia.attr("value", this.IDProvincia).text(this.Provincia);
                $('#arrIDProvinciaQSDirEnv' + pos).append($provincia);

                lastProv = this.IDProvincia;
            }

            //Añadimos los paises.
            if (this.IDPais != lastPais) {
                var $pais = $('<option />');
                $pais.attr("value", this.IDPais).text(this.Pais);
                $('#arrIDPaisQSDirEnv' + pos).append($pais);

                lastPais = this.IDPais;
            }
        });

        //Seleccionamos la Provincia y el pais correspondiente
        if (newProv != "") {
            $(('#arrIDProvinciaQSDirEnv' + pos + ' option[value=').concat(newProv, ']')).attr("selected", "selected");
        }
        if (newPais != "") {
            $(('#arrIDPaisQSDirEnv' + pos + ' option[value=').concat(newPais, ']')).attr("selected", "selected");
        }
    });
}

function mostrarPanelPersonaAutorizada() {
    if ($('#TienePersonasAutorizadasRetMat').val() == "true") {
        $('#panelPersonaAutorizada').show();
    } else {
        $('#panelPersonaAutorizada').hide();
    }
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

function mostrarImportePortesPorEnvio() {
    if ($('#CobroDePortesPorEnvio').val() == "true") {
        $('#lblImportePortesPorEnvio').show();
        $('#ImportePortesPorEnvio').show();
    } else {
        $('#lblImportePortesPorEnvio').hide();
        $('#ImportePortesPorEnvio').hide();
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

function añadirPersona() {
    $prevRow = ($('#panelPersonaAutorizada tr').length - 2).toString();
    if ($prevRow < 1) {
        var $tableBody = $('#panelPersonaAutorizada').find("tbody"),
                $trLast = $tableBody.find("tr:last"),
                $trNew = $trLast.clone();
        $trLast.after($trNew);
        $trLast = $tableBody.find("tr:last");
        $trLast.find('input:text').val('');
        //Regenero los ID
        $rows = ($('#panelPersonaAutorizada tr').length - 1).toString();
        $trLast.find(('#arrNIFPersona').concat($prevRow)).attr("id", ("arrNIFPersona").concat($rows));
        $trLast.find(('#arrNombrePersona').concat($prevRow)).attr("id", ("arrNombrePersona").concat($rows));
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

function mostrarDireccionesDeEnvio() {
    if ($('#TieneDireccionesDeEnvio').val() == "true") {
        $('#panelDireccionesDeEnvio').show();
    } else {
        $('#panelDireccionesDeEnvio').hide();
    }
}

function añadirDirEnvio() {
    var $tbodyLast = $('#panelDireccionEnvio').find("tbody:last"),
        $tbodyNew = $tbodyLast.clone(true);
    $tbodyLast.after($tbodyNew);
    $tbodyLast = $('#panelDireccionEnvio').find("tbody:last");
    $tbodyLast.find('input:text').val(''); //Limpiamos los valores de textbox
    $tbodyLast.find('select').val(''); //Limpiamos los valores de los combos
    //Regenero los ID
    $prevRow = ($('#panelDireccionEnvio tbody').length - 1).toString();
    $rows = ($('#panelDireccionEnvio tbody').length).toString();
    $tbodyLast.find(('#arrNombreDirEnv').concat($prevRow)).attr("id", ("arrNombreDirEnv").concat($rows));
    $tbodyLast.find(('#arrTipoDeViaDirEnv').concat($prevRow)).attr("id", ("arrTipoDeViaDirEnv").concat($rows));
    $tbodyLast.find(('#arrDomicilioDirEnv').concat($prevRow)).attr("id", ("arrDomicilioDirEnv").concat($rows));
    $tbodyLast.find(('#arrlblNumeroDirEnv').concat($prevRow)).attr("id", ("arrlblNumeroDirEnv").concat($rows));
    $tbodyLast.find(('#arrNumeroDirEnv').concat($prevRow)).attr("id", ("arrNumeroDirEnv").concat($rows));
    $tbodyLast.find(('#arrSinNumeroDirEnv').concat($prevRow)).attr("id", ("arrSinNumeroDirEnv").concat($rows));
    $tbodyLast.find(('#arrPisoDirEnv').concat($prevRow)).attr("id", ("arrPisoDirEnv1").concat($rows));
    $tbodyLast.find(('#arrCPDirEnv').concat($prevRow)).attr("id", ("arrCPDirEnv").concat($rows));
    $tbodyLast.find(('#arrMunicipioDirEnv').concat($prevRow)).attr("id", ("arrMunicipioDirEnv").concat($rows));
    $tbodyLast.find(('#arrIDMunicipioQSDirEnv').concat($prevRow)).attr("id", ("arrIDMunicipioQSDirEnv").concat($rows));
    $tbodyLast.find(('#arrIDProvinciaQSDirEnv').concat($prevRow)).attr("id", ("arrIDProvinciaQSDirEnv").concat($rows));
    $tbodyLast.find(('#arrIDPaisQSDirEnv').concat($prevRow)).attr("id", ("arrIDPaisQSDirEnv").concat($rows));    
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

function asignarMailDeFacturacion() {
    if ($("#EsDeExposicion").val() == "true") {
        if (confirm("¿Desea asignar el mail " + $("#MailDeContacto").val() + " para envíar la factura?")){
            $("#MailDeFacturacion").val($("#MailDeContacto").val());
        }
    }
}

function guardarObservacionesGestion(campo) {
    var url = '/Clientes/Clientes/guardarObservaciones';
    var nombre = campo.attr('id');
    var idCliente = nombre.split('-');
    var observaciones = campo.val().substr(0, 200);
    $.getJSON(url, { intIDCliente: idCliente[1], strObservaciones: observaciones });
}