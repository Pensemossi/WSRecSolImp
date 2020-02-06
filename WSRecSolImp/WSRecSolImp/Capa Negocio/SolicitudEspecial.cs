using System;
using System.Collections;
using System.Collections.Generic;


namespace WSRecSolImp.Capa_Negocio
{
    public class ClsSolicitudEspecial
    {
        private string strNombreSolicitudEspecial;
        private string strDescripcionSolicitudEspecial;   

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public string NomSolEspecial { get => strNombreSolicitudEspecial; set => strNombreSolicitudEspecial = value; }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public string DesSolEspecial { get => strDescripcionSolicitudEspecial; set => strDescripcionSolicitudEspecial = value; }
    }

    public class ClsSolicitudesEspeciales 
    {
        private List<ClsSolicitudEspecial> lstSolEsp;

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0, Type = typeof(ClsSolicitudEspecial))]
        public List<ClsSolicitudEspecial> SolicitudEspecial
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

        public IEnumerator<ClsSolicitudEspecial> GetEnumerator()
        {
            foreach (var SolEsp in lstSolEsp)
                yield return SolEsp;
        }

    }
}