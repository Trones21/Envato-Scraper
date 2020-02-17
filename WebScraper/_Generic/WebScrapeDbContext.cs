using System.Data.Entity;

namespace MyWebScraper
{
    public class WebScrapeDbContext : DbContext
    {
        //Author Schema (1:1)        
        public DbSet<Author> Author { get; set; } //I definitely want more properties than this.
        
        public DbSet<AuthorEnvatoSites> Author_EnvatoSites { get; set; }
        public DbSet<AuthorSocialSites> Author_SocialSites { get; set;}
        public DbSet<AuthorBadge> Author_Badges { get; set; } 

        //Author (1:M) --- but DateName is distinct
        public DbSet<AuthorStatsViaList> Author_StatsViaList { get; set; }
        public DbSet<AuthorStatsViaPP> Author_StatsViaProfilePage { get; set; }

        //Category Stats Scraper
        public DbSet<ProductCategoryStats> ProdCatStats { get; set; }
        public DbSet<SiteFilter> SiteFilters { get; set; }

        //Product Schema (1:1)
        public DbSet<Product> Product { get; set; }

        //Product Schema (1:M) -- (Only Insert New, not periodic)     
        public DbSet<ProductAttribute> Product_Attribute { get; set; }
        public DbSet<License> Product_License { get; set; }
        public DbSet<Comment> Product_Comment { get; set; }
        public DbSet<Review> Product_Review { get; set; }
        public DbSet<Tag> Product_Tag { get; set; }

        //Product Schema (1:M) -- Stats (Periodic)
        public DbSet<ProductStatsViaPP> Product_StatsViaProfilePage { get; set; }
        public DbSet<ProductDetailsChangeSet> Product_DetailsChangeSets { get; set; }

        //
        //public DbSet<Collection> DbSet_collection { get; set; }


        ///Temp Table DbSets
        public DbSet<Tag> temp_tags;
    }
}
