using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Utils;
using DevExpress.Xpo;
using DevExpress.XtraPrinting;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using DoAn.Module.BusinessObjects.Authentication;
using DoAn.Module.BusinessObjects.Class;
using static DoAn.Module.BusinessObjects.Class.Define;
using System;
using System.Drawing;
using System.IO;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp.Xpo;
using DocumentFormat.OpenXml.ExtendedProperties;


namespace DoAn.Module.Controllers.ProposalApproval_DetailView
{
    public static class ExportToPDF
    {
        public static void Export(ProposalForm proposalForm)
        {
            if (proposalForm.State == EStatusVB.daduyet)
            {
                XafApplication app = ApplicationHelper.Instance.Application;
                IObjectSpace objectSpace = app.CreateObjectSpace(typeof(ProposalForm));


                // Tạo tài liệu ký duyệt
                RichEditDocumentServer kyProcessor = new()
                {
                    RtfText = proposalForm.Kyduyet
                };

                // Tạo tài liệu nội dung
                using RichEditDocumentServer wordProcessor = new();
                wordProcessor.RtfText = proposalForm.Content;

                // Chèn nội dung vào đầu tài liệu ký duyệt
                DocumentPosition pos = kyProcessor.Document.CreatePosition(0);
                kyProcessor.Document.InsertRtfText(pos, wordProcessor.RtfText);

                // Thay thế {Hoten} bằng tên người dùng


                // Thêm ảnh chữ ký chính
                if (proposalForm.user.Signature != null)
                {
                    using var ms = new MemoryStream(proposalForm.user.Signature);
                    Image img = Image.FromStream(ms);
                    DocumentRange[] ranges = kyProcessor.Document.FindAll("{KyHoten}", SearchOptions.None, kyProcessor.Document.Range);
                    if (ranges.Length > 0)
                    {
                        DocumentRange range = ranges[0];
                        DocumentPosition startPosition = range.Start;
                        kyProcessor.Document.Delete(range);
                        kyProcessor.Document.Images.Insert(startPosition, DocumentImageSource.FromImage(img));
                    }
                }

                // Xử lý chữ ký theo từng chức danh
                Session session = proposalForm.Session;
                XPCollection<Execute> chucdanhs = new(session)
                {
                    Criteria = CriteriaOperator.Parse("Execute=?", false)
                };
                //XPCollection<Execute> chucdanhs = new(session);
                //foreach (ApprovalProcess approvalProcess in proposalForm.templateform.ApprovalProcesses)
                //{
                //    chucdanhs.Add(approvalProcess.execute);
                //}



                // reload the proposalForm in this object space


                //List<Execute> chucdanhs = new List<Execute>();

                //foreach (string id in proposalForm.approved)
                //{
                //    Guid ID = Guid.Parse(id);
                //    ApplicationUser userInCurrentSpace = objectSpace.GetObjectByKey<ApplicationUser>(id);
                //    if (userInCurrentSpace.chucdanh != null)
                //    {
                //        Execute executeInCurrentSpace = objectSpace.GetObjectByKey<Execute>(userInCurrentSpace.chucdanh.Oid);
                //        if (executeInCurrentSpace != null && !chucdanhs.Contains(executeInCurrentSpace))
                //        {
                //            chucdanhs.Add(executeInCurrentSpace);
                //        }
                //    }
                //}

                //foreach (Execute item in chucdanhs)
                //{
                //    string ma = item.Code;
                //    string machuky = item.CodeCK;
                //    if (!string.IsNullOrEmpty(ma))
                //    {
                //        CriteriaOperator criteria = GroupOperator.Combine(
                //            GroupOperatorType.And,
                //            new ContainsOperator("ChucvuNVs", new BinaryOperator("Chucvu", item)),
                //            CriteriaOperator.Parse("Donvi=?", proposalForm.Donvi)
                //        );

                //        ApplicationUser au = objectSpace.FindObject<ApplicationUser>(criteria) ??
                //                    objectSpace.FindObject<ApplicationUser>(new ContainsOperator("ChucvuNVs", new BinaryOperator("Chucvu", item)));

                //        if (au != null)
                //        {
                //            kyProcessor.Document.ReplaceAll(ma, au.Name, SearchOptions.None);
                //            if (au.Signature != null)
                //            {
                //                using var ms = new MemoryStream(au.Signature);
                //                Image img = Image.FromStream(ms);
                //                DocumentRange[] ranges = kyProcessor.Document.FindAll(machuky, SearchOptions.None, kyProcessor.Document.Range);
                //                if (ranges.Length > 0)
                //                {
                //                    DocumentRange range = ranges[0];
                //                    DocumentPosition startPosition = range.Start;
                //                    kyProcessor.Document.Delete(range);
                //                    kyProcessor.Document.Images.Insert(startPosition, DocumentImageSource.FromImage(img));
                //                }
                //            }
                //        }
                //    }
                //}

                // Tùy chọn xuất PDF
                PdfExportOptions options = new()
                {
                    DocumentOptions = { Author = proposalForm.user.Name },
                    ShowPrintDialogOnOpen = true
                };

                string folder_name = proposalForm.user.Oid + "_" + proposalForm.user.Name;
                string downloadPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
                string folderPath = Path.Combine(downloadPath, folder_name);

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                string pdfName = proposalForm.HexCode.ToString() + "_" + proposalForm.user.Name + ".pdf";
                string combined = Path.Combine(folderPath, pdfName);

                using (FileStream pdfFileStream = new FileStream(combined, FileMode.Create))
                {
                    kyProcessor.ExportToPdf(pdfFileStream, options);
                }

                // Lưu các file đính kèm nếu có
                foreach (var attachment in proposalForm.Files)
                {
                    if (attachment.File != null)
                    {
                        string originalName = attachment.File.FileName;
                        string attachmentPath = Path.Combine(folderPath, originalName);
                        using FileStream fs = new FileStream(attachmentPath, FileMode.Create);
                        attachment.File.SaveToStream(fs);
                    }
                }

            }
        }
    }
}
