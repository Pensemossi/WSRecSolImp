// using DataAccess;
//using Utilities;
using AccesoDatos; 
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.Services;
using System.Transactions;
using Utilidades;
using WSRecSolImp.Capa_Negocio;
using System.Data.Common;
using System.Web.Services.Protocols;
using System.IO;
using System.Xml.Serialization;

namespace WSRecSolImp
{
    /// <summary>
    /// Descripción breve de WSRecSolImp
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Para permitir que se llame a este servicio web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente. 
    // [System.Web.Script.Services.ScriptService]

    // Define a SOAP header by deriving from the SoapHeader base class.
    // The header contains just one string value.

    //public class MyHeader : SoapHeader
    //{
    //    public Capa_Autenticacion.UserDetails Usuario;
    //    //public string MyValue;
    //    //public string userName;
    //    //public string password;
    //}

    public class WSInicial : System.Web.Services.WebService
    {
        private const string NombreArchivo = "WSRecSolImp";
        private const string ExtensionArchivo = "txt";
        private const string LogFolder = "Logs";
        private static readonly Object lockThis = new Object();

        private IAccesoDatos objDataAccess;
        public Capa_Autenticacion.UserDetails Usuario;   // Variable miembro a recibir el contenido de la cabecera SOAP MyHeader 

        [WebMethod, SoapHeader("Usuario")]
//        public int RecibirSolLicXML(List<SolicitudLicenciaImportacion> lstSolLicVUCE)
        public int RecibirSolLicXML(String SolLicImpoVUCE)
        {
            int Respuesta = 1;
            bool blnUsuarioAuth = false;
            objDataAccess = new AccesoBaseDatos();

            CargueParametrosConfiguracion();
            if (ValidacionConexionBaseDatos() == false) Respuesta = 0;

            if (Respuesta != 0)
            {
                if (Usuario != null)
                {
                    // SQL para consultar si el usuario es autenticado. El valor de 1 es OK. 0 no es autenticado.
                    // Mayor que uno puede indicar que esta en más de una aplicación. Está solución es temporal.
                    string sql_auth = "SELECT COUNT(*) UsuarioAutenticado FROM " + Utilidades.Configuracion.Esquema_FACTORYSUITE_WS + ".ora_aspnet_users usuarios INNER JOIN " + Utilidades.Configuracion.Esquema_FACTORYSUITE_WS + ".ora_aspnet_membership ms ON usuarios.userid = ms.userid " +
                                      "WHERE usuarios.username = '" + Usuario.usuario + "' AND ms.password = '" + Usuario.password + "'";

                    try
                    {

                        objDataAccess.Conectar(Utilidades.Configuracion.ConexionFactorySuite, Utilidades.Configuracion.ProveedorFactorySuite);
                        DataTable dtAuthUsua = objDataAccess.Consultar(sql_auth);
                        long lNumAuto = Convert.ToInt64(dtAuthUsua.Rows[0].ItemArray[0]);

                        if (lNumAuto == 1)
                            blnUsuarioAuth = true;

                        objDataAccess.DesConectar();

                        if (blnUsuarioAuth) WriteToFile("Validación de usuario exitosa.");
                        else WriteToFile("Validación de usuario fallida.");
                    }
                    catch (Exception objException)
                    {
                        Respuesta = 0;
                        objDataAccess.DesConectar();

                        WriteToFile("No es posible acceder a las tablas USR_FACTORYSUITE.ora_aspnet_users usuarios y USR_FACTORYSUITE.ora_aspnet_membership.");
                        WriteToFile(objException.Message);
                        string strStackTrace = (objException.InnerException != null ? (objException.InnerException.StackTrace != null ? objException.InnerException.StackTrace.ToString() : "-") : (objException.StackTrace != null ? objException.StackTrace.ToString() + "." : "-"));
                        strStackTrace = (strStackTrace.Length > 4000 ? strStackTrace.Substring(0, 4000) : strStackTrace + ".");
                        WriteToFile(strStackTrace);

                        // Guardar en el log de errores de Factory Suite
                        GuardarLogErroresFS(objException);
                    }

                }

                if (blnUsuarioAuth) Respuesta = ManejoSolictudes(SolLicImpoVUCE);
                else Respuesta = 0;
            }

            return Respuesta;
        }

        private int ManejoSolictudes(String SolLicImpoVUCE)
        {
            Serializer ser = new Serializer();
            SolicitudLicenciaImportacion objSolLicImp;

            int Respuesta = 0;
            long IdSolicitud ;
            long IdClaseImportador = -1;
            string strReapertura;
            string strNumTempSol;   // Número de formulario
            string strNumLigRegFinal;
            string strTipoSolicitud;
            string strNitImportador;
            string strNombreImportador;
            string strClaseImportador;
            string strTipoCancelacion;
            string strMotivoCancelacion = "";
            string strVia;
            string strOtraVia;
            string strAduana;
            string strPuertosEmbarque;
            string strOtroPuertoEmbarque;
            string strPaisCompra;
            string strValorFOB;
            ClsSolicitudesEspeciales SolsEsps;
            ClsSubPartidas SubPartidas;


            WriteToFile("XML de Solicitud de Importación: ");
            WriteToFile(SolLicImpoVUCE);

            objSolLicImp = ser.Deserialize<SolicitudLicenciaImportacion>(SolLicImpoVUCE);
            if (objSolLicImp != null)
            {
                // Detallar la solicitud
                WriteToFile("Deserialización del XML de Solicitud de Importación exitosa.");
                strReapertura = objSolLicImp.Reapertura.ToString();
                strNumTempSol = objSolLicImp.NumTemporalSol.ToString();
                strNumLigRegFinal = objSolLicImp.NumLigRegFinal.ToString();
                strTipoSolicitud = objSolLicImp.TipoSolicitud.ToString();
                strNitImportador = objSolLicImp.NitImportador.ToString();
                strNombreImportador = objSolLicImp.NomImportador.ToString(); //.Replace("&", "_y_");
                strClaseImportador = objSolLicImp.ClaseImportador.ToString();
                strTipoCancelacion = objSolLicImp.TipoCancelacion.ToString();
                if (!(objSolLicImp.MotivoCancelacion is null)) strMotivoCancelacion = objSolLicImp.MotivoCancelacion.ToString();
                strVia = objSolLicImp.Via.ToString();
                strOtraVia = objSolLicImp.OtraVia.ToString();
                strAduana = objSolLicImp.Aduana.ToString();
                strPuertosEmbarque = objSolLicImp.PuertoEmbarque.ToString();
                strOtroPuertoEmbarque = objSolLicImp.OtroPuertoEmbarque.ToString();
                strPaisCompra = objSolLicImp.PaisCompra.ToString();
                strValorFOB = objSolLicImp.ValorFOB.ToString();
                SolsEsps = objSolLicImp.SolicitudesEspeciales;
                SubPartidas = objSolLicImp.Subpartidas;

                // Algunas Validaciones, Otras se realizan GuardarSolicitudNueva
               
                Respuesta = ValidarTipoSolicitud(strReapertura, ref strTipoSolicitud, strNumTempSol);

                if (Respuesta != 0)
                {
                    Respuesta = ValidarClaseImportador(strClaseImportador, strNumTempSol, out IdClaseImportador);

                    if (Respuesta != 0)
                    {
                        Respuesta = ValidarTipoCancelacion(strTipoCancelacion, strNumTempSol);

                        if (Respuesta != 0)
                        {
                            // Inserta cabecera de la solicitud y recuperar el ID con que se guardo
                            Respuesta = GuardarSolicitudNueva(strReapertura, 
                                                    strNumTempSol, 
                                                    strNumLigRegFinal,
                                                    strTipoSolicitud,
                                                    strNitImportador,
                                                    strNombreImportador,
                                                    IdClaseImportador,
                                                    strTipoCancelacion,
                                                    strMotivoCancelacion,
                                                    strVia,
                                                    strOtraVia,
                                                    strAduana,
                                                    strOtroPuertoEmbarque,
                                                    strPaisCompra,
                                                    strValorFOB,
                                                    out IdSolicitud
                                                );

                            if (Respuesta != 0)
                            {
                                // Insertar Puertos de Embarque
                                ValidarInsertarPuertos(strPuertosEmbarque, strNumTempSol, IdSolicitud);

                                // Insertar Solicitudes Especiales
                                InsertarSolicitudesEspeciales(SolsEsps, strNumTempSol, IdSolicitud);

                                // Insertar Subpartidas, Items de subpartidas (productos) y sus países de Origen 
                                InsertarSubpartidas(SubPartidas, strNumTempSol, IdSolicitud);

                                // Insertar Solicitud en la cola para que se active el flujo   
                                InsertarColaWF(strReapertura, strNumTempSol, IdSolicitud);
                            }
                        }
                    }
                }
            }
            else
            {
                Respuesta = 0;
                WriteToFile("Falló la Deserialización de la Solicitud de Importación.");
            }

            return Respuesta;
        }

