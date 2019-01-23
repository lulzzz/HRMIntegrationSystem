namespace Shared.Interfaces
{
    public interface ICustomerIdService
    {
        /// <summary>
        /// Gets the customerId from the route.
        /// Throws ArgumentException if not found.
        /// </summary>
        /// <returns></returns>
        int GetCustomerIdNotNull();

        /// <summary>
        /// Gets the customerId from the route.
        /// Returns null if customerId isn't specified
        /// </summary>
        /// <returns></returns>
        int? GetCustomerId();
    }

    public interface IStaticCustomerId
    {
        int? CustomerId { get; set; }
    }
}
