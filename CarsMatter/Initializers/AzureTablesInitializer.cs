using AspNetCore.AsyncInitialization;
using CarsMatter.Infrastructure.Interfaces;
using CarsMatter.Infrastructure.Models.AzureTables;
using System.Threading.Tasks;

namespace CarsMatter.Initializers
{
    public class AzureTablesInitializer : IAsyncInitializer
    {
        private readonly IAzureTable<Brand> brandsTable;
        private readonly IAzureTable<BrandModel> brandModelsTable;
        private readonly IAzureTable<Car> carsTable;
        private readonly IAzureTable<User> usersTable;
        private readonly IAzureTable<FavoriteCar> favoriteCarsTable;
        private readonly IAzureTable<ConsumablesNote> consumablesNotesTable;
        private readonly IAzureTable<RefillNote> refillNotesTable;

        public AzureTablesInitializer(
            IAzureTable<Brand> brandsTable,
            IAzureTable<BrandModel> brandModelsTable, 
            IAzureTable<Car> carsTable, 
            IAzureTable<User> usersTable,
            IAzureTable<FavoriteCar> favoriteCarsTable,
            IAzureTable<ConsumablesNote> consumablesNotesTable,
            IAzureTable<RefillNote> refillNotesTable)
        {
            this.brandsTable = brandsTable;
            this.brandModelsTable = brandModelsTable;
            this.carsTable = carsTable;
            this.usersTable = usersTable;
            this.favoriteCarsTable = favoriteCarsTable;
            this.consumablesNotesTable = consumablesNotesTable;
            this.refillNotesTable = refillNotesTable;
        }

        public async Task InitializeAsync()
        {
            await this.brandsTable.CreateTableIfNotExist();
            await this.brandModelsTable.CreateTableIfNotExist();
            await this.carsTable.CreateTableIfNotExist();
            await this.usersTable.CreateTableIfNotExist();
            await this.favoriteCarsTable.CreateTableIfNotExist();
            await this.consumablesNotesTable.CreateTableIfNotExist();
            await this.refillNotesTable.CreateTableIfNotExist();
        }
    }
}
