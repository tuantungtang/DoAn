//lock decline form in detail view
using DevExpress.ExpressApp;
using DoAn.Module.BusinessObjects.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn.Module.Controllers.ProposalForm_DetailView
{
    public class ReadOnlyForm : ViewController
    {
        public ReadOnlyForm()
        {
            TargetViewId = "ProposalForm_DetailView";
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            Decline();
        }
        private void Decline()
        {
            if (View is DetailView detailView)
            {
                var obj = detailView.CurrentObject as ProposalForm;

                if (!obj.Status.Equals(0))
                {
                    View.AllowEdit["ReadOnly"] = false;
                }
            }
        }
    }
}