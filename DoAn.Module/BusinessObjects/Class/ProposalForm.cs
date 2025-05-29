using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Model;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DoAn.Module.BusinessObjects.Authentication;
using DoAn.Module.Controllers.Global;
using DoAn.Module.Controllers.Notification;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;

namespace DoAn.Module.BusinessObjects.Class
{
    [DefaultClassOptions]
    [NavigationItem("Category")]
    [ImageName("dexuat")]
    [DefaultProperty("Name")]
    [DefaultListViewOptions(MasterDetailMode.ListViewOnly, true, NewItemRowPosition.Top)]
    //[DeferredDeletion(false)]

    //[Persistent("DatabaseTableName")]
    // Specify more UI options using a declarative approach (https://documentation.devexpress.com/#eXpressAppFramework/CustomDocument112701).
    [Appearance("ColorDetailViewNhap1", AppearanceItemType = "LayoutItem", TargetItems = "Donvi", Criteria = "1=1", Context = "DetailView",
        FontColor = "Red", FontStyle = DevExpress.Drawing.DXFontStyle.Bold, Priority = 1)]
    [Appearance("ColorDetailViewNhap2", AppearanceItemType = "LayoutItem", TargetItems = "CS", Criteria = "1=1", Context = "DetailView",
        FontColor = "Red", FontStyle = DevExpress.Drawing.DXFontStyle.Bold, Priority = 1)]
    [Appearance("ColorDetailViewNhap3", AppearanceItemType = "LayoutItem", TargetItems = "templateform", Criteria = "1=1", Context = "DetailView",
        FontColor = "Red", FontStyle = DevExpress.Drawing.DXFontStyle.Bold, Priority = 1)]

    //[Appearance("ColorListView1", AppearanceItemType = "ViewItem", TargetItems = "Name", Criteria = "Trangthai=2", Context = "ListView",
    //    FontColor = "Blue", Priority = 1)]
    //[Appearance("ColorListView2", AppearanceItemType = "ViewItem", TargetItems = "Name", Criteria = "Trangthai=3", Context = "ListView",
    //    FontColor = "Blue", FontStyle = DevExpress.Drawing.DXFontStyle.Bold, Priority = 1)]
    //[Appearance("ColorListView3", AppearanceItemType = "ViewItem", TargetItems = "Name", Criteria = "Trangthai=1", Context = "ListView",
    //    FontColor = "Red", FontStyle = DevExpress.Drawing.DXFontStyle.Strikeout, Priority = 1)]

    [Appearance("DisableDelete", Criteria = "Dagui", AppearanceItemType = "Action", TargetItems = "Delete", Visibility = ViewItemVisibility.Hide)]
    public class ProposalForm(Session session) : BaseObject(session)
    { // Inherit from a different class to provide a custom primary key, concurrency and deletion behavior, etc. (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument113146.aspx).
        public override void AfterConstruction()
        {
            base.AfterConstruction();
            // Place your initialization code here (https://documentation.devexpress.com/eXpressAppFramework/CustomDocument112834.aspx).
            if (Session.IsNewObject(this))
            {
                Date = Define.GetServerDateTime();
                ApplicationUser ns = Define.GetCurrentNhanvien();
                if (ns != null)
                {
                    user = Session.GetObjectByKey<ApplicationUser>(ns.Oid);
                    if (user.branch != null)
                        CS = Session.GetObjectByKey<Branch>(ns.branch.Oid);
                    if (user.depart != null)
                        Donvi = Session.GetObjectByKey<Department>(ns.depart.Oid);

                }
                Dagui = false;
                Daxong = false;
            }
        }

        protected override void OnSaving()
        {
            base.OnSaving();
            if (string.IsNullOrEmpty(Name))
            {
                if (templateform != null && user !=null)
                {
                    Name = templateform.FormName;
                }
                HexCode = HashOIDtoDigits.init(Oid.ToString()).ToString();
            }
            //UpdateTrangthai()
        }

        private DateTime _Date;
        [XafDisplayName("Ngày Đề Xuất"), ModelDefault("AllowEdit", "false")]
        [ModelDefault("EditMask", "dd/MM/yyyy HH:mm")]
        [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy HH:mm}")]
        public DateTime Date
        {
            get { return _Date; }
            set { SetPropertyValue<DateTime>(nameof(Date), ref _Date, value); }
        }

