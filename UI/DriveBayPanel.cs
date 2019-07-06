using BaseLibrary;
using BaseLibrary.UI;
using ContainerLibrary;
using DimensionalStorage.Components;

namespace DimensionalStorage.UI
{
	public class DriveBayPanel : BaseUIPanel<DriveBay>
	{
		public override void OnInitialize()
		{
			Width = Height = (0, 0.25f);
			this.Center();

			for (int i = 0; i < Container.Drives.Slots; i++)
			{
				UIContainerSlot slot = new UIContainerSlot(() => Container.Drives, i)
				{
					Left = (44 * i, 0)
				};
				Append(slot);
			}
		}
	}
}