using BaseLibrary.Items;
using ContainerLibrary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace DimensionalStorage.Items
{
	public abstract class BaseDrive : BaseItem, IItemHandler
	{
		public override bool CloneNewInstances => true;

		public abstract int Capacity { get; }

		public ItemHandler Handler { get; set; }

		public BaseDrive()
		{
			Handler = new ItemHandler(Capacity);
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
			tooltips.Add(new TooltipLine(mod, "DimensionalStorage:DriveTooltip", Handler.Items.Count(i => !i.IsAir) + "/" + Capacity));
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
			Handler.Write(writer);
		}

		public override void NetRecieve(BinaryReader reader)
		{
			Handler.Read(reader);
		}
	}

	public class BasicDrive : BaseDrive
	{
		public override int Capacity => 8;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Basic Drive");
		}
	}

	public class AdvancedDrive : BaseDrive
	{
		public override int Capacity => 16;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Advanced Drive");
		}
	}

	public class EliteDrive : BaseDrive
	{
		public override int Capacity => 32;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Elite Drive");
		}
	}

	public class UltimateDrive : BaseDrive
	{
		public override int Capacity => 64;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Ultimate Drive");
		}
	}
}