        /// <summary>
        /// Funcion para Guardar las Solicitudes Nuevas registradas en VUCE 2
        /// </summary>
        /// <param name="strNumTempSol"></param>
        /// <param name="strTipoSolicitud"></param>
        /// <param name="strNitImportador"></param>
        /// <param name="strNombreImportador"></param>
        /// <param name="IdClaseImportador"></param>
        /// <param name="strTipoCancelacion"></param>
        /// <param name="strMotivoCancelacion"></param>
        /// <param name="strVia"></param>
        /// <param name="strOtraVia"></param>
        /// <param name="strAduana"></param>
        /// <param name="strOtroPuertoEmbarque"></param>
        /// <param name="strPaisCompra"></param>
        /// <param name="strValorFOB"></param>
        /// <param name="IdSolicitud"></param>
        public int GuardarSolicitudNueva( string strReapertura, 
                                          string strNumTempSol,
                                          string strNumLigRegFinal,
                                          string strTipoSolicitud,
                                          string strNitImportador,
                                          string strNombreImportador,
                                          long IdClaseImportador,
                                          string strTipoCancelacion,
                                          string strMotivoCancelacion,
                                          string strVia,
                                          string strOtraVia,
                                          string strAduana,
                                          string strOtroPuertoEmbarque,
                                          string strPaisCompra,
                                          string strValorFOB,
                                          out long IdSolicitud
                                         )
        {
            int Respuesta = 1;
            long IdViaTransporte = -1;
            long IdAduana = -1;
            // long IdPuertoEmbarque = -1;   // Ojo se guarda en una tabla muchos a muchos 
            long IdPais = -1;
            long IdLicencia = -1;
            IdSolicitud = -1;
            string strInsertCESOLICITUDES;

            IdViaTransporte = ValidarViaTransporte(strVia, strNumTempSol, out Respuesta);

            if (Respuesta != 0)
                if (strTipoSolicitud == "2" || strTipoSolicitud == "3")
                    Respuesta =  ValidarLicencia(strNumLigRegFinal, strNumTempSol, out IdLicencia);

            if (Respuesta != 0)
            {
                IdAduana = ValidarAduana(strAduana, strNumTempSol, out Respuesta);

                if (Respuesta != 0)
                {
                    IdPais = ValidarInsertarPais(strPaisCompra, strNumTempSol, out Respuesta);

                    if (Respuesta != 0)
                    {
                        if (strReapertura == "0")
                        {
                            try
                            {
                                switch (strTipoSolicitud)
                                {
                                    // Solicitudes nuevas
                                    case "1":
                                        strInsertCESOLICITUDES = "BEGIN INSERT INTO " + Utilidades.Configuracion.Esquema_SICOQ_WS + ".CE_SOLICITUD_IMPORTACIONES(NUMEROFORMULARIO, IDTIPOSOLICITUD, NUMEROSOLICITUD, NITIMPORTADOR,  " +
                                                                                                        "NOMBREIMPORTADOR, IDCLASEIMPORTADOR, IDTIPOCANCELACION, MOTIVOCANCELACION, IDVIATRANSPORTE, OTRAVIA, IDADUANA, OTROPUERTOEMBARQUE, IDPAISCOMPRA, TOTALUSD ) VALUES " +
                                                                                    "('" + strNumTempSol + "', " +
                                                                                    Convert.ToInt64(strTipoSolicitud) + ", '" +
                                                                                    strNumLigRegFinal + "', '" +
                                                                                    strNitImportador + "', '" +
                                                                                    strNombreImportador + "', " +
                                                                                    IdClaseImportador + ", " +
                                                                                    "DECODE(" + strTipoCancelacion + ", 0, NULL, " + strTipoCancelacion + ") , '" +
                                                                                    strMotivoCancelacion + "', " +
                                                                                    IdViaTransporte + ", '" +
                                                                                    strOtraVia + "', " +
                                                                                    IdAduana + ", '" +
                                                                                    strOtroPuertoEmbarque + "', " +
                                                                                    IdPais + ", " +
                                                                                    strValorFOB + " )  Returning IdSolicitud into :Id; END; ";
                                        break;

                                    // Solicitudes de Modificación
                                    case "2":
                                        strInsertCESOLICITUDES = "BEGIN INSERT INTO " + Utilidades.Configuracion.Esquema_SICOQ_WS + ".CE_SOLICITUD_IMPORTACIONES(NUMEROFORMULARIO, IDTIPOSOLICITUD, NUMEROSOLICITUD, NITIMPORTADOR,  " +
                                                                                                        "NOMBREIMPORTADOR, IDCLASEIMPORTADOR, IDTIPOCANCELACION, MOTIVOCANCELACION, IDVIATRANSPORTE, OTRAVIA, IDADUANA, OTROPUERTOEMBARQUE, IDPAISCOMPRA, TOTALUSD ) VALUES " +
                                                                                    "('" + strNumTempSol + "', " +
                                                                                    Convert.ToInt64(strTipoSolicitud) + ", '" +
                                                                                    strNumLigRegFinal + "', '" +
                                                                                    strNitImportador + "', '" +
                                                                                    strNombreImportador + "', " +
                                                                                    IdClaseImportador + ", " +
                                                                                    "DECODE(" + strTipoCancelacion + ", 0, NULL, " + strTipoCancelacion + ") , '" +
                                                                                    strMotivoCancelacion + "', " +
                                                                                    IdViaTransporte + ", '" +
                                                                                    strOtraVia + "', " +
                                                                                    IdAduana + ", '" +
                                                                                    strOtroPuertoEmbarque + "', " +
                                                                                    IdPais + ", " +
                                                                                    strValorFOB + " )  Returning IdSolicitud into :Id; END; ";
                                        break;

                                    // Solicitudes de Cancelación
                                    case "3":
                                        strInsertCESOLICITUDES = "BEGIN INSERT INTO " + Utilidades.Configuracion.Esquema_SICOQ_WS + ".CE_SOLICITUD_IMPORTACIONES(NUMEROFORMULARIO, IDTIPOSOLICITUD, NUMEROSOLICITUD, NITIMPORTADOR,  " +
                                                                                                        "NOMBREIMPORTADOR, IDCLASEIMPORTADOR, IDTIPOCANCELACION, MOTIVOCANCELACION, IDVIATRANSPORTE, OTRAVIA, IDADUANA, OTROPUERTOEMBARQUE, IDPAISCOMPRA, TOTALUSD, TOTALUSDACTUAL ) VALUES " +
                                                                                    "('" + strNumTempSol + "', " +
                                                                                    Convert.ToInt64(strTipoSolicitud) + ", '" +
                                                                                    strNumLigRegFinal + "', '" +
                                                                                    strNitImportador + "', '" +
                                                                                    strNombreImportador + "', " +
                                                                                    IdClaseImportador + ", " +
                                                                                    "DECODE(" + strTipoCancelacion + ", 0, NULL, " + strTipoCancelacion + ") , '" +
                                                                                    strMotivoCancelacion + "', " +
                                                                                    IdViaTransporte + ", '" +
                                                                                    strOtraVia + "', " +
                                                                                    IdAduana + ", '" +
                                                                                    strOtroPuertoEmbarque + "', " +
                                                                                    IdPais + ", " +
                                                                                    strValorFOB + ", " +
                                                                                    strValorFOB + " )  Returning IdSolicitud into :Id; END; ";
                                        break; 

                                    default:
                                        strInsertCESOLICITUDES = "No Válido";
                                        WriteToFile("Se detecto un Tipo de Solicitud no válido [" + strTipoSolicitud + "] en GuardarSolicitudNueva.");
                                        Respuesta = 0;
                                        return Respuesta; 
                                }

                                DbParameter[] dbParametro = new DbParameter[1];
                                dbParametro[0] = objDataAccess.CrearParametro(ParameterDirection.Output, 1, DbType.Int64, ":Id");
                                objDataAccess.Conectar(Utilidades.Configuracion.ConexionFactorySuite, Utilidades.Configuracion.ProveedorFactorySuite);

                                WriteToFile("Listo para Insertar nueva Solicitud de Importación.");
                                if (strTipoSolicitud == "2" || strTipoSolicitud == "3")
                                    objDataAccess.EjecutaComando("ALTER TRIGGER " + Utilidades.Configuracion.Esquema_SICOQ_WS + ".TR_SOLICITUD_MODIFICACION DISABLE");
                                objDataAccess.EjecutaComando(dbParametro, strInsertCESOLICITUDES);
                                if (strTipoSolicitud == "2" || strTipoSolicitud == "3")
                                    objDataAccess.EjecutaComando("ALTER TRIGGER " + Utilidades.Configuracion.Esquema_SICOQ_WS + ".TR_SOLICITUD_MODIFICACION ENABLE");
                                WriteToFile("Insertada nueva Solicitud de Importación.");

                                // Recuperar el IDSOLICITUD recien creada
                                IdSolicitud = Convert.ToInt64(dbParametro[0].Value);
                                objDataAccess.DesConectar();

                                Respuesta = 1;
                            }
                            catch (Exception e)
                            {
                                Respuesta = 0;

                                if (strTipoSolicitud == "2" || strTipoSolicitud == "3")
                                    objDataAccess.EjecutaComando("ALTER TRIGGER " + Utilidades.Configuracion.Esquema_SICOQ_WS + ".TR_SOLICITUD_MODIFICACION ENABLE");
                                objDataAccess.DesConectar();
                                WriteToFile("Insertada de nueva Solicitud de Importación fallida.");
                                WriteToFile(e.Message);

                                // Guardar en el log de errores de Factory Suite
                                GuardarLogErroresFS(e);
                            }
                        }
                        else
                        {
                            // Cambiar tipo de solicitud a Reapertura   4
                            // Fecha de Reapertura   SYSDATE
                            // Observaciones Raapertura
                            // Usuario Reapertura  UserId = $UserId$ --->  cast(UserId as varchar(36)) UserId, UserName FROM VW_ASPNET_USERS 
                           
                            string strUpdateCESOLICITUDES = "UPDATE " + Utilidades.Configuracion.Esquema_SICOQ_WS + ".CE_SOLICITUD_IMPORTACIONES " +
                                                               "SET IDTIPOSOLICITUD = 4 ," +
                                                                  " FECHAREAPERTURA = sysdate ," +
                                                                  " OBSERVACIONESREAPERTURA = 'Reapertura realizada a través del web service.' ," +
                                                                  " IDUSUARIOREAPERTURA = (SELECT UserId FROM " + Utilidades.Configuracion.Esquema_FACTORYSUITE_WS + ".ora_aspnet_users WHERE UserName = '" + Usuario.usuario + "') "  + 
                                                             "WHERE NUMEROFORMULARIO = '" + strNumTempSol + "'" ;

                            try
                            {
                                WriteToFile("Listo para realizar la reapertura de la Solicitud de Importación: " + strNumTempSol);
                                objDataAccess.Conectar(Utilidades.Configuracion.ConexionFactorySuite, Utilidades.Configuracion.ProveedorFactorySuite);
                                objDataAccess.EjecutaComando(strUpdateCESOLICITUDES);
                                objDataAccess.DesConectar();

                                WriteToFile("Reapertura de Solicitud de Importación exitosa.");
                                Respuesta = 1;
                            }
                            catch (Exception e)
                            {
                                Respuesta = 0;
                                objDataAccess.DesConectar();
                                WriteToFile(e.Message);

                                // Guardar en el log de errores de Factory Suite
                                WriteToFile("Reapertura de Solicitud de Importación fallida.");
                                GuardarLogErroresFS(e);
                            }
                        }
                    }
                }
            }

            return Respuesta;
        }

