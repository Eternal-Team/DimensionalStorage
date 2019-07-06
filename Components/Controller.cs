namespace DimensionalStorage.Components
{
	public class Controller : BaseComponent
	{
		public override int DropItem => DimensionalStorage.Instance.ItemType<Items.Controller>();

		public override int PortCapacity => 8;
		public override int DriveCapacity => 4;
	}
}