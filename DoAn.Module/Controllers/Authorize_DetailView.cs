using DevExpress.ExpressApp;
using DoAn.Module.BusinessObjects.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn.Module.Controllers
{
    public class Authorize_DetailView : ViewController
    {
        public Authorize_DetailView()
        {
            TargetViewId = "Authorize_DetailViews";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            ProposalApproval proposalApproval = ObjectSpace.FindObject<ProposalApproval>(null);
            ProposalForm proposalForm = proposalApproval.proposalform;
            
        }
    }
}