        private void CargueParametrosConfiguracion()
        {
            // Cargue del ConnectionString y el ProviderName
            Utilidades.Configuracion.ConexionFactorySuite = ConfigurationManager.ConnectionStrings["ORCL_QAS"].ConnectionString;
            Utilidades.Configuracion.ProveedorFactorySuite = ConfigurationManager.ConnectionStrings["ORCL_QAS"].ProviderName;
            Utilidades.Configuracion.Usuario_SICOQ_WS = ConfigurationManager.AppSettings["Usuario_SICOQ_WS"];
            //Utilidades.Configuracion.Password_SICOQ_WS = ConfigurationManager.AppSettings["Password_SICOQ_WS"];
            Utilidades.Configuracion.Esquema_SICOQ_WS = ConfigurationManager.AppSettings["Esquema_SICOQ_WS"];
            Utilidades.Configuracion.Esquema_FACTORYSUITE_WS = ConfigurationManager.AppSettings["Esquema_FACTORYSUITE_WS"];
            Utilidades.Configuracion.CodigoSolicitudLicencia = ConfigurationManager.AppSettings["CodigoSolicitudLicencia"];
            Utilidades.Configuracion.CodigoSolicitudModifLic = ConfigurationManager.AppSettings["CodigoSolicitudModifLic"];
            Utilidades.Configuracion.CodigoSolicitudCanceLic = ConfigurationManager.AppSettings["CodigoSolicitudCanceLic"];
            Utilidades.Configuracion.FactorySuiteProxy = ConfigurationManager.AppSettings["FactorySuiteProxy"];
        }

        //METODO PARA VALIDAR CONEXION CON BASE DE DATOS
        private bool ValidacionConexionBaseDatos()
        {
            bool blnValidacionConexionBaseDatos = false;

            try
            {
                objDataAccess.Conectar(Utilidades.Configuracion.ConexionFactorySuite, Utilidades.Configuracion.ProveedorFactorySuite);
                DataTable dtTestBD = objDataAccess.Consultar("select sysdate from dual");

                if (dtTestBD.Rows.Count > 0)
                    blnValidacionConexionBaseDatos = true;
                WriteToFile("Acceso a la Base de datos SICOQ exitoso.");

                objDataAccess.DesConectar();
            }
            catch (Exception objException)
            {
                WriteToFile("No es posible acceder a la Base de datos SICOQ");
                WriteToFile(objException.Message);
            }

            return blnValidacionConexionBaseDatos;
        }

        private void InsertarEntradaLogFS(string strCodigo, string strDescripcion, string strOrigen,
                                          string strTipo, string strTraduccion, string strTraza,
                                          string strMetodo, string strInstruccion)
        {
            // Construcción del SQL INSERT  -- Se asume que el que llama abre y cierra la conexión y controla si hay errores 
            string strQueryLog = " INSERT INTO " + Utilidades.Configuracion.Esquema_FACTORYSUITE_WS + ".FSLogErrores "
                                 + "  (Codigo      "
                                 + "  ,Descripcion "
                                 + "  ,Origen "
                                 + "  ,Tipo  "
                                 + "  ,Traduccion "
                                 + "  ,IdFormulario "
                                 + "  ,UserId "
                                 + "  ,Traza "
                                 + "  ,Metodo "
                                 + "  ,Instruccion "
                                 + "  ,IdWorkflowInstancia "
                                 + "  ,IdWorkflowCola) "
                                 + "VALUES "
                                 + "  ( '" + strCodigo + "'"
                                 + "  , '" + strDescripcion + "' "
                                 + "  , '" + strOrigen + "' "
                                 + "  , '" + strTipo + "' "
                                 + "  , '" + strTraduccion + "' "
                                 + "  , null "
                                 + "  , null "
                                 + "  , '" + strTraza + "' "
                                 + "  , '" + strMetodo + "' "
                                 + "  , '" + strInstruccion + "'"
                                 + "  , null "
                                 + "  , null ) ";

            WriteToFile("Listo para Insertar entrada en FSLogErrores.");

            try
            {
                objDataAccess.EjecutaComando(strQueryLog);
            }
            catch (Exception objException)
            {
                WriteToFile("No es posible acceder escribir a la tabla FSLogErrores");
                WriteToFile(objException.Message);
            }

            WriteToFile("Insertando en la tabla [FSLogErrores].");
        }

        //Guardar en el log de errores
        private void GuardarLogErroresFS(Exception ex)
        {
            string strCodigo;
            string strMensajeError = string.Empty;
            string strMensajeErrorInterno = string.Empty;

            strMensajeError = (ex.Message != null ? ex.Message.ToString() : " ");
            strMensajeErrorInterno = (ex.InnerException != null ? ex.InnerException.Message.ToString() : "");

            if (!strMensajeError.Equals(strMensajeErrorInterno))
            {
                strMensajeError = String.Format("{0}\n ------  \n {1}", ex.Message, strMensajeErrorInterno);
            }
            strCodigo = ex.HResult.ToString();

            string strMensajeOrigen = (ex.InnerException != null ? (ex.InnerException.Source != null ? ex.InnerException.Source.ToString() : "-") : (ex.Source != null ? ex.Source.ToString() + "." : "-"));
            string strMensajeTipo = (ex.InnerException != null ? (ex.InnerException.GetType() != null ? ex.InnerException.GetType().ToString() : "-") : (ex.GetType().GetType() != null ? ex.GetType().GetType().ToString() + "." : "-"));
            string strStackTrace = (ex.InnerException != null ? (ex.InnerException.StackTrace != null ? ex.InnerException.StackTrace.ToString() : "-") : (ex.StackTrace != null ? ex.StackTrace.ToString() + "." : "-"));
            strStackTrace = (strStackTrace.Length > 4000 ? strStackTrace.Substring(0, 4000) : strStackTrace + ".");
            string strTargetSite = (ex.InnerException != null ? (ex.InnerException.TargetSite != null ? ex.InnerException.TargetSite.ToString() : "-") : (ex.TargetSite != null ? ex.TargetSite.ToString() + "." : "-"));

            objDataAccess.Conectar(Utilidades.Configuracion.ConexionFactorySuite, Utilidades.Configuracion.ProveedorFactorySuite);
            InsertarEntradaLogFS(strCodigo, strMensajeError, strMensajeOrigen,
                              strMensajeTipo, strMensajeError, strStackTrace,
                              strTargetSite, strTargetSite);
            objDataAccess.DesConectar();
        }

        private int ValidarTipoSolicitud(string strReapertura, ref string strTipoSolicitud, string strNumTempSol)
        {
            int Respuesta = 0;

            // Validar el valor de la reapertura 
            if (strReapertura == "0" || strReapertura == "1")
            {
                // Validar el valor del tipo de solicitud          
                if (strTipoSolicitud == "1" || strTipoSolicitud == "2" || strTipoSolicitud == "3")
                {
                    //if (strReapertura == "1") strTipoSolicitud = "4";   // Se tiene en cuenta al insertar en la cola 
                    Respuesta = 1;
                    WriteToFile("Formulario de Solicitud " + strNumTempSol + ".");
                    WriteToFile("Validación de Tipo de Solicitud exitosa.");
                }
                else
                {
                    // Error: Tipo de solicitud no válido                      }
                    try
                    {
                        WriteToFile("Validación de Tipo de Solicitud fallida.");
                        Respuesta = 0;

                        string strCodigo = "30002";
                        string strDescripcion = "El valor de Tipo de Solicitud no es valido. Tipo de Solicitud = ''" + strTipoSolicitud + "''";
                        string strOrigen = "Web Service para recibir las solicitudes de importación - Solicitud Temporal: " + strNumTempSol;
                        string strTipo = "Validación de la solicitud de importación en el Web Service";
                        string strTraduccion = "No se puede continuar sin un valor de Tipo de Solicitud valido [1,2,3]";
                        string strTraza = "WSINicial.asmx";
                        string strMetodo = "public int ManejoSolictudes";
                        string strInstruccion = "";

                        objDataAccess.Conectar(Utilidades.Configuracion.ConexionFactorySuite, Utilidades.Configuracion.ProveedorFactorySuite);
                        InsertarEntradaLogFS(strCodigo, strDescripcion, strOrigen,
                                                strTipo, strTraduccion, strTraza,
                                                strMetodo, strInstruccion);
                        objDataAccess.DesConectar();
                    }
                    catch (Exception e)
                    {
                        Respuesta = 0;
                        objDataAccess.DesConectar();
                        WriteToFile(e.Message);

                        // Guardar en el log de errores de Factory Suite
                        GuardarLogErroresFS(e);
                    }
                }
            }
            else
            {
                // Error: Reapertura no válido
                try
                {
                    WriteToFile("Validación de Reapertura fallida.");
                    Respuesta = 0;

                    string strCodigo = "30001";
                    string strDescripcion = "El valor de Reapertura no es valido. Reapertura = ''" + strReapertura + "''";
                    string strOrigen = "Web Service para recibir las solicitudes de importación - Solicitud Temporal: " + strNumTempSol;
                    string strTipo = "Validación de la solicitud de importación en el Web Service";
                    string strTraduccion = "No se puede continuar sin un valor de Reapertura valido [0,1]";
                    string strTraza = "WSINicial.asmx";
                    string strMetodo = "public int ManejoSolictudes";
                    string strInstruccion = "";

                    objDataAccess.Conectar(Utilidades.Configuracion.ConexionFactorySuite, Utilidades.Configuracion.ProveedorFactorySuite);
                    InsertarEntradaLogFS(strCodigo, strDescripcion, strOrigen,
                                            strTipo, strTraduccion, strTraza,
                                            strMetodo, strInstruccion);
                    objDataAccess.DesConectar();
                }
                catch (Exception e)
                {
                    Respuesta = 0;
                    objDataAccess.DesConectar();
                    WriteToFile(e.Message);

                    // Guardar en el log de errores de Factory Suite
                    GuardarLogErroresFS(e);
                }
            }

            return Respuesta;
        }

