using BaseLibrary;
using DimensionalStorage.Network;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader.IO;

namespace DimensionalStorage.Components
{
	public abstract class BaseComponent
	{
		public Cable Parent;
		public abstract int DropItem { get; }

		public virtual void Draw(SpriteBatch spriteBatch)
		{
			Vector2 position = Parent.Position.ToScreenCoordinates();

			spriteBatch.Draw(Main.magicPixel, new Rectangle((int)position.X, (int)position.Y, 16, 16), Color.Red * 0.5f);
		}

		public virtual void Update()
		{
			Main.NewText(Parent.Network.IsValid);
		}

		public virtual TagCompound Save() => new TagCompound();

		public virtual void Load(TagCompound tag)
		{
		}
	}
}