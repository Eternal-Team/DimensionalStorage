using ContainerLibrary;
using DimensionalStorage.Items;
using Terraria.ModLoader.IO;

namespace DimensionalStorage.Components
{
	public class DriveBay : BaseComponent
	{
		public override int DropItem => DimensionalStorage.Instance.ItemType<Items.DriveBay>();

		public ItemHandler Drives;

		public DriveBay()
		{
			Drives = new ItemHandler(4);
			Drives.IsItemValid += (slot, item) => item.modItem is BaseDrive;
		}

		public override TagCompound Save()
		{
			return Drives.Save();
		}

		public override void Load(TagCompound tag)
		{
			Drives.Load(tag);
		}

		public override bool Interact()
		{
			// open inventory
			return false;
		}
	}
}