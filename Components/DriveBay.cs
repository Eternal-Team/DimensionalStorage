using BaseLibrary.UI;
using ContainerLibrary;
using DimensionalStorage.Items;
using System;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace DimensionalStorage.Components
{
	public class DriveBay : BaseComponent, IHasUI
	{
		public override int DropItem => DimensionalStorage.Instance.ItemType<Items.DriveBay>();

		public ItemHandler Drives;

		public DriveBay()
		{
			ID = Guid.NewGuid();

			Drives = new ItemHandler(4);
			Drives.IsItemValid += (slot, item) => item.modItem is BaseDrive;
		}

		public override TagCompound Save() => new TagCompound
		{
			["ID"] = ID.ToString(),
			["Drives"] = Drives.Save()
		};

		public override void Load(TagCompound tag)
		{
			ID = tag.ContainsKey("ID") ? Guid.Parse(tag.GetString("ID")) : ID;
			Drives.Load(tag.GetCompound("Drives"));
		}

		public override bool Interact()
		{
			BaseLibrary.BaseLibrary.PanelGUI.UI.HandleUI(this);

			return true;
		}

		public Guid ID { get; set; }
		public BaseUIPanel UI { get; set; }
		public LegacySoundStyle CloseSound => SoundID.Item1;
		public LegacySoundStyle OpenSound => SoundID.Item1;
	}
}