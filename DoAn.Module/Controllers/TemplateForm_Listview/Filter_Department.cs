using DevExpress.Data.Filtering;
using DevExpress.ExpressApp;
using DevExpress.XtraRichEdit.Model;
using DevExpress.XtraSpreadsheet.Commands;
using DoAn.Module.BusinessObjects.Authentication;
using DoAn.Module.BusinessObjects.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DoAn.Module.Controllers.TemplateForm_Listview
{
    public class Filter_Department:ViewController<ListView>
    {
        public Filter_Department()
        {
            //TargetViewId = "TemplateForm_ListView";
            //TargetViewId = "TemplateForm_LookupListView";
            TargetObjectType = typeof(TemplateForm);
            TargetViewType=ViewType.ListView;
        }
        protected override void OnActivated()
        {
            base.OnActivated();
            filter();
        }
        private void filter()
        {
            if (View is ListView listView)
            {
                

                ApplicationUser nv = Define.GetCurrentNhanvien();
                Department nv_de = ObjectSpace.GetObjectByKey<Department>(nv.depart.Oid);
                var collectionSource = View.CollectionSource;
                collectionSource.Criteria["DepartmentFilter"] = new ContainsOperator("Departments", new InOperator("Oid", nv_de.Oid));
            }
        }
    }
}
