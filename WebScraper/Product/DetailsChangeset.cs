using IronWebScraper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebScraper
{
    /// <summary>
    /// There will only be one changeset per scrape... how to update object
    /// </summary>
    [Table("DetailsChangesets", Schema = "Product")]
    public class ProductDetailsChangeSet
    {
        [Key, Column(Order = 0)]
        public string ProductID { get; set; }
        [Key, Column(Order = 1)]
        public int ChangeSetID { get; set; }
        [Column(TypeName = "Date")]
        public DateTime date_ofScrape { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int ProductDescriptionLength { get; set; }

        public void SetValues(Response response, string ProductID)
        {
            this.ProductID = ProductID;
            date_ofScrape = DateTime.Now;
            ProductName = response.QuerySelector(".item-header__title > h1").InnerText.Trim();
            ProductDescription = response.QuerySelector(".js-item-description").InnerText;
            ProductDescriptionLength = response.QuerySelector(".js-item-description").InnerText.Length;

        }

        public void writetoDBifNew(string ProductID)
        {
            ///Could be a SP - select the most recent changeset and compare to what was scraped,
            ///if there are changes, then add the new one with changeset += 1

            using (var db = new WebScrapeDbContext())
            {
                var MaxChangesetObject = (from item in db.Product_DetailsChangeSets
                                          where item.ProductID == ProductID
                                          orderby item.ChangeSetID
                                          select item).FirstOrDefault();

                if (MaxChangesetObject is null)
                {
                    this.ChangeSetID = 1;
                }
                else
                {
                    this.ChangeSetID = MaxChangesetObject.ChangeSetID + 1;
                }
                db.Product_DetailsChangeSets.Add(this);
            }

        }
    }
}
