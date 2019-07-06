using DimensionalStorage.Components;
using LayerLibrary;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Linq;
using Terraria.ModLoader.IO;

namespace DimensionalStorage.Network
{
	public class Cable : ModLayerElement<Cable>
	{
		public override string Texture => "DimensionalStorage/Textures/Network/Cable";

		public override int DropItem => DimensionalStorage.Instance.ItemType<Items.Cable>();

		public Network Network;

		public BaseComponent Component;

		public Cable()
		{
			Network = new Network(this);
		}

		public override void OnPlace()
		{
			Merge();
		}

		public override void OnRemove()
		{
			Network.RemoveCable(this);
		}

		public void Merge()
		{
			foreach (Cable tube in GetNeighbors()) tube.Network.Merge(Network);
		}

		public override void Draw(SpriteBatch spriteBatch)
		{
			base.Draw(spriteBatch);

			Component?.Draw(spriteBatch);
		}

		public override void Update()
		{
			Component?.Update();
		}

		public override TagCompound Save()
		{
			TagCompound tag = new TagCompound();
			if (Component != null)
			{
				tag["Component"] = new TagCompound
				{
					["Type"] = Component.GetType().Name,
					["Data"] = Component.Save()
				};
			}

			return tag;
		}

		public override void Load(TagCompound tag)
		{
			if (tag.ContainsKey("Component"))
			{
				TagCompound component = tag.GetCompound("Component");
				Component = (BaseComponent)Activator.CreateInstance(DimensionalStorage.Instance.Code.GetTypes().First(type => type.IsSubclassOf(typeof(BaseComponent)) && type.Name == component.GetString("Type")));
				Component.Parent = this;
				Component.Load(component.GetCompound("Data"));
			}
		}
	}
}