using BaseLibrary;
using BaseLibrary.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader.IO;

namespace DimensionalStorage.Components
{
	public class AccessTerminal : BaseComponent, IHasUI
	{
		public override int DropItem => DimensionalStorage.Instance.ItemType<Items.AccessTerminal>();

		public override int PortCapacity => 8;
		public override int DriveCapacity => 4;

		public Guid UUID { get; set; }
		public BaseUIPanel UI { get; set; }
		public LegacySoundStyle CloseSound => SoundID.Item1;
		public LegacySoundStyle OpenSound => SoundID.Item1;

		public AccessTerminal()
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

		public override void Draw(SpriteBatch spriteBatch)
		{
			Vector2 position = Parent.Position.ToScreenCoordinates();

			spriteBatch.Draw(Main.magicPixel, new Rectangle((int)position.X, (int)position.Y, 16, 16), Color.LimeGreen);
		}
	}
}