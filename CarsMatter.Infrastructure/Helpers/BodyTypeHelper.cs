using CarsMatter.Infrastructure.Models.Enums;

namespace CarsMatter.Infrastructure.Helpers
{
    public class BodyTypeHelper
    {
        public static BodyType MapBodyType(string bodyTypeString)
        {
            switch (bodyTypeString)
            {
                case string bodyType when bodyTypeString.Contains("хэтчбек"):
                    return BodyType.Hatchback;
                case string bodyType when bodyTypeString.Contains("седан"):
                    return BodyType.Sedan;
                case string bodyType when bodyTypeString.Contains("универсал"):
                    return BodyType.Wagon;
                case string bodyType when bodyTypeString.Contains("люкс"):
                    return BodyType.Luxury;
                case string bodyType when bodyTypeString.Contains("кабриолет"):
                    return BodyType.Convertible;
                case string bodyType when bodyTypeString.Contains("купе"):
                    return BodyType.Coupe;
                case string bodyType when bodyTypeString.Contains("минивэн"):
                    return BodyType.Minivan;
                case string bodyType when bodyTypeString.Contains("внедорожник"):
                    return BodyType.SUV;
                default:
                    return BodyType.Sedan;
            }
        }
    }
}
