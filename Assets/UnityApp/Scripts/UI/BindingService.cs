using Loxodon.Framework.Binding;
using Loxodon.Framework.Contexts;
using UnityEngine;

namespace Johnny.SimDungeon
{
    public class BindingService : MonoBehaviour
    {
        public static BindingService Instance
        {
            get
            {
                if (s_Instance == null)
                {
                    s_Instance = FindFirstObjectByType<BindingService>();
                }
                return s_Instance;
            }
        }
        private static BindingService s_Instance;

        public static MainGameViewModel MainPanelViewModel;
        public static BuildableObjectsPanelViewModel BuildableObjectsPanelViewModel;
        public static CategoryObjectsPanelViewModel CategoryObjectsPanelViewModel;


        private BindingServiceBundle m_BindingServiceBundle;

        private void Awake()
        {
            Debug.Log($"[------ BindingService ------ ] : Init");
            var context = Context.GetApplicationContext();
            m_BindingServiceBundle = new BindingServiceBundle(context.GetContainer());
            m_BindingServiceBundle.Start();

            MainPanelViewModel = new MainGameViewModel();
            BuildableObjectsPanelViewModel = new BuildableObjectsPanelViewModel();
            CategoryObjectsPanelViewModel = new CategoryObjectsPanelViewModel();

        }

        private void OnDestroy()
        {
            m_BindingServiceBundle.Stop();
            Debug.Log($"[------ BindingService ------ ] : Stop");
        }
    }
}
