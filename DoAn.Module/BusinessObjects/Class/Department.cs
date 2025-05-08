using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DoAn.Module.BusinessObjects.Authentication;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DoAn.Module.BusinessObjects.Class
{
    [DefaultClassOptions]
    [NavigationItem("Category")]
    [ImageName("bophan")]
    [DefaultProperty("DepartmentName")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
    //[DeferredDeletion(false)]

    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Department(Session session) : BaseObject(session)
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        private string _DepartmentName;
        [XafDisplayName("Bộ Phận"), Size(50)]

        public string DepartmentName
        {
            get { return _DepartmentName; }
            set { SetPropertyValue<string>(nameof(DepartmentName), ref _DepartmentName, value); }
        }

        private Branch _branch;
        [XafDisplayName("Cơ Sở")]
        [Association]
        public Branch branch
        {
            get { return _branch; }
            set { SetPropertyValue(nameof(branch), ref _branch, value); }
        }

        [Association]
        [RuleRequiredField(DefaultContexts.Delete, InvertResult = true, CustomMessageTemplate = "'{TargetPropertyName}' có dữ liệu. Không xóa được!")]

        public XPCollection<DepartmentForm> DepartmentForms
        {
            get { return GetCollection<DepartmentForm>(nameof(DepartmentForms)); }
        }


        [Association]
        [XafDisplayName("Phiếu đề xuất"), Browsable(false)]
        [RuleRequiredField(DefaultContexts.Delete, InvertResult = true, CustomMessageTemplate = "'{TargetPropertyName}' có dữ liệu. Không xóa được!")]
        public XPCollection<ProposalForm> ProposalForms
        {
            get { return GetCollection<ProposalForm>(nameof(ProposalForms)); }
        }

        [Association]
        public XPCollection<ApplicationUser> Users
        {
            get { return GetCollection<ApplicationUser>(nameof(Users)); }
        }
    }
}