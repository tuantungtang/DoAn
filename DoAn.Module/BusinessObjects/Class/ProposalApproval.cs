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
    //[System.ComponentModel.DisplayName("Đề Xuất Phê Duyệt")]
    [NavigationItem("Category")]
    [ImageName("steps")]
    [DefaultProperty("Step")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class ProposalApproval(Session session) : BaseObject(session)
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            
        }

           
        protected override void OnSaving()
        {
            base.OnSaving();
            ApprovalDate = Define.GetServerDateTime();
        }

        private int _Step;
        [XafDisplayName("Bước"), ModelDefault("AllowEdit", "false")]

        public int Step
        {
            get { return _Step; }
            set { SetPropertyValue<int>(nameof(Step), ref _Step, value); }
        }

        private DateTime _DueDate;
        [XafDisplayName("Hạn Duyệt"), ModelDefault("AllowEdit", "false")]
        [ModelDefault("EditMask", "dd/MM/yyyy")]
        [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy}")]

        public DateTime DueDate
        {
            get { return _DueDate; }
            set { SetPropertyValue<DateTime>(nameof(DueDate), ref _DueDate, value); }
        }


        private DateTime _ApprovalDate;
        [XafDisplayName("Ngày Phê Duyệt"), ModelDefault("AllowEdit", "false")]
        [ModelDefault("EditMask", "dd/MM/yyyy HH:mm")]
        [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy HH:mm}")]
        public DateTime ApprovalDate
        {
            get { return _ApprovalDate; }
            set { SetPropertyValue<DateTime>(nameof(ApprovalDate), ref _ApprovalDate, value); }
        }

        private string _Note;
        [XafDisplayName("Ý Kiến")]

        public string Note
        {
            get { return _Note; }
            set { SetPropertyValue<string>(nameof(Note), ref _Note, value); }
        }


        private string _Maduyet;
        [Browsable(false)]
        public string Maduyet
        {
            get { return _Maduyet; }
            set { SetPropertyValue<string>(nameof(Maduyet), ref _Maduyet, value); }
        }

        private string _MaChuky;
        [Browsable(false)]
        public string MaChuky
        {
            get { return _MaChuky; }
            set { SetPropertyValue<string>(nameof(MaChuky), ref _MaChuky, value); }
        }


        private ProposalForm _proposalform;
        [XafDisplayName("Phiếu Đề Xuất"), ModelDefault("AllowEdit", "false")]
        [Association]
        public ProposalForm proposalform
        {
            get { return _proposalform; }
            set { SetPropertyValue<ProposalForm>(nameof(proposalform), ref _proposalform, value); }
        }

        private ApplicationUser _user;
        [XafDisplayName("Người Duyệt"), ModelDefault("AllowEdit", "false")]
        [Association]
        public ApplicationUser user
        {
            get { return _user; }
            set { SetPropertyValue<ApplicationUser>(nameof(user), ref _user, value); }
        }

        private Define.EStatusDuyet _State;
        [XafDisplayName("Kết luận")]
        public Define.EStatusDuyet State
        {
            get { return _State; }
            set { SetPropertyValue<Define.EStatusDuyet>(nameof(State), ref _State, value); }
        }


    }
}