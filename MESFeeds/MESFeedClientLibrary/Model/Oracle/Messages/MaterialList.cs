using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace MESFeedClientLibrary.Model.Oracle.Messages
{
    [Serializable, XmlType("MaterialList")]
    public class MaterialList
    {
        [XmlElement("MaterialItem")]
        public List<MaterialItem> MaterialListS
        {
            get;
            private set;
        }

        public MaterialList() 
        {
            this.MaterialListS = new List<MaterialItem>();
        }

        public MaterialList(List<MaterialItem> materialList)
        {
            this.MaterialListS = materialList;
        }

        public void Add(MaterialItem materialItem)
        {
            this.MaterialListS.Add(materialItem);
        }

    }
}
