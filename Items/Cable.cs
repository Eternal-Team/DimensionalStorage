using LayerLibrary;

namespace DimensionalStorage.Items
{
	public class Cable : BaseLayerItem
	{
		public override IModLayer Layer => DimensionalStorage.Instance.NetworkLayer;

		public override void SetStaticDefaults()
		{
			DisplayName.SetDefault("Cable");
		}
	}
}