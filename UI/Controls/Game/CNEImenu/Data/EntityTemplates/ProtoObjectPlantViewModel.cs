﻿namespace CryoFall.CNEI.UI.Controls.Game.CNEImenu.Data
{
    using AtomicTorch.CBND.CoreMod.StaticObjects.Vegetation.Plants;
    using CryoFall.CNEI.UI.Controls.Game.CNEImenu.Managers;
    using JetBrains.Annotations;

    public class ProtoObjectPlantViewModel : ProtoObjectGatherableVegetationViewModel
    {
        public ProtoObjectPlantViewModel([NotNull] IProtoObjectPlant plant) : base(plant)
        {
        }

        /// <summary>
        /// Initilize information about entity - invoked after all entity view Models created,
        /// so you can use links to other entity by using <see cref="EntityViewModelsManager.GetEntityViewModel" />
        /// and <see cref="EntityViewModelsManager.GetAllEntityViewModels" />.
        /// </summary>
        public override void InitInformation()
        {
            base.InitInformation();

            if (ProtoEntity is IProtoObjectPlant plant)
            {
                EntityInformation.Add(new ViewModelEntityInformation("Harvests number",
                    plant.NumberOfHarvests));
            }
        }
    }
}