        private string _Name;
        [XafDisplayName("Tên Đề Xuất"), Size(255)]

        public string Name
        {
            get { return _Name; }
            set { SetPropertyValue<string>(nameof(Name), ref _Name, value); }
        }

        private ApplicationUser _user;
        [XafDisplayName("Người Lập"),ModelDefault("AllowEdit", "false")]
        [RuleRequiredField("Yeucau Nhanvien", DefaultContexts.Save, "Phải có người lập phiếu")]
        [Association("dexuat")]
        public ApplicationUser user
        {
            get { return _user; }
            set { SetPropertyValue<ApplicationUser>(nameof(user), ref _user, value); }
        }

        private string _HexCode;
        [XafDisplayName("Code")]
        public string HexCode
        {
            get => HashOIDtoDigits.init(Oid.ToString()).ToString();
            set { SetPropertyValue<string>(nameof(HexCode), ref _HexCode, value); }
        }
        private bool _IsCoso = false;
        [ImmediatePostData, Browsable(false)]
        public bool IsCoso
        {
            get { return _IsCoso; }
            set { SetPropertyValue<bool>(nameof(IsCoso), ref _IsCoso, value); }
        }


        private Branch _CS;
        [XafDisplayName("Cơ Sở (*)")]
        //[RuleRequiredField(DefaultContexts.Save, CustomMessageTemplate = "'Cơ Sở' không được để trống!")]
        [Appearance("cs", Visibility = ViewItemVisibility.Hide, Criteria = "!IsCoso", Context = "DetailView")]
        [Association]
        public Branch CS
        {
            get { return _CS; }
            set { SetPropertyValue<Branch>(nameof(CS), ref _CS, value); }
        }

        private string _Kyduyet;
        [XafDisplayName("Ký Duyệt")]
        [Size(SizeAttribute.Unlimited)]
        [EditorAlias(EditorAliases.RichTextPropertyEditor)]
        public string Kyduyet
        {
            get { return _Kyduyet; }
            set { SetPropertyValue<string>(nameof(Kyduyet), ref _Kyduyet, value); }
        }
        private Department _Donvi;
        [XafDisplayName("Bộ Phận (*)"),ModelDefault("AllowEdit", "false")]
        //[RuleRequiredField(DefaultContexts.Save, CustomMessageTemplate = "'Bộ Phận' không được để trống!")]
        [Appearance("bp", Visibility = ViewItemVisibility.Hide, Criteria = "IsCoso", Context = "DetailView")]
        [Association]
        public Department Donvi
        {
            get { return _Donvi; }
            set { SetPropertyValue<Department>(nameof(Donvi), ref _Donvi, value); }
        }

        private bool _Dagui = false;
        [XafDisplayName("Đã gửi")]
        public bool Dagui
        {
            get { return _Dagui; }
            set { SetPropertyValue<bool>(nameof(Dagui), ref _Dagui, value); }
        }

        private DateTime _Ngayduyet;
        [XafDisplayName("Ngày duyệt"), ModelDefault("AllowEdit", "false")]
        [ModelDefault("EditMask", "dd/MM/yyyy HH:mm")]
        [ModelDefault("DisplayFormat", "{0:dd/MM/yyyy HH:mm}")]
        public DateTime Ngayduyet
        {
            get { return _Ngayduyet; }
            set { SetPropertyValue<DateTime>(nameof(Ngayduyet), ref _Ngayduyet, value); }
        }

        private ApplicationUser _NguoiXuly;
        [Association("xuly")]
        [XafDisplayName("Xử Lý Sau Duyệt"), ModelDefault("AllowEdit", "false")]
        public ApplicationUser NguoiXuly
        {
            get { return _NguoiXuly; }
            set { SetPropertyValue<ApplicationUser>(nameof(NguoiXuly), ref _NguoiXuly, value); }
        }

        private bool _Daxong;
        [XafDisplayName("Hoàn thành")]
        public bool Daxong
        {
            get { return _Daxong; }
            set { SetPropertyValue<bool>(nameof(Daxong), ref _Daxong, value); }
        }

