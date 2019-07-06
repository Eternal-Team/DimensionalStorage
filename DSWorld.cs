using System.Linq;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace DimensionalStorage
{
	public class DSWorld : ModWorld
	{
		public override TagCompound Save() => new TagCompound
		{
			["Network"] = DimensionalStorage.Instance.NetworkLayer.Save()
		};

		public override void Load(TagCompound tag)
		{
			DimensionalStorage.Instance.NetworkLayer.Load(tag.GetList<TagCompound>("Network").ToList());
		}

		public override void PostDrawTiles()
		{
			Main.spriteBatch.Begin();
			DimensionalStorage.Instance.NetworkLayer.Draw(Main.spriteBatch);
			Main.spriteBatch.End();
		}

		public override void PostUpdate()
		{
			DimensionalStorage.Instance.NetworkLayer.Update();
		}
	}
}