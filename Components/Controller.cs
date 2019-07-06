namespace DimensionalStorage.Components
{
	public class Controller : BaseComponent
	{
		public override int DropItem => DimensionalStorage.Instance.ItemType<Items.Controller>();
	}
}