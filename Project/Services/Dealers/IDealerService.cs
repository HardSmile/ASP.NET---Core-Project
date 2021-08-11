namespace Project.Services.Dealers
{
    public interface IDealerService
    {
        public bool IsDealer(string userId);
        public int IdUser(string userId);
    }
}
