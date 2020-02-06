using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSRecSolImp.Capa_Negocio
{
    public class ClsSubPartida
    {
        private string strConsecutivoSubp;
        private string strNumeroSubpartida;
        private string strDescripcionSubpartida;
        private string strUnidadMedida;
        private string strValorSubp;
        private ClsProductos lstProducto;

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public string ConsecutivoSubp { get => strConsecutivoSubp; set => strConsecutivoSubp = value; }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public string NumSubpartida { get => strNumeroSubpartida; set => strNumeroSubpartida = value; }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
        public string DesSubpartida { get => strDescripcionSubpartida; set => strDescripcionSubpartida = value; }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 3)]
        public string UnidadMedida { get => strUnidadMedida; set => strUnidadMedida = value; }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 4)]
        public string ValorSubp { get => strValorSubp; set => strValorSubp = value; }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 5, Type = typeof(ClsProductos))]
        public ClsProductos Productos
        {
            get
            {
                return lstProducto;
            }
            set
            {
                lstProducto = value;
            }
        }
    }

    public class ClsSubPartidas
    {
        private List<ClsSubPartida> lstSubPart;

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0, Type = typeof(ClsSubPartida))]
        public List<ClsSubPartida> Subpartida
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
        public IEnumerator<ClsSubPartida> GetEnumerator()
        {
            foreach (var SubPart in lstSubPart)
                yield return SubPart;
        }
    }
}