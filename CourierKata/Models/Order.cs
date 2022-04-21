using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CourierKata.Models
{
	public class Order
	{
		public Order()
		{
			ParcelsCosts = new Dictionary<Parcel, int>();
			FreeParcels = new Dictionary<Parcel, int>();
		}

		public bool SpeedyShipping { get; internal set; }
		public int SpeedyShippingCost { get; internal set; }
		public int TotalCost { get; internal set; }
		public IDictionary<Parcel, int> ParcelsCosts { get; private set; }
		public IDictionary<Parcel, int> FreeParcels { get; private set; }
		public int DiscountTotal
		{
			get
			{
				return FreeParcels.Values.Sum();
			}
		}

		public string Invoice { get
			{
				if (_invoice == null)
				{
					_invoice = GetInvoice();
				}
				return _invoice;
			}
		}
		private string _invoice { get; set; }
		private string GetInvoice()
        {
			StringBuilder invoice = new StringBuilder();
			invoice.AppendLine($"Total cost: {TotalCost}");
			foreach(var parcel in ParcelsCosts)
            {
				invoice.AppendLine($"Parcel: {parcel.Key.Size}, Cost: {parcel.Value}");
            }

			invoice.AppendLine($"Speedy Shipping Cost: {SpeedyShippingCost}");

			if(DiscountTotal > 0)
            {
				invoice.AppendLine($"Total Discount: - {DiscountTotal}");
			}

			return invoice.ToString();
        }
	}
}
