using BaseLibrary.UI;
using System;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace DimensionalStorage.Components
{
	public class Controller : BaseComponent, IHasUI
	{
		public override int DropItem => DimensionalStorage.Instance.ItemType<Items.Controller>();

		public override int PortCapacity => 8;
		public override int DriveCapacity => 4;

		public Guid UUID { get; set; }
		public BaseUIPanel UI { get; set; }
		public LegacySoundStyle CloseSound => SoundID.Item1;
		public LegacySoundStyle OpenSound => SoundID.Item1;

		public Controller()
		{
			UUID = Guid.NewGuid();
		}

		public override bool Interact()
		{
			BaseLibrary.BaseLibrary.PanelGUI.UI.HandleUI(this);

			return true;
		}

		public override TagCompound Save() => new TagCompound
		{
			["UUID"] = UUID
		};

		public override void Load(TagCompound tag)
		{
			UUID = tag.Get<Guid>("UUID");
		}
	}
}