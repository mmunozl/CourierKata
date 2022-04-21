using CourierKata;
using CourierKata.Interfaces;
using CourierKata.Services;

using System.Collections.Generic;

using NUnit.Framework;

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
			bool speedyShipping = true;
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

		[Test]
		public void Order_With_Speedy_Shipping_Costs_Double_Than_Without_Shipping()
		{
			// Arrange
			bool speedyShipping = true;
			var smallParcel = new Parcel.ParcelBuilder()
				.SetDimensions(8, 8, 8)
				.Build();
			int expectedCost = 3;
			ICollection<Parcel> parcels = new List<Parcel>();
			parcels.Add(smallParcel);

			// Act
			var orderWithSpeedyShipping = _orderService.CreateOrder(parcels, speedyShipping);
			var orderWithNormalShipping = _orderService.CreateOrder(parcels);

			// Assert
			Assert.AreEqual(orderWithNormalShipping.TotalCost, expectedCost);
			Assert.That(orderWithSpeedyShipping.TotalCost, Is.EqualTo(2 * orderWithNormalShipping.TotalCost));
		}

		[Test]
		public void Order_With_Speedy_Shipping_Does_Not_Affect_Parcel_Cost()
		{
			// Arrange
			bool speedyShipping = true;
			var smallParcel = new Parcel.ParcelBuilder()
				.SetDimensions(8, 8, 8)
				.Build();
			int expectedCost = 3;
			ICollection<Parcel> parcels = new List<Parcel>();
			parcels.Add(smallParcel);

			// Act
			var orderWithSpeedyShipping = _orderService.CreateOrder(parcels, speedyShipping);
			var orderWithNormalShipping = _orderService.CreateOrder(parcels);

			// Assert
			Assert.AreEqual(orderWithNormalShipping.ParcelsCosts, orderWithSpeedyShipping.ParcelsCosts);
		}

		[Test]
		public void Order_With_Heavy_Parcel_Has_Expected_Cost()
		{
			int weight = 53;
			int expectedCost = 53;

			// Arrange
			var overweightParcel = new Parcel.ParcelBuilder()
				.SetDimensions(8, 8, 8)
				.SetWeight(weight)
				.Build();
			ICollection<Parcel> parcels = new List<Parcel>();
			parcels.Add(overweightParcel);

			// Act
			var order = _orderService.CreateOrder(parcels);

			// Assert
			Assert.AreEqual(order.TotalCost, expectedCost);

		}

		[Test]
		public void Order_With_Multiple_Packages_Applies_Expected_Discount()
        {
			// Arrange

			// Add 10 parcels to the parcel list.
			ICollection<Parcel> parcels = new List<Parcel>();
			for (int i = 0; i < 5; i++)
            {
				parcels.Add
				(
					new Parcel.ParcelBuilder()
					.SetDimensions(8, 8, 8)
					.SetWeight(1)
					.Build()
				);
				parcels.Add
				(
					new Parcel.ParcelBuilder()
						.SetDimensions(15, 15, 15)
						.SetWeight(5)
						.Build()
				);
			}

			//  Expected discount: 2 mixed, 1 small, 1 medium.
			//	 My order parcel setup:
			//	    S M S M S M S M S M  (5S + 5M = 55$)
			//	 My order expectations
			//	    S M S M F M S F S F  (F = 1S + 2M = $19)
			//	 If Small Costs $3 and Medium costs $8:
			//	 The expected total cost is: $28
			//	 The expected discount is: $36

			var expectedTotalCost = 36;
			var expectedDiscountCost = 19;

			// Act
			var order = _orderService.CreateOrder(parcels);

			// Assert
			Assert.That(order.DiscountTotal, Is.EqualTo(expectedDiscountCost));
			Assert.That(order.TotalCost, Is.EqualTo(expectedTotalCost));
		}

		// TODO: Test that speedy shipping comes independently in the invoice.
		// TODO: Take common parts of the tests as setup
		// TODO: Add test cases that prove scenarios other than the 'happy path'.
	}
}
