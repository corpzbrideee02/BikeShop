
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Casestudy.DAL.DomainClasses
{
    public class Product
    {
        [Key]
        [StringLength(15)]
        public string Id { get; set; }

        [ForeignKey("BrandId")]
        public Brand Brand { get; set; }
        public int BrandId { get; set; }

        [Column(TypeName = "timestamp")]
        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        [MaxLength(8)]
        public byte[] Timer { get; set; }

        public string ProductName { get; set; }
        public string GraphicName { get; set; }
        [Column(TypeName = "money")]
        public decimal CostPrice { get; set; }

        [Column(TypeName = "money")]
        public decimal MSRP { get; set; }

        public int QtyOnHand { get; set; }
        public int QtyOnBackOrder { get; set; }


        [StringLength(2000)]
        public string Description { get; set; }
    }
}
