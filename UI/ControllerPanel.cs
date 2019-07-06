using BaseLibrary;
using BaseLibrary.UI;
using BaseLibrary.UI.Elements;
using DimensionalStorage.Components;

namespace DimensionalStorage.UI
{
	public class ControllerPanel : BaseUIPanel<Controller>
	{
		public override void OnInitialize()
		{
			Width = Height = (0, 0.25f);
			this.Center();

			UIText textLabel = new UIText("Controller")
			{
				HAlign = 0.5f
			};
			Append(textLabel);

			UIText textDriveCapacity = new UIText("Drive Capacity: " + Container.Parent.Network.DriveCapacity)
			{
				Top = (28, 0)
			};
			Append(textDriveCapacity);

			UIText textPortCapacity = new UIText("Port Capacity: " + Container.Parent.Network.PortCapacity)
			{
				Top = (56, 0)
			};
			Append(textPortCapacity);
		}
	}
}