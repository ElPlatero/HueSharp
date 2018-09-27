using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HueSharp.Messages
{
    public interface IUploadable
    {
        string GetRequestBody();
    }
}
