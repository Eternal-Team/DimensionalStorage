using DimensionalStorage.Components;
using LayerLibrary;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader.IO;

namespace DimensionalStorage.Network
{
	public class NetworkLayer : ModLayer<Cable>
	{
		public override int TileSize => 1;

		public override void Load(List<TagCompound> list)
		{
			base.Load(list);

			foreach (Cable cable in data.Values) cable.Merge();
		}

		public override void Update()
		{
			base.Update();

			foreach (Network network in Network.Networks) network.Update();
		}

		public bool PlaceComponent<T>(BaseComponentItem<T> item) where T : BaseComponent, new()
		{
			int posX = Player.tileTargetX;
			int posY = Player.tileTargetY;

			if (TryGetValue(posX, posY, out Cable cable) && cable.Component == null)
			{
				cable.Component = new T {Parent = cable};
				return true;
			}

			return false;
		}

		public void RemoveComponent()
		{
			int posX = Player.tileTargetX;
			int posY = Player.tileTargetY;

			if (TryGetValue(posX, posY, out Cable cable) && cable.Component != null)
			{
				Item.NewItem(posX * 16, posY * 16, 16, 16, cable.Component.DropItem);
				cable.Component = null;
			}
		}

		public override void Remove()
		{
			RemoveComponent();
			base.Remove();
		}

		public override bool Interact()
		{
			if (TryGetValue(Player.tileTargetX, Player.tileTargetY, out Cable cable))
			{
				return cable.Interact();
			}

			return false;
		}
	}
}