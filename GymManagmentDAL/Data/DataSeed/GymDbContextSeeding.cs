using GymManagmentDAL.Data.Context;
using GymManagmentDAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GymManagmentDAL.Data.DataSeed
{
    public static class GymDbContextSeeding
    {
        public static bool DataSeed(GymDbContext dbContext)
        {
            try
            {
                var HasPlans = dbContext.Plans.Any();
                var HasCategories = dbContext.Categories.Any();
                if (!HasPlans)
                {
                    var Plans = LoodDataFromJsonFile<Plan>("plans.json");
                    if (Plans.Any())
                    {
                        dbContext.Plans.AddRange(Plans);
                    }
                }
                if (!HasCategories)
                {
                    var Categories = LoodDataFromJsonFile<Category>("categories.json");
                    if (Categories.Any())
                    {
                        dbContext.Categories.AddRange(Categories);
                    }
                }
                return dbContext.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                return false;
            }

        }

        private static List<T> LoodDataFromJsonFile<T>(string FileName)
        {
            var fliePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Files", FileName);
            if(!File.Exists(fliePath))throw new FileNotFoundException("Json File Not Found");
            string jsonData = File.ReadAllText(fliePath);
            var Options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };
            return JsonSerializer.Deserialize<List<T>>(jsonData, Options) ?? new List<T>();
        }
    }
}
