using System;
using System.Collections.Generic;
using System.Linq;

using CourierKata.Interfaces;
using CourierKata.Models;

namespace CourierKata.Services
{
	public class OrderService : IOrderService
	{
		private void AddParcelsTo(IEnumerable<Parcel> parcels, Order order)
		{
			foreach(Parcel parcel in parcels)
			{
				AddParcelTo(parcel, order);
			}
		}

		private void AddParcelTo(Parcel parcel, Order order)
		{
			var parcelCost = CalculateParcelCost(parcel);
			order.ParcelsCosts.Add(parcel, parcelCost);
		}

		private int CalculateParcelCost(Parcel parcel)
		{
			var totalParcelCost = 0;
			if (parcel.OverWeight > 0)
				totalParcelCost += parcel.OverWeight * Values.OVERWEIGHT_COST;

			// TODO: move this to a dictionary or a configuration so I don't have to
			// modify the code each time the params for calc the cost are modified.
			switch (parcel.Size)
			{
				case Parcel.ParcelSize.Small:
					totalParcelCost += Values.SMALL_PARCEL_COST;
					break;
				case Parcel.ParcelSize.Medium:
					totalParcelCost +=  Values.MEDIUM_PARCEL_COST;
					break;
				case Parcel.ParcelSize.Large:
					totalParcelCost +=  Values.LARGE_PARCEL_COST;
					break;
				case Parcel.ParcelSize.XL:
					totalParcelCost +=  Values.XL_PARCEL_COST;
					break;
				default:
					throw new ArgumentException("Parcel size is not in the correct format.");
			}
			return totalParcelCost;
		}
		private void CalculateTotalCost(Order order)
		{
			var parcelsCost = order.ParcelsCosts.Values.Sum();
			if (order.SpeedyShipping)
			{
				order.SpeedyShippingCost = parcelsCost;
				order.TotalCost = parcelsCost * Values.SPEEDY_SHIPPING_COST_FACTOR;
			}
			else
			{
				order.TotalCost = parcelsCost;
			}
		}

		public Order CreateOrder(IEnumerable<Parcel> parcels)
		{
			return CreateOrder(parcels, false);
		}
		public Order CreateOrder(IEnumerable<Parcel> parcels, bool speedyShipping)
		{
			var order = new Order();
			AddParcelsTo(parcels, order);
			order.SpeedyShipping = speedyShipping;
			CalculateTotalCost(order);
			return order;
		}
	}
}
