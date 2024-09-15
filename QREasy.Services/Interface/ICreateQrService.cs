using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QREasy.Services.Interface
{
    public interface ICreateQrService
    {
       byte[] CreateQrCode(string link);
    }
}