        private int ValidarClaseImportador(string strClaseImportador, string strNumTempSol, out long IdClaseImportador)
        {
            string strQueryIdClaseImportador = "SELECT IDCLASEIMPORTADOR FROM " + Utilidades.Configuracion.Esquema_SICOQ_WS + ".CE_CLASE_IMPORTADORES WHERE CODIGO = '" + strClaseImportador + "'";
            int Respuesta = 1;  //Publica = 1, Privada = 2 , Mixta = 3
            IdClaseImportador = -1;

            if (strClaseImportador == "1" || strClaseImportador == "2" || strClaseImportador == "3")
            {
                try
                {
                    objDataAccess.Conectar(Utilidades.Configuracion.ConexionFactorySuite, Utilidades.Configuracion.ProveedorFactorySuite);
                    DataTable dtIDClaseImportador = objDataAccess.Consultar(strQueryIdClaseImportador);

                    if (dtIDClaseImportador.Rows.Count > 0)
                    {
                        IdClaseImportador = Convert.ToInt64(dtIDClaseImportador.Rows[0].ItemArray[0]);
                        Respuesta = 1;
                        WriteToFile("Validación de Clase de Importador exitosa.");
                    }
                    else
                    {
                        try
                        {
                            WriteToFile("El valor de Clase de Importador no esta registrado. Clase de Importador = ''" + strClaseImportador + "''");
                            Respuesta = 0;

                            string strCodigo = "30003";
                            string strDescripcion = "El valor de Clase de Importador no esta registrado. Clase de Importador = ''" + strClaseImportador + "''";
                            string strOrigen = "Web Service para recibir las solicitudes de importación - Solicitud Temporal: " + strNumTempSol;
                            string strTipo = "Validación de la solicitud de importación en el Web Service";
                            string strTraduccion = "No se puede continuar sin un valor de Clase de Importador valido [Oficial, Privada]";
                            string strTraza = "WSINicial.asmx";
                            string strMetodo = "public int ValidarClaseImportador";
                            string strInstruccion = "";

                            InsertarEntradaLogFS(strCodigo, strDescripcion, strOrigen,
                                              strTipo, strTraduccion, strTraza,
                                              strMetodo, strInstruccion);
                        }
                        catch (Exception e)
                        {
                            objDataAccess.DesConectar();
                            WriteToFile(e.Message);

                            // Guardar en el log de errores de Factory Suite
                            GuardarLogErroresFS(e);
                        }
                    }

                    objDataAccess.DesConectar();
                }
                catch (Exception ex)
                {
                    Respuesta = 0;
                    objDataAccess.DesConectar();
                    WriteToFile(ex.Message);

                    // Guardar en el log de errores de Factory Suite
                    GuardarLogErroresFS(ex);
                }
            }
            else
            {
                try
                {
                    WriteToFile("Validación de Clase de Importador fallida.");
                    Respuesta = 0;

                    string strCodigo = "30012";
                    string strDescripcion = "El valor de Clase de Importador no es válido. Clase de Importador = ''" + strClaseImportador + "''";
                    string strOrigen = "Web Service para recibir las solicitudes de importación - Solicitud Temporal: " + strNumTempSol;
                    string strTipo = "Validación de la solicitud de importación en el Web Service";
                    string strTraduccion = "No se puede continuar sin un valor de Clase de Importador valido [Oficial, Privada]";
                    string strTraza = "WSINicial.asmx";
                    string strMetodo = "public int ValidarClaseImportador";
                    string strInstruccion = "";

                    objDataAccess.Conectar(Utilidades.Configuracion.ConexionFactorySuite, Utilidades.Configuracion.ProveedorFactorySuite);
                    InsertarEntradaLogFS(strCodigo, strDescripcion, strOrigen,
                                      strTipo, strTraduccion, strTraza,
                                      strMetodo, strInstruccion);
                    objDataAccess.DesConectar();
                }
                catch (Exception e)
                {
                    objDataAccess.DesConectar();
                    WriteToFile(e.Message);

                    // Guardar en el log de errores de Factory Suite
                    GuardarLogErroresFS(e);
                }
            }

            return Respuesta;
        }

        private int ValidarLicencia(string strNumLigRegFinal, string strNumTempSol, out long IdLicencia)
        {
            string strQueryIdLicencia = "SELECT IDLICENCIA FROM " + Utilidades.Configuracion.Esquema_SICOQ_WS + ".CE_LICENCIAS WHERE NUMEROLICENCIA = '" + strNumLigRegFinal + "'";
            int Respuesta = 0;  
            IdLicencia = -1;

            try
            {
                // Recuperar el IdViaTransporte a insertar
                objDataAccess.Conectar(Utilidades.Configuracion.ConexionFactorySuite, Utilidades.Configuracion.ProveedorFactorySuite);
                DataTable dtIDLicencia = objDataAccess.Consultar(strQueryIdLicencia);

                if (dtIDLicencia.Rows.Count > 0)
                {
                    IdLicencia = Convert.ToInt64(dtIDLicencia.Rows[0].ItemArray[0]);
                    Respuesta = 1;
                    WriteToFile("Validando Licencia exitosa.");
                }
                else
                {
                    try
                    {
                        WriteToFile("Validando Licencia fallida.");
                        Respuesta = 0;

                        string strCodigo = "30013";
                        string strDescripcion = "El valor de la Licencia a modificar o cancelar no es existe en la BD. Licencia = ''" + strNumLigRegFinal + "''";
                        string strOrigen = "Web Service para recibir las solicitudes de importación - Solicitud Temporal: " + strNumTempSol;
                        string strTipo = "Validación de la solicitud de importación en el Web Service";
                        string strTraduccion = "No se puede continuar sin una Licencia valido [registrada en la BD]";
                        string strTraza = "WSINicial.asmx";
                        string strMetodo = "public int ValidarLicencia";
                        string strInstruccion = "";

                        InsertarEntradaLogFS(strCodigo, strDescripcion, strOrigen,
                                            strTipo, strTraduccion, strTraza,
                                            strMetodo, strInstruccion);
                    }
                    catch (Exception e)
                    {
                        objDataAccess.DesConectar();
                        WriteToFile(e.Message);

                        // Guardar en el log de errores de Factory Suite
                        GuardarLogErroresFS(e);
                    }

                }

                objDataAccess.DesConectar();
            }
            catch (Exception ex)
            {
                Respuesta = 0;
                WriteToFile(ex.Message);
                objDataAccess.DesConectar();

                // Guardar en el log de errores de Factory Suite
                GuardarLogErroresFS(ex);
            }

            return Respuesta;
        }


        private int ValidarTipoCancelacion(string strTipoCancelacion, string strNumTempSol)
        {
            int Respuesta = 1;

            // Validar el valor de Tipo de Cancelación   
            if (!(strTipoCancelacion == "0" || strTipoCancelacion == "1" || strTipoCancelacion == "2"))
            {
                // Error: Tipo de Cancelación no válido                      }
                try
                {
                    WriteToFile("Validación de Tipo de Cancelación exitosa.");
                    Respuesta = 0;

                    string strCodigo = "30004";
                    string strDescripcion = "El valor de Tipo de Cancelacion no es valido. Tipo de Cancelación = ''" + strTipoCancelacion + "''";
                    string strOrigen = "Web Service para recibir las solicitudes de importación - Solicitud Temporal: " + strNumTempSol;
                    string strTipo = "Validación de la solicitud de importación en el Web Service";
                    string strTraduccion = "No se puede continuar sin un valor de Tipo de Cancelación valido [Oficial, Privada]";
                    string strTraza = "WSINicial.asmx";
                    string strMetodo = "public int ManejoSolictudes";
                    string strInstruccion = "";

                    objDataAccess.Conectar(Utilidades.Configuracion.ConexionFactorySuite, Utilidades.Configuracion.ProveedorFactorySuite);
                    InsertarEntradaLogFS(strCodigo, strDescripcion, strOrigen,
                                            strTipo, strTraduccion, strTraza,
                                            strMetodo, strInstruccion);
                    objDataAccess.DesConectar();
                }
                catch (Exception e)
                {
                    Respuesta = 0;
                    objDataAccess.DesConectar();
                    WriteToFile(e.Message);

                    // Guardar en el log de errores de Factory Suite
                    GuardarLogErroresFS(e);
                }
            }
            else WriteToFile("Validación de Tipo de Cancelación fallida.");

            return Respuesta;
        }

        private long ValidarInsertarPais(string strPaisCompra, string strNumTempSol, out int Respuesta)
        {
            string strQueryIdPais = "SELECT IDPAIS FROM " + Utilidades.Configuracion.Esquema_SICOQ_WS + ".CE_PAISES WHERE CODIGO = '" + strPaisCompra + "'";
            long IdPais = -1;
            Respuesta = 0;

            try
            {
                // Recuperar el IdPaisCompra a insertar
                objDataAccess.Conectar(Utilidades.Configuracion.ConexionFactorySuite, Utilidades.Configuracion.ProveedorFactorySuite);
                DataTable dtIDPais = objDataAccess.Consultar(strQueryIdPais);

                if (dtIDPais.Rows.Count > 0)
                {
                    IdPais = Convert.ToInt64(dtIDPais.Rows[0].ItemArray[0]);
                    Respuesta = 1;
                    WriteToFile("Validación Paí exitosa.");
                }
                else
                {
                    // Construir SQL para insertar nuevo país y generar entrada en FSLog de este cambio nuevo
                    string strInsertPais = "BEGIN INSERT INTO " + Utilidades.Configuracion.Esquema_SICOQ_WS + ".CE_PAISES(CODIGO, NOMBRE) " +
                                                        "VALUES('" + strPaisCompra + "', 'POR DEFINIR, VER SOLICITUD " + strNumTempSol + "')" +
                                                        " Returning IdPais into :Id; END; ";

                    DbParameter[] dbParametro = new DbParameter[1];
                    dbParametro[0] = objDataAccess.CrearParametro(ParameterDirection.Output, 1, DbType.Int64, ":Id");
                    WriteToFile("Listo para insertar pais nuevo.");

                    try
                    {
                        objDataAccess.EjecutaComando(dbParametro, strInsertPais);
                        IdPais = Convert.ToInt64(dbParametro[0].Value);
                        Respuesta = 1;
                        WriteToFile("País Insertado con exito.");

                        string strCodigo = "30101";
                        string strDescripcion = "Se inserto un nuevo país de compra en CE_PAISES asociadoa la Solicitud con IdPais = ''" +
                                                IdPais.ToString() + "'' y Codigo = ''" + strPaisCompra + "''";
                        string strOrigen = "Web Service para recibir las solicitudes de importación - Solicitud Temporal: " + strNumTempSol;
                        string strTipo = "Validación de la solicitud de importación en el Web Service";
                        string strTraduccion = "El código de país asociado a la Solicitud se agregó a la maestra de CE_PAISES";
                        string strTraza = "WSINicial.asmx";
                        string strMetodo = "public int GuardarSolicitudNueva";
                        string strInstruccion = strInsertPais.Replace("'", "''");

                        InsertarEntradaLogFS(strCodigo, strDescripcion, strOrigen,
                                          strTipo, strTraduccion, strTraza,
                                          strMetodo, strInstruccion);
                    }
                    catch (Exception e)
                    {
                        if (IdPais == -1) Respuesta = 0;
                        objDataAccess.DesConectar();
                        WriteToFile(e.Message);

                        // Guardar en el log de errores de Factory Suite
                        GuardarLogErroresFS(e);
                    }

                }

                objDataAccess.DesConectar();
            }
            catch (Exception ex)
            {
                Respuesta = 0;
                objDataAccess.DesConectar();
                WriteToFile(ex.Message);

                // Guardar en el log de errores de Factory Suite
                GuardarLogErroresFS(ex);
            }

            return IdPais;
        }

