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
    //[System.ComponentModel.DisplayName("Vai Trò")]
    [NavigationItem("Category")]
    [ImageName("chucvu")]
    [DefaultProperty("RoleName")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Execute(Session session) : BaseObject(session)
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }
        private string _RoleName;
        [XafDisplayName("Tên Chức Danh"), Size(50)]

        public string RoleName
        {
            get { return _RoleName; }
            set { SetPropertyValue<string>(nameof(RoleName), ref _RoleName, value); }
        }


        private string _RoleNumber;
        [XafDisplayName("Mã Chức Danh")]

        public string RoleNumber
        {
            get { return _RoleNumber; }
            set { SetPropertyValue<string>(nameof(RoleNumber), ref _RoleNumber, value); }
        }

        private string _CodeCK;
        [XafDisplayName("Mã Chữ Ký")]
        public string CodeCK
        {
            get { return _CodeCK; }
            set { SetPropertyValue<string>(nameof(CodeCK), ref _CodeCK, value); }
        }


        [Association, Browsable(false)]
        [RuleRequiredField(DefaultContexts.Delete, InvertResult = true, CustomMessageTemplate = "'{TargetPropertyName}' có dữ liệu. Không xóa được!")]

        public XPCollection<ApprovalProcess> ApprovalProcesses
        {
            get { return GetCollection<ApprovalProcess>(nameof(ApprovalProcesses)); }
        }


        //[Association]
        //[RuleRequiredField(DefaultContexts.Delete, InvertResult = true, CustomMessageTemplate = "'{TargetPropertyName}' có dữ liệu. Không xóa được!")]

        //public XPCollection<TemplateForm> TemplateForms
        //{
        //    get { return GetCollection<TemplateForm>(nameof(TemplateForms)); }
        //}
        private string _Code;
        [XafDisplayName("Mã họ tên")]
        public string Code
        {
            get { return _Code; }
            set { SetPropertyValue<string>(nameof(Code), ref _Code, value); }
        }


        [Association]
        [RuleRequiredField(DefaultContexts.Delete, InvertResult = true, CustomMessageTemplate = "'{TargetPropertyName}' có dữ liệu. Không xóa được!")]

        public XPCollection<ApplicationUser> Users
        {
            get { return GetCollection<ApplicationUser>(nameof(Users)); }
        }

    }
}