using System;
using System.IO;

using Windows.Management.Deployment;

namespace MinecraftMod
{
    public class Appx
    {
        private static PackageManager man = new PackageManager();

        public static void APPX_Unregister(string package)
        {
            foreach (var pkg in man.FindPackages(package))
            {
                if (pkg.InstalledLocation.Path == null)
                    continue;

                _ = man.RemovePackageAsync(package);
            }
        }

        public static void APPX_Register(string modDir)
        {
            _ = man.RegisterPackageAsync(new Uri(modDir + "\\AppxManifest.xml"), null, DeploymentOptions.DevelopmentMode);
        }

        public static void BackupMC(string package)
        {
            string backupDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\TmpBackup";
            string origDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Packages\\" + package;

            if (!Directory.Exists(origDir))
                return; // skip cuz not installed

            if (!Directory.Exists(backupDir))
                Directory.CreateDirectory(backupDir);
            else
            {
                Directory.Delete(backupDir, true);
                Directory.CreateDirectory(backupDir);
            }

            foreach (string str in Directory.GetDirectories(origDir, "*", SearchOption.AllDirectories))
                Directory.CreateDirectory(str.Replace(origDir, backupDir));

            foreach (string str in Directory.GetFiles(origDir, "*.*", SearchOption.AllDirectories))
                File.Copy(str, str.Replace(origDir, backupDir), true);
        }
        public static void RestoreMC(string package)
        {
            string backupDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\TmpBackup";
            string origDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "\\Packages\\" + package;

            if (!Directory.Exists(backupDir))
                throw new Exception("Cant find backup data " + backupDir);

            Directory.CreateDirectory(origDir);

            foreach (string str in Directory.GetDirectories(backupDir, "*", SearchOption.AllDirectories))
                Directory.CreateDirectory(str.Replace(backupDir, origDir));

            foreach (string str in Directory.GetFiles(backupDir, "*.*", SearchOption.AllDirectories))
                File.Copy(str, str.Replace(backupDir, origDir), true);
        }
    }
}
