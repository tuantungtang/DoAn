using ClosedXML.Excel;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Actions;
using DevExpress.ExpressApp.SystemModule;
using DoAn.Module.BusinessObjects.Authentication;
using DoAn.Module.BusinessObjects.Class;
using DoAn.Module.BusinessObjects.Import;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn.Module.Controllers.ApplicationUser_ListView
{
    public class EmployeeDataController : ViewController
    {
        public EmployeeDataController()
        {
            TargetViewId = "ApplicationUser_ListView";
            SimpleAction Data = new(this, "data", "View")
            {
                Caption = "Import File",
                ImageName = "import",
                ConfirmationMessage = "Update employess data from xlxs ?",
                TargetViewId = "ApplicationUser_ListView"
            };
            Data.Execute += Data_Execute;
        }

        private void Data_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(ClsImport));
            ClsImport clsImport = objectSpace.CreateObject<ClsImport>();
            DetailView view = Application.CreateDetailView(objectSpace, "ClsImport_DetailView", true, clsImport);
            ShowViewParameters svp = new()
            {
                CreatedView = view,
                TargetWindow = TargetWindow.Default,
                Context = TemplateContext.View,
            };
            Application.ShowViewStrategy.ShowView(svp, new ShowViewSource(null, null));
        }
    }

    public class ImportAction : ViewController
    {
        private static readonly string[] sperator = ["\n"];
        private ClsImport imp;
        public ImportAction()
        {
            TargetViewId = "ClsImport_DetailView";
            SimpleAction CheckData = new(this, "CheckData", "View")
            {
                Caption = "Check",
                ImageName = "checkdb",
                ToolTip = "Verify data ?",
                TargetViewId = "ClsImport_DetailView"
            };
            CheckData.Execute += CheckData_Execute;

            SimpleAction StoreData = new(this, "StoreData", "View")
            {
                Caption = "Save Data",
                TargetViewId = "ClsImport_DetailView",
                ImageName = "database-save",
                ToolTip = "Save ?",
                ConfirmationMessage = "Save verified data ?",
            };
            StoreData.Execute += StoreData_Execute;
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            ModificationsController modificationsController = Frame.GetController<ModificationsController>();
            if (modificationsController != null)
            {
                modificationsController.SaveAction.Active.SetItemValue("an", false);
                modificationsController.SaveAndCloseAction.Active.SetItemValue("an", false);
                modificationsController.SaveAndNewAction.Active.SetItemValue("an", false);

            }
            //imp = View.CurrentObject as ClsImport;
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            ModificationsController modificationsController = Frame.GetController<ModificationsController>();
            if (modificationsController != null)
            {
                modificationsController.SaveAction.Active.RemoveItem("an");
                modificationsController.SaveAndCloseAction.Active.RemoveItem("an");
                modificationsController.SaveAndNewAction.Active.RemoveItem("an");

            }
        }
        private void StoreData_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            IObjectSpace objectSpace = Application.CreateObjectSpace(typeof(ApplicationUser));
            foreach (ClsEmployee dong in imp.ImportEmployees)
            {
                string numid = dong.EmployeeId;
                string tenns = dong.Name;
                string phone = dong.PhoneNumber;
                string sex = dong.Gender;
                string mail = dong.Email;
                string username = dong.UserName;
                ApplicationUser employee = objectSpace.FindObject<ApplicationUser>(CriteriaOperator.Parse("EmployeeId=?", numid));
                if (employee == null)
                {
                    employee = objectSpace.CreateObject<ApplicationUser>();
                    employee.EmployeeId = numid;
                    employee.Name = tenns;
                    employee.PhoneNumber = phone;
                    employee.Gender = sex;
                    employee.Email = mail;
                    employee.UserName = username;
                    employee.Save();
                }
            }
            objectSpace.CommitChanges();
        }

        private void CheckData_Execute(object sender, SimpleActionExecuteEventArgs e)
        {
            imp = View.CurrentObject as ClsImport;
            if (imp.XlsFile == null)
            {
                CheckClipBoard();
            }
            else
            {
                CheckXlsFile();
            }
        }

        private void CheckXlsFile()
        {
            imp.ImportEmployees.Clear();
            MemoryStream stream = new();
            imp.XlsFile.SaveToStream(stream);
            XLWorkbook workbook = new(stream);
            int so = imp.Sheet;
            if (so == 0) { so = 1; }
            IXLWorksheet worksheet = workbook.Worksheet(so);
            foreach (IXLRow row in worksheet.Rows())
            {
                if (row.RowNumber() > 1)
                {
                    bool isEmptyRow = row.IsEmpty();
                    if (!isEmptyRow)
                    {
                        ClsEmployee nhansu = new()
                        {
                            Oid = Guid.NewGuid(),
                            EmployeeId = row.Cell(1).Value.ToString().Trim(),
                            Name = row.Cell(2).Value.ToString().Trim(),
                            PhoneNumber = row.Cell(3).Value.ToString().Trim(),
                            Gender = row.Cell(4).Value.ToString().Trim(),
                            Email = row.Cell(5).Value.ToString().Trim(),
                            UserName = row.Cell(5).Value.ToString().Trim()
                        };
                        imp.ImportEmployees.Add(nhansu);
                    }
                }
            }
        }

        private void CheckClipBoard()
        {
            string dulieu = imp.NoiDung;
            if (string.IsNullOrEmpty(dulieu))
            {
                return;
            }

            imp.ImportEmployees.Clear();
            string[] lines = dulieu.Split(sperator, StringSplitOptions.None);
            int linecount = lines.Length;
            if (linecount > 0)
            {
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];
                    line = line.Replace("\r", "");
                    line = line.Replace(",", "");
                    line = line.Replace(".", "");
                    if (!string.IsNullOrEmpty(line))
                    {
                        string[] datas = line.Split("\t".ToCharArray());
                        ClsEmployee nhansu = new()
                        {
                            Oid = Guid.NewGuid(),
                            EmployeeId = GetStringInArray(datas, 0),
                            Name = GetStringInArray(datas, 1),
                            PhoneNumber = GetStringInArray(datas, 2),
                            Gender = GetStringInArray(datas, 3),
                            Email = GetStringInArray(datas, 4),
                            UserName = GetStringInArray(datas, 4),
                        };
                        imp.ImportEmployees.Add(nhansu);
                    }
                }
            }
            imp.NoiDung = "";
        }

        private string GetStringInArray(string[] array, int index)
        {
            if (index < array.Length)
                return array[index].Trim();
            else
                return string.Empty;
        }
    }
}
