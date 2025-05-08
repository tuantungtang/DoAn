using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn.Module.BusinessObjects.Class
{
    public class UserEnvironment
    {
        public string UserId { get; set; }
        public string Userkey { get; set; }
        public string Uservalue { get; set; }
    }

    public class QTDuyet
    {
        public string Id { get; set; }
        public int SoTT { get; set; }
        public string Nguoiduyet { get; set; }
        public string Han { get; set; }
        public string Ngay { get; set; }
        public string Ketluan { get; set; }
        public string Ghichu { get; set; }
    }

    [DomainComponent]
    public class ConfirmationWindowParameters
    {
        public ConfirmationWindowParameters() { }
        [ModelDefault("AllowEdit", "False")]
        public string ConfirmationMessage { get; set; }
    }
}
