using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace WSRecSolImp.Capa_Negocio
{
    public class SolicitudLicenciaImportacion
    {
        private string strReapertura;
        private string strNumTempSol;   // Número de formulario
        private string strNumLigRegFinal;
        private string strTipoSolicitud;
        private string strNitImportador;
        private string strNombreImportador;
        private string strClaseImportador;
        private string strTipoCancelacion;
        private string strMotivoCancelacion;
        private string strVia;
        private string strOtraVia;
        private string strAduana;
        private string strPuertoEmbarque;
        private string strOtroPuertoEmbarque;
        private string strPaisCompra;
        private string strValorFOB;
        private ClsSolicitudesEspeciales lstSolEsp;
        private ClsSubPartidas lstSubPart;

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public string Reapertura { get => strReapertura; set => strReapertura = value; }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public string NumTemporalSol { get => strNumTempSol; set => strNumTempSol = value; }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
        public string NumLigRegFinal { get => strNumLigRegFinal; set => strNumLigRegFinal = value; }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 3)]
        public string TipoSolicitud { get => strTipoSolicitud; set => strTipoSolicitud = value; }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 4)]
        public string NitImportador { get => strNitImportador; set => strNitImportador = value; }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 5)]
        public string NomImportador { get => strNombreImportador; set => strNombreImportador = value; }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 6)]
        public string ClaseImportador { get => strClaseImportador; set => strClaseImportador = value; }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 7)]
        public string TipoCancelacion { get => strTipoCancelacion; set => strTipoCancelacion = value; }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 8)]
        public string MotivoCancelacion { get => strMotivoCancelacion; set => strMotivoCancelacion = value; }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 9)]
        public string Via { get => strVia; set => strVia = value; }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 10)]
        public string OtraVia { get => strOtraVia; set => strOtraVia = value; }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 11)]
        public string Aduana { get => strAduana; set => strAduana = value; }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 12)]
        public string PuertoEmbarque { get => strPuertoEmbarque; set => strPuertoEmbarque = value; }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 13)]
        public string OtroPuertoEmbarque { get => strOtroPuertoEmbarque; set => strOtroPuertoEmbarque = value; }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 14)]
        public string PaisCompra { get => strPaisCompra; set => strPaisCompra = value; }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 15)]
        public string ValorFOB { get => strValorFOB; set => strValorFOB = value; }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 16)]
        public ClsSolicitudesEspeciales SolicitudesEspeciales
        {
            get
            {
                return lstSolEsp;
            }
            set
            {
                lstSolEsp = value;
            }
        }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 17)]
        public ClsSubPartidas Subpartidas
        {
            get
            {
                return lstSubPart;
            }
            set
            {
                lstSubPart = value;
            }
        }
    }
}