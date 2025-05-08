//using DevExpress.Data.Filtering;
//using DevExpress.ExpressApp;
//using DevExpress.ExpressApp.Actions;
//using DevExpress.ExpressApp.Blazor;
//using DevExpress.ExpressApp.Xpo;
//using DevExpress.Xpo;
//using DoAn.Module.BusinessObjects.Authentication;
//using DoAn.Module.BusinessObjects.Class;
//using DocumentFormat.OpenXml.Wordprocessing;
//using Microsoft.JSInterop;
//using System;
//using System.Security.AccessControl;

//namespace DoAn.Blazor.Server.Controllers
//{
//    public class ViewDXController: ViewController
//    {
//        private readonly SimpleAction GuiDX;
//        private readonly SimpleAction DuyetDX;
//        //private ProposalApproval CurrentBuocDuyet = null;
//        private ProposalForm CurPhieu;
//        private ApplicationUser curNV;

//        public ViewDXController()
//        {
//            TargetViewId = "viewDX";
//            //SimpleAction Quaylai = new(this, "Quaylai1", "View")
//            //{
//            //    Caption = "",
//            //    ImageName = "back",
//            //    TargetViewId = "viewDX",
//            //    ToolTip = "Quay Lại Trang Trước"
//            //};
//            //Quaylai.Execute += Quaylai_Execute;

//            GuiDX = new(this, "GuiDX", "View")
//            {
//                Caption = "Gửi",
//                ImageName = "gui",
//                TargetViewId = "viewDX",
//                ToolTip = "Gửi hoặc hủy Đề Xuất"
//            };
//            GuiDX.Execute += GuiDX_Execute;

//            DuyetDX = new(this, "DuyetDX", "View")
//            {
//                Caption = "Duyệt",
//                ImageName = "duyet",
//                ToolTip = "Duyệt Đề Xuất",
//                TargetViewId = "viewDX",
//            };
//            DuyetDX.Execute += DuyetDX_Execute;


//        }

//        private void DuyetDX_Execute(object sender, SimpleActionExecuteEventArgs e)
//        {
//            try
//            {
//                curNV = Define.GetCurrentNhanvien();
//                if (curNV!=null)
//                {
//                    IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(ProposalApproval));
//                    objectSpace = View.ObjectSpace;
//                    DetailView dView = Application.CreateDetailView(objectSpace, "DuyetView", true, objectSpace.GetObject(curNV.Name));
//                    e.ShowViewParameters.CreatedView = dView;
//                    e.ShowViewParameters.Context = TemplateContext.View;
//                    e.ShowViewParameters.TargetWindow = TargetWindow.Default;
//                }
//            }
//            catch (Exception ex)
//            {
//                Define.CustomError(ex.Message);
//            }
//        }

//        private void GuiDX_Execute(object sender, SimpleActionExecuteEventArgs e)
//        {
//            if (CurPhieu != null)
//            {
//                ProposalForm p = ObjectSpace.GetObject(CurPhieu);
//                if (p != null)
//                {
//                    if (p.State == Define.EStatusVB.choduyet)
//                    {
//                        string msg = (p.Dagui ? "Chắc chắn thu hồi đề xuất này?" : "Chắc chắn gửi đề xuất này?");
//                        NonPersistentObjectSpace objectSpace =
//                            (NonPersistentObjectSpace)Application.CreateObjectSpace(typeof(ConfirmationWindowParameters));
//                        ConfirmationWindowParameters parameters = new()
//                        {
//                            ConfirmationMessage = msg
//                        };
//                        DetailView confirmationDetailView = Application.CreateDetailView(objectSpace, parameters);
//                        confirmationDetailView.Caption = "Xác nhận";
//                        Application.ShowViewStrategy.ShowViewInPopupWindow(confirmationDetailView, OkDelegate);
//                    }
//                    else
//                        Define.CustomError("Đề xuất đang được xử lý");
//                }
//            }
//        }

//        private void OkDelegate()
//        {
//            CurPhieu.Dagui = !CurPhieu.Dagui;
//            ObjectSpace.CommitChanges();
//            string msg = (CurPhieu.Dagui ? "Bạn đã gửi đề xuất" : "Bạn đã hủy đề xuất");
//            Application.ShowViewStrategy.ShowMessage(msg);
//            Define.SendEmailDexuat(CurPhieu);
//            if (CurPhieu.Dagui)
//                GuiDX.Caption = "Hủy";
//            else
//                GuiDX.Caption = "Gửi";
//        }
//        //private void Quaylai_Execute(object sender, SimpleActionExecuteEventArgs e)
//        //{
//        //    IJSRuntime jsRuntime = (IJSRuntime)((BlazorApplication)Application).ServiceProvider.GetService(typeof(IJSRuntime));
//        //    System.Threading.Tasks.Task.Run(async () => await jsRuntime.InvokeAsync<object>("navigateBack"));
//        //}
//    }
//}
