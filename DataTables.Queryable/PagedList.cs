﻿using System.Collections.Generic;
using System.Linq;

namespace DataTables.Queryable
{
    /// <summary>
    /// Collection of items that represents a single page of data extracted from the <see cref="IDataTablesQueryable{T}"/>
    /// after applying <see cref="DataTablesRequest{T}"/> filter.
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    public interface IPagedList<T> : IList<T>
    {
        /// <summary>
        /// Total items count in the whole collection 
        /// </summary>
        int TotalCount { get; }

        /// <summary>
        /// Count of items per page
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// 1-bazed page number
        /// </summary>
        int PageNumber { get; }

        /// <summary>
        /// Total number of pages in the whole collection
        /// </summary>
        int PagesCount { get; }
    }

    /// <summary>
    /// Internal implementation of <see cref="IPagedList{T}"/> interface.
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    internal class PagedList<T> : List<T>, IPagedList<T>
    {
        public int TotalCount { get; protected set; }
        public int PageNumber { get; protected set; }
        public int PageSize { get; protected set; }
        public int PagesCount { get; protected set; }

        /// <summary>
        /// Creates new instance of <see cref="PagedList{T}"/> collection.
        /// </summary>
        /// <param name="queryable"><see cref="IDataTablesQueryable{T}"/>instance to be paginated</param>
        internal PagedList(IDataTablesQueryable<T> queryable) : base()
        {
            // pagination is on
            if (queryable.Request.PageSize > 0)
            {
                int skipCount = (queryable.Request.PageNumber - 1) * queryable.Request.PageSize;
                int takeCount = queryable.Request.PageSize;

                TotalCount = queryable.Count();
                PageNumber = queryable.Request.PageNumber;
                PageSize = queryable.Request.PageSize;
                PagesCount = TotalCount % PageSize == 0 ? TotalCount / PageSize : TotalCount / PageSize + 1;

                AddRange(queryable.Skip(skipCount).Take(takeCount).ToList());
            }
            // no pagination
            else
            {
                TotalCount = queryable.Count();
                PageNumber = 1;
                PageSize = -1;
                PagesCount = 1;

                AddRange(queryable.ToList());
            }
        }
    }
}
