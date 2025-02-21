using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BusinessLayer.Controller
{
    public interface IRecordController 
    {
        Task<bool> Create(XElement x);
        void Read();
        Task<bool> UpdateAsync(string id, XElement x);
        void Delete();


    }
}