        //private DateTime _Ngayxong;
        //[XafDisplayName("Ngày hoàn thành"), ModelDefault("AllowEdit", "false")]
        //[ModelDefault("EditMask", "dd/MM/yyyy HH:mm")]
        //[ModelDefault("DisplayFormat", "{0:dd/MM/yyyy HH:mm}")]
        //public DateTime Ngayxong
        //{
        //    get { return _Ngayxong; }
        //    set { SetPropertyValue<DateTime>(nameof(Ngayxong), ref _Ngayxong, value); }
        //}

        private bool _Thuchien = false;
        [XafDisplayName("Thực hiện"), Browsable(false)]
        public bool Thuchien
        {
            get { return _Thuchien; }
            set
            {
                bool isModified = SetPropertyValue<bool>(nameof(Thuchien), ref _Thuchien, value);
                //if (isModified && !IsLoading && !IsSaving && !IsDeleted)
                //    RefreshMau();
            }
        }


        private TemplateForm _templateform;
        [XafDisplayName("Mẫu Đơn")]
        [Association]
        [RuleRequiredField("Yeucau mauphieu", DefaultContexts.Save, "Phải có mẫu đề xuất")]
        //[DataSourceProperty(nameof(DSMau))]
        public TemplateForm templateform
        {
            get { return _templateform; }
            set
            {
                bool IsModified = SetPropertyValue<TemplateForm>(nameof(templateform), ref _templateform, value);
                if (IsModified && !IsLoading && !IsSaving && !IsDeleted && value != null)
                {
                    //IsCoso = value.ApdungCS;
                    if (string.IsNullOrEmpty(Name) && user != null && value != null)
                    {
                        Name = templateform.FormName;
                    }
                }
            }
        }

        private XPCollection<TemplateForm> ListMau;

        public XPCollection<TemplateForm> DSMau
        {
            get
            {
                if (ListMau == null)
                {
                    ListMau = new XPCollection<TemplateForm>(Session);
                    //RefreshMau();
                }
                return ListMau;
            }
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

        private Define.EStatusVB _State;
        [XafDisplayName("Trạng Thái"), ModelDefault("AllowEdit", "false")]
        public Define.EStatusVB State
        {
            get { return _State; }
            set { SetPropertyValue<Define.EStatusVB>(nameof(State), ref _State, value); }
        }

        private int _Status;
        [XafDisplayName("Tiến Trình"),ModelDefault("AllowEdit", "false")]
        [EditorAlias("ProgressProperty")]
        [ModelDefault("DisplayFormat", "")]

        public int Status
        {
            get { return _Status; }
            set { SetPropertyValue<int>(nameof(Status), ref _Status, value); }
        }


        [Association]
        public XPCollection<Sharing> Sharings
        {
            get { return GetCollection<Sharing>(nameof(Sharings)); }

        }

        [Association]
        public XPCollection<ProposalApproval> ProposalApprovals
        {
            get { return GetCollection<ProposalApproval>(nameof(ProposalApprovals)); }
        }

        [Association]
        public XPCollection<Attachment> Files
        {
            get { return GetCollection<Attachment>(nameof(Files)); }
        }


        [Association]
        public XPCollection<Comments> Comments
        {
            get { return GetCollection<Comments>(nameof(Comments)); }
        }
        [Association]
        public XPCollection<Notifications> Notifications
        {
            get { return GetCollection<Notifications>(nameof(Notifications)); }
        }
        [NonPersistent]
        [XafDisplayName("Percentage")]
        [ModelDefault("DisplayFormat", "{0:0.##}%")]
        public double Percentage
        {
            get
            {
                if (Status == 0 || Status==-1)
                {
                    return 0;
                }
                else
                {
                    
                    return (double)Status * 100 / templateform.ApprovalProcesses.Count;
                }
            }
        }

        //public List<string> approved ;
        //private Authorize _authorize;
        //[DevExpress.Xpo.Aggregated]
        //public Authorize authorize
        //{
        //    get => _authorize;
        //    set => SetPropertyValue(nameof(authorize), ref _authorize, value);
        //}
    }
}