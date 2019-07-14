using BaseLibrary;
using BaseLibrary.UI;
using BaseLibrary.UI.Elements;
using DimensionalStorage.Components;
using Microsoft.Xna.Framework;
using System.Linq;

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

			UITextButton buttonClose = new UITextButton("X")
			{
				Size = new Vector2(20),
				Left = (-20, 1),
				RenderPanel = false
			};
			buttonClose.OnClick += (evt, element) => BaseLibrary.BaseLibrary.PanelGUI.UI.CloseUI(Container);
			Append(buttonClose);

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

			UIText textItemCapacity = new UIText("Item Capacity: " + Container.Parent.Network.GetDrives().Sum(drive => drive.Slots))
			{
				Top = (84, 0)
			};
			Append(textItemCapacity);
		}
	}
}