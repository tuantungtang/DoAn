using System.ComponentModel;
using System.Text;
using DevExpress.ExpressApp;
using DevExpress.ExpressApp.ConditionalAppearance;
using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Editors;
using DevExpress.ExpressApp.Security;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.BaseImpl;
using DevExpress.Persistent.BaseImpl.PermissionPolicy;
using DevExpress.Persistent.Validation;
using DevExpress.Xpo;
using DoAn.Module.BusinessObjects.Class;
using DoAn.Module.Controllers.Notification;

namespace DoAn.Module.BusinessObjects.Authentication;

[DefaultClassOptions]
[MapInheritance(MapInheritanceType.ParentTable)]
[DefaultProperty(nameof(Name))]
[CurrentUserDisplayImage(nameof(Photo))]
//[DeferredDeletion(false)]

public class ApplicationUser : PermissionPolicyUser, ISecurityUserWithLoginInfo, ISecurityUserLockout
{
    private int accessFailedCount;
    private DateTime lockoutEnd;

    public ApplicationUser(Session session) : base(session) { }

    [Browsable(false)]
    public int AccessFailedCount
    {
        get { return accessFailedCount; }
        set { SetPropertyValue(nameof(AccessFailedCount), ref accessFailedCount, value); }
    }

    [Browsable(false)]
    public DateTime LockoutEnd
    {
        get { return lockoutEnd; }
        set { SetPropertyValue(nameof(LockoutEnd), ref lockoutEnd, value); }
    }

    [Browsable(false)]
    [DevExpress.Xpo.Aggregated, Association("User-LoginInfo")]
    public XPCollection<ApplicationUserLoginInfo> LoginInfo
    {
        get { return GetCollection<ApplicationUserLoginInfo>(nameof(LoginInfo)); }
    }

    IEnumerable<ISecurityUserLoginInfo> IOAuthSecurityUser.UserLogins => LoginInfo.OfType<ISecurityUserLoginInfo>();

    ISecurityUserLoginInfo ISecurityUserWithLoginInfo.CreateUserLoginInfo(string loginProviderName, string providerUserKey)
    {
        ApplicationUserLoginInfo result = new ApplicationUserLoginInfo(Session);
        result.LoginProviderName = loginProviderName;
        result.ProviderUserKey = providerUserKey;
        result.User = this;
        return result;
    }

    //Customize

    private string _EmployeeId;
    [XafDisplayName("Số Thẻ"), Size(50)]
    [RuleRequiredField(DefaultContexts.Save, CustomMessageTemplate = "'Số Thẻ' không được để trống!")]

    public string EmployeeId
    {
        get { return _EmployeeId; }
        set { SetPropertyValue<string>(nameof(EmployeeId), ref _EmployeeId, value); }
    }

    private string _Name;
    [XafDisplayName("Họ Và Tên"), Size(50)]
    [RuleRequiredField(DefaultContexts.Save, CustomMessageTemplate = "'Họ Và Tên' không được để trống!")]
    public string Name
    {
        get { return _Name; }
        set { SetPropertyValue<string>(nameof(Name), ref _Name, value); }
    }

    private string _PhoneNumber;
    [XafDisplayName("Số Điện Thoại"), Size(50)]
    [RuleRequiredField(DefaultContexts.Save, CustomMessageTemplate = "'Số Điện Thoại' không được để trống!")]

    public string PhoneNumber
    {
        get { return _PhoneNumber; }
        set { SetPropertyValue<string>(nameof(PhoneNumber), ref _PhoneNumber, value); }
    }


    private string _Gender;
    [XafDisplayName("Giới Tính")]
    [RuleRequiredField(DefaultContexts.Save, CustomMessageTemplate = "'Giới Tính' không được để trống!")]

    public string Gender
    {
        get { return _Gender; }
        set { SetPropertyValue<string>(nameof(Gender), ref _Gender, value); }
    }

