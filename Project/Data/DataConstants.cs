namespace Project.Data
{
    public class DataConstants
    {

        public class Category 
        {
        
        }
        public class Car 
        {
            public const int CarBrandMaxLength = 20;
            public const int CarBrandMinLength = 2;
            public const int CarModelMaxLength = 20;
            public const int CarModelMinLength = 2;
            public const int CarYearMinLength = 1900;
            public const int CarYearMaxLength = 2050;
            public const int CarDescriptionMinLength = 2;
        }
  public class Dealer 
        {
            public const int DealerNameMaxLength = 25;
            public const int DealerNameMinLength = 2;
            public const int PhoneNumberMinLength = 6;
            public const int PhoneNumberMaxLength = 30;

        }

    }
}
