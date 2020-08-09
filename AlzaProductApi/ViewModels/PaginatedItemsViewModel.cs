using System.Collections.Generic;

namespace AlzaProductApi.ViewModels
{
	/// <summary>
	/// Wrapper for paginated responses
	/// </summary>
	/// <typeparam name="TEntity">Type of paginated model</typeparam>
	public class PaginatedItemsViewModel<TEntity> where TEntity : class
	{
		/// <summary>
		/// Index of current page
		/// </summary>
		public int PageIndex { get; private set; }
		/// <summary>
		/// Size of page
		/// </summary>
		public int PageSize { get; private set; }
		/// <summary>
		/// Total number of items
		/// </summary>
		public long Count { get; private set; }
		/// <summary>
		/// Items of current page
		/// </summary>
		public IEnumerable<TEntity> Data { get; private set; }
		/// <summary>
		/// Constructor
		/// </summary>
		/// <param name="pageIndex">Page index</param>
		/// <param name="pageSize">Page size</param>
		/// <param name="count">Count</param>
		/// <param name="data">Data</param>

		public PaginatedItemsViewModel(int pageIndex, int pageSize, long count, IEnumerable<TEntity> data)
		{
			this.PageIndex = pageIndex;
			this.PageSize = pageSize;
			this.Count = count;
			this.Data = data;
		}
	}
}