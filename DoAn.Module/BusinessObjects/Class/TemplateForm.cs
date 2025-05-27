using DevExpress.CodeParser;
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DoAn.Module.Controllers;
using DocumentFormat.OpenXml.ExtendedProperties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using DocumentFormat.OpenXml.Packaging;



namespace DoAn.Module.BusinessObjects.Class
{
    [DefaultClassOptions]
    [NavigationItem("Category")]
    [ImageName("mauvb")]
    [DefaultProperty("FormName")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
    //[DeferredDeletion(false)]

    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class TemplateForm(Session session) : BaseObject(session)
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        private string _FormName;
        [XafDisplayName("Tên Mẫu Đơn")]

        public string FormName
        {
            get { return _FormName; }
            set { SetPropertyValue<string>(nameof(FormName), ref _FormName, value); }
        }

        private string _Content;
        [XafDisplayName("Nội Dung")]
        //[RuleRequiredField(DefaultContexts.Save, CustomMessageTemplate = "'Nội dung' không được để trống!")]
        [Size(SizeAttribute.Unlimited)]
        [EditorAlias(EditorAliases.RichTextPropertyEditor)]

        public string Content
        {
            get { return _Content; }
            set { SetPropertyValue<string>(nameof(Content), ref _Content, value); }
        }

        //private string _Kyduyet;
        //[XafDisplayName("Ký Duyệt")]
        //[Size(SizeAttribute.Unlimited)]
        //[EditorAlias(EditorAliases.RichTextPropertyEditor)]
        //public string Kyduyet
        //{
        //    get { return _Kyduyet; }
        //    set { SetPropertyValue<string>(nameof(Kyduyet), ref _Kyduyet, value); }
        //}


        //private bool _Thuchien;
        //[XafDisplayName("Thực Hiện")]
        //public bool Thuchien
        //{
        //    get { return _Thuchien; }
        //    set { SetPropertyValue<bool>(nameof(Thuchien), ref _Thuchien, value); }
        //}

        //private bool _Huy;
        //[XafDisplayName("Hủy")]
        //public bool Huy
        //{
        //    get { return _Huy; }
        //    set { SetPropertyValue<bool>(nameof(Huy), ref _Huy, value); }
        //}

        private Category _category;
        [XafDisplayName("Loại Mẫu")]
        [Association]
        public Category category
        {
            get { return _category; }
            set { SetPropertyValue<Category>(nameof(category), ref _category, value); }
        }


        //private Execute _execute;
        //[XafDisplayName("Xử Lý Sau Duyệt")]
        //[Association]
        //public Execute execute
        //{
        //    get { return _execute; }
        //    set { SetPropertyValue<Execute>(nameof(execute), ref _execute, value); }
        //}



        [Association]
        public XPCollection<ProposalForm> ProposalForms
        {
            get { return GetCollection<ProposalForm>(nameof(ProposalForms)); }
        }


        [Association]
        public XPCollection<DepartmentForm> DepartmentForms
        {
            get { return GetCollection<DepartmentForm>(nameof(DepartmentForms)); }
        }
        [Association]
        public XPCollection<Department> Departments
        {
            get { return GetCollection<Department>(nameof(Departments)); }
        }

        [Association]
        public XPCollection<ApprovalProcess> ApprovalProcesses
        {
            get { return GetCollection<ApprovalProcess>(nameof(ApprovalProcesses)); }
        }

    }
}