        private long ValidarViaTransporte(string strVia, string strNumTempSol, out int Respuesta)
        {
            string strQueryIdViaTransporte = "SELECT IDVIATRANSPORTE FROM " + Utilidades.Configuracion.Esquema_SICOQ_WS + ".CE_VIAS_TRANSPORTE WHERE CODIGO = '" + strVia + "'";
            long IdViaTransporte = -1;
            Respuesta = 0;

            try
            {
                // Recuperar el IdViaTransporte a insertar
                objDataAccess.Conectar(Utilidades.Configuracion.ConexionFactorySuite, Utilidades.Configuracion.ProveedorFactorySuite);
                DataTable dtIDVias = objDataAccess.Consultar(strQueryIdViaTransporte);

                if (dtIDVias.Rows.Count > 0)
                {
                    IdViaTransporte = Convert.ToInt64(dtIDVias.Rows[0].ItemArray[0]);
                    Respuesta = 1;
                    WriteToFile("Validación de Vía de Transporte exitosa.");
                }
                else
                {
                    try
                    {
                        WriteToFile("Validación de Vía de Transporte fallida.");

                        string strCodigo = "30005";
                        string strDescripcion = "El valor de Via de Transporte no es valido. Via de Transporte = ''" + strVia + "''";
                        string strOrigen = "Web Service para recibir las solicitudes de importación - Solicitud Temporal: " + strNumTempSol;
                        string strTipo = "Validación de la via de transporte en el Web Service";
                        string strTraduccion = "No se puede continuar sin un valor de via de transporte valido.";
                        string strTraza = "WSINicial.asmx";
                        string strMetodo = "public int GuardarSolicitudNueva";
                        string strInstruccion = strQueryIdViaTransporte.Replace("'", "''");

                        InsertarEntradaLogFS(strCodigo, strDescripcion, strOrigen,
                                          strTipo, strTraduccion, strTraza,
                                          strMetodo, strInstruccion);
                    }
                    catch (Exception e)
                    {
                        objDataAccess.DesConectar();
                        WriteToFile(e.Message);

                        // Guardar en el log de errores de Factory Suite
                        GuardarLogErroresFS(e);
                    }

                }

                objDataAccess.DesConectar();
            }
            catch (Exception ex)
            {
                Respuesta = 0;
                objDataAccess.DesConectar();
                WriteToFile(ex.Message);

                // Guardar en el log de errores de Factory Suite
                GuardarLogErroresFS(ex);
            }

            return IdViaTransporte;
        }

        private long ValidarAduana(string strAduana, string strNumTempSol, out int Respuesta)
        {
            string strQueryIdAduana = "SELECT IDADUANA FROM " + Utilidades.Configuracion.Esquema_SICOQ_WS + ".CE_ADUANAS WHERE CODIGO = '" + strAduana + "'";
            long IdAduana = -1;
            Respuesta = 0;

            try
            {
                // Recuperar el IdAduana a insertar
                objDataAccess.Conectar(Utilidades.Configuracion.ConexionFactorySuite, Utilidades.Configuracion.ProveedorFactorySuite);
                DataTable dtIDAduana = objDataAccess.Consultar(strQueryIdAduana);

                if (dtIDAduana.Rows.Count > 0)
                {
                    IdAduana = Convert.ToInt64(dtIDAduana.Rows[0].ItemArray[0]);
                    Respuesta = 1;

                    WriteToFile("Validación de Aduana exitosa.");
                }
                else
                {
                    try
                    {
                        WriteToFile("Validación de Aduana fallida.");

                        string strCodigo = "30006";
                        string strDescripcion = "El valor de la Aduana no es valido. Aduana = ''" + strAduana + "''";
                        string strOrigen = "Web Service para recibir las solicitudes de importación - Solicitud Temporal: " + strNumTempSol;
                        string strTipo = "Validación de la Aduana en el Web Service";
                        string strTraduccion = "No se puede continuar sin un valor de Aduana valido.";
                        string strTraza = "WSINicial.asmx";
                        string strMetodo = "public int GuardarSolicitudNueva";
                        string strInstruccion = strQueryIdAduana.Replace("'", "''");

                        InsertarEntradaLogFS(strCodigo, strDescripcion, strOrigen,
                                          strTipo, strTraduccion, strTraza,
                                          strMetodo, strInstruccion);
                    }
                    catch (Exception e)
                    {
                        objDataAccess.DesConectar();
                        WriteToFile(e.Message);

                        // Guardar en el log de errores de Factory Suite
                        GuardarLogErroresFS(e);
                    }

                }

                objDataAccess.DesConectar();
            }
            catch (Exception ex)
            {
                Respuesta = 0;
                objDataAccess.DesConectar();
                WriteToFile(ex.Message);

                // Guardar en el log de errores de Factory Suite
                GuardarLogErroresFS(ex);
            }

            return IdAduana;
        }

        private void ValidarInsertarPuertos(string strPuertos, string strNumTempSol, long IdSolicitud)
        {
            string strQueryIdPuerto = "SELECT IDPUERTOEMBARQUE FROM " + Utilidades.Configuracion.Esquema_SICOQ_WS + ".CE_PUERTOS_EMBARQUES WHERE CODIGO = '"; // + strPuertos + "'";
            long IdPuerto = -1;
            long IdPaisDummy = -1;
            string [] strPuertosSeparados = strPuertos.Split('|');

            // Consultar país DUMMY, CODIGO = '9999999999'
            try
            {
                objDataAccess.Conectar(Utilidades.Configuracion.ConexionFactorySuite, Utilidades.Configuracion.ProveedorFactorySuite);
                DataTable dtIDPaisDummy = objDataAccess.Consultar("SELECT IdPais FROM " + Utilidades.Configuracion.Esquema_SICOQ_WS + ".CE_PAISES WHERE CODIGO = '9999999999'");

                if (dtIDPaisDummy.Rows.Count > 0)
                {
                    IdPaisDummy = Convert.ToInt64(dtIDPaisDummy.Rows[0].ItemArray[0]);
                    WriteToFile("Obteniendo Id del país Dummy.");
                }
                else
                {
                    WriteToFile("Fallo la obtención del Id del país Dummy.");

                    string strCodigo = "30007";
                    string strDescripcion = "No se encontro el país dummy (con CODIGO = ''9999999999'') al cual se asocian los puertos nuevos en CE_PUERTOS_EMBARQUES asociadoa la Solicitud ";
                    string strOrigen = "Web Service para recibir las solicitudes de importación - Solicitud Temporal: " + strNumTempSol;
                    string strTipo = "Validación de la solicitud de importación en el Web Service";
                    string strTraduccion = "El código de país asociado a un puerto nuevo no fue posible ubicar";
                    string strTraza = "WSINicial.asmx";
                    string strMetodo = "public int ValidarInsertarPuertos";
                    string strInstruccion = "SELECT IdPais FROM " + Utilidades.Configuracion.Esquema_SICOQ_WS + ".CE_PAISES WHERE CODIGO = ''9999999999''";

                    InsertarEntradaLogFS(strCodigo, strDescripcion, strOrigen,
                                      strTipo, strTraduccion, strTraza,
                                      strMetodo, strInstruccion);

                    objDataAccess.DesConectar();
                    return; 
                }

                objDataAccess.DesConectar();
            }
            catch (Exception e)
            {
                objDataAccess.DesConectar();
                WriteToFile(e.Message);

                // Guardar en el log de errores de Factory Suite
                GuardarLogErroresFS(e);
                return ;
            }

            foreach (string strPuerto in strPuertosSeparados)
            {
                if (string.IsNullOrEmpty(strPuerto)) continue;

                try
                {
                    // Recuperar el IdPuerto a insertar
                    objDataAccess.Conectar(Utilidades.Configuracion.ConexionFactorySuite, Utilidades.Configuracion.ProveedorFactorySuite);
                    DataTable dtIDPuerto = objDataAccess.Consultar(strQueryIdPuerto + strPuerto + "'");

                    if (dtIDPuerto.Rows.Count > 0)
                    {
                        IdPuerto = Convert.ToInt64(dtIDPuerto.Rows[0].ItemArray[0]);
                        WriteToFile("Recuperando Id del Puerto exitosa.");
                    }
                    else
                    {
                        // Construir SQL para insertar nuevo puerto y generar entrada en FSLog de este cambio nuevo
                        string strInsertPuerto = "BEGIN INSERT INTO " + Utilidades.Configuracion.Esquema_SICOQ_WS + ".CE_PUERTOS_EMBARQUES(CODIGO, NOMBRE, IDPAIS) " +
                                                            "VALUES('" + strPuerto + "', 'POR DEFINIR, VER SOLICITUD " + strNumTempSol + "', "  + IdPaisDummy + " )" +
                                                            " Returning IdPuertoEmbarque into :Id; END; ";

                        DbParameter[] dbParametro = new DbParameter[1];
                        dbParametro[0] = objDataAccess.CrearParametro(ParameterDirection.Output, 1, DbType.Int64, ":Id");

                        objDataAccess.EjecutaComando(dbParametro, strInsertPuerto);
                        IdPuerto = Convert.ToInt64(dbParametro[0].Value);
                        WriteToFile("Insertado nuevo puerto en la maestra.");

                        string strCodigo = "30103";
                        string strDescripcion = "Se inserto un nuevo puerto de embarque en CE_PUERTOS_EMBARQUES asociadoa la Solicitud con IdPuerto = ''" +
                                                IdPuerto.ToString() + "'' y Codigo = ''" + strPuerto + "''";
                        string strOrigen = "Web Service para recibir las solicitudes de importación - Solicitud Temporal: " + strNumTempSol;
                        string strTipo = "Validación de la solicitud de importación en el Web Service";
                        string strTraduccion = "El código de puerto asociado a la Solicitud se agregó a la maestra de CE_PUERTOS_EMBARQUES";
                        string strTraza = "WSINicial.asmx";
                        string strMetodo = "public int ValidarInsertarPuertos";
                        string strInstruccion = strInsertPuerto.Replace("'", "''");

                        InsertarEntradaLogFS(strCodigo, strDescripcion, strOrigen,
                                            strTipo, strTraduccion, strTraza,
                                            strMetodo, strInstruccion);
                    }

                    // Insertar registro en la tabla CE_SOLICITUD_PUERTOEMBARQUES
                    objDataAccess.EjecutaComando("INSERT INTO " + Utilidades.Configuracion.Esquema_SICOQ_WS + ".CE_SOLICITUD_PUERTOEMBARQUES (IDSOLICITUD, IDPUERTOEMBARQUE) VALUES (" + IdSolicitud + ", " + IdPuerto + " ) ") ;
                    objDataAccess.DesConectar();
                    WriteToFile("Insertando Puertos de Embarques asociado a la solicitud.");
                }
                catch (Exception ex)
                {
                    objDataAccess.DesConectar();
                    WriteToFile(ex.Message);

                    // Guardar en el log de errores de Factory Suite
                    GuardarLogErroresFS(ex);
                }
            }
        }

