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
    //[System.ComponentModel.DisplayName("Quy Trình Phê Duyệt")]
    [NavigationItem(false)]
    [ImageName("steps")]
    [DefaultProperty("Step")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
    //[DeferredDeletion(false)]

    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ApprovalProcess(Session session) : BaseObject(session)
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        private int _Step;
        [XafDisplayName("Bước Duyệt")]

        public int Step
        {
            get { return _Step; }
            set { SetPropertyValue<int>(nameof(Step), ref _Step, value); }
        }


        private int _DayLeft;
        [XafDisplayName("Số Ngày Duyệt")]
        public int DayLeft
        {
            get { return _DayLeft; }
            set { SetPropertyValue<int>(nameof(DayLeft), ref _DayLeft, value); }
        }



        private Execute _execute;
        [Association]
        [XafDisplayName("Người duyệt")]

        public Execute execute
        {
            get { return _execute; }
            set { SetPropertyValue<Execute>(nameof(execute), ref _execute, value); }
        }




        private TemplateForm _templateform;
        [XafDisplayName("Mẫu Đơn")]
        [Association]
        public TemplateForm templateform
        {
            get { return _templateform; }
            set { SetPropertyValue<TemplateForm>(nameof(templateform), ref _templateform, value); }
        }
    }
}