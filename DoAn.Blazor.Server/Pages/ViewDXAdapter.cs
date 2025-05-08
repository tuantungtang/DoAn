using DevExpress.ExpressApp;
using DevExpress.ExpressApp.Blazor.Components.Models;
using DevExpress.ExpressApp.Blazor.Editors.Adapters;
using DevExpress.ExpressApp.Editors;
using Microsoft.AspNetCore.Components;

namespace DoAn.Blazor.Server.Pages
{
    public class ViewDXAdapter : IComponentAdapter, IComplexControl
    {
        private XafApplication application;
        private RenderFragment component;
        public RenderFragment ComponentContent
        {
            get
            {
                if (component == null)
                {
                    component = builder => {
                        builder.OpenComponent<ViewDX>(0);
                        //builder.AddAttribute(1, nameof(Shopping.Title), application.Title);
                        builder.CloseComponent();
                    };
                }
                return component;
            }
        }

        public IComponentModel ComponentModel => throw new NotImplementedException();

        public void Refresh() { }
        public void Setup(IObjectSpace objectSpace, XafApplication application)
        {
            this.application = application;
        }
        public object GetValue()
        {
            return null;
        }
        public void SetValue(object value) { }
        public event EventHandler ValueChanged;
    }
}
