using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DevExpress.XtraGauges.Core.Base;
using DevExpress.XtraRichEdit.Commands.Internal;
using DoAn.Module.BusinessObjects.Authentication;
using DoAn.Module.BusinessObjects.Class;
namespace DoAn.Module.Controllers.Notification
{
    [DefaultClassOptions]
    [NavigationItem("Category")]
    [DefaultProperty("Notifications")]
    
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
    public class Notifications(Session session) : DevExpress.Persistent.BaseImpl.BaseObject(session)
    {
        //Notify to 1 users
        
        private string _NotificationString;
        [XafDisplayName("Notification"), Size(255)]

        public string NotificationString
        {
            get { return _NotificationString; }
            set { SetPropertyValue<string>(nameof(NotificationString), ref _NotificationString, value); }
        }
        [Association]
        public XPCollection<ProposalForm> proposalForms
        {
            get { return GetCollection<ProposalForm>(nameof(proposalForms));}
        }
        private ApplicationUser _user;
        [Association]
        public ApplicationUser user
        {
            get { return _user; }
            set { SetPropertyValue<ApplicationUser>(nameof(user), ref _user, value); }
        }
        private DateTime _notiTime;
        [XafDisplayName("Time")]
        [ModelDefault("EditMask", "dd/MM/yyyy HH:mm")]
        [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy HH:mm}")]
        public DateTime notiTime
        {
            get { return _notiTime; }
            set { SetPropertyValue<DateTime>(nameof(notiTime), ref _notiTime, value); }
        }
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            notiTime = System.DateTime.Now;
        }
    } 
    
}
