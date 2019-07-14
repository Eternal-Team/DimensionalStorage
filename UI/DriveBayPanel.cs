using BaseLibrary;
using BaseLibrary.UI;
using BaseLibrary.UI.Elements;
using ContainerLibrary;
using DimensionalStorage.Components;
using Microsoft.Xna.Framework;

namespace DimensionalStorage.UI
{
	public class DriveBayPanel : BaseUIPanel<DriveBay>
	{
		public override void OnInitialize()
		{
			Width = (188, 0);
			Height = (84, 0);
			this.Center();

			UIText textLabel = new UIText("Drive Bay")
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

			for (int i = 0; i < Container.Drives.Slots; i++)
			{
				UIContainerSlot slot = new UIContainerSlot(() => Container.Drives, i)
				{
					Top = (28, 0),
					Left = (44 * i, 0)
				};
				Append(slot);
			}
		}
	}
}