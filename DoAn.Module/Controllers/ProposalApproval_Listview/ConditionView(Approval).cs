///Users approve form can see their's forms to approve
///ProposalApproval_ListView
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.PivotGrid.ServerMode.OperationGraph;
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
            TargetViewId = "ApplicationUser_ProposalApprovals_ListView";
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
                Execute execute = user.chucdanh;
                Execute cur = ObjectSpace.GetObject<Execute>(execute);
                Department cur_de = ObjectSpace.GetObjectByKey<Department>(user.depart.Oid);
                //View.CollectionSource.Criteria["Filter3"] = CriteriaOperator.Parse("user = ?", ObjectSpace.GetObjectByKey<ApplicationUser>(user.Oid));

                //View.CollectionSource.Criteria["Filter1"] = CriteriaOperator.Parse("proposalform.Donvi = ?", cur_de);
                //View.CollectionSource.Criteria["Filter2"] = CriteriaOperator.Parse("proposalform.Dagui = ?", true);
                //View.CollectionSource.Criteria["Filter2"] = CriteriaOperator.Parse("");
                //CriteriaOperator criteria1 = CriteriaOperator.Parse("user.chucdanh = ?", cur.Oid);
                //CriteriaOperator criteria2 = CriteriaOperator.Parse("user.chucdanh = ?", cur.Oid);

            }

        }
    }
}
