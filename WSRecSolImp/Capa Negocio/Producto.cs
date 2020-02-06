using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WSRecSolImp.Capa_Negocio
{
    public class ClsProducto
    {
        private string strConsecutivoItem;
        private string strDesProducto;
        private string strCantidadProducto;
        private string strPrecioUnitario;
        private string strValorTotalItem;
        private string strPaisOrigen;
        private ClsPermisos lstPermiso;

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0)]
        public string ConsecutivoItem { get => strConsecutivoItem; set => strConsecutivoItem = value; }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 1)]
        public string DesProducto { get => strDesProducto; set => strDesProducto = value; }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 2)]
        public string CantidadProducto { get => strCantidadProducto; set => strCantidadProducto = value; }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 3)]
        public string PrecioUnitario { get => strPrecioUnitario; set => strPrecioUnitario = value; }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 4)]
        public string ValorTotalItem { get => strValorTotalItem; set => strValorTotalItem = value; }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 5)]
        public string PaisOrigen { get => strPaisOrigen; set => strPaisOrigen = value; }

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 6, Type = typeof(ClsPermisos))]
        public ClsPermisos Permisos
        {
            get
            {
                return lstPermiso;
            }
            set
            {
                lstPermiso = value;
            }
        }
    }

    public class ClsProductos
    {
        private List<ClsProducto> lstProducto;

        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, Order = 0, Type = typeof(ClsProducto))]
        public List<ClsProducto> Producto
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

        public IEnumerator<ClsProducto> GetEnumerator()
        {
            foreach (var Prod in lstProducto)
                yield return Prod;
        }
    }
}