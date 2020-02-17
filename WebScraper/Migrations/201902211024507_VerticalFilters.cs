namespace MyWebScraper.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class VerticalFilters : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "Author.Authors",
                c => new
                    {
                        AuthorName = c.String(nullable: false, maxLength: 128),
                        MemberSince = c.String(),
                        Country = c.String(),
                        EmailAddress = c.String(),
                        dateAdded = c.DateTime(nullable: false, storeType: "date"),
                        datetimeLinkClicked = c.DateTime(),
                        SourceofLinkClicked = c.String(),
                        password = c.String(),
                        logoURL = c.String(),
                    })
                .PrimaryKey(t => t.AuthorName);
            
            CreateTable(
                "Author.Badges",
                c => new
                    {
                        AuthorName_BadgeName = c.String(nullable: false, maxLength: 128),
                        DateAquired = c.DateTime(nullable: false),
                        BadgeName = c.String(),
                    })
                .PrimaryKey(t => t.AuthorName_BadgeName);
            
            CreateTable(
                "Author.EnvatoSites",
                c => new
                    {
                        AuthorName = c.String(nullable: false, maxLength: 128),
                        IsOnAudioJungle = c.Boolean(nullable: false),
                        DateJoinedAudioJungle = c.DateTime(),
                        IsOnCodeCanyon = c.Boolean(nullable: false),
                        DateJoinedCodeCanyon = c.DateTime(),
                        IsOnGraphicRiver = c.Boolean(nullable: false),
                        DateJoinedGraphicRiver = c.DateTime(),
                        IsOnPhotoDune = c.Boolean(nullable: false),
                        DateJoinedPhotoDune = c.DateTime(),
                        IsOnThemeForest = c.Boolean(nullable: false),
                        DateJoinedThemeForest = c.DateTime(),
                        IsOnVideoHive = c.Boolean(nullable: false),
                        DateJoinedVideoHive = c.DateTime(),
                        IsOn3DOcean = c.Boolean(nullable: false),
                        DateJoined3DOcean = c.DateTime(),
                    })
                .PrimaryKey(t => t.AuthorName);
            
            CreateTable(
                "Author.SocialSites",
                c => new
                    {
                        AuthorName = c.String(nullable: false, maxLength: 128),
                        Behance = c.String(),
                        DeviantArt = c.String(),
                        Digg = c.String(),
                        Dribbble = c.String(),
                        Facebook = c.String(),
                        Flickr = c.String(),
                        Github = c.String(),
                        GooglePlus = c.String(),
                        LastFM = c.String(),
                        LinkedIn = c.String(),
                        MySpace = c.String(),
                        Reddit = c.String(),
                        SoundCloud = c.String(),
                        Tumblr = c.String(),
                        Twitter = c.String(),
                        Vimeo = c.String(),
                        Youtube = c.String(),
                    })
                .PrimaryKey(t => t.AuthorName);
            
            CreateTable(
                "Author.StatsViaList",
                c => new
                    {
                        Date = c.DateTime(nullable: false, storeType: "date"),
                        AuthorName = c.String(nullable: false, maxLength: 128),
                        OverallRank = c.Int(nullable: false),
                        Rating = c.Double(),
                        RatingsCount = c.Int(nullable: false),
                        SalesCount = c.Int(nullable: false),
                        followersCount = c.Int(nullable: false),
                        ItemsCount = c.Int(nullable: false),
                        IsAvailableforFreelance = c.Boolean(nullable: false),
                        BadgesCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Date, t.AuthorName });
            
            CreateTable(
                "Author.StatsViaProfilePage",
                c => new
                    {
                        Date = c.DateTime(nullable: false, storeType: "date"),
                        AuthorName = c.String(nullable: false, maxLength: 128),
                        RatingsCount = c.Int(nullable: false),
                        RatingPrecise = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SalesCount = c.Int(nullable: false),
                        followersCount = c.Int(nullable: false),
                        followingCount = c.Int(nullable: false),
                        ItemsCount = c.Int(nullable: false),
                        IsAvailableforFreelance = c.Boolean(nullable: false),
                        BadgesCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Date, t.AuthorName });
            
            CreateTable(
                "ProductCategory.Stats",
                c => new
                    {
                        date_URL = c.String(nullable: false, maxLength: 128),
                        Date = c.DateTime(nullable: false),
                        Level = c.Int(nullable: false),
                        URL = c.String(),
                        Category = c.String(),
                        ItemsCount = c.Int(nullable: false),
                        MinPrice = c.Int(nullable: false),
                        MaxPrice = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.date_URL);
            
            CreateTable(
                "ProductCategory.SiteFilters",
                c => new
                    {
                        date = c.DateTime(nullable: false, storeType: "date"),
                        URL = c.String(nullable: false, maxLength: 128),
                        FilterName = c.String(nullable: false, maxLength: 128),
                        FilterMember = c.String(nullable: false, maxLength: 128),
                        Value = c.String(),
                        ProductCategoryStats_date_URL = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => new { t.date, t.URL, t.FilterName, t.FilterMember })
                .ForeignKey("ProductCategory.Stats", t => t.ProductCategoryStats_date_URL)
                .Index(t => t.ProductCategoryStats_date_URL);
            
            CreateTable(
                "Product.Products",
                c => new
                    {
                        ProductID = c.String(nullable: false, maxLength: 128),
                        Title = c.String(),
                        Author = c.String(),
                        Date_CreatedbyAuthor = c.String(),
                        Date_LastUpdatedbyAuthor = c.DateTime(nullable: false, storeType: "date"),
                        Hierarchy = c.String(),
                        URL = c.String(),
                        logoURL = c.String(),
                        ProductDetailsMaxChangeSetID = c.Int(nullable: false),
                        ProductAttributesMaxChangeSetID = c.Int(nullable: false),
                        GETreq_fullcost = c.Int(nullable: false),
                        Date_ofInitialFullScrape = c.DateTime(storeType: "date"),
                        Date_LastSupplementalScrape = c.DateTime(storeType: "date"),
                        hasComments = c.Boolean(),
                        hasReviews = c.Boolean(),
                    })
                .PrimaryKey(t => t.ProductID);
            
            CreateTable(
                "Product.Attributes",
                c => new
                    {
                        ProductID = c.String(nullable: false, maxLength: 128),
                        AttributeName = c.String(nullable: false, maxLength: 128),
                        AttributeValue = c.String(nullable: false, maxLength: 128),
                        ChangeSetID = c.String(),
                    })
                .PrimaryKey(t => new { t.ProductID, t.AttributeName, t.AttributeValue });
            
            CreateTable(
                "Product.Comments",
                c => new
                    {
                        CommentID = c.String(nullable: false, maxLength: 128),
                        ProductID = c.String(),
                        Date_ofScrape = c.DateTime(nullable: false, storeType: "date"),
                        Commenter = c.String(),
                        TimeSinceCreation = c.String(),
                        Text = c.String(),
                        hasCommenterPurchased = c.Boolean(nullable: false),
                        hasAuthorResponded = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CommentID);
            
            CreateTable(
                "Product.DetailsChangesets",
                c => new
                    {
                        ProductID = c.String(nullable: false, maxLength: 128),
                        ChangeSetID = c.Int(nullable: false),
                        date_ofScrape = c.DateTime(nullable: false, storeType: "date"),
                        ProductName = c.String(),
                        ProductDescription = c.String(),
                        ProductDescriptionLength = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.ProductID, t.ChangeSetID });
            
            CreateTable(
                "Product.Licenses",
                c => new
                    {
                        ProductID = c.String(nullable: false, maxLength: 128),
                        LicenseName = c.String(nullable: false, maxLength: 128),
                        Price = c.String(nullable: false, maxLength: 128),
                        Date_ofScrape = c.DateTime(nullable: false, storeType: "date"),
                        ChangesetID = c.String(),
                    })
                .PrimaryKey(t => new { t.ProductID, t.LicenseName, t.Price });
            
            CreateTable(
                "Product.Reviews",
                c => new
                    {
                        ReviewID = c.String(nullable: false, maxLength: 128),
                        ProductID = c.String(),
                        Date_ofScrape = c.DateTime(nullable: false, storeType: "date"),
                        TimeSinceCreation = c.String(),
                        Reviewer = c.String(),
                        Reason = c.String(),
                        Text = c.String(),
                        Stars = c.Int(nullable: false),
                        hasAuthorResponded = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ReviewID);
            
            CreateTable(
                "Product.StatsViaPP",
                c => new
                    {
                        Date_ofScrape = c.DateTime(nullable: false, storeType: "date"),
                        ProductID = c.String(nullable: false, maxLength: 128),
                        Date_LastUpdatedbyAuthor = c.DateTime(nullable: false, storeType: "date"),
                        Hierarchy = c.String(),
                        SalesCount = c.Int(nullable: false),
                        RatingsCount = c.Int(nullable: false),
                        CommentsCount = c.Int(nullable: false),
                        AvgRating = c.Decimal(nullable: false, precision: 18, scale: 2),
                        percent5star = c.Decimal(nullable: false, precision: 18, scale: 2),
                        percent4star = c.Decimal(nullable: false, precision: 18, scale: 2),
                        percent3star = c.Decimal(nullable: false, precision: 18, scale: 2),
                        percent2star = c.Decimal(nullable: false, precision: 18, scale: 2),
                        percent1star = c.Decimal(nullable: false, precision: 18, scale: 2),
                        TagsCount = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Date_ofScrape, t.ProductID });
            
            CreateTable(
                "Product.Tags",
                c => new
                    {
                        ProductID = c.String(nullable: false, maxLength: 128),
                        TagName = c.String(nullable: false, maxLength: 128),
                        date_ofScrape = c.DateTime(nullable: false, storeType: "date"),
                    })
                .PrimaryKey(t => new { t.ProductID, t.TagName });
            
        }
        
        public override void Down()
        {
            DropForeignKey("ProductCategory.SiteFilters", "ProductCategoryStats_date_URL", "ProductCategory.Stats");
            DropIndex("ProductCategory.SiteFilters", new[] { "ProductCategoryStats_date_URL" });
            DropTable("Product.Tags");
            DropTable("Product.StatsViaPP");
            DropTable("Product.Reviews");
            DropTable("Product.Licenses");
            DropTable("Product.DetailsChangesets");
            DropTable("Product.Comments");
            DropTable("Product.Attributes");
            DropTable("Product.Products");
            DropTable("ProductCategory.SiteFilters");
            DropTable("ProductCategory.Stats");
            DropTable("Author.StatsViaProfilePage");
            DropTable("Author.StatsViaList");
            DropTable("Author.SocialSites");
            DropTable("Author.EnvatoSites");
            DropTable("Author.Badges");
            DropTable("Author.Authors");
        }
    }
}
