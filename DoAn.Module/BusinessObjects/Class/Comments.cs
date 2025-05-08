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
    [System.ComponentModel.DisplayName("Bình Luận")]
    [NavigationItem(false)]
    //[DeferredDeletion(false)]
    [ImageName("comment")]
    //[DefaultProperty("DisplayMemberNameForLookupEditorsOfThisType")]
    //[DefaultListViewOptions(MasterDetailMode.ListViewOnly, false, NewItemRowPosition.None)]
    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    public class Comments(Session session) : BaseObject(session)
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
        }

        private string _Comment;
        [XafDisplayName("Bình Luận")]

        public string Comment
        {
            get { return _Comment; }
            set { SetPropertyValue<string>(nameof(Comment), ref _Comment, value); }
        }


        private ApplicationUser _user;
        [XafDisplayName("Nhân Sự")]
        [Association]
        public ApplicationUser user
        {
            get { return _user; }
            set { SetPropertyValue<ApplicationUser>(nameof(user), ref _user, value); }
        }

        private ProposalForm _proposalform;
        [XafDisplayName("Phiếu Đề Xuất")]
        [Association]
        public ProposalForm proposalform
        {
            get { return _proposalform; }
            set { SetPropertyValue<ProposalForm>(nameof(proposalform), ref _proposalform, value); }
        }

    }
}