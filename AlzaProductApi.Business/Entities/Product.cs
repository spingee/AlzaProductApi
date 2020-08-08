namespace AlzaProductApi.Business.Entities
{

	public class Product
	{
		public int Id { get; private set; }
		public string Name { get; private set; }
		public string ImgUri { get; private set; }
		public decimal Price { get; private set; }
		public string Description { get; private set; }

		public void ChangeDescription(string description)
		{
			Description = description;
		}

		public static Product Create(int id, string name, decimal price, string imageUri, string description = null)
		{
			return new Product
			{
				Id = id,
				Name = name,
				Price = price,
				ImgUri = imageUri,
				Description = description
			};
		}
	}
}