    private string _Email;
    [XafDisplayName("Email")]
    [RuleRequiredField(DefaultContexts.Save, CustomMessageTemplate = "'Email' không được để trống!")]

    public string Email
    {
        get { return _Email; }
        set { SetPropertyValue<string>(nameof(Email), ref _Email, value); }
    }


    private MediaDataObject photo;
    [ImageEditor(ListViewImageEditorMode = ImageEditorMode.PictureEdit,
             DetailViewImageEditorMode = ImageEditorMode.PictureEdit)]
    [XafDisplayName("Avatar")]
    public MediaDataObject Photo
    {
        get { return photo; }
        set { SetPropertyValue(nameof(Photo), ref photo, value); }
    }

    [Delayed(true), VisibleInListViewAttribute(true)]
    [ImageEditor(ListViewImageEditorMode = ImageEditorMode.PictureEdit,
            DetailViewImageEditorMode = ImageEditorMode.PictureEdit)]
    [XafDisplayName("Ảnh chữ ký")]
    public byte[] Signature
    {
        get { return GetDelayedPropertyValue<byte[]>(nameof(Signature)); }
        set { SetDelayedPropertyValue<byte[]>(nameof(Signature), value); }
    }


    private Branch _branch;
    [XafDisplayName("Cơ Sở")]
    [Association]
    public Branch branch
    {
        get { return _branch; }
        set { SetPropertyValue<Branch>(nameof(branch), ref _branch, value); }
    }

    private Department _depart;
    [XafDisplayName("Bộ Phận")]
    [Association]
    public Department depart
    {
        get { return _depart; }
        set { SetPropertyValue<Department>(nameof(depart), ref _depart, value); }
    }

    private Execute _chucdanh;
    [XafDisplayName("Chức Danh")]
    [Association]
    public Execute chucdanh
    {
        get { return _chucdanh; }
        set { SetPropertyValue<Execute>(nameof(chucdanh), ref _chucdanh, value); }
    }


    [Association]
    public XPCollection<Sharing> Sharings
    {
        get { return GetCollection<Sharing>(nameof(Sharings)); }
    }

    [Association]
    [RuleRequiredField(DefaultContexts.Delete, InvertResult = true, CustomMessageTemplate = "'{TargetPropertyName}' có dữ liệu. Không xóa được!")]

    public XPCollection<Attachment> Files
    {
        get { return GetCollection<Attachment>(nameof(Files)); }
    }


    [Association]
    [RuleRequiredField(DefaultContexts.Delete, InvertResult = true, CustomMessageTemplate = "'{TargetPropertyName}' có dữ liệu. Không xóa được!")]

    public XPCollection<ProposalApproval> ProposalApprovals
    {
        get { return GetCollection<ProposalApproval>(nameof(ProposalApprovals)); }
    }


    [Association("dexuat")]
    [XafDisplayName("Đề xuất")]

    [RuleRequiredField(DefaultContexts.Delete, InvertResult = true, CustomMessageTemplate = "'{TargetPropertyName}' có dữ liệu. Không xóa được!")]

    public XPCollection<ProposalForm> ProposalForms
    {
        get { return GetCollection<ProposalForm>(nameof(ProposalForms)); }
    }

    [Association("xuly")]
    [XafDisplayName("Xử lý")]

    [RuleRequiredField(DefaultContexts.Delete, InvertResult = true, CustomMessageTemplate = "'{TargetPropertyName}' có dữ liệu. Không xóa được!")]

    public XPCollection<ProposalForm> Proposals
    {
        get { return GetCollection<ProposalForm>(nameof(Proposals)); }
    }

    [Association]
    public XPCollection<Comments> Comments
    {
        get { return GetCollection<Comments>(nameof(Comments)); }
    }
    [Association]
    public XPCollection<Notifications> Notifies
    {
        get { return GetCollection<Notifications>(nameof(Notifies)); }
    }
    
}
