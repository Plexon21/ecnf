using System;
using System.Collections.Generic;

namespace Fhnw.Ecnf.RoutePlanner.RoutePlannerLib
{
	public enum TransportMode
	{
		Ship,
		Rail,
		Flight,
		Car,
		Bus,
		Tram
	};
	
	/// <summary>
	/// Represents a link between two cities with its distance
	/// </summary>
	public class Link : IComparable
	{
	    double distance;

	    public TransportMode TransportMode { get; } = TransportMode.Car;

	    public City FromCity { get; }

	    public City ToCity { get; }

	    public Link(City _fromCity, City _toCity, double _distance)
		{
			FromCity = _fromCity;
			ToCity = _toCity;
			distance = _distance;
		}

		public Link(City _fromCity, City _toCity, double _distance, TransportMode _transportMode) : this(_fromCity, _toCity, _distance)
		{
			TransportMode = _transportMode;
		}

		public double Distance
		{
			get
			{
				return distance;
			}
		}
		
		/// <summary>
		/// Uses distance as default comparison criteria 
		/// </summary>
		public int CompareTo(object o)
		{
			return distance.CompareTo(((Link)o).distance);
		}
		
		/// <summary>
		/// checks if both cities of the link are included in the passed city list
		/// </summary>
		/// <param name="cities">list of city objects</param>
		/// <returns>true if both link-cities are in the list</returns>
		internal bool IsIncludedIn(List<City> cities)
		{
			var foundFrom = false;
			var foundTo = false;
			foreach (var c in cities)
			{
				if (!foundFrom && c.Name == FromCity.Name)
					foundFrom = true;
				
				if (!foundTo && c.Name == ToCity.Name)
					foundTo = true;
			}
			
			return foundTo && foundFrom;
		}
	}
}
