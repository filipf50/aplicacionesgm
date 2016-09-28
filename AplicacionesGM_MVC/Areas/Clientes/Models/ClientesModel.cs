﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Data.Odbc;
using System.Configuration;
using AplicacionesGM_MVC.Models;
using AplicacionesGM_MVC.Areas.Clientes.Models.DSas400TableAdapters;

namespace AplicacionesGM_MVC.Areas.Clientes.Models
{
    public class ClientesModel
    {
        AplicacionesGM_MVCEntities db = new AplicacionesGM_MVCEntities();
        BDClientesSQLEntities dbClientes = new BDClientesSQLEntities();
        BDPlanificacionTransportesEntities dbPlanificacion = new BDPlanificacionTransportesEntities();

        System.Data.Objects.IObjectSet<aspnet_Empresas> empresas;
        OdbcConnection con = new OdbcConnection(ConfigurationManager.ConnectionStrings["as400ConnectionString"].ConnectionString);

        public ClientesModel() //Constructor
        {
            aspnet_Empresas objEmpresa=new aspnet_Empresas();
            
            //Cargamos las empresas con las que vamos a trabajar
            empresas = db.aspnet_Empresas;
        }

        #region FUNCIONES QS
        private decimal getIDClienteQSByNIF(string strEmpresa, string strNIF)
        {
            decimal decIDCliente = 0;
            string strQuery = "SELECT CLCDG FROM MRVF" + strEmpresa + "COM.CLNCL WHERE CLNIF='" + strNIF + "'";
            OdbcCommand command = new OdbcCommand();
            con.Open();
            command.Connection = con;
            command.CommandText = strQuery;
            var aux=command.ExecuteScalar();
            con.Close();
            command.Dispose();

            if (aux != null)
            {
                decIDCliente = (decimal)aux;
            }
            return decIDCliente;
        }
        private bool insertarClienteEnEmpresaQS(string strEmpresa, aspnet_Clientes objCliente)
        {
            bool blnRes = true;
            OdbcCommand command = new OdbcCommand();
            try
            {
                //Ficha del cliente
                #region TABLA CLNCL
                string strQuery = "INSERT INTO MRVF" + strEmpresa + "COM.CLNCL(CLCDG,CLNIF,CLNBR,CLSGL,CLVIA,CLNMR,CLAMP,CLMNC,CLCDP,CLPAI,CLPRV,CLAG1,CLTF1,CLTF2,CLTFX,CLMAIL,CLURL,CLACT,CLTCL,CLTTR,CLFPG,CLDTPP,CLRCG,CLDF1,CLDF2,CLDF3,CLZON,CLOBS,CLFALC,CLRGO,CLRGOP,CLBLQ,CLTAL,CLTIPT,CLDIV,CLDIVC,CLCCN,CLRCO) ";
                string strValores = "";
                strValores = "VALUES (" + objCliente.QSID.ToString();
                strValores += ",'" + objCliente.NIF.ToString() + "'";
                strValores += ",'" + objCliente.Nombre.ToString() + "'";
                strValores += ",'" + objCliente.TipoDeVia.ToString() + "'";
                strValores += ",'" + objCliente.Domicilio.ToString() + "'";
                strValores += "," + objCliente.Numero.ToString();
                strValores += ",'" + (objCliente.Piso ?? "").ToString() + "'";
                strValores += ",'" + objCliente.Municipio.ToString() + "'";
                strValores += "," + objCliente.CP.ToString();
                strValores += "," + objCliente.IDPaisQS.ToString();
                strValores += "," + objCliente.IDProvinciaQS.ToString();
                strValores += "," + (strEmpresa == "003" || strEmpresa == "033" ? (objCliente.IDAgenteQSMV ?? 0).ToString() : (strEmpresa == "004" || strEmpresa == "044" ? (objCliente.IDAgenteQSHMA ?? 0).ToString() : (objCliente.IDAgenteQSECA ?? 0).ToString()));
                strValores += "," + (objCliente.Telefono1 ?? 0).ToString();
                strValores += "," + (objCliente.Telefono2 ?? 0).ToString();
                strValores += "," + (objCliente.Fax ?? 0).ToString();
                strValores += ",'" + (objCliente.TieneMail ? (objCliente.MailDeContacto.ToString() ?? "") + "'" : "NO TIENE'");
                strValores += ",'" + (objCliente.Contacto ?? "").ToString() + " - " + (objCliente.Web ?? "").ToString() + "'";
                strValores += ",'" + objCliente.IDActividadQS.ToString() + "'";
                strValores += ",'" + objCliente.TipoCliente.ToString() + "'";
                strValores += "," + ((objCliente.Tarifa ?? 0).ToString());
                strValores += "," + objCliente.FormaDePago.ToString();
                strValores += "," + (objCliente.DtoPP ?? 0).ToString().Replace(",", ".");
                strValores += "," + (objCliente.RecargoFinanciero ?? 0).ToString().Replace(",", ".");
                strValores += "," + (objCliente.DiaVtoFijo1 ?? 0).ToString();
                strValores += "," + (objCliente.DiaVtoFijo2 ?? 0).ToString();
                strValores += "," + (objCliente.DiaVtoFijo3 ?? 0).ToString();
                strValores += "," + (objCliente.Zona ?? 0).ToString();
                strValores += ",'" + ((objCliente.Observaciones ?? "").ToString().Length > 60 ? (objCliente.Observaciones ?? "").ToString().Substring(1, 60) : (objCliente.Observaciones ?? "").ToString()) + "'";
                strValores += "," + objCliente.FechaVolcadoQS.Value.ToString("yyyyMMdd");
                //Valores por defecto
                if (objCliente.aspnet_FormasDePago.RequiereDocSEPA)
                {
                    strValores += ",'R'"; //Riesgo Venta
                    strValores += ",'A'"; //Riesgo Pedido
                    strValores += ",'S'"; //Bloqueado
                }
                else
                {
                    strValores += ",'A'"; //Riesgo Venta
                    strValores += ",'A'"; //Riesgo Pedido
                    strValores += ",''"; //Bloqueado
                }
                strValores += ", 'I'"; //Tipo de Albarán
                strValores += ", 'E'"; //Tipo de tarifa
                strValores += ", 'EUR'"; //Divisa
                strValores += ", 'ESP'"; //Divisa cambio

                //Cuenta contable
                if (objCliente.IDActividadQS.ToString().Trim() == "EG")
                {
                    strValores += ", 43300" + objCliente.QSID.ToString();
                }
                else
                {
                    strValores += ", 43000" + objCliente.QSID.ToString();
                }
                strValores += ", " + (strEmpresa == "003" || strEmpresa == "033" ? (objCliente.IDAgenteQSMV ?? 0).ToString() : (strEmpresa == "004" || strEmpresa == "044" ? (objCliente.IDAgenteQSHMA ?? 0).ToString() : (objCliente.IDAgenteQSECA ?? 0).ToString())) + ")";

                strQuery += strValores;
                con.Open();
                command.Connection = con;
                command.CommandText = strQuery;

                var aux = command.ExecuteNonQuery();
                #endregion CLNCL

                //Actualizar tablas auxiliares
                actualizarTablasAuxiliaresClienteEnQS(strEmpresa, objCliente);
            }
            catch (Exception ex)
            {
                blnRes = false;
            }
            finally
            {
                con.Close();
                command.Dispose();
            }
            return blnRes;
        }
        private bool actualizarClienteEnEmpresaQS(string strEmpresa, aspnet_Clientes objCliente)
        {
            bool blnRes = true;
            OdbcCommand command = new OdbcCommand();
            try
            {
                //Actualizamos los datos de la ficha del cliente
                #region TABLA CLNCL
                string strQuery = "UPDATE MRVF" + strEmpresa + "COM.CLNCL SET ";
                string strValores = "";
                strValores += "CLNIF='" + objCliente.NIF.ToString() + "'";
                strValores += ", CLNBR='" + objCliente.Nombre.ToString() + "'";
                strValores += ", CLSGL='" + objCliente.TipoDeVia.ToString() + "'";
                strValores += ", CLVIA='" + objCliente.Domicilio.ToString() + "'";
                strValores += ", CLNMR=" + objCliente.Numero.ToString();
                strValores += ", CLAMP='" + (objCliente.Piso ?? "").ToString() + "'";
                strValores += ", CLMNC='" + objCliente.Municipio.ToString() + "'";
                strValores += ", CLCDP=" + objCliente.CP.ToString();
                strValores += ", CLPAI=" + objCliente.IDPaisQS.ToString();
                strValores += ", CLPRV=" + objCliente.IDProvinciaQS.ToString();
                strValores += ", CLAG1=" + (strEmpresa == "003" || strEmpresa == "033" ? (objCliente.IDAgenteQSMV ?? 0).ToString() : (strEmpresa == "004" || strEmpresa == "044" ? (objCliente.IDAgenteQSHMA ?? 0).ToString() : (objCliente.IDAgenteQSECA ?? 0).ToString()));
                strValores += ", CLTF1=" + (objCliente.Telefono1 ?? 0).ToString();
                strValores += ", CLTF2=" + (objCliente.Telefono2 ?? 0).ToString();
                strValores += ", CLTFX=" + (objCliente.Fax ?? 0).ToString();
                strValores += ", CLMAIL='" + (objCliente.TieneMail ? (objCliente.MailDeContacto.ToString() ?? "") + "'" : "NO TIENE'");
                strValores += ", CLURL='" + (objCliente.Contacto ?? "").ToString() + " - " + (objCliente.Web ?? "").ToString() + "'";
                strValores += ", CLACT='" + objCliente.IDActividadQS.ToString() + "'";
                strValores += ", CLTCL='" + objCliente.TipoCliente.ToString() + "'";
                strValores += ", CLTTR=" + ((objCliente.Tarifa ?? 0).ToString());
                strValores += ", CLFPG=" + objCliente.FormaDePago.ToString();
                strValores += ", CLDTPP=" + (objCliente.DtoPP ?? 0).ToString().Replace(",", ".");
                strValores += ", CLRCG=" + (objCliente.RecargoFinanciero ?? 0).ToString().Replace(",", ".");
                strValores += ", CLDF1=" + (objCliente.DiaVtoFijo1 ?? 0).ToString();
                strValores += ", CLDF2=" + (objCliente.DiaVtoFijo2 ?? 0).ToString();
                strValores += ", CLDF3=" + (objCliente.DiaVtoFijo3 ?? 0).ToString();
                strValores += ", CLZON=" + (objCliente.Zona ?? 0).ToString();
                strValores += ", CLOBS='" + ((objCliente.Observaciones ?? "").ToString().Length > 60 ? (objCliente.Observaciones ?? "").ToString().Substring(1, 60) : (objCliente.Observaciones ?? "").ToString()) + "'";
                //Valores por defecto
                if (objCliente.aspnet_FormasDePago.RequiereDocSEPA)
                {
                    strValores += ", CLRGO='R'"; //Riesgo Venta
                    strValores += ", CLRGOP='A'"; //Riesgo Pedido
                    strValores += ", CLBLQ='S'"; //Bloqueado
                }
                else
                {
                    strValores += ", CLRGO='A'"; //Riesgo Venta
                    strValores += ", CLRGOP='A'"; //Riesgo Pedido
                    strValores += ", CLBLQ=''"; //Bloqueado
                }
                strValores += ", CLTAL='I'"; //Tipo de Albarán
                strValores += ", CLTIPT='E'"; //Tipo de tarifa
                strValores += ", CLDIV='EUR'"; //Divisa
                strValores += ", CLDIVC='ESP'"; //Divisa cambio

                strValores += ", CLRCO=" + (strEmpresa == "003" || strEmpresa == "033" ? (objCliente.IDAgenteQSMV ?? 0).ToString() : (strEmpresa == "004" || strEmpresa == "044" ? (objCliente.IDAgenteQSHMA ?? 0).ToString() : (objCliente.IDAgenteQSECA ?? 0).ToString()));
                strValores += " WHERE CLCDG=" + objCliente.QSID.ToString();

                strQuery += strValores;
                con.Open();
                command.Connection = con;
                command.CommandText = strQuery;
                var aux = command.ExecuteNonQuery();
                #endregion

                //Actualizar tablas auxiliares
                actualizarTablasAuxiliaresClienteEnQS(strEmpresa, objCliente);

            }
            catch (Exception ex)
            {
                blnRes = false;
            }
            finally
            {
                con.Close();
                command.Dispose();
            }
            return blnRes;
        }
        private void actualizarTablasAuxiliaresClienteEnQS(string strEmpresa, aspnet_Clientes objCliente)
        {
            OdbcCommand command = new OdbcCommand();
            string strQuery = "";
            string strValores = "";
            command.Connection = con;
            //Añadimos el control para exceso de deuda
            #region TABLA CLNED
            if (!existeExcesoDeudaClienteQS(strEmpresa, objCliente))
            {
                strQuery = "INSERT INTO MRVF" + strEmpresa + "COM.CLNED (EDCLN,EDEDD) VALUES (" + objCliente.QSID + ", 'S')";
            }
            else
            {
                strQuery = "UPDATE MRVF" + strEmpresa + "COM.CLNED SET EDEDD='S' WHERE EDCLN=" + objCliente.QSID;
            }
            command.CommandText = strQuery;
            if (con.State == System.Data.ConnectionState.Closed)
            {
                con.Open();
            }
            command.ExecuteNonQuery();
            #endregion

            //Gestionamos los fotomontajes
            #region TABLA CLNFT
            if (objCliente.IDActividadQS == "CP" || objCliente.IDActividadQS == "C" || objCliente.IDActividadQS == "PR")
            {
                //Si la actividad del cliente es CONTRUCTOR-PROMOTOR, CONSTRUCTOR O PROMOTOR SE OBLIGA A QUE TENGA FOTOMONTAJE
                if (!existeFotomontajeClienteQS(strEmpresa, objCliente))
                {
                    strQuery = "INSERT INTO MRVF" + strEmpresa + "COM.CLNFT (FTCCN,FTFOT) VALUES (" + objCliente.QSID.ToString() + ", 'S')";
                }
                else
                {
                    strQuery = "UPDATE MRVF" + strEmpresa + "COM.CLNFT SET FTFOT='S' WHERE FTCCN=" + objCliente.QSID.ToString();
                }
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }
                command.CommandText = strQuery;
                command.ExecuteNonQuery();
            }
            else
            {
                //Si la actividad es diferente borramos el registro 
                strQuery = "DELETE FROM MRVF" + strEmpresa + "COM.CLNFT WHERE FTCCN=" + objCliente.QSID.ToString();
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }
                command.CommandText = strQuery;
                command.ExecuteNonQuery();
            }
            #endregion

