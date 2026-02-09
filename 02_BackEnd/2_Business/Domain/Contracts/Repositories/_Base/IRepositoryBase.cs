using Domain.Models.Commands._Base;
using Shared.Enums;
using System.Linq.Expressions;

namespace Domain.Contracts.Repositories._Base
{
    public interface IRepositoryBase<T>
    {
        Task AddAsync(T entity, CancellationToken cancellationToken = default);
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
        Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        Task RemoveAsync(T entity, CancellationToken cancellationToken = default);
        Task RemoveRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> queryWhere, CancellationToken cancellationToken = default);
        Task<int> CountAsync(Expression<Func<T, bool>> queryWhere = null, CancellationToken cancellationToken = default);
        Task<T> GetFirstAsync(bool readOnly, Expression<Func<T, bool>> queryWhere = null, Expression<Func<T, object>>[] queryIncludes = null, CancellationToken cancellationToken = default);
        Task<T> GetSingleAsync(bool readOnly, Expression<Func<T, bool>> queryWhere = null, Expression<Func<T, object>>[] queryIncludes = null, CancellationToken cancellationToken = default);
        Task<List<T>> GetAllAsync(bool readOnly, Expression<Func<T, bool>> queryWhere = null, Expression<Func<T, object>>[] queryIncludes = null, Expression<Func<T, object>> queryOrderBy = null, EnumSortOrder sortOrder = EnumSortOrder.Ascending, CancellationToken cancellationToken = default);
        Task<Tuple<int, List<T>>> GetAllPagedAsync(bool readOnly, CommandPagination pagination, Expression<Func<T, object>> queryOrderBy, Expression<Func<T, bool>> queryWhere = null, Expression<Func<T, object>>[] queryIncludes = null, CancellationToken cancellationToken = default);
    }
}
