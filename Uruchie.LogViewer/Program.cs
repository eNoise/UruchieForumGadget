using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Uruchie.Core.Helpers;

namespace Uruchie.LogViewer
{
    class Program
    {
        static void Main(string[] args)
        {
            var result = HttpHelper.HttpPost("http://uruchie.org/api.php", "module=forum&action=lastmessages&limit=1");
        }
    }
}
