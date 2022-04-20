namespace CourierKata
{
	public class Parcel
	{
		public int Height { get; private set; }
		public int Width { get; private set; }
		public int Length { get; private set; }
		public int Weight { get; private set; }
		public int OverWeight { get; private set; }
		public bool IsHeavyParcel { get; private set; }
		public ParcelSize Size { get; private set; }
		public enum ParcelSize
		{
			Small,
			Medium,
			Large,
			XL
		}
		public class ParcelBuilder
		{
			private readonly Parcel _parcel = new Parcel();
			public Parcel Build() => _parcel;
			public ParcelBuilder SetDimensions(int height, int width, int length)
			{
				// TODO: Add checks of inputs, values can not be null, zero or negative.
				_parcel.Height = height;
				_parcel.Width = width;
				_parcel.Length = length;

				SetSize();

				return this;
			}
			public ParcelBuilder SetWeight(int weight)
            {
				// TODO: Add checks of inputs, values can not be null, zero or negative.
				_parcel.Weight = weight;
				SetOverWeight();
				return this;
			}
			private void SetSize()
			{
				if (_parcel.Height > Values.XL_LOWER_LIMIT || _parcel.Width > Values.XL_LOWER_LIMIT || _parcel.Length > Values.XL_LOWER_LIMIT)
				{
					_parcel.Size = ParcelSize.XL;
				}
				else if (_parcel.Height < Values.SMALL_UPPER_LIMIT && _parcel.Width < Values.SMALL_UPPER_LIMIT && _parcel.Length < Values.SMALL_UPPER_LIMIT)
				{
					_parcel.Size = ParcelSize.Small;
				}
				else if (_parcel.Height < Values.MEDIUM_UPPER_LIMIT && _parcel.Width < Values.MEDIUM_UPPER_LIMIT && _parcel.Length < Values.MEDIUM_UPPER_LIMIT)
				{
					_parcel.Size = ParcelSize.Medium;
				}
				else
				{
					_parcel.Size = ParcelSize.Large;
				}
			}
			private void SetOverWeight()
			{
				// We only consider a parcel over weight when it's over the (previously) XL weight limit
				// and will be treated as a heavy parcel.
				if (Values.XL_WEIGHT_LIMIT < _parcel.Weight)
					_parcel.IsHeavyParcel = true;

				if (_parcel.Weight > Values.HEAVY_PARCEL_LIMIT)
					_parcel.OverWeight = _parcel.Weight - Values.HEAVY_PARCEL_LIMIT;
			}
		}
	}
}
