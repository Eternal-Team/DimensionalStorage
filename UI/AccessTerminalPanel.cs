using BaseLibrary;
using BaseLibrary.UI;
using BaseLibrary.UI.Elements;
using DimensionalStorage.Components;
using Microsoft.Xna.Framework;
using System.Linq;
using Terraria;

namespace DimensionalStorage.UI
{
	public class AccessTerminalPanel : BaseUIPanel<AccessTerminal>
	{
		private UIGrid<UIGridSlot> gridItems;

		private Network.Network Network => Container.Parent.Network;

		public override void OnInitialize()
		{
			Width = (408, 0);
			Height = (304, 0);
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
			gridItems.OnClick += (evt, element) => PopulateGrid();
			gridItems.OnRightClick += (evt, element) => PopulateGrid();
			Append(gridItems);

			PopulateGrid();
		}

		public void PopulateGrid()
		{
			gridItems.Clear();

			foreach (Item item in Network.Items)
			{
				if (item.maxStack > 1 && gridItems.items.Any(gridSlot => gridSlot.Item.type == item.type)) continue;

				UIGridSlot slot = new UIGridSlot(Network, item);
				gridItems.Add(slot);
			}

			while (gridItems.Count < 54 || gridItems.Count % 9 != 0)
			{
				gridItems.Add(new UIGridSlot(Network, new Item()));
			}
		}
	}
}