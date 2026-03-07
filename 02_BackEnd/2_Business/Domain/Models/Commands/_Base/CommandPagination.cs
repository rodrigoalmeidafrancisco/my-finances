using Shared.Enums;

namespace Domain.Models.Commands._Base
{
    public class CommandPagination : CommandRequestNotification
    {
        public CommandPagination()
        {

        }

        public CommandPagination(int pageNumber, int pageSize, EnumSortOrder sortOrder, string columnOrderName, string valueResearch)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            SortOrder = sortOrder;
            ColumnOrderName = columnOrderName;
            ValueResearch = valueResearch;

            ValidateCommand();
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public EnumSortOrder SortOrder { get; set; }
        public string ColumnOrderName { get; set; }
        public string ValueResearch { get; set; }

        public override void ValidateCommand()
        {
            if (PageNumber <= 0)
            {
                AddNotification(nameof(PageNumber), "Número da página deve ser maior que zero.");
            }

            if (PageSize <= 0)
            {
                AddNotification(nameof(PageSize), "Tamanho da página deve ser maior que zero.");
            }
        }

    }
}
