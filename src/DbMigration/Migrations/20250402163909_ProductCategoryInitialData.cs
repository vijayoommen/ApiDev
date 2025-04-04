using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbMigration.Migrations
{
    /// <inheritdoc />
    public partial class ProductCategoryInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            INSERT INTO dbo.ProductCategory (Name, Description) VALUES
                ('Fresh Produce', 'Fresh fruits and vegetables'),
                ('Dairy Products', 'Milk, cheese, yogurt, and other dairy items'),
                ('Meat & Poultry', 'Fresh and processed meat and poultry'),
                ('Packaged Snacks', 'Chips, cookies, and other packaged snacks'),
                ('Bakery Products', 'Breads, pastries, and other baked goods'),
                ('Beverages', 'Juices, coffee, tea, and other drinks'),
                ('Condiments & Sauces', 'Ketchup, mustard, sauces, and seasonings'),
                ('Frozen Foods', 'Frozen meals, vegetables, and desserts'),
                ('Grains & Pasta', 'Rice, pasta, and other grain-based products'),
                ('Canned & Jarred Goods', 'Canned vegetables, fruits, and preserved foods');

            INSERT INTO dbo.Product (Name, Description, Price, ImageUrl, CategoryId) VALUES
                ('Apple', 'Fresh red apples', 1.50, 'images/apple.jpg', (SELECT Id FROM dbo.ProductCategory WHERE Name = 'Fresh Produce')),
                ('Banana', 'Organic ripe bananas', 0.99, 'images/banana.jpg', (SELECT Id FROM dbo.ProductCategory WHERE Name = 'Fresh Produce')),
                ('Carrots', 'Crunchy orange carrots', 1.20, 'images/carrots.jpg', (SELECT Id FROM dbo.ProductCategory WHERE Name = 'Fresh Produce')),
                ('Milk', 'Whole milk 1L', 2.50, 'images/milk.jpg', (SELECT Id FROM dbo.ProductCategory WHERE Name = 'Dairy Products')),
                ('Cheddar Cheese', 'Aged cheddar cheese block', 5.99, 'images/cheddar_cheese.jpg', (SELECT Id FROM dbo.ProductCategory WHERE Name = 'Dairy Products')),
                ('Greek Yogurt', 'Plain Greek yogurt 500g', 3.50, 'images/greek_yogurt.jpg', (SELECT Id FROM dbo.ProductCategory WHERE Name = 'Dairy Products')),
                ('Chicken Breast', 'Boneless skinless chicken breast', 6.99, 'images/chicken_breast.jpg', (SELECT Id FROM dbo.ProductCategory WHERE Name = 'Meat & Poultry')),
                ('Ground Beef', 'Fresh ground beef 1lb', 7.49, 'images/ground_beef.jpg', (SELECT Id FROM dbo.ProductCategory WHERE Name = 'Meat & Poultry')),
                ('Potato Chips', 'Classic salted potato chips', 2.99, 'images/potato_chips.jpg', (SELECT Id FROM dbo.ProductCategory WHERE Name = 'Packaged Snacks')),
                ('Chocolate Cookies', 'Crunchy chocolate chip cookies', 3.99, 'images/chocolate_cookies.jpg', (SELECT Id FROM dbo.ProductCategory WHERE Name = 'Packaged Snacks')),
                ('Whole Wheat Bread', 'Freshly baked whole wheat bread', 2.99, 'images/whole_wheat_bread.jpg', (SELECT Id FROM dbo.ProductCategory WHERE Name = 'Bakery Products')),
                ('Croissant', 'Flaky buttery croissant', 1.75, 'images/croissant.jpg', (SELECT Id FROM dbo.ProductCategory WHERE Name = 'Bakery Products')),
                ('Orange Juice', '100% fresh orange juice 1L', 3.99, 'images/orange_juice.jpg', (SELECT Id FROM dbo.ProductCategory WHERE Name = 'Beverages')),
                ('Coffee Beans', 'Premium roasted coffee beans 250g', 9.99, 'images/coffee_beans.jpg', (SELECT Id FROM dbo.ProductCategory WHERE Name = 'Beverages')),
                ('Ketchup', 'Classic tomato ketchup 500ml', 2.50, 'images/ketchup.jpg', (SELECT Id FROM dbo.ProductCategory WHERE Name = 'Condiments & Sauces')),
                ('Soy Sauce', 'Authentic soy sauce 500ml', 3.49, 'images/soy_sauce.jpg', (SELECT Id FROM dbo.ProductCategory WHERE Name = 'Condiments & Sauces')),
                ('Frozen Pizza', 'Pepperoni frozen pizza', 7.99, 'images/frozen_pizza.jpg', (SELECT Id FROM dbo.ProductCategory WHERE Name = 'Frozen Foods')),
                ('Vanilla Ice Cream', 'Creamy vanilla ice cream 1L', 5.49, 'images/vanilla_ice_cream.jpg', (SELECT Id FROM dbo.ProductCategory WHERE Name = 'Frozen Foods')),
                ('White Rice', 'Long-grain white rice 2kg', 4.99, 'images/white_rice.jpg', (SELECT Id FROM dbo.ProductCategory WHERE Name = 'Grains & Pasta')),
                ('Spaghetti', 'Italian-style spaghetti pasta 500g', 2.49, 'images/spaghetti.jpg', (SELECT Id FROM dbo.ProductCategory WHERE Name = 'Grains & Pasta')),
                ('Canned Tomatoes', 'Diced canned tomatoes 400g', 1.99, 'images/canned_tomatoes.jpg', (SELECT Id FROM dbo.ProductCategory WHERE Name = 'Canned & Jarred Goods')),
                ('Pickles', 'Crunchy dill pickles jar', 3.25, 'images/pickles.jpg', (SELECT Id FROM dbo.ProductCategory WHERE Name = 'Canned & Jarred Goods'));
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // do nothing
        }
    }
}
