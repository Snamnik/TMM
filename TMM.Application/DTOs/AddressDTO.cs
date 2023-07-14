namespace TMM.Application.DTOs
{
    public record AddAddressDTO(string AddressLine1, string AddressLine2, string Town, string County, string Postcode, string Country);
    public record AddressDTO(int AddressId, string AddressLine1, string AddressLine2, string Town, string County, string Postcode, string Country, bool IsMain);

    public static class AddressMapper
    {
        public static AddressDTO Map(this Address address) =>
            new AddressDTO(address.Id, address.AddressLine1, address.AddressLine2, address.Town, address.County, address.Postcode, address.Country, address.IsMain);

        public static Address Map(this AddAddressDTO address) =>
        new Address(address.AddressLine1, address.AddressLine2, address.Town, address.County, address.Postcode, address.Country);
    }

}
