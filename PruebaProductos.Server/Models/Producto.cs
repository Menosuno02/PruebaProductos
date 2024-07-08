using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace PruebaProductos.Server.Models
{
    [Table("Productos")]
    public class Producto
    {
        [Key]
        [Column("codigo")]
        public int CodigoProducto {  get; set; }
        
        [Column("descripcion")]
        public string DescripcionProducto { get; set; }
        [Column("fechacreacion")]
        public DateTime FechaCreacion {  get; set; }
        [Column("categoria")]
        public string Categoria {  get; set; }
        [Column("precio")]
        public decimal Precio { get; set; }
    }
}
