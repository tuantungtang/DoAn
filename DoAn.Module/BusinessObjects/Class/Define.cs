using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Xpo;
using DevExpress.Xpo;
using DevExpress.XtraRichEdit;
using DoAn.Module.BusinessObjects.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DoAn.Module.BusinessObjects.Class
{
    public static class Define
    {
        public static bool bAdmin;
        public static string testMail = "";
        public static string defEmail;
        public static string defEmailName;
        public static string defPassword;

        public enum EStatusDuyet
        {
            [XafDisplayName("Pending processing")]
            choduyet = 0,
            [XafDisplayName("Disagree")]
            huy = 1,//chuyen vb sang readonly
            [XafDisplayName("Agree")]
            daduyet = 2,
        }
        public enum EStatusVB
        {
            [XafDisplayName("Pending approval")]
            choduyet = 0,
            [XafDisplayName("Cancel")]
            dahuy = 1,
            [XafDisplayName("Browsing")]
            dangduyet = 2,
            [XafDisplayName("Approved")]
            daduyet = 3,
        }
        public static void CustomInfo(string msg, bool bDone = false)
        {
            XafApplication app = ApplicationHelper.Instance.Application;
            if (bDone)
                app.ShowViewStrategy.ShowMessage("Đã hoàn thành tác vụ", InformationType.Success);
            else
                app.ShowViewStrategy.ShowMessage(msg, InformationType.Success);
        }
        public static void CustomError(string msg)
        {
            XafApplication app = ApplicationHelper.Instance.Application;
            app.ShowViewStrategy.ShowMessage(msg, InformationType.Error);
        }
        public static DateTime GetServerDateTime()
        {
            XafApplication app = ApplicationHelper.Instance.Application;
            using IObjectSpace objectSpace = app.CreateObjectSpace(typeof(ApplicationUser));
            using Session session = ((XPObjectSpace)objectSpace).Session;
            string sql = "SELECT GETDATE();";
            DateTime timeCurrent = (DateTime)session.ExecuteScalar(sql);
            return timeCurrent;
        }


        public static ApplicationUser GetCurrentNhanvien()
        {
            ApplicationUser nv = null;
            XafApplication app = ApplicationHelper.Instance.Application;
            IObjectSpace os = app.CreateObjectSpace(typeof(ApplicationUser));
            ApplicationUser CurentUser = os.GetObjectByKey<ApplicationUser>(SecuritySystem.CurrentUserId);
            if (CurentUser != null)
            {
                if (!string.IsNullOrEmpty(CurentUser.UserName))
                {
                    nv = os.FindObject<ApplicationUser>(CriteriaOperator.Parse("UserName=?", CurentUser.UserName));
                }
            }
            //os.Dispose();
            return nv;
        }


        public static List<UserEnvironment> UserEnvironments = new();

        public static int GetUserIntValue(string userKey)
        {
            int value = 0;
            foreach (UserEnvironment item in UserEnvironments)
            {
                if (item.UserId == SecuritySystem.CurrentUserId.ToString() && item.Userkey == userKey)
                {
                    value = CommonLib.CInt(item.Uservalue);
                    break;
                }
            }
            return value;
        }


        public static void AddUserValue(string userKey, string value)
        {
            bool Co = false;
            foreach (UserEnvironment item in UserEnvironments)
            {
                if (item.UserId == SecuritySystem.CurrentUserId.ToString() && item.Userkey == userKey)
                {
                    Co = true;
                    item.Uservalue = value;
                    return;
                }
            }
            if (!Co)
            {
                UserEnvironment item = new()
                {
                    UserId = SecuritySystem.CurrentUserId.ToString(),
                    Userkey = userKey,
                    Uservalue = value
                };
                UserEnvironments.Add(item);
            }
        }

        public static bool LapphieuDX(ProposalForm phieu, IObjectSpace objectSpace)
        {
            bool lapOK = true;
            try
            {
                if (phieu != null)
                {
                    if (phieu.templateform == null)
                    {
                        Define.CustomError("Cần chọn mẫu");
                        return false;
                    }
                    if (phieu.IsCoso)
                    {
                        if (phieu.CS == null)
                        {
                            Define.CustomError("Cần chọn cơ sở trực tiếp");
                            return false;
                        }
                    }
                    else
                    {
                        if (phieu.Donvi == null)
                        {
                            Define.CustomError("Cần chọn bộ phận nhận trực tiếp");
                            return false;
                        }
                    }
                    if (phieu.Content != null)
                    {
                        Define.CustomError("Đề xuất đã có nội dung, không tạo lại được");
                        return false;
                    }
                    var richServer = new RichEditDocumentServer();
                    richServer.Document.RtfText = phieu.templateform.Content;
                    //Thong tin nguoi lap
                    if (phieu.user != null)
                    {
                        string hoten = phieu.user.Name;
                        string donvi = "";
                        if (phieu.user.depart != null)
                            donvi = phieu.user.depart.DepartmentName;
                        string tencs = "";
                        if (phieu.user.branch != null)
                        {
                            tencs = phieu.user.branch.BranchName;
                        }
                        richServer.Document.ReplaceAll("{Hoten}", hoten, DevExpress.XtraRichEdit.API.Native.SearchOptions.None);
                        richServer.Document.ReplaceAll("{Donvi}", donvi, DevExpress.XtraRichEdit.API.Native.SearchOptions.None);
                        richServer.Document.ReplaceAll("{CS}", tencs, DevExpress.XtraRichEdit.API.Native.SearchOptions.None);
                    }
                    phieu.Content = richServer.Document.RtfText;
                    phieu.Save();
                    foreach (ApprovalProcess item in phieu.templateform.ApprovalProcesses)
                    {
                        if (item.execute != null)
                        {
                            ProposalApproval duyet = objectSpace.CreateObject<ProposalApproval>();
                            duyet.proposalform = phieu;
                            duyet.Step = item.Step;
                            if (phieu.Donvi != null)
                            {
                                CriteriaOperator criteria = GroupOperator.Combine(GroupOperatorType.And,
                                new BinaryOperator("chucdanh", item.execute),
                                CriteriaOperator.Parse("depart=?", phieu.Donvi));
                                ApplicationUser ns = objectSpace.FindObject<ApplicationUser>(criteria);
                                ns ??= objectSpace.FindObject<ApplicationUser>(new BinaryOperator("chucdanh", item.execute));
                                if (ns != null)
                                {
                                    duyet.user = ns;
                                    duyet.Maduyet = item.execute.RoleNumber;
                                    duyet.MaChuky = item.execute.CodeCK;
                                }
                                if (item.DayLeft > 0 && phieu.Date != DateTime.MinValue)
                                {
                                    duyet.DueDate = phieu.Date.AddDays(item.DayLeft);
                                }
                                duyet.Save();
                            }

                        }
                    }
                    objectSpace.CommitChanges();
                }
            }
            catch
            {
                lapOK = false;
            }

            return lapOK;
        }

        public static void SendEmailDexuat(ProposalForm phieu)
        {
            if (phieu != null)
            {
                string tieude = "Thông báo hệ thống trình ký";
                XafApplication app = ApplicationHelper.Instance.Application;
                IObjectSpace objectSpace = app.CreateObjectSpace(typeof(ProposalApproval));
                ProposalForm phieumoi = objectSpace.GetObject(phieu);
                int sott = 1;
                ProposalApproval duyetmoi = objectSpace.FindObject<ProposalApproval>(CriteriaOperator.Parse("proposalform=? && Step=?", phieumoi, sott));
                if (duyetmoi != null)
                {
                    //string addr3 = duyetmoi.Nguoiduyet.Email;
                    string addr3 = (string.IsNullOrEmpty(testMail) ? duyetmoi.user.Email : testMail);
                    string tenmail3 = duyetmoi.proposalform.user.Name;
                    if (phieumoi.Dagui)
                    {
                        string noidung3 = "Đề xuất số <b>" + duyetmoi.proposalform.Oid + "</b> - " + duyetmoi.proposalform.Name +
                            "<br>Người đề xuất: <b>" + duyetmoi.proposalform.user.Name + "</b>" +
                            "<br>Bộ phận công tác: <b>" + duyetmoi.proposalform.user.depart.DepartmentName + "</b><br><br>"
                            + " <b>ĐANG CHỜ XEM XÉT</b><br><br>" +
                            "Bạn vui lòng CLICK vào link sau để xem và giải quyết:<br>" +
                            "<a href='https://confirm.skylineschool.edu.vn:7979/'><b> Confirm </b></a>" +
                            "<br><br>Nếu gặp khó khăn hay vướng mắc gì, vui lòng liên hệ <b>BAN CÔNG NGHỆ</b> để được hỗ trợ." +
                            "<br>Xin cảm ơn.";
                        //SendEmail(tieude, noidung3, addr3, tenmail3);
                    }
                    else
                    {
                        string noidung3 = "Đề xuất số <b>" + duyetmoi.proposalform.Oid + "</b> - " + duyetmoi.proposalform.Name +
                            "<br>Người đề xuất: <b>" + duyetmoi.proposalform.user.Name + "</b>" +
                            "<br>Bộ phận công tác: <b>" + duyetmoi.proposalform.user.depart.DepartmentName + "</b><br><br>"
                            + " <b>ĐÃ ĐƯỢC TÁC GIẢ THU HỒI</b><br><br>";

                        //SendEmail(tieude, noidung3, addr3, tenmail3);

                    }
                }
            }
        }
    }
}
