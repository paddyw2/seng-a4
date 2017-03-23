// Class: ProductFacade
// Provides an interface for
// accessing product information
// and controlling product
// hardware

using System;
using System.Collections.Generic;
using Frontend4;
using Frontend4.Hardware;

public class ProductFacade
{
    private HardwareFacade facade;
    private List<ProductKind> productSpecs;

    public ProductFacade(HardwareFacade facade)
    {
        // save hardware facade reference
        this.facade = facade;
    }

    public void Configure(List<ProductKind> products)
    {
        // updates the product list and
        // configures the hardware
        productSpecs = products;
        facade.Configure(products);
    }

    public int getPrice(int id)
    {
        // returns a product price by
        // button id
        return productSpecs[id].Cost.Value;
    }

    public int getQuantity(int id)
    {
        // returns a product quantity
        // by button id
        ProductRack[] racks = facade.ProductRacks;
        return racks[id].Count;
    }

    public void dispenseProduct(int id)
    {
        // dispenses a product by button id
        ProductRack[] racks = facade.ProductRacks;
        racks[id].DispenseProduct();
    }
}