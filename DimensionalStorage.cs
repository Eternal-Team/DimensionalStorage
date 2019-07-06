using BaseLibrary;
using DimensionalStorage.Network;
using Starbound.Input;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;

namespace DimensionalStorage
{
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

			NetworkLayer = new NetworkLayer();

			if (!Main.dedServ)
			{
				MouseEvents.ButtonPressed += (sender, args) =>
				{
					if (args.Button != MouseButton.Right) return;

					if (NetworkLayer.Interact())
					{
						Main.mouseRight = Main.mouseRightRelease = false;
						if (PlayerInput.MouseKeys.Contains("Mouse2")) PlayerInput.MouseKeys.Remove("Mouse2");
					}
				};
			}
		}

		public override void Unload()
		{
			Utility.UnloadNullableTypes();
		}
	}
}