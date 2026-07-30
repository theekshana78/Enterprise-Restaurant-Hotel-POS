using System.Collections.Generic;

namespace EnterprisePOS.Core.Entities
{
    public class Recipe
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public List<RecipeIngredient> Ingredients { get; set; } = new List<RecipeIngredient>();
    }

    public class RecipeIngredient
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public int IngredientProductId { get; set; }
        public string IngredientName { get; set; } = string.Empty;
        public double QuantityRequired { get; set; }
        public string Unit { get; set; } = "g";
    }
}
