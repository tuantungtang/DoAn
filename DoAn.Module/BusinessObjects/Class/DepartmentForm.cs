using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace DoAn.Module.BusinessObjects.Class
{
    [DefaultClassOptions]
    [ImageName("bophan")]
    [NavigationItem("Category")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
    //[DeferredDeletion(false)]

    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class DepartmentForm(Session session) : BaseObject(session)
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        private TemplateForm _templateform;
        [XafDisplayName("Mẫu Sử Dụng")]
        [Association]
        public TemplateForm templateform
        {
            get { return _templateform; }
            set { SetPropertyValue<TemplateForm>(nameof(templateform), ref _templateform, value); }
        }


        private Department _department;
        [XafDisplayName("Bộ Phận")]
        [Association]
        public Department department
        {
            get { return _department; }
            set { SetPropertyValue<Department>(nameof(department), ref _department, value); }
        }
    }
}