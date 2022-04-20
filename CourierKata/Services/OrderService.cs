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

		private int CalculateCostForSize(Parcel.ParcelSize size)
		{
			// TODO: move this to a dictionary or a configuration so I don't have to
			// modify the code each time the params for calc the cost are modified.
			switch (size)
			{
				case Parcel.ParcelSize.Small:
					return Values.SMALL_PARCEL_COST;
				case Parcel.ParcelSize.Medium:
					return Values.MEDIUM_PARCEL_COST;
				case Parcel.ParcelSize.Large:
					return Values.LARGE_PARCEL_COST;
				case Parcel.ParcelSize.XL:
					return Values.XL_PARCEL_COST;
				default:
					throw new ArgumentException("Parcel size is not in the correct format.");
			}
		}
		private int CalculateParcelCost(Parcel parcel)
		{
			var totalParcelCost = 0;

			// TODO: Give more thought to how to add cost for overweight.
			if (parcel.IsHeavyParcel)
				totalParcelCost += Values.HEAVY_PARCEL_COST;
			else
				totalParcelCost += CalculateCostForSize(parcel.Size);

			if (parcel.OverWeight > 0)
				totalParcelCost += parcel.OverWeight * Values.OVERWEIGHT_COST;

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
