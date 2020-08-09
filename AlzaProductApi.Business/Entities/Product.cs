namespace AlzaProductApi.Business.Entities
{

	public class Product
	{
		private byte[] _timestamp;
		public int Id { get; private set; }
		public string Name { get; private set; }
		public string ImgUri { get; private set; }
		public decimal Price { get; private set; }
		public string Description { get; private set; }
		

		public void ChangeDescription(string description)
		{
			Description = description;
		}

		public static Product Create(string name, decimal price, string imageUri, string description = null)
		{
			return new Product
			       {
				       Name = name,
				       Price = price,
				       ImgUri = imageUri,
				       Description = description
			       };
		}
	}
}
