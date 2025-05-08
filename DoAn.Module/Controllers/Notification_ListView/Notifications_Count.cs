using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Notifications;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.XtraSpreadsheet.Commands;
using DoAn.Module.BusinessObjects.Authentication;
using DoAn.Module.BusinessObjects.Class;
using DoAn.Module.Controllers.Notification;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn.Module.Controllers.Notification_ListView
{
    public class Notifications_Count : WindowController
    {
        public Notifications_Count()
        {
            //TargetWindowType = WindowType.;
            //https://supportcenter.devexpress.com/ticket/details/t1002750/xaf-how-to-show-the-number-of-list-view-items-in-the-navigation-control
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            var user = Define.GetCurrentNhanvien();
            Application.LoggingOn += Application_LoggingOn;
            
        }

        private void Application_LoggingOn(object sender, LogonEventArgs e)
        {
            var user = Define.GetCurrentNhanvien();
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(Notifications));
            int count = objectSpace.GetObjects<Notifications>()
                                   .Count(n => n.user.Oid == user.Oid);
        }

        private void Application_LoggedOn(object sender, LogonEventArgs e)
        {
            int i = 0;
        }
    }
}
