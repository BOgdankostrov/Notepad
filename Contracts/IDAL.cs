using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
   public interface IDAL
    {
        string GetData(string path);
        void saveDate(string data);
    }
}