        private long ValidarUnidades(string strUnidad, string strNumTempSol, out int Respuesta)
        {
            string strQueryIdUnidad = "SELECT IDUNIDAD FROM " + Utilidades.Configuracion.Esquema_SICOQ_WS + ".UNIDAD WHERE CODIGO = '" + strUnidad + "'";
            long IdUnidad = -1;
            Respuesta = 0;

            try
            {
                // Recuperar el IdUnidad a insertar
                objDataAccess.Conectar(Utilidades.Configuracion.ConexionFactorySuite, Utilidades.Configuracion.ProveedorFactorySuite);
                DataTable dtIDUnidad = objDataAccess.Consultar(strQueryIdUnidad);
                WriteToFile("Validación de Unidad.");

                if (dtIDUnidad.Rows.Count > 0)
                {
                    IdUnidad = Convert.ToInt64(dtIDUnidad.Rows[0].ItemArray[0]);
                    Respuesta = 1;
                }
                else
                {
                    try
                    {
                        string strCodigo = "30008";
                        string strDescripcion = "El valor de la Unidad no es valido. Unidad = ''" + strUnidad + "''";
                        string strOrigen = "Web Service para recibir las solicitudes de importación - Solicitud Temporal: " + strNumTempSol;
                        string strTipo = "Validación de la unidad en el Web Service";
                        string strTraduccion = "No se puede continuar sin un valor de la unidad valido.";
                        string strTraza = "WSINicial.asmx";
                        string strMetodo = "private void InsertarSubpartidas";
                        string strInstruccion = strQueryIdUnidad.Replace("'", "''");

                        InsertarEntradaLogFS(strCodigo, strDescripcion, strOrigen,
                                          strTipo, strTraduccion, strTraza,
                                          strMetodo, strInstruccion);
                    }
                    catch (Exception e)
                    {
                        objDataAccess.DesConectar();
                        // Guardar en el log de errores de Factory Suite
                        GuardarLogErroresFS(e);
                    }

                }

                objDataAccess.DesConectar();
            }
            catch (Exception ex)
            {
                Respuesta = 0;
                objDataAccess.DesConectar();

                // Guardar en el log de errores de Factory Suite
                GuardarLogErroresFS(ex);
            }

            return IdUnidad;
        }

        private void InsertarSolicitudesEspeciales(ClsSolicitudesEspeciales SolsEsps, string strNumTempSol, long IdSolicitud)
        {
            string strSolictudesEspeciales = "";

            foreach (ClsSolicitudEspecial objSolEsp in SolsEsps)
            {
                strSolictudesEspeciales += objSolEsp.DesSolEspecial + ".\n" ; //Environment.NewLine;
            }

            try
            {
                // Actualizar DESCRIPCIONSOLICITUDESPECIAL en CE_SOLICITUD_IMPORTACIONES
                objDataAccess.Conectar(Utilidades.Configuracion.ConexionFactorySuite, Utilidades.Configuracion.ProveedorFactorySuite);
                objDataAccess.EjecutaComando("UPDATE " + Utilidades.Configuracion.Esquema_SICOQ_WS + ".CE_SOLICITUD_IMPORTACIONES SET DESCRIPCIONSOLICITUDESPECIAL = '" + strSolictudesEspeciales + "' WHERE IDSOLICITUD = " + IdSolicitud);
                objDataAccess.DesConectar();
                WriteToFile("Actualizado Solicitudes especiales de la Solicitud.");
            }
            catch (Exception ex)
            {
                objDataAccess.DesConectar();
                WriteToFile("Actualizado Solicitudes especiales fallida.");
                WriteToFile(ex.Message);

                // Guardar en el log de errores de Factory Suite
                GuardarLogErroresFS(ex);
            }
        }

