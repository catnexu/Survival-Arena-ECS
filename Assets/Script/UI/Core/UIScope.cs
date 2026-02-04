using Infrastructure;
using UnityEngine;

namespace UI
{
    public static class UIScope
    {
        public static void Build(IServiceLocator locator, GameObject uiRoot)
        {
            var storage = uiRoot.GetComponent<UIStorage>();
            storage.FieldViewProvider.Initialize(storage.UICamera);
            locator.Register<IFieldViewProvider>(storage.FieldViewProvider);
        }
    }
}