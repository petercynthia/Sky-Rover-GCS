using log4net;
using MissionPlanner.Utilities;
using SKYROVER.GCS.DeskTop.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKYROVER.GCS.DeskTop
{
    public class L10N
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static CultureInfo ConfigLang;

        static L10N()
        {
            ConfigLang = GetConfigLang();
            Strings.Culture = ConfigLang;
            //In .NET 4.5,System.Globalization.CultureInfo.DefaultThreadCurrentCulture & DefaultThreadCurrentUICulture is avaiable
        }

        public static CultureInfo GetConfigLang()
        {
            if (Settings.Instance["language"] == null)
                return CultureInfo.CurrentUICulture;
            else
                return CultureInfoEx.GetCultureInfo(Settings.Instance["language"]);
        }
    }
}