            //Añadimos los datos de Observaciones con los datos de la FICHA LOGÍSTICA
            #region TABLA CLNOB
            if (objCliente.TieneFichaLogistica)
            {
                if (!existeFichaLogisticaClienteQS(strEmpresa, objCliente))
                {
                    strQuery = "INSERT INTO MRVF" + strEmpresa + "COM.CLNOB (OBCDG,OBPM1,OBPM2,OBPM3,OBPM4,OBPM5,OBPM6,OBPM7,OBPO1,OBPO2,OBAM1,OBAM2,OBAM3,OBAM4,OBAM5,OBAM6,OBAM7,OBAE1,OBAE2,OBFB2) ";
                    strValores = "VALUES (" + objCliente.QSID.ToString();
                    strValores += ", 'HORARIO: " + objCliente.Horario.ToString() + "'";
                    strValores += ", 'HORARIO VERANO: " + (objCliente.HorarioDeVerano ?? "").ToString() + "'";
                    strValores += ", 'M.T.PROPIOS: " + (objCliente.MedioDeTransportePropio ? "SI" : "NO") + "/AUT.RECOGIDAS:" + (objCliente.NIFPersonalAutorizadoRetiradaMaterial ?? "").ToString() + " " + (objCliente.NombrePersonalAutorizadoRetiradaMaterial ?? "").ToString() + "'";
                    strValores += ", 'P.X ENVIO: " + (objCliente.CobroDePortesPorEnvio ?? "").ToString() + " / V. PARA SERVICIO: " + objCliente.aspnet_TiposDeVehiculo.Nombre.ToString() + "'";
                    strValores += ", 'MEDIOS DE DESCARGA: " + objCliente.aspnet_MediosDeDescarga.Nombre + " / CAMION PLUMA: " + (objCliente.NecesitaCamionConPluma ? "SI" : "NO") + "'";
                    strValores += ", '" + (objCliente.PesaElMaterial ? "EL CLIENTE PESA EL MATERIAL CON " + objCliente.aspnet_InstrumentosDePesaje.Nombre.ToString() : "EL CLIENTE NO PESA EL MATERIAL") + "'";
                    strValores += ", 'REQUERIMIENTOS DE PREVENCION: " + (objCliente.RequerimientosDePrevencion ?? "").ToString() + "'";
                    strValores += ", 'HORARIO: " + objCliente.Horario.ToString() + "'";
                    strValores += ", 'HORARIO VERANO: " + (objCliente.HorarioDeVerano ?? "").ToString() + "'";
                    strValores += ", 'HORARIO: " + objCliente.Horario.ToString() + "'";
                    strValores += ", 'HORARIO VERANO: " + (objCliente.HorarioDeVerano ?? "").ToString() + "'";
                    strValores += ", 'M.T.PROPIOS: " + (objCliente.MedioDeTransportePropio ? "SI" : "NO") + "/AUT.RECOGIDAS:" + (objCliente.NIFPersonalAutorizadoRetiradaMaterial ?? "").ToString() + " " + (objCliente.NombrePersonalAutorizadoRetiradaMaterial ?? "").ToString() + "'";
                    strValores += ", 'P.X ENVIO: " + (objCliente.CobroDePortesPorEnvio ?? "").ToString() + " / V. PARA SERVICIO: " + objCliente.aspnet_TiposDeVehiculo.Nombre.ToString() + "'";
                    strValores += ", 'MEDIOS DE DESCARGA: " + objCliente.aspnet_MediosDeDescarga.Nombre + " / CAMION PLUMA: " + (objCliente.NecesitaCamionConPluma ? "SI" : "NO") + "'";
                    strValores += ", '" + (objCliente.PesaElMaterial ? "EL CLIENTE PESA EL MATERIAL CON " + objCliente.aspnet_InstrumentosDePesaje.Nombre.ToString() : "EL CLIENTE NO PESA EL MATERIAL") + "'";
                    strValores += ", 'REQUERIMIENTOS DE PREVENCION: " + (objCliente.RequerimientosDePrevencion ?? "").ToString() + "'";
                    strValores += ", 'HORARIO: " + objCliente.Horario.ToString() + "'";
                    strValores += ", 'HORARIO VERANO: " + (objCliente.HorarioDeVerano ?? "").ToString() + "'";
                    strValores += ", '" + (objCliente.DirEnvioFactura ?? "").ToString() + "')";
                }
                else
                {
                    strQuery = "UPDATE MRVF" + strEmpresa + "COM.CLNOB SET ";
                    strValores = "OBPM1='HORARIO: " + objCliente.Horario.ToString() + "'";
                    strValores += ", OBPM2='HORARIO VERANO: " + (objCliente.HorarioDeVerano ?? "").ToString() + "'";
                    strValores += ", OBPM3='M.T.PROPIOS: " + (objCliente.MedioDeTransportePropio ? "SI" : "NO") + "/AUT.RECOGIDAS:" + (objCliente.NIFPersonalAutorizadoRetiradaMaterial ?? "").ToString() + " " + (objCliente.NombrePersonalAutorizadoRetiradaMaterial ?? "").ToString() + "'";
                    strValores += ", OBPM4='P.X ENVIO: " + (objCliente.CobroDePortesPorEnvio ?? "").ToString() + " / V. PARA SERVICIO: " + objCliente.aspnet_TiposDeVehiculo.Nombre.ToString() + "'";
                    strValores += ", OBPM5='MEDIOS DE DESCARGA: " + objCliente.aspnet_MediosDeDescarga.Nombre + " / CAMION PLUMA: " + (objCliente.NecesitaCamionConPluma ? "SI" : "NO") + "'";
                    strValores += ", OBPM6='" + (objCliente.PesaElMaterial ? "EL CLIENTE PESA EL MATERIAL CON " + objCliente.aspnet_InstrumentosDePesaje.Nombre.ToString() : "EL CLIENTE NO PESA EL MATERIAL") + "'";
                    strValores += ", OBPM7='REQUERIMIENTOS DE PREVENCION: " + (objCliente.RequerimientosDePrevencion ?? "").ToString() + "'";
                    strValores += ", OBPO1='HORARIO: " + objCliente.Horario.ToString() + "'";
                    strValores += ", OBPO2='HORARIO VERANO: " + (objCliente.HorarioDeVerano ?? "").ToString() + "'";
                    strValores += ", OBAM1='HORARIO: " + objCliente.Horario.ToString() + "'";
                    strValores += ", OBAM2='HORARIO VERANO: " + (objCliente.HorarioDeVerano ?? "").ToString() + "'";
                    strValores += ", OBAM3='M.T.PROPIOS: " + (objCliente.MedioDeTransportePropio ? "SI" : "NO") + "/AUT.RECOGIDAS:" + (objCliente.NIFPersonalAutorizadoRetiradaMaterial ?? "").ToString() + " " + (objCliente.NombrePersonalAutorizadoRetiradaMaterial ?? "").ToString() + "'";
                    strValores += ", OBAM4='P.X ENVIO: " + (objCliente.CobroDePortesPorEnvio ?? "").ToString() + " / V. PARA SERVICIO: " + objCliente.aspnet_TiposDeVehiculo.Nombre.ToString() + "'";
                    strValores += ", OBAM5='MEDIOS DE DESCARGA: " + objCliente.aspnet_MediosDeDescarga.Nombre + " / CAMION PLUMA: " + (objCliente.NecesitaCamionConPluma ? "SI" : "NO") + "'";
                    strValores += ", OBAM6='" + (objCliente.PesaElMaterial ? "EL CLIENTE PESA EL MATERIAL CON " + objCliente.aspnet_InstrumentosDePesaje.Nombre.ToString() : "EL CLIENTE NO PESA EL MATERIAL") + "'";
                    strValores += ", OBAM7='REQUERIMIENTOS DE PREVENCION: " + (objCliente.RequerimientosDePrevencion ?? "").ToString() + "'";
                    strValores += ", OBAE1='HORARIO: " + objCliente.Horario.ToString() + "'";
                    strValores += ", OBAE2='HORARIO VERANO: " + (objCliente.HorarioDeVerano ?? "").ToString() + "'";
                    strValores += ", OBFB2='" + (objCliente.DirEnvioFactura ?? "").ToString() + "' ";
                    strValores += "WHERE OBCDG=" + objCliente.QSID.ToString();
                }
                strQuery += strValores;
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }
                command.CommandText = strQuery;
                command.ExecuteNonQuery();
            }
            else
            {
                //Si no tenemos ficha logística borramos los datos que puedan existir
                strQuery = "DELETE FROM MRVF" + strEmpresa + "COM.CLNOB WHERE OBCDG=" + objCliente.QSID.ToString();
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }
                command.CommandText = strQuery;
                command.ExecuteNonQuery();
            }
            #endregion

            //Datos factura electrónica
            #region TABLA CLNCX
            if (!objCliente.NoAdmiteFacturacionElectronica)
            {
                if (!existeFacturaElectronicaClienteQS(strEmpresa, objCliente))
                {
                    strQuery = "INSERT INTO MRVF" + strEmpresa + "COM.CLNCX (CXCDG,CXFFD,CXTIPR,CXEMLF,CXFOR,CXFIR,CXENV) ";
                    strValores = "VALUES (" + objCliente.QSID.ToString();
                    strValores += ", 'S'";
                    strValores += ", 'R'";
                    strValores += ", '" + objCliente.MailDeFacturacion.ToString() + "'";
                    strValores += ", 'P'";
                    strValores += ", 'F'";
                    strValores += ", 'M')";
                }
                else
                {
                    strQuery = "UPDATE MRVF" + strEmpresa + "COM.CLNCX SET ";
                    strValores = " CXFFD='S'";
                    strValores += ", CXTIPR='R'";
                    strValores += ", CXEMLF='" + objCliente.MailDeFacturacion.ToString() + "'";
                    strValores += ", CXFOR='P'";
                    strValores += ", CXFIR='F'";
                    strValores += ", CXENV='M'";
                    strValores += " WHERE CXCDG =" + objCliente.QSID.ToString();
                }
                strQuery += strValores;
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }
                command.CommandText = strQuery;
                command.ExecuteNonQuery();
            }
            else
            {
                //Si no admite factura electrónica borramos los datos que pudiesen existir
                strQuery = "DELETE FROM MRVF" + strEmpresa + "COM.CLNCX WHERE CXCDG=" + objCliente.QSID.ToString();
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }
                command.CommandText = strQuery;
                command.ExecuteNonQuery();
            }
            #endregion

            //Grabamos el IBAM si corresponde
            #region TABLA CLNIB
            if ((objCliente.IBAN ?? "").ToString() != "")
            {
                if (!existeIBANClienteQS(strEmpresa, objCliente))
                {
                    strQuery = "INSERT INTO MRVF" + strEmpresa + "COM.CLNIB (IBCDG, IBIBAN) ";
                    strValores = "VALUES ( " + objCliente.QSID.ToString();
                    strValores += ", '" + objCliente.IBAN.ToString() + "')";
                }
                else
                {
                    strQuery = "UPDATE MRVF" + strEmpresa + "COM.CLNIB SET ";
                    strValores = "IBIBAN ='" + objCliente.IBAN.ToString() + "' ";
                    strValores += "WHERE IBCDG=" + objCliente.QSID.ToString();
                }
                strQuery += strValores;
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }
                command.CommandText = strQuery;
                command.ExecuteNonQuery();
            }
            else
            {
                //Si no tiene IBAN borramos la información que pudiese existir
                strQuery = "DELETE FROM MRVF" + strEmpresa + "COM.CLNIB WHERE IBCDG=" + objCliente.QSID.ToString();
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }
                command.CommandText = strQuery;
                command.ExecuteNonQuery();
            }
            #endregion

            //Grabamos los datos en PRESUNTOS
            #region TABLA BDGPR
            if (!existePresuntoClienteQS(strEmpresa, objCliente))
            {
                strQuery = "INSERT INTO MRVBQSF" + strEmpresa + ".BDGPR (PRCDG, PRCSG,PRCLN,PRNBR,PRCIF,PRSGL,PRVIA,PRNMR,PRAMP,PRMNC,PRCDP,PRPAI,PRPRV,PRTF1,PRTF2,PRFAX,PRMAIL,PRURL,PRDIVC,PRAGT,PRCMA1,PRCMA2,PRCMA3,PRCMN1,PRCMN2) ";
                strValores = " VALUES (" + objCliente.QSID.ToString();
                strValores += "," + objCliente.QSID.ToString();
                strValores += "," + objCliente.QSID.ToString();
                strValores += ",'" + objCliente.Nombre.ToString() + "'";
                strValores += ",'" + objCliente.NIF.ToString() + "'";
                strValores += ",'" + objCliente.TipoDeVia.ToString() + "'";
                strValores += ",'" + objCliente.Domicilio.ToString() + "'";
                strValores += "," + objCliente.Numero.ToString();
                strValores += ",'" + (objCliente.Piso ?? "").ToString() + "'";
                strValores += ",'" + objCliente.Municipio.ToString() + "'";
                strValores += "," + objCliente.CP.ToString();
                strValores += "," + objCliente.IDPaisQS.ToString();
                strValores += "," + objCliente.IDProvinciaQS.ToString();
                strValores += "," + (objCliente.Telefono1 ?? 0).ToString();
                strValores += "," + (objCliente.Telefono2 ?? 0).ToString();
                strValores += "," + (objCliente.Fax ?? 0).ToString();
                strValores += ",'" + (objCliente.TieneMail ? (objCliente.MailDeContacto.ToString() ?? "") + "'" : "NO TIENE'");
                strValores += ",'" + (objCliente.Contacto ?? "").ToString() + " - " + (objCliente.Web ?? "").ToString() + "'";
                strValores += ",'ESP'";
                strValores += "," + (strEmpresa == "003" || strEmpresa == "033" ? (objCliente.IDAgenteQSMV ?? 0).ToString() : (strEmpresa == "004" || strEmpresa == "044" ? (objCliente.IDAgenteQSHMA ?? 0).ToString() : (objCliente.IDAgenteQSECA ?? 0).ToString()));
                strValores += ",'" + objCliente.FrecuenciaVisita.ToString() + "'";
                strValores += ",'" + objCliente.DiasVisita.ToString() + "'";
                strValores += ",'" + objCliente.FormaContacto.ToString() + "'";
                strValores += "," + objCliente.ConsumoPotencial.ToString().Replace(",", ".");
                strValores += "," + objCliente.PrevisionAnual.ToString().Replace(",", ".") + ")";
            }
            else
            {
                strQuery = "UPDATE MRVBQSF" + strEmpresa + ".BDGPR SET ";
                strValores = " PRCSG=" + objCliente.QSID.ToString();
                strValores += ", PRCLN=" + objCliente.QSID.ToString();
                strValores += ", PRNBR='" + objCliente.Nombre.ToString() + "'";
                strValores += ", PRCIF='" + objCliente.NIF.ToString() + "'";
                strValores += ", PRSGL='" + objCliente.TipoDeVia.ToString() + "'";
                strValores += ", PRVIA='" + objCliente.Domicilio.ToString() + "'";
                strValores += ", PRNMR=" + objCliente.Numero.ToString();
                strValores += ", PRAMP='" + (objCliente.Piso ?? "").ToString() + "'";
                strValores += ", PRMNC='" + objCliente.Municipio.ToString() + "'";
                strValores += ", PRCDP=" + objCliente.CP.ToString();
                strValores += ", PRPAI=" + objCliente.IDPaisQS.ToString();
                strValores += ", PRPRV=" + objCliente.IDProvinciaQS.ToString();
                strValores += ", PRTF1=" + (objCliente.Telefono1 ?? 0).ToString();
                strValores += ", PRTF2=" + (objCliente.Telefono2 ?? 0).ToString();
                strValores += ", PRFAX=" + (objCliente.Fax ?? 0).ToString();
                strValores += ", PRMAIL='" + (objCliente.TieneMail ? (objCliente.MailDeContacto.ToString() ?? "") + "'" : "NO TIENE'");
                strValores += ", PRURL='" + (objCliente.Contacto ?? "").ToString() + " - " + (objCliente.Web ?? "").ToString() + "'";
                strValores += ", PRDIVC='ESP'";
                strValores += ", PRAGT=" + (strEmpresa == "003" || strEmpresa == "033" ? (objCliente.IDAgenteQSMV ?? 0).ToString() : (strEmpresa == "004" || strEmpresa == "044" ? (objCliente.IDAgenteQSHMA ?? 0).ToString() : (objCliente.IDAgenteQSECA ?? 0).ToString()));
                strValores += ", PRCMA1='" + objCliente.FrecuenciaVisita.ToString() + "'";
                strValores += ", PRCMA2='" + objCliente.DiasVisita.ToString() + "'";
                strValores += ", PRCMA3='" + objCliente.FormaContacto.ToString() + "'";
                strValores += ", PRCMN1=" + objCliente.ConsumoPotencial.ToString().Replace(",", ".");
                strValores += ", PRCMN2=" + objCliente.PrevisionAnual.ToString().Replace(",", ".");
                strValores += " WHERE PRCDG=" + objCliente.QSID.ToString();
            }
            strQuery += strValores;
            if (con.State == System.Data.ConnectionState.Closed)
            {
                con.Open();
            }
            command.CommandText = strQuery;
            command.ExecuteNonQuery();
            #endregion
        }
        private bool existeExcesoDeudaClienteQS(string strEmpresa, aspnet_Clientes objCliente)
        {
            bool blnRes = false;
            OdbcCommand command = new OdbcCommand();
            command.Connection = con;
            if (con.State == System.Data.ConnectionState.Closed)
            {
                con.Open();
            }
            command.CommandText = "SELECT COUNT(*) FROM MRVF" + strEmpresa + "COM.CLNED WHERE EDCLN=" + objCliente.QSID.ToString();
            int rows = (int)command.ExecuteScalar();
            if (rows > 0)
            {
                blnRes = true;
            }
            con.Close();
            command.Dispose();
            return blnRes;
        }
        private bool existeFotomontajeClienteQS(string strEmpresa, aspnet_Clientes objCliente)
        {
            bool blnRes = false;
            OdbcCommand command = new OdbcCommand();
            command.Connection = con;
            if (con.State == System.Data.ConnectionState.Closed)
            {
                con.Open();
            }
            command.CommandText = "SELECT COUNT(*) FROM MRVF" + strEmpresa + "COM.CLNFT WHERE FTCCN=" + objCliente.QSID.ToString();
            int rows = (int)command.ExecuteScalar();
            if (rows > 0)
            {
                blnRes = true;
            }
            con.Close();
            command.Dispose();
            return blnRes;
        }
        private bool existeFichaLogisticaClienteQS(string strEmpresa, aspnet_Clientes objCliente)
        {
            bool blnRes = false;
            OdbcCommand command = new OdbcCommand();
            command.Connection = con;
            if (con.State == System.Data.ConnectionState.Closed)
            {
                con.Open();
            }
            command.CommandText = "SELECT COUNT(*) FROM MRVF" + strEmpresa + "COM.CLNOB WHERE OBCDG=" + objCliente.QSID.ToString();
            int rows = (int)command.ExecuteScalar();
            if (rows > 0)
            {
                blnRes = true;
            }
            con.Close();
            command.Dispose();
            return blnRes;
        }
        private bool existeFacturaElectronicaClienteQS(string strEmpresa, aspnet_Clientes objCliente)
        {
            bool blnRes = false;
            OdbcCommand command = new OdbcCommand();
            command.Connection = con;
            if (con.State == System.Data.ConnectionState.Closed)
            {
                con.Open();
            }
            command.CommandText = "SELECT COUNT(*) FROM MRVF" + strEmpresa + "COM.CLNCX WHERE CXCDG=" + objCliente.QSID.ToString();
            int rows = (int)command.ExecuteScalar();
            if (rows > 0)
            {
                blnRes = true;
            }
            con.Close();
            command.Dispose();
            return blnRes;
        }
        private bool existeIBANClienteQS(string strEmpresa, aspnet_Clientes objCliente)
        {
            bool blnRes = false;
            OdbcCommand command = new OdbcCommand();
            command.Connection = con;
            if (con.State == System.Data.ConnectionState.Closed)
            {
                con.Open();
            }
            command.CommandText = "SELECT COUNT(*) FROM MRVF" + strEmpresa + "COM.CLNIB WHERE IBCDG=" + objCliente.QSID.ToString();
            int rows = (int)command.ExecuteScalar();
            if (rows > 0)
            {
                blnRes = true;
            }
            con.Close();
            command.Dispose();
            return blnRes;
        }
        private bool existePresuntoClienteQS(string strEmpresa, aspnet_Clientes objCliente)
        {
            bool blnRes = false;
            OdbcCommand command = new OdbcCommand();
            command.Connection = con;
            if (con.State == System.Data.ConnectionState.Closed)
            {
                con.Open();
            }
            command.CommandText = "SELECT COUNT(*) FROM MRVBQSF" + strEmpresa + ".BDGPR WHERE PRCDG=" + objCliente.QSID.ToString();
            int rows = (int)command.ExecuteScalar();
            if (rows > 0)
            {
                blnRes = true;
            }
            con.Close();
            command.Dispose();
            return blnRes;
        }
        public bool existeClienteEnEmpresaQS(string strEmpresa, decimal? QSID)
        {
            bool blnRes = true;
            string strQuery = "SELECT CLCDG FROM MRVF" + strEmpresa + "COM.CLNCL WHERE CLCDG=" + QSID.ToString() + "";
            OdbcCommand command = new OdbcCommand();
            con.Open();
            command.Connection = con;
            command.CommandText = strQuery;
            var aux = command.ExecuteScalar();
            con.Close();
            command.Dispose();

            if (aux == null)
            {
                blnRes = false;
            }
            return blnRes;
        }
        public decimal getIDCliente(string strEmpresas, string strNIF, bool blnEsDeExposicion)
        {
            decimal decIDCliente=0;

            //Buscamos el cliente en QS
            foreach (aspnet_Empresas objEmpresa in empresas)
            {
                decIDCliente = getIDClienteQSByNIF(objEmpresa.QSID.ToString(),strNIF);
                if (decIDCliente != 0)
                {
                    if (strEmpresas.Contains(objEmpresa.QSID))
                    {
                        //Si la empresa en la que lo hemos encontrado se encuentra entre las que se quiere cursar el alta, desbloqueamos el cliente.
                        //si no, lo dejamos bloqueado ya que se creará en una nueva empresa y es posible que no se quiera desbloquear para esta.
                        desbloquearClienteQS(objEmpresa.QSID, decIDCliente);
                    }
                    break;
                }                
            }

            //Si en este punto aún no hemos encontrado el cliente lo buscamos en la BD de Clientes por si estuviese ahí creado.
            if (decIDCliente == 0)
            {
                CLIENTES_DAR_DE_ALTA_EN_MAC auxCliente = dbClientes.CLIENTES_DAR_DE_ALTA_EN_MAC.FirstOrDefault(c => c.CIF_CORREGIDO == strNIF);

                if (auxCliente != null)
                {
                    decIDCliente = auxCliente.NUEVO_CODIGO;
                }                
            }

            //Si aún no hemos econtrado el cliente, cogemos un código nuevo.
            if (decIDCliente == 0)
            {
                if (!blnEsDeExposicion)
                {
                    //Cliente no es de exposición
                    decIDCliente = dbClientes.CLIENTES_DAR_DE_ALTA_EN_MAC.Where(c => c.NUEVO_CODIGO < 60000).Max(c => c.NUEVO_CODIGO)+1;
                }
                else
                {
                    //Cliente de exposición
                    decIDCliente = dbClientes.CLIENTES_DAR_DE_ALTA_EN_MAC.Where(c => c.NUEVO_CODIGO >= 60000).Max(c => c.NUEVO_CODIGO)+1;
                }
                bool blnGrabado = false;
                int intIntentos = 1;
                while (blnGrabado == false && intIntentos<=5)
                {
                    CLIENTES_DAR_DE_ALTA_EN_MAC auxCliente = new CLIENTES_DAR_DE_ALTA_EN_MAC();
                    try
                    {
                        //Guardamos el registro para evitar dar el mismo código a dos usuarios.
                        auxCliente.NUEVO_CODIGO = decIDCliente;
                        dbClientes.AddToCLIENTES_DAR_DE_ALTA_EN_MAC(auxCliente);
                        dbClientes.SaveChanges();
                        blnGrabado = true;
                    }
                    catch (Exception ex)
                    {
                        //Si ha dado error al guardar, borramos la entidad añadida y probamos con un código superior.
                        dbClientes.DeleteObject(auxCliente);
                        decIDCliente++;
                        intIntentos++;
                    }
                }
                if (!blnGrabado)
                {
                    decIDCliente = 0;
                }
            }

            return decIDCliente;
        }
        public void desbloquearClienteQS(string strEmpresa, decimal decIDCliente)
        {
            string strQuery = "UPDATE MRVF" + strEmpresa + "COM.CLNCL SET CLBLQ='' WHERE CLCDG=" + decIDCliente.ToString();
            OdbcCommand command = new OdbcCommand();
            con.Open();
            command.Connection = con;
            command.CommandText = strQuery;
            
            var aux = command.ExecuteNonQuery();
            con.Close();
            command.Dispose();
        }
        public bool crearClienteEnQS(string strEmpresa, aspnet_Clientes objCliente)
        {
            bool blnRes = true;
            try
            {
                if (!existeClienteEnEmpresaQS(strEmpresa, (decimal)objCliente.QSID))
                {
                    blnRes = insertarClienteEnEmpresaQS(strEmpresa, objCliente);
                }
                else
                {
                    blnRes = actualizarClienteEnEmpresaQS(strEmpresa, objCliente);
                }
            }
            catch(Exception ex)
            {
                blnRes=false;
            }
            return blnRes;
        }
        public bool borrarClienteEnEmpresaQS(string strEmpresa, decimal? QSID)
        {
            bool blnRes = false;
            OdbcCommand command = new OdbcCommand();
            try
            {
                command.Connection = con;
                con.Open();
                
                //Borramos los datos del cliente
                #region TABLA CLNCL
                command.CommandText = "DELETE FROM MRVF" + strEmpresa + "COM.CLNCL WHERE CLCDG=" + QSID.ToString();
                command.ExecuteNonQuery();
                #endregion

                //Borramos los datos del fotomontaje
                #region TABLA CLNFT
                command.CommandText = "DELETE FROM MRVF" + strEmpresa + "COM.CLNFT WHERE FTCCN=" + QSID.ToString();
                command.ExecuteNonQuery();
                #endregion

                //Borramos los datos de exceso de deuda
                #region TABLA CLNED
                command.CommandText = "DELETE FROM MRVF" + strEmpresa + "COM.CLNED WHERE EDCLN=" + QSID.ToString();
                command.ExecuteNonQuery();
                #endregion

                //Borramos los datos de la ficha logística de Observaicones
                #region TABLA CLNOB
                command.CommandText = "DELETE FROM MRVF" + strEmpresa + "COM.CLNOB WHERE OBCDG=" + QSID.ToString();
                command.ExecuteNonQuery();
                #endregion

                //Borramos los datos de la factura electrónica
                #region TABLA CLNCX
                command.CommandText = "DELETE FROM MRVF" + strEmpresa + "COM.CLNCX WHERE CXCDG=" + QSID.ToString();
                command.ExecuteNonQuery();
                #endregion

                //Borramos los datos del IBAN
                #region TABLA CLNIB
                command.CommandText = "DELETE FROM MRVF" + strEmpresa + "COM.CLNIB WHERE IBCDG=" + QSID.ToString();
                command.ExecuteNonQuery();
                #endregion

                //Borramos los datos de presuntos
                #region TABLA BDGPR
                command.CommandText = "DELETE FROM MRVBQSF" + strEmpresa + ".BDGPR WHERE PRCDG=" + QSID.ToString();
                command.ExecuteNonQuery();
                #endregion
            }
            catch (Exception ex)
            {
                throw new Exception("Error al borrar el cliente " + QSID.ToString() + " de la empresa " + strEmpresa + "." + ex.Message);
            }
            finally
            {
                con.Close();
                command.Dispose();
            }
            return blnRes;
        }
        public bool setRiesgoClienteEnEmpresaQS(string strEmpresa, decimal? QSID, int intRiesgoConcedido)
        {
            bool blnRes = true;
            OdbcCommand command = new OdbcCommand();
            try
            {
                command.Connection = con;
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }
                command.CommandText = "UPDATE MRVF" + strEmpresa + "COM.CLNCL SET CLRIE=" + intRiesgoConcedido.ToString() + " WHERE CLCDG=" + QSID.ToString();
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                blnRes = false;
            }
            finally
            {
                con.Close();
                command.Dispose();
            }
            return blnRes;
        }
        #endregion

        #region FUNCIONES SQL SERVER
        #region BD CLIENTES
        public void borrarClienteEnBDClientes(aspnet_Clientes objCliente)
        {

            CLIENTES_DAR_DE_ALTA_EN_MAC deleted = dbClientes.CLIENTES_DAR_DE_ALTA_EN_MAC.FirstOrDefault(c=>c.NUEVO_CODIGO == objCliente.QSID);
            if (deleted != null)
            {
                //Borramos las relaciones en EF
                deleted.ACCIONARIADO.Clear();
                deleted.SOCIEDADES_VINCULADAS.Clear();
                deleted.INFORMES_COMERCIALES.Clear();
                deleted.PROVEEDORES_HABITUALES.Clear();
                deleted.CLIENTES_HABITUALES.Clear();

                //Borramos el cliente
                dbClientes.ObjectStateManager.ChangeObjectState(deleted, System.Data.EntityState.Deleted);
                dbClientes.DeleteObject(deleted);
                dbClientes.SaveChanges();
            }
        }
        public bool crearClienteEnBDClientes(aspnet_Clientes objCliente)
        {
            bool blnRes = false;
            string strInformeComercial = "";
            string strAlquiler = "";
            string strPropietario = "";
            try
            {
                //Asignamos los valores
                //Obtenemos los datos registro del cliente
                CLIENTES_DAR_DE_ALTA_EN_MAC objNewCli = dbClientes.CLIENTES_DAR_DE_ALTA_EN_MAC.FirstOrDefault(c => c.NUEVO_CODIGO == (decimal)objCliente.QSID);
                if (objNewCli == null)
                {
                    objNewCli = new CLIENTES_DAR_DE_ALTA_EN_MAC();
                    objNewCli.NUEVO_CODIGO = (decimal)objCliente.QSID;
                    dbClientes.AddToCLIENTES_DAR_DE_ALTA_EN_MAC(objNewCli);
                }
                else
                {
                    //Borramos todas las tablas vinculadas para llenarlas con los nuevos datos.
                    objNewCli.ACCIONARIADO.Clear();
                    objNewCli.SOCIEDADES_VINCULADAS.Clear();
                    objNewCli.INFORMES_COMERCIALES.Clear();
                    objNewCli.PROVEEDORES_HABITUALES.Clear();
                    objNewCli.CLIENTES_HABITUALES.Clear();
                    
                    dbClientes.ObjectStateManager.ChangeObjectState(objNewCli, System.Data.EntityState.Modified);
                }

                objNewCli.CIF_CORREGIDO = objCliente.NIF;
                objNewCli.NOMCLI = objCliente.Nombre;

                if (objCliente.Gerente != "" || objCliente.TieneSocios == 1)
                {
                    //Creamos un registro en Accionariado y en AccionariadoDetalle
                    ACCIONARIADO objNewAccionariado = new ACCIONARIADO();
                    objNewAccionariado.CLIENTE = (decimal)objCliente.QSID;
                    objNewAccionariado.AÑO = System.DateTime.Now.Year;
                    
                    //Creamos un registro en AccionariadoDetalle con el Gerente
                    if (objCliente.Gerente != null)
                    {
                        ACCIONARIADO_detalle objNewAccDetalle = new ACCIONARIADO_detalle();
                        dbClientes.AddToACCIONARIADO_detalle(objNewAccDetalle);
                        objNewAccDetalle.CLIENTE = (decimal)objCliente.QSID;
                        objNewAccDetalle.AÑO = System.DateTime.Now.Year;
                        objNewAccDetalle.NOMBRE = objCliente.Gerente;
                        objNewAccDetalle.texto = "GERENTE";
                        //Añadimos la entidad al ACCIONARIADO
                        objNewAccionariado.ACCIONARIADO_detalle.Add(objNewAccDetalle);
                    }

                    if (objCliente.TieneSocios == 1)
                    {
                        //Añadimos los socios
                        foreach (aspnet_Accionariado objSocio in objCliente.aspnet_Accionariado)
                        {
                            ACCIONARIADO_detalle objNewSocio = new ACCIONARIADO_detalle();
                            objNewSocio.CLIENTE = (decimal)objCliente.QSID;
                            objNewSocio.AÑO = System.DateTime.Now.Year;
                            objNewSocio.CIF = objSocio.CIF;
                            objNewSocio.NOMBRE = objSocio.Nombre;
                            objNewSocio.PORCENTAJE = objSocio.Porcentaje;
                            objNewSocio.texto = "SOCIO";
                            //Añadimos la entidad al ACCIONARIADO
                            objNewAccionariado.ACCIONARIADO_detalle.Add(objNewSocio);
                        }
                    }
                    //Añadimos la entidad al CLIENTE
                    objNewCli.ACCIONARIADO.Add(objNewAccionariado);
                }

                //Creamos un registro en SOCIEDADES_VINCULADAS con el grupo empresarial
                if (objCliente.GrupoEmpresarial != null)
                {
                    SOCIEDADES_VINCULADAS objNewSociedad = new SOCIEDADES_VINCULADAS();
                    objNewSociedad.Cliente = (decimal)objCliente.QSID;
                    objNewSociedad.Cliente_QS = (decimal)objCliente.QSID;
                    objNewSociedad.Nombre = objCliente.GrupoEmpresarial;
                    objNewSociedad.Cif = objCliente.NIF;
                    objNewSociedad.texto = "GRUPO EMPRESARIAL";
                    //Añadimos la entidad al CLIENTE
                    objNewCli.SOCIEDADES_VINCULADAS.Add(objNewSociedad);
                }

                if (objCliente.TieneEmpresasVinculadas == 1)
                {
                    //Añadimos las sociedades vinculadas.
                    foreach (aspnet_SociedadesVinculadas objSociedad in objCliente.aspnet_SociedadesVinculadas)
                    {
                        SOCIEDADES_VINCULADAS objNewSociedadV = new SOCIEDADES_VINCULADAS();
                        objNewSociedadV.Cliente = (decimal)objCliente.QSID;
                        objNewSociedadV.Nombre = objSociedad.Nombre;
                        objNewSociedadV.Cif = objSociedad.CIF;
                        //Añadimos la entidad al CLIENTE
                        objNewCli.SOCIEDADES_VINCULADAS.Add(objNewSociedadV);                        
                    }
                }

                //Añadimos los clientes habituales
                foreach (aspnet_ClientesHabituales objCliHab in objCliente.aspnet_ClientesHabituales)
                {
                    CLIENTES_HABITUALES objNewCliHab = new CLIENTES_HABITUALES();
                    objNewCliHab.Cliente = objCliente.QSID;
                    objNewCliHab.Fecha1 = objCliente.FechaVolcadoQS;
                    objNewCliHab.Cif1 = objCliHab.NIF;
                    objNewCliHab.NombreCliente1 = objCliHab.NombreCliente;
                    //Añadimos la entidad al CLIENTE
                    objNewCli.CLIENTES_HABITUALES.Add(objNewCliHab);
                }

                //Añadimos los proveedores habituales
                foreach (aspnet_ProveedoresHabituales objProvHab in objCliente.aspnet_ProveedoresHabituales)
                {
                    PROVEEDORES_HABITUALES objNewProvHab = new PROVEEDORES_HABITUALES();
                    objNewProvHab.Cliente = objCliente.QSID;
                    objNewProvHab.Fecha1 = objCliente.FechaVolcadoQS;
                    objNewProvHab.Cif1 = objProvHab.NIF;
                    objNewProvHab.NombreProveedor1 = objProvHab.NombreProveedor;
                    //Añadimos la entidad al CLIENTE
                    objNewCli.PROVEEDORES_HABITUALES.Add(objNewProvHab);
                }

                //Cargamos los datos que grabaremos en INFORMES_COMERCIALES
                #region DATOS INFORMES COMERCIALES
                //Añadimos los datos correspondientes al informe comercial
                #region INFORME COMERCIAL
                //Cargamos la Actividad
                INSAETableAdapter taAct = new INSAETableAdapter();
                DSas400.INSAEDataTable dtAct = new DSas400.INSAEDataTable();
                taAct.FillByID(dtAct, objCliente.IDActividadQS);
                strInformeComercial += "ACTIVIDAD: " + dtAct.Rows[0]["AENBR"].ToString().Trim() + Environment.NewLine;
                dtAct.Dispose();
                taAct.Dispose();

                strInformeComercial += "ANTIGÜEDAD: " + objCliente.Antguedad + Environment.NewLine;
                strInformeComercial += "CONSUMO POTENCIAL: " + objCliente.ConsumoPotencial + ", PREVISIÓN ANUAL: " + objCliente.PrevisionAnual + Environment.NewLine;
                strInformeComercial += "CONSUME: " + objCliente.Consume + Environment.NewLine;
                
                if (objCliente.FormaDePagoSolicitada != null)
                {
                    strInformeComercial += "FORMA DE PAGO SOLICITADA: " + objCliente.FormaDePagoSolicitada + Environment.NewLine;
                }

                //Cargamos la forma de pago
                INSFPTableAdapter taFP = new INSFPTableAdapter();
                DSas400.INSFPDataTable dtFP = new DSas400.INSFPDataTable();
                taFP.FillByID(dtFP, (decimal)objCliente.FormaDePago);
                strInformeComercial += "FORMA DE PAGO: " + dtFP.Rows[0]["FPNBR"].ToString().Trim() + " " + dtFP.Rows[0]["FPNBR2"].ToString().Trim() + Environment.NewLine;
                dtFP.Dispose();
                taFP.Dispose();
                
                if (objCliente.NoTieneVtosFijos == true)
                {
                    strInformeComercial += "VTOS. FIJOS: NO TIENE" + Environment.NewLine;
                }
                else
                {
                    strInformeComercial += "VTOS. FIJOS: " + objCliente.DiaVtoFijo1 + (objCliente.DiaVtoFijo2 != null ? " - " + objCliente.DiaVtoFijo2 : "") + (objCliente.DiaVtoFijo3 != null ? " - " + objCliente.DiaVtoFijo3 : "") + Environment.NewLine;
                }

                strInformeComercial += "LÍMITE PROPUESTO: " + objCliente.LimitePropuesto.ToString() + Environment.NewLine;

                if (objCliente.NumTrabajadores != null)
                {
                    strInformeComercial += "N. TRABAJADORES: " + objCliente.NumTrabajadores + Environment.NewLine;
                }

                if (objCliente.Contacto != null)
                {
                    strInformeComercial += "PERSONA DE CONTACTO: " + objCliente.Contacto + Environment.NewLine;
                }

                //Añadimos la información de los bancos
                if (objCliente.aspnet_BancosCliente.Count > 0)
                {
                    strInformeComercial += "BANCOS CON LOS QUE TRABAJA:" + Environment.NewLine;
                    foreach (aspnet_BancosCliente objBanco in objCliente.aspnet_BancosCliente)
                    {
                        strInformeComercial += "\t - Banco: " + objBanco.Nombre + " Sucursal:" + objBanco.Sucursal + Environment.NewLine;
                    }
                }

                if (objCliente.Observaciones != null)
                {
                    strInformeComercial += "OBSERVACIONES: " + objCliente.Observaciones + Environment.NewLine;
                }
                #endregion

                //Añadimos los datos de elementos de alquiler
                #region ALQUILER
                if (objCliente.OficinasA != null || objCliente.NavesA != null || objCliente.TerrenosA != null || objCliente.VehiculosA != null || objCliente.MaquinariaA != null)
                {
                    strAlquiler = "ALQUILER:" + Environment.NewLine;

                    if (objCliente.OficinasA != null)
                    {
                        strAlquiler += "\t -" + objCliente.OficinasA + " m2 de oficinas." + Environment.NewLine;
                    }
                    if (objCliente.NavesA != null)
                    {
                        strAlquiler += "\t -" + objCliente.NavesA + " m2 de naves." + Environment.NewLine;
                    }
                    if (objCliente.TerrenosA != null)
                    {
                        strAlquiler += "\t -" + objCliente.TerrenosA + " m2 de terrenos." + Environment.NewLine;
                    }
                    if (objCliente.VehiculosA != null)
                    {
                        strAlquiler += "\t -" + objCliente.VehiculosA + " vehículos." + Environment.NewLine;
                    }
                    if (objCliente.MaquinariaA != null)
                    {
                        strAlquiler += "\t -" + objCliente.MaquinariaA + " maquinas." + Environment.NewLine;
                    }
                }
                #endregion

                //Añadimos los datos de elementos en propiedad
                #region PROPIEDAD
                if (objCliente.OficinasP != null || objCliente.NavesP != null || objCliente.TerrenosP != null || objCliente.VehiculosP != null || objCliente.MaquinariaP != null)
                {
                    strPropietario = "PROPIEDAD:" + Environment.NewLine;

                    if (objCliente.OficinasP != null)
                    {
                        strPropietario += "\t -" + objCliente.OficinasP + " m2 de oficinas." + Environment.NewLine;
                    }
                    if (objCliente.NavesP != null)
                    {
                        strPropietario += "\t -" + objCliente.NavesP + " m2 de naves." + Environment.NewLine;
                    }
                    if (objCliente.TerrenosP != null)
                    {
                        strPropietario += "\t -" + objCliente.TerrenosP + " m2 de terrenos." + Environment.NewLine;
                    }
                    if (objCliente.VehiculosP != null)
                    {
                        strPropietario += "\t -" + objCliente.VehiculosP + " vehículos." + Environment.NewLine;
                    }
                    if (objCliente.MaquinariaP != null)
                    {
                        strPropietario += "\t -" + objCliente.MaquinariaP + " maquinas." + Environment.NewLine;
                    }
                }
                #endregion

                //Volcamos la información a la BD
                #region VOLCADO A BD
                INFORMES_COMERCIALES objNewInforme = new INFORMES_COMERCIALES();
                objNewInforme.CLIENTE = objCliente.QSID;
                objNewInforme.FECHA = System.DateTime.Now;
                objNewInforme.VALIDAR = true;
                
                //Cargamos el agente asociado al cliente y asignamos su nombre en el campo persona.
                if (objCliente.IDAgenteQSMV != null && objCliente.Empresas.Contains("003"))
                {
                    AGTAG003TableAdapter ta=new AGTAG003TableAdapter();
                    DSas400.AGTAG003DataTable dt = new DSas400.AGTAG003DataTable();
                    ta.FillByID(dt, (decimal)objCliente.IDAgenteQSMV);
                    objNewInforme.PERSONA = dt.Rows[0]["AGNBR"].ToString().Substring(0, 50).Trim();
                    ta.Dispose();
                    dt.Dispose();
                }
                else if (objCliente.IDAgenteQSHMA != null && objCliente.Empresas.Contains("004"))
                {
                    AGTAG004TableAdapter ta = new AGTAG004TableAdapter();
                    DSas400.AGTAG004DataTable dt = new DSas400.AGTAG004DataTable();
                    ta.FillByID(dt, (decimal)objCliente.IDAgenteQSMV);
                    objNewInforme.PERSONA = dt.Rows[0]["AGNBR"].ToString().Substring(1, 50).Trim();
                    ta.Dispose();
                    dt.Dispose();
                }
                else if (objCliente.IDAgenteQSECA != null && objCliente.Empresas.Contains("006"))
                {
                    AGTAG006TableAdapter ta = new AGTAG006TableAdapter();
                    DSas400.AGTAG006DataTable dt = new DSas400.AGTAG006DataTable();
                    ta.FillByID(dt, (decimal)objCliente.IDAgenteQSMV);
                    objNewInforme.PERSONA = dt.Rows[0]["AGNBR"].ToString().Substring(1, 50);
                    ta.Dispose();
                    dt.Dispose();
                }

                objNewInforme.TEXTO = strInformeComercial;
                objNewInforme.INMUEBLES = strAlquiler;
                objNewInforme.PROPIETARIO = strPropietario;

                if (objCliente.CompañiaSeguroVentas != null)
                {
                    objNewInforme.ASEGURA_VENTAS = true;
                    objNewInforme.NO_ASEGURA_VENTAS = false;
                    objNewInforme.ASEGURADORA = objCliente.CompañiaSeguroVentas;
                }
                else
                {
                    objNewInforme.ASEGURA_VENTAS = false;
                    objNewInforme.NO_ASEGURA_VENTAS = true;
                }
                //Añadimos la entidad al CLIENTE
                objNewCli.INFORMES_COMERCIALES.Add(objNewInforme);
                #endregion

                #endregion

                //Grabamos los datos a la BD
                dbClientes.SaveChanges();
                blnRes = true;
            }
            catch (Exception ex)
            {
                blnRes = false;
            }
            return blnRes;
        }
        public bool setRiesgoClienteEnBDClientes(aspnet_Clientes objCliente)
        {
            CLIENTES_DAR_DE_ALTA_EN_MAC objCli = dbClientes.CLIENTES_DAR_DE_ALTA_EN_MAC.FirstOrDefault(c => c.NUEVO_CODIGO == objCliente.QSID);
            bool blnRes = true;
            try
            {
                if (objCli != null)
                {
                    objCli.limiteMV = objCliente.LimiteAsignadoMV;
                    objCli.limiteHMA = objCliente.LimiteAsignadoHMA;
                    objCli.limiteECA = objCliente.LimiteAsignadoECA;
                    dbClientes.ObjectStateManager.ChangeObjectState(objCli, System.Data.EntityState.Modified);
                    dbClientes.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                blnRes = false;
            }
            return blnRes;
        }
        #endregion
        #region BD PLANIFICACIONTRANSPORTES
        public bool crearExcepcionesClienteEnBDPlanificacion(int intIDDelegacion, aspnet_Clientes objCliente)
        {
            bool blnRes = true;
            try
            {
                Excepciones_cliente excCli = dbPlanificacion.Excepciones_cliente.FirstOrDefault(exp => exp.codigoQS == objCliente.QSID);

                if (excCli == null)
                {
                    excCli = new Excepciones_cliente();
                    dbPlanificacion.AddToExcepciones_cliente(excCli);
                }
                else
                {
                    dbPlanificacion.ObjectStateManager.ChangeObjectState(excCli, System.Data.EntityState.Modified);
                }

                excCli.DELEGACION = intIDDelegacion;
                excCli.codigoQS = Convert.ToInt32(objCliente.QSID);
                excCli.cliente = objCliente.Nombre;
                excCli.horario = objCliente.Horario;
                excCli.horarioVerano = objCliente.HorarioDeVerano;
                excCli.camionConPluma = objCliente.NecesitaCamionConPluma;
                excCli.tipoCamion = Convert.ToInt16(objCliente.IDTipoVehiculoServicio);
                excCli.medioDescarga = Convert.ToInt16(objCliente.IDMedioDeDescarga);

                dbPlanificacion.SaveChanges();
            }
            catch (Exception ex)
            {
                blnRes = false;
            }
            return blnRes;
        }
        #endregion
        #endregion
    }
}