        private void InsertarSubpartidas(ClsSubPartidas SubPartidas, string strNumTempSol, long IdSolicitud)
        {
            long IdUnidad = -1;
            long IdPais = -1;
            long NumeroItem = 0;
            long NumeroItemSb = 0;
            long IdSolSubp = -1;
            long IdSolSubpIt = -1;
            int Respuesta = 0;

            string strInsertSubp = "BEGIN INSERT INTO " + Utilidades.Configuracion.Esquema_SICOQ_WS + ".CE_SOLICITUD_SUBPARTIDAS (IDSOLICITUD, SUBPARTIDA, NUMEROITEM, IDUNIDAD, DESCRIPCIONMERCANCIA, VALORITEM) VALUES (";
            string strInsertItSubp = "BEGIN INSERT INTO " + Utilidades.Configuracion.Esquema_SICOQ_WS + ".CE_SOLICITUD_SUBPARTIDAS_ITEMS (IDSUBPARTIDA, NUMEROITEM, DESCRIPCIONMERCANCIA, CANTIDAD, IDUNIDAD, PRECIOUNITARIO, VALOR) VALUES (";

            foreach (ClsSubPartida objSubp in SubPartidas)
            {
                IdSolSubp = -1;
                IdUnidad = ValidarUnidades(objSubp.UnidadMedida, strNumTempSol, out Respuesta);
                if (Respuesta == 0) continue;
                NumeroItem = NumeroItem + 1;

                // Insertar Subpartidas  
                try
                {
                    if (String.IsNullOrEmpty(objSubp.ValorSubp)) objSubp.ValorSubp = "0";
                    objDataAccess.Conectar(Utilidades.Configuracion.ConexionFactorySuite, Utilidades.Configuracion.ProveedorFactorySuite);
                    DbParameter[] dbParametro = new DbParameter[1];
                    dbParametro[0] = objDataAccess.CrearParametro(ParameterDirection.Output, 1, DbType.Int64, ":Id");
                    objDataAccess.EjecutaComando(dbParametro, strInsertSubp + IdSolicitud + ", '" + objSubp.NumSubpartida + "', " +  objSubp.ConsecutivoSubp  + ", " + IdUnidad + ",'" + objSubp.DesSubpartida.Replace("'", "''") + "', " + objSubp.ValorSubp + " )  Returning IdSolicitudSubpartida into :Id; END;  ");
                    IdSolSubp = Convert.ToInt64(dbParametro[0].Value);
                    objDataAccess.DesConectar();
                    WriteToFile("Inserción de Subpartida exitosa.");
                }
                catch (Exception ex)
                {
                    objDataAccess.DesConectar();
                    WriteToFile("Falló al insertar la Subpartida.");
                    WriteToFile(ex.Message);

                    // Guardar en el log de errores de Factory Suite
                    GuardarLogErroresFS(ex);
                }

                if (IdSolSubp != -1)
                {
                    NumeroItemSb = 0;

                    foreach (ClsProducto objProd in objSubp.Productos)
                    {
                        // Insertar Productos 
                        IdSolSubpIt = -1;
                        NumeroItemSb = NumeroItemSb + 1; 

                        try
                        {
                            objDataAccess.Conectar(Utilidades.Configuracion.ConexionFactorySuite, Utilidades.Configuracion.ProveedorFactorySuite);
                            DbParameter[] dbParametro = new DbParameter[1];
                            dbParametro[0] = objDataAccess.CrearParametro(ParameterDirection.Output, 1, DbType.Int64, ":Id");
                            objDataAccess.EjecutaComando(dbParametro, strInsertItSubp + IdSolSubp + ", " + objProd.ConsecutivoItem + ", '" + objProd.DesProducto.Replace("'", "''") + "', " + objProd.CantidadProducto + ", " + IdUnidad + ", " + objProd.PrecioUnitario + ", " + objProd.ValorTotalItem + " )  Returning IdSubpartidaItem into :Id; END;  ");
                            IdSolSubpIt = Convert.ToInt64(dbParametro[0].Value);
                            objDataAccess.DesConectar();
                            WriteToFile("Inserción de  Productos o Items de SubPartida.");

                            // Insertar Paises de Origen 
                            //
                            if (IdSolSubpIt != -1)
                            {
                                string[] strPaisesOrigenSeparados = objProd.PaisOrigen.Split('|');
                                foreach (string strPais in strPaisesOrigenSeparados)
                                {
                                    if (string.IsNullOrEmpty(strPais)) continue;

                                    IdPais = ValidarInsertarPais(strPais, strNumTempSol, out Respuesta);

                                    // Insertar Paises de Origen 
                                    try
                                    {
                                        objDataAccess.Conectar(Utilidades.Configuracion.ConexionFactorySuite, Utilidades.Configuracion.ProveedorFactorySuite);
                                        objDataAccess.EjecutaComando("INSERT INTO " + Utilidades.Configuracion.Esquema_SICOQ_WS + ".CE_SOLIC_SUBP_ITEMS_PAIS_ORIG (IDSUBPARTIDAITEM, IDPAIS) VALUES (" + IdSolSubpIt + ", " + IdPais + " ) ");
                                        objDataAccess.DesConectar();
                                        WriteToFile("Inserción de País de Origen asociado al producto (o item de subpartida).");
                                    }
                                    catch (Exception ex)
                                    {
                                        objDataAccess.DesConectar();
                                        WriteToFile("Falló al insertar en la tabla país de Origen del Producto (o item de subpartida) .");
                                        WriteToFile(ex.Message);

                                        // Guardar en el log de errores de Factory Suite
                                        GuardarLogErroresFS(ex);
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            objDataAccess.DesConectar();
                            WriteToFile("Falló al insertar Producto (item de  Subpartida).");
                            WriteToFile(ex.Message);

                            // Guardar en el log de errores de Factory Suite
                            GuardarLogErroresFS(ex);
                        }
                    }
                }
            }
        }

        private void InsertarColaWF(string strReapertura, string strNumTempSol, long IdSolicitud)
        {
            string strConsultaSolImp = "SELECT IDTIPOSOLICITUD, NUMEROFORMULARIO, NITIMPORTADOR, NOMBREIMPORTADOR, IDCLASEIMPORTADOR, IDADUANA, IDPAISCOMPRA, IDVIATRANSPORTE, DESCRIPCIONSOLICITUDESPECIAL  " + 
                " FROM " + Utilidades.Configuracion.Esquema_SICOQ_WS + ".CE_SOLICITUD_IMPORTACIONES WHERE IDSOLICITUD = " + IdSolicitud.ToString() ;
            
            string strInsertItColaWF = "INSERT INTO " + Utilidades.Configuracion.Esquema_FACTORYSUITE_WS + 
                ".FSWORKFLOWCOLAS (IdWorkflow, Instancia, BookMark, Xml, IdEstado, Accion, IndDevolver, IdPuntoDevolucion) VALUES (";

            DataTable dtSolImp, dtWorkFlow, dtWFEstados;
            string strXML;
            long IdTipoSolicitud;
            long IdWorkFlow, IdEstado;
            IdWorkFlow = -1;

            //DataTable dtUsuario s;
            //string strUserId, strIdEstado ;
            //if (strReapertura == "1")
            //{
            //    try
            //    {
            //        // Validar que la solicitud este disponible para reapertura: INDAPROBADA = 0 AND IDTIPOSOLICITUD IN(1, 2) 
            //        objDataAccess.Conectar(Utilidades.Configuracion.ConexionFactorySuite, Utilidades.Configuracion.ProveedorFactorySuite);
            //        dtSolImp = objDataAccess.Consultar("SELECT IDTIPOSOLICITUD FROM " + Utilidades.Configuracion.Esquema_SICOQ_WS +
            //                    ".CE_SOLICITUD_IMPORTACIONES WHERE IDSOLICITUD = " + IdSolicitud.ToString() + " AND INDAPROBADA = 0  AND IDTIPOSOLICITUD IN(1, 2)");
            //        WriteToFile("Recuperando Id del Tipo de Solicitud.");

            //        if (dtSolImp.Rows.Count > 0)
            //        {
            //            // Solicitud habilitada para iniciar proceso de reapertura
            //            // Obtener Id del usuario, [validar vigencia]
            //            string strConsultaUser = "SELECT CAST(UserId as varchar(36)) UserId, FECHAFINVIGENCIA FROM " + Utilidades.Configuracion.Esquema_FACTORYSUITE_WS + 
            //                ".VW_ASPNET_USERS WHERE USERNAME = '" + Utilidades.Configuracion.Usuario_SICOQ_WS + "'";
            //            dtUsuarios = objDataAccess.Consultar(strConsultaUser);

            //            if (dtUsuarios.Rows.Count > 0)
            //            {
            //                strUserId = dtUsuarios.Rows[0].ItemArray[0].ToString();
            //                WriteToFile("Recuperado Id Usuario [VW_ASPNET_USERS].");

            //                // Actualizar Solicitud para activar la reapertura - Ya realizado previamente
            //                string strUpdateSolicitud = "UPDATE CE_SOLICITUD_IMPORTACIONES SET IDTIPOSOLICITUD = 4, FECHAREAPERTURA = sysdate, IDUSUARIOREAPERTURA = '"
            //                                        + strUserId + "', " + " OBSERVACIONESREAPERTURA = ''Solicitud autorizada desde VUCE'' WHERE IDSOLICITUD = " + IdSolicitud;
            //            }
            //            else
            //            {
            //                // Usuario no encontrado o vigente 
            //                // La solicitud no cumple las condiciones para iniciar proceso de reapertura
            //                string strCodigo = "30013";
            //                string strDescripcion = "El usuario configurado para autorizar el flujo de reapertura no existe, para la solicitud con Id de solicitud es = ''" + IdSolicitud + "''";
            //                string strOrigen = "Web Service para recibir las solicitudes de importación - Solicitud Temporal: " + strNumTempSol;
            //                string strTipo = "Validación del usuario autorizado para lanzar el flujo de reapertura desde el Web Service";
            //                string strTraduccion = "No se puede continuar sin un usuario autorizado para lanzar el flujo de reapertura.";
            //                string strTraza = "WSINicial.asmx";
            //                string strMetodo = "private void InsertarColaWF";
            //                string strInstruccion = strConsultaUser.Replace("'", "''");

            //                InsertarEntradaLogFS(strCodigo, strDescripcion, strOrigen,
            //                                  strTipo, strTraduccion, strTraza,
            //                                  strMetodo, strInstruccion);
            //            }
            //        }
            //        else
            //        {
            //            // La solicitud no cumple las condiciones para iniciar proceso de reapertura
            //            string strCodigo = "30012";
            //            string strDescripcion = "La solicitud no cumple los criterios para lanzar el flujo de reapertura cuyo Id de solicitud es = ''" + IdSolicitud + "''";
            //            string strOrigen = "Web Service para recibir las solicitudes de importación - Solicitud Temporal: " + strNumTempSol;
            //            string strTipo = "Validación del estado de la solicitud y lanzar el flujo de reapertura desde el Web Service";
            //            string strTraduccion = "No se puede continuar sin la solicitud no esta habilitada para reapertura.";
            //            string strTraza = "WSINicial.asmx";
            //            string strMetodo = "private void InsertarColaWF";
            //            string strInstruccion = "SELECT IDTIPOSOLICITUD FROM " + Utilidades.Configuracion.Esquema_SICOQ_WS +
            //                    ".CE_SOLICITUD_IMPORTACIONES WHERE IDSOLICITUD = " + IdSolicitud.ToString() + " AND INDAPROBADA = 0  AND IDTIPOSOLICITUD IN(1, 2)";
            //            strInstruccion.Replace("'", "''");

            //            InsertarEntradaLogFS(strCodigo, strDescripcion, strOrigen,
            //                              strTipo, strTraduccion, strTraza,
            //                              strMetodo, strInstruccion);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        objDataAccess.DesConectar();

            //        // Guardar en el log de errores de Factory Suite
            //        GuardarLogErroresFS(ex);
            //    }

            //    objDataAccess.DesConectar();
            //    return;   // return 0 si no hay reapertura
            //}

            // Recuperar solicitud recien guardada 
            try
            {
                objDataAccess.Conectar(Utilidades.Configuracion.ConexionFactorySuite, Utilidades.Configuracion.ProveedorFactorySuite);
                dtSolImp = objDataAccess.Consultar(strConsultaSolImp);
                WriteToFile("Recuperando Info de la Solicitud recien creada.");

                if (dtSolImp.Rows.Count > 0)
                {
                    IdTipoSolicitud = Convert.ToInt64(dtSolImp.Rows[0].ItemArray[0]);

                    //Consultar el workflow de acuerdo al tipo de solicitud o traer todas y recuperar el que corresponda
                    switch (IdTipoSolicitud)
                    {
                        case 1:
                            dtWorkFlow = objDataAccess.Consultar("SELECT IDWORKFLOW FROM " + Utilidades.Configuracion.Esquema_FACTORYSUITE_WS + ".FSWORKFLOWS WHERE CODIGO = '" + Utilidades.Configuracion.CodigoSolicitudLicencia + "'");
                            if (dtWorkFlow.Rows.Count > 0) IdWorkFlow = Convert.ToInt64(dtWorkFlow.Rows[0].ItemArray[0]);
                            break;

                        case 2:
                            dtWorkFlow = objDataAccess.Consultar("SELECT IDWORKFLOW FROM " + Utilidades.Configuracion.Esquema_FACTORYSUITE_WS + ".FSWORKFLOWS WHERE CODIGO = '" + Utilidades.Configuracion.CodigoSolicitudModifLic + "'");
                            if (dtWorkFlow.Rows.Count > 0) IdWorkFlow = Convert.ToInt64(dtWorkFlow.Rows[0].ItemArray[0]);
                            break;

                        case 3:
                            dtWorkFlow = objDataAccess.Consultar("SELECT IDWORKFLOW FROM " + Utilidades.Configuracion.Esquema_FACTORYSUITE_WS + ".FSWORKFLOWS WHERE CODIGO = '" + Utilidades.Configuracion.CodigoSolicitudCanceLic + "'");
                            if (dtWorkFlow.Rows.Count > 0) IdWorkFlow = Convert.ToInt64(dtWorkFlow.Rows[0].ItemArray[0]);
                            break;

                        default:
                            IdWorkFlow = -1;
                            // Error: no es del tipo esperado
                            break;
                    }

                    WriteToFile("Recuperado Id del tipo de Workflow.");

                }
                else
                {
                    // No se encontro registro de la nueva solicitud, reportar que no se pudo lanzar el flujo 
                    WriteToFile("No se encontro el Id del tipo de Workflow.");

                    string strCodigo = "30009";
                    string strDescripcion = "No se encontro la solicitud para lanzar el flujo cuyo Id de solicitud es = ''" + IdSolicitud + "''";
                    string strOrigen = "Web Service para recibir las solicitudes de importación - Solicitud Temporal: " + strNumTempSol;
                    string strTipo = "Validación de la solicitud con la cual se va a lanzar el flujo desde el Web Service";
                    string strTraduccion = "No se puede continuar sin una solicitud válida.";
                    string strTraza = "WSINicial.asmx";
                    string strMetodo = "private void InsertarColaWF";
                    string strInstruccion = strConsultaSolImp.Replace("'", "''");

                    InsertarEntradaLogFS(strCodigo, strDescripcion, strOrigen,
                                      strTipo, strTraduccion, strTraza,
                                      strMetodo, strInstruccion);

                    objDataAccess.DesConectar();
                    return; 
                }

                if (IdWorkFlow != -1)
                {
                    dtWFEstados = objDataAccess.Consultar("SELECT IDWORKFLOWESTADO FROM " + Utilidades.Configuracion.Esquema_FACTORYSUITE_WS + ".FSWORKFLOWESTADOS WHERE CODIGO = '01'");
                    WriteToFile("Recuperando Id Estado Workflow.");

                    if (dtWFEstados.Rows.Count > 0) IdEstado = Convert.ToInt64(dtWFEstados.Rows[0].ItemArray[0]);
                    else
                    {
                        // Error del Estado = "SIN INICIAR" no existe 
                        WriteToFile("Fallo al recuperar Id Estado Workflow.");

                        string strCodigo = "30011";
                        string strDescripcion = "No se encontro el estado del flujo INICIAR cuyo código es 01 para lanzar el flujo cuyo Id de solicitud es = ''" + IdSolicitud + "''";
                        string strOrigen = "Web Service para recibir las solicitudes de importación - Solicitud Temporal: " + strNumTempSol;
                        string strTipo = "Validación del estado INICIAR del flujo con el cual se va a lanzar el flujo desde el Web Service";
                        string strTraduccion = "No se puede continuar sin que este estado del flujo este definido.";
                        string strTraza = "WSINicial.asmx";
                        string strMetodo = "private void InsertarColaWF";
                        string strInstruccion = "SELECT IDWORKFLOWESTADO FROM " + Utilidades.Configuracion.Esquema_FACTORYSUITE_WS + ".FSWORKFLOWESTADOS WHERE CODIGO = 01";
                        strInstruccion.Replace("'", "''");

                        InsertarEntradaLogFS(strCodigo, strDescripcion, strOrigen,
                                          strTipo, strTraduccion, strTraza,
                                          strMetodo, strInstruccion);

                        objDataAccess.DesConectar();
                        return; 
                    }


                    strXML = "<REGISTRO><IDSOLICITUD><![CDATA[" + IdSolicitud + "]]></IDSOLICITUD>" +
                             "<IDTIPOSOLICITUD><![CDATA[" + IdTipoSolicitud + "]]></IDTIPOSOLICITUD><NUMEROFORMULARIO><![CDATA[" + dtSolImp.Rows[0].ItemArray[1].ToString() + "]]></NUMEROFORMULARIO>" +
                             "<NITIMPORTADOR><![CDATA[" + dtSolImp.Rows[0].ItemArray[2].ToString() + "]]></NITIMPORTADOR><NOMBREIMPORTADOR><![CDATA[" + dtSolImp.Rows[0].ItemArray[3].ToString() + "]]></NOMBREIMPORTADOR>" +
                             "<REPRESENTANTELEGALIMPORTADOR><![CDATA[]]></REPRESENTANTELEGALIMPORTADOR><DIRECCIONIMPORTADOR><![CDATA[]]>" +
                             "</DIRECCIONIMPORTADOR><TELEFONOIMPORTADOR><![CDATA[]]></TELEFONOIMPORTADOR><IDCIUDADIMPORTADOR><![CDATA[]]></IDCIUDADIMPORTADOR>" +
                             "<NOMBRESIA><![CDATA[]]></NOMBRESIA><NITSIA><![CDATA[]]></NITSIA><TELEFONOSIA><![CDATA[]]></TELEFONOSIA>" +
                             "<IDCIUDADSIA><![CDATA[]]></IDCIUDADSIA><ANEXOS><![CDATA[]]></ANEXOS><IDCLASEIMPORTADOR><![CDATA[" + dtSolImp.Rows[0].ItemArray[4].ToString() + "]]></IDCLASEIMPORTADOR>" +
                             "<IDREGIMEN><![CDATA[]]></IDREGIMEN><IDESTADOMERCANCIA><![CDATA[]]></IDESTADOMERCANCIA><NOMBREEXPORTADOR><![CDATA[]]></NOMBREEXPORTADOR>" +
                             "<CIUDADEXPORTADOR><![CDATA[ce]]></CIUDADEXPORTADOR><NOMBRECONSIGNATARIO><![CDATA[co]]></NOMBRECONSIGNATARIO>" +
                             "<IDADUANA><![CDATA[" + dtSolImp.Rows[0].ItemArray[5].ToString() + "]]></IDADUANA><IDPAISORIGEN><![CDATA[]]></IDPAISORIGEN><IDPAISCOMPRA><![CDATA[" + dtSolImp.Rows[0].ItemArray[6].ToString() + "]]></IDPAISCOMPRA>" +
                             "<PUERTOEMBARQUE><![CDATA[]]></PUERTOEMBARQUE><IDVIATRANSPORTE><![CDATA[" + dtSolImp.Rows[0].ItemArray[7].ToString() + "]]></IDVIATRANSPORTE>" +
                             "<DESCRIPCIONREMBOLSO><![CDATA[]]></DESCRIPCIONREMBOLSO><TOTALUSD><![CDATA[]]></TOTALUSD>" +
                             "<DESCRIPCIONSOLICITUDESPECIAL><![CDATA[" + dtSolImp.Rows[0].ItemArray[8].ToString() + "]]></DESCRIPCIONSOLICITUDESPECIAL></REGISTRO>"; 


                    // Insertar Registro en la cola  
                    try
                    {
                        objDataAccess.Conectar(Utilidades.Configuracion.ConexionFactorySuite, Utilidades.Configuracion.ProveedorFactorySuite);
                        objDataAccess.EjecutaComando(strInsertItColaWF + IdWorkFlow + ", '', '', " + "'" + strXML + "', " + IdEstado + ", 'INICIAR', 0, null )");
                        objDataAccess.DesConectar();
                        WriteToFile("Insertado workflow en la Cola.");

                    }
                    catch (Exception ex)
                    {
                        objDataAccess.DesConectar();
                        WriteToFile("Falló al insertar el workflow en la Cola.");
                        WriteToFile(ex.Message);

                        // Guardar en el log de errores de Factory Suite
                        GuardarLogErroresFS(ex);
                    }
                }
                else
                {
                    // No se encontro la definición del flujo a lanzar 
                    WriteToFile("No se encontro la definición del workflow a lanzar.");

                    string strCodigo = "30010";
                    string strDescripcion = "No se encontro la definción del flujo a lanzar asociado a la solicitud cuyo Id es = ''" + IdSolicitud + "''";
                    string strOrigen = "Web Service para recibir las solicitudes de importación - Solicitud Temporal: " + strNumTempSol;
                    string strTipo = "Validación del flujo a lanzar asociado a la solicitud desde el Web Service";
                    string strTraduccion = "No se puede continuar sin una definición de flujo válida.";
                    string strTraza = "WSINicial.asmx";
                    string strMetodo = "private void InsertarColaWF";
                    string strInstruccion = "SELECT IDWORKFLOW FROM " + Utilidades.Configuracion.Esquema_FACTORYSUITE_WS + ".FSWORKFLOWS WHERE CODIGO = [" +
                                             Utilidades.Configuracion.CodigoSolicitudLicencia + ", " + Utilidades.Configuracion.CodigoSolicitudModifLic + ", " + Utilidades.Configuracion.CodigoSolicitudCanceLic + "]";
                    strInstruccion.Replace("'", "''");

                    InsertarEntradaLogFS(strCodigo, strDescripcion, strOrigen,
                                      strTipo, strTraduccion, strTraza,
                                      strMetodo, strInstruccion);

                }

                objDataAccess.DesConectar();
            }
            catch (Exception ex)
            {
                objDataAccess.DesConectar();
                WriteToFile("Falló al insertar el flujo en la cola.");
                WriteToFile(ex.Message);

                // Guardar en el log de errores de Factory Suite
                GuardarLogErroresFS(ex);
            }

        }

        /// <summary>
        /// Escribe en un archivo de texto
        /// </summary>
        /// <param name="Mensaje"></param>
        public void WriteToFile(string Mensaje)
        {
            string applicationDir = AppDomain.CurrentDomain.BaseDirectory;
            string path = applicationDir + LogFolder + "\\";
            string filename = string.Format("Log_{0}_{1}.{2}", DateTime.Now.ToString("yyyyMMdd"), NombreArchivo, ExtensionArchivo);

            try
            {
                //Create directory if doesn't exists
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);


                //Control exclusive access to resource I don't know if the method needs to be static, so that there's only one instance of it
                lock (lockThis)
                {
                    using (StreamWriter file = new StreamWriter(path + filename, true))
                    {
                        file.WriteLine(DateTime.Now.ToString() + " " + Mensaje);
                        file.AutoFlush = true;

                    }
                }

            }
            catch (Exception ex)
            {
                //Exception Handling
                // Guardar en el log de errores de Factory Suite
                //GuardarLogErroresFS(ex);
            }
        }

    }

    /// <summary>
    /// Método para descerializar un objeto XML
    /// </summary>
    public class Serializer
    {
        public T Deserialize<T>(string input) where T : class
        {
            System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(T));

            input = input.Replace("&", "_Y_");

            using (StringReader sr = new StringReader(input))
            {
                try
                {
                    return (T)ser.Deserialize(sr);
                }
                catch (Exception ex)
                {
//                    GuardarLogErroresFS(ex);
                    return null;
                }
            }
        }

        public string Serialize<T>(T ObjectToSerialize)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(ObjectToSerialize.GetType());

            using (StringWriter textWriter = new StringWriter())
            {
                xmlSerializer.Serialize(textWriter, ObjectToSerialize);
                return textWriter.ToString();
            }
        }
    }
}