[Table("Stats_ratingsGroupings", Schema = "ProductCategory")]
public class RatingGroupings
{
    [Key, ForeignKey("ProductCategoryStats")]
    public string lvl_date_URL { get; set; }
    public int All { get; set; }
    public int OneStarPlus { get; set; }
    public int TwoStarPlus { get; set; }
    public int ThreeStarPlus { get; set; }
    public int FourStarPlus { get; set; }
    public virtual ProductCategoryStats ProductCategoryStats { get; set; }
    public void SetValues(HtmlNode node)
    {
        All = Convert.ToInt32(node.QuerySelectorAll("._3RQES")[0].InnerText.Replace(",", ""));
        OneStarPlus = Convert.ToInt32(node.QuerySelectorAll("._3RQES")[1].InnerText.Replace(",", ""));
        TwoStarPlus = Convert.ToInt32(node.QuerySelectorAll("._3RQES")[2].InnerText.Replace(",", ""));
        ThreeStarPlus = Convert.ToInt32(node.QuerySelectorAll("._3RQES")[3].InnerText.Replace(",", ""));
        FourStarPlus = Convert.ToInt32(node.QuerySelectorAll("._3RQES")[4].InnerText.Replace(",", ""));
    }
}

[Table("Stats_salesGroupings", Schema = "ProductCategory")]
public class SalesGroupings
{
    [Key, ForeignKey("ProductCategoryStats")]
    public string lvl_date_URL { get; set; }
    public int NoSales { get; set; }
    public int LowSales { get; set; }
    public int MediumSales { get; set; }
    public int HighSales { get; set; }
    public int TopSellers { get; set; }
    public virtual ProductCategoryStats ProductCategoryStats { get; set; }
    public void SetValues(HtmlNode node)
    {
        NoSales = Convert.ToInt32(node.QuerySelectorAll("._3Iuwi")[0].QuerySelector("._2kkyX").InnerText.Replace(",", ""));
        LowSales = Convert.ToInt32(node.QuerySelectorAll("._3Iuwi")[1].QuerySelector("._2kkyX").InnerText.Replace(",", ""));
        MediumSales = Convert.ToInt32(node.QuerySelectorAll("._3Iuwi")[2].QuerySelector("._2kkyX").InnerText.Replace(",", ""));
        HighSales = Convert.ToInt32(node.QuerySelectorAll("._3Iuwi")[3].QuerySelector("._2kkyX").InnerText.Replace(",", ""));
        TopSellers = Convert.ToInt32(node.QuerySelectorAll("._3Iuwi")[4].QuerySelector("._2kkyX").InnerText.Replace(",", ""));
    }
}

[Table("Stats_dateAddedGroupings", Schema = "ProductCategory")]
public class DateAddedGroupings
{
    [Key, ForeignKey("ProductCategoryStats")]
    public string lvl_date_URL { get; set; }
    public int All { get; set; }
    public int InLastYear { get; set; }
    public int InLastMonth { get; set; }
    public int InLastWeek { get; set; }
    public int InLastDay { get; set; }
    public virtual ProductCategoryStats ProductCategoryStats { get; set; }
    public void SetValues(HtmlNode node)
    {
        All = Convert.ToInt32(node.QuerySelectorAll("._3RQES")[0].InnerText.Replace(",", ""));
        InLastYear = Convert.ToInt32(node.QuerySelectorAll("._3RQES")[1].InnerText.Replace(",", ""));
        InLastMonth = Convert.ToInt32(node.QuerySelectorAll("._3RQES")[2].InnerText.Replace(",", ""));
        InLastWeek = Convert.ToInt32(node.QuerySelectorAll("._3RQES")[3].InnerText.Replace(",", ""));
        InLastDay = Convert.ToInt32(node.QuerySelectorAll("._3RQES")[4].InnerText.Replace(",", ""));
    }
}

public class StatsSetter {
   // var salesNode = filterspanel.QuerySelector("div[data-test-selector='filter-sales']");
   //this.salesGroupings.SetValues(salesNode);

   // var RatingsNode = filterspanel.QuerySelector("div[data-test-selector='filter-rating']");
   //         this.ratingGroupings.SetValues(RatingsNode);

   //         var DateAddedNode = filterspanel.QuerySelector("div[data-test-selector='filter-date-added']");
   //         this.dateAddedGroupings.SetValues(DateAddedNode);
        }