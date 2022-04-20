﻿using System;
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
			// TODO: move this to a dictionary or a configuration so I don't have to
			// modify the code each time the params for calc the cost are modified.
			switch (parcel.Size)
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
					throw new ArgumentException("Package size is not in the correct format.");
			}
		}

		private void CalculateTotalCost(Order order)
		{
			order.TotalCost = order.ParcelsCosts.Values.Sum();
		}

		public Order CreateOrder(IEnumerable<Parcel> parcels)
		{
			var order = new Order();
			AddParcelsTo(parcels, order);
			CalculateTotalCost(order);
			return order;
		}
	}
}