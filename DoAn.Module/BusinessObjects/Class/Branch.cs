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
    [NavigationItem(false)]
    [ImageName("coso")]
    [DefaultProperty("BranchName")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
    //[DeferredDeletion(false)]

    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Branch(Session session) : BaseObject(session)
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        private string _BranchName;
        [XafDisplayName("Tên Cơ Sở"), Size(50)]

        public string BranchName
        {
            get { return _BranchName; }
            set { SetPropertyValue<string>(nameof(BranchName), ref _BranchName, value); }
        }


        [Association, Browsable(false)]
        [RuleRequiredField(DefaultContexts.Delete, InvertResult = true, CustomMessageTemplate = "'{TargetPropertyName}' có dữ liệu. Không xóa được!")]

        public XPCollection<ApplicationUser> Users
        {
            get { return GetCollection<ApplicationUser>(nameof(Users)); }
        }


        [Association, Browsable(false)]
        [RuleRequiredField(DefaultContexts.Delete, InvertResult = true, CustomMessageTemplate = "'{TargetPropertyName}' có dữ liệu. Không xóa được!")]

        public XPCollection<Department> Departments
        {
            get { return GetCollection<Department>(nameof(Departments)); }
        }

        [Association]
        [XafDisplayName("Phiếu đề xuất"), Browsable(false)]
        [RuleRequiredField(DefaultContexts.Delete, InvertResult = true, CustomMessageTemplate = "'{TargetPropertyName}' có dữ liệu. Không xóa được!")]
        public XPCollection<ProposalForm> ProposalForms
        {
            get { return GetCollection<ProposalForm>(nameof(ProposalForms)); }
        }
    }
}