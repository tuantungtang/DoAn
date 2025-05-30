//export proposal form to pdf with signature and content
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
using System.Security.Cryptography;
using DoAn.Module.Controllers.Notification;


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


                // create document 
                RichEditDocumentServer kyProcessor = new()
                {
                    RtfText = proposalForm.Kyduyet
                };

                // add content
                using RichEditDocumentServer wordProcessor = new();
                wordProcessor.RtfText = proposalForm.Content;


                DocumentPosition pos = kyProcessor.Document.CreatePosition(0);
                kyProcessor.Document.InsertRtfText(pos, wordProcessor.RtfText);



                // add signature
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

                // add signature by execute and department
                Session session = proposalForm.Session;
                List<Execute> chucdanhs = new List<Execute>();
                foreach (ProposalApproval proposal in proposalForm.ProposalApprovals)
                {
                    ApplicationUser ID = proposal.user;
                    ApplicationUser userInCurrentSpace = objectSpace.GetObjectByKey<ApplicationUser>(ID.Oid);

                    if (userInCurrentSpace.chucdanh != null)
                    {
                        Execute executeInCurrentSpace = objectSpace.GetObjectByKey<Execute>(userInCurrentSpace.chucdanh.Oid);

                        chucdanhs.Add(objectSpace.GetObject(executeInCurrentSpace));

                    }
                }

                foreach (Execute item in chucdanhs)
                {
                    string ma = item.Code;
                    string machuky = item.CodeCK;
                    if (machuky != null)
                    {
                        ApplicationUser au = item.Users[0];
                        Department department = proposalForm.Donvi;
                        foreach (ApplicationUser user in item.Users)
                        {
                            if (user.depart == department)
                            {
                                au = user;
                                break;
                            }
                        }

                        if (au != null)
                        {
                            if (au.Signature != null)
                            {
                                using var ms = new MemoryStream(au.Signature);
                                Image img = Image.FromStream(ms);

                                DocumentRange[] ranges = kyProcessor.Document.FindAll(machuky, SearchOptions.None, kyProcessor.Document.Range);
                                if (ranges.Length > 0)
                                {
                                    DocumentRange range = ranges[0];
                                    DocumentPosition startPosition = range.Start;
                                    kyProcessor.Document.Delete(range);
                                    kyProcessor.Document.Images.Insert(startPosition, DocumentImageSource.FromImage(img));
                                }
                                DocumentRange[] ranges2 = kyProcessor.Document.FindAll(ma, SearchOptions.None, kyProcessor.Document.Range);
                                if (ranges2.Length > 0)
                                {
                                    DocumentRange text_range = ranges2[0];

                                    kyProcessor.Document.Replace(text_range, au.Name.ToString());

                                }

                            }
                        }
                    }
                }

                // export to pdf options
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

                //delete "..." in document
                string dot = "…";
                kyProcessor.Document.ReplaceAll(dot, "", SearchOptions.None);

                using (FileStream pdfFileStream = new FileStream(combined, FileMode.Create))
                {
                    kyProcessor.ExportToPdf(pdfFileStream, options);
                }

                // dowload attachments
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



                #region log file content
                string logFileName = proposalForm.HexCode.ToString() + "_" + proposalForm.user.Name + "_log.pdf";
                string logFilePath = Path.Combine(folderPath, logFileName);
                PdfExportOptions logOptions = new()
                {
                    DocumentOptions = { Author = proposalForm.user.Name },
                    ShowPrintDialogOnOpen = true
                };
                RichEditDocumentServer log_text = new RichEditDocumentServer();
                log_text.Document.AppendText($"Proposal Form: {proposalForm.Name}\n");
                log_text.Document.AppendText($"Created by: {proposalForm.user.Name}\n");
                log_text.Document.AppendText($"Created on: {proposalForm.Date}\n");


                log_text.Document.AppendText("Approvals history:\n");


                int approvalCount = proposalForm.ProposalApprovals.Count;
                Table approvalsTable = log_text.Document.Tables.Create(log_text.Document.Range.End, approvalCount + 1, 5);


                log_text.Document.InsertText(approvalsTable[0, 0].Range.Start, "Step");
                log_text.Document.InsertText(approvalsTable[0, 1].Range.Start, "Approver");
                log_text.Document.InsertText(approvalsTable[0, 2].Range.Start, "State");
                log_text.Document.InsertText(approvalsTable[0, 3].Range.Start, "Approval Date");
                log_text.Document.InsertText(approvalsTable[0, 4].Range.Start, "Note");


                var reversedApprovals = proposalForm.ProposalApprovals.Reverse<ProposalApproval>().ToList();
                for (int i = 0; i < approvalCount; i++)
                {
                    var approval = reversedApprovals[i];
                    log_text.Document.InsertText(approvalsTable[i + 1, 0].Range.Start, approval.Step.ToString());
                    log_text.Document.InsertText(approvalsTable[i + 1, 1].Range.Start, approval.user?.Name ?? "");
                    log_text.Document.InsertText(approvalsTable[i + 1, 2].Range.Start, approval.State.ToString());
                    log_text.Document.InsertText(approvalsTable[i + 1, 3].Range.Start, approval.ApprovalDate.ToString());
                    log_text.Document.InsertText(approvalsTable[i + 1, 4].Range.Start, approval.Note ?? "");
                }
                log_text.Document.AppendText("\n\nAttachments:\n");
                var files = proposalForm.Files.ToList();
                if (files.Count != 0)
                {
                    Table attachmentsTable = log_text.Document.Tables.Create(log_text.Document.Range.End, files.Count + 1, 1);
                    log_text.Document.InsertText(attachmentsTable[0, 0].Range.Start, "File Name");

                    for (int i = 0; i < files.Count; i++)
                    {
                        log_text.Document.InsertText(attachmentsTable[i + 1, 0].Range.Start, files[i].File.FileName);
                    }
                }
                var sharing = proposalForm.Sharings.ToList();
                if (sharing.Count != 0)
                {
                    log_text.Document.AppendText("\n\nShared with:\n");
                    Table sharingTable = log_text.Document.Tables.Create(log_text.Document.Range.End, sharing.Count + 1, 1);
                    log_text.Document.InsertText(sharingTable[0, 0].Range.Start, "User");
                    for (int i = 0; i < sharing.Count; i++)
                    {
                        log_text.Document.InsertText(sharingTable[i + 1, 0].Range.Start, sharing[i].user?.Name ?? "");
                    }
                }


                log_text.Document.AppendText($"Exported on: {proposalForm.Ngayduyet}");
                #endregion

                //export log to pdf
                using (FileStream logFileStream = new FileStream(logFilePath, FileMode.Create))
                {
                    log_text.ExportToPdf(logFileStream, logOptions);
                }
                #region notify to executor
                Department form_department = proposalForm.Donvi;
                ApplicationUser executeuser = objectSpace.GetObjectByKey<ApplicationUser>(proposalForm.user.Oid);
                foreach (ApplicationUser applicationUser in form_department.Users)
                {
                    if (applicationUser.chucdanh.ToString() == "EXECUTOR")
                    {

                        Notifications notification = objectSpace.CreateObject<Notifications>();
                        notification.user = objectSpace.GetObject<ApplicationUser>(applicationUser);
                        notification.proposalForms.Add(objectSpace.GetObject<ProposalForm>(proposalForm));
                        notification.NotificationString = $"{proposalForm.Name} need to be executed.";
                        notification.notiTime = DateTime.Now;
                        
                        executeuser = applicationUser;
                        objectSpace.CommitChanges();
                    }
                   
                }

                #endregion
                string execute_folder = executeuser.Oid + "_" + executeuser.UserName.ToString();
                string folder_execute = Path.Combine(downloadPath, execute_folder);
                
                if (!Directory.Exists(folder_execute))
                {
                    Directory.CreateDirectory(folder_execute);
                }
                string executeName = proposalForm.HexCode.ToString() + "_" + executeuser.UserName.ToString() + "_execute.pdf";
                string executePath = Path.Combine(folder_execute, executeName);
                using (FileStream executeFileStream = new FileStream(executePath, FileMode.Create))
                {
                    kyProcessor.ExportToPdf(executeFileStream, logOptions);
                }
                foreach (var attachment in proposalForm.Files)
                {
                    if (attachment.File != null)
                    {
                        string originalName = attachment.File.FileName;
                        string attachmentPath = Path.Combine(folder_execute, originalName);
                        using FileStream fs = new FileStream(attachmentPath, FileMode.Create);
                        attachment.File.SaveToStream(fs);
                    }
                }
                string log_execute = Path.Combine(folder_execute, executeName + "_log.pdf");
                using (FileStream logFileStream = new FileStream(log_execute, FileMode.Create))
                {
                    log_text.ExportToPdf(logFileStream, logOptions);
                }


            }
        }
    }
}
