using CourierKata.Models;
using System.Collections.Generic;

namespace CourierKata.Interfaces
{
	public interface IOrderService
	{
		public Order CreateOrder(IEnumerable<Parcel> parcels);
		public Order CreateOrder(IEnumerable<Parcel> parcels, bool speedyShipping);
	}
}
