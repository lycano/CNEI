﻿namespace AtomicTorch.CBND.CNEI.UI.Controls.Game.CNEI.Data
{
    using AtomicTorch.CBND.GameApi.Data.Items;
    using AtomicTorch.CBND.GameApi.Scripting;
    using System.Collections.Generic;

    public class ProtoItemViewModel : ProtoEntityViewModel
    {
        public ProtoItemViewModel(IProtoItem item) : base(item, item.Icon)
        {
            this.Description = item.Description;
            this.IsStackable = item.IsStackable;
            this.MaxItemsPerStack = item.MaxItemsPerStack;
        }

        public string Description { get; }

        public bool IsStackable { get; }

        public ushort MaxItemsPerStack { get; }

        public FilteredObservableWithPaging<RecipeViewModel> RecipeVMList { get; private set; }

        public FilteredObservableWithPaging<RecipeViewModel> UsageVMList { get; private set; }
    }
}