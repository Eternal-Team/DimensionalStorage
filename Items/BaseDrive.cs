using BaseLibrary.Items;
using ContainerLibrary;
using System.Collections.Generic;
using System.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace DimensionalStorage.Items
{
	public abstract class BaseDrive : BaseItem, IItemHandler
	{
		public override bool CloneNewInstances => true;

		public ItemHandler Handler { get; set; }

		public BaseDrive()
		{
			Handler = new ItemHandler(10);
		}

		public override ModItem Clone()
		{
			BaseDrive clone = (BaseDrive)base.Clone();
			clone.Handler = Handler.Clone();
			return clone;
		}

		public override void SetDefaults()
		{
			item.rare = 0;
		}

		public override void ModifyTooltips(List<TooltipLine> tooltips)
		{
			//tooltips.Add(new TooltipLine(mod, "PortableStorage:BagTooltip", Language.GetText("Mods.PortableStorage.BagTooltip." + GetType().Name).Format(Handler.Slots)));
		}

		public override TagCompound Save() => new TagCompound
		{
			["Items"] = Handler.Save()
		};

		public override void Load(TagCompound tag)
		{
			Handler.Load(tag.GetCompound("Items"));
		}

		public override void NetSend(BinaryWriter writer)
		{
			Handler.Serialize(writer);
		}

		public override void NetRecieve(BinaryReader reader)
		{
			Handler.Deserialize(reader);
		}
	}

	public class BasicDrive : BaseDrive
	{
		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Basic Drive");
		}
	}
}