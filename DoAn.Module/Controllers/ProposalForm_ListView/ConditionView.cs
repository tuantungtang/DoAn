///Users create form can see their forms 
///ProposalForm
using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.XtraSpreadsheet.Commands;
using DoAn.Module.BusinessObjects.Authentication;
using DoAn.Module.BusinessObjects.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn.Module.Controllers.ProposalForm_ListView
{
    public class ConditionView : ViewController
    {
        public ConditionView()
        {
            TargetViewId = "ProposalForm_ListView";
    
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            SetFilter();
        }
        protected override void OnViewControlsCreated()
        {
            base.OnViewControlsCreated();
            ObjectSpace.Reloaded += ObjectSpace_Reloaded;
        }

        private void ObjectSpace_Reloaded(object sender, EventArgs e)
        {
            SetFilter();
        }
        private void SetFilter()
        {
            if (View is ListView listView)
            {
                ApplicationUser nv = Define.GetCurrentNhanvien();
                ((ListView)View).CollectionSource.Criteria["filter"] = CriteriaOperator.Parse("user.Oid=?", nv.Oid);
            }   
        }
    }
}
