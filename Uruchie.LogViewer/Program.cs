using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Uruchie.ForumGadjet.Helpers;
using Uruchie.ForumGadjet.Model;
using Uruchie.ForumGadjet.Service;

namespace Uruchie.LogViewer
{
    class Program
    {
        static void Main(string[] args)
        {
            UruchieForumService.LoadDataAsync<LogInfo>("http://uruchie.org/api.php", "module=forum&action=getlogmessage&app=ForumGadget", Loaded);
            Console.ReadKey();
        }

        private static void Loaded(OperationCompletedEventArgs<LogInfo> obj)
        {
            string file = "C:\\syslog.txt";
            if (File.Exists(file))
                File.Delete(file);
            File.WriteAllLines(file, obj.Result.Messages.OrderBy(i => i.IpAddress).Select(i =>
                string.Format("[{1}][{0}][{2}]", i.MessageType, i.IpAddress, i.Message)).ToArray());
            foreach (var item in obj.Result.Messages)
            {
                Console.WriteLine(item);
            }
        }
    }
}
