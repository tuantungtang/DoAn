///Users approve form can see their's forms to approve
///ProposalApproval_ListView
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DoAn.Module.BusinessObjects.Authentication;
using DoAn.Module.BusinessObjects.Class;
using DocumentFormat.OpenXml.ExtendedProperties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace DoAn.Module.Controllers.ProposalApproval_Listview
{
    public class ConditionView_Approval_:ViewController<ListView>
    {
        public ConditionView_Approval_()
        {
            TargetViewId = "ProposalApproval_ListView";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            SetFilter();
        }

        private void SetFilter()
        {
            if (View is ListView listView)
            {
                ApplicationUser user = Define.GetCurrentNhanvien();
                /*
                 temporary fixed
                 */
                object obj = View.CurrentObject;
                if (user.chucdanh is not null) { }
                Execute execute = user.chucdanh;
                Execute cur = ObjectSpace.GetObject<Execute>(execute); 
                //View.CollectionSource.Criteria["Filter1"] = CriteriaOperator.Parse("user.Oid = ?", ObjectSpace.GetKeyValue(user));
                
                View.CollectionSource.Criteria["Filter1"] = CriteriaOperator.Parse("user.Oid = ?", user.Oid);

            }

        }
    }
}
