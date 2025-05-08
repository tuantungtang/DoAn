using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.XtraCharts.Native;
using DoAn.Module.BusinessObjects.Class;
using DoAn.Module.BusinessObjects.Import;
using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn.Module.Controllers.ProposalForm_ListView
{
    public class ProposalFormController: ObjectViewController<ListView, ProposalForm>
    {
        public ProposalFormController() 
        {
            TargetViewId = "ProposalForm_ListView";
            SimpleAction newPhieu = new(this, "newPhieu", "View")
            {
                Caption = "Create New Form",
                ImageName = "dxmoi",
                TargetViewId = "ProposalForm_ListView",
                ConfirmationMessage = "Bạn có muốn tạo phiếu đề xuất ?",
            };
            newPhieu.Execute += NewPhieu_Execute;
        }

        private void NewPhieu_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(ProposalForm));
            ProposalForm pmoi = objectSpace.CreateObject<ProposalForm>();
            DetailView dView = Application.CreateDetailView(objectSpace, "Phieumoi", true, pmoi);
            Application.ShowViewStrategy.ShowViewInPopupWindow(dView, () => Lapphieu(pmoi, objectSpace));
        }
        private void Lapphieu(ProposalForm pmoi, IObjectSpace objectSpace)
        {
            if (Define.LapphieuDX(pmoi, objectSpace))
            {
                IObjectSpace objectSpace1 = Application.CreateObjectSpace(typeof(ProposalForm));
                ProposalForm p = objectSpace1.GetObject(pmoi);
                DetailView view = Application.CreateDetailView(objectSpace1, "ProposalForm_DetailView", true, p);
                ShowViewParameters svp = new()
                {
                    CreatedView = view,
                    TargetWindow = TargetWindow.Default,
                    Context = TemplateContext.View
                };
                Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
            }
        }
    }
}
