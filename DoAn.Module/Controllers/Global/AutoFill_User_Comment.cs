///set user create comment be current user
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.SystemModule;
using DevExpress.XtraRichEdit.Model.History;
using DoAn.Module.BusinessObjects.Authentication;
using DoAn.Module.BusinessObjects.Class;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace DoAn.Module.Controllers.Global
{
    public class AutoFill_User_Comment : ObjectViewController<DetailView, BusinessObjects.Class.Comments>
    {
        protected override void OnActivated()
        {
            base.OnActivated();

            if (View.ViewEditMode == ViewEditMode.Edit && View.ObjectSpace.IsNewObject(View.CurrentObject))
            {
                var attachmment = View.CurrentObject as BusinessObjects.Class.Comments;

                string curuser = SecuritySystem.CurrentUserName;
                ApplicationUser user = View.ObjectSpace.FindObject<ApplicationUser>(
                                                new BinaryOperator("UserName", curuser)
                                                                                    );
                attachmment.user = user;
            }
        }
    }
}
