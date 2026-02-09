using Data.Contexts;
using Domain.Contracts.Repositories._Base;
using Domain.Models.Commands._Base;
using Microsoft.EntityFrameworkCore;
using Shared.Enums;
using Shared.Usefuls;
using System.Linq.Expressions;

namespace Data.Repositories._Base
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly ContextDefault _context;

        public RepositoryBase(ContextDefault context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        /// <summary>
        /// Adiciona uma entidade ao contexto e salva as alterações
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            ValidateEntity(entity);
            await _context.AddAsync(entity, cancellationToken);
            await SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Adiciona uma coleção de entidades ao contexto e salva as alterações
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            var entitiesList = ValidateEntities(entities);
            await _context.AddRangeAsync(entitiesList, cancellationToken);
            await SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Atualiza uma entidade no contexto e salva as alterações
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
        {
            ValidateEntity(entity);
            _context.Update(entity);
            await SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Atualiza uma coleção de entidades no contexto e salva as alterações
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task UpdateRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            var entitiesList = ValidateEntities(entities);
            _context.UpdateRange(entitiesList);
            await SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Remove uma entidade do contexto e salva as alterações
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task RemoveAsync(T entity, CancellationToken cancellationToken = default)
        {
            ValidateEntity(entity);
            _context.Remove(entity);
            await SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Remove uma coleção de entidades do contexto e salva as alterações
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task RemoveRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            var entitiesList = ValidateEntities(entities);
            _context.RemoveRange(entitiesList);
            await SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Salva as alterações pendentes no contexto
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// Verifica se existe alguma entidade que atenda aos critérios especificados
        /// </summary>
        /// <param name="queryWhere">Expressão de filtro</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        /// <returns>True se existe, false caso contrário</returns>
        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> queryWhere, CancellationToken cancellationToken = default)
        {
            if (queryWhere == null)
            {
                throw new ArgumentNullException(nameof(queryWhere));
            }

            var query = BaseQuery(true, queryWhere);
            return await query.AnyAsync(queryWhere, cancellationToken);
        }

        /// <summary>
        /// Conta o número de entidades que atendem aos critérios especificados
        /// </summary>
        /// <param name="queryWhere">Expressão de filtro (opcional)</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        /// <returns>Número de entidades</returns>
        public async Task<int> CountAsync(Expression<Func<T, bool>> queryWhere = null, CancellationToken cancellationToken = default)
        {
            var query = BaseQuery(true, queryWhere);
            return await query.CountAsync(cancellationToken);
        }

        /// <summary>
        /// Obtém a primeira entidade baseada nos critérios especificados
        /// </summary>
        /// <param name="readOnly">Indica se a consulta deve ser somente leitura</param>
        /// <param name="queryWhere">Expressão de filtro para a consulta</param>
        /// <param name="queryIncludes">Expressões de include para objetos relacionados</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        /// <returns>A entidade encontrada ou null se não encontrada</returns>
        public async Task<T> GetFirstAsync(bool readOnly, Expression<Func<T, bool>> queryWhere = null, Expression<Func<T, object>>[] queryIncludes = null, CancellationToken cancellationToken = default)
        {
            var query = BaseQuery(readOnly, queryWhere, queryIncludes);
            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Obtém a primeira entidade baseada nos critérios especificados, porém lança uma exceção se mais de uma entidade for encontrada
        /// </summary>
        /// <param name="readOnly">Indica se a consulta deve ser somente leitura</param>
        /// <param name="queryWhere">Expressão de filtro para a consulta</param>
        /// <param name="queryIncludes">Expressões de include para objetos relacionados</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        /// <returns>A entidade encontrada ou null se não encontrada</returns>
        public async Task<T> GetSingleAsync(bool readOnly, Expression<Func<T, bool>> queryWhere = null, Expression<Func<T, object>>[] queryIncludes = null, CancellationToken cancellationToken = default)
        {
            var query = BaseQuery(readOnly, queryWhere, queryIncludes);
            return await query.SingleOrDefaultAsync(cancellationToken);
        }

        /// <summary>
        /// Seleciona uma lista de registros com base nos critérios especificados
        /// </summary>
        /// <param name="readOnly">Indica se a consulta deve ser somente leitura</param>
        /// <param name="queryWhere">Expressão de filtro para a consulta</param>
        /// <param name="queryIncludes">Expressões de include para objetos relacionados</param>
        /// <param name="queryOrderBy">Expressão de ordenação</param>
        /// <param name="orderDescending">Indica se a ordenação deve ser descendente</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        /// <returns>Lista de entidades</returns>
        public async Task<List<T>> GetAllAsync(bool readOnly, Expression<Func<T, bool>> queryWhere = null, Expression<Func<T, object>>[] queryIncludes = null, Expression<Func<T, object>> queryOrderBy = null, EnumTypeOrdering typeOrdering = EnumTypeOrdering.Asc, CancellationToken cancellationToken = default)
        {
            var query = BaseQuery(readOnly, queryWhere, queryIncludes, queryOrderBy, typeOrdering);
            return await query.ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Seleciona uma lista paginada de registros
        /// </summary>
        /// <param name="readOnly">Indica se a consulta deve ser somente leitura</param>
        /// <param name="pagination">Informações de paginação (número da página e tamanho da página)</param>
        /// <param name="queryOrderBy">Expressão de ordenação (obrigatória para paginação consistente)</param>
        /// <param name="queryWhere">Expressão de filtro para a consulta</param>
        /// <param name="queryIncludes">Expressões de include para objetos relacionados</param>
        /// <param name="cancellationToken">Token de cancelamento</param>
        /// <returns>Tupla contendo o total de registros e a lista paginada</returns>
        /// <exception cref="ArgumentException">Quando os parâmetros de paginação são inválidos</exception>
        public async Task<Tuple<int, List<T>>> GetAllPagedAsync(bool readOnly, CommandPagination pagination, Expression<Func<T, object>> queryOrderBy, Expression<Func<T, bool>> queryWhere = null, Expression<Func<T, object>>[] queryIncludes = null, CancellationToken cancellationToken = default)
        {
            if (pagination == null || queryOrderBy == null)
            {
                throw new ArgumentException("A paginação ou query de paginação não pode ser nula.", nameof(pagination));
            }
            else
            {
                pagination.ValidateCommand();

                if (pagination.IsValid == false)
                {
                    throw new ArgumentException(string.Join("; ", pagination.Notifications.SelectFluntNotification()));
                }
            }

            // Query para contagem (sem includes para melhor performance)
            var countQuery = BaseQuery(true, queryWhere);

            // Obtém o total de registros
            var countTask = await countQuery.CountAsync(cancellationToken);

            // Query principal com paginação
            var itemsQuery = BaseQuery(readOnly, queryWhere, queryIncludes, queryOrderBy, pagination.TypeOrdering);
            itemsQuery = itemsQuery.Skip((pagination.PageNumber - 1) * pagination.PageSize).Take(pagination.PageSize);

            // Obtém os itens da página
            var itemsTask = await itemsQuery.ToListAsync(cancellationToken);

            //Retornando o resultado como uma tupla
            return new Tuple<int, List<T>>(countTask, itemsTask);
        }

        #region Métodos Auxiliares Privados

        private IQueryable<T> BaseQuery(bool readOnly, Expression<Func<T, bool>> queryWhere = null, Expression<Func<T, object>>[] queryIncludes = null, Expression<Func<T, object>> queryOrderBy = null, EnumTypeOrdering typeOrdering = EnumTypeOrdering.Asc)
        {
            IQueryable<T> query;

            query = readOnly ? _context.Set<T>().AsNoTracking() : _context.Set<T>();

            if (queryWhere != null)
            {
                query = query.Where(queryWhere);
            }

            if (queryIncludes?.Any() == true)
            {
                query = queryIncludes.Aggregate(query, (current, include) => current.Include(include));
            }

            if (queryOrderBy != null)
            {
                query = typeOrdering == EnumTypeOrdering.Desc ? query.OrderByDescending(queryOrderBy) : query.OrderBy(queryOrderBy);
            }

            return query;
        }

        private static void ValidateEntity(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }
        }

        private static List<T> ValidateEntities(IEnumerable<T> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            var entitiesList = entities.ToList();

            if (entitiesList.Any() == false)
            {
                throw new ArgumentException("A coleção de entidades não pode estar vazia.", nameof(entities));
            }

            return entitiesList;
        }

        #endregion Métodos Auxiliares Privados
    }
}
