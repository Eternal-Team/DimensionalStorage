using ContainerLibrary;
using DimensionalStorage.Components;
using DimensionalStorage.Items;
using System.Collections.Generic;
using System.Linq;
using Controller = DimensionalStorage.Components.Controller;
using DriveBay = DimensionalStorage.Components.DriveBay;

namespace DimensionalStorage.Network
{
	public class Network
	{
		public static List<Network> Networks = new List<Network>();

		private List<Cable> Cables;

		public bool IsValid => Cables.Count(cable => cable.Component is Controller) == 1;

		public int DriveCapacity => Cables.Sum(cable => cable.Component?.DriveCapacity ?? 0);

		public int IOCapacity => Cables.Sum(cable => cable.Component?.IOCapacity ?? 0);

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
		}
	}
}