using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Log4NetExample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {

            LoggerHelper.Info(new { A = DateTime.Now });
            LoggerHelper.Warning(new { B = string.Format("Özel nesne Örnek") }, new Exception("Eğer hata var ise eklemek için exception parametresi"));
            LoggerHelper.Error(new { C = int.MaxValue, D = int.MinValue });
            LoggerHelper.Fatal(new { E = "istenilen her obje koyulabilir", F = new Version(1,2,3,4) });

            base.OnStartup(e);
        }
    }
}
