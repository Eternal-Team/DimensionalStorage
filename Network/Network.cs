using DimensionalStorage.Components;
using System.Collections.Generic;
using System.Linq;

namespace DimensionalStorage.Network
{
	public class Network
	{
		public static List<Network> Networks = new List<Network>();

		private List<Cable> Cables;

		public bool IsValid => Cables.Count(cable => cable.Component is Controller) == 1;

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