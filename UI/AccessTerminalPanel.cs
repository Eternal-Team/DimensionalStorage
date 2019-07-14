using BaseLibrary;
using BaseLibrary.UI;
using BaseLibrary.UI.Elements;
using DimensionalStorage.Components;
using Microsoft.Xna.Framework;
using Terraria;

namespace DimensionalStorage.UI
{
	public class AccessTerminalPanel : BaseUIPanel<AccessTerminal>
	{
		private UIGrid<UIGridSlot> gridItems;

		public override void OnInitialize()
		{
			Width = Height = (0, 0.25f);
			this.Center();

			UIText textLabel = new UIText("Access Terminal")
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

			gridItems = new UIGrid<UIGridSlot>(9)
			{
				Top = (28, 0),
				Width = (0, 1),
				Height = (-28, 1)
			};
			gridItems.OnClick += (a, b) =>
			{
				Container.Parent.Network.InsertItem(ref Main.mouseItem);
				PopulateGrid();
			};
			Append(gridItems);

			PopulateGrid();
		}

		public void PopulateGrid()
		{
			gridItems.Clear();

			// technically only care about unique types and stacks

			foreach (Item item in Container.Parent.Network.Items)
			{
				UIGridSlot slot = new UIGridSlot(item);
				gridItems.Add(slot);
			}
		}
	}
}