using NUnit.Framework;
using CourierKata;

namespace CourierKataTest
{
	public class ParcelTests
	{
		[TestCase(8, 7, 9, Parcel.ParcelSize.Small)]
		[TestCase(40, 30, 20, Parcel.ParcelSize.Medium)]
		[TestCase(60, 40, 60, Parcel.ParcelSize.Large)]
		[TestCase(101, 5, 5, Parcel.ParcelSize.XL)]
		public void Create_Parcel_From_Builder_Returns_Parcel_With_Expected_Size(int length, int width, int height, Parcel.ParcelSize expectedSize)
		{
			var createdParcel = new Parcel.ParcelBuilder()
				.SetDimensions(height, width, length)
				.Build();

			Assert.AreEqual(createdParcel.Size, expectedSize);
		}
		// TODO: Add test cases that prove scenarios other than the 'happy path'.
	}
}