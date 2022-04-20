using CourierKata;
using CourierKata.Interfaces;
using CourierKata.Services;
using NUnit.Framework;
using System.Collections.Generic;

namespace CourierKataTest
{
	public class OrderTests
	{
		private IOrderService _orderService;

		[SetUp]
		public void SetUp()
		{
			_orderService = new OrderService();
		}

		[Test]
		public void Order_Created_For_A_Set_Of_Parcels_Contains_The_Expected_Number_Of_Parcels()
		{
			// Arrange
			var smallParcel = new Parcel.ParcelBuilder()
				.SetDimensions(8, 8, 8)
				.Build();
			int expectedCost = 3;
			ICollection<Parcel> parcels = new List<Parcel>();
			parcels.Add(smallParcel);

			// Act
			var order = _orderService.CreateOrder(parcels);
		
			// Assert
			Assert.AreEqual(order.ParcelsCosts.Count, parcels.Count);
			Assert.AreEqual(order.TotalCost, expectedCost);
		}

		[Test]
		public void Invoice_Of_An_Order_Has_Correct_Format()
		{
			// Arrange
			var smallParcel = new Parcel.ParcelBuilder()
				.SetDimensions(8, 8, 8)
				.Build();
			int expectedCost = 3;
			ICollection<Parcel> parcels = new List<Parcel>();
			parcels.Add(smallParcel);

			// Act
			var order = _orderService.CreateOrder(parcels);
			var invoice = order.Invoice;

			// Assert
			Assert.That(invoice, Does.Contain($"Total cost: {expectedCost}"));
			Assert.That(invoice, Does.Contain($"Parcel: {smallParcel.Size}, Cost: 3"));
		}
		// TODO: Add test cases that prove scenarios other than the 'happy path'.
	}
}
