using System.Threading.Tasks;
using System.Xml.Linq;

namespace BusinessLayer.Controller
{
    internal class OracleProductController : IRecordController
    {
        public Task<bool> Create(XElement x)
        {
            throw new System.NotImplementedException();
        }

        public void Delete()
        {
            throw new System.NotImplementedException();
        }

        public void Read()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> UpdateAsync(string id, XElement x)
        {
            throw new System.NotImplementedException();
        }
    }
}