using BaseLibrary;
using DimensionalStorage.Network;
using Terraria.ModLoader;

namespace DimensionalStorage
{
	// central processor, co-processors (IO, disks, autocrafting...)
	// IO (items, fluids, energy???) (import, export, storage bus)
	// disks (various sizes)
	// auto-crafting (tree)
	// terminals (crafting - similar to what Container Library does)
	// security - player whitelists
	// remote access (hand-held item, quantum bridges)

	public class DimensionalStorage : Mod
	{
		internal static DimensionalStorage Instance;
		public NetworkLayer NetworkLayer;

		public override void Load()
		{
			Instance = this;

			NetworkLayer=new NetworkLayer();
		}

		public override void Unload()
		{
			Utility.UnloadNullableTypes();
		}
	}
}