using Microsoft.Win32;

namespace Uruchie.ForumGadjet.Helpers
{
    public class SystemInfoHelper
    {
        // код взять отсюда: http://social.msdn.microsoft.com/Forums/en/csharpgeneral/thread/22930fed-4681-4079-a683-85f1ee0d6731
        public static string GetInstalledDotNetVersions()
        {
            string versions = "";
            try
            {
                const string componentsKeyName = "SOFTWARE\\Microsoft\\Active Setup\\Installed Components";
                string strName, version;
                RegistryKey componentsKey = Registry.LocalMachine.OpenSubKey(componentsKeyName);
                string[] instComps = componentsKey.GetSubKeyNames();
                foreach (string instComp in instComps)
                {
                    RegistryKey key = componentsKey.OpenSubKey(instComp);
                    strName = (string) key.GetValue(null); // Gets the (Default) value from this key
                    if (strName != null && strName.IndexOf(".NET Framework") >= 0)
                    {
                        // Let's try to get any version information that's available
                        version = (string) key.GetValue("Version");
                        versions += (version ?? "") +"; ";
                    }
                }
            }
            catch
            {
            }
            return versions;
        }

        public static string GetInstalledSilverlightVersions()
        {
            return "";
        }
    }
}