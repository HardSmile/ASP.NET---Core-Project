namespace Project.Services.Cars
{
    public class CarDetailsServiceModel : CarServiceModel
    {
        public string Description { get; set; }

        public int DeakerId { get; init; }
        public string DealerName { get; init; }
        public int CategoryId { get; init; }
        public string UserId { get; init; }
    }
}
