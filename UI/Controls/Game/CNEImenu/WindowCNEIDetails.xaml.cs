﻿namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu
{
    using AtomicTorch.CBND.CoreMod.ClientComponents.Input;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.GameApi.Scripting;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using System.Collections.Generic;

    public partial class WindowCNEIdetails : BaseUserControlWithWindow
    {
        private Stack<ProtoEntityViewModel> entityVMStack = new Stack<ProtoEntityViewModel>();

        private static ClientInputContext windowInputContext;

        public static WindowCNEIdetails Instance { get; private set; }

        public static WindowCNEIdetails Open(ProtoEntityViewModel entityViewModel)
        {
            if (Instance == null)
            {
                var instance = new WindowCNEIdetails();
                instance.entityVMStack.Push(entityViewModel);
                Instance = instance;
                Api.Client.UI.LayoutRootChildren.Add(instance);
            }
            else
            {
                if (Instance.entityVMStack.Peek() != entityViewModel)
                {
                    Instance.entityVMStack.Push(entityViewModel);
                    Instance.DataContext = Instance.entityVMStack.Peek();
                }
            }
            return Instance;
        }

        protected override void OnLoaded()
        {
            base.OnLoaded();
            Resources.MergedDictionaries.Add(EntityViewModelsManager.AllEntityTemplatesResourceDictionary);
            DataContext = entityVMStack.Peek();
            windowInputContext = ClientInputContext.Start("CNEI details")
                .HandleButtonDown(GameButton.CNEImenuBack, OnBackButtonDown);
        }

        private protected void OnBackButtonDown()
        {
            if (entityVMStack.Count > 1)
            {
                entityVMStack.Pop();
                DataContext = entityVMStack.Peek();
            }
            else
            {
                CloseWindow();
            }
        }

        protected override void OnUnloaded()
        {
            base.OnUnloaded();
            DataContext = null;
            entityVMStack.Clear();
            windowInputContext?.Stop();
            windowInputContext = null;
            if (Instance == this)
            {
                Instance = null;
            }
        }
    }
}