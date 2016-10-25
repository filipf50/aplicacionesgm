using System;
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
        private bool insertarClienteEnEmpresaQS(string strEmpresa, aspnet_Clientes objCliente, ref string errMessage)
        {
            bool blnRes = true;
            OdbcCommand command = new OdbcCommand();
            try
            {
                //Ficha del cliente
                #region TABLA CLNCL
                command.Parameters.Clear();
                string strQuery = "INSERT INTO MRVF" + strEmpresa + "COM.CLNCL(CLCDG,CLNIF,CLNBR,CLSGL,CLVIA,CLNMR,CLAMP,CLMNC,CLCDP,CLPAI,CLPRV,CLAG1,CLTF1,CLTF2,CLTFX,CLURL,CLACT,CLTCL,CLTTR,CLFPG,CLDTPP,CLRCG,CLDF1,CLDF2,CLDF3,CLZON,CLFALC,CLRGO,CLRGOP,CLBLQ,CLTAL,CLTIPT,CLDIV,CLDIVC,CLCCN,CLRCO,CLMAIL) ";
                strQuery += "VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";
                command.Parameters.Add("CLCDG", OdbcType.Numeric, 10).Value = objCliente.QSID;
                command.Parameters.Add("CLNIF", OdbcType.Char, 15).Value = objCliente.NIF.ToString();
                command.Parameters.Add("CLNBR", OdbcType.Char, 30).Value = objCliente.Nombre.ToString();
                command.Parameters.Add("CLSGL", OdbcType.Char, 2).Value = objCliente.TipoDeVia.ToString();
                command.Parameters.Add("CLVIA", OdbcType.Char, 25).Value = objCliente.Domicilio.ToString();
                command.Parameters.Add("CLNMR", OdbcType.Numeric, 5).Value = (objCliente.Numero ?? 0);
                command.Parameters.Add("CLAMP", OdbcType.Char, 15).Value = (objCliente.Piso ?? "").ToString();
                command.Parameters.Add("CLMNC", OdbcType.Char, 25).Value = objCliente.Municipio.ToString();
                command.Parameters.Add("CLCDP", OdbcType.Numeric, 5).Value = objCliente.CP;
                command.Parameters.Add("CLPAI", OdbcType.Numeric, 3).Value = objCliente.IDPaisQS;
                command.Parameters.Add("CLPRV", OdbcType.Numeric, 2).Value = objCliente.IDProvinciaQS;
                command.Parameters.Add("CLAG1", OdbcType.Numeric, 10).Value = (strEmpresa == "003" || strEmpresa == "033" ? (objCliente.IDAgenteQSMV ?? 0) : (strEmpresa == "004" || strEmpresa == "044" ? (objCliente.IDAgenteQSHMA ?? 0) : (strEmpresa == "006") ? (objCliente.IDAgenteQSECA ?? 0) : ((objCliente.IDAgenteQSHMA ?? 0) != 0 ? (objCliente.IDAgenteQSHMA ?? 0) : (objCliente.IDAgenteQSMV ?? 0))));
                command.Parameters.Add("CLTF1", OdbcType.Numeric, 10).Value = (objCliente.Telefono1 ?? 0);
                command.Parameters.Add("CLTF2", OdbcType.Numeric, 10).Value = (objCliente.Telefono2 ?? 0);
                command.Parameters.Add("CLTFX", OdbcType.Numeric, 10).Value = (objCliente.Fax ?? 0);
                command.Parameters.Add("CLURL", OdbcType.Char, 60).Value = (objCliente.Contacto ?? "").ToString() + " - " + (objCliente.Web ?? "").ToString();
                command.Parameters.Add("CLACT", OdbcType.Char, 4).Value = (objCliente.IDActividadQS ?? "").ToString();
                command.Parameters.Add("CLTCL", OdbcType.Char, 1).Value = (objCliente.TipoCliente ?? "").ToString();
                command.Parameters.Add("CLTTR", OdbcType.Numeric, 2).Value = ((objCliente.Tarifa ?? 0));
                command.Parameters.Add("CLFPG", OdbcType.Numeric, 2).Value = objCliente.FormaDePago;
                command.Parameters.Add("CLDTPP", OdbcType.Numeric).Value = (objCliente.DtoPP ?? 0);
                command.Parameters.Add("CLRCG", OdbcType.Numeric).Value = (objCliente.RecargoFinanciero ?? 0);
                command.Parameters.Add("CLDF1", OdbcType.Numeric, 2).Value = (objCliente.DiaVtoFijo1 ?? 0);
                command.Parameters.Add("CLDF2", OdbcType.Numeric, 2).Value = (objCliente.DiaVtoFijo2 ?? 0);
                command.Parameters.Add("CLDF3", OdbcType.Numeric, 2).Value = (objCliente.DiaVtoFijo3 ?? 0);
                command.Parameters.Add("CLZON", OdbcType.Numeric, 5).Value = (objCliente.Zona ?? 0);
                command.Parameters.Add("CLFALC", OdbcType.Char, 1).Value = objCliente.FechaVolcadoQS.Value.ToString("yyyyMMdd");
                
                //Valores por defecto
                if (objCliente.aspnet_FormasDePago.RequiereDocSEPA)
                {
                    command.Parameters.Add("CLRGO", OdbcType.Char, 1).Value = "R"; //Riesgo Venta
                    command.Parameters.Add("CLRGOP", OdbcType.Char, 1).Value = "R"; //Riesgo Pedido
                    command.Parameters.Add("CLBLQ", OdbcType.Char, 1).Value = "S"; //Bloqueado
                }
                else
                {
                    command.Parameters.Add("CLRGO", OdbcType.Char, 1).Value = "A"; //Riesgo Venta
                    command.Parameters.Add("CLRGOP", OdbcType.Char, 1).Value = "A"; //Riesgo Pedido
                    command.Parameters.Add("CLBLQ", OdbcType.Char, 1).Value = "N"; //Bloqueado
                }
                command.Parameters.Add("CLTAL", OdbcType.Char, 1).Value = "I"; //Tipo de Albarán
                command.Parameters.Add("CLTIPT", OdbcType.Char, 1).Value = "E"; //Tipo de tarifa
                command.Parameters.Add("CLDIV", OdbcType.Char, 3).Value = "EUR"; //Divisa
                command.Parameters.Add("CLDIVC", OdbcType.Char, 3).Value = "ESP"; //Divisa cambio
                command.Parameters.Add("CLCCN", OdbcType.Numeric, 10).Value = getCuentaContableClienteQS(objCliente); //Cuenta contable;
                command.Parameters.Add("CLRCO", OdbcType.Numeric, 10).Value = (strEmpresa == "003" || strEmpresa == "033" ? (objCliente.IDAgenteQSMV ?? 0) : (strEmpresa == "004" || strEmpresa == "044" ? (objCliente.IDAgenteQSHMA ?? 0) : (strEmpresa == "006") ? (objCliente.IDAgenteQSECA ?? 0) : ((objCliente.IDAgenteQSHMA ?? 0) != 0 ? (objCliente.IDAgenteQSHMA ?? 0) : (objCliente.IDAgenteQSMV ?? 0))));
                command.Parameters.Add("CLMAIL", OdbcType.Char, 40).Value = (objCliente.TieneMail ? (objCliente.MailDeContacto.ToString() ?? "") : "NO TIENE");

                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }
                command.Connection = con;
                command.CommandText = strQuery;

                var aux = command.ExecuteNonQuery();
                #endregion CLNCL

                //Actualizar tablas auxiliares
                if (!actualizarTablasAuxiliaresClienteEnQS(strEmpresa, objCliente, ref errMessage))
                {
                    blnRes = false;
                }
                else
                {                
                    //Actualizamos los datos contables (Función F7 del Mto. de clientes de QS)
                    blnRes=actualizarDatosContablesClienteEnQS(strEmpresa, objCliente, ref errMessage);
                }
            }
            catch (Exception ex)
            {
                blnRes = false;
                errMessage += "-Error en Models.ClientesModel.insertarClienteEnEmpresaQS(" + strEmpresa + ", " + objCliente.Nombre + "): " + ex.Message + (ex.InnerException != null ? ex.InnerException.ToString() : "");
            }
            finally
            {
                con.Close();
                command.Dispose();
            }
            return blnRes;
        }
        private bool actualizarClienteEnEmpresaQS(string strEmpresa, aspnet_Clientes objCliente,ref string errMessage)
        {
            bool blnRes = true;
            OdbcCommand command = new OdbcCommand();
            try
            {
                //Actualizamos los datos de la ficha del cliente
                #region TABLA CLNCL
                string strQuery = "UPDATE MRVF" + strEmpresa + "COM.CLNCL SET CLNIF=?, CLNBR=?, CLSGL=?, CLVIA=?, CLNMR=?, CLAMP=?, CLMNC=?, CLCDP=?, CLPAI=?, CLPRV=?, CLAG1=?, CLTF1=?, CLTF2=?, CLTFX=?, CLURL=?, CLACT=?, CLTCL=?, CLTTR=?, CLFPG=?, CLDTPP=?, CLRCG=?, CLDF1=?, CLDF2=?, CLDF3=?, CLZON=?, CLFALC=?, ";
                strQuery += "CLRGO=?, CLRGOP=?, CLBLQ=?, CLTAL=?, CLTIPT=?, CLDIV=?, CLDIVC=?, CLRCO=?, CLMAIL=?, CLSWB=? WHERE CLCDG=?";
                command.Parameters.Add("CLNIF", OdbcType.Char, 15).Value = objCliente.NIF.ToString();
                command.Parameters.Add("CLNBR", OdbcType.Char, 30).Value = objCliente.Nombre.ToString();
                command.Parameters.Add("CLSGL", OdbcType.Char, 2).Value = objCliente.TipoDeVia.ToString();
                command.Parameters.Add("CLVIA", OdbcType.Char, 25).Value = objCliente.Domicilio.ToString();
                command.Parameters.Add("CLNMR", OdbcType.Numeric, 5).Value = (objCliente.Numero ?? 0);
                command.Parameters.Add("CLAMP", OdbcType.Char, 15).Value = (objCliente.Piso ?? "").ToString();
                command.Parameters.Add("CLMNC", OdbcType.Char, 25).Value = objCliente.Municipio.ToString();
                command.Parameters.Add("CLCDP", OdbcType.Numeric, 5).Value = objCliente.CP;
                command.Parameters.Add("CLPAI", OdbcType.Numeric, 3).Value = objCliente.IDPaisQS;
                command.Parameters.Add("CLPRV", OdbcType.Numeric, 2).Value = objCliente.IDProvinciaQS;
                command.Parameters.Add("CLAG1", OdbcType.Numeric, 10).Value = (strEmpresa == "003" || strEmpresa == "033" ? (objCliente.IDAgenteQSMV ?? 0) : (strEmpresa == "004" || strEmpresa == "044" ? (objCliente.IDAgenteQSHMA ?? 0) : (strEmpresa == "006") ? (objCliente.IDAgenteQSECA ?? 0) : ((objCliente.IDAgenteQSHMA ?? 0) != 0 ? (objCliente.IDAgenteQSHMA ?? 0) : (objCliente.IDAgenteQSMV ?? 0))));
                command.Parameters.Add("CLTF1", OdbcType.Numeric, 10).Value = (objCliente.Telefono1 ?? 0);
                command.Parameters.Add("CLTF2", OdbcType.Numeric, 10).Value = (objCliente.Telefono2 ?? 0);
                command.Parameters.Add("CLTFX", OdbcType.Numeric, 10).Value = (objCliente.Fax ?? 0);
                command.Parameters.Add("CLURL", OdbcType.Char, 60).Value = (objCliente.Contacto ?? "").ToString() + " - " + (objCliente.Web ?? "").ToString();
                command.Parameters.Add("CLACT", OdbcType.Char, 4).Value = (objCliente.IDActividadQS ?? "").ToString();
                command.Parameters.Add("CLTCL", OdbcType.Char, 1).Value = (objCliente.TipoCliente ?? "").ToString();
                command.Parameters.Add("CLTTR", OdbcType.Numeric, 2).Value = ((objCliente.Tarifa ?? 0));
                command.Parameters.Add("CLFPG", OdbcType.Numeric, 2).Value = objCliente.FormaDePago;
                command.Parameters.Add("CLDTPP", OdbcType.Numeric).Value = (objCliente.DtoPP ?? 0);
                command.Parameters.Add("CLRCG", OdbcType.Numeric).Value = (objCliente.RecargoFinanciero ?? 0);
                command.Parameters.Add("CLDF1", OdbcType.Numeric, 2).Value = (objCliente.DiaVtoFijo1 ?? 0);
                command.Parameters.Add("CLDF2", OdbcType.Numeric, 2).Value = (objCliente.DiaVtoFijo2 ?? 0);
                command.Parameters.Add("CLDF3", OdbcType.Numeric, 2).Value = (objCliente.DiaVtoFijo3 ?? 0);
                command.Parameters.Add("CLZON", OdbcType.Numeric, 5).Value = (objCliente.Zona ?? 0);
                command.Parameters.Add("CLFALC", OdbcType.Char, 1).Value = objCliente.FechaVolcadoQS.Value.ToString("yyyyMMdd");

                //Valores por defecto
                if (objCliente.aspnet_FormasDePago.RequiereDocSEPA)
                {
                    command.Parameters.Add("CLRGO", OdbcType.Char, 1).Value = "R"; //Riesgo Venta
                    command.Parameters.Add("CLRGOP", OdbcType.Char, 1).Value = "R"; //Riesgo Pedido
                    command.Parameters.Add("CLBLQ", OdbcType.Char, 1).Value = "S"; //Bloqueado
                }
                else
                {
                    command.Parameters.Add("CLRGO", OdbcType.Char, 1).Value = "A"; //Riesgo Venta
                    command.Parameters.Add("CLRGOP", OdbcType.Char, 1).Value = "A"; //Riesgo Pedido
                    command.Parameters.Add("CLBLQ", OdbcType.Char, 1).Value = "N"; //Bloqueado
                }
                command.Parameters.Add("CLTAL", OdbcType.Char, 1).Value = "I"; //Tipo de Albarán
                command.Parameters.Add("CLTIPT", OdbcType.Char, 1).Value = "E"; //Tipo de tarifa
                command.Parameters.Add("CLDIV", OdbcType.Char, 3).Value = "EUR"; //Divisa
                command.Parameters.Add("CLDIVC", OdbcType.Char, 3).Value = "ESP"; //Divisa cambio
                command.Parameters.Add("CLRCO", OdbcType.Numeric, 10).Value = (strEmpresa == "003" || strEmpresa == "033" ? (objCliente.IDAgenteQSMV ?? 0) : (strEmpresa == "004" || strEmpresa == "044" ? (objCliente.IDAgenteQSHMA ?? 0) : (strEmpresa == "006") ? (objCliente.IDAgenteQSECA ?? 0) : ((objCliente.IDAgenteQSHMA ?? 0) != 0 ? (objCliente.IDAgenteQSHMA ?? 0) : (objCliente.IDAgenteQSMV ?? 0))));
                command.Parameters.Add("CLMAIL", OdbcType.Char, 40).Value = (objCliente.TieneMail ? (objCliente.MailDeContacto.ToString() ?? "") : "NO TIENE");
                command.Parameters.Add("CLSWB", OdbcType.Char, 1).Value = ""; //Switch de baja
                command.Parameters.Add("CLCDG", OdbcType.Numeric, 10).Value = objCliente.QSID;
                
                command.Connection = con;
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }
                
                command.CommandText = strQuery;
                var aux = command.ExecuteNonQuery();
                #endregion

                //Actualizar tablas auxiliares
                //actualizarTablasAuxiliaresClienteEnQS(strEmpresa, objCliente,ref errMessage);
                if (!actualizarTablasAuxiliaresClienteEnQS(strEmpresa, objCliente, ref errMessage))
                {
                    blnRes = false;
                }
                else
                {
                    //Actualizamos los datos contables (Función F7 del Mto. de clientes de QS)
                    blnRes = actualizarDatosContablesClienteEnQS(strEmpresa, objCliente, ref errMessage);
                }




            }
            catch (Exception ex)
            {
                blnRes = false;
                errMessage += "-Error en Models.ClientesModel.actualizarClienteEnEmpresaQS(" + strEmpresa + ", " + objCliente.Nombre + "): " + ex.Message;
            }
            finally
            {
                con.Close();
                command.Dispose();
            }
            return blnRes;
        }
        private bool actualizarTablasAuxiliaresClienteEnQS(string strEmpresa, aspnet_Clientes objCliente, ref string errMessage)
        {
            OdbcCommand command = new OdbcCommand();
            bool blnRes = true;
            string strQuery = "";
            int i = 0;
            command.Connection = con;
            try
            {
                //Añadir registro en CLNCM con el código de municipio
                #region TABLA CLNCM
                command.Parameters.Clear();

                if (!existeMunicipioClienteQS(strEmpresa, objCliente))
                {
                    strQuery = "INSERT INTO MRVF" + strEmpresa + "COM.CLNCM (CMCLN, CMMUN) VALUES (?,?)";
                    command.Parameters.Add("ID", OdbcType.Numeric, 10).Value = objCliente.QSID;
                }
                else
                {
                    strQuery = "UPDATE MRVF" + strEmpresa + "COM.CLNCM SET CMMUN=? WHERE CMCLN=?";
                }

                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }
                command.CommandText = strQuery;
                command.Parameters.Add("MUN", OdbcType.Numeric, 4).Value = objCliente.IDMunicipioQS;

                if (strQuery.Contains("UPDATE"))
                {
                    command.Parameters.Add("ID", OdbcType.Numeric, 10).Value = objCliente.QSID;
                }

                command.ExecuteNonQuery();
                #endregion

                //Añadimos el control para exceso de deuda
                #region TABLA CLNED
                command.Parameters.Clear();
                if (!existeExcesoDeudaClienteQS(strEmpresa, objCliente))
                {
                    strQuery = "INSERT INTO MRVF" + strEmpresa + "COM.CLNED (EDCLN,EDEDD) VALUES (?,?)";
                    command.Parameters.Add("ID", OdbcType.Numeric, 10).Value = objCliente.QSID;
                }
                else
                {
                    strQuery = "UPDATE MRVF" + strEmpresa + "COM.CLNED SET EDEDD=? WHERE EDCLN=?";
                }
                command.CommandText = strQuery;
                command.Parameters.Add("VAL", OdbcType.Char, 1).Value = "S";

                if (strQuery.Contains("UPDATE"))
                {
                    command.Parameters.Add("ID", OdbcType.Numeric, 10).Value = objCliente.QSID;
                }

                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }
                command.ExecuteNonQuery();

                #endregion

                //Gestionamos los fotomontajes
                #region TABLA CLNAI
                string strFotoMontaje = "N";
                if ((objCliente.IDActividadQS ?? "") == "CP" || (objCliente.IDActividadQS ?? "") == "C" || (objCliente.IDActividadQS ?? "") == "PR")
                {
                    //Si la actividad del cliente es CONTRUCTOR-PROMOTOR, CONSTRUCTOR O PROMOTOR SE OBLIGA A QUE TENGA FOTOMONTAJE
                    strFotoMontaje = "S";
                }

                if (!existeFotomontajeAnexoClienteQS(strEmpresa, objCliente))
                {
                    strQuery = "INSERT INTO MRVF" + strEmpresa + "COM.CLNAI (AICDG,AIFTM) VALUES (?,?) ";
                    command.Parameters.Clear();
                    command.Parameters.Add("ID", OdbcType.Numeric, 10).Value = objCliente.QSID;
                    command.Parameters.Add("VAL", OdbcType.Char, 1).Value = strFotoMontaje;
                }
                else
                {
                    strQuery = "UPDATE MRVF" + strEmpresa + "COM.CLNAI SET AIFTM=? WHERE AICDG=?";
                    command.Parameters.Clear();
                    command.Parameters.Add("VAL", OdbcType.Char, 1).Value = strFotoMontaje;
                    command.Parameters.Add("ID", OdbcType.Numeric, 10).Value = objCliente.QSID;
                }
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }
                command.CommandText = strQuery;
                command.ExecuteNonQuery();
                #endregion
                #region TABLA CLNFT
                if (!existeFotomontajeClienteQS(strEmpresa, objCliente))
                {
                    strQuery = "INSERT INTO MRVF" + strEmpresa + "COM.CLNFT (FTCCN,FTFOT) VALUES (?,?)";
                    command.Parameters.Clear();
                    command.Parameters.Add("ID", OdbcType.Numeric, 10).Value = objCliente.QSID;
                    command.Parameters.Add("VAL", OdbcType.Char, 1).Value = strFotoMontaje;
                }
                else
                {
                    strQuery = "UPDATE MRVF" + strEmpresa + "COM.CLNFT SET FTFOT=? WHERE FTCCN=?";
                    command.Parameters.Clear();
                    command.Parameters.Add("VAL", OdbcType.Char, 1).Value = strFotoMontaje;
                    command.Parameters.Add("ID", OdbcType.Numeric, 10).Value = objCliente.QSID;
                }

                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }
                command.CommandText = strQuery;
                command.ExecuteNonQuery();
                #endregion

                //Añadimos los datos de Observaciones con los datos de la FICHA LOGÍSTICA
                #region TABLA CLNOB
                string strAutorizadasRetMat = "";
                if (!objCliente.EsDeExposicion || !objCliente.EsClienteParticular || !objCliente.RecogeEnNuestrasInstalaciones)
                {
                    command.Parameters.Clear();
                    if (!existeFichaLogisticaClienteQS(strEmpresa, objCliente))
                    {
                        strQuery = "INSERT INTO MRVF" + strEmpresa + "COM.CLNOB (OBCDG,OBPM1,OBPM2,OBPM3,OBPM4,OBPM5,OBPM6,OBPM7,OBPO1,OBPO2,OBPO3,OBAM1,OBAM2,OBAM3,OBAM4,OBAM5,OBAM6,OBAM7,OBAE1,OBAE2,OBAE3,OBFB1,OBFB2,OBFB3) ";
                        strQuery += "VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";
                        command.Parameters.Add("OBCDG", OdbcType.Numeric, 10).Value = objCliente.QSID;
                    }
                    else
                    {
                        strQuery = "UPDATE MRVF" + strEmpresa + "COM.CLNOB SET OBPM1=?,OBPM2=?,OBPM3=?,OBPM4=?,OBPM5=?,OBPM6=?,OBPM7=?,OBPO1=?,OBPO2=?,OBPO3=?,OBAM1=?,OBAM2=?,OBAM3=?,OBAM4=?,OBAM5=?,OBAM6=?,OBAM7=?,OBAE1=?,OBAE2=?,OBAE3=?,OBFB1=?,OBFB2=?,OBFB3=? WHERE OBCDG=?";
                    }
                    //Añadimos los parámetros
                    #region PARAMETROS
                    command.Parameters.Add("OBPM1", OdbcType.Char, 60).Value = ("HORARIO: " + (objCliente.Horario ?? "").ToString()).ToUpper(); //OBPM1
                    command.Parameters.Add("OBPM2", OdbcType.Char, 60).Value = ("HORARIO VERANO: " + (objCliente.HorarioDeVerano ?? "").ToString()).ToUpper(); //OBPM2
                    command.Parameters.Add("OBPM3", OdbcType.Char, 60).Value = ("M.T.PROPIOS:" + (objCliente.MedioDeTransportePropio ? "SI" : "NO") + "/PORTES:" + (objCliente.CobroDePortesPorEnvio ? objCliente.ImportePortesPorEnvio.ToString() + "E" : "NO") + "/CAMION GRUA:" + (objCliente.NecesitaCamionConPluma ? "SI" : "NO") + "/VEHICULO:" + objCliente.aspnet_TiposDeVehiculo.Nombre.ToString()).ToUpper(); //OBPM3
                    command.Parameters.Add("OBPM4", OdbcType.Char, 60).Value = ("M.DESCARGA: " + (objCliente.aspnet_MediosDeDescarga.Nombre ?? "") + "/ " + (objCliente.PesaElMaterial ? "PESA CON " + objCliente.aspnet_InstrumentosDePesaje.Nombre.ToString() : "NO PESA EL MATERIAL")).ToUpper(); //OBPM4
                    command.Parameters.Add("OBPM5", OdbcType.Char, 60).Value = ("OBLIG. CERT. CALIDAD:" + ((objCliente.RequerimientosDeCalidad ?? "").Contains('2') ? "SI" : "NO") + "/PRL: " + (objCliente.RequerimientosDePrevencion ?? "").ToString().Substring(0, ((objCliente.RequerimientosDePrevencion ?? "").Length > 30 ? 30 : (objCliente.RequerimientosDePrevencion ?? "").Length))).ToUpper();//OBPM5
                    if (objCliente.aspnet_PersonasRetiradaMat.Count > 0)
                    {
                        //Si tenemos personas autorizadas para retirada de material las añadimos en los campos OBPM6 Y OBPM7
                        strAutorizadasRetMat = "AUT.RECOGIDAS:";
                        i = 6;
                        foreach (var persona in objCliente.aspnet_PersonasRetiradaMat)
                        {
                            command.Parameters.Add("OBPM" + i, OdbcType.Char, 60).Value = (strAutorizadasRetMat + persona.NIF.ToString() + " " + persona.Nombre.ToString()).ToUpper(); //OBPM6 y 7
                            i++;
                            strAutorizadasRetMat = "";
                        }
                        if (objCliente.aspnet_PersonasRetiradaMat.Count == 1)
                        {
                            command.Parameters.Add("OBPM7", OdbcType.Char, 60).Value = "";
                        }
                    }
                    else
                    {
                        //Si no tenemos personas autorizadas para retirada de material, dejamos los campos OBPM6 Y OBPM7 vacíos.
                        command.Parameters.Add("OBPM6", OdbcType.Char, 60).Value = "";
                        command.Parameters.Add("OBPM7", OdbcType.Char, 60).Value = "";
                    }
                    command.Parameters.Add("OBPO1", OdbcType.Char, 50).Value = ("HORARIO: " + (objCliente.Horario ?? "").ToString()).ToUpper(); //OBPO1
                    command.Parameters.Add("OBPO2", OdbcType.Char, 50).Value = ("HORARIO VERANO: " + (objCliente.HorarioDeVerano ?? "").ToString()).ToUpper(); //OBPO2
                    command.Parameters.Add("OBPO3", OdbcType.Char, 50).Value = ("CONTACTO DESCARGA:" + ((objCliente.PersonaDeDescarga ?? "").Length > 30 ? (objCliente.PersonaDeDescarga ?? "").ToString().Substring(0, 30) : (objCliente.PersonaDeDescarga ?? "").ToString())).ToUpper(); //OBPO3
                    command.Parameters.Add("OBAM1", OdbcType.Char, 60).Value = ("HORARIO: " + (objCliente.Horario ?? "").ToString()).ToUpper(); //OBAM1
                    command.Parameters.Add("OBAM2", OdbcType.Char, 60).Value = ("HORARIO VERANO: " + (objCliente.HorarioDeVerano ?? "").ToString()).ToUpper(); //OBAM2
                    command.Parameters.Add("OBAM3", OdbcType.Char, 60).Value = ("M.T.PROPIOS:" + (objCliente.MedioDeTransportePropio ? "SI" : "NO") + "/PORTES:" + (objCliente.CobroDePortesPorEnvio ? objCliente.ImportePortesPorEnvio.ToString() + "E" : "NO") + "/CAMION GRUA:" + (objCliente.NecesitaCamionConPluma ? "SI" : "NO") + "/VEHICULO:" + objCliente.aspnet_TiposDeVehiculo.Nombre.ToString()).ToUpper(); //OBAM3
                    command.Parameters.Add("OBAM4", OdbcType.Char, 60).Value = ("M.DESCARGA: " + (objCliente.aspnet_MediosDeDescarga.Nombre ?? "") + "/ " + (objCliente.PesaElMaterial ? "PESA CON " + objCliente.aspnet_InstrumentosDePesaje.Nombre.ToString() : "NO PESA EL MATERIAL")).ToUpper(); //OBAM4
                    command.Parameters.Add("OBAM5", OdbcType.Char, 60).Value = ("OBLIG. CERT. CALIDAD:" + ((objCliente.RequerimientosDeCalidad ?? "").Contains('2') ? "SI" : "NO") + "/PRL: " + (objCliente.RequerimientosDePrevencion ?? "").ToString().Substring(0, ((objCliente.RequerimientosDePrevencion ?? "").Length > 30 ? 30 : (objCliente.RequerimientosDePrevencion ?? "").Length))).ToUpper();//OBAM5
                    if (objCliente.aspnet_PersonasRetiradaMat.Count > 0)
                    {
                        //Si tenemos personas autorizadas para retirada de material las añadimos en los campos OBPM6 Y OBPM7
                        strAutorizadasRetMat = "AUT.RECOGIDAS:";
                        i = 6;
                        foreach (var persona in objCliente.aspnet_PersonasRetiradaMat)
                        {
                            command.Parameters.Add("OBAM" + i, OdbcType.Char, 60).Value = (strAutorizadasRetMat + persona.NIF.ToString() + " " + persona.Nombre.ToString()).ToUpper(); //OBAM6 y 7
                            i++;
                            strAutorizadasRetMat = "";
                        }
                        if (objCliente.aspnet_PersonasRetiradaMat.Count == 1)
                        {
                            command.Parameters.Add("OBAM", OdbcType.Char, 60).Value = "";
                        }
                    }
                    else
                    {
                        //Si no tenemos personas autorizadas para retirada de material, dejamos los campos OBPM6 Y OBPM7 vacíos.
                        command.Parameters.Add("OBAM6", OdbcType.Char, 60).Value = "";
                        command.Parameters.Add("OBAM7", OdbcType.Char, 60).Value = "";
                    }
                    command.Parameters.Add("OBAE1", OdbcType.Char, 50).Value = ("HORARIO: " + (objCliente.Horario ?? "").ToString()).ToUpper(); //OBAE1
                    command.Parameters.Add("OBAE2", OdbcType.Char, 50).Value = ("HORARIO VERANO: " + (objCliente.HorarioDeVerano ?? "").ToString()).ToUpper(); //OBAE2
                    command.Parameters.Add("OBAE3", OdbcType.Char, 50).Value = ("CONTACTO DESCARGA:" + ((objCliente.PersonaDeDescarga ?? "").Length > 30 ? (objCliente.PersonaDeDescarga ?? "").ToString().Substring(0, 30) : (objCliente.PersonaDeDescarga ?? "").ToString())).ToUpper(); //OBAE3

                    if (objCliente.NoAdmiteFacturacionElectronica)
                    {
                        //Si no admite facturación electrónica añadimos la dirección de envío postal indicada
                        LocalizacionesModel objLocalizaciones = new LocalizacionesModel();
                        command.Parameters.Add("OBFB1", OdbcType.Char, 60).Value = "ENVIAR FACTURAS A LA SIGUIENTE DIRECCIÓN:"; //OBFB1

                        if (objCliente.TieneApartadoPostalFacturacion)
                        {
                            //Si tiene apartado de correos lo indicamos
                            command.Parameters.Add("OBFB2", OdbcType.Char, 60).Value = ("APARTADO DE CORREOS: " + objCliente.ApatadoDeCorreosFacturacion.ToString()).ToUpper();
                        }
                        else
                        {
                            //Si no tiene apartado de correos indicamos la dirección postal
                            command.Parameters.Add("OBFB2", OdbcType.Char, 60).Value = (objCliente.TipoDeViaFacturacion.ToString() + ". " + objCliente.DomicilioFacturacion.ToString() + ((objCliente.NumeroFacturacion ?? 0).ToString() != "0" ? " Nº " + (objCliente.NumeroFacturacion ?? 0).ToString() : "") + ((objCliente.PisoFacturacion ?? "").ToString() != "" ? " Piso " + (objCliente.PisoFacturacion ?? "").ToString() : "") + "'").ToUpper(); //OBFB2
                        }

                        //Estos datos son comunes tenga o no apartado de correos
                        command.Parameters.Add("OBFB3", OdbcType.Char, 60).Value = (objCliente.CPFacturacion.ToString() + "(" + objCliente.MunicipioFacturacion.ToString() + ") " + objLocalizaciones.getNombreProvincia((int)objCliente.IDProvinciaQSFacturacion) + " " + objLocalizaciones.getNombrePais((int)objCliente.IDPaisQSFacturacion)).ToUpper(); //OBFB3
                    }
                    else
                    {
                        //Si admite facturación electrónica los datos de facturación postal los dejamos vacíos
                        command.Parameters.Add("OBFB1", OdbcType.Char, 60).Value = "";
                        command.Parameters.Add("OBFB2", OdbcType.Char, 60).Value = "";
                        command.Parameters.Add("OBFB3", OdbcType.Char, 60).Value = "";
                    }
                    if (strQuery.Contains("UPDATE"))
                    {
                        //Si estamos haciendo un UPDATE añadimos un último parámetro con el código de cliente para el WHERE
                        command.Parameters.Add("OBCDG", OdbcType.Numeric, 10).Value = objCliente.QSID;
                    }
                    #endregion

                    command.Connection = con;
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
                    command.Parameters.Clear();
                    if (!existeFacturaElectronicaClienteQS(strEmpresa, objCliente))
                    {
                        strQuery = "INSERT INTO MRVF" + strEmpresa + "COM.CLNCX (CXCDG,CXFFD,CXJOF,CXTIPR,CXEMLF,CXFOR,CXFIR,CXENV) VALUES(?,?,?,?,?,?,?,?) ";
                        command.Parameters.Add("CXCDB", OdbcType.Numeric, 10).Value = objCliente.QSID;
                    }
                    else
                    {
                        strQuery = "UPDATE MRVF" + strEmpresa + "COM.CLNCX SET CXFFD=?, CXJOF=?, CXTIPR=?, CXEMLF=?, CXFOR=?, CXFIR=?,  CXENV=? WHERE CXCDG =?";
                    }
                    //Añadimos los parámetros
                    #region PARAMETROS
                    command.Parameters.Add("CXFFD", OdbcType.Char, 1).Value = "S";
                    command.Parameters.Add("CXJOF", OdbcType.Char, 1).Value = (objCliente.TipoDocumento == 1 ? "F" : "J");
                    command.Parameters.Add("CXTIPR", OdbcType.Char, 1).Value = "R";
                    command.Parameters.Add("CXEMLF", OdbcType.Char, 60).Value = objCliente.MailDeFacturacion.ToString();
                    command.Parameters.Add("CXFOR", OdbcType.Char, 1).Value = "P";
                    command.Parameters.Add("CXFIR", OdbcType.Char, 1).Value = "F";
                    command.Parameters.Add("CXENV", OdbcType.Char, 1).Value = "M";

                    if (strQuery.Contains("UPDATE"))
                    {
                        //Si estamos haciendo un UPDATE añadimos un último parámetro con el código de cliente para el WHERE
                        command.Parameters.Add("CXCDB", OdbcType.Numeric, 10).Value = objCliente.QSID;
                    }
                    #endregion

                    if (con.State == System.Data.ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    command.CommandText = strQuery;
                    command.ExecuteNonQuery();
                }
                else
                {
                    command.Parameters.Clear();

                    //Si no admite factura electrónica borramos los datos que pudiesen existir
                    strQuery = "DELETE FROM MRVF" + strEmpresa + "COM.CLNCX WHERE CXCDG=?";
                    if (con.State == System.Data.ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    command.Parameters.Add("CXCDB", OdbcType.Numeric, 10).Value = objCliente.QSID;

                    command.CommandText = strQuery;
                    command.ExecuteNonQuery();
                }
                #endregion

                //Grabamos el IBAN si corresponde
                #region TABLA CLNIB
                if ((objCliente.IBANENTIDAD ?? "").ToString() != "")
                {
                    command.Parameters.Clear();
                    if (!existeIBANClienteQS(strEmpresa, objCliente))
                    {
                        strQuery = "INSERT INTO MRVF" + strEmpresa + "COM.CLNIB (IBCDG, IBIBAN) VALUES (?,?) ";
                        command.Parameters.Add("IBCDG", OdbcType.Numeric, 10).Value = objCliente.QSID;

                    }
                    else
                    {
                        strQuery = "UPDATE MRVF" + strEmpresa + "COM.CLNIB SET IBIBAN=? WHERE IBCDG=?";
                    }

                    //Añadimos los parámetros
                    #region PARAMETROS
                    command.Parameters.Add("IBIBAN", OdbcType.Char, 34).Value = objCliente.IBANSIGLAS.ToString() + objCliente.IBANCODE.ToString() + objCliente.IBANENTIDAD.ToString() + objCliente.IBANSUCURSAL.ToString() + objCliente.IBANDC.ToString() + objCliente.IBANCCC.ToString();

                    if (strQuery.Contains("UPDATE"))
                    {
                        //Si estamos haciendo un UPDATE añadimos un último parámetro con el código de cliente para el WHERE
                        command.Parameters.Add("IBCDG", OdbcType.Numeric, 10).Value = objCliente.QSID;
                    }
                    #endregion
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
                    command.Parameters.Clear();
                    strQuery = "DELETE FROM MRVF" + strEmpresa + "COM.CLNIB WHERE IBCDG=?";
                    if (con.State == System.Data.ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    command.Parameters.Add("IBCDG", OdbcType.Numeric, 10).Value = objCliente.QSID;
                    command.CommandText = strQuery;
                    command.ExecuteNonQuery();
                }
                #endregion

                //Grabamos los datos en PRESUNTOS
                #region TABLA BDGPR
                command.Parameters.Clear();
                if (!existePresuntoClienteQS(strEmpresa, objCliente))
                {
                    strQuery = "INSERT INTO MRVBQSF" + strEmpresa + ".BDGPR (PRCDG, PRCSG,PRCLN,PRNBR,PRCIF,PRSGL,PRVIA,PRNMR,PRAMP,PRMUN,PRMNC,PRCDP,PRPAI,PRPRV,PRTF1,PRTF2,PRFAX,PRURL,PRDIVC,PRAGT,PRCMA1,PRCMA2,PRCMA3,PRCMN1,PRCMN2,PRMAIL) ";
                    strQuery += " VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)";
                    command.Parameters.Add("PRCDG", OdbcType.Numeric, 10).Value = objCliente.QSID;
                }
                else
                {
                    strQuery = "UPDATE MRVBQSF" + strEmpresa + ".BDGPR SET PRCSG=?,PRCLN=?,PRNBR=?,PRCIF=?,PRSGL=?,PRVIA=?,PRNMR=?,PRAMP=?,PRMUN=?,PRMNC=?,PRCDP=?,PRPAI=?,PRPRV=?,PRTF1=?,PRTF2=?,PRFAX=?,PRURL=?,PRDIVC=?,PRAGT=?,PRCMA1=?,PRCMA2=?,PRCMA3=?,PRCMN1=?,PRCMN2=?,PRMAIL=? WHERE PRCDG=?";
                }
                //Añadimos los parámetros
                #region PARAMETROS
                command.Parameters.Add("PRCSG", OdbcType.Numeric, 5).Value = objCliente.QSID;
                command.Parameters.Add("PRCLN", OdbcType.Numeric, 10).Value = objCliente.QSID;
                command.Parameters.Add("PRNBR", OdbcType.Char, 30).Value = objCliente.Nombre.ToString().ToUpper();
                command.Parameters.Add("PRCIF", OdbcType.Char, 15).Value = objCliente.NIF.ToString().ToUpper();
                command.Parameters.Add("PRSGL", OdbcType.Char, 2).Value = objCliente.TipoDeVia.ToString().ToUpper();
                command.Parameters.Add("PRVIA", OdbcType.Char, 25).Value = objCliente.Domicilio.ToString().ToUpper();
                command.Parameters.Add("PRNMR", OdbcType.Numeric, 5).Value = (objCliente.Numero ?? 0);
                command.Parameters.Add("PRAMP", OdbcType.Char, 15).Value = (objCliente.Piso ?? "").ToString().ToUpper();
                command.Parameters.Add("PRMUN", OdbcType.Numeric, 4).Value = (objCliente.IDMunicipioQS ?? 0);
                command.Parameters.Add("PRMNC", OdbcType.Char, 25).Value = objCliente.Municipio.ToString().ToUpper();
                command.Parameters.Add("PRCDP", OdbcType.Numeric, 5).Value = objCliente.CP;
                command.Parameters.Add("PRPAI", OdbcType.Numeric, 3).Value = objCliente.IDPaisQS;
                command.Parameters.Add("PRPRV", OdbcType.Numeric, 2).Value = objCliente.IDProvinciaQS;
                command.Parameters.Add("PRTF1", OdbcType.Numeric, 10).Value = (objCliente.Telefono1 ?? 0);
                command.Parameters.Add("PRTF2", OdbcType.Numeric, 10).Value = (objCliente.Telefono2 ?? 0);
                command.Parameters.Add("PRFAX", OdbcType.Numeric, 10).Value = (objCliente.Fax ?? 0).ToString();
                command.Parameters.Add("PRURL", OdbcType.Char, 60).Value = (objCliente.Contacto ?? "").ToString().ToUpper() + " - " + (objCliente.Web ?? "").ToString();
                command.Parameters.Add("PRDIVC", OdbcType.Char, 3).Value = "ESP";
                command.Parameters.Add("PRAGT", OdbcType.Numeric, 10).Value = (strEmpresa == "003" || strEmpresa == "033" ? (objCliente.IDAgenteQSMV ?? 0).ToString() : (strEmpresa == "004" || strEmpresa == "044" ? (objCliente.IDAgenteQSHMA ?? 0).ToString() : (strEmpresa == "006") ? (objCliente.IDAgenteQSECA ?? 0).ToString() : ((objCliente.IDAgenteQSHMA ?? 0).ToString() != "0" ? (objCliente.IDAgenteQSHMA ?? 0).ToString() : (objCliente.IDAgenteQSMV ?? 0).ToString())));
                command.Parameters.Add("PRCMA1", OdbcType.Char, 3).Value = (objCliente.FrecuenciaVisita ?? "").ToString();
                command.Parameters.Add("PRCMA2", OdbcType.Char, 3).Value = (objCliente.DiasVisita ?? "").ToString();
                command.Parameters.Add("PRCMA3", OdbcType.Char, 3).Value = (objCliente.FormaContacto ?? "").ToString();
                command.Parameters.Add("PRCMN1", OdbcType.Numeric, 8).Value = (objCliente.ConsumoPotencial ?? 0).ToString().Replace(",", ".");
                command.Parameters.Add("PRCMN2", OdbcType.Numeric, 8).Value = (objCliente.PrevisionAnual ?? 0).ToString().Replace(",", ".");
                command.Parameters.Add("PRMAIL", OdbcType.Char, 40).Value = (objCliente.TieneMail ? (objCliente.MailDeContacto.ToString() ?? "") + "'" : "NO TIENE'");
                if (strQuery.Contains("UPDATE"))
                {
                    //Si estamos haciendo un UPDATE añadimos un último parámetro con el código de cliente para el WHERE
                    command.Parameters.Add("PRCDG", OdbcType.Numeric, 10).Value = objCliente.QSID;
                }
                #endregion
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }
                command.CommandText = strQuery;
                command.ExecuteNonQuery();
                #endregion

                //Grabamos los datos en Direcciones de envío
                #region CLNDE
                //Borramos las posibles direcciones de envío asociadas al cliente
                command.Parameters.Clear();
                strQuery = "DELETE FROM MRVF" + strEmpresa + "COM.CLNDE WHERE DECDG=?";
                command.Parameters.Add("DECDG", OdbcType.Numeric, 10).Value = objCliente.QSID;
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }
                command.CommandText = strQuery;
                command.ExecuteNonQuery();

                i = 1;
                foreach (var direccion in objCliente.aspnet_ClientesDirEnv)
                {
                    command.Parameters.Clear();
                    strQuery = "INSERT INTO MRVF" + strEmpresa + "COM.CLNDE (DECDG,DEDEN,DENBR,DESGL,DEVIA,DENMR,DEAMP,DEMNC,DECDP,DEPRV,DEPAI,DETF1,DETFX) ";
                    strQuery += " VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?) ";
                    command.Parameters.Add("DECDG", OdbcType.Numeric, 10).Value = objCliente.QSID;
                    command.Parameters.Add("DEDEN", OdbcType.Numeric, 2).Value = i;
                    command.Parameters.Add("PRNBR", OdbcType.Char, 30).Value = direccion.Nombre.ToString().ToUpper();
                    command.Parameters.Add("DESGL", OdbcType.Char, 2).Value = direccion.TipoDeVía.ToString().ToUpper();
                    command.Parameters.Add("DEVIA", OdbcType.Char, 25).Value = direccion.Domicilio.ToString().ToUpper();
                    command.Parameters.Add("DENMR", OdbcType.Numeric, 5).Value = (direccion.Numero ?? 0);
                    command.Parameters.Add("DEAMP", OdbcType.Char, 15).Value = direccion.Piso.ToString().ToUpper();
                    command.Parameters.Add("DEMNC", OdbcType.Char, 25).Value = direccion.Municipio.ToString().ToUpper();
                    command.Parameters.Add("DECDP", OdbcType.Numeric, 5).Value = (direccion.CP ?? 0);
                    command.Parameters.Add("DEPRV", OdbcType.Numeric, 2).Value = direccion.IDProvinciaQS;
                    command.Parameters.Add("DEPAI", OdbcType.Numeric, 3).Value = direccion.IDPaisQS;
                    command.Parameters.Add("DETF1", OdbcType.Numeric, 10).Value = (direccion.Telefono ?? 0);
                    command.Parameters.Add("DETFX", OdbcType.Numeric, 10).Value = (direccion.Fax ?? 0);

                    if (con.State == System.Data.ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    command.CommandText = strQuery;
                    command.ExecuteNonQuery();

                    command.Parameters.Clear();
                    //Grabamos también el registro en la tabla de direcciones de envío / municipios 
                    strQuery = "INSERT INTO MRVF" + strEmpresa + "COM.CLNDM (DMCLN,DMDEN,DMMUN) ";
                    strQuery += " VALUES (?,?,?)";
                    command.Parameters.Add("DMCLN", OdbcType.Numeric, 10).Value = objCliente.QSID;
                    command.Parameters.Add("DMDEN", OdbcType.Numeric, 2).Value = i;
                    command.Parameters.Add("DMDEN", OdbcType.Numeric, 4).Value = direccion.IDMunicipioQS;

                    if (con.State == System.Data.ConnectionState.Closed)
                    {
                        con.Open();
                    }
                    command.CommandText = strQuery;
                    command.ExecuteNonQuery();
                }
                return blnRes;
            }
            catch (Exception ex)
            {
                errMessage += Environment.NewLine + " - Error en actualizarTablasAuxiliaresClienteEnQs(" + strEmpresa + "): " + ex.Message;
                blnRes = false;
                return blnRes;
            }
            #endregion
        }
        private bool actualizarDatosContablesClienteEnQS(string strEmpresa, aspnet_Clientes objCliente, ref string errMessage)
        {
            bool blnRes = true;
            string strQuery = "";
            Int64 ctaContable = 0;
            OdbcCommand command = new OdbcCommand();
            try
            {
                //Cuentas personales
                #region TABLA CTBCP
                ctaContable=getCuentaContableClienteQS(objCliente);

                if (!existeCuentaPersonalClienteEnQS(strEmpresa, ctaContable))
                {
                    strQuery = "INSERT INTO MRVF" + strEmpresa + "COM.CTBCP(CPCDG,CPSGL,CPVIA,CPNMR,CPMNC,CPCDP,CPNAC,CPPRV,CPNIF,CPSWB) VALUES (?,?,?,?,?,?,?,?,?,?)";

                    command.Parameters.Add("CPCDG", OdbcType.Numeric, 10).Value = ctaContable;
                }
                else
                {
                    strQuery = "UPDATE MRVF" + strEmpresa + "COM.CTBCP SET CPSGL=?,CPVIA=?,CPNMR=?,CPMNC=?,CPCDP=?,CPNAC=?,CPPRV=?,CPNIF=?,CPSWB=? WHERE CPCDG=?";
                }

                command.Parameters.Add("CPSGL", OdbcType.Char, 2).Value = objCliente.TipoDeVia.ToString();
                command.Parameters.Add("CPVIA", OdbcType.Char, 25).Value = objCliente.Domicilio.ToString();
                command.Parameters.Add("CPNMR", OdbcType.Numeric, 5).Value = (objCliente.Numero ?? 0);
                command.Parameters.Add("CPMNC", OdbcType.Char, 25).Value = (objCliente.Municipio ?? "");
                command.Parameters.Add("CPCDP", OdbcType.Numeric, 5).Value = objCliente.CP;
                command.Parameters.Add("CPNAC", OdbcType.Numeric, 3).Value = (objCliente.IDPaisQS ?? 0);
                command.Parameters.Add("CPPRV", OdbcType.Numeric, 2).Value = (objCliente.IDProvinciaQS ?? 0);
                command.Parameters.Add("CPNIF", OdbcType.Char, 15).Value = (objCliente.NIF ?? "");
                command.Parameters.Add("CPSWB", OdbcType.Char, 1).Value = "";

                if (strQuery.Contains("UPDATE")){
                    command.Parameters.Add("CPCDG", OdbcType.Numeric, 10).Value = ctaContable;
                }

                command.Connection=con;

                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }

                command.CommandText=strQuery;
                command.ExecuteNonQuery();

                #endregion

                //Grupos contables
                #region TABLA CTBGC
                command.Parameters.Clear();

                if (!existeGrupoContableClienteEnQS(strEmpresa, ctaContable))
                {
                    strQuery = "INSERT INTO MRVF" + strEmpresa + "COM.CTBGC(GCCDG,GCCTA,GCNBR,GCMDE,GCMDB,GCPRE,GCDIV,GCNDG,GCNDN,GCSWB) VALUES (?,?,?,?,?,?,?,?,?,?)";

                    command.Parameters.Add("GCCDG", OdbcType.Numeric, 10).Value = ctaContable;
                }
                else
                {
                    strQuery = "UPDATE MRVF" + strEmpresa + "COM.CTBGC SET GCCTA=?,GCNBR=?,GCMDE=?,GCMDB=?,GCPRE=?,GCDIV=?,GCNDG=?,GCNDN=?,GCSWB=? WHERE GCCDG=?";
                }

                command.Parameters.Add("GCCTA", OdbcType.Numeric, 10).Value = ctaContable;
                command.Parameters.Add("GCNBR", OdbcType.Char, 30).Value = objCliente.Nombre;
                command.Parameters.Add("GCMDE", OdbcType.Numeric, 3).Value = 0;
                command.Parameters.Add("GCMDB", OdbcType.Numeric, 3).Value = 130;
                command.Parameters.Add("GCPRE", OdbcType.Char, 1).Value = "C";
                command.Parameters.Add("GCDIV", OdbcType.Char, 3).Value = "EUR";
                command.Parameters.Add("GCNDG", OdbcType.Numeric, 2).Value = 10;
                command.Parameters.Add("GCNDN", OdbcType.Numeric, 2).Value = 0;
                command.Parameters.Add("GCSWB", OdbcType.Char, 1).Value = "";

                if (strQuery.Contains("UPDATE"))
                {
                    command.Parameters.Add("GCCDG", OdbcType.Numeric, 10).Value = ctaContable;
                }
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }
                command.CommandText = strQuery;
                command.ExecuteNonQuery();
                #endregion

                //Plan de cuentas
                #region TABLA CTBMC
                command.Parameters.Clear();

                if (!existePlanDeCuentasClienteEnQS(strEmpresa, ctaContable))
                {
                    strQuery = "INSERT INTO MRVF" + strEmpresa + "COM.CTBMC(MCNIV,MCCTA,MCNBR,MCMPE,MCMPB,MCPRE,MCDIV,MCSWB) VALUES (?,?,?,?,?,?,?,?)";

                    command.Parameters.Add("MCNIV", OdbcType.Numeric, 1).Value = 3;
                    command.Parameters.Add("MCCTA", OdbcType.Numeric, 10).Value = ctaContable;
                }
                else
                {
                    strQuery = "UPDATE MRVF" + strEmpresa + "COM.CTBMC SET MCNBR=?,MCMPE=?,MCMPB=?,MCPRE=?,MCDIV=?,MCSWB=? WHERE MCNIV=? AND MCCTA=?";
                }

                command.Parameters.Add("MCNBR", OdbcType.Char, 30).Value = objCliente.Nombre;
                command.Parameters.Add("MCMPE", OdbcType.Numeric, 2).Value = 0;
                command.Parameters.Add("MCMPB", OdbcType.Numeric, 2).Value = 30;
                command.Parameters.Add("MCPRE", OdbcType.Char, 1).Value = "C";
                command.Parameters.Add("MCDIV", OdbcType.Char, 3).Value = "EUR";
                command.Parameters.Add("MCSWB", OdbcType.Char, 1).Value = "";

                if (strQuery.Contains("UPDATE"))
                {
                    command.Parameters.Add("MCNIV", OdbcType.Numeric, 1).Value = 3;
                    command.Parameters.Add("MCCTA", OdbcType.Numeric, 10).Value = ctaContable;
                }
                
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }
                command.CommandText = strQuery;
                command.ExecuteNonQuery();

                #endregion
            }
            catch (Exception ex)
            {
                blnRes=false;
                errMessage += Environment.NewLine + " - Error en actualizarDatosContablesClienteEnQS(" + strEmpresa + "): " + ex.Message.ToString();
            }
            return blnRes;
        }
        private void actualizarTablasAuxiliaresClienteEnQS_ORI(string strEmpresa, aspnet_Clientes objCliente)
        {
            OdbcCommand command = new OdbcCommand();
            string strQuery = "";
            string strValores = "";
            command.Connection = con;
            //Añadir registro en CLNCM con el código de municipio
            #region TABLA CLNCM
            if (!existeMunicipioClienteQS(strEmpresa, objCliente))
            {
                strQuery = "INSERT INTO MRVF" + strEmpresa + "COM.CLNCM (CMCLN, CMMUN) VALUES (" + objCliente.QSID + "," + objCliente.IDMunicipioQS + ")";
            }
            else
            {
                strQuery = "UPDATE MRVF" + strEmpresa + "COM.CLNCM SET CMMUN=" + objCliente.IDMunicipioQS + " WHERE CMCLN=" + objCliente.QSID;
            }
            if (con.State == System.Data.ConnectionState.Closed)
            {
                con.Open();
            }
            command.CommandText = strQuery;
            command.ExecuteNonQuery();
            #endregion

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
            string strFotoMontaje = "N";
            if ((objCliente.IDActividadQS ?? "") == "CP" || (objCliente.IDActividadQS ?? "") == "C" || (objCliente.IDActividadQS ?? "") == "PR")
            {
                //Si la actividad del cliente es CONTRUCTOR-PROMOTOR, CONSTRUCTOR O PROMOTOR SE OBLIGA A QUE TENGA FOTOMONTAJE
                strFotoMontaje = "S";
            }

            if (!existeFotomontajeAnexoClienteQS(strEmpresa, objCliente))
            {
                strQuery = "INSERT INTO MRVF" + strEmpresa + "COM.CLNAI (AICDG,AIFTM) VALUES (" + objCliente.QSID.ToString() + ", '" + strFotoMontaje + "')";
            }
            else
            {
                strQuery = "UPDATE MRVF" + strEmpresa + "COM.CLNAI SET AIFTM='" + strFotoMontaje + "' WHERE AICDG=" + objCliente.QSID.ToString();
            }
            if (con.State == System.Data.ConnectionState.Closed)
            {
                con.Open();
            }
            command.CommandText = strQuery;
            command.ExecuteNonQuery();

            if (!existeFotomontajeClienteQS(strEmpresa, objCliente))
            {
                strQuery = "INSERT INTO MRVF" + strEmpresa + "COM.CLNFT (FTCCN,FTFOT) VALUES (" + objCliente.QSID.ToString() + ", '" + strFotoMontaje + "')";
            }
            else
            {
                strQuery = "UPDATE MRVF" + strEmpresa + "COM.CLNFT SET FTFOT='" + strFotoMontaje + "' WHERE FTCCN=" + objCliente.QSID.ToString();
            }
            if (con.State == System.Data.ConnectionState.Closed)
            {
                con.Open();
            }
            command.CommandText = strQuery;
            command.ExecuteNonQuery();
            #endregion

            //Añadimos los datos de Observaciones con los datos de la FICHA LOGÍSTICA
            #region TABLA CLNOB
            string strPersonasAutorizadasRetMat = "";
            if (!objCliente.EsDeExposicion || !objCliente.EsClienteParticular || !objCliente.RecogeEnNuestrasInstalaciones)
            {
                if (!existeFichaLogisticaClienteQS(strEmpresa, objCliente))
                {
                    if (objCliente.aspnet_PersonasRetiradaMat.Count > 0)
                    {
                        //Si tenemos personas autorizadas para retirada de material las añadimos en los campos OBPM6 Y OBPM7
                        strPersonasAutorizadasRetMat = ", 'AUT.RECOGIDAS:";
                        foreach (var persona in objCliente.aspnet_PersonasRetiradaMat)
                        {
                            strPersonasAutorizadasRetMat += persona.NIF.ToString() + " " + persona.Nombre.ToString() + "', '";
                        }
                        if (objCliente.aspnet_PersonasRetiradaMat.Count == 1)
                        {
                            strPersonasAutorizadasRetMat += "'"; //Si sólo hay una persona autorizada, cerramos la comilla para que el campo OBPM7 quede vacío
                        }
                        else
                        {
                            strPersonasAutorizadasRetMat = strValores.Substring(0, strValores.Length - 3); //Si hay 2 quitamos ", '" de la cadena
                        }
                    }
                    else
                    {
                        //Si no tenemos personas autorizadas para retirada de material, dejamos los campos OBPM6 Y OBPM7 vacíos.
                        strPersonasAutorizadasRetMat = ", '',''";
                    }

                    strQuery = "INSERT INTO MRVF" + strEmpresa + "COM.CLNOB (OBCDG,OBPM1,OBPM2,OBPM3,OBPM4,OBPM5,OBPM6,OBPM7,OBPO1,OBPO2,OBPO3,OBAM1,OBAM2,OBAM3,OBAM4,OBAM5,OBAM6,OBAM7,OBAE1,OBAE2,OBAE3,OBFB1,OBFB2,OBFB3) ";
                    strValores = "VALUES (" + objCliente.QSID.ToString();
                    strValores += ", 'HORARIO: " + objCliente.Horario.ToString() + "'"; //OBPM1
                    strValores += ", 'HORARIO VERANO: " + (objCliente.HorarioDeVerano ?? "").ToString() + "'"; //OBPM2
                    strValores += ", 'M.T.PROPIOS:" + (objCliente.MedioDeTransportePropio ? "SI" : "NO") + "/PORTES:" + (objCliente.CobroDePortesPorEnvio ? objCliente.ImportePortesPorEnvio.ToString() + "E" : "NO") + "/CAMION GRUA:" + (objCliente.NecesitaCamionConPluma ? "SI" : "NO") + "/VEHICULO:" + objCliente.aspnet_TiposDeVehiculo.Nombre.ToString() + "'"; //OBPM3
                    strValores += ", 'M.DESCARGA: " + objCliente.aspnet_MediosDeDescarga.Nombre + "/ " + (objCliente.PesaElMaterial ? "PESA CON " + objCliente.aspnet_InstrumentosDePesaje.Nombre.ToString() : "NO PESA EL MATERIAL") + "'"; //OBPM4
                    strValores += ", 'OBLIG. CERT. CALIDAD:" + ((objCliente.RequerimientosDeCalidad ?? "").Contains('2') ? "SI" : "NO") + "/PRL: " + (objCliente.RequerimientosDePrevencion ?? "").ToString().Substring(0, ((objCliente.RequerimientosDePrevencion ?? "").Length > 30 ? 30 : (objCliente.RequerimientosDePrevencion ?? "").Length)) + "'";//OBPM5
                    strValores += strPersonasAutorizadasRetMat; //OBPM6 y OBPM7
                    strValores += ", 'HORARIO: " + objCliente.Horario.ToString() + "'"; //OBPO1
                    strValores += ", 'HORARIO VERANO: " + (objCliente.HorarioDeVerano ?? "").ToString() + "'"; //OBPO2
                    strValores += ", 'CONTACTO DESCARGA:" + ((objCliente.PersonaDeDescarga ?? "").Length > 30 ? (objCliente.PersonaDeDescarga ?? "").ToString().Substring(0, 30) : (objCliente.PersonaDeDescarga ?? "").ToString()) + "'"; //OBPO3
                    strValores += ", 'HORARIO: " + objCliente.Horario.ToString() + "'"; //OBAM1
                    strValores += ", 'HORARIO VERANO: " + (objCliente.HorarioDeVerano ?? "").ToString() + "'"; //OBAM2
                    strValores += ", 'M.T.PROPIOS:" + (objCliente.MedioDeTransportePropio ? "SI" : "NO") + "/PORTES:" + (objCliente.CobroDePortesPorEnvio ? objCliente.ImportePortesPorEnvio.ToString() + "E" : "NO") + "/CAMION GRUA:" + (objCliente.NecesitaCamionConPluma ? "SI" : "NO") + "/VEHICULO:" + objCliente.aspnet_TiposDeVehiculo.Nombre.ToString() + "'"; //OBAM3
                    strValores += ", 'M.DESCARGA: " + objCliente.aspnet_MediosDeDescarga.Nombre + "/ " + (objCliente.PesaElMaterial ? "PESA CON " + objCliente.aspnet_InstrumentosDePesaje.Nombre.ToString() : "NO PESA EL MATERIAL") + "'"; //OBAM4
                    strValores += ", 'OBLIG. CERT. CALIDAD:" + (objCliente.RequerimientosDeCalidad.Contains('2') ? "SI" : "NO") + "/PRL: " + (objCliente.RequerimientosDePrevencion ?? "").ToString().Substring(0, ((objCliente.RequerimientosDePrevencion ?? "").Length > 30 ? 30 : (objCliente.RequerimientosDePrevencion ?? "").Length)) + "'";//OBAM5
                    strValores += strPersonasAutorizadasRetMat; //OBAM6 y //OBAM7
                    strValores += ", 'HORARIO: " + objCliente.Horario.ToString() + "'"; //OBAE1
                    strValores += ", 'HORARIO VERANO: " + (objCliente.HorarioDeVerano ?? "").ToString() + "'"; //OBAE2
                    strValores += ", 'CONTACTO DESCARGA:" + ((objCliente.PersonaDeDescarga ?? "").Length > 30 ? (objCliente.PersonaDeDescarga ?? "").ToString().Substring(0, 30) : (objCliente.PersonaDeDescarga ?? "").ToString()) + "'"; //OBPO3
                    if (objCliente.NoAdmiteFacturacionElectronica)
                    {
                        LocalizacionesModel objLocalizaciones = new LocalizacionesModel();
                        strValores += ", 'ENVIAR FACTURAS A LA SIGUIENTE DIRECCIÓN:'";
                        strValores += ", '" + objCliente.TipoDeViaFacturacion.ToString() + ". " + objCliente.DomicilioFacturacion.ToString() +  ((objCliente.NumeroFacturacion??0).ToString()!="0" ? " Nº " +(objCliente.NumeroFacturacion??0).ToString() : "") + ((objCliente.PisoFacturacion??"").ToString()!="" ? " Piso " +(objCliente.PisoFacturacion??"").ToString() : "") + "'";
                        strValores += ", '" + objCliente.CPFacturacion.ToString() + "(" + objCliente.MunicipioFacturacion.ToString() + ") " + objLocalizaciones.getNombreProvincia((int)objCliente.IDProvinciaQSFacturacion) + " " + objLocalizaciones.getNombrePais((int)objCliente.IDPaisQSFacturacion) + "')";
                    }
                    else
                    {
                        strValores += ", '','','')";
                    }
                }
                else
                {
                    if (objCliente.aspnet_PersonasRetiradaMat.Count > 0)
                    {
                        //Si tenemos personas autorizadas para retirada de material las añadimos en los campos OBPM6 Y OBPM7
                        strPersonasAutorizadasRetMat = ", OBPM6='AUT.RECOGIDAS:";
                        foreach (var persona in objCliente.aspnet_PersonasRetiradaMat)
                        {
                            strPersonasAutorizadasRetMat += persona.NIF.ToString() + " " + persona.Nombre.ToString() + "', OBPM7='";
                        }
                        if (objCliente.aspnet_PersonasRetiradaMat.Count == 1)
                        {
                            strPersonasAutorizadasRetMat += "'"; //Si sólo hay una persona autorizada, cerramos la comilla para que el campo OBPM7 quede vacío
                        }
                        else
                        {
                            strPersonasAutorizadasRetMat = strValores.Substring(0, strValores.Length - 8); //Si hay 2 quitamos ", '" de la cadena
                        }
                    }
                    else
                    {
                        //Si no tenemos personas autorizadas para retirada de material, dejamos los campos OBPM6 Y OBPM7 vacíos.
                        strPersonasAutorizadasRetMat = ", OBPM6='',OBPM7=''";
                    }

                    strQuery = "UPDATE MRVF" + strEmpresa + "COM.CLNOB SET ";
                    strValores = "OBPM1='HORARIO: " + objCliente.Horario.ToString() + "'"; //OBPM1
                    strValores += ", OBPM2='HORARIO VERANO: " + (objCliente.HorarioDeVerano ?? "").ToString() + "'"; //OBPM2
                    strValores += ", OBPM3='M.T.PROPIOS:" + (objCliente.MedioDeTransportePropio ? "SI" : "NO") + "/PORTES:" + (objCliente.CobroDePortesPorEnvio ? objCliente.ImportePortesPorEnvio.ToString() + "E" : "NO") + "/CAMION GRUA:" + (objCliente.NecesitaCamionConPluma ? "SI" : "NO") + "/VEHICULO:" + objCliente.aspnet_TiposDeVehiculo.Nombre.ToString() + "'"; //OBPM3
                    strValores += ", OBPM4='M.DESCARGA: " + objCliente.aspnet_MediosDeDescarga.Nombre + "/ " + (objCliente.PesaElMaterial ? "PESA CON " + objCliente.aspnet_InstrumentosDePesaje.Nombre.ToString() : "NO PESA EL MATERIAL") + "'"; //OBPM4
                    strValores += ", OBPM5='OBLIG. CERT. CALIDAD:" + (objCliente.RequerimientosDeCalidad.Contains('2') ? "SI" : "NO") + "/PRL: " + (objCliente.RequerimientosDePrevencion ?? "").ToString().Substring(0, ((objCliente.RequerimientosDePrevencion ?? "").Length > 30 ? 30 : (objCliente.RequerimientosDePrevencion ?? "").Length)) + "'";//OBPM5
                    strValores += strPersonasAutorizadasRetMat; //OBPM6 y OBPM7
                    strValores += ", OBPO1='HORARIO: " + objCliente.Horario.ToString() + "'"; //OBPO1
                    strValores += ", OBPO2='HORARIO VERANO: " + (objCliente.HorarioDeVerano ?? "").ToString() + "'"; //OBPO2
                    strValores += ", OBPO3='CONTACTO DESCARGA: " + ((objCliente.PersonaDeDescarga ?? "").Length > 30 ? (objCliente.PersonaDeDescarga ?? "").ToString().Substring(0, 30) : (objCliente.PersonaDeDescarga ?? "").ToString()) + "'"; //OBPO3
                    strValores += ", OBAM1='HORARIO: " + objCliente.Horario.ToString() + "'"; //OBAM1
                    strValores += ", OBAM2='HORARIO VERANO: " + (objCliente.HorarioDeVerano ?? "").ToString() + "'"; //OBAM2
                    strValores += ", OBAM3='M.T.PROPIOS:" + (objCliente.MedioDeTransportePropio ? "SI" : "NO") + "/PORTES:" + (objCliente.CobroDePortesPorEnvio ? objCliente.ImportePortesPorEnvio.ToString() + "E" : "NO") + "/CAMION GRUA:" + (objCliente.NecesitaCamionConPluma ? "SI" : "NO") + "/VEHICULO:" + objCliente.aspnet_TiposDeVehiculo.Nombre.ToString() + "'"; //OBAM3
                    strValores += ", OBAM4='M.DESCARGA: " + objCliente.aspnet_MediosDeDescarga.Nombre + "/ " + (objCliente.PesaElMaterial ? "PESA CON " + objCliente.aspnet_InstrumentosDePesaje.Nombre.ToString() : "NO PESA EL MATERIAL") + "'"; //OBAM4
                    strValores += ", OBAM5='OBLIG. CERT. CALIDAD:" + (objCliente.RequerimientosDeCalidad.Contains('2') ? "SI" : "NO") + "/PRL: " + (objCliente.RequerimientosDePrevencion ?? "").ToString().Substring(0, ((objCliente.RequerimientosDePrevencion ?? "").Length > 30 ? 30 : (objCliente.RequerimientosDePrevencion ?? "").Length)) + "'";//OBAM5
                    strValores += strPersonasAutorizadasRetMat.Replace("OBPM6", "OBAM6").Replace("OBPM7", "OBAM7"); //OBAM6 y //OBAM7
                    strValores += ", OBAE1='HORARIO: " + objCliente.Horario.ToString() + "'"; //OBAE1
                    strValores += ", OBAE2='HORARIO VERANO: " + (objCliente.HorarioDeVerano ?? "").ToString() + "'"; //OBAE2
                    strValores += ", OBAE3='CONTACTO DESCARGA:" + ((objCliente.PersonaDeDescarga ?? "").Length > 30 ? (objCliente.PersonaDeDescarga ?? "").ToString().Substring(0, 30) : (objCliente.PersonaDeDescarga ?? "").ToString()) + "'";  //OBAE3
                    if (objCliente.NoAdmiteFacturacionElectronica)
                    {
                        LocalizacionesModel objLocalizaciones = new LocalizacionesModel();
                        strValores += ", OBFB1='ENVIAR FACTURAS A LA SIGUIENTE DIRECCIÓN:'";
                        strValores += ", OBFB2='" + objCliente.TipoDeViaFacturacion.ToString() + ". " + objCliente.DomicilioFacturacion.ToString() + " Nº " + objCliente.NumeroFacturacion.ToString() + " Piso " + objCliente.PisoFacturacion.ToString() + "'";
                        strValores += ", OBFB3='" + objCliente.CPFacturacion.ToString() + "(" + objCliente.MunicipioFacturacion.ToString() + ")" + objLocalizaciones.getNombreProvincia((int)objCliente.IDProvinciaQSFacturacion) + " " + objLocalizaciones.getNombrePais((int)objCliente.IDPaisQSFacturacion) + "'";
                    }
                    else
                    {
                        strValores += ", OBFB1='', OBFB2='', OBFB3=''";
                    }

                    strValores += " WHERE OBCDG=" + objCliente.QSID.ToString();
                }
                strQuery += strValores.ToUpper();
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
                    strQuery = "INSERT INTO MRVF" + strEmpresa + "COM.CLNCX (CXCDG,CXFFD,CXTIPR,CXEMLF,CXFOR,CXFIR,CXENV,CXJOF) ";
                    strValores = "VALUES (" + objCliente.QSID.ToString();
                    strValores += ", 'S'";
                    strValores += ", 'R'";
                    strValores += ", '" + objCliente.MailDeFacturacion.ToString() + "'";
                    strValores += ", 'P'";
                    strValores += ", 'F'";
                    strValores += ", 'M'";
                    strValores += ",'" + (objCliente.TipoDocumento == 1 ? "F" : "J") + "')";
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
                    strValores += ", CXJOF='" + (objCliente.TipoDocumento == 1 ? "F" : "J") + "'";
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

            //Grabamos el IBAN si corresponde
            #region TABLA CLNIB
            if ((objCliente.IBANENTIDAD ?? "").ToString() != "")
            {
                if (!existeIBANClienteQS(strEmpresa, objCliente))
                {
                    strQuery = "INSERT INTO MRVF" + strEmpresa + "COM.CLNIB (IBCDG, IBIBAN) ";
                    strValores = "VALUES ( " + objCliente.QSID.ToString();
                    strValores += ", '" + objCliente.IBANSIGLAS.ToString() + objCliente.IBANCODE.ToString() + objCliente.IBANENTIDAD.ToString() + objCliente.IBANSUCURSAL.ToString() + objCliente.IBANDC.ToString()+objCliente.IBANCCC.ToString()+ "')";
                }
                else
                {
                    strQuery = "UPDATE MRVF" + strEmpresa + "COM.CLNIB SET ";
                    strValores = "IBIBAN ='" + objCliente.IBANSIGLAS.ToString() + objCliente.IBANCODE.ToString() + objCliente.IBANENTIDAD.ToString() + objCliente.IBANSUCURSAL.ToString() + objCliente.IBANDC.ToString() + objCliente.IBANCCC.ToString() + "' ";
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
                strQuery = "INSERT INTO MRVBQSF" + strEmpresa + ".BDGPR (PRCDG, PRCSG,PRCLN,PRNBR,PRCIF,PRSGL,PRVIA,PRNMR,PRAMP,PRMNC,PRCDP,PRPAI,PRPRV,PRTF1,PRTF2,PRFAX,PRURL,PRDIVC,PRAGT,PRCMA1,PRCMA2,PRCMA3,PRCMN1,PRCMN2,PRMAIL) ";
                strValores = " VALUES (" + objCliente.QSID.ToString();
                strValores += "," + objCliente.QSID.ToString();
                strValores += "," + objCliente.QSID.ToString();
                strValores += ",'" + objCliente.Nombre.ToString() + "'";
                strValores += ",'" + objCliente.NIF.ToString() + "'";
                strValores += ",'" + objCliente.TipoDeVia.ToString() + "'";
                strValores += ",'" + objCliente.Domicilio.ToString() + "'";
                strValores += "," + (objCliente.Numero??0).ToString();
                strValores += ",'" + (objCliente.Piso ?? "").ToString() + "'";
                strValores += ",'" + objCliente.Municipio.ToString() + "'";
                strValores += "," + objCliente.CP.ToString();
                strValores += "," + objCliente.IDPaisQS.ToString();
                strValores += "," + objCliente.IDProvinciaQS.ToString();
                strValores += "," + (objCliente.Telefono1 ?? 0).ToString();
                strValores += "," + (objCliente.Telefono2 ?? 0).ToString();
                strValores += "," + (objCliente.Fax ?? 0).ToString();
                strValores += ",'" + (objCliente.Contacto ?? "").ToString() + " - " + (objCliente.Web ?? "").ToString() + "'";
                strValores += ",'ESP'";
                strValores += "," + (strEmpresa == "003" || strEmpresa == "033" ? (objCliente.IDAgenteQSMV ?? 0).ToString() : (strEmpresa == "004" || strEmpresa == "044" ? (objCliente.IDAgenteQSHMA ?? 0).ToString() : (strEmpresa == "006") ? (objCliente.IDAgenteQSECA ?? 0).ToString() : ((objCliente.IDAgenteQSHMA ?? 0).ToString() != "0" ? (objCliente.IDAgenteQSHMA ?? 0).ToString() : (objCliente.IDAgenteQSMV ?? 0).ToString())));
                strValores += ",'" + (objCliente.FrecuenciaVisita ?? "").ToString() + "'";
                strValores += ",'" + (objCliente.DiasVisita ??"").ToString() + "'";
                strValores += ",'" + (objCliente.FormaContacto??"").ToString() + "'";
                strValores += "," + (objCliente.ConsumoPotencial??0).ToString().Replace(",", ".");
                strValores += "," + (objCliente.PrevisionAnual??0).ToString().Replace(",", ".");

                //Pasamos todo a mayúsculas excepto el mail
                strValores = strValores.ToUpper();

                strValores += ",'" + (objCliente.TieneMail ? (objCliente.MailDeContacto.ToString() ?? "") + "'" : "NO TIENE'") + ")";
                
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
                strValores += ", PRNMR=" + (objCliente.Numero??0).ToString();
                strValores += ", PRAMP='" + (objCliente.Piso ?? "").ToString() + "'";
                strValores += ", PRMNC='" + objCliente.Municipio.ToString() + "'";
                strValores += ", PRCDP=" + objCliente.CP.ToString();
                strValores += ", PRPAI=" + objCliente.IDPaisQS.ToString();
                strValores += ", PRPRV=" + objCliente.IDProvinciaQS.ToString();
                strValores += ", PRTF1=" + (objCliente.Telefono1 ?? 0).ToString();
                strValores += ", PRTF2=" + (objCliente.Telefono2 ?? 0).ToString();
                strValores += ", PRFAX=" + (objCliente.Fax ?? 0).ToString();
                strValores += ", PRURL='" + (objCliente.Contacto ?? "").ToString() + " - " + (objCliente.Web ?? "").ToString() + "'";
                strValores += ", PRDIVC='ESP'";
                strValores += ", PRAGT=" + (strEmpresa == "003" || strEmpresa == "033" ? (objCliente.IDAgenteQSMV ?? 0).ToString() : (strEmpresa == "004" || strEmpresa == "044" ? (objCliente.IDAgenteQSHMA ?? 0).ToString() : (strEmpresa == "006") ? (objCliente.IDAgenteQSECA ?? 0).ToString() : ((objCliente.IDAgenteQSHMA ?? 0).ToString() != "0" ? (objCliente.IDAgenteQSHMA ?? 0).ToString() : (objCliente.IDAgenteQSMV ?? 0).ToString())));
                strValores += ", PRCMA1='" + (objCliente.FrecuenciaVisita??"").ToString() + "'";
                strValores += ", PRCMA2='" + (objCliente.DiasVisita??"").ToString() + "'";
                strValores += ", PRCMA3='" + (objCliente.FormaContacto??"").ToString() + "'";
                strValores += ", PRCMN1=" + (objCliente.ConsumoPotencial??0).ToString().Replace(",", ".");
                strValores += ", PRCMN2=" + (objCliente.PrevisionAnual??0).ToString().Replace(",", ".");

                //Pasamos todo a mayúsculas excepto el mail
                strValores = strValores.ToUpper();

                strValores += ", PRMAIL='" + (objCliente.TieneMail ? (objCliente.MailDeContacto.ToString() ?? "") + "'" : "NO TIENE'");
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

            //Grabamos los datos en Direcciones de envío
            #region CLNDE
            int i=1;
            foreach (var direccion in objCliente.aspnet_ClientesDirEnv)
            {
                strQuery = "INSERT INTO MRVF" + strEmpresa + "COM.CLNDE (DECDG,DEDEN,DENBR,DESGL,DEVIA,DENMR,DEAMP,DEMNC,DECDP,DEPAI,DEPRV,DETF1,DETFX) ";
                strValores = " VALUES (" + objCliente.QSID.ToString();
                strValores += "," + i;
                strValores += ",'" + direccion.Nombre.ToString() + "'";
                strValores += ",'" + direccion.TipoDeVía.ToString() + "'";
                strValores += ",'" + direccion.Domicilio.ToString() + "'";
                strValores += "," + (direccion.Numero??0);
                strValores += ",'" + direccion.Piso.ToString() + "'"; 
                strValores += ",'" + direccion.Municipio.ToString() + "'"; 
                strValores += "," + direccion.CP;
                strValores += "," + direccion.IDProvinciaQS;
                strValores += "," + direccion.IDPaisQS;
                strValores += "," + (direccion.Telefono??0);
                strValores += "," + (direccion.Fax??0) + ")";
                
                strValores = strValores.ToUpper();

                strQuery += strValores;
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }
                command.CommandText = strQuery;
                command.ExecuteNonQuery();

                //Grabamos también el registro en la tabla de direcciones de envío / municipios 
                strQuery = "INSERT INTO MRVF" + strEmpresa + "COM.CLNDM (DMCLN,DMDEN,DMMUN) ";
                strValores = " VALUES (" + objCliente.QSID.ToString();
                strValores += "," + i;
                strValores += "," + direccion.IDMunicipioQS + ")";

                strValores = strValores.ToUpper();

                strQuery += strValores;
                if (con.State == System.Data.ConnectionState.Closed)
                {
                    con.Open();
                }
                command.CommandText = strQuery;
                command.ExecuteNonQuery();
                     
            }
            #endregion
        }
        private bool existeMunicipioClienteQS(string strEmpresa, aspnet_Clientes objCliente)
        {
            bool blnRes = false;
            OdbcCommand command = new OdbcCommand();
            command.Connection = con;
            if (con.State == System.Data.ConnectionState.Closed)
            {
                con.Open();
            }
            command.CommandText = "SELECT COUNT(*) FROM MRVF" + strEmpresa + "COM.CLNCM WHERE CMCLN=" + objCliente.QSID.ToString();
            int rows = (int)command.ExecuteScalar();
            if (rows > 0)
            {
                blnRes = true;
            }
            con.Close();
            command.Dispose();
            return blnRes;
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
        private bool existeFotomontajeAnexoClienteQS(string strEmpresa, aspnet_Clientes objCliente)
        {
            bool blnRes = false;
            OdbcCommand command = new OdbcCommand();
            command.Connection = con;
            if (con.State == System.Data.ConnectionState.Closed)
            {
                con.Open();
            }
            command.CommandText = "SELECT COUNT(*) FROM MRVF" + strEmpresa + "COM.CLNAI WHERE AICDG=" + objCliente.QSID.ToString();
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
        public bool existeClienteEnEmpresaQS(string strEmpresa, decimal? QSID, ref string strAgente)
        {
            bool blnRes = true;
            string strQuery = "SELECT CLAG1 FROM MRVF" + strEmpresa + "COM.CLNCL WHERE CLCDG=" + QSID.ToString() + "";
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
            else
            {
                strAgente = aux.ToString();
            }
            return blnRes;
        }
        private bool existeCuentaPersonalClienteEnQS(string strEmpresa, Int64 ctaContable)
        {
            bool blnRes = false;
            OdbcCommand command = new OdbcCommand();
            command.Connection = con;
            if (con.State == System.Data.ConnectionState.Closed)
            {
                con.Open();
            }
            command.CommandText = "SELECT COUNT(*) FROM MRVF" + strEmpresa + "COM.CTBCP WHERE CPCDG=" + ctaContable.ToString();
            int rows = (int)command.ExecuteScalar();
            if (rows > 0)
            {
                blnRes = true;
            }
            con.Close();
            command.Dispose();
            return blnRes;
        }
        private bool existeGrupoContableClienteEnQS(string strEmpresa, Int64 ctaContable)
        {
            bool blnRes = false;
            OdbcCommand command = new OdbcCommand();
            command.Connection = con;
            if (con.State == System.Data.ConnectionState.Closed)
            {
                con.Open();
            }
            command.CommandText = "SELECT COUNT(*) FROM MRVF" + strEmpresa + "COM.CTBGC WHERE GCCDG=" + ctaContable.ToString();
            int rows = (int)command.ExecuteScalar();
            if (rows > 0)
            {
                blnRes = true;
            }
            con.Close();
            command.Dispose();
            return blnRes;
        }
        private bool existePlanDeCuentasClienteEnQS(string strEmpresa, Int64 ctaContable)
        {
            bool blnRes = false;
            OdbcCommand command = new OdbcCommand();
            command.Connection = con;
            if (con.State == System.Data.ConnectionState.Closed)
            {
                con.Open();
            }
            command.CommandText = "SELECT COUNT(*) FROM MRVF" + strEmpresa + "COM.CTBMC WHERE MCNIV=3 AND MCCTA=" + ctaContable.ToString();
            int rows = (int)command.ExecuteScalar();
            if (rows > 0)
            {
                blnRes = true;
            }
            con.Close();
            command.Dispose();
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
        public bool crearClienteEnQS(string strEmpresa, aspnet_Clientes objCliente, ref string errMessage)
        {
            bool blnRes = true;
            string agenteCliente = "";
            string agenteEmpresa=((strEmpresa=="003" || strEmpresa=="033") ? (objCliente.IDAgenteQSMV ?? 0).ToString() : (strEmpresa == "004" || strEmpresa=="044") ? (objCliente.IDAgenteQSHMA ?? 0).ToString() : (objCliente.IDAgenteQSECA ?? 0).ToString());
            try
            {
                if (!existeClienteEnEmpresaQS(strEmpresa, (decimal)objCliente.QSID, ref agenteCliente))
                {
                    blnRes = insertarClienteEnEmpresaQS(strEmpresa, objCliente,ref errMessage);
                }
                else
                {
                    //Si en este punto el agente empresa aún es 0 y estamos trabajando con la empresa 001, le asignamos uno de los del cliente
                    if (agenteEmpresa == "0" && strEmpresa == "001")
                    {
                        agenteEmpresa = ((objCliente.IDAgenteQSMV ?? 0).ToString() != "0" ? (objCliente.IDAgenteQSMV ?? 0).ToString() : (objCliente.IDAgenteQSHMA ?? 0).ToString() != "0" ? (objCliente.IDAgenteQSHMA ?? 0).ToString() : (objCliente.IDAgenteQSECA ?? 0).ToString());
                    }

                    if (agenteCliente == agenteEmpresa || strEmpresa=="001")
                    {
                        //Solo actualizamos si los agentes son los mismos o la empresa es la 001
                        blnRes = actualizarClienteEnEmpresaQS(strEmpresa, objCliente, ref errMessage);
                    }
                    else
                    {
                        errMessage += Environment.NewLine + "-Error en Model.ClientesModel.CrearClienteEnQS: El cliente " + objCliente.Nombre + " ya existe en la empresa " + strEmpresa + " y tiene asignado un agente distinto al actual. Modifique el agente en QS y vuelva a intentarlo.";
                        blnRes = false;
                    }
                }
            }
            catch(Exception ex)
            {
                blnRes=false;
                errMessage += Environment.NewLine + "-Error en Model.ClientesModel.CrearClienteEnQS: Error al crear el cliente " + objCliente.Nombre + " en la empresa " + strEmpresa + ". " + ex.Message;
            }
            return blnRes;
        }
        public bool borrarClienteEnEmpresaQS(string strEmpresa, decimal? QSID, ref string errMessage)
        {
            bool blnRes = false;
            OdbcCommand command = new OdbcCommand();
            try
            {
                command.Connection = con;
                con.Open();
                
                //Borramos los datos de cliente / municipio
                #region TABLA CLNCM
                command.CommandText = "DELETE FROM MRVF" + strEmpresa + "COM.CLNCM WHERE CMCLN=" + QSID.ToString();
                command.ExecuteNonQuery();
                #endregion

                //Borramos los datos del fotomontaje
                #region TABLA CLNFT
                command.CommandText = "DELETE FROM MRVF" + strEmpresa + "COM.CLNAI WHERE AICDG=" + QSID.ToString();
                command.ExecuteNonQuery();
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

                //Borramos los datos de las direcciones de envío y municipios de las mismas
                #region TABLA CLNDE Y CLNDM
                command.CommandText = "DELETE FROM MRVF" + strEmpresa + "COM.CLNDE WHERE DECDG=" + QSID.ToString();
                command.ExecuteNonQuery();
                command.CommandText = "DELETE FROM MRVF" + strEmpresa + "COM.CLNDM WHERE DMCLN=" + QSID.ToString();
                command.ExecuteNonQuery();
                #endregion

                //Borramos los datos del cliente
                #region TABLA CLNCL
                command.CommandText = "DELETE FROM MRVF" + strEmpresa + "COM.CLNCL WHERE CLCDG=" + QSID.ToString();
                command.ExecuteNonQuery();
                #endregion

            }
            catch (Exception ex)
            {
                blnRes = false;
                errMessage += Environment.NewLine+ "- Error al borrar el cliente " + QSID.ToString() + " de la empresa " + strEmpresa + "." + ex.Message;
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
        private Int64 getCuentaContableClienteQS(aspnet_Clientes objCliente)
        {
            Int64 intCtaContable = 0;
            //Cuenta contable
            if ((objCliente.IDActividadQS ?? "").ToString().Trim() == "EG")
            {
                intCtaContable = Convert.ToInt64("43300" + objCliente.QSID.ToString().PadLeft(5,'0'));
            }
            else
            {
                intCtaContable = Convert.ToInt64("43000" + objCliente.QSID.ToString().PadLeft(5, '0'));
            }
            return intCtaContable;
        }
        
        #endregion

        #region FUNCIONES SQL SERVER
        #region BD CLIENTES
        public void borrarClienteEnBDClientes(aspnet_Clientes objCliente,ref string errMessage)
        {

            CLIENTES_DAR_DE_ALTA_EN_MAC deleted = dbClientes.CLIENTES_DAR_DE_ALTA_EN_MAC.FirstOrDefault(c=>c.NUEVO_CODIGO == objCliente.QSID);
            try
            {
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
            catch (Exception ex)
            {
                errMessage += Environment.NewLine + "- Error en ClientesModel.borrarClienteEnBDClientes al borrar el cliente " + objCliente.Nombre + ": " + ex.Message;
            }
        }
        public bool crearClienteEnBDClientes(aspnet_Clientes objCliente, ref string errMessage)
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
                    //Relleno los campos bit para evitar problemas con el ODBC de ACCESS
                    objNewCli.alerta = false;
                    objNewCli.creditBureau = false;
                    objNewCli.opcAutMoraval = false;
                    objNewCli.opcAutHierros = false;
                    objNewCli.opcAutAltamira = false;
                    objNewCli.opcAutBancoMoraval = false;
                    objNewCli.opcAutBancoHierros = false;
                    objNewCli.opcAutBancoAltamira = false;
                    objNewCli.envFormMoraval = false;
                    objNewCli.envFormHierros = false;
                    objNewCli.envFormAltamira = false;
                    objNewCli.aut_giros_validez_MV = false;
                    objNewCli.aut_giros_validez_HMA = false;
                    objNewCli.aut_giros_validez_ECA = false;
                    objNewCli.sin_fincabilidad = false;
                    objNewCli.localizador_registros = false;
                    objNewCli.chkAutCopiaIBAN = false;
                    objNewCli.opcOtrosAutGiros = false;
                    objNewCli.chkInforma = false;
                    objNewCli.chkAutCopia = false;
                    objNewCli.opcAutMoravalB2B = false;
                    objNewCli.opcAutHierrosB2B = false;
                    objNewCli.opcAutAltamiraB2B = false;
                }
                else
                {
                    objNewCli.opcAutHierros = false;
                    
                    //Si existen registros para el año / cliente lo borramos
                    ACCIONARIADO objAccionariado = objNewCli.ACCIONARIADO.FirstOrDefault(ac => ac.AÑO == System.DateTime.Now.Year && ac.CLIENTE == objCliente.QSID);
                    if (objAccionariado != null)
                    {
                        objAccionariado.ACCIONARIADO_detalle.Clear();
                        objNewCli.ACCIONARIADO.Remove(objAccionariado);
                    }

                    INFORMES_COMERCIALES objInfCom = objNewCli.INFORMES_COMERCIALES.FirstOrDefault(inf => inf.FECHA == Convert.ToDateTime(System.DateTime.Now.ToString("dd/MM/yyyy")) && inf.CLIENTE == objCliente.QSID);

                    if (objInfCom != null)
                    {
                        objNewCli.INFORMES_COMERCIALES.Remove(objInfCom);
                    }

                    objNewCli.SOCIEDADES_VINCULADAS.Clear();
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
                    objNewCliHab.Fecha1 = Convert.ToDateTime(System.DateTime.Now.ToString("dd/MM/yyyy"));
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
                    objNewProvHab.Fecha1 = Convert.ToDateTime(System.DateTime.Now.ToString("dd/MM/yyyy"));
                    objNewProvHab.Cif1 = objProvHab.NIF;
                    objNewProvHab.NombreProveedor1 = objProvHab.NombreProveedor;
                    //Añadimos la entidad al CLIENTE
                    objNewCli.PROVEEDORES_HABITUALES.Add(objNewProvHab);
                }

                //Cargamos los datos que grabaremos en INFORMES_COMERCIALES
                #region DATOS INFORMES COMERCIALES
                //Añadimos los datos correspondientes al informe comercial
                #region INFORME COMERCIAL
                if (objCliente.IDActividadQS != null)
                {
                    //Cargamos la Actividad
                    INSAETableAdapter taAct = new INSAETableAdapter();
                    DSas400.INSAEDataTable dtAct = new DSas400.INSAEDataTable();
                    taAct.FillByID(dtAct, objCliente.IDActividadQS);
                    strInformeComercial += "ACTIVIDAD: " + dtAct.Rows[0]["AENBR"].ToString().Trim() + Environment.NewLine;
                    dtAct.Dispose();
                    taAct.Dispose();
                }
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
                objNewInforme.FECHA = Convert.ToDateTime(System.DateTime.Now.ToString("dd/MM/yyyy"));
                objNewInforme.VALIDAR = true;
                
                //Cargamos el agente asociado al cliente y asignamos su nombre en el campo persona.
                if (objCliente.IDAgenteQSMV != null && objCliente.Empresas.Contains("003"))
                {
                    AGTAG003TableAdapter ta=new AGTAG003TableAdapter();
                    DSas400.AGTAG003DataTable dt = new DSas400.AGTAG003DataTable();
                    ta.FillByID(dt, (decimal)objCliente.IDAgenteQSMV);
                    objNewInforme.PERSONA = dt.Rows[0]["AGNBR"].ToString().Substring(0, (dt.Rows[0]["AGNBR"].ToString().Length > 50 ? 50 : dt.Rows[0]["AGNBR"].ToString().Length)).Trim();
                    ta.Dispose();
                    dt.Dispose();
                }
                else if (objCliente.IDAgenteQSHMA != null && objCliente.Empresas.Contains("004"))
                {
                    AGTAG004TableAdapter ta = new AGTAG004TableAdapter();
                    DSas400.AGTAG004DataTable dt = new DSas400.AGTAG004DataTable();
                    ta.FillByID(dt, (decimal)objCliente.IDAgenteQSHMA);
                    objNewInforme.PERSONA = dt.Rows[0]["AGNBR"].ToString().Substring(0, (dt.Rows[0]["AGNBR"].ToString().Length > 50 ? 50 : dt.Rows[0]["AGNBR"].ToString().Length)).Trim();
                    ta.Dispose();
                    dt.Dispose();
                }
                else if (objCliente.IDAgenteQSECA != null && objCliente.Empresas.Contains("006"))
                {
                    AGTAG006TableAdapter ta = new AGTAG006TableAdapter();
                    DSas400.AGTAG006DataTable dt = new DSas400.AGTAG006DataTable();
                    ta.FillByID(dt, (decimal)objCliente.IDAgenteQSECA);
                    objNewInforme.PERSONA = dt.Rows[0]["AGNBR"].ToString().Substring(0, (dt.Rows[0]["AGNBR"].ToString().Length > 50 ? 50 : dt.Rows[0]["AGNBR"].ToString().Length)).Trim();
                    ta.Dispose();
                    dt.Dispose();
                }

                objNewInforme.TEXTO = strInformeComercial;
                objNewInforme.INMUEBLES = strAlquiler;
                objNewInforme.PROPIETARIO = strPropietario;

                if (objCliente.IDAseguradora != null)
                {
                    objNewInforme.ASEGURA_VENTAS = true;
                    objNewInforme.NO_ASEGURA_VENTAS = false;
                    objNewInforme.ASEGURADORA = objCliente.aspnet_Aseguradoras.Nombre.ToString();
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
                errMessage += Environment.NewLine + "- Error en crearClienteEnBDClientes: " + ex.Message;
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
        public bool crearExcepcionesClienteEnBDPlanificacion(aspnet_Clientes objCliente)
        {
            bool blnRes = true;
            try
            {
                return true;
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

                excCli.DELEGACION = (int)objCliente.DelegacionID;
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