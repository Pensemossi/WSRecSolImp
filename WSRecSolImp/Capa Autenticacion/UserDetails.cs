using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Xml;
using Utilidades;


namespace WSRecSolImp.Capa_Autenticacion
{
    public class UserDetails : System.Web.Services.Protocols.SoapHeader
    {
        public string usuario { get; set; }
        public string password { get; set; }


        public bool IsValid()
        {
            //Write the logic to Check the User Details From DataBase   
            //i can chek with some hardcode details UserName=Nitin and Password=Pandit   
            //return this.usuario == Utilidades.Configuracion.Usuario_SICOQ_WS && 
            //        this.password == Utilidades.Configuracion.Password_SICOQ_WS ;

            //string token = Authenticate(new NetworkCredential(this.usuario, this.password));

//            // SQL para consultar si el usuario es autenticado. El valor de 1 es OK. 0 no es autenticado.
//            // Mayor que uno puede indicar que esta en más de una aplicación. Está solución es temporal.
//            string sql_auth = "SELECT COUNT(*) UsuarioAutenticado FROM usr_factorysuitev1.ora_aspnet_users usuarios INNER JOIN usr_factorysuitev1.ora_aspnet_membership ms ON usuarios.userid = ms.userid " +
//                              "WHERE usuarios.username = '" + this.usuario + "' AND ms.password = '" + this.password + "'" ;
//;
//            objDataAccess.Conectar(Utilidades.Configuracion.ConexionFactorySuite, Utilidades.Configuracion.ProveedorFactorySuite);
//            DataTable dtTestBD = objDataAccess.Consultar("select sysdate from dual");

//            if (dtTestBD.Rows.Count > 0)
//                blnValidacionConexionBaseDatos = true;

//            objDataAccess.DesConectar();

            return ValidarUsuarioMembership(this.usuario, this.password);

            //Utilidades.Configuracion.Token = token;

            //var userToken = token; // new UserToken { IdToken = token };

            //return userToken;
            //return token.Length>0 ; 
            //it'll check the details and will return true or false    
        }

        public static bool ValidarUsuarioMembership(string strUserName, string strPassword)
        {
            //string strRespuesta = "";
            bool blnUsuario = Membership.ValidateUser(strUserName, strPassword);
            //if (!blnUsuario)
            //{
            //    strRespuesta = "Autenticación fallida, usuario o clave no corresponde ...";
            //}

            return blnUsuario;
        }

        //private string Authenticate(NetworkCredential credentials)
        //{
        //    string result = "";

        //    HttpContent content = new StringContent(Convert.ToBase64String(Encoding.ASCII.GetBytes($"USUARIO={credentials.UserName}&CLAVE={credentials.Password}&ACCION=login")));

        //    using (var Client = new HttpClient())
        //    {
        //        Client.DefaultRequestHeaders.Accept.Clear();
        //        Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));

        //        try
        //        {
        //            var Response = Client.PostAsync(Utilidades.Configuracion.FactorySuiteProxy, content);

        //            if (Response.Result == HttpStatusCode.OK)
        //            {
        //                var ResultWebAPI = Response.Content.ReadAsStringAsync();

        //                XmlDocument document = new XmlDocument();

        //                document.LoadXml(ResultWebAPI);

        //                result = document.SelectSingleNode("/ROOT/XML/RETORNO/TOKEN").InnerText;

        //                if (string.IsNullOrEmpty(result)) throw new Exception(document.SelectSingleNode("/ROOT/XML/RETORNO/MENSAJE").InnerText);
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //    }

        //    return result;
        //}

        /// <summary>
        /// Método que verifica si las credenciales del usuario son válidas en el sistema
        /// </summary>
        //private void AutenticarUsuario(string strIdUsuario, string strPassword)
        //{
        //    //Declaración de variables
        //    string strToken = string.Empty;

        //    //Se evalúa si el usuario o la contraseña están vacías
        //    if ((string.IsNullOrEmpty(strIdUsuario) || string.IsNullOrEmpty(strPassword)))
        //    {
        //        //Se muestra el mensaje de error 
        //        return;
        //    }
        //    else //Si no están vacías
        //    {
        //        try
        //        {
        //            //Instanciamos el servicio web
        //            wsFactorySuite.ServicioFactorySuiteClient objWSFactorySuite = new wsFactorySuite.ServicioFactorySuiteClient();
        //            //Ejecutamos el método que me trae el listado
        //            string strMensaje = objWSFactorySuite.AutenticarUsuario(txtUsuario.Text, txtContrasena.Text, ref strToken, ref strIdUsuario);
        //            //Verificamos si las credenciales proporcionadas por el usuario son válidas
        //            //string strMensaje = Seguridad.AutenticarUsuario(txtUsuario.Text, txtContrasena.Text, ref strToken, ref strIdUsuario);
        //            //Ok
        //            if (string.IsNullOrEmpty(strMensaje))
        //            {
        //                Formulario.Token = strToken;
        //                Formulario.IdUsuario = strIdUsuario;
        //                frmFactoryTools frmFactoryTool = new frmFactoryTools(this);
        //                frmFactoryTool.Show();

        //            }
        //            else //No son válidas
        //            {
        //                MessageBox.Show(string.Format("{0}. \nPor favor contacte al administrador", strMensaje), "Error de Validación", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Formulario.MostrarMensajeError(ex);
        //            txtUsuario.Focus();
        //        }
        //    }

        //}
    }
}