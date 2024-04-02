﻿namespace ECommerce.Business.Models.Dtos.ShippingPlaces
{
    public class ShippingPlaceCheckoutDto
    {
        public Guid DistrictId { get; set; }
        public string Neighborhood { get; set; }
        public string Street { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
    }
}
