using BaseLibrary;
using ContainerLibrary;
using DimensionalStorage.Components;
using DimensionalStorage.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Controller = DimensionalStorage.Components.Controller;
using DriveBay = DimensionalStorage.Components.DriveBay;

namespace DimensionalStorage.Network
{
	public class Network
	{
		public static List<Network> Networks = new List<Network>();

		private List<Cable> Cables;

		public bool IsValid => Cables.Count(cable => cable.Component is Controller) == 1;

		public int DriveCapacity => GetComponents().Sum(component => component.DriveCapacity);

		public int PortCapacity => GetComponents().Sum(component => component.PortCapacity);

		public IEnumerable<BaseComponent> GetComponents()
		{
			foreach (Cable cable in Cables)
			{
				if (cable.Component != null) yield return cable.Component;
			}
		}

		public IEnumerable<ItemHandler> GetDrives()
		{
			int count = 0;
			foreach (BaseComponent component in GetComponents())
			{
				if (component is DriveBay bay)
				{
					foreach (BaseDrive drive in bay.Drives.Items.OfType<BaseDrive>())
					{
						count++;
						if (count > DriveCapacity) yield break;
						yield return drive.Handler;
					}
				}
			}
		}

		public IEnumerable<BasePort> GetPorts()
		{
			int count = 0;
			foreach (BaseComponent component in GetComponents())
			{
				if (component is BasePort port)
				{
					count++;
					if (count > PortCapacity) yield break;
					yield return port;
				}
			}
		}

		public List<Item> Items => GetDrives().SelectMany(drive => drive.Items).ToList();

		public void InsertItem(ref Item item)
		{
			if (item.IsAir) return;

			foreach (Item other in Items)
			{
				if (other.type == item.type && other.stack < other.maxStack)
				{
					int count = Math.Min(item.stack, other.maxStack - other.stack);
					other.stack += count;
					item.stack -= count;
					if (item.stack <= 0)
					{
						item.TurnToAir();
						return;
					}
				}
			}

			foreach (ItemHandler drive in GetDrives())
			{
				for (int i = 0; i < drive.Items.Count; i++)
				{
					Item driveItem = drive.Items[i];
					if (!driveItem.IsAir) continue;

					driveItem.SetDefaults(item.type);
					int count = Math.Min(item.stack, driveItem.maxStack - driveItem.stack);
					driveItem.stack = count;
					item.stack -= count;
					if (item.stack <= 0)
					{
						item.TurnToAir();
						return;
					}
				}
			}
		}

		public Network(Cable tube)
		{
			Networks.Add(this);

			Cables = new List<Cable> {tube};
		}

		public void AddCable(Cable tile)
		{
			if (!Cables.Contains(tile))
			{
				Networks.Remove(tile.Network);
				tile.Network = this;
				Cables.Add(tile);
			}
		}

		public void RemoveCable(Cable tile)
		{
			if (Cables.Contains(tile))
			{
				Cables.Remove(tile);
				Reform();
			}
		}

		public void Merge(Network other)
		{
			foreach (Cable cable in other.Cables) AddCable(cable);
		}

		public void Reform()
		{
			Networks.Remove(this);

			foreach (Cable cable in Cables) cable.Network = new Network(cable);

			foreach (Cable cable in Cables) cable.Merge();
		}

		public void Update()
		{
			if (!IsValid) return;

			foreach (BasePort port in GetPorts()) port.Update();
		}
	}
}