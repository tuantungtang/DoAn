using DevExpress.ExpressApp.DC;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn.Module.BusinessObjects.Import
{
    [DomainComponent]
    [System.ComponentModel.DisplayName("Cập Nhật Nhân Sự")]
    [ImageName("import")]
    [NavigationItem(false)]
    public class ClsEmployee
    {
        [DevExpress.ExpressApp.Data.Key]
        public Guid Oid { get; set; }

        [XafDisplayName("Employee ID")]
        public string EmployeeId { get; set; }

        [XafDisplayName("Name")]
        public string Name { get; set; }

        [XafDisplayName("Phone Number")]
        public string PhoneNumber { get; set; }
       
        [XafDisplayName("Gender")]
        public string Gender { get; set; }

        [XafDisplayName("Email")]
        public string Email { get; set; }

        [XafDisplayName("User Name")]
        public string UserName { get; set; }
    }

    [DefaultClassOptions]
    [System.ComponentModel.DisplayName("Update Data")]
    [ImageName("import")]
    [NavigationItem(false)]


    public class ClsImport(Session session) : BaseObject(session)
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        //[FileTypeFilter("Excel File",1,"*.xls","*.xlsx")]
        //[FileTypeFilter("Spreadsheets", 1, "*.xls", "*.xlsb", "*.xlsx", "*.ods", "*.csv")]

        [XafDisplayName("Select File")]
        [ImmediatePostData]


        public FileData XlsFile { get; set; }
        [XafDisplayName("Number Sheet")]

        public int Sheet { get; set; }
        [XafDisplayName("Data from ClipBoard")]
        [VisibleInListView(false)]

        public string NoiDung { get; set; }

        private BindingList<ClsEmployee> _ImportEmployees;
        [XafDisplayName("User")]

        public BindingList<ClsEmployee> ImportEmployees
        {
            get
            {
                _ImportEmployees ??= new BindingList<ClsEmployee>();
                return _ImportEmployees;
            }

        }

    }
}
