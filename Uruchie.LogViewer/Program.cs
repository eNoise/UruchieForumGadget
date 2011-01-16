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
            UruchieForumService.LoadDataAsync<LogInfo>("http://uruchie.org/api.php", "module=forum&action=getlogmessage&app=ForumGadget&logtype=DEBUG&filter=message", Loaded);
            Console.ReadKey();
        }

        private static void Loaded(OperationCompletedEventArgs<LogInfo> obj)
        {
            string file = "C:\\syslog.txt";
            if (File.Exists(file))
                File.Delete(file);
            File.WriteAllLines(file, obj.Result.Messages.Select(i => i.Message).ToArray());
            foreach (var item in obj.Result.Messages)
            {
                Console.WriteLine(item);
            }
        }
    }
}
