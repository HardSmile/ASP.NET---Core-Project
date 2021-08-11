namespace Project.Services.Dealers
{
    using Project.Data;
    using System.Linq;

    public class DealerService : IDealerService
    {
        private readonly CarRentingDbContext data;

        public DealerService(CarRentingDbContext data)
            => this.data = data;


        public bool IsDealer(string userId) =>
            this.data
            .Dealers
            .Any(d => d.UserId == userId);
        public int IdUser(string userId)
       => this.data
                .Dealers
                .Where(d => d.UserId == userId)
                .Select(d => d.Id)
                .FirstOrDefault();
    }
}
