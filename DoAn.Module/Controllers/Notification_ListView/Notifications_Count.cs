using DevExpress.CodeParser;
using DevExpress.Data.Filtering;
using DevExpress.Entity.Model.Metadata;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Model;
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
using System.Runtime.ConstrainedExecution;
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
        private ShowNavigationItemController navigationItemController;
        protected override void OnFrameAssigned()
        {
            UnsubscribeFromEvents();
            base.OnFrameAssigned();
            navigationItemController = Frame.GetController<ShowNavigationItemController>();
            if (navigationItemController is not null)
            {
                navigationItemController.NavigationItemCreated += NavigationItemController_NavigationItemCreated;
            }
        }

        private void NavigationItemController_NavigationItemCreated(object sender, NavigationItemCreatedEventArgs e)
        {
            var lvid = Application.GetListViewId(typeof(Notifications));
            if (e.ModelNavigationItem.Id == lvid)
            {
                
                using (IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(Notifications))){
                    IModelListView modelListView = (IModelListView)e.ModelNavigationItem.View;
                    var user = Define.GetCurrentNhanvien();
                    int objectCount = objectSpace.GetObjectsCount(typeof(Notifications), CriteriaOperator.Parse(modelListView.Criteria) & CriteriaOperator.Parse("user.Oid = ?", user.Oid));

                    e.NavigationItem.Caption = "Notifications " + (objectCount > 0 ? $"({objectCount})" : string.Empty);
                }
            }
        }

        private void UnsubscribeFromEvents()
        {
            if (navigationItemController != null)
            {
                navigationItemController.NavigationItemCreated -= NavigationItemController_NavigationItemCreated;
                navigationItemController = null;
            }
        }
        protected override void Dispose(bool disposing)
        {
            UnsubscribeFromEvents();
            base.Dispose(disposing);
        }
    }
}
