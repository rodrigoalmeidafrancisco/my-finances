using System.ComponentModel;

namespace Shared.Enums
{
    /// <summary>
    /// Especifica a direção da ordem de classificação.
    /// </summary>
    public enum EnumSortOrder
    {
        /// <summary>
        /// Ordem crescente.
        /// </summary>
        [Description("Crescente")]
        Ascending = 0,

        /// <summary>
        /// Ordem decrescente.
        /// </summary>
        [Description("Decrescente")]
        Descending = 1
    }
}
