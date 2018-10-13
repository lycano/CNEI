﻿namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.Characters;
    using AtomicTorch.CBND.CoreMod.StaticObjects.Loot;
    using AtomicTorch.CBND.CoreMod.StaticObjects.Minerals;
    using AtomicTorch.CBND.CoreMod.StaticObjects.Structures;
    using AtomicTorch.CBND.CoreMod.StaticObjects.Vegetation;
    using AtomicTorch.CBND.CoreMod.Systems.ServerOperator;
    using AtomicTorch.CBND.CoreMod.UI.Controls.Core;
    using AtomicTorch.CBND.GameApi.Data.Items;
    using AtomicTorch.GameEngine.Common.Client.MonoGame.UI;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;

    public class ViewModelWindowCNEImenu : BaseViewModel
    {
        private string searchText = string.Empty;

        // Default settings.
        private bool isDefaultViewOn = true;
        private bool isShowingEntityWithTemplates = false;
        private bool isShowingAll = false;

        public static List<Type> defaultViewTypes = new List<Type>()
        {
            typeof(IProtoItem),
            typeof(IProtoObjectStructure),
            typeof(IProtoCharacterMob),
            typeof(IProtoObjectMineral),
            typeof(IProtoObjectLoot),
            typeof(IProtoObjectVegetation),
        };

        public FilteredObservableWithPaging<ProtoEntityViewModel> FilteredEntityVMList { get; }

        public int PageCapacity = 154;

        public BaseCommand NextPage { get; }

        public BaseCommand PrevPage { get; }

        public BaseCommand ChangeViewPreset { get; }

        public BaseCommand ToggleSettings { get; }

        // TODO: rewrite settings filtering (listbox of comboxes to select what types to show)
        private bool SettingsFilter(ProtoEntityViewModel entityVM)
        {
            return IsShowingAll ||
                   (IsShowingEntityWithTemplates && entityVM.GetType().IsSubclassOf(typeof(ProtoEntityViewModel))) ||
                   (IsDefaultViewOn && (defaultViewTypes.Any(t => t.IsInstanceOfType(entityVM.ProtoEntity))));
        }

        private bool SearchFilter(ProtoEntityViewModel entityVM)
        {
            return (entityVM.Title.ToLower().Contains(searchText.ToLower())
                    || entityVM.GetType().ToString().ToLower().Contains(searchText.ToLower())
                    || entityVM.GetType().Name.ToLower().Contains(searchText.ToLower()));
        }

        public ViewModelWindowCNEImenu()
        {
            FilteredEntityVMList =
                new FilteredObservableWithPaging<ProtoEntityViewModel>(EntityViewModelsManager
                    .GetAllEntityViewModels());
            FilteredEntityVMList.AddFilter(SettingsFilter);
            FilteredEntityVMList.AddFilter(SearchFilter);
            FilteredEntityVMList.SetPageCapacity(PageCapacity);

            NextPage = new ActionCommand(() => FilteredEntityVMList.NextPage());
            PrevPage = new ActionCommand(() => FilteredEntityVMList.PrevPage());
            ChangeViewPreset = new ActionCommand(TypeHierarchySelectView.Open);
            ToggleSettings = new ActionCommandWithParameter(isChecked =>
            {
                if ((bool) isChecked == false)
                {
                    EntityViewModelsManager.SaveSettings();
                }
            });
        }

        public string SearchText
        {
            get => searchText;
            set
            {
                value = value?.TrimStart() ?? string.Empty;
                if (searchText == value)
                {
                    return;
                }
                searchText = value;
                NotifyThisPropertyChanged();
                FilteredEntityVMList.Refresh();
            }
        }

        public bool IsDefaultViewOn
        {
            get => isDefaultViewOn;
            set
            {
                if (value == isDefaultViewOn)
                {
                    return;
                }
                isDefaultViewOn = value;
                if (!isDefaultViewOn)
                {
                    isShowingEntityWithTemplates = false;
                    NotifyPropertyChanged("IsShowingEntityWithTemplates");
                    isShowingAll = false;
                    NotifyPropertyChanged("IsShowingAll");
                }
                FilteredEntityVMList.Refresh();
                NotifyThisPropertyChanged();
            }
        }

        public bool IsShowingEntityWithTemplates
        {
            get => isShowingEntityWithTemplates;
            set
            {
                if (value == isShowingEntityWithTemplates)
                {
                    return;
                }
                isShowingEntityWithTemplates = value;
                if (isShowingEntityWithTemplates)
                {
                    IsDefaultViewOn = true;
                }
                else
                {
                    isShowingAll = false;
                    NotifyPropertyChanged("IsShowingAll");
                }
                FilteredEntityVMList.Refresh();
                NotifyThisPropertyChanged();
            }
        }

        public bool IsShowingAll
        {
            get => isShowingAll;
            set
            {
                if (value == isShowingAll)
                {
                    return;
                }
                isShowingAll = value;
                if (isShowingAll)
                {
                    isDefaultViewOn = true;
                    NotifyPropertyChanged("IsDefaultViewOn");
                    isShowingEntityWithTemplates = true;
                    NotifyPropertyChanged("IsShowingEntityWithTemplates");
                }
                FilteredEntityVMList.Refresh();
                NotifyThisPropertyChanged();
            }
        }

        public bool IsTypeVisible
        {
            get => (EntityViewModelsManager.TypeVisibility == Visibility.Visible);
            set
            {
                if (value == IsTypeVisible)
                {
                    return;
                }

                EntityViewModelsManager.TypeVisibility = value ? Visibility.Visible : Visibility.Collapsed;
                FilteredEntityVMList.Refresh();
                NotifyThisPropertyChanged();
            }
        }

        // TODO: Update this on Operator status change.
        public bool IsCreativePanelVisibile
        {
            get => ServerOperatorSystem.ClientIsOperator();
        }
    }
}